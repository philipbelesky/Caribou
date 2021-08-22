namespace Caribou.Processing
{
    using System;
    using System.Collections.Generic;
    using Caribou.Models;
    using Rhino;
    using Rhino.Geometry;

    public static class GetOSMBoundaries
    {
        public static List<Rectangle3d> GetBoundariesFromResult(RequestHandler result)
        {
            var unitScale = RhinoMath.UnitScale(UnitSystem.Meters, RhinoDoc.ActiveDoc.ModelUnitSystem);
            Coord lengthPerDegree = TranslateToXYManually.GetDegreesPerAxis(result.MinBounds, result.MaxBounds, unitScale);

            var boundaries = new List<Rectangle3d>();
            for (var i = 0; i < result.AllBounds.Count; i++)
            {
                var lowerCoord = result.AllBounds[i].Item1;
                var upperCoord = result.AllBounds[i].Item2;
                var lowerLeft = TranslateToXYManually.GetPointFromLatLong(lowerCoord, lengthPerDegree, result.MinBounds);
                var upperRight = TranslateToXYManually.GetPointFromLatLong(upperCoord, lengthPerDegree, result.MinBounds);
                var boundary = new Rectangle3d(Plane.WorldXY, lowerLeft, upperRight);
                boundaries.Add(boundary);
            }
            return boundaries;
        }
    }
}
