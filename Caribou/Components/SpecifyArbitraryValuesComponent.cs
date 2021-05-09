namespace Caribou.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Caribou.Properties;
    using Grasshopper.Kernel;

    /// <summary>Provides affordances for specifying arbitrary key:value pairings to be found.</summary>
    public class SpecifyArbitraryValuesComponent : CaribouComponent
    {
        public SpecifyArbitraryValuesComponent() : base("Specify tags and values to extract", "Set Tags", "TODO", "OSM") { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            // None needed
        }

        protected override void CaribouRegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("OSM Features", "F", "A list of OSM tags and keys", GH_ParamAccess.list);
        }

        protected override void CaribouSolveInstance(IGH_DataAccess da)
        {
        }

        public override Guid ComponentGuid => new Guid("19eb5d29-d1d1-43bf-aa40-8d9694c03481");

        protected override System.Drawing.Bitmap Icon => Resources.icons_icon_test;
    }
}
