namespace Caribou.Processing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public readonly struct DataRequestResult
    {
        // A specific key:value pair represent feature/subfeatures to search for in an OSM file
        // Setup as a struct rather than dict for easier use of a specific hardcoded key to mean "all subfeatures"
        public const string SearchAllKey = "__ALL__"; // magic value; represents finding all subfeatures

        public DataRequestResult(string feature, string subFeature)
        {
            this.Feature = feature;
            this.SubFeature = subFeature; // "" means to not search by subfeature
            if (string.IsNullOrEmpty(this.SubFeature))
            {
                this.SubFeature = SearchAllKey;
            }
        }

        public string Feature { get; }

        public string SubFeature { get; }

        public override string ToString() => $"({this.Feature}, {this.SubFeature})";
    }

    // A two-tier dictionary array organised by feature:subfeature and storing results from the OSM parse
    public struct ResultsForFeatures
    {
        public ResultsForFeatures(DataRequestResult[] requestedFeatures)
        {
            this.Nodes = new Dictionary<string, Dictionary<string, List<Coord>>>();
            this.Ways = new Dictionary<string, Dictionary<string, List<Coord[]>>>();
            for (int i = 0; i < requestedFeatures.Length; i++)
            {
                // For each feature initialise its keys and lists (if needed)
                var feature = requestedFeatures[i].Feature;
                var subFeature = requestedFeatures[i].SubFeature;

                if (this.Nodes.Keys.Contains(feature))
                {
                    this.Nodes[feature][subFeature] = new List<Coord>();
                }
                else
                {
                    this.Nodes[feature] = new Dictionary<string, List<Coord>>
                    {
                        { subFeature, new List<Coord>() },
                    };
                }

                if (this.Ways.Keys.Contains(feature))
                {
                    this.Ways[feature][subFeature] = new List<Coord[]>();
                }
                else
                {
                    this.Ways[feature] = new Dictionary<string, List<Coord[]>>
                    {
                        { subFeature, new List<Coord[]>() },
                    };
                }
            }

            this.LatLonBounds = (new Coord(0, 0), new Coord(0, 0));
        }

        public Dictionary<string, Dictionary<string, List<Coord>>> Nodes { get; }

        public Dictionary<string, Dictionary<string, List<Coord[]>>> Ways { get; }

        public (Coord, Coord) LatLonBounds { get; set; }

        public void AddNodeGivenFeature(string tagKey, string tagValue, double lat, double lon)
        {
            if (this.Nodes[tagKey].ContainsKey(tagValue))
            {
                // If this particular value is already present in the dictionary (e.g. already added before)
                this.Nodes[tagKey][tagValue].Add(new Coord(lat, lon));
            }
            else
            {
                this.Nodes[tagKey][tagValue] = new List<Coord>() { new Coord(lat, lon) };
            }
        }

        // Same as above but without the unecessary check as the feature:subFeature were known/set during init
        public void AddNodeGivenFeatureAndSubFeature(string tagKey, string tagValue, double lat, double lon)
        {
            this.Nodes[tagKey][tagValue].Add(new Coord(lat, lon));
        }

        public void SetLatLonBounds(double latMin, double lonMin, double latMax, double lonMax)
        {
            this.LatLonBounds = (new Coord(latMin, lonMin), new Coord(latMax, lonMax));
        }
    }
}
