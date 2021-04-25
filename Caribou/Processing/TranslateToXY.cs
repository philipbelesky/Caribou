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
            var distanceForLatLon = GetDistanceForLatLong(foundItems.extentsMin, foundItems.extentsMax, unitScale);

            foreach (var featureType in foundItems.Nodes.Keys)
            {
                foreach (var subfeatureType in foundItems.Nodes[featureType].Keys)
                {
                    foreach (var coord in foundItems.Nodes[featureType][subfeatureType])
                    {
                        results.Add(GetPointFromLatLong(coord, foundItems.extentsMin, distanceForLatLon));
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
            var distanceForLatLon = GetDistanceForLatLong(foundItems.extentsMin, foundItems.extentsMax, unitScale);

            foreach (var featureType in foundItems.Ways.Keys)
            {
                foreach (var subfeatureType in foundItems.Ways[featureType].Keys)
                {
                    foreach (var wayCoords in foundItems.Ways[featureType][subfeatureType])
                    {
                        linePoints = new List<Point3d>();
                        foreach (var coord in wayCoords)
                        {
                            linePoints.Add(GetPointFromLatLong(coord, foundItems.extentsMin, distanceForLatLon));
                        }

                        results.Add(new Polyline(linePoints));
                    }
                }
            }

            return results;
        }

        public static (double, double) GetDistanceForLatLong(Coord minLatLon, Coord maxLatLon, double unitScale)
        {
            double earthRadius = 6371000.0;
            double centerLat = minLatLon.Latitude + (maxLatLon.Latitude * 0.5);

            double a = System.Math.Cos(centerLat * System.Math.PI / 180);
            double distanceForLatDegree = (System.Math.PI * a * earthRadius) / 180;
            double distanceForLonDegree = (System.Math.PI * earthRadius) / 180;

            return (distanceForLatDegree * unitScale, distanceForLonDegree * unitScale);
        }

        public static Point3d GetPointFromLatLong(Coord ptCoord, Coord minLatlLon, (double, double) distanceForLatLon)
        {
            var xy = GetXYFromLatLon(ptCoord.Latitude, ptCoord.Longitude, minLatlLon, distanceForLatLon);
            return new Point3d(xy.Item1, xy.Item2, 0);
        }

        // Separated out mostly to enable unit testing (e.g. not require Rhinocommon)
        public static (double, double) GetXYFromLatLon(double lat, double lon, Coord minLatlLon, (double, double) distanceForLatLon)
        {
            var y = (lat - minLatlLon.Latitude) * distanceForLatLon.Item1;
            var x = (lon - minLatlLon.Longitude) * distanceForLatLon.Item2;
            return (x, y);
        }
    }
}
