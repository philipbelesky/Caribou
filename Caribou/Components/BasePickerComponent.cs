namespace Caribou.Components
{
    using System;
    using System.Collections.Generic;
    using Caribou.Forms;
    using Eto.Forms;

    /// <summary>
    /// For components providing buttons and messages below the component as well as serialised state
    /// </summary>
    public abstract class BasePickerComponent : CaribouComponent
    {
        protected BaseCaribouForm componentForm;
        protected TreeGridItemCollection selectionState;
        protected List<string> selectionStateSerialized = new List<string>(); // For outputing to definition and below component
        protected readonly string storageKeyForSelectionState = "selectionSerialised";
        protected readonly string storageKeyForCustomFlag = "selectionCustomFlag"; // Includes obscure or filter by union

        protected BasePickerComponent(string name, string nickname, string description, string subCategory)
            : base(name, nickname, description, subCategory) { }

        // Required button methods
        protected abstract string GetButtonTitle(); // Return title for button
        public override void CreateAttributes() // Setup button in GH UI
        {
            m_attributes = new CustomSetButton(this, this.GetButtonTitle(), this.ButtonOpenAction);
        }

        // Required form methods
        protected abstract BaseCaribouForm GetFormForComponent(); // Provide component-specific form type

        public void ButtonOpenAction() // Form-button interaction; passed to CustomSetButton as handler action
        {
            this.componentForm = this.GetFormForComponent(); // Need to remake whenever form is opened
            int x = (int)Mouse.Position.X - 5;
            int y = (int)Mouse.Position.Y - 250;
            this.componentForm.Location = new Eto.Drawing.Point(x, y);
            this.componentForm.Closed += (sender, e) => { StartFormClose(); };
            this.componentForm.Show();
        }

        protected abstract void CustomFormClose(); // Handler for component-specific actions during form closing 

        protected void StartFormClose() // Handler for form closure with option for custom state setting
        {
            this.selectionState = this.componentForm.mainRow.data;
            CustomFormClose(); // Tracking custom state
            FinishFormClose();
        }

        protected void FinishFormClose()
        {
            this.selectionStateSerialized = GetSelectedKeyValuesFromForm();
            this.ExpireSolution(true); // Recalculate output
        }

        // Messages below buttons
        protected void OutputMessageBelowComponent()
        {
            if (this.selectionStateSerialized.Count == 0)
            {
                this.Message = "\u00A0No Features Selected\u00A0\u00A0"; // Spacing to ensure in black bit
            }
            else
            {
                var spacedKeyValues = string.Join(",", this.selectionStateSerialized.ToArray());
                var message = LineSpaceKeyValues(spacedKeyValues, this.selectionStateSerialized.Count);
                this.Message = "\u00A0" + message + "\u00A0";
            }
        }

        // To persist the selection state variables we need to override Write to record state in the definition
        public override bool Write(GH_IO.Serialization.GH_IWriter writer)
        {
            var csvSelection = string.Join(",", this.selectionStateSerialized.ToArray());
            writer.SetString(storageKeyForSelectionState, csvSelection);
            writer.SetBoolean(storageKeyForCustomFlag, GetCustomFlagToSerialize());
            return base.Write(writer);
        }

        // Get the customKeyProperty value from a component-specific state flag
        protected abstract bool GetCustomFlagToSerialize();

        // To persist selection state variables we need to override Read to check for state in the definiton
        public override bool Read(GH_IO.Serialization.GH_IReader reader)
        {
            if (reader.ItemExists(this.storageKeyForCustomFlag))
                SetCustomFlagFromDeserialize(reader.GetBoolean(storageKeyForCustomFlag));

            if (reader.ItemExists(storageKeyForSelectionState))
            {
                var csvSelection = reader.GetString(storageKeyForSelectionState);
                this.selectionState = SelectionCollection.DeserialiseKeyValues(csvSelection, GetCustomFlagToSerialize());
                this.selectionStateSerialized = GetSelectedKeyValuesFromForm();
            }
            return base.Read(reader);
        }

        // Set the component-specific state flag from the deserialisation of the customKeyProperty value
        protected abstract void SetCustomFlagFromDeserialize(bool valueToApply);

        protected List<string> GetSelectedKeyValuesFromForm()
        {
            var selectedKVs = new List<string>();
            for (var i = 0; i < this.selectionState.Count; i++)
            {
                var item = this.selectionState[i] as TreeGridItem;
                SelectionCollection.GetKeyValueTextIfSelected(item, ref selectedKVs);
            }

            return selectedKVs;
        }

        // Affordance for assembling the below-component list of values
        protected static string LineSpaceKeyValues(string message, int tagCount)
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
    }
}
