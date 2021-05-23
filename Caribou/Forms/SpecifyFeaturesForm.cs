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

        public SpecifyFeaturesForm()
        {
            // sets the client (inner) size of the window for your content
            this.ClientSize = new Eto.Drawing.Size(windowWidth, windowHeight);
            this.Padding = 10;

            this.Title = "Select Features and Sub-Features";
            this.Resizable = false;

            var mainRow = new MainRow(windowWidth);

            Content = new StackLayout
            {
                Padding = 10,
                Items = {
                    mainRow.viewForm,
                    new StackLayout() {
                        Padding = 10,
                    }, // Just for padding
                    BottomRow.GetLayout(windowWidth),
                }
            };
        }

    }
}
