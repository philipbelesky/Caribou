namespace Caribou.Forms
{
    using System;
    using System.Collections.Generic;
    using Caribou.Forms;
    using Eto.Drawing;
    using Eto.Forms;

    public class FilterTagsForm : BaseForm
    {
        public FilterTagsForm(TreeGridItemCollection selectionState, bool hideObscureFeatures)
            : base(selectionState, "Select Tags to Filter For", hideObscureFeatures)
        { }

        protected override string GetLabelForHideObscure() => " Hide unique tag types (e.g. addresses, websites)";
    }
}
