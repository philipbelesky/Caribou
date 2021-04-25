using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caribou.Processing;

namespace Caribou.Data
{
    // A two-tier dictionary array organised by feature:subfeature and storing results from the OSM parse
    public class RequestResults
    {
        public RequestResults(List<FeatureRequest> requestedFeatures)
        {
            this.Nodes = new Dictionary<string, Dictionary<string, List<Coord>>>();
            this.Ways = new Dictionary<string, Dictionary<string, List<Coord[]>>>();
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
                        [subFeature] = new List<Coord>(),
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
                        [subFeature] = new List<Coord[]>(),
                    };
                }
            }

            this.PrimaryFeaturesToFind = this.Nodes.Keys.ToList();
        }

        public List<string> PrimaryFeaturesToFind { get; }
        public Dictionary<string, Dictionary<string, List<Coord>>> Nodes { get; }
        public Dictionary<string, Dictionary<string, List<Coord[]>>> Ways { get; }
        public Coord extentsMin { get; set; }
        public Coord extentsMax { get; set; }

        public void AddNodeGivenFeature(string feature, string subFeature, Coord coord)
        {
            if (this.Nodes[feature].ContainsKey(subFeature))
            {
                // If this particular value is already present in the dictionary (e.g. already added before)
                this.Nodes[feature][subFeature].Add(coord);
            }
            else
            {
                this.Nodes[feature][subFeature] = new List<Coord>(); 
                this.Nodes[feature][subFeature].Add(coord);
            }
        }

        // Same as above but without the unecessary check as the feature:subFeature were known/set during init
        public void AddNodeGivenFeatureAndSubFeature(string feature, string subFeature, Coord coord)
        {
            this.Nodes[feature][subFeature].Add(coord);
        }

        public void AddWayGivenFeature(string feature, string subFeature, Coord[] wayCoords)
        {
            if (this.Ways[feature].ContainsKey(subFeature))
            {
                // If this particular value is already present in the dictionary (e.g. already added before)
                this.Ways[feature][subFeature].Add(wayCoords);
            }
            else
            {
                this.Ways[feature][subFeature] = new List<Coord[]>();
                this.Ways[feature][subFeature].Add(wayCoords);
            }
        }

        // Same as above but without the unecessary check as the feature:subFeature were known/set during init
        public void AddWayGivenFeatureAndSubFeature(string feature, string subFeature, Coord[] wayCoords)
        {
            this.Ways[feature][subFeature].Add(wayCoords);
        }

        public void SetLatLonBounds(double latMin, double lonMin, double latMax, double lonMax)
        {
            this.extentsMin = new Coord(latMin, lonMin);
            this.extentsMax = new Coord(latMax, lonMax);
        }
    }

}
