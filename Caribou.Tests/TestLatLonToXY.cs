namespace Caribou.Tests
{
    using System.Collections.Generic;
    using Caribou.Processing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rhino.Geometry;

    [TestClass]
    public class TestLatLonToXY
    {
        [TestMethod]
        public void ParseNodeCoordsToXY()
        {
            var testA = new Coord(-37.8113371, 144.9700479);
            var resultA = new Point3d(999, 999, 0);            
        }

        [TestMethod]
        public void ParseWayCoordsToXY()
        {
            var testA = new List<Coord>() {
                new Coord(-37.8113371, 144.9700479),
            };
            var resultA = new List<Point3d>() {
                new Point3d(999, 999, 0),
            };
        }
    }
}
