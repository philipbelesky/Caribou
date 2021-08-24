namespace Caribou.Forms
{
    using System;
    using System.Collections.Generic;
    using Caribou.Forms;
    using Eto.Drawing;
    using Eto.Forms;

    public class FilterFeaturesForm : BaseCaribouForm
    {
        public FilterFeaturesForm(TreeGridItemCollection selectionState, bool resultsMustHaveAllTags)
            : base(selectionState, "Select Tags to Filter For", resultsMustHaveAllTags)
        { }

        protected override void AddCustomButtonsToTop()
        {
            return;
        }
    }
}
