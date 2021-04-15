namespace Caribou.Processing
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Grasshopper.Kernel;
    using Rhino.Geometry;

    public class LoadAndParseWorker : WorkerInstance
    {
        public LoadAndParseWorker() : base(null) { }

        public override void DoWork(Action<string, double> reportProgress, Action done)
        {
            // Checking for cancellation
            if (CancellationToken.IsCancellationRequested) { return; }

            // Do the work 

            done();
        }

        public override WorkerInstance Duplicate() => new LoadAndParseWorker();

        public override void GetData(IGH_DataAccess da, GH_ComponentParamServer ghParams)
        {
            if (CancellationToken.IsCancellationRequested) return;

            // da.GetData(0, ref file);
        }

        public override void SetData(IGH_DataAccess da)
        {
            if (CancellationToken.IsCancellationRequested) return;

            // da.SetData(0, spiral); 

            // Can't use the GHBComponent approach to logging; so construct output for Debug param manually
            var debugOutput = new List<string> { $"Test." };
            da.SetDataList(1, debugOutput);
        }
    }
}
