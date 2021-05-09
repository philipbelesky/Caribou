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

    public class MelbourneCase
    {
        protected static MessagesWrapper msgs = new MessagesWrapper();
        protected static OSMXMLs OSMXMLs = new OSMXMLs(new List<string>() {
            Properties.Resources.MelbourneOSM
        }, ref msgs);

        protected static ParseRequest mainFeatures = new ParseRequest(
            new List<OSMMetaData>() {
                new OSMMetaData("amenity", "", true), // 610 nodes 45 ways
                new OSMMetaData("highway", "", true),  // 143 nodes, 615 ways
                new OSMMetaData("building", "", true) // 140 nodes, 466 ways
            }
        );

        protected static ParseRequest miscSubFeatures = new ParseRequest(
            new List<string>() {
                "amenity:restaurant", // 173 nodes; 0 ways
                "amenity:place_of_worship", // 2 node; 7 ways
                "highway:residential", // 0 nodes; 5 ways
                "highway:residential", // 0 nodes; 5 ways
                "building:retail" // // 130 nodes 19 ways
            }, ref msgs);
    }
}
