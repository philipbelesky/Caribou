namespace Caribou.Workers
{
    using System.Collections.Generic;
    using Caribou.Models;
    using Caribou.Components;
    using Caribou.Processing;
    using Grasshopper.Kernel;
    using Grasshopper.Kernel.Data;
    using Grasshopper.Kernel.Types;
    using Rhino.Geometry;

    /// <summary>Asynchronous task to identify and output all OSM nodes as Points for a given request.</summary>
    public class ParseBuildingsWorker : BaseLoadAndParseWorker
    {
        private GH_Structure<GH_Brep> buildingOutputs = new GH_Structure<GH_Brep>();
        private Dictionary<OSMMetaData, List<Brep>> foundBuildings;
        private bool outputHeighed;

        protected override OSMGeometryType WorkerType() {
            return OSMGeometryType.Building;
        }

        public ParseBuildingsWorker(GH_Component parent)
            : base(parent) // Pass parent component back to base class so state (e.g. remarks) can bubble up
        {
        }

        protected override void GetExtraData(IGH_DataAccess da)
        {
            base.GetExtraData(da);
            da.GetData(2, ref this.outputHeighed);
        }

        public override WorkerInstance Duplicate() => new ParseBuildingsWorker(this.Parent);

        public override void MakeGeometryForComponentType()
        {
            // Translate OSM nodes to Rhino Surfaces
            this.foundBuildings = TranslateToXYManually.BuildingBrepsFromCoords(this.result, this.outputHeighed);
        }

        public override void GetTreeForComponentType()
        {
            this.buildingOutputs = TreeFormatters.MakeTreeForBuildings(this.foundBuildings);
        }

        public override void OutputTreeForComponentType(IGH_DataAccess da)
        {
            if (this.buildingOutputs != null)
                da.SetDataTree(0, this.buildingOutputs);
                if (this.buildingOutputs.DataCount == 0)
                    this.RuntimeMessages.Add(new Message(
                        "No buildings were found with the specified features or tags.",
                        Message.Level.Warning));
        }
    }
}
