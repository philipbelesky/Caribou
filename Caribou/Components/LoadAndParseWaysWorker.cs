namespace Caribou.Processing
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Caribou.Data;
    using Grasshopper;
    using Grasshopper.Kernel;
    using Grasshopper.Kernel.Data;
    using Rhino.Geometry;

    /// <summary>Asynchronous task to identify and output all OSM ways as Polylines for a given request.</summary>
    public class LoadAndParseWaysWorker : BaseLoadAndParseWorker
    {
        private DataTree<Polyline> foundWays;

        public LoadAndParseWaysWorker(GH_Component parent)
            : base(parent) // Pass parent component back to base class so state (e.g. remarks) can bubble up
        {
        }

        public override WorkerInstance Duplicate() => new LoadAndParseWaysWorker(this.Parent);

        protected override void WorkerSetData(IGH_DataAccess da)
        {
            if (this.CancellationToken.IsCancellationRequested)
            {
                return;
            }

            da.SetDataTree(0, this.foundWays);
            da.SetDataTree(1, this.foundOSMItems);
            da.SetDataTree(2, this.foundElementsReport);
            // Can't use the GHBComponent approach to logging; so construct output for Debug param manually
            da.SetDataList(3, this.debugOutput);
        }
    }
}
