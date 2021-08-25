namespace Caribou.Models
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Caribou.Processing;
    using Grasshopper.Kernel.Data;
    using Grasshopper.Kernel.Types;

    /// <summary>
    /// A datatype and series of methods that each type of component uses to structure its requested metadata and its returned data
    /// Basically provides a ton of shared logic independent of Way/Node requested types.
    /// </summary>
    public class RequestHandler
    {
        public List<string> XmlPaths;
        public ParseRequest RequestedMetaData;
        public Coord MinBounds;
        public Coord MaxBounds;
        public List<Tuple<Coord, Coord>> AllBounds;

        public Dictionary<OSMMetaData, List<FoundItem>> FoundData; // The collected items per request
        public List<string> FoundItemIds; // Used to track for duplicate ways/nodes across files

        public string WorkerId; // Used for progress reporting
        public Action<string, double> ReportProgress;
        public List<int> LinesPerFile;

        public RequestHandler(List<string> providedXMLs, ParseRequest requestedMetaData, OSMGeometryType requestedType,
                              Action<string, double> reportProgress, string workerId)
        {
            this.XmlPaths = providedXMLs;
            this.RequestedMetaData = requestedMetaData;

            // Setup data holders
            this.FoundItemIds = new List<string>();
            this.FoundData = new Dictionary<OSMMetaData, List<FoundItem>>();
            foreach (OSMMetaData metaData in requestedMetaData.Requests)
            {
                this.FoundData[metaData] = new List<FoundItem>();
            }

            // Setup reporting infrastructure
            this.WorkerId = workerId;
            this.ReportProgress = reportProgress;
            if (providedXMLs[0].Length < 1000) // Don't calculate line lengths when working with tests (e.g. passed by contents not path)
                this.LinesPerFile = ProgressReporting.GetLineLengthsForFiles(providedXMLs, requestedType);
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

        public void AddBuildingIfMatchesRequest(string nodeId, Dictionary<string, string> nodeTags, List<Coord> coords)
        {
            if (nodeTags.ContainsKey("building"))
                AddWayIfMatchesRequest(nodeId, nodeTags, coords);
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
            var ci = CultureInfo.InvariantCulture;

            if (string.IsNullOrEmpty(nodeId) || this.FoundItemIds.Contains(nodeId))
            {
                return matches;
            }

            foreach (var request in this.RequestedMetaData.Requests)
            {
                var requestedKey = request.ParentType;
                var requestedValue = request.TagType;

                if (requestedKey == null)
                {
                    // If we are only looking for a key, e.g. all <tag k="building">
                    if (tagsOfFoundNode.ContainsKey(requestedValue))
                    {
                        matches.Add(request);
                    }
                }
                else if (tagsOfFoundNode.ContainsKey(requestedKey.TagType))
                {
                    var testValue = tagsOfFoundNode[requestedKey.TagType];
                    // If we are looking for a key:value pair, e.g .all <tag k="building" v="retail"/>
                    // We don't care about case for matching values, e.g. "Swanston St" vs "swanston st"
                    if (testValue != null && testValue.ToLower(ci) == requestedValue.ToLower(ci))
                    {
                        matches.Add(request);
                    }
                }
            }

            return matches;
        }

        public GH_Structure<GH_String> GetTreeForItemTags()
        {
            return TreeFormatters.MakeTreeForItemTags(this);
        }

        public GH_Structure<GH_String> GetTreeForMetaDataReport()
        {
            return TreeFormatters.MakeTreeForMetaDataReport(this);
        }
    }
}
