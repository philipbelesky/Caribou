namespace Caribou.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Grasshopper.Kernel;

    public abstract class BaseFindComponent : CaribouAsyncComponent
    {
        protected const string ReportDescription = "The name, count, and description of each feature";
        public BaseFindComponent(string name, string nickname, string description)
            : base(name, nickname, description, "Parse") { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("OSM File Path", "FP", "The path to XML file(s) downloaded from Open Street map", GH_ParamAccess.list);
            pManager.AddTextParameter("OSM Features", "OF", "A list of features and subfeatures to extract from the OSM file, in a 'key=value' format separated by newlines or commas", GH_ParamAccess.list);
        }

        protected void AddCommonOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Tags", "T", "The metadata attached to each particular node", GH_ParamAccess.tree);
            pManager.AddTextParameter("Report", "R", ReportDescription, GH_ParamAccess.tree);
            pManager.AddRectangleParameter("Bounds", "B", "The boundary extends of the OSM file(s)", GH_ParamAccess.list);
        }
         
        public override GH_Exposure Exposure => GH_Exposure.primary;
    }
}
