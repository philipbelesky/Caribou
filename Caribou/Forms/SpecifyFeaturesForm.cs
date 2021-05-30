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
        private int windowWidth = 1000;
        private int windowHeight = 920;
        private int buttonHeight = 40;
        private int buttonWidth = 200;
        private int padding = 10;

        public SpecifyFeaturesForm()
        {
            this.Padding = padding;
            this.Title = "Select Features and Sub-Features";
            this.Resizable = true;

            var mainRow = new TableStrip();

            var formLayout = new DynamicLayout()
            {
                Padding = 10,
                Spacing = new Size(10, 10),
                Size = new Size(windowWidth, windowHeight),
            };

            formLayout.BeginVertical();
            formLayout.BeginHorizontal();
            formLayout.Add(ControlStrip.GetHider());
            formLayout.Add(ControlStrip.GetCheckLabel());
            formLayout.Add(null, true);
            formLayout.AddAutoSized(ControlStrip.GetUpdate(buttonWidth, buttonHeight));
            formLayout.Add(new Label() { Width = 10 }); // Spacer
            formLayout.AddAutoSized(ControlStrip.GetCancel(buttonWidth, buttonHeight));
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
