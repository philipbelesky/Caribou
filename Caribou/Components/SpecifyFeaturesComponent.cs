namespace Caribou.Components
{
    using System;
    using Grasshopper.Kernel;
    using Caribou.Models;
    using Caribou.Properties;
    using Caribou.Forms;

    /// <summary>Provides a GUI interface to selecting/specifying predefined OSM features/subfeatures.</summary>
    public class SpecifyFeaturesComponent : BasePickerComponent
    {
        public SpecifyFeaturesComponent() : base("Specify Features", "OSM Specify",
            "Provides a graphical interface to specify a list of OSM features that the Extract components will then find.", "Select")
        {            
            this.selectableOSMs = OSMPrimaryTypes.GetTreeCollection(); // Setup form-items for tags provided and parsed into OSM/Form objects
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
            // If solving for the first time after a load, where stored state has been READ()
            if (this.storedSelectionState != null)
            {
                this.selectableOSMs = TreeGridUtilities.SetSelectionsFromStoredState(
                    this.selectableOSMs, this.storedSelectionState); // Init options with state
                this.storedSelectionState = null; // Reset state now that it is set 
            }

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
