namespace Caribou.Forms
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Caribou.Models;
    using Eto.Forms;

    /// <summary>Translates from the Feature/SubFeature datasets into the UI elements in the view.</summary>
    public static class SelectionCollection
    {
        private static readonly int keyValueIndex = 4;

        public static TreeGridItemCollection GetCollection(bool includeObscure)
        {
            var itemsForCollection = new List<TreeGridItem>();

            foreach (var item in OSMDefinedFeatures.GetDefinedFeaturesForForm())
            {
                var treeItem = GetItem(item.Key, item.Value, includeObscure);
                itemsForCollection.Add(treeItem);
            }

            var treeCollection = new TreeGridItemCollection(itemsForCollection);
            return treeCollection;
        }

        private static TreeGridItem GetItem(OSMSelectableFeature parentFeature, List<OSMSelectableFeature> childFeatures, bool hideObscureFeatures)
        {
            var parentItem = new TreeGridItem
            {
                Values = parentFeature.GetColumnData()
            };

            foreach (var child in childFeatures)
            {
                if (child.ShowCounts && hideObscureFeatures && child.IsObscure()) { 
                    continue; 
                }

                var childItem = new TreeGridItem
                {
                    Values = child.GetColumnData()
                };
                parentItem.Children.Add(childItem);
            }

            return parentItem;
        }

        public static void GetKeyValueTextIfSelected(TreeGridItem item, ref List<string> keyvalues)
        {
            var childSelections = new List<string>();
            for (var j = 0; j < item.Children.Count; j++)
            {
                var childItem = item.Children[j] as TreeGridItem;
                var childIsChecked = childItem.Values[1].ToString() == "True";
                if (childIsChecked)
                {
                    childSelections.Add(childItem.Values[keyValueIndex].ToString());
                }
            }

            var itemIsChecked = item.Values[1].ToString() == "True";
            if (itemIsChecked && childSelections.Count == item.Children.Count)
            {
                // If all children are selected; just add the top-level x=* KeyValue
                keyvalues.Add(item.Values[keyValueIndex].ToString());
            }
            else
            {
                keyvalues.AddRange(childSelections); // If some children are selected; just add them
            }

            return;
        }

        public static TreeGridItemCollection DeserialiseKeyValues(string csvSelection, bool includeObscure)
        {
            // Need to set selection state back from list of keyValue strings that persisted
            var newSelectionState = SelectionCollection.GetCollection(includeObscure);
            var csvItems = csvSelection.Split(',').ToList();

            foreach (TreeGridItem item in newSelectionState)
            {
                var itemKeyVal = item.Values[keyValueIndex].ToString();
                if (csvItems.Contains(itemKeyVal))
                {
                    item.Values[1] = "True";
                    foreach (TreeGridItem childItem in item.Children)
                    {
                        childItem.Values[1] = "True";
                    }
                }
                else
                {
                    foreach (TreeGridItem childItem in item.Children)
                    {
                        var childItemKeyVal = childItem.Values[keyValueIndex].ToString();
                        if (csvItems.Contains(childItemKeyVal))
                        {
                            childItem.Values[1] = "True";
                        }
                    }
                }
            }

            return newSelectionState;
        }
    }
}
