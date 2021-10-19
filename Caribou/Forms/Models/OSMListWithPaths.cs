namespace Caribou.Forms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Caribou.Models;
    using Grasshopper.Kernel.Data;
    using Grasshopper.Kernel.Types;

    /// <summary>
    /// Convenience for creating and passing a list of OSMMetaData and their associated mappings to geometry paths
    /// </summary>
    public struct OSMListWithPaths
    {
        public List<OSMTag> items;
        public Dictionary<OSMTag, List<GH_Path>> pathsPerItem;

        public OSMListWithPaths(GH_Structure<GH_String> tagsTree)
        {
            items = new List<OSMTag>();
            pathsPerItem = new Dictionary<OSMTag, List<GH_Path>>();
            GH_Path tagPath;
            List<GH_String> itemsInPath;
            GH_String tagString;
            OSMTag osmItem;

            // Convert from tree of strings representing tags to linear list of OSM Items
            for (int pathIndex = 0; pathIndex < tagsTree.Paths.Count; pathIndex++)
            {
                tagPath = tagsTree.Paths[pathIndex];
                itemsInPath = tagsTree[tagPath];

                for (int tagIndex = 0; tagIndex < itemsInPath.Count; tagIndex++)
                {
                    // Make new item and track the path it came from
                    tagString = itemsInPath[tagIndex];
                    osmItem = new OSMTag(tagString.Value);
                    if (osmItem != null)
                    {
                        AddItem(osmItem, ref items, ref pathsPerItem);
                        pathsPerItem[osmItem].Add(tagPath);

                        AddItem(osmItem.Key, ref items, ref pathsPerItem);
                        pathsPerItem[osmItem.Key].Add(tagPath);
                    }
                }
            }
        }

        private void AddItem(OSMTag item, ref List<OSMTag> items,
                            ref Dictionary<OSMTag, List<GH_Path>> pathsPerItem)
        {
            if (!pathsPerItem.ContainsKey(item))
            {
                items.Add(item);
                pathsPerItem[item] = new List<GH_Path>();
            }
        }
    }
}
