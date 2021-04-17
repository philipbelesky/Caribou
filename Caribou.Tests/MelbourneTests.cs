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
        private RequestedFeature[] restarauntsAndJewelers = new RequestedFeature[]
        {
            new RequestedFeature( "amenity", "restaurant" ), new RequestedFeature( "craft", "jeweller" )
        };
        private RequestedFeature[] restaraunts = new RequestedFeature[] 
        {
            new RequestedFeature("amenity", ""), new RequestedFeature("highway",  "")
        };

        [TestMethod]
        public void ParseMelbourneForKeyValueViaXMLReader()
        {
            var matches = Caribou.Processing.FindNodesViaXMLReader.FindByFeatures(restarauntsAndJewelers, melbourneFile);
            Assert.AreEqual(matches.Results["amenity"]["restaurant"].Count, 173);
            Assert.AreEqual(matches.Results["amenity"]["restaurant"][0].Latitude, -37.8134515);
            Assert.AreEqual(matches.Results["craft"]["jeweller"].Count, 1);
            Assert.AreEqual(matches.Results["craft"]["jeweller"][0].Longitude, 144.9658410);
        }

        [TestMethod]
        public void ParseMelbourneForKeyValueViaXMLDocument()
        {
            var matches = Caribou.Processing.FindNodesViaXMLDocument.FindByFeatures(restarauntsAndJewelers, melbourneFile);
            Assert.AreEqual(matches.Results["amenity"]["restaurant"].Count, 173);
            Assert.AreEqual(matches.Results["amenity"]["restaurant"][0].Latitude, -37.8134515);
            Assert.AreEqual(matches.Results["craft"]["jeweller"].Count, 1);
            Assert.AreEqual(matches.Results["craft"]["jeweller"][0].Longitude, 144.9658410);
        }

        [TestMethod]
        public void ParseMelbourneForKeyValueViaLinq()
        {
            var matches = Caribou.Processing.FindNodesViaLinq.FindByFeatures(restarauntsAndJewelers, melbourneFile);
            Assert.AreEqual(matches.Results["amenity"]["restaurant"].Count, 173);
            Assert.AreEqual(matches.Results["amenity"]["restaurant"][0].Latitude, -37.8134515);
            Assert.AreEqual(matches.Results["craft"]["jeweller"].Count, 1);
            Assert.AreEqual(matches.Results["craft"]["jeweller"][0].Longitude, 144.9658410);
        }

        [TestMethod]
        public void ParseMelbourneForKeyViaXMLReader()
        {
            var matches = Caribou.Processing.FindNodesViaXMLReader.FindByFeatures(restaraunts, melbourneFile);
            Assert.AreEqual(CountForKey(matches, "amenity"), 655);
            Assert.AreEqual(CountForKey(matches, "highway"), 755);
        }

        [TestMethod]
        public void ParseMelbourneForKeyViaXMLDocument()
        {
            var matches = Caribou.Processing.FindNodesViaXMLReader.FindByFeatures(restaraunts, melbourneFile);
            Assert.AreEqual(CountForKey(matches, "amenity"), 655);
            Assert.AreEqual(CountForKey(matches, "highway"), 755);
        }

        [TestMethod]
        public void ParseMelbourneForKeyViaLinq()
        {
            var matches = Caribou.Processing.FindNodesViaXMLReader.FindByFeatures(restaraunts, melbourneFile);
            Assert.AreEqual(CountForKey(matches, "amenity"), 655);
            Assert.AreEqual(CountForKey(matches, "highway"), 755);
        }

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
