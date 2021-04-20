using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caribou.Processing;

namespace Caribou.Data
{
    // A two-tier dictionary array organised by feature:subfeature and storing results from the OSM parse
    public struct RequestResults
    {
        public RequestResults(List<FeatureRequest> requestedFeatures)
        {
            this.Nodes = new Dictionary<string, Dictionary<string, List<Coord>>>();
            this.Ways = new Dictionary<string, Dictionary<string, List<List<Coord>>>>();
            foreach (var requestedFeature in requestedFeatures)
            {
                // For each feature initialise its keys and lists (if needed)
                var feature = requestedFeature.PimraryFeature;
                var subFeature = requestedFeature.SubFeature;

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
                    this.Ways[feature][subFeature] = new List<List<Coord>>();
                }
                else
                {
                    this.Ways[feature] = new Dictionary<string, List<List<Coord>>>
                {
                    { subFeature, new List<List<Coord>>() },
                };
                }
            }

            this.PrimaryFeaturesToFind = this.Nodes.Keys.ToList();
            this.LatLonBounds = (new Coord(0, 0), new Coord(0, 0));
        }

        public List<string> PrimaryFeaturesToFind { get; }
        public Dictionary<string, Dictionary<string, List<Coord>>> Nodes { get; }

        public Dictionary<string, Dictionary<string, List<List<Coord>>>> Ways { get; }

        public (Coord, Coord) LatLonBounds { get; set; }

        public void AddNodeGivenFeature(string tagKey, string tagValue, Coord coord)
        {
            if (this.Nodes[tagKey].ContainsKey(tagValue))
            {
                // If this particular value is already present in the dictionary (e.g. already added before)
                this.Nodes[tagKey][tagValue].Add(coord);
            }
            else
            {
                this.Nodes[tagKey][tagValue] = new List<Coord>() { coord };
            }
        }

        // Same as above but without the unecessary check as the feature:subFeature were known/set during init
        public void AddNodeGivenFeatureAndSubFeature(string tagKey, string tagValue, Coord coord)
        {
            this.Nodes[tagKey][tagValue].Add(coord);
        }

        public void AddWayGivenFeature(string tagKey, string tagValue, List<Coord> wayCoords)
        {
            if (this.Ways[tagKey].ContainsKey(tagValue))
            {
                // If this particular value is already present in the dictionary (e.g. already added before)
                this.Ways[tagKey][tagValue].Add(wayCoords);
            }
            else
            {
                this.Ways[tagKey][tagValue] = new List<List<Coord>>() { wayCoords };
            }
        }

        // Same as above but without the unecessary check as the feature:subFeature were known/set during init
        public void AddWayGivenFeatureAndSubFeature(string tagKey, string tagValue, List<Coord> wayCoords)
        {
            this.Ways[tagKey][tagValue].Add(wayCoords);
        }

        public void SetLatLonBounds(double latMin, double lonMin, double latMax, double lonMax)
        {
            this.LatLonBounds = (new Coord(latMin, lonMin), new Coord(latMax, lonMax));
        }
    }

}
