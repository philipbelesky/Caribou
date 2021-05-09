namespace Caribou.Processing
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Caribou.Data;
    using Grasshopper;
    using Grasshopper.Kernel;
    using Grasshopper.Kernel.Data;
    using Grasshopper.Kernel.Types;
    using Rhino.Geometry;

    /// <summary>Asynchronous task to identify and output all OSM nodes as Points for a given request.</summary>
    public class LoadAndParseNodesWorker : BaseLoadAndParseWorker
    {
        private GH_Structure<GH_Point> foundNodes;

        public LoadAndParseNodesWorker(GH_Component parent)
            : base(parent) // Pass parent component back to base class so state (e.g. remarks) can bubble up
        {
        }

        public override WorkerInstance Duplicate() => new LoadAndParseNodesWorker(this.Parent);

        public override void MakeGeometryForComponentType()
        {
            //// Translate OSM nodes to Rhino points
            //this.foundNodes = TranslateToXYManually.NodePointsFromCoords(this.foundItems);
        }

        public override void MakeTreeForComponentType()
        {
            this.foundNodes = new GH_Structure<GH_Point>();
        }

        public override void OutputTreeForComponentType(IGH_DataAccess da)
        {
            da.SetDataTree(0, this.foundNodes);
        }
    }
}
