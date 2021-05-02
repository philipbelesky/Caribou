using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caribou.Tests
{
    using Caribou.Data;
    using Caribou.Processing;

    public class MelbourneCase
    {
        protected readonly string melbourneFile = Properties.Resources.MelbourneOSM;

        protected readonly List<FeatureRequest> mainFeatures = new List<FeatureRequest>()
        {
            new FeatureRequest("amenity", ""), // 610 nodes 45 ways
            new FeatureRequest("highway",  ""), // 143 nodes, 615 ways
            new FeatureRequest("building",  "") // 140 nodes, 466 ways
        };

        protected readonly List<FeatureRequest> miscSubFeatures = new List<FeatureRequest>()
        {
            new FeatureRequest( "amenity", "restaurant" ), // 173 nodes; 0 ways
            new FeatureRequest( "amenity", "place_of_worship" ), // 2 node; 7 ways
            new FeatureRequest( "highway", "residential" ), // 0 nodes; 5 ways
            new FeatureRequest( "highway", "residential" ), // 0 nodes; 5 ways
            new FeatureRequest( "building", "retail" ) // // 130 nodes 19 ways
        };
    }
}
