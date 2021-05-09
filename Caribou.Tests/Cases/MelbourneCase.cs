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

        protected readonly List<ParseRequest> mainFeatures = new List<ParseRequest>()
        {
            new ParseRequest("amenity", ""), // 610 nodes 45 ways
            new ParseRequest("highway",  ""), // 143 nodes, 615 ways
            new ParseRequest("building",  "") // 140 nodes, 466 ways
        };

        protected readonly List<ParseRequest> miscSubFeatures = new List<ParseRequest>()
        {
            new ParseRequest( "amenity", "restaurant" ), // 173 nodes; 0 ways
            new ParseRequest( "amenity", "place_of_worship" ), // 2 node; 7 ways
            new ParseRequest( "highway", "residential" ), // 0 nodes; 5 ways
            new ParseRequest( "highway", "residential" ), // 0 nodes; 5 ways
            new ParseRequest( "building", "retail" ) // // 130 nodes 19 ways
        };
    }
}
