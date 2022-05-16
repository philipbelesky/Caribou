namespace Caribou.Tests.Parsing
{
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Caribou.Processing;

    [TestClass]
    public class TestHeightParsing
    {
        [TestMethod]
        public void TestValidHeighTagOptions()
        {
            var mPerL = InterpretHeights.METERS_PER_LEVEL;

            var a = new Dictionary<string, string>() { { "building:level", "2" } };
            Assert.AreEqual(InterpretHeights.ParseHeight(a, 1), 2 * mPerL);

            var b = new Dictionary<string, string>() { { "height", "7.5" } };
            Assert.AreEqual(InterpretHeights.ParseHeight(b, 1), 7.5);

            var c = new Dictionary<string, string>() { { "height", "28.7 " } };
            Assert.AreEqual(InterpretHeights.ParseHeight(c, 1), 28.7);

            var d = new Dictionary<string, string>() { { "building:height", "111" } };
            Assert.AreEqual(InterpretHeights.ParseHeight(d, 1), 111);

            var e = new Dictionary<string, string>() { { "levels", "77" } };
            Assert.AreEqual(InterpretHeights.ParseHeight(e, 1), 77 * mPerL);

            var f = new Dictionary<string, string>() { { "stories", "24" } };
            Assert.AreEqual(InterpretHeights.ParseHeight(f, 1), 24 * mPerL);

            var g = new Dictionary<string, string>() { { "height", "7'4\"" } };
            Assert.AreEqual(InterpretHeights.ParseHeight(g, 1), 2.2352);

            var h = new Dictionary<string, string>() { { "height", "22'" } };
            Assert.AreEqual(InterpretHeights.ParseHeight(h, 1), 22 * InterpretHeights.FT_TO_M);

            var i = new Dictionary<string, string>() { { "height", "11' 4\"" } };
            var expected = (11 * InterpretHeights.FT_TO_M) + (4 * InterpretHeights.INCHES_TO_M);
            Assert.AreEqual(InterpretHeights.ParseHeight(i, 1), expected);

            var j = new Dictionary<string, string>() { { "height", "15 m'" } };
            Assert.AreEqual(InterpretHeights.ParseHeight(h, 1), 15 * mPerL);
        }

        [TestMethod]
        public void TestInValidHeighTagOptions()
        {
            var a = new Dictionary<string, string>() { { "building:height", "xyz" } };
            Assert.AreEqual(InterpretHeights.ParseHeight(a, 1), 0.0);

            var b = new Dictionary<string, string>() { { "height", "0.5;25" } };
            Assert.AreEqual(InterpretHeights.ParseHeight(b, 1), 0.0);
        }
    }
}
