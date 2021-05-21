namespace Caribou.Tests
{
    using System.Collections.Generic;
    using System.Drawing;
    using Caribou.Data;
    using Caribou.Processing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestLatLonViaLibToXY
    {
        private double mScale = 1.0;
        private double mmScale = 0.001;
        private Coord testMinBounds = new Coord(-37.8164200, 144.9627400); // melbourne.xml
        private Coord testMaxBounds = new Coord(-37.8089200, 144.9710600); // melbourne.xml

        //[TestMethod]
        //public void GetUTMZones()
        //{
        //    int zone;
        //    zone = GetRhinoCoordinateSystem.GetUTMZone(-37.8, 144.9);
        //    Assert.AreEqual(55, zone);
        //    zone = GetRhinoCoordinateSystem.GetUTMZone(0.1, 6.1);
        //    Assert.AreEqual(32, zone);
        //    zone = GetRhinoCoordinateSystem.GetUTMZone(-80.1, -60.1);
        //    Assert.AreEqual(20, zone);
        //}

        //[TestMethod]
        //public void SimpleNHemisphere()
        //{
        //    var pt = new double[] { 4.296545, 50.880324 }; // LON - LAT
        //    var expectedXY = new double[] { 591210.304913748, 5637317.31895618 }; // East-North
        //    var bounds = new Coord(50.1, 4.1);
        //    var result = GetRhinoCoordinateSystem.MostSimpleExample(bounds, pt);

        //    Assert.AreEqual(expectedXY[0], result[0], 0.01);
        //    Assert.AreEqual(expectedXY[1], result[1], 0.01);
        //}

        //[TestMethod]
        //public void SimpleSHemisphere()
        //{
        //    var pt = new double[] { 144.9710600, -37.8089200 }; // LON - LAT
        //    var expectedXY = new double[] { 321393.91, 5813446.27 }; // East-North
        //    var bounds = new Coord(-37.8, 144.9);
        //    var result = GetRhinoCoordinateSystem.MostSimpleExample(bounds, pt);

        //    Assert.AreEqual(expectedXY[0], result[0], 0.01);
        //    Assert.AreEqual(expectedXY[1], result[1], 0.01);
        //}

        //[TestMethod]
        //public void TranslateLatLonToXYWithoutBoundsM()
        //{
        //    var pt = new double[] { 144.9710600, -37.8089200 }; // LON - LAT
        //    var expectedXY = new double[] { 321393.91, 5813446.27 }; // East-North
        //    var bounds = new Coord(-37.8, 144.9); // 0 lat to test against absolute ref

        //    var transformation = GetRhinoCoordinateSystem.GetTransformation(testMinBounds, mScale, true);
        //    var result = TranslateToXYViaLib.GetXYFromLatLon(pt, transformation);
        //    Assert.AreEqual(expectedXY[0], result[0], 0.01); // If origin not changed: 321393.90983082756, 5813446.2700856943
        //    Assert.AreEqual(expectedXY[1], result[1], 0.01);
        //}

        //[TestMethod]
        //public void TranslateLatLonToXYWithoutBoundsMM()
        //{
        //    var pt = new double[] { 144.9710600, -37.8089200 }; // LON - LAT
        //    var expectedXY = new double[] { 321393000.91, 5813446000.27 }; // East-North
        //    var bounds = new Coord(-37.8, 144.9); // 0 lat to test against absolute ref

        //    var transformation = GetRhinoCoordinateSystem.GetTransformation(bounds, mmScale, true);
        //    var result = TranslateToXYViaLib.GetXYFromLatLon(pt, transformation);
        //    Assert.AreEqual(expectedXY[0], result[0], 1000); // If origin not changed: 321393.90983082756, 5813446.2700856943
        //    Assert.AreEqual(expectedXY[1], result[1], 1000);
        //}

        // Disabled as not working
        // Providing a boundary and translating to XY at the boundary should return (0,0,0)
        // But the construction off the offsets in the UTM conversion is not working
        //[TestMethod]
        //public void TranslateLatLonToXYWithBoundsMAtZero()
        //{
        //    var pt = new double[] { -37.8164200, 144.9627400 }; // min bounds for Mel
        //    var expectedPT = new double[] { 0, 0 };

        //    var transformation = GetRhinoCoordinateSystem.GetTransformation(testMinBounds, mScale);
        //    var result = TranslateToXYViaLib.GetXYFromLatLon(pt, transformation);
        //    Assert.AreEqual(expectedPT[0], result[0]);
        //    Assert.AreEqual(expectedPT[1], result[1]);
        //}

        //[TestMethod]
        //public void TranslateLatLonToXYWithBoundsM()
        //{
        //    var pt = new double[] { -37.8161790, 144.9666110 }; // nodeID 2102129133
        //    var expectedPT = new double[] { 714.33, 848.19 };

        //    var transformation = GetRhinoCoordinateSystem.GetTransformation(testMinBounds, mScale);
        //    var result = TranslateToXYViaLib.GetXYFromLatLon(pt, transformation);
        //    Assert.AreEqual(expectedPT[0], result[0]);
        //    Assert.AreEqual(expectedPT[1], result[1]);
        //}
    }
}
