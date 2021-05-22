namespace Caribou.Forms
{
    using System;
    using Eto.Forms;

    /// <summary>The bottom row of the window containing the button types.</summary>
    public static class BottomRow
    {
        public static StackLayout GetLayout(int windowWidth)
        {
            var updateButton = new Button()
            {
                Text = "Update Selection",
                Width = (windowWidth / 2) - 35,
                Height = 40,

            };
            //updateButton.Click += (sender, e) => { UpdateAndClose(); };

            var cancelButton = new Button()
            {
                Text = "Cancel Update",
                Width = (windowWidth / 2) - 35,
                Height = 40,
            };
            //cancelButton.Click += (sender, e) => { CancelAndClose(); };

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
    }
}
