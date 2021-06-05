namespace Caribou.Components
{
    using System;
    using System.Collections.Generic;
    using Caribou.Forms;
    using Eto.Drawing;
    using Eto.Forms;

    public class SpecifyFeaturesForm : Form
    {
        public TableStrip mainRow;
        private int windowWidth = 1000;
        private int windowHeight = 633; // Need to be large enough to show buttom row
        private int buttonHeight = 40;
        private int buttonWidth = 200;
        private int padding = 10;
        private TreeGridItemCollection providedState;
        public bool hideObscureFeatures;

        public SpecifyFeaturesForm(TreeGridItemCollection selectionState, bool hideObscureFeatures)
        {
            this.providedState = selectionState;
            this.hideObscureFeatures = hideObscureFeatures;

            this.Padding = padding;
            this.Title = "Select Features and Sub-Features";
            this.Resizable = true;
            this.Topmost = true; // Put form atop Grasshopper (MacOS)
            this.mainRow = new TableStrip(selectionState);

            var topButtons = new DynamicLayout();
            topButtons.BeginHorizontal();
            topButtons.Add(ControlStrip.GetHider(ToggleObscureFeatures, this.hideObscureFeatures));
            topButtons.Add(null);
            topButtons.Add(ControlStrip.GetExpandAll(buttonWidth - 90, buttonHeight, ExpandAll));
            topButtons.Add(new Label() { Width = 10 });
            topButtons.Add(ControlStrip.GetCollapseAll(buttonWidth - 90, buttonHeight, CollapseAll));
            topButtons.Add(new Label() { Width = 10 });
            topButtons.Add(ControlStrip.GetSelectAll(buttonWidth - 90, buttonHeight, SelectAll));
            topButtons.Add(new Label() { Width = 10 });
            topButtons.Add(ControlStrip.GetSelectNone(buttonWidth - 90, buttonHeight, SelectNone));
            topButtons.EndHorizontal();
                        
            var bottomButtons = new DynamicLayout();
            bottomButtons.BeginHorizontal();
            bottomButtons.Add(null);
            bottomButtons.Add(ConfirmStrip.GetUpdate(buttonWidth, buttonHeight, UpdateAndClose));
            bottomButtons.Add(new Label() { Width = 10 });
            bottomButtons.Add(ConfirmStrip.GetCancel(buttonWidth, buttonHeight, CancelAndClose));
            bottomButtons.EndHorizontal();

            var topRow = new TableRow { Cells = { topButtons } };
            var middleRow = new TableRow 
            {
                ScaleHeight = true,
                Cells = {
                    new TableCell { ScaleWidth = true, Control = this.mainRow.viewForm }
                }
            };
            var bottomRow = new TableRow { Cells = { bottomButtons } };

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

        private void SelectAll() => this.SetSelection("True"); 

        private void SelectNone() => this.SetSelection("False");

        private void ExpandAll() => this.SetRollout(true);

        private void CollapseAll() => this.SetRollout(false);

        private void ToggleObscureFeatures()
        {
            this.hideObscureFeatures = !this.hideObscureFeatures;
            this.mainRow.viewForm.DataStore = SelectionCollection.GetCollection(this.hideObscureFeatures);
            this.mainRow.viewForm.ReloadData();
        }

        private void SetSelection(string boolAsString)
        {
            foreach (TreeGridItem item in this.mainRow.data)
            {
                item.SetValue(1, boolAsString);
                foreach (TreeGridItem childItem in item.Children)
                {
                    childItem.SetValue(1, boolAsString);
                }
            }
            this.mainRow.viewForm.ReloadData();
        }

        private void SetRollout(bool value)
        {
            foreach (TreeGridItem item in this.mainRow.data)
            {
                item.Expanded = value;
            }
            this.mainRow.viewForm.ReloadData();
        }

        private void UpdateAndClose() // Just from the button
        {
            this.Close();
        }

        private void CancelAndClose() // Just from the button
        {
            this.mainRow.data = this.providedState; // Revert to initially provided state
            this.Close();
        }
    }
}
