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
        public void ParseMelbourneAForKeyValue()
        {
            var matches = Caribou.Processing.FindNodes.FindByFeaturesA(restarauntsAndJewelers, melbourneFile);
            Assert.AreEqual(matches.Results["amenity"]["restaurant"].Count, 173);
            Assert.AreEqual(matches.Results["amenity"]["restaurant"][0].Latitude, -37.8134515);
            Assert.AreEqual(matches.Results["craft"]["jeweller"].Count, 1);
            Assert.AreEqual(matches.Results["craft"]["jeweller"][0].Longitude, 144.9658410);
        }

        [TestMethod]
        public void ParseMelbourneAForKey()
        {
            var matches = Caribou.Processing.FindNodes.FindByFeaturesA(restaraunts, melbourneFile);
            var amenityMatches = 0;
            foreach (var matchedSubFeature in matches.Results["amenity"].Keys)
            {
                amenityMatches += matches.Results["amenity"][matchedSubFeature].Count;
            }
            Assert.AreEqual(amenityMatches, 655);

            var highwayMatches = 0;
            foreach (var matchedSubFeature in matches.Results["highway"].Keys)
            {
                highwayMatches += matches.Results["highway"][matchedSubFeature].Count;
            }
            Assert.AreEqual(highwayMatches, 755);
        }
    }
}
