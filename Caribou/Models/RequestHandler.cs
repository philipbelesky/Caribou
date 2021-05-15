namespace Caribou.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Caribou.Models;
    using Grasshopper;
    using Grasshopper.Kernel.Data;
    using Grasshopper.Kernel.Types;

    /// <summary>
    /// A datatype and series of methods that each type of component uses to structure its requested metadata and its returned data
    /// Basically provides a ton of shared logic independent of Way/Node requested types.
    /// </summary>
    public class RequestHandler
    {
        public OSMXMLFiles XmlCollection;
        public ParseRequest RequestedMetaData;
        public Coord MinBounds;
        public Coord MaxBounds;

        public Dictionary<OSMMetaData, List<FoundItem>> FoundData; // The collected items per request
        public List<string> FoundItemIds; // Used to track for duplicate ways/nodes across files

        public RequestHandler(OSMXMLFiles providedXMLs, ParseRequest requestedMetaData)
        {
            this.XmlCollection = providedXMLs;
            this.RequestedMetaData = requestedMetaData;

            this.FoundData = new Dictionary<OSMMetaData, List<FoundItem>>();
            foreach (OSMMetaData metaData in requestedMetaData.Requests)
            {
                this.FoundData[metaData] = new List<FoundItem>();
            }

            this.FoundItemIds = new List<string>();
        }

        public void AddWayIfMatchesRequest(string nodeId, Dictionary<string, string> nodeTags, List<Coord> coords)
        {
            var matches = RequestsThatWantItem(nodeId, nodeTags);
            if (matches.Count > 0)
            {
                this.FoundItemIds.Add(nodeId);
                foreach (var match in matches)
                {
                    AddItem(match, nodeTags, coords);
                }
            }
        }

        public void AddNodeIfMatchesRequest(string nodeId, Dictionary<string, string> nodeTags, double lat, double lon)
        {
            var matches = RequestsThatWantItem(nodeId, nodeTags);
            if (matches.Count > 0)
            {
                this.FoundItemIds.Add(nodeId);
                var coords = new List<Coord>() { new Coord(lat, lon) };
                foreach (var match in matches)
                {
                    AddItem(match, nodeTags, coords);
                }
            }
        }

        private void AddItem(OSMMetaData match, Dictionary<string, string> nodeTags, List<Coord> coords)
        {
            var newFind = new FoundItem(nodeTags, coords);
            this.FoundData[match].Add(newFind);
        }

        private List<OSMMetaData> RequestsThatWantItem(string nodeId, Dictionary<string, string> tagsOfFoundNode)
        {
            var matches = new List<OSMMetaData>();

            if (string.IsNullOrEmpty(nodeId) || this.FoundItemIds.Contains(nodeId))
            {
                return matches;
            }

            foreach (var request in this.RequestedMetaData.Requests)
            {
                var requestedKey = request.ParentType;
                var requestedValue = request.ThisType;

                if (requestedKey == null)
                {
                    // If we are only looking for a key, e.g. all <tag k="building">
                    if (tagsOfFoundNode.ContainsKey(requestedValue))
                    {
                        matches.Add(request);
                    }
                }
                else if (tagsOfFoundNode.ContainsKey(requestedKey.ThisType))
                {
                    // If we are looking for a key:value pair, e.g .all <tag k="building" v="retail"/>
                    if (tagsOfFoundNode[requestedKey.ThisType] == requestedValue)
                    {
                        matches.Add(request);
                    }
                }
            }

            return matches;
        }

        public GH_Structure<GH_String> GetTreeForItemTags()
        {
            // TODO
            return TreeFormatters.MakeTreeForItemTags(this);
        }

        public GH_Structure<GH_String> GetTreeForMetaDataReport()
        {
            // TODO
            return TreeFormatters.MakeTreeForMetaDataReport(this);
        }
    }
}
