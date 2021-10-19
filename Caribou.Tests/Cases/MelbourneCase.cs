namespace Caribou.Tests.Cases
{
    using System.Collections.Generic;
    using Caribou.Models;
    using Caribou.Tests.Parsing;

    public class MelbourneCase : BaseParsingTest
    {
        protected static readonly List<string> OSMXMLs = new List<string>() {
            Properties.Resources.MelbourneOSM
        };

        protected static readonly ParseRequest mainFeatures = new ParseRequest(
            new List<OSMTag>() {
                new OSMTag("amenity"), // 610 nodes 45 ways
                new OSMTag("HIGHWAY"),  // 143 nodes, 615 ways
                new OSMTag("building") // 140 nodes, 466 ways
            }
        );

        protected static readonly ParseRequest miscSubFeatures = new ParseRequest(
            new List<string>() {
                "amenity=restaurant", // 173 nodes; 0 ways
                "Amenity=place_of_worship", // 2 node; 7 ways
                "highway=Residential", // 0 nodes; 5 ways
                "highway=residential", // 0 nodes; 5 ways
                "building=retail" // // 130 nodes 19 ways
            });
    }
}
