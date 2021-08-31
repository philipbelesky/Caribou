namespace Caribou.Forms
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Caribou.Models;
    using Eto.Forms;

    /// <summary>Translates from the Feature/SubFeature datasets or Tag sets into the UI elements in the view.</summary>
    public abstract class TreeGridUtilities
    {
        private static readonly int keyValueIndex = 4;

        #region Making TreeGridItem Collections
        /// <summary>Get the OSM items to be shown in the form given a provided list [of defined features or tags] /// </summary>
        public static TreeGridItemCollection MakeOSMCollectionWithoutState(SelectableDataCollection osmItems, bool hideObscure)
        {
            var itemsForCollection = new List<TreeGridItem>();
            var sortedItems = osmItems.tagHierarchy.OrderBy(x => x.Key.Name).ToDictionary(x => x.Key, x => x.Value);

            foreach (var item in sortedItems)
            {
                if (hideObscure && (Array.IndexOf(OSMUniqueTags.names, item.Key.TagType) >= 0))
                    continue; // Filter out obscure parent-types by matching against a static list

                var treeItem = GetItem(item.Key, item.Value, hideObscure);
                itemsForCollection.Add(treeItem);
            }

            return new TreeGridItemCollection(itemsForCollection);
        }

        public static TreeGridItemCollection MakeOSMCollectionFromStoredState(
            SelectableDataCollection selectableData, string selectedKeyValues, bool hideObscure)
        {
            // Need to set selection state back from list of keyValue strings that persisted
            var newSelectionState = TreeGridUtilities.MakeOSMCollectionWithoutState(selectableData, hideObscure);
            var csvItems = selectedKeyValues.Split(',').ToList();

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
        #endregion

        /// <summary>Given a pre-existing tree grid collection, show/hide items based on if features should be hidden </summary>
        public static TreeGridItemCollection FilterOSMCollection(
            TreeGridItemCollection providedSelectionState, bool hideObscureFeatures)
        {
            foreach (var item in providedSelectionState)
            {
                // TODO toggle based on obscurity
            }
            return providedSelectionState;
        }

        #region Translating to/from TreeGridItems
        private static TreeGridItem GetItem(
            OSMSelectableData parentFeature, List<OSMSelectableData> childFeatures, bool hideObscureFeatures)
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
        #endregion
    }
}
