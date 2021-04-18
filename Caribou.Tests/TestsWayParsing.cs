namespace Caribou.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Caribou.Processing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestsWayParsing : MelbourneCase
    {
        private int allHighways = 615;
        private int allAmenities = 45;
        private int allAmenitiesRestaurants = 0;
        private int allAmenitiesWorship = 7;
        private int allHighwaysResidential = 5;
        private int CountWaysFoundForKey(ResultsForFeatures matches, string key)
        {
            var matchesCount = 0;
            foreach (var matchedSubFeature in matches.Ways[key].Keys)
            {
                matchesCount += matches.Ways[key][matchedSubFeature].Count;
            }
            return matchesCount;
        }

        [TestMethod]
        public void ParseWaysGivenKeyViaXMLDocument()
        {
            var matches = ParseViaXMLDocument.FindByFeatures(restarauntsAndHighways, melbourneFile);
            Assert.AreEqual(allAmenities, CountWaysFoundForKey(matches, "amenity"));
            Assert.AreEqual(allHighways, CountWaysFoundForKey(matches, "highway"));
        }

        [TestMethod]
        public void ParseWaysGivenKeyViaLinq()
        {
            var matches = ParseViaLinq.FindByFeatures(restarauntsAndHighways, melbourneFile);
            Assert.AreEqual(allAmenities, CountWaysFoundForKey(matches, "amenity"));
            Assert.AreEqual(allHighways, CountWaysFoundForKey(matches, "highway"));
        }

        [TestMethod]
        public void ParseNodesGivenKeyViaXMLReader()
        {
            var matches = ParseViaXMLReader.FindByFeatures(restarauntsAndHighways, melbourneFile);
            Assert.AreEqual(allAmenities, CountWaysFoundForKey(matches, "amenity"));
            Assert.AreEqual(143, CountWaysFoundForKey(matches, "highway"));
        }

        [TestMethod]
        public void ParseWaysGivenKeyValueViaXMLReader()
        {
            var matches = ParseViaXMLReader.FindByFeatures(miscBagOfFeaturesAndSubs, melbourneFile);

        }

        [TestMethod]
        public void ParseWaysGivenKeyValueViaXMLDocument()
        {
            var matches = ParseViaXMLDocument.FindByFeatures(miscBagOfFeaturesAndSubs, melbourneFile);

        }

        [TestMethod]
        public void ParseWaysGivenKeyValueViaLinq()
        {
            var matches = ParseViaLinq.FindByFeatures(miscBagOfFeaturesAndSubs, melbourneFile);
            Assert.AreEqual(173, matches.Nodes["amenity"]["restaurant"].Count);
            Assert.AreEqual(-37.8134515, matches.Nodes["amenity"]["restaurant"][0].Latitude);
            Assert.AreEqual(1, matches.Nodes["craft"]["jeweller"].Count);
            Assert.AreEqual(144.9658410, matches.Nodes["craft"]["jeweller"][0].Longitude);
            Assert.AreEqual(0, matches.Nodes["highway"]["residential"].Count);
        }
    }
}
