namespace Caribou
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Grasshopper.Kernel;
    using Rhino.Geometry;

    public class CaribouWorker : WorkerInstance
    {
        // This demo/example was developed by Dimitrie Stefanescu for the [Speckle Systems project](https://speckle.systems)
        // This implementation is a near-direct copy of that published in [this repository](https://github.com/specklesystems/GrasshopperAsyncComponent/)

        private Plane test = Plane.WorldXY;

        public CaribouWorker() : base(null) { }

        public override void DoWork(Action<string, double> reportProgress, Action done)
        {
            // Checking for cancellation
            if (CancellationToken.IsCancellationRequested) { return; }

            // Do the work
            done();
        }

        public override WorkerInstance Duplicate() => new CaribouWorker();

        public override void GetData(IGH_DataAccess da, GH_ComponentParamServer ghParams)
        {
            if (CancellationToken.IsCancellationRequested) return;
            // da.GetData(0, ref test);
            // TODO: validation cases
            // If the ReadFile output is set to per-line or total file
            // If a filepath is provided by mistake 
        }

        public override void SetData(IGH_DataAccess da)
        {
            if (CancellationToken.IsCancellationRequested) return;
            // da.SetData(0, null);
        }
    }
}
