namespace Caribou.Tests.Bounds
{
    using System.Collections.Generic;
    using Caribou.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestMultipleBoundsParsing : MelbourneCase
    {
        private Coord expectedMinBounds = new Coord(-37.9164200, 144.9127400); // Lowest left
        private Coord expectedMaxBounds = new Coord(-37.2164200, 144.9710600); // Highest right
        private RequestHandler results = new RequestHandler(new List<string>() {
            Properties.Resources.MelbourneOSM,
            Properties.Resources.SimpleOSM
        }, mainFeatures, OSMGeometryType.Node, reportProgress, "Test");

        [TestMethod]
        public void ParseBoundsViaXMLReader()
        {
            Caribou.Processing.ParseViaXMLReader.GetBounds(ref results, true);
            CheckResult();
        }

        [TestMethod]
        public void ParseBoundsViaXMLDocument()
        {
            Caribou.Processing.ParseViaXMLDocument.GetBounds(ref results);
            CheckResult();
        }

        [TestMethod]
        public void ParseBoundsViaLinq()
        {
            Caribou.Processing.ParseViaLinq.GetBounds(ref results);
            CheckResult();
        }

        private void CheckResult()
        {
            Assert.AreEqual(expectedMinBounds, results.MinBounds);
            Assert.AreEqual(expectedMaxBounds, results.MaxBounds);
            Assert.AreEqual(expectedMinBounds.Latitude, results.MinBounds.Latitude);
            Assert.AreEqual(expectedMaxBounds.Longitude, results.MaxBounds.Longitude);
        }
    }
}
