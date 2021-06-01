namespace Caribou.Processing
{
    using System;
    using Caribou.Components;
    using Caribou.Properties;
    using Grasshopper.Kernel;

    /// <summary>Identifies and outputs all OSM node-type items that contain any of the requested metadata. Logic in worker.</summary>
    public class FindNodesComponent : BaseFindComponent
    {
        public FindNodesComponent()
            : base("Extract Nodes", "Nodes", "Load and parse node (e.g. point) data from an OSM file based on its metadata")
        {
            this.BaseWorker = new ParseNodesWorker(this);
        }

        protected override void CaribouRegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Nodes", "N", "Nodes; e.g. points that describe a location of interest", GH_ParamAccess.tree);
            pManager.AddTextParameter("Tags", "T", "The metadata attached to each particular node", GH_ParamAccess.tree);
            pManager.AddTextParameter("Report", "R", ReportDescription, GH_ParamAccess.tree);
        }

        public override Guid ComponentGuid => new Guid("912176ea-061e-2b5b-9642-8417372d6371");

        protected override System.Drawing.Bitmap Icon => Resources.icons_nodes;
    }
}
