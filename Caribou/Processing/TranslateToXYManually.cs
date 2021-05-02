namespace Caribou.Processing
{
    using System.Collections.Generic;
    using Caribou.Data;
    using Rhino;
    using Rhino.Geometry;
    using System;

    public static class TranslateToXYManually
    {
        public static List<Point3d> NodePointsFromCoords(RequestResults foundItems)
        {
            var results = new List<Point3d>();
            var unitScale = RhinoMath.UnitScale(UnitSystem.Meters, RhinoDoc.ActiveDoc.ModelUnitSystem); // OSM conversion assumes meters
            Coord lengthPerDegree = GetDegreesPerAxis(foundItems.extentsMin, foundItems.extentsMax, unitScale);

            foreach (var featureType in foundItems.Nodes.Keys)
            {
                foreach (var subfeatureType in foundItems.Nodes[featureType].Keys)
                {
                    foreach (var coord in foundItems.Nodes[featureType][subfeatureType])
                    {
                        results.Add(GetPointFromLatLong(coord, lengthPerDegree, foundItems.extentsMin));
                    }
                }
            }

            return results;
        }

        public static List<Polyline> WayPolylinesFromCoords(RequestResults foundItems)
        {
            List<Point3d> linePoints;
            var results = new List<Polyline>();
            var unitScale = RhinoMath.UnitScale(UnitSystem.Meters, RhinoDoc.ActiveDoc.ModelUnitSystem);
            Coord lengthPerDegree = GetDegreesPerAxis(foundItems.extentsMin, foundItems.extentsMax, unitScale);

            foreach (var featureType in foundItems.Ways.Keys)
            {
                foreach (var subfeatureType in foundItems.Ways[featureType].Keys)
                {
                    foreach (var wayCoords in foundItems.Ways[featureType][subfeatureType])
                    {
                        linePoints = new List<Point3d>();
                        foreach (var coord in wayCoords)
                        {
                            linePoints.Add(GetPointFromLatLong(coord, lengthPerDegree, foundItems.extentsMin));
                        }

                        results.Add(new Polyline(linePoints));
                    }
                }
            }

            return results;
        }

        public static Coord GetDegreesPerAxis(Coord min, Coord max, double unitScale) { 
            // Thanks to Elk for this code!
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
