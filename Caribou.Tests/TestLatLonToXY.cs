namespace Caribou.Tests
{
    using System.Collections.Generic;
    using System.Drawing;
    using Caribou.Data;
    using Caribou.Processing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    //[TestClass]
    //public class TestLatLonToXY
    //{
    //    private readonly double mScale = 1.0;
    //    private readonly double mmScale = 1000;
    //    private double[] result;
    //    private readonly Coord testMinBounds = new Coord(-37.8164200, 144.9627400); // simple.xml
    //    private readonly Coord testMaxBounds = new Coord(-37.8089200, 144.9710600); // simple.xml

    //    private readonly Coord simpleAmenityNode = new Coord(-37.8134515, 144.9689196);
    //    private readonly double[] expectedAmenityNode = new double[] { 542.854104293, 330.082139745, 0 };

    //    private readonly Coord simpleCraftNode = new Coord(-37.8149890, 144.9658410);
    //    private readonly double[] expectedCraftNode = new double[] { 272.410929093, 159.119940029, 0 };

    //    // This is to avoid using RhinoCommon; it replaces GetPointFromLatLong in TranslateToXYManually
    //    private static double[] MockGetPointFromLatLong(Coord ptCoord, Coord degreesPerCoord, Coord minExtends)
    //    {
    //        var x = (ptCoord.Longitude - minExtends.Longitude) * degreesPerCoord.Latitude;
    //        var y = (ptCoord.Latitude - minExtends.Latitude) * degreesPerCoord.Longitude;
    //        return new double[] { x, y, 0 };
    //    }

    //    [TestMethod]
    //    public void TranslateLatLonToXYInM()
    //    {
    //        var lengthPerDegree = TranslateToXYManually.GetDegreesPerAxis(testMinBounds, testMaxBounds, mScale);

    //        result = MockGetPointFromLatLong(simpleAmenityNode, lengthPerDegree, testMinBounds);
    //        Assert.AreEqual(expectedAmenityNode[0] * mScale, result[0], 0.01);
    //        Assert.AreEqual(expectedAmenityNode[1] * mScale, result[1], 0.01);

    //        result = MockGetPointFromLatLong(simpleCraftNode, lengthPerDegree, testMinBounds);
    //        Assert.AreEqual(expectedCraftNode[0] * mScale, result[0], 0.01);
    //        Assert.AreEqual(expectedCraftNode[1] * mScale, result[1], 0.01);
    //    }

    //    [TestMethod]
    //    public void TranslateLatLonToXYInMM()
    //    {
    //        var lengthPerDegree = TranslateToXYManually.GetDegreesPerAxis(testMinBounds, testMaxBounds, mmScale);

    //        result = MockGetPointFromLatLong(simpleAmenityNode, lengthPerDegree, testMinBounds);
    //        Assert.AreEqual(expectedAmenityNode[0] * mmScale, result[0], 0.01);
    //        Assert.AreEqual(expectedAmenityNode[1] * mmScale, result[1], 0.01);

    //        result = MockGetPointFromLatLong(simpleCraftNode, lengthPerDegree, testMinBounds);
    //        Assert.AreEqual(expectedCraftNode[0] * mmScale, result[0], 0.01);
    //        Assert.AreEqual(expectedCraftNode[1] * mmScale, result[1], 0.01);
    //    }
    //}
}
