namespace Caribou.Tests.Models
{
    using System.Globalization;
    using System.Threading;
    using Caribou.Models;
    using Caribou.Tests.Cases;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestSingleBoundsParsing : MelbourneCase
    {
        private Coord expectedMinBounds = new Coord(-37.8164200, 144.9127400);
        private Coord expectedMaxBounds = new Coord(-37.2164200, 144.9710600);
        private RequestHandler results = new RequestHandler(OSMXMLs, mainFeatures, 
                                                            OSMGeometryType.Node, reportProgress, "Test");

        [TestMethod]
        public void ParseBoundsViaXMLReader()
        {
            Caribou.Processing.ParseViaXMLReader.GetBounds(ref results, true);
            CheckResult();
        }

        [TestMethod]
        public void ParseBoundsViaXMLReaderLocaleA()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);
            Caribou.Processing.ParseViaXMLReader.GetBounds(ref results, true);
            CheckResult();
        }

        [TestMethod]
        public void ParseBoundsViaXMLReaderLocaleB()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-FR", false);
            Caribou.Processing.ParseViaXMLReader.GetBounds(ref results, true);
            CheckResult();
        }

        [TestMethod]
        public void ParseBoundsViaXMLReaderLocaleC()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("it-IT", false);
            Caribou.Processing.ParseViaXMLReader.GetBounds(ref results, true);
            CheckResult();
        }

        [TestMethod]
        public void ParseBoundsViaXMLReaderLocaleD()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-Hans", false);
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
