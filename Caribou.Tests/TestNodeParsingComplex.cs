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
        const int allAmenities = 610;
        const int allHighways = 143;
        const int allBuildings = 140;
        const int allAmenitiesRestaurants = 173;
        const int allAmenitiesWorship = 2;
        const int allHighwaysResidential = 0;
        const int allBuildingsRetail = 130;
        const double firstWorkshopLon = 144.963903;
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
