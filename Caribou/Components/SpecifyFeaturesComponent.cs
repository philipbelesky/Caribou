namespace Caribou.Components
{
    using System;
    using System.Windows.Forms;
    using Caribou.Properties;
    using Eto.Forms;
    using GH_IO.Serialization;
    using Grasshopper.GUI;
    using Grasshopper.GUI.Canvas;
    using Grasshopper.Kernel;
    using Grasshopper.Kernel.Attributes;

    /// <summary>Provides a GUI interface to selecting/specifying predefined OSM features/subfeatures.</summary>
    public class SpecifyFeaturesComponent : CaribouComponent
    {
        private string textOfKeyValueSelected = "TODO key:value";
        private SpecifyFeaturesForm pickerForm;

        public SpecifyFeaturesComponent() : base("Specify Features/SubFeatures", "Specify Features",
            "Provides a graphical interface (via double-click or right-click menu) to specify a list of OSM features.", "OSM") 
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager) { }

        protected override void CaribouRegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("OSM Features", "F", "A list of OSM features and subfeatures", GH_ParamAccess.list);
        }

        protected override void CaribouSolveInstance(IGH_DataAccess da)
        {
        }

        public override void CreateAttributes()
        {
            m_attributes = new MySpecialComponentAttributes(this); // Add custom attrributes so double-clicks open form
        }

        private class MySpecialComponentAttributes : GH_ComponentAttributes
        {
            public MySpecialComponentAttributes(IGH_Component component)
              : base(component) { }

            public override GH_ObjectResponse RespondToMouseDoubleClick(GH_Canvas sender, GH_CanvasMouseEvent e)
            {
                (Owner as SpecifyFeaturesComponent)?.OpenFeaturePicker();
                return GH_ObjectResponse.Handled;
            }
        }

        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            Menu_AppendItem(menu, "Select Features", this.OpenFeaturePickerFromMenu, null, true, false);
            Menu_AppendSeparator(menu);
        }

        private void OpenFeaturePickerFromMenu(object sender, EventArgs e)
        {
            OpenFeaturePicker();
        }

        private void OpenFeaturePicker()
        {
            this.pickerForm = new SpecifyFeaturesForm();
            var mousePos = Mouse.Position;
            int x = (int)mousePos.X + 20;
            int y = (int)mousePos.Y - 160;
            this.pickerForm.Location = new Eto.Drawing.Point(x, y);

            this.pickerForm.Show();
        }

        public override bool Read(GH_IReader reader) // Add message below component
        {
            this.Message = textOfKeyValueSelected;
            return base.Read(reader);
        }

        public override void AddedToDocument(GH_Document document) // Add message below component
        {
            this.Message = textOfKeyValueSelected;
            base.AddedToDocument(document);
        }

        public override Guid ComponentGuid => new Guid("cc8d82ba-f381-46ee-8014-7e2d1bff824c");

        protected override System.Drawing.Bitmap Icon => Resources.icons_select;
    }
}
