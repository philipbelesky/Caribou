namespace Caribou.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Caribou.Properties;
    using Grasshopper.Kernel;

    public class SpecifyFeaturesComponent : CaribouComponent
    {
        public SpecifyFeaturesComponent() : base ("Specify features and subfeatures to extract", "Set Features", "TODO", "OSM") { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            // None needed
        }

        protected override void CaribouRegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("OSM Features", "F", "A list of OSM features and subfeatures", GH_ParamAccess.list);
        }

        protected override void CaribouSolveInstance(IGH_DataAccess da)
        {

        }

        public override Guid ComponentGuid => new Guid("cc8d82ba-f381-46ee-8014-7e2d1bff824c");

        protected override System.Drawing.Bitmap Icon => Resources.icons_icon_test;
    }
}
