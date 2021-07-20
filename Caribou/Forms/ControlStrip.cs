namespace Caribou.Forms
{
    using System;
    using Eto.Forms;

    /// <summary>The bottom row of the window containing the button types.</summary>
    public static class ControlStrip
    {
        public static CheckBox GetHider(Action toggleAction, bool hideObscureFeaturesState)
        {
            var showHideMinorFeatures = new CheckBox()
            {
                Checked = hideObscureFeaturesState,
            };
            showHideMinorFeatures.CheckedChanged += (sender, e) => { toggleAction(); };
            return showHideMinorFeatures;
        }

        /// <summary>Checkbox labels are misaligned on macOS if they have a provided Text.</summary>
        public static Label GetHiderLabel(Action toggleAction)
        {
            var showHideMinorFeaturesLabel = new Label()
            {
                Text = " Hide SubFeatures with very low counts",
            };
            showHideMinorFeaturesLabel.MouseDown += (sender, e) => { toggleAction(); };
            return showHideMinorFeaturesLabel;
        }

        public static Button GetSelectAll(int buttonWidth, int buttonHeight, Action selectAll)
        {
            var allButton = new Button()
            {
                Text = "🔘 Select All",
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
                Text = "⚪ Select None",
                Width = buttonWidth,
                Height = buttonHeight,
            };
            noneButton.Click += (sender, e) => { selectNone(); };
            return noneButton;
        }
        public static Button GetExpandAll(int buttonWidth, int buttonHeight, Action expandAll)
        {
            var allButton = new Button()
            {
                Text = "⬇️ Expand All",
                Width = buttonWidth,
                Height = buttonHeight,
            };
            allButton.Click += (sender, e) => { expandAll(); };
            return allButton;
        }

        public static Button GetCollapseAll(int buttonWidth, int buttonHeight, Action collapseAll)
        {
            var collapseButton = new Button()
            {
                Text = "⬆️ Collapse All",
                Width = buttonWidth,
                Height = buttonHeight,
            };
            collapseButton.Click += (sender, e) => { collapseAll(); };
            return collapseButton;
        }

    }
}
