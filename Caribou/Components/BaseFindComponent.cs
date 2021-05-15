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
        protected const string reportDescription = "The name, count, and description of each feature";
        public BaseFindComponent(string name, string nickname, string description)
            : base(name, nickname, description, "OSM Parsers") { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("OSM Content", "C", "The contents of an XML OSM file (use the output of a Read File component)", GH_ParamAccess.list);
            pManager.AddTextParameter("OSM Features", "F", "A list of features and subfeatures to extract from the OSM file, in a 'key=value' format separated by newlines or commas", GH_ParamAccess.list);
        }
        public override GH_Exposure Exposure => GH_Exposure.primary;
    }
}
