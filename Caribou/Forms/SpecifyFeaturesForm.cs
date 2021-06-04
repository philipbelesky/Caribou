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
        private int windowHeight = 1033; // Need to be large enough to show buttom row
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

            var formLayout = new DynamicLayout()
            {
                Padding = 10,
                Spacing = new Size(10, 10),
                Size = new Size(windowWidth, windowHeight),
            };

            var update = ConfirmStrip.GetUpdate(buttonWidth, buttonHeight, UpdateAndClose);
            var cancel = ConfirmStrip.GetCancel(buttonWidth, buttonHeight, CancelAndClose);
            var selectAll = ControlStrip.GetSelectAll(buttonWidth - 100, buttonHeight, SelectAll);
            var selectNone = ControlStrip.GetSelectNone(buttonWidth - 100, buttonHeight, SelectNone);

            formLayout.BeginVertical();
            formLayout.BeginHorizontal();

            formLayout.Add(ControlStrip.GetHider());
            formLayout.Add(ControlStrip.GetCheckLabel());
            formLayout.Add(null, true);
            formLayout.AddAutoSized(selectAll);
            formLayout.Add(new Label() { Width = 10 }); // Spacer
            formLayout.AddAutoSized(selectNone);

            formLayout.EndHorizontal();
            formLayout.EndVertical();
            formLayout.BeginVertical();
            formLayout.BeginHorizontal();

            formLayout.Add(this.mainRow.viewForm);

            formLayout.EndHorizontal();
            formLayout.EndVertical();
            formLayout.BeginVertical();
            formLayout.BeginHorizontal();

            formLayout.Add(null, true);
            formLayout.AddAutoSized(update);
            formLayout.Add(new Label() { Width = 10 }); // Spacer
            formLayout.AddAutoSized(cancel);

            formLayout.EndHorizontal();
            formLayout.EndVertical();


            Content = formLayout;
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
