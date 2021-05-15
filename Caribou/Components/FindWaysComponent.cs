namespace Caribou.Processing
{
    using System;
    using System.Windows.Forms;
    using Caribou.Properties;
    using Grasshopper.Kernel;
    using Rhino.Geometry;

    /// <summary>Identifies and outputs all OSM way-type items that contain any of the requested metadata. Logic in worker.</summary>
    public class FindWaysComponent : CaribouAsyncComponent
    {
        public FindWaysComponent()
            : base("OpenStreetMap", "OSM", "Load and parse way (e.g. polyline) data from an OSM file based on its metadata", "OSM")
        {
            this.BaseWorker = new ParseWaysWorker(this);
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("OSM Content", "C", "The contents of an XML OSM file (use the output of a Read File component)", GH_ParamAccess.list);
            pManager.AddTextParameter("OSM Features", "F", "A list of features and subfeatures to extract from the OSM file, in a 'key:value' format separated by newlines or commas", GH_ParamAccess.list);
        }

        protected override void CaribouRegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Ways", "W", "Ways; e.g. nodes linked in a linear order via a Polyline", GH_ParamAccess.tree);
            pManager.AddTextParameter("Tags", "T", "The metadata attached to each particular node", GH_ParamAccess.tree);
            pManager.AddTextParameter("Report", "R", "The name, count, and description of each feature", GH_ParamAccess.tree);
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

        public override Guid ComponentGuid => new Guid("f677053e-0416-433b-9a8e-ce3124998b7d");

        protected override System.Drawing.Bitmap Icon => Resources.icons_ways;
    }
}
