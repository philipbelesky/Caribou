namespace Caribou.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Caribou.Data;
    using Caribou.Processing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestsWayParsingMultiple : MultipleCase
    {
        const int allCrafts = 3;
        const int allAmenities = 2;
        const int allBuildings = 5; 
        const int allAmenitiesRestaurants = 2; // Present uniquely A and uniquely in B
        const int allCraftJewellers = 2; // Present as duplicates in A and B; unique in C
        const int allBuildingsRetail = 2; // Two uniques in A and one duplicate; one duplicate in B and one in C

        //[TestMethod]
        //public void ParseNodesGivenKeyViaXMLDocument()
        //{
        //    var matches = ParseViaXMLDocument.FindByFeatures(mainFeatures, simpleFile);
        //    Assert.AreEqual(allCrafts, CountNodesFoundForKey(matches, "craft"));
        //    Assert.AreEqual(allAmenities, CountNodesFoundForKey(matches, "amenity"));
        //    Assert.AreEqual(allBuildings, CountNodesFoundForKey(matches, "building"));
        //}

        //[TestMethod]
        //public void ParseNodesGivenKeyViaLinq()
        //{
        //    var matches = ParseViaLinq.FindByFeatures(mainFeatures, simpleFile);
        //    Assert.AreEqual(allCrafts, CountNodesFoundForKey(matches, "craft"));
        //    Assert.AreEqual(allAmenities, CountNodesFoundForKey(matches, "amenity"));
        //    Assert.AreEqual(allBuildings, CountNodesFoundForKey(matches, "building"));
        //}

        [TestMethod]
        public void ParseNodesGivenFeatureViaXMLReader()
        {
            var results = fetchResultsViaXMLReader(OSMXMLs, mainFeatures, "node");

            Assert.AreEqual(allCrafts, results.FoundData[craftsData].Count);
            Assert.AreEqual(allAmenities, results.FoundData[amenitiesData].Count);
            Assert.AreEqual(allBuildings, results.FoundData[buildingsData].Count);
        }

        [TestMethod]
        public void ParseNodesGivenSubFeatureValueViaXMLReader()
        {
            var results = fetchResultsViaXMLReader(OSMXMLs, miscSubFeatures, "node");

            Assert.AreEqual(allAmenitiesRestaurants, results.FoundData[amenitiesRestaurantsData].Count);
            Assert.AreEqual(allCraftJewellers, results.FoundData[craftJewellersData].Count);
            Assert.AreEqual(allBuildingsRetail, results.FoundData[buildingsRetailData].Count);
        }

        //[TestMethod]
        //public void ParseNodesGivenKeyValueViaXMLDocument()
        //{
        //    var matches = ParseViaXMLDocument.FindByFeatures(miscSubFeatures, simpleFile);
        //    Assert.AreEqual(allAmenitiesRestaurants, matches.Nodes["amenity"]["restaurant"].Count);
        //    Assert.AreEqual(firstRestaurantLat, matches.Nodes["amenity"]["restaurant"][0].Latitude);

        //    Assert.AreEqual(allCraftJewellers, matches.Nodes["craft"]["jeweller"].Count);
        //    Assert.AreEqual(allBuildingsRetail, matches.Nodes["building"]["retail"].Count);
        //}

        //[TestMethod]
        //public void ParseNodesGivenKeyValueViaLinq()
        //{
        //    var matches = ParseViaLinq.FindByFeatures(miscSubFeatures, simpleFile);
        //    Assert.AreEqual(allAmenitiesRestaurants, matches.Nodes["amenity"]["restaurant"].Count);
        //    Assert.AreEqual(firstRestaurantLat, matches.Nodes["amenity"]["restaurant"][0].Latitude);

        //    Assert.AreEqual(allCraftJewellers, matches.Nodes["craft"]["jeweller"].Count);
        //    Assert.AreEqual(allBuildingsRetail, matches.Nodes["building"]["retail"].Count);
        //}
    }
}
