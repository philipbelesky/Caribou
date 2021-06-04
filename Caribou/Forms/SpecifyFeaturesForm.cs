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

        public SpecifyFeaturesForm(TreeGridItemCollection selectionState)
        {
            this.providedState = selectionState;
            this.Padding = padding;
            this.Title = "Select Features and Sub-Features";
            this.Resizable = true;
            this.Topmost = true; // Put form atop Grasshopper (MacOS)
            this.Closed += (sender, e) => { HandleClose(); };
            this.mainRow = new TableStrip(selectionState);

            var topButtons = new DynamicLayout();
            topButtons.BeginHorizontal();
            topButtons.Add(ControlStrip.GetHider());
            topButtons.Add(ControlStrip.GetCheckLabel());
            topButtons.Add(null);
            topButtons.Add(ControlStrip.GetSelectAll(buttonWidth - 100, buttonHeight, SelectAll));
            topButtons.Add(new Label() { Width = 10 });
            topButtons.Add(ControlStrip.GetSelectNone(buttonWidth - 100, buttonHeight, SelectNone));
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

        private void HandleClose() // Can be triggered in window chrome
        {
        }

        private void SelectAll() // Just from the button
        {
            
        }

        private void SelectNone() // Just from the button
        {
            
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
