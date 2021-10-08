namespace Caribou.Tests.Parsing
{
    using System.Collections.Generic;
    using System.Linq;
    using Caribou.Models;
    using Caribou.Tests.Cases;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestWayParsingSimple : SimpleCase
    {
        // Counting number of ways per type
        const int AllCrafts = 2;
        const int AllAmenities = 0;
        const int AllBuildings = 1;
        const int AllAmenitiesRestaurants = 0;
        const int AllCraftJewellers = 1;
        const int AllBuildingsRetail = 0;
        const int AllNamedThings = 2;
        const int AllWikiRelated = 1;
        const int AllTramRoutes = 1;
        const int AllTramStops = 0;
        // Counting number of nodes per way
        const int NdsInEquitableHouse = 5;
        const int NdsInEquitablePlaza = 4;
        // Comparing nodes within a away
        readonly Coord firstNdForEquitableHouse = new Coord(-37.8154598, 144.9630487);
        readonly Coord secondNdForEquitablePlaza = new Coord(-37.81558, 144.963115);

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

            Assert.AreEqual(AllCrafts, CountWaysForMetaData(results, craftsData));
            Assert.AreEqual(AllAmenities, CountWaysForMetaData(results, amenitiesData));
            Assert.AreEqual(AllBuildings, CountWaysForMetaData(results, buildingsData));

            Assert.AreEqual(NdsInEquitableHouse, equitableHouse.Coords.Count);
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

            Assert.AreEqual(AllAmenitiesRestaurants, CountWaysForMetaData(results, amenitiesRestaurantsData));
            Assert.AreEqual(AllCraftJewellers, CountWaysForMetaData(results, craftJewellersData));
            Assert.AreEqual(AllBuildingsRetail, CountWaysForMetaData(results, buildingsRetailData));

            Assert.AreEqual(NdsInEquitablePlaza, equitablePlaza.Coords.Count);
            Assert.AreEqual(secondNdForEquitablePlaza, equitablePlaza.Coords[1]);
            Assert.AreEqual("jeweller", equitablePlaza.Tags["craft"]);
            Assert.AreEqual("Equitable Plaza", equitablePlaza.Tags["name"]);
        }

        [TestMethod]
        public void ParseWaysGivenKeyValueViaXMLReader()
        {
            var results = fetchResultsViaXMLReader(OSMXMLs, arbitraryKeyValues, OSMGeometryType.Way);

            Assert.AreEqual(AllNamedThings, CountWaysForMetaData(results, namedThingsData));
            Assert.AreEqual(AllWikiRelated, CountWaysForMetaData(results, wikiRelatedData));
            Assert.AreEqual(AllTramRoutes, CountWaysForMetaData(results, tramRoutesData));
            Assert.AreEqual(AllTramStops, CountWaysForMetaData(results, tramStopsData));
            Assert.AreEqual("falconry", results.FoundData[namedThingsData][0].Tags["craft"]);
            Assert.AreEqual("Equitable House", results.FoundData[namedThingsData][0].Tags["name"]);
        }

        [TestMethod]
        public void ParseWaysGivenDoubleKeyViaXMLReader()
        {
            var queryA = new OSMTag("addr:street", "Swanston Street");
            var test = new ParseRequest(new List<OSMTag>() { queryA });
            var results = fetchResultsViaXMLReader(OSMXMLs, test, OSMGeometryType.Way);
            Assert.AreEqual(1, CountWaysForMetaData(results, queryA));

            var queryB = new List<string>() { "addr:street=Swanston Street" };
            test = new ParseRequest(queryB);
            results = fetchResultsViaXMLReader(OSMXMLs, test, OSMGeometryType.Way);
            Assert.AreEqual(1, CountWaysForMetaData(results, queryA));

            var queryC = new List<string>() { "addr:street=swanston street" };
            test = new ParseRequest(queryC);
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
