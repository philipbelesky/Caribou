namespace Caribou
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.ServiceModel.Syndication;
    using System.Xml;
    using Caribou.Properties;
    using Grasshopper.Kernel;

    public class ImportFeaturesComponent : CaribouAsyncComponent
    {
        public ImportFeaturesComponent() : base(
            "Import Features", "IF", "Reads data from an OSM XML file and produces geometry for a given feature type", "Caribou")
        {
            BaseWorker = new CaribouWorker();
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            // No input parameters needed
        }

        protected override void CaribouRegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

        public override Guid ComponentGuid => new Guid("a094da44-e3b6-4b46-b206-1ab81294696a");

        protected override System.Drawing.Bitmap Icon => Resources.icons_icon_plugin;
    }
}
