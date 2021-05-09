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
        protected static MessagesWrapper messages = new MessagesWrapper();
        private OSMMetaData expectedParsedAmenityRestaraunt = new OSMMetaData("restaurant", "", false);
        private OSMMetaData notExpectedParsedAmenityRestaraunt = new OSMMetaData("amenity", "", false);

        [TestMethod]
        public void TestSingleExample()
        {
            var input = new List<string>() { "amenity:restaurant" };
            var results = new ParseRequest(input, ref messages);
            Assert.AreEqual(results.RequestedMetaData[0], expectedParsedAmenityRestaraunt);
            Assert.AreNotEqual(results.RequestedMetaData[0], notExpectedParsedAmenityRestaraunt);
        }

        //[TestMethod]
        //public void TestTripleExampleNewLine()
        //{
        //    var input = new List<string>() {
        //        "amenity:restaurant",
        //        "highway:residential",
        //        "waterway"
        //    };
        //    var results = new ParseRequest(input, ref messages);
        //    Assert.AreEqual(results.RequestedMetaData[0], expectedParsedAmenityRestaraunt);
        //    Assert.AreNotEqual(results.RequestedMetaData[0], notExpectedParsedAmenityRestaraunt);
        //    Assert.AreEqual(results[1], new ParseRequest("highway", "residential"));
        //    Assert.AreEqual(results[2], new ParseRequest("waterway", null));
        //}

        //[TestMethod]
        //public void TestDoubleExampleNewLine()
        //{
        //    var input = new List<string>() {
        //        "amenity:restaurant\nhighway:residential",
        //        "waterway"
        //    };
        //    var results = ParseRequest.ParseFeatureRequestFromGrasshopper(input);
        //    Assert.AreEqual(results[0], new ParseRequest("amenity", "restaurant"));
        //    Assert.AreNotEqual(results[0], new ParseRequest("amenity", "zz"));
        //    Assert.AreEqual(results[1], new ParseRequest("highway", "residential"));
        //    Assert.AreEqual(results[2], new ParseRequest("waterway", null));
        //}
        //[TestMethod]
        //public void TestTripleExampleComma()
        //{
        //    var input = new List<string>() {
        //        "amenity:restaurant,highway:residential,",
        //        "waterway"
        //    };
        //    var results = ParseRequest.ParseFeatureRequestFromGrasshopper(input);
        //    Assert.AreEqual(results[0], new ParseRequest("amenity", "restaurant"));
        //    Assert.AreNotEqual(results[0], new ParseRequest("amenity", "zz"));
        //    Assert.AreEqual(results[1], new ParseRequest("highway", "residential"));
        //    Assert.AreEqual(results[2], new ParseRequest("waterway", null));
        //}

        //[TestMethod]
        //public void TestDoubleExampleComma()
        //{
        //    var input = new List<string>() {
        //        "amenity:restaurant,highway:residential",
        //        ",waterway"
        //    };
        //    var results = ParseRequest.ParseFeatureRequestFromGrasshopper(input);
        //    Assert.AreEqual(results[0], new ParseRequest("amenity", "restaurant"));
        //    Assert.AreNotEqual(results[0], new ParseRequest("amenity", "zz"));
        //    Assert.AreEqual(results[1], new ParseRequest("highway", "residential"));
        //    Assert.AreEqual(results[2], new ParseRequest("waterway", null));
        //}
    }
}
