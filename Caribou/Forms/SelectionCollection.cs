namespace Caribou.Forms
{
    using System;
    using System.Collections.Generic;
    using Caribou.Data;
    using Eto.Forms;

    /// <summary>Translates from the Feature/SubFeature datasets into the UI elements in the view.</summary>
    public static class SelectionCollection
    {
        public static TreeGridItemCollection GetCollection()
        {
            var itemsForCollection = new List<TreeGridItem>();

            foreach (var item in OSMDefinedFeatures.GetAll())
            {
                var treeItem = GetItem(item.Key, item.Value);
                itemsForCollection.Add(treeItem);
            }

            var treeCollection = new TreeGridItemCollection(itemsForCollection);
            return treeCollection;
        }

        private static TreeGridItem GetItem(OSMMetaData parentFeature, List<OSMMetaData> childFeatures)
        {
            var parentItem = new TreeGridItem
            {
                Values = new string[] {
                    parentFeature.Name, parentFeature.Explanation, "false"
                },
            };

            foreach (var child in childFeatures)
            {
                var childItem = new TreeGridItem
                {
                    Values = new string[] {
                        child.Name, child.Explanation, "false"
                    }
                };
                parentItem.Children.Add(childItem);
            }

            return parentItem;
        }
    }
}
