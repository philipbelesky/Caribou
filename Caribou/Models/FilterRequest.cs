namespace Caribou.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Grasshopper.Kernel.Data;
    using Grasshopper.Kernel.Types;

    /// <summary>
    /// A wrapper around a list of tags provided; parsing their values to OSMItems and tracking their paths
    /// </summary>
    public struct FilterRequest
    {
        public List<OSMMetaData> Requests;
        // For tracking geometry that is associated with a particular tag
        public Dictionary<OSMMetaData, List<GH_Path>> PathsOfRequest;

        // Requests coming to a tree input in Grasshopper, e.g. the Filter components
        public FilterRequest(GH_Structure<GH_String> tagsTree)
        {
            this.Requests = new List<OSMMetaData>();
            this.PathsOfRequest = new Dictionary<OSMMetaData, List<GH_Path>>();

            // Convert from tree to linear list
            for (int pathIndex = 0; pathIndex < tagsTree.Paths.Count; pathIndex++)
            {
                var path = tagsTree.Paths[pathIndex];
                List<GH_String> itemsInPath = tagsTree[path];   
                for (int tagIndex = 0; tagIndex < itemsInPath.Count; tagIndex++)
                {
                    // Make new item and track the path it came from
                    var tagString = itemsInPath[tagIndex];
                    var osmItem = new OSMMetaData(tagString.ToString());
                    if (osmItem != null)
                    {
                        if (!this.Requests.Contains(osmItem)) // Prevent duplicates
                            this.Requests.Add(osmItem);

                        if (this.PathsOfRequest.ContainsKey(osmItem))
                            this.PathsOfRequest[osmItem].Add(path);
                        else
                            this.PathsOfRequest[osmItem] = new List<GH_Path>() { path };
                    }
                }
            }
        }
    }
}
