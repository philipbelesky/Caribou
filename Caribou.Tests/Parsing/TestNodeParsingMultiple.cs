namespace Caribou.Tests.Parsing
{
    using Caribou.Models;
    using Caribou.Tests.Cases;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestNodeParsingMultiple : MultipleCase
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
            var results = fetchResultsViaXMLReader(OSMXMLs, mainFeatures, OSMGeometryType.Node);

            Assert.AreEqual(allCrafts, CountNodesForMetaData(results, craftsData)); 
            Assert.AreEqual(allAmenities, CountNodesForMetaData(results, amenitiesData)); 
            Assert.AreEqual(allBuildings, CountNodesForMetaData(results, buildingsData)); 
        }

        [TestMethod]
        public void ParseNodesGivenSubFeatureValueViaXMLReader()
        {
            var results = fetchResultsViaXMLReader(OSMXMLs, miscSubFeatures, OSMGeometryType.Node);

            Assert.AreEqual(allAmenitiesRestaurants, CountNodesForMetaData(results, amenitiesRestaurantsData));
            Assert.AreEqual(allCraftJewellers, CountNodesForMetaData(results, craftJewellersData));
            Assert.AreEqual(allBuildingsRetail, CountNodesForMetaData(results, buildingsRetailData));
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
