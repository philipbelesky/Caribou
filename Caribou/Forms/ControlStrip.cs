﻿namespace Caribou.Forms
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