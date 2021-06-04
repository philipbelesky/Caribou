namespace Caribou.Forms
{
    using System;
    using Eto.Forms;

    /// <summary>The bottom row of the window containing the button types.</summary>
    public static class ControlStrip
    {
        public static CheckBox GetHider()
        {
            var showHideMinor = new CheckBox()
            {
                Checked = true,
            };
            return showHideMinor;
        }

        // On MacOS having the label inline with checkbox creates an alignment issue
        // TODO: set clicks on this to set the status of the checkbox
        public static Label GetCheckLabel()
        {
            var label = new Label()
            {
                Text = " Hide SubFeatures with very low counts",
            };
            return label;
        }

        public static Button GetSelectAll(int buttonWidth, int buttonHeight, Action selectAll)
        {
            var allButton = new Button()
            {
                Text = "Select All",
                Width = buttonWidth,
                Height = buttonHeight,
            };
            allButton.Click += (sender, e) => { selectAll(); };
            return allButton;
        }

        public static Button GetSelectNone(int buttonWidth, int buttonHeight, Action selectNone)
        {
            var noneButton = new Button()
            {
                Text = "Select None",
                Width = buttonWidth,
                Height = buttonHeight,
            };
            noneButton.Click += (sender, e) => { selectNone(); };
            return noneButton;
        }

    }
}
