namespace Caribou.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MelbourneTests
    {
        private string melbourneFile = Properties.Resources.MelbourneOSM;
        private Dictionary<string, string> restarauntsAndJewelers = new Dictionary<string, string>
        {
            { "amenity", "restaurant" },
            { "craft", "jeweller" },
        };

        [TestMethod]
        public void ParseMelbourneA()
        {
            var result = Caribou.Processing.FindNodes.FindByFeaturesA(restarauntsAndJewelers, melbourneFile);
            Assert.AreEqual(result["amenity:restaurant"].Count, 173);
            Assert.AreEqual(result["amenity:restaurant"][0].Latitude, -37.8134515);
            Assert.AreEqual(result["craft:jeweller"].Count, 1);
            Assert.AreEqual(result["craft:jeweller"][0].Longitude, 144.9658410);
        }
    }
}
