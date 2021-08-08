namespace Caribou.Processing
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Caribou.Data;
    using Caribou.Models;
    using Grasshopper;
    using Grasshopper.Kernel;
    using Grasshopper.Kernel.Data;
    using Grasshopper.Kernel.Types;
    using Rhino.Geometry;

    /// <summary>Asynchronous task to identify and output all OSM nodes as Points for a given request.</summary>
    public class ParseNodesWorker : BaseLoadAndParseWorker
    {
        private GH_Structure<GH_Point> nodeOutputs;
        private Dictionary<OSMMetaData, List<Point3d>> foundNodes;
        protected override OSMGeometryType WorkerType()
        {
            return OSMGeometryType.Node;
        }

        public ParseNodesWorker(GH_Component parent)
            : base(parent) // Pass parent component back to base class so state (e.g. remarks) can bubble up
        {
        }

        public override WorkerInstance Duplicate() => new ParseNodesWorker(this.Parent);

        public override void MakeGeometryForComponentType()
        {
            // Translate OSM nodes to Rhino points
            this.foundNodes = TranslateToXYManually.NodePointsFromCoords(this.result);
        }

        public override void GetTreeForComponentType()
        {
            this.nodeOutputs = TreeFormatters.MakeTreeForNodes(this.foundNodes);
        }

        public override void OutputTreeForComponentType(IGH_DataAccess da)
        {
            da.SetDataTree(0, this.nodeOutputs);
        }
    }
}
