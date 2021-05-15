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

    /// <summary>Asynchronous task to identify and output all OSM ways as Polylines for a given request.</summary>
    public class ParseWaysWorker : BaseLoadAndParseWorker
    {
        private GH_Structure<GH_Curve> wayOutputs;
        private Dictionary<OSMMetaData, List<PolylineCurve>> foundWays;

        public ParseWaysWorker(GH_Component parent)
            : base(parent) // Pass parent component back to base class so state (e.g. remarks) can bubble up
        {
        }

        public override WorkerInstance Duplicate() => new ParseWaysWorker(this.Parent);

        public override void ExtractCoordsForComponentType()
        {
            ParseViaXMLReader.FindItemsByTag(ref this.result, OSMGeometryType.Way);
        }

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
            da.SetDataTree(0, this.wayOutputs);
        }
    }
}
