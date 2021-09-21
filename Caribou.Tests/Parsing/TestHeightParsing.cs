namespace Caribou.Tests.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Caribou.Processing;

    [TestClass]
    public class TestHeightParsing
    {

        [TestMethod]
        public void TestHeighTagOptions()
        {
            var mPerL = IdentifyBuildingHeight.METERS_PER_LEVEL;

            var a = new Dictionary<string, string>() { { "building:level", "2" } };
            Assert.AreEqual(IdentifyBuildingHeight.ParseHeight(a, 1), 2 * mPerL);

            var b = new Dictionary<string, string>() { { "height", "7.5" } };
            Assert.AreEqual(IdentifyBuildingHeight.ParseHeight(b, 1), 7.5);

            var c = new Dictionary<string, string>() { { "height", "28.7 " } };
            Assert.AreEqual(IdentifyBuildingHeight.ParseHeight(c, 1), 28.7);

            var d = new Dictionary<string, string>() { { "building:height", "111" } };
            Assert.AreEqual(IdentifyBuildingHeight.ParseHeight(d, 1), 111);

            var e = new Dictionary<string, string>() { { "levels", "77" } };
            Assert.AreEqual(IdentifyBuildingHeight.ParseHeight(e, 1), 77 * mPerL);

            var f = new Dictionary<string, string>() { { "stories", "24" } };
            Assert.AreEqual(IdentifyBuildingHeight.ParseHeight(f, 1), 24 * mPerL);

            var g = new Dictionary<string, string>() { { "height", "7'4\"" } };
            Assert.AreEqual(IdentifyBuildingHeight.ParseHeight(g, 1), 2.2352);

            var h = new Dictionary<string, string>() { { "height", "22'" } };
            Assert.AreEqual(IdentifyBuildingHeight.ParseHeight(h, 1), 22 * IdentifyBuildingHeight.FT_TO_M);

            var i = new Dictionary<string, string>() { { "height", "11' 4\"" } };
            var expected = (11 * IdentifyBuildingHeight.FT_TO_M) + (4 * IdentifyBuildingHeight.INCHES_TO_M);
            Assert.AreEqual(IdentifyBuildingHeight.ParseHeight(i, 1), expected);
        }
    }
}
