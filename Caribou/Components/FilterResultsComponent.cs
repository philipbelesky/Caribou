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
        private string PreviousTagsDescription { get; set; } // Need to track this internally to figure out when to force-refresh the form

        protected const string ReportDescription = "The name, description, and number of items found of each specified tag";

        public FilterResultsComponent() : base("Filter Tags", "OSM Filter",
            "Provides a graphical interface of OSM features to filter the results of an Extract component based on common tags.", "Select")
        {
            this.selectionState = null;
        }

        #region InOut Params
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Items", "I", "Nodes, Ways, or Building outputs from one of the Extract components", GH_ParamAccess.tree);
            pManager.AddTextParameter("Tags", "R", "The Tags output from the same extract component whose nodes/ways/buildings you are providing as Items", GH_ParamAccess.tree);
        }

        protected override void CaribouRegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGeometryParameter("Items", "I", "The geometry that possess the specified tags", GH_ParamAccess.tree);
            pManager.AddTextParameter("Tags", "T", "The metadata attached to each particular item", GH_ParamAccess.tree);
            pManager.AddTextParameter("Report", "R", "The name, count, and description of each feature", GH_ParamAccess.tree);
        }
        #endregion

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

            // If solving for the first time after a load, where state has been READ(), use that to make state
            if (this.storedState != null)
            { 
                // If initing from a GHX with saved state
                this.selectionState = TreeGridUtilities.MakeOSMCollectionFromStoredState(
                    this.selectableData, storageKeyForHideObscure, this.hideObscureFeatures);
                this.selectionStateSerialized = GetSelectedKeyValuesFromForm(); // Load selected form items as key-values
                this.PreviousTagsDescription = tagsTree.DataDescription(false, false);
                this.storedState = null; // Reset flag
            }
            else if (this.selectionState == null || tagsTree.DataDescription(false, false) != this.PreviousTagsDescription)
            {
                // If initing from a GHX without saved state or if the incoming data just changed; OR...
                // If the provided tags have updated then we need to recalculate what the form should show
                this.selectionState = TreeGridUtilities.MakeOSMCollectionWithoutState(this.selectableData, this.hideObscureFeatures);
                this.selectionStateSerialized = GetSelectedKeyValuesFromForm(); // Load selected form items as key-values
                this.PreviousTagsDescription = tagsTree.DataDescription(false, false);
            }

            logger.NoteTiming("State loading/making");

            // Match geometry paths to selected filters
            var geometryOutput = new GH_Structure<IGH_Goo>();
            var tagOutput = new GH_Structure<GH_String>();
            // Setup tracking dictionary for the report tree output
            var foundItemCountsForResult = new Dictionary<OSMMetaData, int>();

            for (int i = 0; i < this.selectionStateSerialized.Count; i++)
            {
                string itemKeyValue = this.selectionStateSerialized[i];
                OSMMetaData osmItem = new OSMMetaData(itemKeyValue);

                if (!requests.pathsPerItem.ContainsKey(osmItem))
                    continue;

                foundItemCountsForResult[osmItem] = 0;
                // Match keyvalues to OSMListwithpaths
                for (int j = 0; j < requests.pathsPerItem[osmItem].Count; j++)
                {
                    GH_Path inputPath = requests.pathsPerItem[osmItem][j];                    

                    var geometryItemsForPath = itemsTree.get_Branch(inputPath);
                    var tagItemsForPath = tagsTree.get_Branch(inputPath);
                    if (geometryItemsForPath == null)
                        continue; // No provided geometry path for that OSM item

                    foundItemCountsForResult[osmItem] += 1;
                    for (int k = 0; k < geometryItemsForPath.Count; k++)
                    {
                        GH_Path outputPathFor = new GH_Path(i, j);
                        geometryOutput.EnsurePath(outputPathFor); // Need to ensure even an empty path exists to enable data matching
                        tagOutput.EnsurePath(outputPathFor); // Need to ensure even an empty path exists to enable data matching

                        geometryOutput.Append(geometryItemsForPath[k] as IGH_Goo, outputPathFor);
                        foreach (GH_String tag in tagItemsForPath)
                        {
                            tagOutput.Append(tag, outputPathFor);
                        }
                    }
                }
            }
            logger.NoteTiming("Geometry matching");

            var requestReport = TreeFormatters.MakeReportForRequests(foundItemCountsForResult);
            logger.NoteTiming("Tree formatting");

            this.OutputMessageBelowComponent();
            da.SetDataTree(0, geometryOutput);
            da.SetDataTree(1, tagOutput);
            da.SetDataTree(2, requestReport);
        }

        protected override BaseCaribouForm GetFormForComponent() => new FilterFeaturesForm(this.selectionState, this.hideObscureFeatures);
        protected override string GetButtonTitle() => "Filter\nTags";
        protected override void ButtonOpenAction() // Form-button interaction; passed to CustomSetButton as handler action
        {
            if (this.selectionState != null) // e.g. if we have connected inputs, and thus state to show in the form
                OpenForm();
        }
        protected override string GetNoSelectionMessage() => "No Tags Selected";

        // Standard GH
        public override Guid ComponentGuid => new Guid("0e86143a-d051-488b-bf65-b91087bce4ac");
        protected override System.Drawing.Bitmap Icon => Resources.icons_filter;
    }
}
