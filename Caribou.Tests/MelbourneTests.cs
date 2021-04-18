namespace Caribou.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Caribou.Processing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MelbourneTests
    {
        private string melbourneFile = Properties.Resources.MelbourneOSM;
        private RequestedFeature[] miscBagOfFeaturesAndSubs = new RequestedFeature[]
        {
            new RequestedFeature( "amenity", "restaurant" ), 
            new RequestedFeature( "craft", "jeweller" ), 
            new RequestedFeature( "highway", "residential" ) // Null case; these are ways
        };
        private RequestedFeature[] restaraunts = new RequestedFeature[] 
        {
            new RequestedFeature("amenity", ""), new RequestedFeature("highway",  "")
        };

        [TestMethod]
        public void ParseMelbourneForKeyValueViaXMLReader()
        {
            var matches = Caribou.Processing.FindNodesViaXMLReader.FindByFeatures(miscBagOfFeaturesAndSubs, melbourneFile);
            Assert.AreEqual(173, matches.Results["amenity"]["restaurant"].Count);
            Assert.AreEqual(-37.8134515, matches.Results["amenity"]["restaurant"][0].Latitude);
            Assert.AreEqual(1, matches.Results["craft"]["jeweller"].Count);
            Assert.AreEqual(144.9658410, matches.Results["craft"]["jeweller"][0].Longitude);
            Assert.AreEqual(0, matches.Results["highway"]["residential"].Count);
        }

        [TestMethod]
        public void ParseMelbourneForKeyValueViaXMLDocument()
        {
            var matches = Caribou.Processing.FindNodesViaXMLDocument.FindByFeatures(miscBagOfFeaturesAndSubs, melbourneFile);
            Assert.AreEqual(173, matches.Results["amenity"]["restaurant"].Count);
            Assert.AreEqual(-37.8134515, matches.Results["amenity"]["restaurant"][0].Latitude);
            Assert.AreEqual(1, matches.Results["craft"]["jeweller"].Count);
            Assert.AreEqual(144.9658410, matches.Results["craft"]["jeweller"][0].Longitude);
            Assert.AreEqual(0, matches.Results["highway"]["residential"].Count);
        }

        //[TestMethod]
        //public void ParseMelbourneForKeyValueViaLinq()
        //{
        //    var matches = Caribou.Processing.FindNodesViaLinq.FindByFeatures(miscBagOfFeaturesAndSubs, melbourneFile);
        //    Assert.AreEqual(173, matches.Results["amenity"]["restaurant"].Count);
        //    Assert.AreEqual(-37.8134515, matches.Results["amenity"]["restaurant"][0].Latitude);
        //    Assert.AreEqual(1, matches.Results["craft"]["jeweller"].Count);
        //    Assert.AreEqual(144.9658410, matches.Results["craft"]["jeweller"][0].Longitude);
        //    Assert.AreEqual(0, matches.Results["highway"]["residential"].Count);
        //}

        [TestMethod]
        public void ParseMelbourneForKeyViaXMLReader()
        {
            var matches = Caribou.Processing.FindNodesViaXMLReader.FindByFeatures(restaraunts, melbourneFile);
            Assert.AreEqual(610, CountForKey(matches, "amenity"));
            Assert.AreEqual(143, CountForKey(matches, "highway"));
        }

        [TestMethod]
        public void ParseMelbourneForKeyViaXMLDocument()
        {
            var matches = Caribou.Processing.FindNodesViaXMLDocument.FindByFeatures(restaraunts, melbourneFile);
            Assert.AreEqual(610, CountForKey(matches, "amenity"));
            Assert.AreEqual(143, CountForKey(matches, "highway"));
        }

        //[TestMethod]
        //public void ParseMelbourneForKeyViaLinq()
        //{
        //    var matches = Caribou.Processing.FindNodesViaLinq.FindByFeatures(restaraunts, melbourneFile);
        //    Assert.AreEqual(655, CountForKey(matches, "amenity"));
        //    Assert.AreEqual(755, CountForKey(matches, "highway"));
        //}

        private int CountForKey(ResultsForFeatures matches, string key)
        {
            var matchesCount = 0;
            foreach (var matchedSubFeature in matches.Results[key].Keys)
            {
                matchesCount += matches.Results[key][matchedSubFeature].Count;
            }
            return matchesCount;
        }
    }
}
