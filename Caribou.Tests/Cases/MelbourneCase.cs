using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caribou.Tests
{
    using Caribou.Processing;

    public class MelbourneCase
    {
        protected string melbourneFile = Properties.Resources.MelbourneOSM;

        protected DataRequestedFeature[] restarauntsAndHighways = new DataRequestedFeature[]
        {
            new DataRequestedFeature("amenity", ""), // 610 nodes 45 ways
            new DataRequestedFeature("highway",  "") // 143 nodes, 615 ways
        };

        protected DataRequestedFeature[] miscBagOfFeaturesAndSubs = new DataRequestedFeature[]
        {
            new DataRequestedFeature( "amenity", "restaurant" ), // 173 nodes; 0 ways
            new DataRequestedFeature( "amenity", "place_of_worship" ), // 2 node; 7 ways
            new DataRequestedFeature( "highway", "residential" ) // 0 nodes; 5 ways
        };

    }
}
