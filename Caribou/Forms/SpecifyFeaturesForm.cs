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
        private int windowWidth = 600;

        public SpecifyFeaturesForm()
        {
            // sets the client (inner) size of the window for your content
            this.ClientSize = new Eto.Drawing.Size(windowWidth, 800);
            this.Padding = 10;

            this.Title = "Select Features and Sub-Features";
            this.Resizable = false;

            Content = new StackLayout
            {
                Padding = 10,
                Items = {
                    "Hello World!",
                    MainRow.GetLayout(windowWidth, testData),
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

        private Dictionary<string, List<string>> testData = new Dictionary<string, List<string>>
        {
            { "aerialway", new List<string>() { "A", "B", "C", "D" } },
            { "aeroway", new List<string>() { "A", "B", "C", "D" } },
            { "amenity", new List<string>() { "A", "B", "C", "D" } },
            { "barrier", new List<string>() { "A", "B", "C", "D" } },
            { "boundary", new List<string>() { "A", "B", "C", "D" } },
            { "building", new List<string>() { "A", "B", "C", "D" } },
            { "craft", new List<string>() { "A", "B", "C", "D" } },
            { "emergency", new List<string>() { "A", "B", "C", "D" } },
            { "geological", new List<string>() { "A", "B", "C", "D" } },
            { "healthcare", new List<string>() { "A", "B", "C", "D" } },
            { "highway", new List<string>() { "A", "B", "C", "D" } },
            { "historic", new List<string>() { "A", "B", "C", "D" } },
            { "landuse", new List<string>() { "A", "B", "C", "D" } },
            { "leisure", new List<string>() { "A", "B", "C", "D" } },
            { "man_made", new List<string>() { "A", "B", "C", "D" } },
            { "military", new List<string>() { "A", "B", "C", "D" } },
            { "natural", new List<string>() { "A", "B", "C", "D" } },
            { "office", new List<string>() { "A", "B", "C", "D" } },
            { "place", new List<string>() { "A", "B", "C", "D" } }
        };
    }
}
