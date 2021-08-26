namespace Caribou.Components
{
    using System;
    using Caribou.Properties;
    using Eto.Forms;
    using Grasshopper.Kernel;
    using Caribou.Forms;
    using System.Collections.Generic;

    /// <summary>Provides a GUI interface to selecting/specifying predefined OSM features/subfeatures.</summary>
    public class SpecifyFeaturesComponent : BasePickerComponent
    {
        protected bool hideObscureFeatures = true;

        public SpecifyFeaturesComponent() : base("Specify Features", "OSM Specify",
            "Provides a graphical interface to specify a list of OSM features that the Extract components will then find.", "Select")
        {
            this.selectionState = SelectionCollection.GetCollection(this.hideObscureFeatures);
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager) { }

        protected override void CaribouRegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("OSM Features", "OF", "A list of OSM features and subfeatures", GH_ParamAccess.list);
        }

        protected override void CaribouSolveInstance(IGH_DataAccess da)
        {
            this.OutputMessageBelowComponent();
            da.SetDataList(0, selectionStateSerialized); // Update downstream text
        }

        // Methods required for button-opening
        protected override BaseCaribouForm GetFormForComponent() => new SpecifyFeaturesForm(this.selectionState, this.hideObscureFeatures);
        protected override string GetButtonTitle() => "Specify\nFeatures";

        protected override void CustomFormClose()
        {
            this.hideObscureFeatures = this.componentForm.customFlagState;
        }

        // Methods required for serial/deserial -ization
        protected override bool GetCustomFlagToSerialize() => this.hideObscureFeatures;

        protected override void SetCustomFlagFromDeserialize(bool valueToApply) {
            this.hideObscureFeatures = valueToApply;
        }

        // Standard GH
        public override Guid ComponentGuid => new Guid("cc8d82ba-f381-46ee-8014-7e2d1bff824c");
        protected override System.Drawing.Bitmap Icon => Resources.icons_select;
    }
}
