namespace Caribou.Components
{
    using System;
    using Caribou.Properties;
    using Grasshopper.Kernel;
    using Caribou.Forms;
    using System.Collections.Generic;
    using Caribou.Models;
    using Caribou.Forms.Models;

    /// <summary>Provides a GUI interface to selecting/specifying predefined OSM features/subfeatures.</summary>
    public class SpecifyFeaturesComponent : BasePickerComponent
    {

        public SpecifyFeaturesComponent() : base("Specify Features", "OSM Specify",
            "Provides a graphical interface to specify a list of OSM features that the Extract components will then find.", "Select")
        {
            // Setup form-items for tags provided and parsed into OSM/Form objects
            var indexOfParents = new Dictionary<string, int>();

            var primaryFeatures = new List<OSMMetaData>(OSMDefinedFeatures.Primary.Values);
            for (var i = 0; i < primaryFeatures.Count; i++)
            {
                var parentItem = new CaribouTreeGridItem(primaryFeatures[i], 0, 0, false);

                // Insert untagged item
                var description = $"Items that are specified as {primaryFeatures[i].Name}, but without more specific subfeature information";
                var childUntaggedOSM = new OSMMetaData("yes", "", 
                                                       description, primaryFeatures[i]);
                var childUntaggedItem = new CaribouTreeGridItem(childUntaggedOSM, 0, 0, false);
                parentItem.Children.Add(childUntaggedItem);

                this.selectableOSMs.Add(parentItem);
                indexOfParents[primaryFeatures[i].TagType] = i;
            }

            foreach (var item in OSMDefinedFeatures.SubFeatures())
            {
                var parentItem = this.selectableOSMs[indexOfParents[item["feature"]]] as CaribouTreeGridItem;

                var childOSM = new OSMMetaData(item["subfeature"], null, item["description"], 
                    parentItem.OSMData);
                var childItem = new CaribouTreeGridItem(childOSM, int.Parse(item["nodes"]), int.Parse(item["ways"]), false);
                parentItem.Children.Add(childItem);
            }
        }

        #region InOut Params
        protected override void RegisterInputParams(GH_InputParamManager pManager) { }

        protected override void CaribouRegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("OSM Features", "OF", "A list of OSM features and subfeatures", GH_ParamAccess.list);
        }
        #endregion

        protected override void CaribouSolveInstance(IGH_DataAccess da)
        {
            // If solving for the first time after a load, where state has been READ(), use that to make state
            if (this.storedSelectionState != null)
                this.selectableOSMs = TreeGridUtilities.SetSelectionsFromStoredState(this.selectableOSMs, this.storedSelectionState);

            this.selectionStateSerialized = GetSelectedKeyValuesFromForm();
            this.OutputMessageBelowComponent();
            da.SetDataList(0, selectionStateSerialized); // Update downstream text
        }

        protected override BaseCaribouForm GetFormForComponent() => new SpecifyFeaturesForm(this.selectableOSMs, this.hideObscureFeatures);

        protected override string GetButtonTitle() => "Specify\nFeatures";

        protected override string GetNoSelectionMessage() => "No Features Selected";

        // Standard GH
        public override Guid ComponentGuid => new Guid("cc8d82ba-f381-46ee-8014-7e2d1bff824c");
        protected override System.Drawing.Bitmap Icon => Resources.icons_select;
    }
}
