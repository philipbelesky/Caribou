namespace Caribou.Tests
{
    using Caribou.Data;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestBoundsParsing : MelbourneCase
    {
        private Coord expectedMinBounds = new Coord(-37.8164200, 144.9627400);
        private Coord expectedMaxBounds = new Coord(-37.8089200, 144.9710600);
        private RequestHandler result = new RequestHandler(OSMXMLs, mainFeatures);

        [TestMethod]
        public void ParseBoundsViaXMLReader()
        {
            Caribou.Processing.ParseViaXMLReader.GetBounds(ref result);
            CheckResult();
        }

        [TestMethod]
        public void ParseBoundsViaXMLDocument()
        {
            Caribou.Processing.ParseViaXMLDocument.GetBounds(ref result);
            CheckResult();
        }

        [TestMethod]
        public void ParseBoundsViaLinq()
        {
            Caribou.Processing.ParseViaLinq.GetBounds(ref result);
            CheckResult();
        }

        private void CheckResult()
        {
            Assert.AreEqual(expectedMinBounds, result.MinBounds);
            Assert.AreEqual(expectedMaxBounds, result.MaxBounds);
            Assert.AreEqual(expectedMinBounds.Latitude, result.MinBounds.Latitude);
            Assert.AreEqual(expectedMaxBounds.Longitude, result.MaxBounds.Longitude);
        }
    }
}
