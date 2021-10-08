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

        /// <summary>Takes a list of key:values (as a single string) and then sets the corresponding Form rows as selected if they are present</summary>
        public static TreeGridItemCollection SetSelectionsFromStoredState(TreeGridItemCollection selectableData, string selectedKeyValues)
        {
            var csvItems = selectedKeyValues.Split(',').ToList();

            foreach (OSMTreeGridItem item in selectableData)
            {
                var itemKeyVal = item.Values[keyValueIndex].ToString();
                if (csvItems.Contains(itemKeyVal))
                {
                    item.Values[1] = "True";
                    foreach (OSMTreeGridItem childItem in item.Children)
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

        /// <summary>Given a pre-existing tree grid collection, show/hide items based on if obscure features should be hidden </summary>
        public static TreeGridItemCollection FilterByObscurity(TreeGridItemCollection selectableData, 
            bool hideObscureFeatures, TreeGridItemCollection currentSelectableData = null)
        {
            // Need to clone the items and make a new list to preserve the original Collection as unfiltered
            var newSelectableData = new TreeGridItemCollection();

            for (var i = 0; i < selectableData.Count; i++)
            {
                // Try to preserve open/close and selected/unselected state during filtering
                var previousItem = selectableData[i] as OSMTreeGridItem;
                if (currentSelectableData != null)
                {
                    var currentItem = currentSelectableData.Where(t => t.ToString() == previousItem.ToString()).First();
                    if (currentItem != null)
                        previousItem = currentItem  as OSMTreeGridItem;
                }

                var currentTagExpanded = previousItem.Expanded;
                var currentTagSelected = previousItem.IsSelected();

                var originalTag = selectableData[i] as OSMTreeGridItem;
                if (originalTag.IsObscure && hideObscureFeatures)
                    continue;

                var newTag = new OSMTreeGridItem(originalTag.OSMData, originalTag.OSMData.NodeCount, 
                    originalTag.OSMData.WayCount, originalTag.IsParsed, currentTagSelected, currentTagExpanded);

                foreach (OSMTreeGridItem originalChild in originalTag.Children)
                    if (!originalChild.IsObscure || !hideObscureFeatures)
                        newTag.Children.Add(originalChild);
                
                newSelectableData.Add(newTag);
            }

            return newSelectableData;
        }

        /// <summary>Given a (parent) feature's form item, check it and its children to see if they were selected (in the form)</summary>
        public static void GetKeyValueTextIfSelected(TreeGridItem item, ref List<string> keyvalues)
        {
            var childSelections = new List<string>();
            for (var j = 0; j < item.Children.Count; j++)
            {
                var childItem = item.Children[j] as OSMTreeGridItem;
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
