using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caribou.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Caribou.Tests
{
    [TestClass]
    public class TestInputHandling
    {
        [TestMethod]
        public void TestSingleExample()
        {
            var input = new List<string>() { "amenity:restaurant" };
            var results = FeatureRequest.ParseFeatureRequestFromGrasshopper(input);
            Assert.AreEqual(results[0], new FeatureRequest("amenity", "restaurant"));
            Assert.AreNotEqual(results[0], new FeatureRequest("amenity", "zz"));
        }

        [TestMethod]
        public void TestTripleExampleNewLine()
        {
            var input = new List<string>() { 
                "amenity:restaurant", 
                "highway:residential",
                "waterway"
            };
            var results = FeatureRequest.ParseFeatureRequestFromGrasshopper(input);
            Assert.AreEqual(results[0], new FeatureRequest("amenity", "restaurant"));
            Assert.AreNotEqual(results[0], new FeatureRequest("amenity", "zz"));
            Assert.AreEqual(results[1], new FeatureRequest("highway", "residential"));
            Assert.AreEqual(results[2], new FeatureRequest("waterway", null));
        }

        [TestMethod]
        public void TestDoubleExampleNewLine()
        {
            var input = new List<string>() {
                "amenity:restaurant\nhighway:residential",
                "waterway"
            };
            var results = FeatureRequest.ParseFeatureRequestFromGrasshopper(input);
            Assert.AreEqual(results[0], new FeatureRequest("amenity", "restaurant"));
            Assert.AreNotEqual(results[0], new FeatureRequest("amenity", "zz"));
            Assert.AreEqual(results[1], new FeatureRequest("highway", "residential"));
            Assert.AreEqual(results[2], new FeatureRequest("waterway", null));
        }
        [TestMethod]
        public void TestTripleExampleComma()
        {
            var input = new List<string>() {
                "amenity:restaurant,highway:residential,",
                "waterway"
            };
            var results = FeatureRequest.ParseFeatureRequestFromGrasshopper(input);
            Assert.AreEqual(results[0], new FeatureRequest("amenity", "restaurant"));
            Assert.AreNotEqual(results[0], new FeatureRequest("amenity", "zz"));
            Assert.AreEqual(results[1], new FeatureRequest("highway", "residential"));
            Assert.AreEqual(results[2], new FeatureRequest("waterway", null));
        }

        [TestMethod]
        public void TestDoubleExampleComma()
        {
            var input = new List<string>() {
                "amenity:restaurant,highway:residential",
                ",waterway"
            };
            var results = FeatureRequest.ParseFeatureRequestFromGrasshopper(input);
            Assert.AreEqual(results[0], new FeatureRequest("amenity", "restaurant"));
            Assert.AreNotEqual(results[0], new FeatureRequest("amenity", "zz"));
            Assert.AreEqual(results[1], new FeatureRequest("highway", "residential"));
            Assert.AreEqual(results[2], new FeatureRequest("waterway", null));
        }
    }
}
