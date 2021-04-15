namespace Caribou.Processing
{
    using System;
    using Caribou.Properties;
    using Grasshopper.Kernel;
    using Rhino.Geometry;

    public class LoadAndParseComponent : CaribouAsyncComponent
    {
        public LoadAndParseComponent() : base(
         "OpenStreetMap", "OSM", "Load and parse data from an OSM file based on its key", "OSM")
        {
            BaseWorker = new CaribouWorker();
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("OSM Content", "C", "The contents of an XML OSM file (use the output of a Read File component", GH_ParamAccess.list);
        }

        protected override void CaribouRegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Ways", "W", "Ways; e.g. nodes linked in a linear order via a Polyline", GH_ParamAccess.list);
            pManager.AddPointParameter("Nodes", "N", "Notes; e.g. points that describe a location of interest", GH_ParamAccess.list);
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

        public override Guid ComponentGuid => new Guid("912176ea-061e-2b5b-9642-8417372d6371");

        protected override System.Drawing.Bitmap Icon => Resources.icons_icon_test;
    }
}
