using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caribou.Processing
{
    public struct RequestedFeature
    {
        // A specific key:value pair represent feature/subfeatures to search for in an OSM file
        // Setup as a struct rather than dict for easier use of a specific hardcoded key to mean "all subfeatures"
        public RequestedFeature(string feature, string subFeature)
        {
            Feature = feature;
            SubFeature = subFeature; // "" means to not search by subfeature
            if (string.IsNullOrEmpty(SubFeature))
            {
                SubFeature = SearchAllKey;
            }
        }
        public string Feature { get; }
        public string SubFeature { get; }
        public const string SearchAllKey = "__ALL__"; // magic value; represents finding all subfeatures

        public override string ToString() => $"({Feature}, {SubFeature})";
    }

    // A two-tier dictionary array organised by feature:subfeature and storing results from the OSM parse
    public struct ResultsForFeatures
    {
        public ResultsForFeatures(RequestedFeature[] requestedFeatures)
        {
            Results = new Dictionary<string, Dictionary<string, List<Coords>>>();
            for (int i = 0; i < requestedFeatures.Length; i++)
            {
                Results[requestedFeatures[i].Feature] = new Dictionary<string, List<Coords>>
                {
                    { requestedFeatures[i].SubFeature, new List<Coords>() }
                };
            }
        }

        public Dictionary<string, Dictionary<string, List<Coords>>> Results { get; } 
    }
}
