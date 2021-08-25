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
        private FilterFeaturesForm componentForm;
        private FilterRequest filterableTags;
        // By default any item with any of the specified tags passed. If true, items must possess all tags
        private bool resultsMustHaveAllTags = false;

        protected const string ReportDescription = "The name, description, and number of items found of each specified tag";

        public FilterResultsComponent() : base("Filter Tags", "OSM Filter",
            "Provides a graphical interface of OSM features to filter the results of an Extract component based on common tags.", "Select")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Items", "I", "TODO", GH_ParamAccess.tree);
            pManager.AddTextParameter("Tags", "R", "TODO", GH_ParamAccess.tree);
            // TODO key:value params
        }

        protected override void CaribouRegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGeometryParameter("Items", "I", "The geometry that possess the specified tags", GH_ParamAccess.tree);
            pManager.AddTextParameter("Tags", "T", "The metadata attached to each particular item", GH_ParamAccess.tree);
            pManager.AddTextParameter("Report", "R", "The name, count, and description of each feature", GH_ParamAccess.tree);
        }

        protected override void CaribouSolveInstance(IGH_DataAccess da)
        {
            GH_Structure<IGH_Goo> itemsTree;
            da.GetDataTree(0, out itemsTree);
            // TODO: validate

            GH_Structure<GH_String> tagsTree;
            da.GetDataTree(1, out tagsTree);
            // TODO: validate

            this.filterableTags = new FilterRequest(tagsTree);

            // TODO: Match geometry paths to selected filters

            this.OutputMessageBelowComponent();
        }

        // Methods required for button-opening
        protected override Eto.Forms.Form GetFormForComponent()
        {
            this.componentForm = new FilterFeaturesForm(this.selectionState, this.resultsMustHaveAllTags);
            return this.componentForm;
        }

        protected override string GetButtonTitle() => "Specify\nTags";

        protected override void StartFormClose()
        {
            this.selectionState = this.componentForm.mainRow.data;
            this.resultsMustHaveAllTags = this.componentForm.customFlagState;
            FinishFormClose();
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
