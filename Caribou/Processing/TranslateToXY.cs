namespace Caribou.Processing
{
    using System.Collections.Generic;
    using Caribou.Data;
    using Rhino;
    using Rhino.Geometry;
    using ProjNet.CoordinateSystems.Transformations;
    using GeoAPI.CoordinateSystems.Transformations;
    using System;
    using ProjNet.CoordinateSystems;

    public class TranslateToXY
    {
        public static List<Point3d> NodePointsFromCoords(RequestResults foundItems)
        {
            var results = new List<Point3d>();
            var unitScale = RhinoMath.UnitScale(UnitSystem.None, RhinoDoc.ActiveDoc.ModelUnitSystem);
            var unitName = RhinoDoc.ActiveDoc.ModelUnitSystem.ToString();
            var unitTrans = GetRhinoCoordinateSystem.GetTransformation(foundItems.extentsMin, unitScale, unitName);

            foreach (var featureType in foundItems.Nodes.Keys)
            {
                foreach (var subfeatureType in foundItems.Nodes[featureType].Keys)
                {
                    foreach (var coord in foundItems.Nodes[featureType][subfeatureType])
                    {
                        results.Add(GetPointFromLatLong(coord, unitTrans));
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

        //public static (double, double) GetDistanceForLatLong(Coord minLatLon, Coord maxLatLon, double unitScale)
        //{
        //    double earthRadius = 6371000.0;
        //    double centerLat = minLatLon.Latitude + (maxLatLon.Latitude * 0.5);

        //    double a = System.Math.Cos(centerLat * System.Math.PI / 180);
        //    double distanceForLatDegree = (System.Math.PI * a * earthRadius) / 180;
        //    double distanceForLonDegree = (System.Math.PI * earthRadius) / 180;

        //    return (distanceForLatDegree * unitScale, distanceForLonDegree * unitScale);
        //}

        public static Point3d GetPointFromLatLong(Coord ptCoord, ICoordinateTransformation transformation)
        {
            double[] latLonCoord = new double[] { ptCoord.Latitude, ptCoord.Longitude };
            var xy = GetXYFromLatLon(latLonCoord, transformation);
  
            return new Point3d(xy[0], xy[1], 0);
        }

        //// Separated out mostly to enable unit testing (e.g. not require Rhinocommon)
        public static double[] GetXYFromLatLon(double[] latlonCoord, ICoordinateTransformation transformation)
        {
            return transformation.MathTransform.Transform(latlonCoord);
        }
    }
}
