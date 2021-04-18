namespace Caribou.Processing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public readonly struct RequestedFeature
    {
        // A specific key:value pair represent feature/subfeatures to search for in an OSM file
        // Setup as a struct rather than dict for easier use of a specific hardcoded key to mean "all subfeatures"
        public string Feature { get; }
        public string SubFeature { get; }
        public const string SearchAllKey = "__ALL__"; // magic value; represents finding all subfeatures

        public RequestedFeature(string feature, string subFeature)
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
        public Dictionary<string, Dictionary<string, List<Coords>>> Results { get; }

        public ResultsForFeatures(RequestedFeature[] requestedFeatures)
        {
            this.Results = new Dictionary<string, Dictionary<string, List<Coords>>>();
            for (int i = 0; i < requestedFeatures.Length; i++)
            {
                this.Results[requestedFeatures[i].Feature] = new Dictionary<string, List<Coords>>
                {
                    { requestedFeatures[i].SubFeature, new List<Coords>() }
                };
            }
        }

        public void AddCoordForFeature(string tagKey, string tagValue, double lat, double lon)
        {
            if (this.Results[tagKey].ContainsKey(tagValue))
            {
                // If this particular value is already present in the dictionary (e.g. already added before)
                this.Results[tagKey][tagValue].Add(new Coords(lat, lon));
            }
            else
            {
                this.Results[tagKey][tagValue] = new List<Coords>() { new Coords(lat, lon) };
            }
        }

        // Same as above but without the unecessary check as the feature:subFeature were known/set during init
        public void AddCoordForFeatureAndSubFeature(string tagKey, string tagValue, double lat, double lon)
        {
            this.Results[tagKey][tagValue].Add(new Coords(lat, lon));
        }

    }
}
