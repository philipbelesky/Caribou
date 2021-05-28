namespace Caribou.Forms
{
    using System;
    using System.Collections.Generic;
    using Caribou.Data;
    using Caribou.Models;
    using Eto.Forms;

    /// <summary>Translates from the Feature/SubFeature datasets into the UI elements in the view.</summary>
    public static class SelectionCollection
    {
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

        private static TreeGridItem GetItem(OSMSelectableFeature parentFeature, List<OSMSelectableFeature> childFeatures, bool includeObscure)
        {
            var parentItem = new TreeGridItem
            {
                Values = parentFeature.GetColumnData()
            };

            foreach (var child in childFeatures)
            {
                if (!includeObscure && child.IsObscure() && child.ShowCounts)
                {
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
    }
}
