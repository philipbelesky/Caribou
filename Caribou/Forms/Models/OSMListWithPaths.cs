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
        public List<OSMMetaData> items;
        public Dictionary<string, List<GH_Path>> pathsPerItem;

        public OSMListWithPaths(GH_Structure<GH_String> tagsTree)
        {
            items = new List<OSMMetaData>();
            pathsPerItem = new Dictionary<string, List<GH_Path>>();
            // Convert from tree of strings representing tags to linear list of OSM Items
            for (int pathIndex = 0; pathIndex < tagsTree.Paths.Count; pathIndex++)
            {
                var tagPath = tagsTree.Paths[pathIndex];
                List<GH_String> itemsInPath = tagsTree[tagPath];
                for (int tagIndex = 0; tagIndex < itemsInPath.Count; tagIndex++)
                {
                    // Make new item and track the path it came from
                    var tagString = itemsInPath[tagIndex];
                    var osmItem = new OSMMetaData(tagString.ToString());
                    if (osmItem != null)
                    {
                        if (!items.Contains(osmItem)) // Prevent duplicates
                            items.Add(osmItem);

                        if (pathsPerItem.ContainsKey(osmItem.ToString()))
                            pathsPerItem[osmItem.ToString()].Add(tagPath);
                        else
                            pathsPerItem[osmItem.ToString()] = new List<GH_Path>() { tagPath };
                    }
                }
            }
        }
    }
}
