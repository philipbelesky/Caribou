namespace Caribou.Forms
{
    using System;
    using Eto.Forms;

    /// <summary>The bottom row of the window containing the button types.</summary>
    public static class BottomRow
    {
        public static StackLayout GetLayout(int windowWidth, int buttonHeight)
        {
            var buttonWidth = 200;
            var spacing = 24;
            var toggleWidth = windowWidth - (buttonWidth * 2) - spacing - 64;

            var showHideMinor = new CheckBox()
            {
                Text = "Hide SubFeatures with very low counts",
                Width = toggleWidth,
                Checked = true,
                Height = buttonHeight,
            };

            var updateButton = new Button()
            {
                Text = "✅ Update Selection",
                Width = buttonWidth,
                Height = buttonHeight,

            };
            updateButton.Click += (sender, e) => { UpdateAndClose(); };

            var cancelButton = new Button()
            {
                Text = "❌ Cancel Update",
                Width = buttonWidth,
                Height = buttonHeight,
            };
            cancelButton.Click += (sender, e) => { CancelAndClose(); };

            var layout = new StackLayout
            {
                Orientation = Orientation.Horizontal,
                Items = {
                    showHideMinor, updateButton, cancelButton
                },
                Spacing = spacing,
                Padding = 0,
            };
            return layout;
        }

        private static void UpdateAndClose()
        {
            Application.Instance.Quit();
        }

        private static void CancelAndClose()
        {
            Application.Instance.Quit();
        }
    }
}
