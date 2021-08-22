namespace Caribou.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Caribou.Models;
    using Caribou.Processing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestWayParsingMultiple : MultipleCase
    {
        const int allCrafts = 0;
        const int allAmenities = 4;
        const int allBuildings = 0;
        const int allAmenitiesRestaurant = 3;
        const int allCraftJewellers = 0;
        const int allBuildingsRetail = 0;

        //    //[TestMethod]
        //    public void ParseWaysGivenKeyViaXMLDocument()
        //    {
        //        var matches = ParseViaXMLDocument.FindByFeatures(mainFeatures, melbourneFile);
        //        Assert.AreEqual(allAmenities, CountWaysFoundForKey(matches, "amenity"));
        //        Assert.AreEqual(allHighways, CountWaysFoundForKey(matches, "highway"));
        //        Assert.AreEqual(allBuildings, CountWaysFoundForKey(matches, "building"));
        //    }

        //    //[TestMethod]
        //    public void ParseWaysGivenKeyViaLinq()
        //    {
        //        var matches = ParseViaLinq.FindByFeatures(mainFeatures, melbourneFile);
        //        Assert.AreEqual(allAmenities, CountWaysFoundForKey(matches, "amenity"));
        //        Assert.AreEqual(allHighways, CountWaysFoundForKey(matches, "highway"));
        //        Assert.AreEqual(allBuildings, CountWaysFoundForKey(matches, "building"));
        //    }

        [TestMethod]
        public void ParseWaysGivenFeatureViaXMLReader()
        {
            var results = fetchResultsViaXMLReader(OSMXMLs, mainFeatures, OSMGeometryType.Way);

            Assert.AreEqual(allCrafts, CountWaysForMetaData(results, craftsData));
            Assert.AreEqual(allAmenities, CountWaysForMetaData(results, amenitiesData));
            Assert.AreEqual(allBuildings, CountWaysForMetaData(results, buildingsData));
        }

        [TestMethod]
        public void ParseWaysGivenSubFeatureValueViaXMLReader()
        {
            var results = fetchResultsViaXMLReader(OSMXMLs, miscSubFeatures, OSMGeometryType.Way);

            Assert.AreEqual(allAmenitiesRestaurant, CountWaysForMetaData(results, amenitiesRestaurantsData));
            Assert.AreEqual(allCraftJewellers, CountWaysForMetaData(results, craftJewellersData));
            Assert.AreEqual(allBuildingsRetail, CountWaysForMetaData(results, buildingsRetailData));
        }

        //    //[TestMethod]
        //    public void ParseWaysGivenKeyValueViaXMLDocument()
        //    {
        //        var matches = ParseViaXMLDocument.FindByFeatures(miscSubFeatures, melbourneFile);
        //        Assert.AreEqual(allAmenitiesRestaurants, matches.Ways["amenity"]["restaurant"].Count);
        //        Assert.AreEqual(allAmenitiesWorship, matches.Ways["amenity"]["place_of_worship"].Count);
        //        Assert.AreEqual(allHighwaysResidential, matches.Ways["highway"]["residential"].Count);
        //        Assert.AreEqual(allBuildingsRetail, matches.Ways["building"]["retail"].Count);

        //        Assert.AreEqual(firstAmenityWorshipFirstNodeLat, matches.Ways["amenity"]["place_of_worship"][0][0].Latitude);
        //        var itemCountA = matches.Ways["amenity"]["place_of_worship"][0].Length;
        //        Assert.AreEqual(firstAmenityWorshipFirstNodeLat, matches.Ways["amenity"]["place_of_worship"][0][itemCountA - 1].Latitude);
        //        Assert.AreEqual(firstAmenityWorksipLastNodeLat, matches.Ways["amenity"]["place_of_worship"][0][itemCountA - 2].Latitude);
        //        Assert.AreEqual(allHighwaysResidential, matches.Ways["highway"]["residential"].Count);
        //        var itemCountB = matches.Ways["highway"]["residential"][0].Length;
        //        Assert.AreEqual(firstHighwayResidentialFirstNodeLon, matches.Ways["highway"]["residential"][0][1].Latitude);
        //        Assert.AreEqual(firstHighwayResidentialLastNodeLon, matches.Ways["highway"]["residential"][0][itemCountB - 2].Latitude);
        //    }

        //    //[TestMethod]
        //    public void ParseWaysGivenKeyValueViaLinq()
        //    {
        //        var matches = ParseViaLinq.FindByFeatures(miscSubFeatures, melbourneFile);
        //        Assert.AreEqual(allAmenitiesRestaurants, matches.Ways["amenity"]["restaurant"].Count);
        //        Assert.AreEqual(allAmenitiesWorship, matches.Ways["amenity"]["place_of_worship"].Count);
        //        Assert.AreEqual(allHighwaysResidential, matches.Ways["highway"]["residential"].Count);
        //        Assert.AreEqual(allBuildingsRetail, matches.Ways["building"]["retail"].Count);

        //        Assert.AreEqual(firstAmenityWorshipFirstNodeLat, matches.Ways["amenity"]["place_of_worship"][0][0].Latitude);
        //        var itemCountA = matches.Ways["amenity"]["place_of_worship"][0].Length;
        //        Assert.AreEqual(firstAmenityWorshipFirstNodeLat, matches.Ways["amenity"]["place_of_worship"][0][itemCountA - 1].Latitude);
        //        Assert.AreEqual(firstAmenityWorksipLastNodeLat, matches.Ways["amenity"]["place_of_worship"][0][itemCountA - 2].Latitude);
        //        Assert.AreEqual(allHighwaysResidential, matches.Ways["highway"]["residential"].Count);
        //        var itemCountB = matches.Ways["highway"]["residential"][0].Length;
        //        Assert.AreEqual(firstHighwayResidentialFirstNodeLon, matches.Ways["highway"]["residential"][0][itemCountB - 1].Latitude);
        //        Assert.AreEqual(firstHighwayResidentialLastNodeLon, matches.Ways["highway"]["residential"][0][itemCountB - 2].Latitude);
        //    }
    }
}
