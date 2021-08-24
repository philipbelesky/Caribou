namespace Caribou.Workers
{
    using System.Collections.Generic;
    using Caribou.Models;
    using Caribou.Processing;
    using Grasshopper.Kernel;
    using Grasshopper.Kernel.Data;
    using Grasshopper.Kernel.Types;
    using Rhino.Geometry;

    /// <summary>Asynchronous task to identify and output all OSM nodes as Points for a given request.</summary>
    public class ParseBuildingsWorker : BaseLoadAndParseWorker
    {
        private GH_Structure<GH_Surface> buildingOutputs = new GH_Structure<GH_Surface>();
        private Dictionary<OSMMetaData, List<Surface>> foundBuildings;        
        protected override OSMGeometryType WorkerType() {
            return OSMGeometryType.Building;
        }

        public ParseBuildingsWorker(GH_Component parent)
            : base(parent) // Pass parent component back to base class so state (e.g. remarks) can bubble up
        {
        }

        public override WorkerInstance Duplicate() => new ParseBuildingsWorker(this.Parent);

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
            if (this.buildingOutputs != null)
                da.SetDataTree(0, this.buildingOutputs);
        }
    }
}
