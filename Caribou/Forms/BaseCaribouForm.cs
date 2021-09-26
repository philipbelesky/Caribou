namespace Caribou.Forms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Eto.Drawing;
    using Eto.Forms;

    public abstract class BaseCaribouForm : Form
    {
        #region Class Variables
        protected readonly TreeGridItemCollection providedSelectionState; // Passed from component during form init
        public bool shouldHideObscureItems; // E.g. toggle obscure features flag

        public TableStrip mainRow;
        protected DynamicLayout topButtons;
        protected DynamicLayout bottomButtons;
        protected CheckBox obscureFeaturesCheckbox; // Need to track so we can manually set state

        protected readonly int windowWidth = 1000;
        protected readonly int windowHeight = 633; // Need to be large enough to show buttom row
        protected readonly int buttonHeight = 40;
        protected readonly int buttonWidth = 200;
        protected readonly int padding = 0;
        #endregion

        #region Form Setup
        public BaseCaribouForm(TreeGridItemCollection providedSelectableItems, string formTitle, bool hideObscure) 
        {
            this.providedSelectionState = providedSelectableItems;
            this.shouldHideObscureItems = hideObscure;

            this.Padding = padding;
            this.Title = formTitle;
            this.Resizable = true;
            this.Topmost = true; // Put form atop Grasshopper (MacOS)

            this.mainRow = new TableStrip(TreeGridUtilities.FilterByObscurity(
                this.providedSelectionState, this.shouldHideObscureItems));

            this.topButtons = new DynamicLayout();
            this.topButtons.BeginHorizontal();
            AddCustomButtonsToTop();
            this.topButtons.Add(null);
            this.topButtons.Add(ControlStrip.GetExpandAll(buttonWidth - 90, buttonHeight, ExpandAll));
            this.topButtons.Add(new Label() { Width = 10 });
            this.topButtons.Add(ControlStrip.GetCollapseAll(buttonWidth - 90, buttonHeight, CollapseAll));
            this.topButtons.Add(new Label() { Width = 10 });
            this.topButtons.Add(ControlStrip.GetSelectAll(buttonWidth - 90, buttonHeight, SelectAll));
            this.topButtons.Add(new Label() { Width = 10 });
            this.topButtons.Add(ControlStrip.GetSelectNone(buttonWidth - 90, buttonHeight, SelectNone));
            this.topButtons.EndHorizontal();

            this.bottomButtons = new DynamicLayout();
            this.bottomButtons.BeginHorizontal();
            this.bottomButtons.Add(null);
            this.bottomButtons.Add(ConfirmStrip.GetUpdate(buttonWidth, buttonHeight, UpdateAndClose));
            this.bottomButtons.Add(new Label() { Width = 10 });
            this.bottomButtons.Add(ConfirmStrip.GetCancel(buttonWidth, buttonHeight, CancelAndClose));
            this.bottomButtons.EndHorizontal();

            FinishTableLayout();
        }
        #endregion

        private void AddCustomButtonsToTop()
        {
            this.obscureFeaturesCheckbox = ControlStrip.GetHider(
                ToggleObscureFeatures, this.shouldHideObscureItems, GetLabelForHideObscure());
            this.topButtons.Add(obscureFeaturesCheckbox);
        }

        protected abstract string GetLabelForHideObscure(); // Provide a label for the obscure toggle

        protected void FinishTableLayout()
        {
            // Create rows
            var topRow = new TableRow { Cells = { this.topButtons } };
            var middleRow = new TableRow
            {
                ScaleHeight = true,
                Cells = {
                    new TableCell { ScaleWidth = true, Control = this.mainRow.viewForm }
                }
            };
            var bottomRow = new TableRow { Cells = { this.bottomButtons } };

            // Create overall layout
            Content = new TableLayout()
            {
                Padding = 10,
                Spacing = new Size(10, 10),
                Size = new Size(windowWidth, windowHeight),
                Rows = {
                    topRow,
                    middleRow,
                    bottomRow
                }
            };
        }

        #region Interaction Handlers
        protected void SelectAll() => this.SetSelection("True");
        protected void SelectNone() => this.SetSelection("False");
        protected void ExpandAll() => this.SetRollout(true);
        protected void CollapseAll() => this.SetRollout(false);

        private void SetSelection(string boolAsString)
        {
            foreach (CaribouTreeGridItem item in this.mainRow.viewForm.DataStore as TreeGridItemCollection)
            {
                item.SetValue(1, boolAsString);
                foreach (TreeGridItem childItem in item.Children)
                    childItem.SetValue(1, boolAsString);
            }
            this.mainRow.viewForm.ReloadData();
        }

        private void SetRollout(bool value)
        {
            foreach (TreeGridItem item in this.mainRow.viewForm.DataStore as TreeGridItemCollection)
                item.Expanded = value;

            this.mainRow.viewForm.ReloadData();
        }

        private void UpdateAndClose() // Just from the button
        {
            this.Close();
        }

        private void CancelAndClose() // Just from the button
        {
            this.Close();
        }

        protected void ToggleObscureFeatures()
        {
            this.shouldHideObscureItems = this.obscureFeaturesCheckbox.Checked.Value;
            var newState = TreeGridUtilities.FilterByObscurity(this.providedSelectionState, this.shouldHideObscureItems, 
                this.mainRow.GetCurrentData());

            this.mainRow.viewForm.DataStore = newState;
            this.mainRow.viewForm.ReloadData();
        }
        #endregion
    }
}
