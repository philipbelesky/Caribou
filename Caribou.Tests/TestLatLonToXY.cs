namespace Caribou.Tests
{
    using System.Collections.Generic;
    using System.Drawing;
    using Caribou.Data;
    using Caribou.Processing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestLatLonToXY
    {
        [TestMethod]
        public void TranslateLatLonToXY()
        {
            var mScale = 1.0;
            var mmScale = 1000.0;
            var testA = new Coord(-37.8161790, 144.9666110);
            var testB = new Coord(-37.8149890, 144.9658410);

            var result1 = TranslateToXY.GetXYFromLatLong(testA.Latitude, testA.Longitude, mScale);
            Assert.AreEqual(result1.Item1, 340.052469);
            Assert.AreEqual(result1.Item2, 26.797977);

            var result2 = TranslateToXY.GetXYFromLatLong(testA.Latitude, testA.Longitude, mmScale);
            Assert.AreEqual(result2.Item1, 340052.469047);
            Assert.AreEqual(result2.Item2, 26797.977322);

            var result3 = TranslateToXY.GetXYFromLatLong(testB.Latitude, testB.Longitude, mScale);
            Assert.AreEqual(result3.Item1, 272.410929);
            Assert.AreEqual(result3.Item2, 159.11994);

            var result4 = TranslateToXY.GetXYFromLatLong(testB.Latitude, testB.Longitude, mmScale);
            Assert.AreEqual(result4.Item1, 272410.929093);
            Assert.AreEqual(result4.Item2, 159119.940029);
        }
    }
}
