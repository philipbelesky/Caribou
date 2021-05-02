namespace Caribou
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Grasshopper.Kernel;
    using Timer = System.Timers.Timer;

    public abstract class CaribouAsyncComponent : CaribouComponent
    {
        // This is a base class that can be used by all of a plugin's components to do calculations asynchronously
        // This approach was developed by Dimitrie Stefanescu for the [Speckle Systems project](https://speckle.systems)
        // This implementation is a near-direct copy of that published in [this repository](https://github.com/specklesystems/GrasshopperAsyncComponent/)
        public ConcurrentDictionary<string, double> ProgressReports;
        public List<WorkerInstance> Workers;
        public readonly List<CancellationTokenSource> CancellationSources;
        Action<string, double> reportProgress;
        Action done;
        Timer displayProgressTimer;
        int state;
        int setData;
        List<Task> tasks;

        // Pass the constructor parameters up to the main GHBComponent abstract class
        protected CaribouAsyncComponent(string name, string nickname, string description, string subCategory)
            : base(name, nickname, description, subCategory)
        {
            this.displayProgressTimer = new Timer(333) { AutoReset = false };
            this.displayProgressTimer.Elapsed += this.DisplayProgress;

            this.reportProgress = (id, value) =>
            {
                this.ProgressReports[id] = value;
                if (!this.displayProgressTimer.Enabled)
                {
                    this.displayProgressTimer.Start();
                }
            };

            this.done = () =>
            {
                Interlocked.Increment(ref this.state);
                if (this.state == this.Workers.Count && this.setData == 0)
                {
                    Interlocked.Exchange(ref this.setData, 1);

                    // We need to reverse the workers list to set the outputs in the same order as the inputs.
                    this.Workers.Reverse();

                    Rhino.RhinoApp.InvokeOnUiThread((Action)delegate
                    {
                        this.ExpireSolution(true);
                    });
                }
            };

            this.ProgressReports = new ConcurrentDictionary<string, double>();

            this.Workers = new List<WorkerInstance>();
            this.CancellationSources = new List<CancellationTokenSource>();
            this.tasks = new List<Task>();
        }

        /// <summary>
        /// Sets this property inside the constructor of your derived component.
        /// </summary>
        public WorkerInstance BaseWorker { get; set; }

        /// <summary>
        /// Optional: if you have opinions on how the default system task scheduler should treat your workers, set it here.
        /// </summary>
        public TaskCreationOptions? TaskCreationOptions { get; set; }

        public virtual void DisplayProgress(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (this.Workers.Count == 0 || this.ProgressReports.Values.Count == 0)
            {
                return;
            }

            if (this.Workers.Count == 1)
            {
                this.Message = this.ProgressReports.Values.Last().ToString("0.00%");
            }
            else
            {
                double total = 0;
                foreach (var kvp in this.ProgressReports)
                {
                    total += kvp.Value;
                }

                this.Message = (total / this.Workers.Count).ToString("0.00%");
            }

            Rhino.RhinoApp.InvokeOnUiThread((Action)delegate
            {
                this.OnDisplayExpired(true);
            });
        }

        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalMenuItems(menu);
            Menu_AppendItem(menu, "Cancel", (s, e) =>
            {
                this.RequestCancellation();
            });
        }

        public void RequestCancellation()
        {
            foreach (var source in this.CancellationSources)
            {
                source.Cancel();
            }

            this.CancellationSources.Clear();
            this.Workers.Clear();
            this.ProgressReports.Clear();
            this.tasks.Clear();

            Interlocked.Exchange(ref this.state, 0);
            Interlocked.Exchange(ref this.setData, 0);
            this.Message = "Cancelled";
            this.OnDisplayExpired(true);
        }

        protected override void BeforeSolveInstance()
        {
            if (this.state != 0 && this.setData == 1)
            {
                return;
            }

            Debug.WriteLine("Killing");

            foreach (var source in this.CancellationSources)
            {
                source.Cancel();
            }

            this.CancellationSources.Clear();
            this.Workers.Clear();
            this.ProgressReports.Clear();
            this.tasks.Clear();

            Interlocked.Exchange(ref this.state, 0);
        }

        protected override void AfterSolveInstance()
        {
            System.Diagnostics.Debug.WriteLine("After solve instance was called " + this.state + " ? " + this.Workers.Count);

            // We need to start all the tasks as close as possible to each other.
            if (this.state == 0 && this.tasks.Count > 0 && this.setData == 0)
            {
                System.Diagnostics.Debug.WriteLine("After solve INVOKATIONM");
                foreach (var task in this.tasks)
                {
                    task.Start();
                }
            }
        }

        protected override void ExpireDownStreamObjects()
        {
            // Prevents the flash of null data until the new solution is ready
            if (this.setData == 1)
            {
                base.ExpireDownStreamObjects();
            }
        }

        protected override void CaribouSolveInstance(IGH_DataAccess da)
        {
            if (this.state == 0)
            {
                if (this.BaseWorker == null)
                {
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Worker class not provided.");
                    return;
                }

                var currentWorker = this.BaseWorker.Duplicate();
                if (currentWorker == null)
                {
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Could not get a worker instance.");
                    return;
                }

                // Let the worker collect data.
                currentWorker.GetData(da, this.Params);

                // Create the task
                var tokenSource = new CancellationTokenSource();
                currentWorker.CancellationToken = tokenSource.Token;
                currentWorker.Id = $"Worker-{da.Iteration}";

                var currentRun = this.TaskCreationOptions != null
                  ? new Task(() => currentWorker.DoWork(this.reportProgress, this.done), tokenSource.Token, (TaskCreationOptions)this.TaskCreationOptions)
                  : new Task(() => currentWorker.DoWork(this.reportProgress, this.done), tokenSource.Token);

                // Add cancellation source to our bag
                this.CancellationSources.Add(tokenSource);

                // Add the worker to our list
                this.Workers.Add(currentWorker);

                this.tasks.Add(currentRun);

                return;
            }

            if (this.setData == 0)
            {
                return;
            }

            if (this.Workers.Count > 0)
            {
                Interlocked.Decrement(ref this.state);
                this.Workers[this.state].SetData(da);
            }

            if (this.state != 0)
            {
                return;
            }

            this.CancellationSources.Clear();
            this.Workers.Clear();
            this.ProgressReports.Clear();
            this.tasks.Clear();

            Interlocked.Exchange(ref this.setData, 0);

            this.Message = "Done";
            this.OnDisplayExpired(true);
        }
    }
}
