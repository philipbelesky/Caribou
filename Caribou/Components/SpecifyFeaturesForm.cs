namespace Caribou.Components
{
    using System;
    using Rhino.UI;
    using Eto.Drawing;
    using Eto.Forms;
    using System.Collections.Generic;

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
                    ContentSelection(),
                    ButtomRow(),
                }
            };
        }

        private TreeGridView ContentSelection()
        {
            var featureSelect = new TreeGridView()
            {
                
            };
            var column1 = new GridColumn()
            {
                HeaderText = "Feature",
                DataCell = new TextBoxCell(0),
                Width = 200,
            };
            featureSelect.Columns.Add(column1);

            var column2 = new GridColumn()
            {
                HeaderText = "SubFeature",
                DataCell = new TextBoxCell(1),
                Width = 200,
            };
            featureSelect.Columns.Add(column2);

            var column3 = new GridColumn()
            {
                HeaderText = "Include",
                DataCell = new CheckBoxCell(2),
                Width = 100,
            };
            featureSelect.Columns.Add(column3);

            var treeCollection = new TreeGridItemCollection();

            // add one tree grid item with child
            var parent = new TreeGridItem { Values = new string[] {
                    "Parent", "Parent prop 1"
                },
                Expanded = true
            };
            var child = new TreeGridItem { Values = new string[] {
                    "Child", "Child prop 1"
                }
            };
            parent.Children.Add(child);

            featureSelect.DataStore = new TreeGridItemCollection { parent };

            return featureSelect;
        }

        private StackLayout ButtomRow()
        {
            var updateButton = new Button()
            {
                Text = "Update Selection",
                Width = (windowWidth / 2) - 35,
                Height = 40,

            };
            updateButton.Click += (sender, e) => { UpdateAndClose(); };

            var cancelButton = new Button()
            {
                Text = "Cancel Update",
                Width = (windowWidth / 2) - 35,
                Height = 40,
            };
            cancelButton.Click += (sender, e) => { CancelAndClose(); };

            var layout = new StackLayout
            {
                Orientation = Orientation.Horizontal,
                Items = {
                    updateButton, cancelButton
                },
                Spacing = 25,
                Padding = 0,
            };
            return layout;
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
