using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caribou.Tests
{
    using Caribou.Data;
    using Caribou.Processing;

    public class SimpleCase
    {
        protected readonly string simpleFile = Properties.Resources.SimpleOSM;

        protected readonly List<FeatureRequest> mainFeatures = new List<FeatureRequest>()
        {
            new FeatureRequest("craft", ""), // 2 nodes 2 ways
            new FeatureRequest("amenity",  ""), // 1 node 0 ways
            new FeatureRequest("building",  "")  // 2 nodes 1 ways
        };

        protected readonly List<FeatureRequest> miscSubFeatures = new List<FeatureRequest>()
        {
            new FeatureRequest( "amenity", "restaurant" ), // 1 nodes; 0 ways
            new FeatureRequest( "craft", "jeweller" ), // 1 nodes; 1 way
            new FeatureRequest( "building", "retail" ) // // 2 nodes 1 ways
        };
    }
}
