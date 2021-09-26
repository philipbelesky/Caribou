namespace Caribou.Forms
{
    using System;
    using System.Collections.Generic;
    using Caribou.Forms;
    using Eto.Drawing;
    using Eto.Forms;

    public class SpecifyFeaturesForm : BaseCaribouForm
    {
        public SpecifyFeaturesForm(TreeGridItemCollection selectionState, bool hideObscureFeatures) 
            : base(selectionState, "Specify Features and Sub-Features", hideObscureFeatures)
        { }

        protected override string GetLabelForHideObscure() => " Hide sub features with very low (worldwide) counts";
    }
}
