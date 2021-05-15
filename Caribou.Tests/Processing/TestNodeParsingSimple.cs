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
        const double expectedFirstRestaurantLat = -37.8134515;
        const double expectedFirstBuildingLon = 144.9658410;
        const int allNamedThings = 1;
        const int allWikiRelated = 1;
        const int allTramRoutes = 0;
        const int allTramStops = 1;

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
            var firstBuilding = results.FoundData[buildingsData][0];

            Assert.AreEqual(allCrafts, CountNodesForMetaData(results, craftsData));
            Assert.AreEqual(allAmenities, CountNodesForMetaData(results, amenitiesData));
            Assert.AreEqual(allBuildings, CountNodesForMetaData(results, buildingsData));
            Assert.AreEqual(expectedFirstBuildingLon, firstBuilding.Coords[0].Longitude);
            Assert.AreEqual("falconry", firstBuilding.Tags["craft"]);
            Assert.AreEqual("retail", firstBuilding.Tags["building"]);
        }

        [TestMethod]
        public void ParseNodesGivenSubFeatureValueViaXMLReader()
        {
            var results = fetchResultsViaXMLReader(OSMXMLs, miscSubFeatures, OSMGeometryType.Node);
            var firstRestaurant = results.FoundData[amenitiesRestaurantsData][0];

            Assert.AreEqual(allAmenitiesRestaurants, CountNodesForMetaData(results, amenitiesRestaurantsData));
            Assert.AreEqual(allCraftJewellers, CountNodesForMetaData(results, craftJewellersData));
            Assert.AreEqual(allBuildingsRetail, CountNodesForMetaData(results, buildingsRetailData));

            Assert.AreEqual(expectedFirstRestaurantLat, firstRestaurant.Coords[0].Latitude);
            Assert.AreEqual("restaurant", firstRestaurant.Tags["amenity"]);
        }

        [TestMethod]
        public void ParseNodesGivenKeyValueViaXMLReader()
        {
            var results = fetchResultsViaXMLReader(OSMXMLs, arbitraryKeyValues, OSMGeometryType.Node);

            Assert.AreEqual(allAmenitiesRestaurants, CountNodesForMetaData(results, amenitiesRestaurantsData));
            Assert.AreEqual(allNamedThings, CountNodesForMetaData(results, namedThingsData));
            Assert.AreEqual(allWikiRelated, CountNodesForMetaData(results, wikiRelatedData));
            Assert.AreEqual(allTramRoutes, CountNodesForMetaData(results, tramRoutesData));
            Assert.AreEqual(allTramStops, CountNodesForMetaData(results, tramStopsData));
            Assert.AreEqual("falconry", results.FoundData[namedThingsData][0].Tags["craft"]);
            Assert.AreEqual("Ye Olde Falcon Store", results.FoundData[namedThingsData][0].Tags["name"]);
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
