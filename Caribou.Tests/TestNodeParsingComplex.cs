namespace Caribou.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Caribou.Data;
    using Caribou.Processing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestNodeParsingComplex : MelbourneCase
    {
        private int allAmenities = 610;
        private int allHighways = 143;
        private int allBuildings = 140;
        private int allAmenitiesRestaurants = 173;
        private int allAmenitiesWorship = 2;
        private int allHighwaysResidential;
        private int allBuildingsRetail = 130;
        private double firstWorkshopLon = 144.963903;
        private double firstRestaurantLat = -37.8134515;

        private int CountNodesFoundForKey(RequestResults matches, string key)
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
            var matches = ParseViaXMLDocument.FindByFeatures(mainFeatures, melbourneFile);
            Assert.AreEqual(allAmenities, CountNodesFoundForKey(matches, "amenity"));
            Assert.AreEqual(allHighways, CountNodesFoundForKey(matches, "highway"));
            Assert.AreEqual(allBuildings, CountNodesFoundForKey(matches, "building"));
        }

        [TestMethod]
        public void ParseNodesGivenKeyViaLinq()
        {
            var matches = ParseViaLinq.FindByFeatures(mainFeatures, melbourneFile);
            Assert.AreEqual(allAmenities, CountNodesFoundForKey(matches, "amenity"));
            Assert.AreEqual(allHighways, CountNodesFoundForKey(matches, "highway"));
            Assert.AreEqual(allBuildings, CountNodesFoundForKey(matches, "building"));
        }

        [TestMethod]
        public void ParseNodesGivenKeyViaXMLReader()
        {
            var matches = ParseViaXMLReader.FindByFeatures(mainFeatures, melbourneFile);
            Assert.AreEqual(allAmenities, CountNodesFoundForKey(matches, "amenity"));
            Assert.AreEqual(allHighways, CountNodesFoundForKey(matches, "highway"));
            Assert.AreEqual(allBuildings, CountNodesFoundForKey(matches, "building"));
        }

        [TestMethod]
        public void ParseNodesGivenKeyValueViaXMLReader()
        {
            var matches = ParseViaXMLReader.FindByFeatures(miscSubFeatures, melbourneFile);
            Assert.AreEqual(allAmenitiesRestaurants, matches.Nodes["amenity"]["restaurant"].Count);
            Assert.AreEqual(firstRestaurantLat, matches.Nodes["amenity"]["restaurant"][0].Latitude);

            Assert.AreEqual(allAmenitiesWorship, matches.Nodes["amenity"]["place_of_worship"].Count);
            Assert.AreEqual(firstWorkshopLon, matches.Nodes["amenity"]["place_of_worship"][0].Longitude);

            Assert.AreEqual(allHighwaysResidential, matches.Nodes["highway"]["residential"].Count);
            Assert.AreEqual(allBuildingsRetail, matches.Nodes["building"]["retail"].Count);
        }

        [TestMethod]
        public void ParseNodesGivenKeyValueViaXMLDocument()
        {
            var matches = ParseViaXMLDocument.FindByFeatures(miscSubFeatures, melbourneFile);
            Assert.AreEqual(allAmenitiesRestaurants, matches.Nodes["amenity"]["restaurant"].Count);
            Assert.AreEqual(firstRestaurantLat, matches.Nodes["amenity"]["restaurant"][0].Latitude);

            Assert.AreEqual(allAmenitiesWorship, matches.Nodes["amenity"]["place_of_worship"].Count);
            Assert.AreEqual(firstWorkshopLon, matches.Nodes["amenity"]["place_of_worship"][0].Longitude);

            Assert.AreEqual(allHighwaysResidential, matches.Nodes["highway"]["residential"].Count);
            Assert.AreEqual(allBuildingsRetail, matches.Nodes["building"]["retail"].Count);
        }

        [TestMethod]
        public void ParseNodesGivenKeyValueViaLinq()
        {
            var matches = ParseViaLinq.FindByFeatures(miscSubFeatures, melbourneFile);
            Assert.AreEqual(allAmenitiesRestaurants, matches.Nodes["amenity"]["restaurant"].Count);
            Assert.AreEqual(firstRestaurantLat, matches.Nodes["amenity"]["restaurant"][0].Latitude);

            Assert.AreEqual(allAmenitiesWorship, matches.Nodes["amenity"]["place_of_worship"].Count);
            Assert.AreEqual(firstWorkshopLon, matches.Nodes["amenity"]["place_of_worship"][0].Longitude);

            Assert.AreEqual(allHighwaysResidential, matches.Nodes["highway"]["residential"].Count);
            Assert.AreEqual(allBuildingsRetail, matches.Nodes["building"]["retail"].Count);
        }
    }
}
