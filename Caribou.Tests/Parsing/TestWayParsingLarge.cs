namespace Caribou.Tests.Parsing
{
    using System.Collections.Generic;
    using System.Linq;
    using Caribou.Models;
    using Caribou.Tests.Cases;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestWayParsingLarge : MelbourneCase
    {
        const int AllHighways = 612;
        const int AllAmenities = 45;
        const int AllBuildings = 466;
        const int AllAmenitiesRestaurants = 0;
        const int AllAmenitiesWorship = 6;
        const int AllHighwaysResidential = 5;
        const int AllBuildingsRetail = 19;
        const int FirstHighwaysResidentialNodesCount = 42;
        const double FirstAmenityWorshipFirstNodeLat = -37.8164641;
        const double FirstAmenityWorksipLastNodeLat = -37.8165976; // Actually 2nd to last as closed
        const double FirstHighwayResidentialFirstNodeLon = 144.9735701;
        const double FirstHighwayResidentialLastNodeLon = 144.9659697; // Actually last as not closed

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

            Assert.AreEqual(AllAmenities, CountWaysForMetaData(results, amenitiesData));
            Assert.AreEqual(AllHighways, CountWaysForMetaData(results, highwaysData));
            Assert.AreEqual(AllBuildings, CountWaysForMetaData(results, buildingsData));
        }

        [TestMethod]
        public void ParseWaysGivenKeyValueViaXMLReader()
        {
            var results = fetchResultsViaXMLReader(OSMXMLs, miscSubFeatures, OSMGeometryType.Way);

            Assert.AreEqual(AllAmenitiesRestaurants, CountWaysForMetaData(results, amenitiesRestaurantsData));
            Assert.AreEqual(AllAmenitiesWorship, CountWaysForMetaData(results, amenitiesWorshipData));
            Assert.AreEqual(AllBuildingsRetail, CountWaysForMetaData(results, buildingsRetailData));
            Assert.AreEqual(AllHighwaysResidential, CountWaysForMetaData(results, highwayResidentialData));

            var itemCountA = results.FoundData[amenitiesWorshipData][0].Coords.Count;
            Assert.AreEqual(FirstAmenityWorshipFirstNodeLat, results.FoundData[amenitiesWorshipData][0].Coords[0].Latitude);
            Assert.AreEqual(FirstAmenityWorshipFirstNodeLat, results.FoundData[amenitiesWorshipData][0].Coords[itemCountA - 1].Latitude);
            Assert.AreEqual(FirstAmenityWorksipLastNodeLat, results.FoundData[amenitiesWorshipData][0].Coords[itemCountA - 2].Latitude);
            Assert.AreEqual(FirstHighwaysResidentialNodesCount, results.FoundData[highwayResidentialData][0].Coords.Count);

            var itemCountB = results.FoundData[highwayResidentialData][0].Coords.Count;
            Assert.AreEqual(FirstHighwayResidentialFirstNodeLon, results.FoundData[highwayResidentialData][0].Coords[0].Longitude);
            Assert.AreEqual(FirstHighwayResidentialLastNodeLon, results.FoundData[highwayResidentialData][0].Coords[itemCountB - 1].Longitude);
        }

        [TestMethod]
        public void ParseWaysGivenDoubleKeyViaXMLReader()
        {
            var query = new OSMTag("addr:street", "Swanston Street");
            var test = new ParseRequest(new List<OSMTag>() { query });
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
