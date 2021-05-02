namespace Caribou.Processing
{
    using System;
    using Caribou.Data;
    using ProjNet.CoordinateSystems.Transformations;
    using ProjNet.CoordinateSystems;
    using System.Collections.Generic;
    using ProjNet.Converters.WellKnownText;
    using ProjNet.CoordinateSystems;
    using ProjNet.CoordinateSystems.Transformations;
    using GeoAPI.CoordinateSystems;
    using GeoAPI.CoordinateSystems.Transformations;

    public static class GetRhinoCoordinateSystem
    {
        public static ICoordinateTransformation GetTransformation(Coord min, double unitScale, bool useUTMBounds = false)
        {
            // Customise our TO coordinate system's origin given bounds of OSM file
            var zoneIsNorth = min.Latitude > 0.0;
            int utmZone = GetUTMZone(min.Latitude, min.Longitude);
            var latitudeOfOrigin = useUTMBounds ? 0 : min.Latitude;
            var centralMeridian = useUTMBounds ? (utmZone * 6) - 183 : min.Longitude; // UTMBounds mostly for testing

            // Setup our TO coordinate system (UTM but with modified units/origin to match Rhino)
            var pcs_UTM = AlterStandardWGS(utmZone, zoneIsNorth, centralMeridian, latitudeOfOrigin, unitScale, useUTMBounds);

            CoordinateTransformationFactory ctfac = new CoordinateTransformationFactory();
            ICoordinateTransformation trans = ctfac.CreateFromCoordinateSystems(pcs_UTM.GeographicCoordinateSystem, pcs_UTM);

            return trans;
        }

        private static IProjectedCoordinateSystem AlterStandardWGS(int utmZone, bool zoneIsNorth, double centralMeridian, double latitudeOfOrigin, double unitScale, bool useUTMBounds)
        {
            IProjectedCoordinateSystem pcs_UTM = ProjectedCoordinateSystem.WGS84_UTM(utmZone, zoneIsNorth);
            GeoAPI.CoordinateSystems.ICoordinateSystemFactory cFac = new ProjNet.CoordinateSystems.CoordinateSystemFactory();

            var pInfo = new List<ProjectionParameter>();
            pInfo.Add(new ProjectionParameter("latitude_of_origin", latitudeOfOrigin));
            pInfo.Add(new ProjectionParameter("central_meridian", centralMeridian));
            pInfo.Add(new ProjectionParameter("scale_factor", 0.9996));
            if (useUTMBounds)
            {
                pInfo.Add(new ProjectionParameter("false_easting", 500000 / unitScale));
                pInfo.Add(new ProjectionParameter("false_northing", zoneIsNorth ? 0 : 10000000 / unitScale));
            }
            else
            {
                pInfo.Add(new ProjectionParameter("false_easting", 0));
                pInfo.Add(new ProjectionParameter("false_northing", zoneIsNorth ? 0 : 10000000));
            }
            IProjection projection = cFac.CreateProjection("UTM", "Transverse_Mercator", pInfo);

            pcs_UTM.Projection = projection;
            pcs_UTM.LinearUnit.MetersPerUnit = unitScale;
            return pcs_UTM;
        }

        public static int GetUTMZone(double lat, double lon)
        {
            if (lat >= 56 && lat < 64 && lon >= 3 && lon < 13)
            {
                return 32;
            }
            if (lat >= 72 && lat < 84)
            {
                if (lon >= 0 && lon < 9)
                    return 31;
                else if (lon >= 9 && lon < 21)
                    return 33;
                if (lon >= 21 && lon < 33)
                    return 35;
                if (lon >= 33 && lon < 42)
                    return 37;
            }
            return (int)Math.Ceiling((lon + 180) / 6);
        }

        // Just here to be unit tested against as a working example
        // Note fromPointLongLat order
        public static double[] MostSimpleExample(Coord min, double[] fromPointLonLat)
        {
            // See https://github.com/bozhink/ProcessingTools/blob/d83a57e955e6bae815bbfe7d886d109b08abd136/src/ProcessingTools/Infrastructure/Geo/Transformers/UtmCoordianesTransformer.cs
            int utmZone = GetUTMZone(min.Latitude, min.Longitude);
            bool zoneIsNorth = min.Latitude > 0.0;

            ICoordinateSystem gcs_WGS84 = GeographicCoordinateSystem.WGS84;
            IProjectedCoordinateSystem pcs_UTM31N = ProjectedCoordinateSystem.WGS84_UTM(utmZone, zoneIsNorth);

            CoordinateTransformationFactory ctfac = new CoordinateTransformationFactory();
            ICoordinateTransformation trans = ctfac.CreateFromCoordinateSystems(gcs_WGS84, pcs_UTM31N);

            // Seemingly contra to documentation the transformation here only works if in LON-LAT order
            double[] toPoint = trans.MathTransform.Transform(fromPointLonLat);
            return toPoint;
        }
    }
}
