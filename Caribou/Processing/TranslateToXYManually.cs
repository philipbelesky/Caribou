namespace Caribou.Processing
{
    using System;
    using System.Collections.Generic;
    using Caribou.Models;
    using Caribou.Processing;
    using Grasshopper;
    using Grasshopper.Kernel.Data;
    using Rhino;
    using Rhino.Geometry;

    public static class TranslateToXYManually
    {
        public static Dictionary<OSMMetaData, List<Point3d>> NodePointsFromCoords(RequestHandler result)
        {
            var geometryResult = new Dictionary<OSMMetaData, List<Point3d>>();
            var unitScale = RhinoMath.UnitScale(UnitSystem.Meters, RhinoDoc.ActiveDoc.ModelUnitSystem); // OSM conversion assumes meters
            Coord lengthPerDegree = GetDegreesPerAxis(result.MinBounds, result.MaxBounds, unitScale);

            foreach (var entry in result.FoundData)
            {
                geometryResult[entry.Key] = new List<Point3d>();
                foreach (FoundItem item in entry.Value)
                {
                    var pt = GetPointFromLatLong(item.Coords[0], lengthPerDegree, result.MinBounds);
                    geometryResult[entry.Key].Add(pt);
                }
            }

            return geometryResult;
        }

        public static Dictionary<OSMMetaData, List<PolylineCurve>> WayPolylinesFromCoords(RequestHandler result)
        {
            var geometryResult = new Dictionary<OSMMetaData, List<PolylineCurve>>();
            var unitScale = RhinoMath.UnitScale(UnitSystem.Meters, RhinoDoc.ActiveDoc.ModelUnitSystem); // OSM conversion assumes meters
            Coord lengthPerDegree = GetDegreesPerAxis(result.MinBounds, result.MaxBounds, unitScale);

            foreach (var entry in result.FoundData)
            {
                geometryResult[entry.Key] = new List<PolylineCurve>();
                foreach (FoundItem item in entry.Value)
                {
                    var linePoints = new List<Point3d>();
                    foreach (var coord in item.Coords)
                    {
                        var pt = GetPointFromLatLong(coord, lengthPerDegree, result.MinBounds);
                        linePoints.Add(pt);
                    }

                    var polyLine = new PolylineCurve(linePoints); // Creating a polylinecurve from scratch makes invalid geometry
                    geometryResult[entry.Key].Add(polyLine);
                }
            }

            return geometryResult;
        }

        public static Dictionary<OSMMetaData, List<Surface>> BuildingSurfacesFromCoords(RequestHandler result)
        {
            var geometryResult = new Dictionary<OSMMetaData, List<Surface>>();
            var unitScale = RhinoMath.UnitScale(UnitSystem.Meters, RhinoDoc.ActiveDoc.ModelUnitSystem); // OSM conversion assumes meters
            Coord lengthPerDegree = GetDegreesPerAxis(result.MinBounds, result.MaxBounds, unitScale);

            foreach (var entry in result.FoundData)
            {
                geometryResult[entry.Key] = new List<Surface>();
                var lines = new List<PolylineCurve>();
                foreach (FoundItem item in entry.Value)
                {
                    var linePoints = new List<Point3d>();
                    foreach (var coord in item.Coords)
                    {
                        var pt = GetPointFromLatLong(coord, lengthPerDegree, result.MinBounds);
                        linePoints.Add(pt);
                    }

                    var polyLine = new PolylineCurve(linePoints); // Creating a polylinecurve from scratch makes invalid geometry
                    var height = IdentifyBuildingHeight.ParseHeight(item.Tags, unitScale);
                    if (height > 0.0)
                    {
                        if (polyLine.ClosedCurveOrientation() == CurveOrientation.Clockwise)
                            height *= -1; // If curve plane's Z != global Z; extrude in opposite direction

                        var surface = Extrusion.Create(polyLine, height, true);
                        geometryResult[entry.Key].Add(surface);
                    }
                }
            }
            return geometryResult;
        }

        // Thanks to Elk for this code!
        public static Coord GetDegreesPerAxis(Coord min, Coord max, double unitScale)
        {
            double meanEarthRadius = 6371000;
            // Get the median latitude value for evaluating the earth's radius at the location
            double averageLat = min.Latitude + ((max.Latitude - min.Latitude) / 2);
            double yLenPerDegree = ((Math.PI * meanEarthRadius) / 180) * unitScale;
            double xLenPerDegree = ((Math.PI * (Math.Cos((averageLat * Math.PI) / 180) * 6371000)) / 180) * unitScale;
            return new Coord(xLenPerDegree, yLenPerDegree);
        }

        public static Point3d GetPointFromLatLong(Coord ptCoord, Coord degreesPerCoord, Coord minExtends)
        {
            var x = (ptCoord.Longitude - minExtends.Longitude) * degreesPerCoord.Latitude;
            var y = (ptCoord.Latitude - minExtends.Latitude) * degreesPerCoord.Longitude;
            return new Point3d(x, y, 0);
        }
    }
}
