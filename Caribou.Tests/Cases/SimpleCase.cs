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

    public class SimpleCase : BaseNodeParsingTest
    {
        protected static OSMXMLFiles OSMXMLs = new OSMXMLFiles(new List<string>() {
            Properties.Resources.SimpleOSM
        }, ref messages);

        protected static ParseRequest mainFeatures = new ParseRequest(
            new List<OSMMetaData>() {
                new OSMMetaData("craft"), // 2 nodes 2 ways
                new OSMMetaData("amenity"), // 1 node 0 ways
                new OSMMetaData("building")  // 2 nodes 1 ways
            }
        );

        protected static ParseRequest miscSubFeatures = new ParseRequest(
            new List<string>() {
                "amenity:restaurant", // 1 nodes; 0 ways
                "craft:jeweller", // 1 nodes; 1 ways
                "building:retail", // 2 nodes; 0 ways
            }, ref messages); 
    }
}
