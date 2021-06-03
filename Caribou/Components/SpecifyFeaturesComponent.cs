namespace Caribou.Components
{
    using System;
    using System.Linq;
    using System.Windows.Forms;
    using Caribou.Properties;
    using Eto.Forms;
    using GH_IO.Serialization;
    using Rhino.UI;
    using Grasshopper.GUI;
    using Grasshopper.GUI.Canvas;
    using Grasshopper.Kernel;
    using Grasshopper.Kernel.Attributes;
    using Caribou.Forms;
    using System.Collections.Generic;

    /// <summary>Provides a GUI interface to selecting/specifying predefined OSM features/subfeatures.</summary>
    public class SpecifyFeaturesComponent : CaribouComponent
    {
        private SpecifyFeaturesForm pickerForm;
        private List<string> selectionOutput = new List<string>();
        private TreeGridItemCollection selectionState = SelectionCollection.GetCollection(false);

        public SpecifyFeaturesComponent() : base("Specify Features", "OSM Specify",
            "Provides a graphical interface (via double-click or right-click menu) to specify a list of OSM features.", "OSM")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager) { }

        protected override void CaribouRegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Features", "F", "A list of OSM features and subfeatures", GH_ParamAccess.list);
        }

        protected override void CaribouSolveInstance(IGH_DataAccess da)
        {
            if (this.selectionOutput.Count == 0)
            {
                this.Message = "\u00A0No Features Selected\u00A0\u00A0"; // Spacing to ensure in black bit
            }
            else
            {
                var spacedKeyValues = string.Join(",", this.selectionOutput.ToArray());
                var message = LineSpaceKeyValues(spacedKeyValues, this.selectionOutput.Count);
                this.Message = "\u00A0" + message + "\u00A0";
            }

            da.SetDataList(0, selectionOutput); // Update downstream text
        }

        public void OpenFeaturePicker()
        {
            this.pickerForm = new SpecifyFeaturesForm(this.selectionState);

            int x = (int)Mouse.Position.X - 5;
            int y = (int)Mouse.Position.Y - 40;
            this.pickerForm.Location = new Eto.Drawing.Point(x, y);
            this.pickerForm.Closed += (sender, e) => { HandlePickerClose(); };
            this.pickerForm.Show();
        }

        private void HandlePickerClose()
        {
            this.selectionState = this.pickerForm.mainRow.data;
            this.selectionOutput = GetSelectedKeyValuesFromForm();
            this.ExpireSolution(true); // Recalculate output
        }

        private List<string> GetSelectedKeyValuesFromForm()
        {
            var selectedKVs = new List<string>();
            for (var i = 0; i < this.selectionState.Count; i++)
            {
                var item = this.selectionState[i] as TreeGridItem;
                SelectionCollection.GetKeyValueTextIfSelected(item, ref selectedKVs);
            }

            return selectedKVs;
        }

        // To persist the selection variable we need to override Read and Write
        public override bool Write(GH_IO.Serialization.GH_IWriter writer)
        {
            var csvSelection = string.Join(",", this.selectionOutput.ToArray());
            writer.SetString("selectionSerialised", csvSelection);
            return base.Write(writer);
        }

        public override bool Read(GH_IO.Serialization.GH_IReader reader)
        {
            var csvSelection = reader.GetString("selectionSerialised");
            if (csvSelection != null)
            {
                this.selectionState = SelectionCollection.DeserialiseKeyValues(csvSelection);
                this.selectionOutput = GetSelectedKeyValuesFromForm();
            }
            return base.Read(reader);
        }

        // Affordances for right-click menu and double click shortcut
        public static string LineSpaceKeyValues(string message, int tagCount)
        {
            if (tagCount <= 3)
            {
                return message;
            }

            string[] individualKeyVals = message.Split(',');
            int characterCounter = 0;
            string lineSpacedKeyVals = "";

            for (int i = 0; i < individualKeyVals.Length; i++)
            {
                lineSpacedKeyVals += individualKeyVals[i] + ",";
                characterCounter += individualKeyVals[i].Length;

                if (characterCounter > 30) // Linelength
                {
                    lineSpacedKeyVals += "\u00A0\u00A0\n\u00A0";
                    characterCounter = 0;
                }
            }
            return lineSpacedKeyVals;
        }

        public override void CreateAttributes()
        {
            m_attributes = new CustomSetButton(this, this.OpenFeaturePicker); // Add custom attrributes so double-clicks open form
        }

        public override Guid ComponentGuid => new Guid("cc8d82ba-f381-46ee-8014-7e2d1bff824c");

        protected override System.Drawing.Bitmap Icon => Resources.icons_select;
    }
}
