namespace Caribou.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Caribou.Models;
    using Caribou.Processing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestWayParsingSimple : SimpleCase
    {
        // Counting number of ways per type
        const int allCrafts = 2;
        const int allAmenities = 0;
        const int allBuildings = 1;
        const int allAmenitiesRestaurants = 0;
        const int allCraftJewellers = 1;
        const int allBuildingsRetail = 0;
        const int allNamedThings = 2;
        const int allWikiRelated = 1;
        const int allTramRoutes = 1;
        const int allTramStops = 0;
        // Counting number of nodes per way
        const int ndsInEquitableHouse = 4;
        const int ndsInEquitablePlaza = 3;
        // Comparing nodes within a away
        readonly Coord firstNdForEquitableHouse = new Coord(-37.8154598, 144.9630487);
        readonly Coord secondNdForEquitablePlaza = new Coord(-37.8153878, 144.9632926);


        //    //[TestMethod]
        //    public void ParseWaysGivenKeyViaXMLDocument()
        //    {
        //        var matches = ParseViaXMLDocument.FindByFeatures(mainFeatures, simpleFile);
        //        Assert.AreEqual(allCrafts, CountWaysFoundForKey(matches, "craft"));
        //        Assert.AreEqual(allAmenities, CountWaysFoundForKey(matches, "amenity"));
        //        Assert.AreEqual(allBuildings, CountWaysFoundForKey(matches, "building"));
        //    }

        //    //[TestMethod]
        //    public void ParseWaysGivenKeyViaLinq()
        //    {
        //        var matches = ParseViaLinq.FindByFeatures(mainFeatures, simpleFile);
        //        Assert.AreEqual(allCrafts, CountWaysFoundForKey(matches, "craft"));
        //        Assert.AreEqual(allAmenities, CountWaysFoundForKey(matches, "amenity"));
        //        Assert.AreEqual(allBuildings, CountWaysFoundForKey(matches, "building"));
        //    }

        [TestMethod]
        public void ParseWaysGivenFeatureViaXMLReader()
        {
            var results = fetchResultsViaXMLReader(OSMXMLs, mainFeatures, OSMGeometryType.Way);
            var equitableHouse = results.FoundData[buildingsData][0];

            Assert.AreEqual(allCrafts, CountWaysForMetaData(results, craftsData));
            Assert.AreEqual(allAmenities, CountWaysForMetaData(results, amenitiesData));
            Assert.AreEqual(allBuildings, CountWaysForMetaData(results, buildingsData));

            Assert.AreEqual(ndsInEquitableHouse, equitableHouse.Coords.Count);
            Assert.AreEqual(firstNdForEquitableHouse, equitableHouse.Coords[0]);
            Assert.AreEqual("yes", equitableHouse.Tags["building"]);
            Assert.AreEqual("falconry", equitableHouse.Tags["craft"]);
            Assert.AreEqual("Equitable House", equitableHouse.Tags["name"]);
        }

        [TestMethod]
        public void ParseWaysGivenSubFeatureViaXMLReader()
        {
            var results = fetchResultsViaXMLReader(OSMXMLs, miscSubFeatures, OSMGeometryType.Way);
            var equitablePlaza = results.FoundData[craftJewellersData][0];

            Assert.AreEqual(allAmenitiesRestaurants, CountWaysForMetaData(results, amenitiesRestaurantsData));
            Assert.AreEqual(allCraftJewellers, CountWaysForMetaData(results, craftJewellersData));
            Assert.AreEqual(allBuildingsRetail, CountWaysForMetaData(results, buildingsRetailData));

            Assert.AreEqual(ndsInEquitablePlaza, equitablePlaza.Coords.Count);
            Assert.AreEqual(secondNdForEquitablePlaza, equitablePlaza.Coords[1]);
            Assert.AreEqual("jeweller", equitablePlaza.Tags["craft"]);
            Assert.AreEqual("Equitable Plaza", equitablePlaza.Tags["name"]);
        }

        [TestMethod]
        public void ParseWaysGivenKeyValueViaXMLReader()
        {
            var results = fetchResultsViaXMLReader(OSMXMLs, arbitraryKeyValues, OSMGeometryType.Way);

            Assert.AreEqual(allNamedThings, CountWaysForMetaData(results, namedThingsData));
            Assert.AreEqual(allWikiRelated, CountWaysForMetaData(results, wikiRelatedData));
            Assert.AreEqual(allTramRoutes, CountWaysForMetaData(results, tramRoutesData));
            Assert.AreEqual(allTramStops, CountWaysForMetaData(results, tramStopsData));
            Assert.AreEqual("falconry", results.FoundData[namedThingsData][0].Tags["craft"]);
            Assert.AreEqual("Equitable House", results.FoundData[namedThingsData][0].Tags["name"]);
        }

        [TestMethod]
        public void ParseWaysGivenDoubleKeyViaXMLReader()
        {
            var queryA = new OSMMetaData("Swanston Street", "addr:street");
            var test = new ParseRequest(new List<OSMMetaData>() { queryA });
            var results = fetchResultsViaXMLReader(OSMXMLs, test, OSMGeometryType.Way);
            Assert.AreEqual(1, CountWaysForMetaData(results, queryA));

            var queryB = new List<string>() { "addr:street=Swanston Street" };
            test = new ParseRequest(queryB, ref messages);
            results = fetchResultsViaXMLReader(OSMXMLs, test, OSMGeometryType.Way);
            Assert.AreEqual(1, CountWaysForMetaData(results, queryA));

            var queryC = new List<string>() { "addr:street=swanston street" };
            test = new ParseRequest(queryC, ref messages);
            results = fetchResultsViaXMLReader(OSMXMLs, test, OSMGeometryType.Way);
            Assert.AreEqual(1, CountWaysForMetaData(results, queryA));
        }

        //    //[TestMethod]
        //    public void ParseWaysGivenKeyValueViaXMLDocument()
        //    {
        //        var matches = ParseViaXMLDocument.FindByFeatures(miscSubFeatures, simpleFile);
        //        Assert.AreEqual(allAmenitiesRestaurants, matches.Ways["amenity"]["restaurant"].Count);
        //        Assert.AreEqual(allCraftJewellers, matches.Ways["craft"]["jeweller"].Count);
        //        Assert.AreEqual(allBuildingsRetail, matches.Ways["building"]["retail"].Count);
        //    }

        //    //[TestMethod]
        //    public void ParseWaysGivenKeyValueViaLinq()
        //    {
        //        var matches = ParseViaLinq.FindByFeatures(miscSubFeatures, simpleFile);
        //        Assert.AreEqual(allAmenitiesRestaurants, matches.Ways["amenity"]["restaurant"].Count);
        //        Assert.AreEqual(allCraftJewellers, matches.Ways["craft"]["jeweller"].Count);
        //        Assert.AreEqual(allBuildingsRetail, matches.Ways["building"]["retail"].Count);
        //    }
        //}
    }
}
