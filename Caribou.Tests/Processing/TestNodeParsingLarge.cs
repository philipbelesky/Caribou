namespace Caribou.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Caribou.Data;
    using Caribou.Processing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestNodeParsingLarge : MelbourneCase
    {
        const int allAmenities = 610;
        const int allHighways = 143;
        const int allBuildings = 140;
        const int allAmenitiesRestaurants = 173;
        const int allAmenitiesWorship = 2;
        const int allHighwaysResidential = 0;
        const int allBuildingsRetail = 130;
        const double expectedFirstWorshopLon = 144.963903;
        const double expectedFirstRestaurantLat = -37.8134515;

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

            Assert.AreEqual(allAmenities, CountNodesForMetaData(results, amenitiesData));
            Assert.AreEqual(allHighways, CountNodesForMetaData(results, highwaysData));
            Assert.AreEqual(allBuildings, CountNodesForMetaData(results, buildingsData));
        }

        [TestMethod]
        public void ParseNodesGivenKeyValueViaXMLReader()
        {
            var results = fetchResultsViaXMLReader(OSMXMLs, miscSubFeatures, OSMGeometryType.Node);
            var firstRestaurantLat = results.FoundData[amenitiesRestaurantsData].First().Coords.First().Latitude;
            var firstFirstWorshopLon = results.FoundData[amenitiesWorshipData].First().Coords.First().Longitude;
                        
            Assert.AreEqual(allAmenitiesRestaurants, CountNodesForMetaData(results, amenitiesRestaurantsData));
            Assert.AreEqual(expectedFirstRestaurantLat, firstRestaurantLat);
            Assert.AreEqual(allAmenitiesWorship, CountNodesForMetaData(results, amenitiesWorshipData));
            Assert.AreEqual(expectedFirstWorshopLon, firstFirstWorshopLon);            
            Assert.AreEqual(allHighwaysResidential, CountNodesForMetaData(results, highwayResidentialData));
            Assert.AreEqual(allBuildingsRetail, CountNodesForMetaData(results, buildingsRetailData));
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
