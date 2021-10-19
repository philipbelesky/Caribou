namespace Caribou.Workers
{
    using System;
    using System.Collections.Generic;
    using Caribou.Components;
    using Caribou.Models;
    using Caribou.Processing;
    using Grasshopper.Kernel;
    using Grasshopper.Kernel.Data;
    using Grasshopper.Kernel.Types;
    using Rhino.Geometry;

    /// <summary>Asynchronous task to identify and output all OSM ways as Polylines for a given request.</summary>
    public class ParseWaysWorker : BaseLoadAndParseWorker
    {
        private GH_Structure<GH_Curve> wayOutputs = new GH_Structure<GH_Curve>();
        private Dictionary<OSMTag, List<PolylineCurve>> foundWays;
        protected override OSMGeometryType WorkerType()
        {
            return OSMGeometryType.Way;
        }

        public ParseWaysWorker(GH_Component parent)
            : base(parent) // Pass parent component back to base class so state (e.g. remarks) can bubble up
        {
        }

        public override WorkerInstance Duplicate() => new ParseWaysWorker(this.Parent);

        public override void MakeGeometryForComponentType()
        {
            // Translate OSM ways to Rhino polylines
            this.foundWays = TranslateToXYManually.WayPolylinesFromCoords(this.result);
        }

        public override void GetTreeForComponentType()
        {
            this.wayOutputs = TreeFormatters.MakeTreeForWays(this.foundWays);
        }

        public override void OutputTreeForComponentType(IGH_DataAccess da)
        {
            if (this.wayOutputs != null)
                da.SetDataTree(0, this.wayOutputs);
                if (this.wayOutputs.DataCount == 0)
                    this.RuntimeMessages.Add(new Message(
                        "No ways were found with the specified features or tags.",
                        Message.Level.Warning));
        }
    }
}
