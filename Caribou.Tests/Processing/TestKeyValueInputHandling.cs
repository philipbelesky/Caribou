﻿namespace Caribou.Tests.Processing
{
    using System.Collections.Generic;
    using Caribou.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestKeyValueInputHandling
    {
        private List<string> input;
        private ParseRequest results;
        private readonly OSMTag expectedParsedGeological = new OSMTag("geological");
        private readonly OSMTag expectedParsedAmenityRestaraunt = new OSMTag("amenity", "restaurant");
        private readonly OSMTag expectedParsedHighWayResidential = new OSMTag("highway", "residential");
        private readonly OSMTag expectedParsedWaterway = new OSMTag("waterway");

        [TestMethod]
        public void TestSingleKey()
        {
            input = new List<string>() { "geological" };
            results = new ParseRequest(input);
            Assert.AreEqual(expectedParsedGeological, results.Requests[0]);
        }

        [TestMethod]
        public void TestSingleKeyValue()
        { 
            input = new List<string>() { "amenity=restaurant" };
            results = new ParseRequest(input);
            Assert.AreEqual(expectedParsedAmenityRestaraunt, results.Requests[0]);
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
            results = new ParseRequest(input);
            Assert.AreEqual(new OSMTag("name"), results.Requests[0]);
            Assert.AreEqual(new OSMTag("wikipedia"), results.Requests[1]);
            Assert.AreEqual(new OSMTag("route_master", "tram"), results.Requests[2]);
            Assert.AreEqual(new OSMTag("tram_stop", "yes"), results.Requests[3]);
        }

        private void CheckResult(List<string> input)
        {
            results = new ParseRequest(input);
            Assert.AreEqual(expectedParsedAmenityRestaraunt, results.Requests[0]);
            Assert.AreEqual(expectedParsedHighWayResidential, results.Requests[1]);
            Assert.AreEqual(expectedParsedWaterway, results.Requests[2]);
        }
    }
}
