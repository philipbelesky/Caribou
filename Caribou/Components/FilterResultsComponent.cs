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

        private List<OSMMetaData> requests = new List<OSMMetaData>();
        // For tracking geometry that is associated with a particular tag
        private Dictionary<OSMMetaData, List<GH_Path>> pathsPerRequest = new Dictionary<OSMMetaData, List<GH_Path>>();

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

            ParseTagsToOSM(tagsTree); // Parse provided tree into OSM objects and a dictionary of paths per object
            logger.NoteTiming("Tag parsing");

            this.selectableData = ParseOSMToSelectableOSM(); // Setup form-able items for tags provided and parsed into OSM/Form objects
            logger.NoteTiming("Tag processing");

            this.selectionState = TreeGridUtilities.MakeOSMCollection(this.selectableData);
            logger.NoteTiming("Tag processing");

            // TODO: Match geometry paths to selected filters

            logger.NoteTiming("Geometry matching");

            this.OutputMessageBelowComponent();
        }

        private void ParseTagsToOSM(GH_Structure<GH_String> tagsTree)
        {
            // Convert from tree of strings representing tags to linear list of OSM Items
            for (int pathIndex = 0; pathIndex < tagsTree.Paths.Count; pathIndex++)
            {
                var path = tagsTree.Paths[pathIndex];
                List<GH_String> itemsInPath = tagsTree[path];
                for (int tagIndex = 0; tagIndex < itemsInPath.Count; tagIndex++)
                {
                    // Make new item and track the path it came from
                    var tagString = itemsInPath[tagIndex];
                    var osmItem = new OSMMetaData(tagString.ToString());
                    if (osmItem != null)
                    {
                        if (!this.requests.Contains(osmItem)) // Prevent duplicates
                            this.requests.Add(osmItem);

                        if (this.pathsPerRequest.ContainsKey(osmItem))
                            this.pathsPerRequest[osmItem].Add(path);
                        else
                            this.pathsPerRequest[osmItem] = new List<GH_Path>() { path };
                    }
                }
            }
        }

        // Translate a list of OSMMetaData items into OSMSelectableFeatures for showing in the form
        // E.g. like  OSMDefinedFeatures.GetDefinedFeaturesForForm() but for dynamic tags
        private Dictionary<OSMSelectableData, List<OSMSelectableData>> ParseOSMToSelectableOSM()
        {
            var allDataInHierarchy = new Dictionary<OSMSelectableData, List<OSMSelectableData>>();

            foreach (var tag in this.requests)
            {
                var selectableTag = new OSMSelectableData(tag.TagType, tag.Name, tag.Explanation, 0, 0, false);

                var parent = tag.ParentType;
                var selectableParent = new OSMSelectableData(parent.TagType, parent.Name, parent.Explanation, 0, 0, false);

                if (!allDataInHierarchy.ContainsKey(selectableParent))
                    allDataInHierarchy[selectableParent] = new List<OSMSelectableData>() { selectableTag };
                else
                    allDataInHierarchy[selectableParent].Add(selectableTag);
            }

            // Sort child items alphabetically (parents are sorted in MakeOSMCollection)
            foreach (var parentType in allDataInHierarchy)
                parentType.Value.Sort();

            return allDataInHierarchy;
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
