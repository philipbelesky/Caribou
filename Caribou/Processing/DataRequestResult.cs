namespace Caribou.Processing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public readonly struct DataRequestedFeature
    {
        // A specific key:value pair represent feature/subfeatures to search for in an OSM file
        // Setup as a struct rather than dict for easier use of a specific hardcoded key to mean "all subfeatures"
        public string Feature { get; }
        public string SubFeature { get; }
        public const string SearchAllKey = "__ALL__"; // magic value; represents finding all subfeatures

        public DataRequestedFeature(string feature, string subFeature)
        {
            Feature = feature;
            SubFeature = subFeature; // "" means to not search by subfeature
            if (string.IsNullOrEmpty(SubFeature))
            {
                SubFeature = SearchAllKey;
            }
        }

        public override string ToString() => $"({Feature}, {SubFeature})";
    }

    // A two-tier dictionary array organised by feature:subfeature and storing results from the OSM parse
    public struct ResultsForFeatures
    {
        public Dictionary<string, Dictionary<string, List<Coords>>> Nodes { get; }

        public Dictionary<string, Dictionary<string, List<Coords[]>>> Ways { get; }

        public (Coords, Coords) LatLonBounds { get; set; }

        public ResultsForFeatures(DataRequestedFeature[] requestedFeatures)
        {
            this.Nodes = new Dictionary<string, Dictionary<string, List<Coords>>>();
            this.Ways = new Dictionary<string, Dictionary<string, List<Coords[]>>>();
            for (int i = 0; i < requestedFeatures.Length; i++)
            {
                // For each feature initialise its keys and lists (if needed)
                var feature = requestedFeatures[i].Feature;
                var subFeature = requestedFeatures[i].SubFeature;

                if (this.Nodes.Keys.Contains(feature))
                {
                    this.Nodes[feature][subFeature] = new List<Coords>();
                }
                else
                {
                    this.Nodes[feature] = new Dictionary<string, List<Coords>>
                    {
                        { subFeature, new List<Coords>() }
                    };
                }

                if (this.Ways.Keys.Contains(feature))
                {
                    this.Ways[feature][subFeature] = new List<Coords[]>();
                }
                else
                {
                    this.Ways[feature] = new Dictionary<string, List<Coords[]>>
                    {
                        { subFeature, new List<Coords[]>() }
                    };
                }
            }
            this.LatLonBounds = (new Coords (0, 0), new Coords(0, 0));
        }

        public void AddNodeGivenFeature(string tagKey, string tagValue, double lat, double lon)
        {
            if (this.Nodes[tagKey].ContainsKey(tagValue))
            {
                // If this particular value is already present in the dictionary (e.g. already added before)
                this.Nodes[tagKey][tagValue].Add(new Coords(lat, lon));
            }
            else
            {
                this.Nodes[tagKey][tagValue] = new List<Coords>() { new Coords(lat, lon) };
            }
        }

        // Same as above but without the unecessary check as the feature:subFeature were known/set during init
        public void AddNodeGivenFeatureAndSubFeature(string tagKey, string tagValue, double lat, double lon)
        {
            this.Nodes[tagKey][tagValue].Add(new Coords(lat, lon));
        }

        public void SetLatLonBounds(double latMin, double lonMin, double latMax, double lonMax)
        {
            this.LatLonBounds = (new Coords(latMin, lonMin), new Coords(latMax, lonMax));
        }
    }
}
