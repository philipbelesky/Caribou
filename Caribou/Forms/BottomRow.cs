namespace Caribou.Forms
{
    using System;
    using Eto.Forms;

    /// <summary>The bottom row of the window containing the button types.</summary>
    public static class BottomRow
    {
        public static CheckBox GetHider(int buttonHeight)
        {
            var showHideMinor = new CheckBox()
            {
                Text = "Hide SubFeatures with very low counts",
                Checked = true,
                Height = buttonHeight,
            };
            return showHideMinor;
        }
        public static Button GetUpdate(int buttonWidth, int buttonHeight)
        {
            var updateButton = new Button()
            {
                Text = "✅ Update Selection",
                Width = buttonWidth,
                Height = buttonHeight,
            };
            updateButton.Click += (sender, e) => { UpdateAndClose(); };
            return updateButton;
        }

        public static Button GetCancel(int buttonWidth, int buttonHeight)
        {
            var cancelButton = new Button()
            {
                Text = "❌ Cancel Update",
                Width = buttonWidth,
                Height = buttonHeight,
            };
            cancelButton.Click += (sender, e) => { CancelAndClose(); };
            return cancelButton;
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
