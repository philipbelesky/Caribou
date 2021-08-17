namespace Caribou.Processing
{
    using System;
    using Caribou.Components;
    using Caribou.Properties;
    using Grasshopper.Kernel;

    /// <summary>Identifies and outputs all OSM way-type items that building tags alongside any of the requested metadata. Logic in worker.</summary>
    public class FindBuildingsComponent : BaseFindComponent
    {
        public FindBuildingsComponent()
            : base("Extract Buildings", "Buildings", "Load and parse node (e.g. point) data from an OSM file based on its metadata")
        {
            this.BaseWorker = new ParseBuildingsWorker(this);
        }

        protected override void CaribouRegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddSurfaceParameter("Buildings", "B", "Buildings as extrusions from associated way geometries", GH_ParamAccess.tree);
            AddCommonOutputParams(pManager);
        }

        public override Guid ComponentGuid => new Guid("d6b1b021-2b5d-4fa6-9cf2-eb368dd632a1");

        protected override System.Drawing.Bitmap Icon => Resources.icons_buildings;
    }
}
