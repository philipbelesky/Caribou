namespace Caribou.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Caribou.Data;
    using Caribou.Processing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestNodeParsingSimple : SimpleCase
    {
        const int allCrafts = 2;
        const int allAmenities = 1;
        const int allBuildings = 2;
        const int allAmenitiesRestaurants = 1;
        const int allCraftJewellers = 1;
        const int allBuildingsRetail = 2;
        const double firstRestaurantLat = -37.8134515;

        private static int CountNodesFoundForKey(RequestResults matches, string key)
        {
            var matchesCount = 0;
            foreach (var matchedSubFeature in matches.Nodes[key].Keys)
            {
                matchesCount += matches.Nodes[key][matchedSubFeature].Count;
            }
            return matchesCount;
        }

        [TestMethod]
        public void ParseNodesGivenKeyViaXMLDocument()
        {
            var matches = ParseViaXMLDocument.FindByFeatures(mainFeatures, simpleFile);
            Assert.AreEqual(allCrafts, CountNodesFoundForKey(matches, "craft"));
            Assert.AreEqual(allAmenities, CountNodesFoundForKey(matches, "amenity"));
            Assert.AreEqual(allBuildings, CountNodesFoundForKey(matches, "building"));
        }

        [TestMethod]
        public void ParseNodesGivenKeyViaLinq()
        {
            var matches = ParseViaLinq.FindByFeatures(mainFeatures, simpleFile);
            Assert.AreEqual(allCrafts, CountNodesFoundForKey(matches, "craft"));
            Assert.AreEqual(allAmenities, CountNodesFoundForKey(matches, "amenity"));
            Assert.AreEqual(allBuildings, CountNodesFoundForKey(matches, "building"));
        }

        [TestMethod]
        public void ParseNodesGivenKeyViaXMLReader()
        {
            var matches = ParseViaXMLReader.FindByFeatures(mainFeatures, simpleFile);
            Assert.AreEqual(allCrafts, CountNodesFoundForKey(matches, "craft"));
            Assert.AreEqual(allAmenities, CountNodesFoundForKey(matches, "amenity"));
            Assert.AreEqual(allBuildings, CountNodesFoundForKey(matches, "building"));
        }

        [TestMethod]
        public void ParseNodesGivenKeyValueViaXMLReader()
        {
            var matches = ParseViaXMLReader.FindByFeatures(miscSubFeatures, simpleFile);
            Assert.AreEqual(allAmenitiesRestaurants, matches.Nodes["amenity"]["restaurant"].Count);
            Assert.AreEqual(firstRestaurantLat, matches.Nodes["amenity"]["restaurant"][0].Latitude);

            Assert.AreEqual(allCraftJewellers, matches.Nodes["craft"]["jeweller"].Count);
            Assert.AreEqual(allBuildingsRetail, matches.Nodes["building"]["retail"].Count);
        }

        [TestMethod]
        public void ParseNodesGivenKeyValueViaXMLDocument()
        {
            var matches = ParseViaXMLDocument.FindByFeatures(miscSubFeatures, simpleFile);
            Assert.AreEqual(allAmenitiesRestaurants, matches.Nodes["amenity"]["restaurant"].Count);
            Assert.AreEqual(firstRestaurantLat, matches.Nodes["amenity"]["restaurant"][0].Latitude);

            Assert.AreEqual(allCraftJewellers, matches.Nodes["craft"]["jeweller"].Count);
            Assert.AreEqual(allBuildingsRetail, matches.Nodes["building"]["retail"].Count);
        }

        [TestMethod]
        public void ParseNodesGivenKeyValueViaLinq()
        {
            var matches = ParseViaLinq.FindByFeatures(miscSubFeatures, simpleFile);
            Assert.AreEqual(allAmenitiesRestaurants, matches.Nodes["amenity"]["restaurant"].Count);
            Assert.AreEqual(firstRestaurantLat, matches.Nodes["amenity"]["restaurant"][0].Latitude);

            Assert.AreEqual(allCraftJewellers, matches.Nodes["craft"]["jeweller"].Count);
            Assert.AreEqual(allBuildingsRetail, matches.Nodes["building"]["retail"].Count);
        }
    }
}
