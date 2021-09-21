namespace Caribou.Processing
{
    using System;
    using Caribou.Components;
    using Caribou.Properties;
    using Caribou.Workers;
    using Grasshopper.Kernel;

    /// <summary>Identifies and outputs all OSM way-type items that building tags alongside any of the requested metadata. Logic in worker.</summary>
    public class FindBuildingsComponent : BaseFindComponent
    {
        public FindBuildingsComponent()
            : base("Extract Buildings", "Buildings", "Load and parse node (e.g. point) data from an OSM file based on its metadata")
        {
            this.BaseWorker = new ParseBuildingsWorker(this);
        }

        protected override void RegisterExtraInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Output Heighted", "OH?", 
                "If true, only outputs buildings with height data. If false, only outputs buildings without height data.", 
                GH_ParamAccess.item, true);
        }

        protected override void CaribouRegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBrepParameter("Buildings", "B", "Buildings as extrusions from associated way geometries", GH_ParamAccess.tree);
            AddCommonOutputParams(pManager);
        }

        public override Guid ComponentGuid => new Guid("d6b1b021-2b5d-4fa6-9cf2-eb368dd632a1");

        protected override System.Drawing.Bitmap Icon => Resources.icons_buildings;
    }
}
