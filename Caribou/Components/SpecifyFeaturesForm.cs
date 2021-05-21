namespace Caribou.Components
{
    using System;
    using Rhino.UI;
    using Eto.Drawing;
    using Eto.Forms;

    public class SpecifyFeaturesForm : Form
    {
        public SpecifyFeaturesForm()
        {
            // sets the client (inner) size of the window for your content
            this.ClientSize = new Eto.Drawing.Size(600, 800);
            this.Padding = 10;

            this.Title = "Select Features and Sub-Features";
            this.Resizable = false;
        }
    }
}
