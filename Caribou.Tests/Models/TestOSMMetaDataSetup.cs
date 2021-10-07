namespace Caribou.Tests.Models
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Caribou.Models;
    using System.Collections.Generic;

    [TestClass]
    public class TestOSMMetaDataSetup
    {

        [TestMethod]
        public void TestPrimaryKeyParsing() // E.g. those defined in OSMDefinedFeatures at Primary lvl
        {
            var highwaysParentA = new OSMTag("highway");
            var highwaysParentB = new OSMTag("highway=*");
            var highwaysParentC = new OSMTag("highway", "");
            var highwaysParentD = new OSMTag("highway", "Highway", "Roads", null);

            foreach (var item in new List<OSMTag> { highwaysParentA, highwaysParentB, highwaysParentC, highwaysParentD })
            {
                Assert.AreEqual(item.ToString(), "highway=*");
                Assert.AreEqual(item.IsParent(), true);
                Assert.AreNotEqual(item.Description, "");
            }
        }

        [TestMethod]
        public void TestPrimaryKeyValueParsing() // E.g. defined subfeatures with a primary feature
        {
            var highwaysParentA = new OSMTag("highway");

            var highwaysChildA = new OSMTag("highway=residential");
            Assert.IsTrue(highwaysChildA.WayCount > 0);
            var highwaysChildB = new OSMTag("highway", "residential");
            Assert.IsTrue(highwaysChildB.WayCount > 0);
            var highwaysChildC = new OSMTag("residential", "Highway", "Roads", highwaysParentA);

            foreach (var item in new List<OSMTag> { highwaysChildA, highwaysChildB, highwaysChildC })
            {
                Assert.AreEqual(item.ToString(), "highway=residential");
                Assert.AreEqual(item.IsParent(), false);
                Assert.AreNotEqual(item.Description, "");
                Assert.AreEqual(item.Key, highwaysParentA);
            }
        }

        [TestMethod]
        public void TestKeyParsing() // E.g. those defined in MiscFeatures at Primary lvl
        {
            var randomA = new OSMTag("rental"); 
            Assert.AreEqual(randomA.ToString(), "rental=*");

            var randomB = new OSMTag("passenger"); 
            Assert.AreEqual(randomB.ToString(), "passenger=*");

            foreach (var item in new List<OSMTag> { randomA, randomB })
            {
                Assert.AreEqual(item.IsParent(), true);
            }
        }

        [TestMethod]
        public void TestKeyValueParsingA() // E.g. defined subfeatures without a primary feature
        {
            var miscA = new OSMTag("capacity=charging");
            Assert.AreEqual(miscA.ToString(), "capacity=charging");
            Assert.AreEqual(miscA.IsParent(), false);

            Assert.AreEqual(miscA.Key.ToString(), "capacity=*");
            Assert.AreEqual(miscA.Key.IsParent(), true);
        }

        [TestMethod]
        public void TestKeyValueParsingB()
        {
            var websiteParentA = new OSMTag("website");
            var websiteParentB = new OSMTag("website=*");
            var websiteParentC = new OSMTag("website", "*");

            foreach (var item in new List<OSMTag> { websiteParentA, websiteParentB, websiteParentC })
            {
                Assert.AreEqual(item.ToString(), "website=*");
                Assert.AreEqual(item.Name, "Website");
                Assert.AreEqual(item.Key, null);
            }
        }

        [TestMethod]
        public void TestKeyValueParsingC()
        {
            var websiteParentA = new OSMTag("website");

            var websiteChildA = new OSMTag("website=http://www.google.com");
            var websiteChildB = new OSMTag("website", "http://www.google.com");

            foreach (var item in new List<OSMTag> { websiteChildA, websiteChildB })
            {
                Assert.AreEqual(item.ToString(), "website=http://www.google.com");
                Assert.AreEqual(item.Key, websiteParentA);
            }
        }
    }
}
