namespace Caribou.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Caribou.Processing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestsWayParsing : MelbourneCase
    {
        private int allHighways = 612;
        private int allAmenities = 45;
        private int allAmenitiesRestaurants;
        private int allAmenitiesWorship = 6;
        private int allHighwaysResidential = 5;
        private double firstAmenityWorshipFirstNodeLat = -37.8164641;
        private double firstAmenityWorksipLastNodeLat = -37.8165976; // Actually 2nd to last as closed 
        private double firstHighwayResidentialFirstNodeLon = 144.9735701;
        private double firstHighwayResidentialLastNodeLon = 144.9659697; // Actually last as not closed 

        private int CountWaysFoundForKey(ResultsForFeatures matches, string key)
        {
            var matchesCount = 0;
            foreach (var matchedSubFeature in matches.Ways[key].Keys)
            {
                matchesCount += matches.Ways[key][matchedSubFeature].Count;
            }
            return matchesCount;
        }

        [TestMethod]
        public void ParseWaysGivenKeyViaXMLDocument()
        {
            var matches = ParseViaXMLDocument.FindByFeatures(restarauntsAndHighways, melbourneFile);
            Assert.AreEqual(allAmenities, CountWaysFoundForKey(matches, "amenity"));
            Assert.AreEqual(allHighways, CountWaysFoundForKey(matches, "highway"));
        }

        [TestMethod]
        public void ParseWaysGivenKeyViaLinq()
        {
            var matches = ParseViaLinq.FindByFeatures(restarauntsAndHighways, melbourneFile);
            Assert.AreEqual(allAmenities, CountWaysFoundForKey(matches, "amenity"));
            Assert.AreEqual(allHighways, CountWaysFoundForKey(matches, "highway"));
        }

        [TestMethod]
        public void ParseWaysGivenKeyViaXMLReader()
        {
            var matches = ParseViaXMLReader.FindByFeatures(restarauntsAndHighways, melbourneFile);
            Assert.AreEqual(allAmenities, CountWaysFoundForKey(matches, "amenity"));
            Assert.AreEqual(allHighways, CountWaysFoundForKey(matches, "highway"));
        }

        [TestMethod]
        public void ParseWaysGivenKeyValueViaXMLReader()
        {
            var matches = ParseViaXMLReader.FindByFeatures(miscBagOfFeaturesAndSubs, melbourneFile);
            Assert.AreEqual(allAmenitiesRestaurants, matches.Ways["amenity"]["restaurant"].Count);

            //Assert.AreEqual(allAmenitiesWorship, matches.Ways["amenity"]["place_of_worship"].Count);
            var nodesCountA = matches.Ways["amenity"]["place_of_worship"][0].Count;
            Assert.AreEqual(firstAmenityWorshipFirstNodeLat, matches.Ways["amenity"]["place_of_worship"][0][0].Latitude);
            Assert.AreEqual(firstAmenityWorshipFirstNodeLat, matches.Ways["amenity"]["place_of_worship"][0][nodesCountA - 1].Latitude);
            Assert.AreEqual(firstAmenityWorksipLastNodeLat, matches.Ways["amenity"]["place_of_worship"][0][nodesCountA  - 2].Latitude);

            Assert.AreEqual(allHighwaysResidential, matches.Ways["highway"]["residential"].Count);
            var nodesCountB = matches.Ways["highway"]["residential"][0].Count;
            Assert.AreEqual(firstHighwayResidentialFirstNodeLon, matches.Ways["highway"]["residential"][0][0].Longitude);
            Assert.AreEqual(firstHighwayResidentialLastNodeLon, matches.Ways["highway"]["residential"][0][nodesCountB - 1].Longitude);
        }

        [TestMethod]
        public void ParseWaysGivenKeyValueViaXMLDocument()
        {
            var matches = ParseViaXMLDocument.FindByFeatures(miscBagOfFeaturesAndSubs, melbourneFile);

            Assert.AreEqual(allAmenitiesWorship, matches.Ways["amenity"]["place_of_worship"].Count);
            Assert.AreEqual(firstAmenityWorshipFirstNodeLat, matches.Ways["amenity"]["place_of_worship"][0][0].Latitude);
            Assert.AreEqual(firstAmenityWorshipFirstNodeLat, matches.Ways["amenity"]["place_of_worship"][0][-1].Latitude);
            Assert.AreEqual(firstAmenityWorksipLastNodeLat, matches.Ways["amenity"]["place_of_worship"][0][-2].Latitude);

            Assert.AreEqual(allHighwaysResidential, matches.Ways["highway"]["residential"].Count);
            Assert.AreEqual(firstHighwayResidentialFirstNodeLon, matches.Ways["highway"]["residential"][0][-1].Latitude);
            Assert.AreEqual(firstHighwayResidentialLastNodeLon, matches.Ways["highway"]["residential"][0][-2].Latitude);
        }

        [TestMethod]
        public void ParseWaysGivenKeyValueViaLinq()
        {
            var matches = ParseViaLinq.FindByFeatures(miscBagOfFeaturesAndSubs, melbourneFile);

            Assert.AreEqual(allAmenitiesWorship, matches.Ways["amenity"]["place_of_worship"].Count);
            Assert.AreEqual(firstAmenityWorshipFirstNodeLat, matches.Ways["amenity"]["place_of_worship"][0][0].Latitude);
            Assert.AreEqual(firstAmenityWorshipFirstNodeLat, matches.Ways["amenity"]["place_of_worship"][0][-1].Latitude);
            Assert.AreEqual(firstAmenityWorksipLastNodeLat, matches.Ways["amenity"]["place_of_worship"][0][-2].Latitude);

            Assert.AreEqual(allHighwaysResidential, matches.Ways["highway"]["residential"].Count);
            Assert.AreEqual(firstHighwayResidentialFirstNodeLon, matches.Ways["highway"]["residential"][0][-1].Latitude);
            Assert.AreEqual(firstHighwayResidentialLastNodeLon, matches.Ways["highway"]["residential"][0][-2].Latitude);
        }
    }
}
