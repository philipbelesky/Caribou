namespace Caribou.Forms
{
    using System;
    using System.Collections.Generic;
    using Eto.Forms;

    /// <summary>Translates from the Feature/SubFeature datasets into the UI elements in the view.</summary>
    public static class SelectionCollection
    {
        public static TreeGridItemCollection GetCollection(Dictionary<string, List<string>> testData)
        {
            var itemsForCollection = new List<TreeGridItem>();

            foreach (var item in testData)
            {
                var treeItem = GetItem(item.Key, item.Value);
                itemsForCollection.Add(treeItem);
            }

            var treeCollection = new TreeGridItemCollection(itemsForCollection);
            return treeCollection;
        }

        private static TreeGridItem GetItem(string parentTitle, List<string> childrenTitles)
        {
            var parent = new TreeGridItem
            {
                Values = new string[] {
                    parentTitle, "Parent prop 1", "false"
                },
            };

            foreach (var childTitle in childrenTitles)
            {
                var child = new TreeGridItem
                {
                    Values = new string[] {
                        childTitle, "Child prop 1", "false"
                    }
                };
                parent.Children.Add(child);
            }

            return parent;
        }
    }
}
