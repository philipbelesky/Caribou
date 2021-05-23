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
        private int windowWidth = 700;
        private int windowHeight = 1000;
        private int buttonHeight = 40;
        private int buttonWidth = 200;
        private int padding = 10;

        public SpecifyFeaturesForm()
        {
            // sets the client (inner) size of the window for your content
            this.ClientSize = new Eto.Drawing.Size(windowWidth, windowHeight);
            this.Padding = padding;

            this.Title = "Select Features and Sub-Features";
            this.Resizable = true;

            var mainRow = new MainRow();

            var formLayout = new DynamicLayout()
            {
                Padding = 10,
                Spacing = new Size(10, 10),
            };

            formLayout.BeginVertical();
            formLayout.BeginHorizontal();
            formLayout.Add(BottomRow.GetHider(buttonHeight));
            formLayout.Add(null);
            formLayout.Add(BottomRow.GetUpdate(buttonWidth, buttonHeight));
            formLayout.Add(BottomRow.GetCancel(buttonWidth, buttonHeight));
            formLayout.EndHorizontal();
            formLayout.EndVertical();

            formLayout.BeginVertical();
            formLayout.BeginHorizontal();
            formLayout.Add(mainRow.viewForm);
            formLayout.EndHorizontal();
            formLayout.EndVertical();

            Content = formLayout;
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
        }

    }
}
