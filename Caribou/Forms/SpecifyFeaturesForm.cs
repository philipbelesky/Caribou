namespace Caribou.Components
{
    using System;
    using Rhino.UI;
    using Eto.Drawing;
    using Eto.Forms;
    using System.Collections.Generic;
    using Caribou.Forms;

    public class SpecifyFeaturesForm : Form
    {
        private int windowWidth = 1100;
        private int windowHeight = 700;
        private int buttonHeight = 40;
        private int padding = 10;

        public SpecifyFeaturesForm()
        {
            // sets the client (inner) size of the window for your content
            this.ClientSize = new Eto.Drawing.Size(windowWidth, windowHeight);
            this.Padding = padding;

            this.Title = "Select Features and Sub-Features";
            this.Resizable = true;

            var tableHeight = windowHeight - buttonHeight - (padding * 2) - 56;
            var tableWidth = windowWidth - (padding * 2);
            var mainRow = new MainRow(tableWidth, tableHeight);

            Content = new StackLayout
            {
                Padding = 10,
                Items = {
                    "Double click to add a row",
                    mainRow.viewForm,
                    new StackLayout() {
                        Padding = 10,
                    }, // Just for padding
                    BottomRow.GetLayout(windowWidth, buttonHeight),
                }
            };
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
        }

    }
}
