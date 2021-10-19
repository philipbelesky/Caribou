namespace Caribou.Tests.Parsing
{
    using System.Collections.Generic;
    using Caribou.Models;
    using Caribou.Tests.Cases;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestNodeParsingSimple : SimpleCase
    {
        const int AllCrafts = 2;
        const int AllAmenities = 1;
        const int AllBuildings = 2;
        const int AllAmenitiesRestaurants = 1;
        const int AllCraftJewellers = 1;
        const int AllBuildingsRetail = 2;
        const double ExpectedFirstRestaurantLat = -37.8134515;
        const double ExpectedFirstBuildingLon = 144.9658410;
        const int AllNamedThings = 1;
        const int AllWikiRelated = 1;
        const int AllTramRoutes = 0;
        const int AllTramStops = 1;

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

            Assert.AreEqual(AllCrafts, CountNodesForMetaData(results, craftsData));
            Assert.AreEqual(AllAmenities, CountNodesForMetaData(results, amenitiesData));
            Assert.AreEqual(AllBuildings, CountNodesForMetaData(results, buildingsData));
            Assert.AreEqual(ExpectedFirstBuildingLon, firstBuilding.Coords[0].Longitude);
            Assert.AreEqual("falconry", firstBuilding.Tags["craft"]);
            Assert.AreEqual("retail", firstBuilding.Tags["building"]);
        }

        [TestMethod]
        public void ParseNodesGivenSubFeatureValueViaXMLReader()
        {
            var results = fetchResultsViaXMLReader(OSMXMLs, miscSubFeatures, OSMGeometryType.Node);
            var firstRestaurant = results.FoundData[amenitiesRestaurantsData][0];

            Assert.AreEqual(AllAmenitiesRestaurants, CountNodesForMetaData(results, amenitiesRestaurantsData));
            Assert.AreEqual(AllCraftJewellers, CountNodesForMetaData(results, craftJewellersData));
            Assert.AreEqual(AllBuildingsRetail, CountNodesForMetaData(results, buildingsRetailData));

            Assert.AreEqual(ExpectedFirstRestaurantLat, firstRestaurant.Coords[0].Latitude);
            Assert.AreEqual("restaurant", firstRestaurant.Tags["amenity"]);
        }

        [TestMethod]
        public void ParseNodesGivenKeyValueViaXMLReader()
        {
            var results = fetchResultsViaXMLReader(OSMXMLs, arbitraryKeyValues, OSMGeometryType.Node);

            Assert.AreEqual(AllAmenitiesRestaurants, CountNodesForMetaData(results, amenitiesRestaurantsData));
            Assert.AreEqual(AllNamedThings, CountNodesForMetaData(results, namedThingsData));
            Assert.AreEqual(AllWikiRelated, CountNodesForMetaData(results, wikiRelatedData));
            Assert.AreEqual(AllTramRoutes, CountNodesForMetaData(results, tramRoutesData));
            Assert.AreEqual(AllTramStops, CountNodesForMetaData(results, tramStopsData));
            Assert.AreEqual("falconry", results.FoundData[namedThingsData][0].Tags["craft"]);
            Assert.AreEqual("Ye Olde Falcon Store", results.FoundData[namedThingsData][0].Tags["name"]);
        }

        [TestMethod]
        public void ParseNodesGivenDoubleKeyViaXMLReader()
        {
            var queryA = new OSMTag("addr:street", "Swanston Street");
            var test = new ParseRequest(new List<OSMTag>() { queryA });
            var results = fetchResultsViaXMLReader(OSMXMLs, test, OSMGeometryType.Node);
            Assert.AreEqual(2, CountNodesForMetaData(results, queryA));

            var queryB = new List<string>() { "addr:street=Swanston Street" };
            test = new ParseRequest(queryB);
            results = fetchResultsViaXMLReader(OSMXMLs, test, OSMGeometryType.Node);
            Assert.AreEqual(2, CountNodesForMetaData(results, queryA));

            var queryC = new List<string>() { "addr:street=swanston street" };
            test = new ParseRequest(queryC);
            results = fetchResultsViaXMLReader(OSMXMLs, test, OSMGeometryType.Node);
            Assert.AreEqual(2, CountNodesForMetaData(results, queryA));
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
