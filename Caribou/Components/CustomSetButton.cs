namespace Caribou.Components
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Eto.Forms;
    using Grasshopper.GUI;
    using Grasshopper.GUI.Canvas;
    using Grasshopper.Kernel;

    public class CustomSetButton : Grasshopper.Kernel.Attributes.GH_ComponentAttributes
    {
        // Adapted from https://www.grasshopper3d.com/forum/topics/create-radio-button-on-grasshopper-component?commentId=2985220%3AComment%3A835552

        private Action buttonClickHandler;
        private int buttonHeight = 45;
        private int buttonWidthIncrease = 5;
        
        public CustomSetButton(SpecifyFeaturesComponent owner, Action openFormCallback) : base(owner) {
            this.buttonClickHandler = openFormCallback;
        }

        protected override void Layout()
        {
            base.Layout();

            System.Drawing.Rectangle componentRect = GH_Convert.ToRectangle(Bounds);
            componentRect.Height += buttonHeight;

            System.Drawing.Rectangle buttonRect = componentRect;
            buttonRect.Y = buttonRect.Bottom - buttonHeight;
            buttonRect.Height = buttonHeight;
            buttonRect.Inflate(-3, -3); // Shrink button bounds for cleaner insert

            Bounds = componentRect;
            ButtonBounds = buttonRect;
        }
        private System.Drawing.Rectangle ButtonBounds { get; set; }

        protected override void Render(GH_Canvas canvas, System.Drawing.Graphics graphics, GH_CanvasChannel channel)
        {
            base.Render(canvas, graphics, channel);

            if (channel == GH_CanvasChannel.Objects)
            {
                GH_Capsule button = GH_Capsule.CreateTextCapsule(
                    ButtonBounds, ButtonBounds, GH_Palette.Black, "Specify\nFeatures", 2, 0);
                button.Render(graphics, Selected, Owner.Locked, false);
                button.Dispose();
            }
        }
        public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                System.Drawing.RectangleF rec = ButtonBounds;
                if (rec.Contains(e.CanvasLocation))
                {
                    this.buttonClickHandler();
                    return GH_ObjectResponse.Handled;
                }
            }
            return base.RespondToMouseDown(sender, e);
        }
    }
}
