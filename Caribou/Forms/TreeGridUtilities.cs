namespace Caribou.Forms
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Caribou.Models;
    using Eto.Forms;
    using Caribou.Forms.Models;

    /// <summary>Translates from the Feature/SubFeature datasets or Tag sets into the UI elements in the view.</summary>
    public static class TreeGridUtilities
    {
        private static readonly int keyValueIndex = 4;

        public static TreeGridItemCollection SetSelectionsFromStoredState(TreeGridItemCollection selectableData, string selectedKeyValues)
        {
            var csvItems = selectedKeyValues.Split(',').ToList();

            foreach (CaribouTreeGridItem item in selectableData)
            {
                var itemKeyVal = item.Values[keyValueIndex].ToString();
                if (csvItems.Contains(itemKeyVal))
                {
                    item.Values[1] = "True";
                    foreach (CaribouTreeGridItem childItem in item.Children)
                        childItem.Values[1] = "True";
                }
                else
                {
                    foreach (TreeGridItem childItem in item.Children)
                    {
                        var childItemKeyVal = childItem.Values[keyValueIndex].ToString();
                        if (csvItems.Contains(childItemKeyVal))
                            childItem.Values[1] = "True";
                    }
                }
            }

            return selectableData;
        }

        /// <summary>Given a pre-existing tree grid collection, show/hide items based on if features should be hidden </summary>
        public static TreeGridItemCollection FilterByObscurity(TreeGridItemCollection selectableData, bool hideObscureFeatures)
        {
            if (!hideObscureFeatures)
                return selectableData;
            
            for (int i = selectableData.Count - 1; i >= 0; i--)
                  if ((selectableData[i] as CaribouTreeGridItem).IsObscure)
                    selectableData.RemoveAt(i);

            return selectableData;
        }

        public static void GetKeyValueTextIfSelected(TreeGridItem item, ref List<string> keyvalues)
        {
            var childSelections = new List<string>();
            for (var j = 0; j < item.Children.Count; j++)
            {
                var childItem = item.Children[j] as CaribouTreeGridItem;
                var childIsChecked = childItem.Values[1].ToString() == "True";
                if (childIsChecked)
                    childSelections.Add(childItem.Values[keyValueIndex].ToString());
            }

            var itemIsChecked = item.Values[1].ToString() == "True";
            if (itemIsChecked && childSelections.Count == item.Children.Count)
                keyvalues.Add(item.Values[keyValueIndex].ToString()); // If all children are selected; just add the top-level x=* KeyValue
            else
                keyvalues.AddRange(childSelections); // If some children are selected; just add them

            return;
        }
    }
}
