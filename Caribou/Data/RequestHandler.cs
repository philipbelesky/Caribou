namespace Caribou.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Grasshopper;
    using Grasshopper.Kernel.Data;
    using Grasshopper.Kernel.Types;

    /// <summary>
    /// A datatype and series of methods that each type of component uses to structure it's requested metadata and its returned data
    /// Basically provides a ton of shared logic independent of Way/Node requested types
    /// </summary>
    public class RequestHandler
    {
        public OSMXMLFiles XmlCollection;
        private ParseRequest requestedMetaData;
        public Coord MinBounds;
        public Coord MaxBounds;

        public Dictionary<OSMMetaData, List<FoundItem>> FoundData; // The collected items per request
        public List<string> FoundItemIds; // Used to track for duplicate ways/nodes across files

        public RequestHandler(OSMXMLFiles providedXMLs, ParseRequest requestedMetaData)
        {
            this.XmlCollection = providedXMLs;
            this.requestedMetaData = requestedMetaData;
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
            FoundData[match].Add(new FoundItem(nodeTags, coords));
        }

        private List<OSMMetaData> RequestsThatWantItem(string nodeId, Dictionary<string, string> nodeTags)
        {
            var matches = new List<OSMMetaData>();

            if (string.IsNullOrEmpty(nodeId))
            {
                return matches;
            }
            if (this.FoundItemIds.Contains(nodeId))
            {
                return matches;
            }

            foreach (var request in this.requestedMetaData.RequestedMetaData)
            {
                if (!nodeTags.ContainsKey(request.Id))
                {
                    continue;
                }
                else if (request.Key == null)
                {
                    matches.Add(request);
                }
                else if (request.Key.Id == nodeTags[request.Key.Id])
                {
                    matches.Add(request);
                }
            }
            return matches;
        }

        public GH_Structure<GH_String> MakeTreeForItemTags()
        {
            // TODO
            return new GH_Structure<GH_String>();
        }

        public GH_Structure<GH_String> MakeTreeForMetaDataReport()
        {
            // TODO
            return new GH_Structure<GH_String>();
        }

    }
}
