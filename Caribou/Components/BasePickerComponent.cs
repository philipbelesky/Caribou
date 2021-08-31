namespace Caribou.Components
{
    using System;
    using System.Collections.Generic;
    using Caribou.Forms;
    using Eto.Forms;
    using Grasshopper.Kernel;

    /// <summary>
    /// For components providing buttons and messages below the component as well as serialised state
    /// </summary>
    public abstract class BasePickerComponent : CaribouComponent
    {
        #region Class Variables
        protected BaseCaribouForm componentForm;
        protected SelectableDataCollection selectableData; // Items for form to render
        protected TreeGridItemCollection selectionState; // Current state provided to/from the form 
        protected string storedState; // The component's state, as saved into the GHX and available to deserialise

        protected List<string> selectionStateSerialized = new List<string>(); // For outputing to definition and below component
        protected readonly string storageKeyForSelectionState = "selectionSerialised";
        protected readonly string storageKeyForHideObscure = "selectionHidesObscure"; // Includes obscure or filter by union
        protected bool hideObscureFeatures = true;

        protected BasePickerComponent(string name, string nickname, string description, string subCategory)
            : base(name, nickname, description, subCategory) {
        }
        #endregion

        // Required button methods
        protected abstract string GetButtonTitle(); // Return title for button
        public override void CreateAttributes() // Setup button in GH UI
        {
            m_attributes = new CustomSetButton(this, this.GetButtonTitle(), this.ButtonOpenAction);
        }

        // Required form methods
        protected abstract BaseCaribouForm GetFormForComponent(); // Provide component-specific form type

        #region Form Interaction
        // Form-button interaction; passed to CustomSetButton as handler action
        // Virtual so that the FilterResults form can refuse to open the form when it has no state
        protected virtual void ButtonOpenAction() => OpenForm();

        protected void OpenForm()
        {
            this.componentForm = this.GetFormForComponent(); // Need to remake whenever form is opened
            int x = (int)Mouse.Position.X - 5;
            int y = (int)Mouse.Position.Y - 250;
            this.componentForm.Location = new Eto.Drawing.Point(x, y);
            this.componentForm.Closed += (sender, e) => { StartFormClose(); };
            this.componentForm.Show();
        }

        protected void StartFormClose() // Handler for form closure with option for custom state setting
        {
            this.selectionState = this.componentForm.mainRow.data;
            this.hideObscureFeatures = this.componentForm.shouldHideObscureItems;
            FinishFormClose();
        }

        protected void FinishFormClose()
        {
            this.selectionStateSerialized = GetSelectedKeyValuesFromForm();
            this.ExpireSolution(true); // Recalculate output
        }
        #endregion

        protected abstract string GetNoSelectionMessage();

        #region Below-Component Messaging 
        protected void OutputMessageBelowComponent()
        {
            if (this.selectionStateSerialized.Count == 0)
            {
                this.Message = $"\u00A0{this.GetNoSelectionMessage()}\u00A0\u00A0"; // Spacing to ensure in black bit
            }
            else
            {
                var spacedKeyValues = string.Join(",", this.selectionStateSerialized.ToArray());
                var message = LineSpaceKeyValues(spacedKeyValues, this.selectionStateSerialized.Count);
                this.Message = "\u00A0" + message + "\u00A0";
            }
        }
        #endregion

        #region State Persistence
        // To persist the selection state variables we need to override Write to record state in the definition
        public override bool Write(GH_IO.Serialization.GH_IWriter writer)
        {
            var csvSelection = string.Join(",", this.selectionStateSerialized.ToArray());
            writer.SetString(storageKeyForSelectionState, csvSelection);
            writer.SetBoolean(storageKeyForHideObscure, this.hideObscureFeatures);
            return base.Write(writer);
        }

        // To persist selection state variables we need to override Read to check for state in the definiton
        public override bool Read(GH_IO.Serialization.GH_IReader reader)
        {
            if (reader.ItemExists(this.storageKeyForHideObscure)) // Load form UI state (e.g. hide obscure)
                this.hideObscureFeatures = reader.GetBoolean(storageKeyForHideObscure);

            // Load selected items stored as key=value lists
            if (reader.ItemExists(storageKeyForSelectionState))
            {
                var stateKeyValues = reader.GetString(storageKeyForSelectionState);
                if (!string.IsNullOrEmpty(stateKeyValues))
                    this.storedState = reader.GetString(storageKeyForSelectionState); 
            }
            return base.Read(reader);
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

        #endregion

        protected List<string> GetSelectedKeyValuesFromForm()
        {
            var selectedKVs = new List<string>();
            for (var i = 0; i < this.selectionState.Count; i++)
            {
                var item = this.selectionState[i] as TreeGridItem;
                TreeGridUtilities.GetKeyValueTextIfSelected(item, ref selectedKVs);
            }

            return selectedKVs;
        }

    }
}
