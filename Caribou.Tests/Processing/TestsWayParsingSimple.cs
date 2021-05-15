namespace Caribou.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Caribou.Data;
    using Caribou.Processing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestsWayParsingSimple : SimpleCase
    {
        // Counting number of ways per type
        const int allCrafts = 2;
        const int allAmenities = 0;
        const int allBuildings = 1;
        const int allAmenitiesRestaurants = 0;
        const int allCraftJewellers = 1;
        const int allBuildingsRetail = 0;
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
        public void ParseWaysGivenKeyViaXMLReader()
        {
            var results = fetchResultsViaXMLReader(OSMXMLs, mainFeatures, OSMTypes.Way);

            Assert.AreEqual(allCrafts, CountWaysForMetaData(results, craftsData));
            Assert.AreEqual(allAmenities, CountWaysForMetaData(results, amenitiesData));
            Assert.AreEqual(allBuildings, CountWaysForMetaData(results, buildingsData));

            Assert.AreEqual(ndsInEquitableHouse, results.FoundData[buildingsData][0].Coords.Count());
            Assert.AreEqual(firstNdForEquitableHouse, results.FoundData[buildingsData][0].Coords[0]);
        }

        [TestMethod]
        public void ParseWaysGivenKeyValueViaXMLReader()
        {
            var results = fetchResultsViaXMLReader(OSMXMLs, miscSubFeatures, OSMTypes.Way);

            Assert.AreEqual(allAmenitiesRestaurants, CountWaysForMetaData(results, amenitiesRestaurantsData));
            Assert.AreEqual(allCraftJewellers, CountWaysForMetaData(results, craftJewellersData));
            Assert.AreEqual(allBuildingsRetail, CountWaysForMetaData(results, buildingsRetailData));

            Assert.AreEqual(ndsInEquitablePlaza, results.FoundData[craftJewellersData][0].Coords.Count());
            Assert.AreEqual(secondNdForEquitablePlaza, results.FoundData[craftJewellersData][0].Coords[1]);
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
