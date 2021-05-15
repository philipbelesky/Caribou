namespace Caribou.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Caribou.Components;
    using Caribou.Data;
    using Caribou.Processing;
    using Caribou.Tests.Processing;

    public class MelbourneCase : BaseNodeParsingTest
    {
        protected static OSMXMLFiles OSMXMLs = new OSMXMLFiles(new List<string>() {
            Properties.Resources.MelbourneOSM
        }, ref messages);

        protected static ParseRequest mainFeatures = new ParseRequest(
            new List<OSMMetaData>() {
                new OSMMetaData("amenity"), // 610 nodes 45 ways
                new OSMMetaData("highway"),  // 143 nodes, 615 ways
                new OSMMetaData("building") // 140 nodes, 466 ways
            }
        );

        protected static ParseRequest miscSubFeatures = new ParseRequest(
            new List<string>() {
                "amenity:restaurant", // 173 nodes; 0 ways
                "amenity:place_of_worship", // 2 node; 7 ways
                "highway:residential", // 0 nodes; 5 ways
                "highway:residential", // 0 nodes; 5 ways
                "building:retail" // // 130 nodes 19 ways
            }, ref messages);
    }
}
