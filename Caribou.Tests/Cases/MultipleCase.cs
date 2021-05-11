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

    public class MultipleCase : BaseNodeParsingTest
    {
        protected static OSMXMLFiles OSMXMLs = new OSMXMLFiles(new List<string>() {
            Properties.Resources.MultipleA,
            Properties.Resources.MultipleB,
            Properties.Resources.MultipleC,
        }, ref messages);

        protected static ParseRequest mainFeatures = new ParseRequest(
            new List<OSMMetaData>() {
                new OSMMetaData("craft"), // x nodes x ways
                new OSMMetaData("amenity"), // x nodes x ways
                new OSMMetaData("building")  // x nodes x ways
            }
        );

        protected static ParseRequest miscSubFeatures = new ParseRequest(
            new List<string>() {
                "amenity:restaurant", // x nodes x ways
                "craft:jeweller", // x nodes x ways
                "building:retail", // x nodes x ways
            }, ref messages); 
    }
}
