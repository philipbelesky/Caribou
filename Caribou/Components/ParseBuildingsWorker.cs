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
    public class ParseBuildingsWorker : BaseLoadAndParseWorker
    {
        private GH_Structure<GH_Surface> buildingOutputs;
        private Dictionary<OSMMetaData, List<Surface>> foundBuildings;

        public ParseBuildingsWorker(GH_Component parent)
            : base(parent) // Pass parent component back to base class so state (e.g. remarks) can bubble up
        {
        }

        public override WorkerInstance Duplicate() => new ParseBuildingsWorker(this.Parent);

        public override void ExtractCoordsForComponentType(Action<string, double> reportProgress)
        {
            ParseViaXMLReader.FindItemsByTag(ref this.result, OSMGeometryType.Building, reportProgress, Id);
        }

        public override void MakeGeometryForComponentType()
        {
            // Translate OSM nodes to Rhino Surfaces
            this.foundBuildings = TranslateToXYManually.BuildingSurfacesFromCoords(this.result);
        }

        public override void GetTreeForComponentType()
        {
            this.buildingOutputs = TreeFormatters.MakeTreeForBuildings(this.foundBuildings);
        }

        public override void OutputTreeForComponentType(IGH_DataAccess da)
        {
            da.SetDataTree(0, this.buildingOutputs);
        }
    }
}
