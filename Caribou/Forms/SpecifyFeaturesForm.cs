﻿namespace Caribou.Components
{
    using System;
    using Rhino.UI;
    using Eto.Drawing;
    using Eto.Forms;
    using System.Collections.Generic;
    using Caribou.Forms;

    public class SpecifyFeaturesForm : Form
    {
        private int windowWidth = 800;

        public SpecifyFeaturesForm()
        {
            // sets the client (inner) size of the window for your content
            this.ClientSize = new Eto.Drawing.Size(windowWidth, 700);
            this.Padding = 10;

            this.Title = "Select Features and Sub-Features";
            this.Resizable = false;

            Content = new StackLayout
            {
                Padding = 10,
                Items = {
                    MainRow.GetLayout(windowWidth),
                    new StackLayout() {
                        Padding = 10,
                    }, // Just for padding
                    BottomRow.GetLayout(windowWidth),
                }
            };
        }

        private void UpdateAndClose()
        {
            Application.Instance.Quit();
        }

        private void CancelAndClose()
        {
            Application.Instance.Quit();
        }
    }
}
