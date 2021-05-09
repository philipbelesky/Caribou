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

        protected readonly List<ParseRequest> mainFeatures = new List<ParseRequest>()
        {
            new ParseRequest("craft", ""), // 2 nodes 2 ways
            new ParseRequest("amenity",  ""), // 1 node 0 ways
            new ParseRequest("building",  "")  // 2 nodes 1 ways
        };

        protected readonly List<ParseRequest> miscSubFeatures = new List<ParseRequest>()
        {
            new ParseRequest( "amenity", "restaurant" ), // 1 nodes; 0 ways
            new ParseRequest( "craft", "jeweller" ), // 1 nodes; 1 way
            new ParseRequest( "building", "retail" ) // // 2 nodes 0 ways
        };
    }
}
