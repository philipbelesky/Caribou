namespace Caribou.Processing
{
    using System.Collections.Generic;
    using Caribou.Data;
    using Rhino;
    using Rhino.Geometry;

    public class TranslateToXY
    {
        public static List<Point3d> NodePointsFromCoords(RequestResults foundItems)
        {
            var results = new List<Point3d>();
            var unitScale = RhinoMath.UnitScale(UnitSystem.None, RhinoDoc.ActiveDoc.ModelUnitSystem);

            foreach (var featureType in foundItems.Nodes.Keys)
            {
                foreach (var subfeatureType in foundItems.Nodes[featureType].Keys)
                {
                    foreach (var coord in foundItems.Nodes[featureType][subfeatureType])
                    {
                        results.Add(GetPointFromLatLong(coord, unitScale));
                    }
                }
            }

            return results;
        }

        public static List<Polyline> WayPolylinesFromCoords(RequestResults foundItems)
        {
            var linePoints = new List<Point3d>();
            var results = new List<Polyline>();
            var unitScale = RhinoMath.UnitScale(UnitSystem.None, RhinoDoc.ActiveDoc.ModelUnitSystem);

            foreach (var featureType in foundItems.Ways.Keys)
            {
                foreach (var subfeatureType in foundItems.Ways[featureType].Keys)
                {
                    foreach (var wayCoords in foundItems.Ways[featureType][subfeatureType])
                    {
                        linePoints = new List<Point3d>();
                        foreach (var coord in wayCoords)
                        {
                            linePoints.Add(GetPointFromLatLong(coord, unitScale));
                        }

                        results.Add(new Polyline(linePoints));
                    }
                }
            }

            return results;
        }

        public static Point3d GetPointFromLatLong(Coord coord, double unitScale)
        {
            var latlonprimitive = GetXYFromLatLong(coord.Latitude, coord.Longitude, unitScale);
            return new Point3d(latlonprimitive.Item1, latlonprimitive.Item2, 0);
        }

        // Separated out mostly to enable unit testing (e.g. not require Rhinocommon)
        public static (double, double) GetXYFromLatLong(double lat, double lon, double unitScale)
        {
            return (lat * unitScale, lon * unitScale);
        }
    }
}
