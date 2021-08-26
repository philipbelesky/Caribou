namespace Caribou.Forms
{
    using System;
    using System.Collections.Generic;
    using Caribou.Forms;
    using Eto.Drawing;
    using Eto.Forms;

    public class SpecifyFeaturesForm : BaseCaribouForm
    {
        private CheckBox obscureFeaturesCheckbox; // Need to track so we can manually set state

        public SpecifyFeaturesForm(TreeGridItemCollection selectionState, bool hideObscureFeatures) 
            : base(selectionState, "Specify Features and Sub-Features", hideObscureFeatures)
        { }

        protected override void AddCustomButtonsToTop()
        {
            this.obscureFeaturesCheckbox = ControlStrip.GetHider(ToggleObscureFeatures, this.customFlagState);
            this.topButtons.Add(obscureFeaturesCheckbox);
        }

        private void ToggleObscureFeatures()
        {
            this.customFlagState = this.obscureFeaturesCheckbox.Checked.Value;
            this.mainRow.viewForm.DataStore = TreeGridUtilities.FilterOSMCollection(this.providedSelectionState, this.customFlagState);
            this.mainRow.viewForm.ReloadData();
        }
    }
}
