namespace Caribou.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Caribou.Models;
    using Caribou.Processing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestNodeParsingLarge : MelbourneCase
    {
        const int AllAmenities = 610;
        const int AllHighways = 143;
        const int AllBuildings = 140;
        const int AllAmenitiesRestaurants = 173;
        const int AllAmenitiesWorship = 2;
        const int AllHighwaysResidential = 0;
        const int AllBuildingsRetail = 130;
        const double ExpectedFirstWorshopLon = 144.963903;
        const double ExpectedFirstRestaurantLat = -37.8134515;

        //[TestMethod]
        //public void ParseNodesGivenKeyViaXMLDocument()
        //{
        //    var matches = ParseViaXMLDocument.FindByFeatures(mainFeatures, melbourneFile);
        //    Assert.AreEqual(allAmenities, CountNodesFoundForKey(matches, "amenity"));
        //    Assert.AreEqual(allHighways, CountNodesFoundForKey(matches, "highway"));
        //    Assert.AreEqual(allBuildings, CountNodesFoundForKey(matches, "building"));
        //}

        //[TestMethod]
        //public void ParseNodesGivenKeyViaLinq()
        //{
        //    var matches = ParseViaLinq.FindByFeatures(mainFeatures, melbourneFile);
        //    Assert.AreEqual(allAmenities, CountNodesFoundForKey(matches, "amenity"));
        //    Assert.AreEqual(allHighways, CountNodesFoundForKey(matches, "highway"));
        //    Assert.AreEqual(allBuildings, CountNodesFoundForKey(matches, "building"));
        //}

        [TestMethod]
        public void ParseNodesGivenKeyViaXMLReader()
        {
            var results = fetchResultsViaXMLReader(OSMXMLs, mainFeatures, OSMGeometryType.Node);

            Assert.AreEqual(AllAmenities, CountNodesForMetaData(results, amenitiesData));
            Assert.AreEqual(AllHighways, CountNodesForMetaData(results, highwaysData));
            Assert.AreEqual(AllBuildings, CountNodesForMetaData(results, buildingsData));
        }

        [TestMethod]
        public void ParseNodesGivenKeyValueViaXMLReader()
        {
            var results = fetchResultsViaXMLReader(OSMXMLs, miscSubFeatures, OSMGeometryType.Node);
            var firstRestaurantLat = results.FoundData[amenitiesRestaurantsData].First().Coords.First().Latitude;
            var firstFirstWorshopLon = results.FoundData[amenitiesWorshipData].First().Coords.First().Longitude;
                        
            Assert.AreEqual(AllAmenitiesRestaurants, CountNodesForMetaData(results, amenitiesRestaurantsData));
            Assert.AreEqual(ExpectedFirstRestaurantLat, firstRestaurantLat);
            Assert.AreEqual(AllAmenitiesWorship, CountNodesForMetaData(results, amenitiesWorshipData));
            Assert.AreEqual(ExpectedFirstWorshopLon, firstFirstWorshopLon);            
            Assert.AreEqual(AllHighwaysResidential, CountNodesForMetaData(results, highwayResidentialData));
            Assert.AreEqual(AllBuildingsRetail, CountNodesForMetaData(results, buildingsRetailData));
        }

        [TestMethod]
        public void ParseNodesGivenDoubleKeyViaXMLReader()
        {
            var query = new OSMMetaData("Swanston Street", "addr:street");
            var test = new ParseRequest(new List<OSMMetaData>() { query });
            var results = fetchResultsViaXMLReader(OSMXMLs, test, OSMGeometryType.Node);
            Assert.AreEqual(80, CountNodesForMetaData(results, query));

            var rawQuery = new List<string>() { "addr:street=Swanston Street" };
            test = new ParseRequest(rawQuery);
            results = fetchResultsViaXMLReader(OSMXMLs, test, OSMGeometryType.Node);
            Assert.AreEqual(80, CountNodesForMetaData(results, query));
        }

        //[TestMethod]
        //public void ParseNodesGivenKeyValueViaXMLDocument()
        //{
        //    var matches = ParseViaXMLDocument.FindByFeatures(miscSubFeatures, melbourneFile);
        //    Assert.AreEqual(allAmenitiesRestaurants, matches.Nodes["amenity"]["restaurant"].Count);
        //    Assert.AreEqual(firstRestaurantLat, matches.Nodes["amenity"]["restaurant"][0].Latitude);

        //    Assert.AreEqual(allAmenitiesWorship, matches.Nodes["amenity"]["place_of_worship"].Count);
        //    Assert.AreEqual(firstWorkshopLon, matches.Nodes["amenity"]["place_of_worship"][0].Longitude);

        //    Assert.AreEqual(allHighwaysResidential, matches.Nodes["highway"]["residential"].Count);
        //    Assert.AreEqual(allBuildingsRetail, matches.Nodes["building"]["retail"].Count);
        //}

        //[TestMethod]
        //public void ParseNodesGivenKeyValueViaLinq()
        //{
        //    var matches = ParseViaLinq.FindByFeatures(miscSubFeatures, melbourneFile);
        //    Assert.AreEqual(allAmenitiesRestaurants, matches.Nodes["amenity"]["restaurant"].Count);
        //    Assert.AreEqual(firstRestaurantLat, matches.Nodes["amenity"]["restaurant"][0].Latitude);

        //    Assert.AreEqual(allAmenitiesWorship, matches.Nodes["amenity"]["place_of_worship"].Count);
        //    Assert.AreEqual(firstWorkshopLon, matches.Nodes["amenity"]["place_of_worship"][0].Longitude);

        //    Assert.AreEqual(allHighwaysResidential, matches.Nodes["highway"]["residential"].Count);
        //    Assert.AreEqual(allBuildingsRetail, matches.Nodes["building"]["retail"].Count);
        //}
    }
}
