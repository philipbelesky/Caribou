namespace Caribou.Components
{
    using System;
    using System.Collections.Generic;
    using Grasshopper.Kernel;
    using Grasshopper.Kernel.Types;
    using Grasshopper.Kernel.Data;
    using Caribou.Forms;
    using Caribou.Properties;
    using Caribou.Models;
    using Caribou.Forms.Models;
    using Eto.Forms;
    using System.Linq;
    using Rhino.Geometry;

    /// <summary>Provides a GUI interface to selecting/specifying OSM features for a given set of nodes/ways/buildings provided upstream </summary>
    public class FilterResultsComponent : BasePickerComponent
    {
        private string PreviousTagsDescription { get; set; } // Need to track this internally to figure out when to force-refresh the form

        protected const string ReportDescription = "The name, description, and number of items found of each specified tag";
        protected bool ProvidedNodes; // Tracking if provided data is points or curves/breps

        public FilterResultsComponent() : base("Filter Tags", "OSM Filter",
            "Provides a graphical interface of OSM features to filter the results of an Extract component based on common tags.", "Select")
        {
            this.selectableOSMs = null; // Set during solve
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

            #region Input Parsing
            GH_Structure<IGH_Goo> itemsTree;
            da.GetDataTree(0, out itemsTree);
            if (itemsTree.Branches[0][0] as IGH_GeometricGoo == null)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error,
                   "It looks like you have provided a non-geometry input to the Items parameter. This input should connect to the Nodes, Ways, or Buildings outputs produced by the Extract components.");
                return;
            }
            var geometryTest = itemsTree.Branches[0][0] as GH_Point;
            ProvidedNodes = geometryTest != null;

            GH_Structure<GH_String> tagsTree;
            da.GetDataTree(1, out tagsTree);
            if (tagsTree.Branches[0].Count >= 3)
                if (tagsTree.Branches[0][2].ToString().Contains(" found"))
                {
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error,
                       "It looks like you have provided a Report parameter output as the Tag parameter input. Use a Tag parameter output instead.");
                    return;
                }

            if (itemsTree.PathCount != tagsTree.PathCount)
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning,
                    "The path counts of the Items and Tags do not match - check these are coming from the same component.");
            else if (itemsTree.Branches.Count != tagsTree.Branches.Count)
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, 
                    "The branch structure of the Items and Tags do not match - check these are coming from the same component.");

            logger.NoteTiming("Input capture");
            #endregion

            #region Form Data Setup
            // Setup form-presentable items for tags provided and parsed into OSM objects
            var requests = new OSMListWithPaths(tagsTree); // Parse provided tree into OSM objects and a dictionary of paths per object
            logger.NoteTiming("Tag parsing");

            // If tags have changed we write out current state so we can try to preserve it in the new tags list
            if (tagsTree.DataDescription(false, false) != this.PreviousTagsDescription) 
                this.storedSelectionState = GetStateKeys();
            
            // If loading from scratch, or if the tags have changed
            if (this.storedSelectionState != null) 
            {
                var availableOSMs = GetSelectableTagsFromInputTree(requests);
                this.selectableOSMs = TreeGridUtilities.SetSelectionsFromStoredState(
                    availableOSMs, this.storedSelectionState);
                this.storedSelectionState = null; // Reset flag
            }

            this.PreviousTagsDescription = tagsTree.DataDescription(false, false); // Track tag input identity
            logger.NoteTiming("State loading/making");
            #endregion

            #region Outputting
            // Match geometry paths to selected filters
            var geometryOutput = new GH_Structure<IGH_Goo>();
            var tagOutput = new GH_Structure<GH_String>();
            // Setup tracking dictionary for the report tree output
            var foundItemCountsForResult = new Dictionary<OSMTag, int>();

            for (int i = 0; i < this.selectionStateSerialized.Count; i++)
            {
                string itemKeyValue = this.selectionStateSerialized[i];
                OSMTag osmItem = new OSMTag(itemKeyValue);

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

            this.selectionStateSerialized = GetSelectedKeyValuesFromForm();
            this.OutputMessageBelowComponent();

            da.SetDataTree(0, geometryOutput);
            da.SetDataTree(1, tagOutput);
            da.SetDataTree(2, requestReport);
            logger.NoteTiming("Data tree setting");
            #endregion
        }

        /// <summary>Parse the Grasshopper data tree into the form's data tree</summary>
        protected TreeGridItemCollection GetSelectableTagsFromInputTree(OSMListWithPaths requests)
        {
            var indexOfParents = new Dictionary<string, int>();
            var sortedTags = requests.items.OrderBy(t => t.ToString()).ToList();
            var selectableTags = new TreeGridItemCollection();

            foreach (var tag in sortedTags)
            {
                if (tag.Key != null)
                {
                    if (!indexOfParents.ContainsKey(tag.Key.Value))
                    {
                        var parentItem = new CaribouTreeGridItem(tag.Key, 0, 0, true, false);
                        selectableTags.Add(parentItem);
                        indexOfParents[parentItem.OSMData.Value] = selectableTags.Count - 1;
                    }
                }

                var nodeCount = 0; var wayCount = 0;
                if (ProvidedNodes)
                    nodeCount = requests.pathsPerItem[tag].Count();
                else
                    wayCount = requests.pathsPerItem[tag].Count();

                var childItem = new CaribouTreeGridItem(tag, nodeCount, wayCount, true, false);
                if (childItem.OSMData.Key != null)
                {
                    var parentKey = indexOfParents[childItem.OSMData.Key.Value];
                    var parent = selectableTags[parentKey] as CaribouTreeGridItem;
                    parent.Children.Add(childItem);
                }
            }

            return selectableTags;
        }

        protected override BaseCaribouForm GetFormForComponent() => new FilterFeaturesForm(this.selectableOSMs, this.hideObscureFeatures);
        protected override string GetButtonTitle() => "Filter\nTags";
        protected override void ButtonOpenAction() // Form-button interaction; passed to CustomSetButton as handler action
        {
            if (this.selectableOSMs != null) // e.g. if we have connected inputs, and thus state to show in the form
                OpenForm();
        }
        protected override string GetNoSelectionMessage() => "No Tags Selected";

        // Standard GH
        public override Guid ComponentGuid => new Guid("0e86143a-d051-488b-bf65-b91087bce4ac");
        protected override System.Drawing.Bitmap Icon => Resources.icons_filter;
    }
}
