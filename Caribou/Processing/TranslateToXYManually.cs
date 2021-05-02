namespace Caribou.Processing
{
    using System;
    using System.Collections.Generic;
    using Caribou.Data;
    using Grasshopper;
    using Grasshopper.Kernel.Data;
    using Rhino;
    using Rhino.Geometry;

    public static class TranslateToXYManually
    {
        public static DataTree<Point3d> NodePointsFromCoords(RequestResults foundItems)
        {
            var results = new DataTree<Point3d>();
            var unitScale = RhinoMath.UnitScale(UnitSystem.Meters, RhinoDoc.ActiveDoc.ModelUnitSystem); // OSM conversion assumes meters
            Coord lengthPerDegree = GetDegreesPerAxis(foundItems.ExtentsMin, foundItems.ExtentsMax, unitScale);

            var i = 0;
            foreach (var featureType in foundItems.Nodes.Keys)
            {
                var j = 0;
                foreach (var subfeatureType in foundItems.Nodes[featureType].Keys)
                {
                    if (subfeatureType == FeatureRequest.SearchAllKey)
                    {
                        continue;
                    }

                    GH_Path subfeaturePath = new GH_Path(i, j);
                    results.EnsurePath(subfeaturePath);
                    j++;

                    foreach (var coord in foundItems.Nodes[featureType][subfeatureType])
                    {
                        var pt = GetPointFromLatLong(coord, lengthPerDegree, foundItems.ExtentsMin);
                        results.Add(pt, subfeaturePath);
                    }
                }

                i++;
            }

            return results;
        }

        public static DataTree<Polyline> WayPolylinesFromCoords(RequestResults foundItems)
        {
            List<Point3d> linePoints;
            var results = new DataTree<Polyline>();
            var unitScale = RhinoMath.UnitScale(UnitSystem.Meters, RhinoDoc.ActiveDoc.ModelUnitSystem);
            Coord lengthPerDegree = GetDegreesPerAxis(foundItems.ExtentsMin, foundItems.ExtentsMax, unitScale);

            var i = 0;
            foreach (var featureType in foundItems.Ways.Keys)
            {
                var j = 0;
                foreach (var subfeatureType in foundItems.Ways[featureType].Keys)
                {
                    if (subfeatureType == FeatureRequest.SearchAllKey)
                    {
                        continue;
                    }

                    GH_Path subfeaturePath = new GH_Path(i, j);
                    results.EnsurePath(subfeaturePath);
                    j++;

                    foreach (var wayCoords in foundItems.Ways[featureType][subfeatureType])
                    {
                        linePoints = new List<Point3d>();
                        foreach (var coord in wayCoords)
                        {
                            linePoints.Add(GetPointFromLatLong(coord, lengthPerDegree, foundItems.ExtentsMin));
                        }

                        var crv = new Polyline(linePoints);
                        results.Add(crv, subfeaturePath);
                    }
                }

                i++;
            }

            return results;
        }

        public static Coord GetDegreesPerAxis(Coord min, Coord max, double unitScale)
        {
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
