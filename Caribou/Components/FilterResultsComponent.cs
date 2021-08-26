namespace Caribou.Components
{
    using System;
    using Caribou.Properties;
    using Eto.Forms;
    using Grasshopper.Kernel;
    using Caribou.Forms;
    using System.Collections.Generic;
    using Grasshopper.Kernel.Types;
    using Grasshopper.Kernel.Data;
    using Caribou.Models;

    /// <summary>
    /// Provides a GUI interface to selecting/specifying OSM features for a given set of nodes/ways/buildings provided upstream
    /// </summary>
    public class FilterResultsComponent : BasePickerComponent
    {
        // By default any item with any of the specified tags passed. If true, items must possess all tags
        private bool resultsMustHaveAllTags = false;
        // For tracking geometry that is associated with a particular tag
        public Dictionary<OSMMetaData, List<GH_Path>> pathsPerRequest;

        protected const string ReportDescription = "The name, description, and number of items found of each specified tag";

        public FilterResultsComponent() : base("Filter Tags", "OSM Filter",
            "Provides a graphical interface of OSM features to filter the results of an Extract component based on common tags.", "Select")
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Items", "I", "TODO", GH_ParamAccess.tree);
            pManager.AddTextParameter("Tags", "R", "TODO", GH_ParamAccess.tree);
            // TODO key:value params provided as overrides
        }

        protected override void CaribouRegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGeometryParameter("Items", "I", "The geometry that possess the specified tags", GH_ParamAccess.tree);
            pManager.AddTextParameter("Tags", "T", "The metadata attached to each particular item", GH_ParamAccess.tree);
            pManager.AddTextParameter("Report", "R", "The name, count, and description of each feature", GH_ParamAccess.tree);
        }

        protected override void CaribouSolveInstance(IGH_DataAccess da)
        {
            logger.Reset();

            GH_Structure<IGH_Goo> itemsTree;
            da.GetDataTree(0, out itemsTree);
            // TODO: validate

            GH_Structure<GH_String> tagsTree;
            da.GetDataTree(1, out tagsTree);
            // TODO: validate

            logger.NoteTiming("Input capture");

            var requests = new OSMListWithPaths(tagsTree); // Parse provided tree into OSM objects and a dictionary of paths per object
            logger.NoteTiming("Tag parsing");

            this.selectableData = new SelectableDataCollection(requests.items); // Setup form-able items for tags provided and parsed into OSM/Form objects
            logger.NoteTiming("Tag processing");

            this.selectionState = TreeGridUtilities.MakeOSMCollection(this.selectableData);
            logger.NoteTiming("Tag processing");

            // TODO: Match geometry paths to selected filters

            logger.NoteTiming("Geometry matching");

            this.OutputMessageBelowComponent();
        }

        // Methods required for button-opening
        protected override BaseCaribouForm GetFormForComponent() => new FilterFeaturesForm(this.selectionState, this.resultsMustHaveAllTags);
        protected override string GetButtonTitle() => "Specify\nTags";
        protected override string GetNoSelectionMessage() => "No Tags Selected";

        protected override void CustomFormClose()
        {
            this.resultsMustHaveAllTags = this.componentForm.customFlagState;
        }

        // Methods required for serial/deserial -ization
        protected override bool GetCustomFlagToSerialize() => this.resultsMustHaveAllTags;

        protected override void SetCustomFlagFromDeserialize(bool valueToApply)
        {
            this.resultsMustHaveAllTags = valueToApply;
        }

        // Standard GH
        public override Guid ComponentGuid => new Guid("0e86143a-d051-488b-bf65-b91087bce4ac");
        protected override System.Drawing.Bitmap Icon => Resources.icons_filter;
    }
}
