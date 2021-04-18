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
        protected readonly string melbourneFile = Properties.Resources.MelbourneOSM;

        protected readonly DataRequestResult[] restarauntsAndHighways = new DataRequestResult[]
        {
            new DataRequestResult("amenity", ""), // 610 nodes 45 ways
            new DataRequestResult("highway",  "") // 143 nodes, 615 ways
        };

        protected readonly DataRequestResult[] miscBagOfFeaturesAndSubs = new DataRequestResult[]
        {
            new DataRequestResult( "amenity", "restaurant" ), // 173 nodes; 0 ways
            new DataRequestResult( "amenity", "place_of_worship" ), // 2 node; 7 ways
            new DataRequestResult( "highway", "residential" ) // 0 nodes; 5 ways
        };

    }
}
