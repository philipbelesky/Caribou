namespace Caribou.Processing
{
    using System;
    using System.Windows.Forms;
    using Caribou.Properties;
    using Grasshopper.Kernel;
    using Rhino.Geometry;

    public class LoadAndParseComponent : CaribouAsyncComponent
    {
        public bool AddCountsToFeatureReporting = true;

        public LoadAndParseComponent()
            : base("OpenStreetMap", "OSM", "Load and parse data from an OSM file based on its key", "OSM")
            {
                this.BaseWorker = new LoadAndParseWorker(this);
            }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("OSM Content", "C", "The contents of an XML OSM file (use the output of a Read File component)", GH_ParamAccess.list);
            pManager.AddTextParameter("OSM Features", "F", "A list of features and subfeatures to extract from the OSM file, in a 'key:value' format separated by newlines or commas", GH_ParamAccess.list);
        }

        protected override void CaribouRegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Nodes", "N", "Notes; e.g. points that describe a location of interest", GH_ParamAccess.tree);
            pManager.AddCurveParameter("Ways", "W", "Ways; e.g. nodes linked in a linear order via a Polyline", GH_ParamAccess.tree);
            pManager.AddTextParameter("Features", "F", "The requested OSM features formatted as a tree structure to match the content", GH_ParamAccess.tree);
        }

        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);
            Menu_AppendItem(menu, "Report quantities", ToggleQuantityReporting, true, AddCountsToFeatureReporting);
            Menu_AppendSeparator(menu);
        }

        private void ToggleQuantityReporting(object sender, EventArgs e)
        {
            this.AddCountsToFeatureReporting = !this.AddCountsToFeatureReporting;
            ExpireSolution(true);
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

        public override Guid ComponentGuid => new Guid("912176ea-061e-2b5b-9642-8417372d6371");

        protected override System.Drawing.Bitmap Icon => Resources.icons_nodes;
    }
}
