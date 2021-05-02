namespace Caribou.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Caribou.Data;
    using Caribou.Processing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestBoundsParsing : MelbourneCase
    {
        private (Coord, Coord) boundsLatLon = (
            new Coord(-37.8164200, 144.9627400), 
            new Coord(-37.8089200, 144.9710600)
        );

        [TestMethod]
        public void ParseBoundsViaXMLReader()
        {
            var matches = Caribou.Processing.ParseViaXMLReader.FindByFeatures(miscSubFeatures, melbourneFile);
            Assert.AreEqual(boundsLatLon.Item1, matches.ExtentsMin);
            Assert.AreEqual(boundsLatLon.Item2, matches.ExtentsMax);
            Assert.AreEqual(boundsLatLon.Item1.Latitude, matches.ExtentsMin.Latitude);
        }

        [TestMethod]
        public void ParseBoundsViaXMLDocument()
        {
            var matches = Caribou.Processing.ParseViaXMLDocument.FindByFeatures(miscSubFeatures, melbourneFile);
            Assert.AreEqual(boundsLatLon.Item1, matches.ExtentsMin);
            Assert.AreEqual(boundsLatLon.Item2, matches.ExtentsMax);
            Assert.AreEqual(boundsLatLon.Item1.Latitude, matches.ExtentsMin.Latitude);
        }

        [TestMethod]
        public void ParseBoundsViaLinq()
        {
            var matches = Caribou.Processing.ParseViaLinq.FindByFeatures(miscSubFeatures, melbourneFile);
            Assert.AreEqual(boundsLatLon.Item1, matches.ExtentsMin);
            Assert.AreEqual(boundsLatLon.Item2, matches.ExtentsMax);
            Assert.AreEqual(boundsLatLon.Item1.Latitude, matches.ExtentsMin.Latitude);
        }
    }
}
