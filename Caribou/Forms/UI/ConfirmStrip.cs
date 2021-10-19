namespace Caribou.Forms
{
    using System;
    using Eto.Forms;

    public static class ConfirmStrip
    {
        public static Button GetUpdate(int buttonWidth, int buttonHeight, Action updateAndClose)
        {
            var updateButton = new Button()
            {
                Text = "✅ Update Selection",
                Width = buttonWidth,
                Height = buttonHeight,
            };
            updateButton.Click += (sender, e) => { updateAndClose(); };
            return updateButton;
        }

        public static Button GetCancel(int buttonWidth, int buttonHeight, Action cancelAndClose)
        {
            var cancelButton = new Button()
            {
                Text = "❌ Cancel Update",
                Width = buttonWidth,
                Height = buttonHeight,
            };
            cancelButton.Click += (sender, e) => { cancelAndClose(); };
            return cancelButton;
        }
    }
}
