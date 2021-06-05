namespace Caribou
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;
    using Caribou.Components;
    using Grasshopper.Kernel;

    /// <summary>
    /// A class that holds the actual compute logic and encapsulates the state it needs. Every <see cref="CaribouAsyncComponent"/> needs to have one.
    /// </summary>
    public abstract class WorkerInstance
    {
        // This is a class that enables a particular computation to be preformed asynchronously by components using CaribouAsyncComponent
        // This approach was developed by Dimitrie Stefanescu for the [Speckle Systems project](https://speckle.systems)
        // This implementation is a near-direct copy of that published in [this repository](https://github.com/specklesystems/GrasshopperAsyncComponent/)

        /// <summary>
        /// The parent component. Useful for passing state back to the host component.
        /// </summary>
        public GH_Component Parent { get; set; }

        /// <summary>
        /// This token is set by the parent <see cref="CaribouAsyncComponent"/>.
        /// </summary>
        public CancellationToken CancellationToken { get; set; }

        /// <summary>
        /// This is set by the parent <see cref="CaribouAsyncComponent"/>. You can set it yourself, but it's not really worth it.
        /// </summary>
        public string Id { get; set; }

        // Note that inheritors should provide parents using this constructor so state can be passed back up
        protected WorkerInstance(GH_Component parent)
        {
            Parent = parent;
        }

        /// <summary>
        /// This is a "factory" method. It should return a fresh instance of this class, but with all the necessary state that you might have passed on directly from your component.
        /// </summary>
        /// <returns></returns>
        public abstract WorkerInstance Duplicate();

        /// <summary>
        /// This method is where the actual calculation/computation/heavy lifting should be done.
        /// <b>Make sure you always check as frequently as you can if <see cref="WorkerInstance.CancellationToken"/> is cancelled. For an example, see the <see cref="PrimeCalculatorWorker"/>.</b>
        /// </summary>
        /// <param name="reportProgress">Call this to report progress up to the parent component.</param>
        /// <param name="done">Call this when everything is <b>done</b>. It will tell the parent component that you're ready to <see cref="SetData(IGH_DataAccess)"/>.</param>
        public abstract void DoWork(Action<string, double> reportProgress, Action done);

        // Per issues #11 and #14 in https://github.com/specklesystems/GrasshopperAsyncComponent/ this helps prevent messages vanishing
        protected MessagesWrapper RuntimeMessages { get; set; } = new MessagesWrapper();

        protected LoggingHandler logger = new LoggingHandler();

        // As per RuntimeMessages, we need to write out any messages passed up
        /// <summary>
        /// Write your data setting logic here. <b>Do not call this function directly from this class. It will be invoked by the parent <see cref="CaribouAsyncComponent"/> after you've called `Done` in the <see cref="DoWork(Action{string}, Action{string, GH_RuntimeMessageLevel}, Action)"/> function.</b>
        /// </summary>
        /// <param name="da"></param>
        public void SetData(IGH_DataAccess da)
        {
            WorkerSetData(da); // Worker must implement a custom SetData as per below

            // Report any messages done by the worker instance
            // We must manually translate here from the mock warning types back to GH types due to unit testing requirements
            foreach (var msg in RuntimeMessages.Messages)
            {
                foreach (var level in msg.Keys)
                {
                    switch (level)
                    {
                        case MessagesWrapper.Level.Error:
                            Parent.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, msg[level]);
                            break;
                        case MessagesWrapper.Level.Warning:
                            Parent.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, msg[level]);
                            break;
                        case MessagesWrapper.Level.Remark:
                            Parent.AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, msg[level]);
                            break;
                    }
                }
            }

#if DEBUG
            da.SetDataList(logger.indexOfDebugOutput, logger.debugLogs);
#endif
        }

        protected abstract void WorkerSetData(IGH_DataAccess da);

        /// <summary>
        /// Write your data collection logic here. <b>Do not call this method directly. It will be invoked by the parent <see cref="CaribouAsyncComponent"/>.</b>
        /// </summary>
        /// <param name="da"></param>
        /// <param name="params"></param>
        public abstract void GetData(IGH_DataAccess da, GH_ComponentParamServer ghParams);
    }
}
