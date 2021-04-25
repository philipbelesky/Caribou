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
        private double mScale = 1.0;
        private double mmScale = 1000.0;
        private Coord testMinBounds = new Coord(-37.8164200, 144.9627400); // melbourne.xml
        private Coord testMaxBounds = new Coord(-37.8089200, 144.9710600); // melbourne.xml

        [TestMethod]
        public void GetUTMZones()
        {
            int zone;
            zone = GetRhinoCoordinateSystem.GetUTMZone(144.1);
            Assert.AreEqual(55, zone);
            zone = GetRhinoCoordinateSystem.GetUTMZone(-66);
            Assert.AreEqual(20, zone);
            zone = GetRhinoCoordinateSystem.GetUTMZone(102.1);
            Assert.AreEqual(48, zone);
        }

        [TestMethod]
        public void TranslateLatLonToXYm()
        {
            var pt = new double[] { -37.8161790, 144.9666110 }; // nodeID 2102129133
            var expectedPT = new double[] { 340.052469, 26.797977 };
            var transformation = GetRhinoCoordinateSystem.GetTransformation(testMinBounds, mScale, "m");
            var result = TranslateToXY.GetXYFromLatLon(pt, transformation);
            Assert.AreEqual(expectedPT, pt);
        }

        [TestMethod]
        public void TranslateLatLonToXYmm()
        {
            var pt = new double[] { -37.8161790, 144.9666110 }; // nodeID 2102129133
            var expectedPT = new double[] { 340052.469047, 26797.977322 };
            var transformation = GetRhinoCoordinateSystem.GetTransformation(testMinBounds, mmScale, "mm");
            var result = TranslateToXY.GetXYFromLatLon(pt, transformation);
            Assert.AreEqual(expectedPT, pt);
        }

        //[TestMethod]
        //public void TranslateLatLonToXYB()
        //{
        //    var pt = new Coord(-37.8149890, 144.9658410); // nodeID 3877497258
        //    var d = TranslateToXY.GetDistanceForLatLong(testMinBounds, testMaxBounds, mScale);

        //    var result3 = TranslateToXY.GetXYFromLatLon(pt.Latitude, pt.Longitude, testMinBounds, d);
        //    Assert.AreEqual(result3.Item1, 272.410929);
        //    Assert.AreEqual(result3.Item2, 159.11994);

        //    var result4 = TranslateToXY.GetXYFromLatLon(pt.Latitude, pt.Longitude, testMinBounds, d);
        //    Assert.AreEqual(result4.Item1, 272410.929093);
        //    Assert.AreEqual(result4.Item2, 159119.940029);
        //}
    }
}
