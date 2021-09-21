namespace Caribou.Tests.Cases
{
    using System.Collections.Generic;
    using Caribou.Models;
    using Caribou.Tests.Parsing;

    public class SimpleCase : BaseParsingTest
    {
        protected static readonly List<string> OSMXMLs = new List<string>() {
            Properties.Resources.SimpleOSM
        };

        protected static readonly ParseRequest mainFeatures = new ParseRequest(
            new List<OSMMetaData>() {
                new OSMMetaData("craft"), // 2 nodes 2 ways
                new OSMMetaData("amenity"), // 1 node 0 ways
                new OSMMetaData("Building")  // 2 nodes 1 ways
            }
        );

        protected static readonly ParseRequest miscSubFeatures = new ParseRequest(
            new List<string>() {
                "amenity=restaurant", // 1 nodes; 0 ways
                "Craft=jeweller", // 1 nodes; 1 ways
                "building=Retail", // 2 nodes; 0 ways
            });

        protected static readonly ParseRequest arbitraryKeyValues = new ParseRequest(
            new List<string>() {
                "amenity=restaurant", // 1 nodes; 0 ways
                "name=", // 1 nodes; 2 ways
                "wikipedia", // 1 nodes; 1 way
                "route_master=tram", // 0 nodes 1 way
                "tram_stop=yes" // 1 nodes 0 way
            });
    }
}
