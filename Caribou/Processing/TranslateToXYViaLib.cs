namespace Caribou.Processing
{
    using System.Collections.Generic;
    using Caribou.Data;
    using GeoAPI.CoordinateSystems.Transformations;
    using Rhino;
    using Rhino.Geometry;

    public static class TranslateToXYViaLib
    {
        public static List<Point3d> NodePointsFromCoordsViaLib(RequestResults foundItems)
        {
            var results = new List<Point3d>();
            var unitScale = RhinoMath.UnitScale(UnitSystem.None, RhinoDoc.ActiveDoc.ModelUnitSystem);
            var unitName = RhinoDoc.ActiveDoc.ModelUnitSystem.ToString();
            var unitTrans = GetRhinoCoordinateSystem.GetTransformation(foundItems.ExtentsMin, unitScale);

            foreach (var featureType in foundItems.Nodes.Keys)
            {
                foreach (var subfeatureType in foundItems.Nodes[featureType].Keys)
                {
                    foreach (var coord in foundItems.Nodes[featureType][subfeatureType])
                    {
                        results.Add(GetPointFromLatLongViaLib(coord, unitTrans));
                    }
                }
            }

            return results;
        }

        public static List<Point3d> NodePointsFromCoords(RequestResults foundItems)
        {
            var results = new List<Point3d>();
            var unitScale = RhinoMath.UnitScale(UnitSystem.None, RhinoDoc.ActiveDoc.ModelUnitSystem);
            var unitTrans = GetRhinoCoordinateSystem.GetTransformation(foundItems.ExtentsMin, unitScale);

            foreach (var featureType in foundItems.Nodes.Keys)
            {
                foreach (var subfeatureType in foundItems.Nodes[featureType].Keys)
                {
                    foreach (var coord in foundItems.Nodes[featureType][subfeatureType])
                    {
                        results.Add(GetPointFromLatLongViaLib(coord, unitTrans));
                    }
                }
            }

            return results;
        }

        public static List<Polyline> WayPolylinesFromCoords(RequestResults foundItems)
        {
            List<Point3d> linePoints;
            var results = new List<Polyline>();

            foreach (var featureType in foundItems.Ways.Keys)
            {
                foreach (var subfeatureType in foundItems.Ways[featureType].Keys)
                {
                    foreach (var wayCoords in foundItems.Ways[featureType][subfeatureType])
                    {
                        linePoints = new List<Point3d>();
                        foreach (var coord in wayCoords)
                        {
                            //linePoints.Add(GetPointFromLatLong(coord, foundItems.extentsMin));
                        }

                        results.Add(new Polyline(linePoints));
                    }
                }
            }

            return results;
        }

        public static Point3d GetPointFromLatLongViaLib(Coord ptCoord, ICoordinateTransformation transformation)
        {
            double[] longLatWGS = new double[] { ptCoord.Latitude, ptCoord.Longitude }; // Note: LONG then LAT
            var eastNorthUTM = GetXYFromLatLon(longLatWGS, transformation); // Returns in Y-X
            return new Point3d(eastNorthUTM[0], eastNorthUTM[1], 0);
        }

        // Separated out mostly to enable unit testing (e.g. not require Rhinocommon)
        public static double[] GetXYFromLatLon(double[] latLon, ICoordinateTransformation transformation)
        {
            return transformation.MathTransform.Transform(latLon);
        }
    }
}
