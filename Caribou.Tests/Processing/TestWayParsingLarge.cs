namespace Caribou.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Caribou.Models;
    using Caribou.Processing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestWayParsingLarge : MelbourneCase
    {
        const int allHighways = 612;
        const int allAmenities = 45;
        const int allBuildings = 466;
        const int allAmenitiesRestaurants = 0;
        const int allAmenitiesWorship = 6;
        const int allHighwaysResidential = 5;
        const int allBuildingsRetail = 19;
        const int firstHighwaysResidentialNodesCount = 42;
        const double firstAmenityWorshipFirstNodeLat = -37.8164641;
        const double firstAmenityWorksipLastNodeLat = -37.8165976; // Actually 2nd to last as closed
        const double firstHighwayResidentialFirstNodeLon = 144.9735701;
        const double firstHighwayResidentialLastNodeLon = 144.9659697; // Actually last as not closed

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
        public void ParseWaysGivenKeyViaXMLReader()
        {
            var results = fetchResultsViaXMLReader(OSMXMLs, mainFeatures, OSMGeometryType.Way);

            Assert.AreEqual(allAmenities, CountWaysForMetaData(results, amenitiesData));
            Assert.AreEqual(allHighways, CountWaysForMetaData(results, highwaysData));
            Assert.AreEqual(allBuildings, CountWaysForMetaData(results, buildingsData));
        }

        [TestMethod]
        public void ParseWaysGivenKeyValueViaXMLReader()
        {
            var results = fetchResultsViaXMLReader(OSMXMLs, miscSubFeatures, OSMGeometryType.Way);

            Assert.AreEqual(allAmenitiesRestaurants, CountWaysForMetaData(results, amenitiesRestaurantsData));
            Assert.AreEqual(allAmenitiesWorship, CountWaysForMetaData(results, amenitiesWorshipData));
            Assert.AreEqual(allBuildingsRetail, CountWaysForMetaData(results, buildingsRetailData));
            Assert.AreEqual(allHighwaysResidential, CountWaysForMetaData(results, highwayResidentialData));

            var itemCountA = results.FoundData[amenitiesWorshipData][0].Coords.Count;
            Assert.AreEqual(firstAmenityWorshipFirstNodeLat, results.FoundData[amenitiesWorshipData][0].Coords[0].Latitude);
            Assert.AreEqual(firstAmenityWorshipFirstNodeLat, results.FoundData[amenitiesWorshipData][0].Coords[itemCountA - 1].Latitude);
            Assert.AreEqual(firstAmenityWorksipLastNodeLat, results.FoundData[amenitiesWorshipData][0].Coords[itemCountA - 2].Latitude);
            Assert.AreEqual(firstHighwaysResidentialNodesCount, results.FoundData[highwayResidentialData][0].Coords.Count);

            var itemCountB = results.FoundData[highwayResidentialData][0].Coords.Count;
            Assert.AreEqual(firstHighwayResidentialFirstNodeLon, results.FoundData[highwayResidentialData][0].Coords[0].Longitude);
            Assert.AreEqual(firstHighwayResidentialLastNodeLon, results.FoundData[highwayResidentialData][0].Coords[itemCountB - 1].Longitude);
        }

        [TestMethod]
        public void ParseWaysGivenDoubleKeyViaXMLReader()
        {
            var query = new OSMMetaData("Swanston Street", "addr:street");
            var test = new ParseRequest(new List<OSMMetaData>() { query });
            var results = fetchResultsViaXMLReader(OSMXMLs, test, OSMGeometryType.Way);
            Assert.AreEqual(2, CountWaysForMetaData(results, query));

            var rawQuery = new List<string>() { "addr:street=Swanston Street" };
            test = new ParseRequest(rawQuery);
            results = fetchResultsViaXMLReader(OSMXMLs, test, OSMGeometryType.Way);
            Assert.AreEqual(2, CountWaysForMetaData(results, query));
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
        //}
    }
}
