namespace Caribou.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Caribou.Data;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestInputHandling
    {
        [TestMethod]
        public void TestSingleExample()
        {
            var input = new List<string>() { "amenity:restaurant" };
            var results = ParseRequest.ParseFeatureRequestFromGrasshopper(input);
            Assert.AreEqual(results[0], new ParseRequest("amenity", "restaurant"));
            Assert.AreNotEqual(results[0], new ParseRequest("amenity", "zz"));
        }

        [TestMethod]
        public void TestTripleExampleNewLine()
        {
            var input = new List<string>() {
                "amenity:restaurant",
                "highway:residential",
                "waterway"
            };
            var results = ParseRequest.ParseFeatureRequestFromGrasshopper(input);
            Assert.AreEqual(results[0], new ParseRequest("amenity", "restaurant"));
            Assert.AreNotEqual(results[0], new ParseRequest("amenity", "zz"));
            Assert.AreEqual(results[1], new ParseRequest("highway", "residential"));
            Assert.AreEqual(results[2], new ParseRequest("waterway", null));
        }

        [TestMethod]
        public void TestDoubleExampleNewLine()
        {
            var input = new List<string>() {
                "amenity:restaurant\nhighway:residential",
                "waterway"
            };
            var results = ParseRequest.ParseFeatureRequestFromGrasshopper(input);
            Assert.AreEqual(results[0], new ParseRequest("amenity", "restaurant"));
            Assert.AreNotEqual(results[0], new ParseRequest("amenity", "zz"));
            Assert.AreEqual(results[1], new ParseRequest("highway", "residential"));
            Assert.AreEqual(results[2], new ParseRequest("waterway", null));
        }
        [TestMethod]
        public void TestTripleExampleComma()
        {
            var input = new List<string>() {
                "amenity:restaurant,highway:residential,",
                "waterway"
            };
            var results = ParseRequest.ParseFeatureRequestFromGrasshopper(input);
            Assert.AreEqual(results[0], new ParseRequest("amenity", "restaurant"));
            Assert.AreNotEqual(results[0], new ParseRequest("amenity", "zz"));
            Assert.AreEqual(results[1], new ParseRequest("highway", "residential"));
            Assert.AreEqual(results[2], new ParseRequest("waterway", null));
        }

        [TestMethod]
        public void TestDoubleExampleComma()
        {
            var input = new List<string>() {
                "amenity:restaurant,highway:residential",
                ",waterway"
            };
            var results = ParseRequest.ParseFeatureRequestFromGrasshopper(input);
            Assert.AreEqual(results[0], new ParseRequest("amenity", "restaurant"));
            Assert.AreNotEqual(results[0], new ParseRequest("amenity", "zz"));
            Assert.AreEqual(results[1], new ParseRequest("highway", "residential"));
            Assert.AreEqual(results[2], new ParseRequest("waterway", null));
        }
    }
}
