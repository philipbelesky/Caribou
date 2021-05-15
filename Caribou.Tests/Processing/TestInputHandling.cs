namespace Caribou.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Caribou.Components;
    using Caribou.Data;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestInputHandling
    {
        private List<string> input;
        private ParseRequest results;
        protected static MessagesWrapper messages = new MessagesWrapper();
        private OSMMetaData expectedParsedGeological = new OSMMetaData("geological");
        private OSMMetaData expectedParsedAmenityRestaraunt = new OSMMetaData("restaurant", "amenity");
        private OSMMetaData expectedParsedHighWayResidential = new OSMMetaData("residential", "highway");
        private OSMMetaData expectedParsedWaterway = new OSMMetaData("waterway");

        [TestMethod]
        public void TestSingleKey()
        {
            input = new List<string>() { "geological" };
            results = new ParseRequest(input, ref messages);
            Assert.AreEqual(results.Requests[0], expectedParsedGeological);
        }

        [TestMethod]
        public void TestSingleKeyValue()
        { 
            input = new List<string>() { "amenity=restaurant" };
            results = new ParseRequest(input, ref messages);
            Assert.AreEqual(results.Requests[0], expectedParsedAmenityRestaraunt);
        }

        [TestMethod]
        public void TestTripleExampleNewLine()
        {
            input = new List<string>() {
                "amenity=restaurant",
                "highway=residential",
                "waterway"
            };
            CheckResult(input);
        }

        [TestMethod]
        public void TestDoubleExampleNewLine()
        {
            input = new List<string>() {
                "amenity=restaurant\nhighway=residential",
                "waterway=*"
            };
            CheckResult(input);
        }
        [TestMethod]
        public void TestTripleExampleComma()
        {
            input = new List<string>() {
                "amenity=restaurant,highway=residential,",
                "waterway"
            };
            CheckResult(input);
        }

        [TestMethod]
        public void TestDoubleExampleComma()
        {
            input = new List<string>() {
                "amenity=restaurant,highway=residential",
                ",waterway=*"
            };
            CheckResult(input);
        }

        [TestMethod]
        public void CheckRandomExmaple()
        {
            input = new List<string>() {
                "name=", // 1 nodes; 2 ways
                "wikipedia", // 1 nodes; 1 way
                "route_master=tram", // 0 nodes 1 way
                "tram_stop=yes" // 1 nodes 0 way
            };
            results = new ParseRequest(input, ref messages);
            Assert.AreEqual(results.Requests[0], new OSMMetaData("name"));
            Assert.AreEqual(results.Requests[1], new OSMMetaData("wikipedia"));
            Assert.AreEqual(results.Requests[2], new OSMMetaData("tram", "route_master"));
            Assert.AreEqual(results.Requests[3], new OSMMetaData("yes", "tram_stop"));
        }

        private void CheckResult(List<string> input)
        {
            results = new ParseRequest(input, ref messages);
            Assert.AreEqual(results.Requests[0], expectedParsedAmenityRestaraunt);
            Assert.AreEqual(results.Requests[1], expectedParsedHighWayResidential);
            Assert.AreEqual(results.Requests[2], expectedParsedWaterway);
        }
    }
}
