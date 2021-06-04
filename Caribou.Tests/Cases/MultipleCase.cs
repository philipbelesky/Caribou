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

    public class MultipleCase : BaseParsingTest
    {
        protected static readonly List<string> OSMXMLs = new List<string>() {
            Properties.Resources.MultipleA,
            Properties.Resources.MultipleB,
            Properties.Resources.MultipleC,
        };

        protected static readonly ParseRequest mainFeatures = new ParseRequest(
            new List<OSMMetaData>() {
                new OSMMetaData("craft"), // x nodes x ways
                new OSMMetaData("Amenity"), // x nodes x ways
                new OSMMetaData("building")  // x nodes x ways
            }
        );

        protected static readonly ParseRequest miscSubFeatures = new ParseRequest(
            new List<string>() {
                "amenity=restaurant", // x nodes x ways
                "Craft=JEWELLER", // x nodes x ways
                "building=retail", // x nodes x ways
            }, ref messages); 
    }
}
