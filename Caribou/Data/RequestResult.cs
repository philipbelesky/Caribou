namespace Caribou.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Caribou.Processing;
    using Grasshopper;
    using Grasshopper.Kernel.Data;

    // A two-tier dictionary array organised by feature:subfeature and storing results from the OSM parse
    public class RequestResults
    {
        public RequestResults(List<FeatureRequest> requestedFeatures)
        {
            this.Nodes = new Dictionary<string, Dictionary<string, List<Coord>>>();
            this.Ways = new Dictionary<string, Dictionary<string, List<Coord[]>>>();
            this.RequestedFeatures = requestedFeatures;

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
        public List<FeatureRequest> RequestedFeatures { get; }
        public Dictionary<string, Dictionary<string, List<Coord>>> Nodes { get; }
        public Dictionary<string, Dictionary<string, List<Coord[]>>> Ways { get; }
        public Coord ExtentsMin { get; set; }
        public Coord ExtentsMax { get; set; }

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
            this.ExtentsMin = new Coord(latMin, lonMin);
            this.ExtentsMax = new Coord(latMax, lonMax);
        }

        public DataTree<string> ReportFoundFeatures(bool includeCounts)
        {
            var results = new DataTree<string>();

            var i = 0;
            foreach (var featureType in this.RequestedFeatures)
            {
                //string name;
                //GH_Path subfeaturePath = new GH_Path(i);
                //results.EnsurePath(subfeaturePath);

                //if (featureType.SubFeature == FeatureRequest.SearchAllKey)
                //{
                //    name = $"{featureType}:unspecified";
                //}
                //else
                //{
                //    name = $"{featureType}:{subfeatureType}";
                //}

                //if (includeCounts) // TODO: wire up to menu toggle
                //{
                //    var nodeCount = this.Nodes[featureType][subfeatureType].Count.ToString();
                //    var wayCount = this.Ways[featureType][subfeatureType].Count.ToString();
                //    name = name.PadRight(20, ' '); // pad string so the counts are easy to read
                //    name += $" {nodeCount.PadLeft(5)} nodes {wayCount.PadLeft(5)} ways";
                //}

                //results.Add(name, subfeaturePath);
                i++;
            }

            return results;
        }
    }
}
