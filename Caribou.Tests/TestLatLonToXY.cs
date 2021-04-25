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
            zone = GetRhinoCoordinateSystem.GetUTMZone(-37.8, 144.9);
            Assert.AreEqual(55, zone);
            zone = GetRhinoCoordinateSystem.GetUTMZone(0.1, 6.1);
            Assert.AreEqual(32, zone);
            zone = GetRhinoCoordinateSystem.GetUTMZone(-80.1, -60.1);
            Assert.AreEqual(20, zone);
        }

        [TestMethod]
        public void TranslateLatLonToXYProvidedNHemisphere()
        {
            var pt = new double[] { 4.296545, 50.880324 }; // LONG-LAT
            var expectedXY = new double[] { 591210.304913748, 5637317.31895618 }; // X-Y
            var bounds = new Coord( 50.1, 4.1);
            var result = GetRhinoCoordinateSystem.MostSimpleExample(bounds, pt);

            Assert.AreEqual(expectedXY[0], result[0], 0.0001);
            Assert.AreEqual(expectedXY[1], result[1], 0.0001);
        }

        [TestMethod]
        public void TranslateLatLonToXYProvidedSHemisphere()
        {
            var pt = new double[] { 144.9710600, -37.8089200 }; // LONG-LAT
            var expectedXY = new double[] { 3894936.459223728, -15057560.845605332 }; // X-Y
            var bounds = new Coord(-37.8, 144.9);
            var result = GetRhinoCoordinateSystem.MostSimpleExample(bounds, pt);

            Assert.AreEqual(expectedXY[0], result[0], 0.0001);
            Assert.AreEqual(expectedXY[1], result[1], 0.0001);
        }

        //[TestMethod]
        //public void TranslateLatLonToXYWithoutBounds()
        //{
        //    var pt = new double[] { -37.8161790, 144.9666110 }; // nodeID 2102129133
        //    var expectedXY = new double[] { 3894941.5147579345, -15056535.280682813 };
        //    var bounds = new Coord(-37, 144);

        //    var transformation = GetRhinoCoordinateSystem.MostSimpleExample(bounds);
        //    var result = TranslateToXY.GetXYFromLatLon(pt, transformation);
        //    Assert.AreEqual(expectedXY[0], result[0]);
        //    Assert.AreEqual(expectedXY[1], result[1]);
        //}

        //[TestMethod]
        //public void TranslateLatLonToXYm()
        //{
        //    var pt = new double[] { -37.8161790, 144.9666110 }; // nodeID 2102129133
        //    var expectedPT = new double[] { 340.052469, 26.797977 };
        //    var transformation = GetRhinoCoordinateSystem.GetTransformation(testMinBounds, mScale, "m");
        //    var result = TranslateToXY.GetXYFromLatLon(pt, transformation);
        //    Assert.AreEqual(expectedPT[0], result[0]);
        //    Assert.AreEqual(expectedPT[1], result[1]);
        //}

        //[TestMethod]
        //public void TranslateLatLonToXYmm()
        //{
        //    var pt = new double[] { -37.8161790, 144.9666110 }; // nodeID 2102129133
        //    var expectedPT = new double[] { 340052.469047, 26797.977322 };
        //    var transformation = GetRhinoCoordinateSystem.GetTransformation(testMinBounds, mmScale, "mm");
        //    var result = TranslateToXY.GetXYFromLatLon(pt, transformation);
        //    Assert.AreEqual(expectedPT[0], result[0]);
        //    Assert.AreEqual(expectedPT[1], result[1]);
        //}

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
