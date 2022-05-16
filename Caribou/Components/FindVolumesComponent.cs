namespace Caribou.Processing
{
    using System;
    using Caribou.Components;
    using Caribou.Properties;
    using Caribou.Workers;
    using Grasshopper.Kernel;

    /// <summary>Identifies and outputs all OSM way-type items that building tags alongside any of the requested metadata. Logic in worker.</summary>
    public class FindVolumesComponent : BaseFindComponent
    {
        public FindVolumesComponent()
            : base("Extract Volumes", "Volumes", "Try to create volumes or outlines of data with heigh values (e.g. buildings) by parsing data in an OSM file.")
        {
            this.BaseWorker = new ParseVolumesWorker(this);
        }

        protected override void RegisterExtraInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Output Heighted", "OH?",
                "If true, only outputs items with height data. If false, only outputs items without height data.", 
                GH_ParamAccess.item, true);
        }

        protected override void CaribouRegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBrepParameter("Volumes", "V", "Buildings and/or other objects as extrusions from associated way geometries", GH_ParamAccess.tree);
            AddCommonOutputParams(pManager);
        }

        public override Guid ComponentGuid => new Guid("d6b1b021-2b5d-4fa6-9cf2-eb368dd632a1");

        protected override System.Drawing.Bitmap Icon => Resources.icons_volumes;
    }
}
