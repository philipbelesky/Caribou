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
    public class ParseVolumesWorker : BaseLoadAndParseWorker
    {
        private GH_Structure<GH_Brep> volumeOutputs = new GH_Structure<GH_Brep>();
        private Dictionary<OSMTag, List<Brep>> foundVolumes;
        private bool outputHeighed;

        protected override OSMGeometryType WorkerType() {
            return OSMGeometryType.Way;
        }

        public ParseVolumesWorker(GH_Component parent)
            : base(parent) // Pass parent component back to base class so state (e.g. remarks) can bubble up
        {
        }

        protected override void GetExtraData(IGH_DataAccess da)
        {
            base.GetExtraData(da);
            da.GetData(2, ref this.outputHeighed);
        }

        public override WorkerInstance Duplicate() => new ParseVolumesWorker(this.Parent);

        public override void MakeGeometryForComponentType()
        {
            // Translate OSM nodes to Rhino Surfaces
            this.foundVolumes = TranslateToXYManually.BuildingBrepsFromCoords(ref this.result, this.outputHeighed);
        }

        public override void GetTreeForComponentType()
        {
            this.volumeOutputs = TreeFormatters.MakeTreeForBuildings(this.foundVolumes);
        }

        public override void OutputTreeForComponentType(IGH_DataAccess da)
        {
            if (this.volumeOutputs != null)
                da.SetDataTree(0, this.volumeOutputs);
                if (this.volumeOutputs.DataCount == 0)
                    this.RuntimeMessages.Add(new Message(
                        "No volumes were found with the specified features or tags.",
                        Message.Level.Warning));
        }
    }
}
