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

    public static class GetRhinoCoordinateSystem
    {
        //public static ICoordinateTransformation GetTransformation(Coord min, double unitScale, string unitName)
        //{
        //    // Setup our FROM coordinate system (WSG84 per OSM)
        //    GeographicCoordinateSystem wgsCS;
        //    wgsCS = ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84;

        //    // Customise our TO coordinate system's units given Rhino doc
        //    var unitForCS = new LinearUnit(unitScale, unitName, string.Empty, 0, string.Empty, string.Empty, string.Empty);
 
        //    // Customise our TO coordinate system's origin given bounds of OSM file
        //    var latitudeOfOrigin = min.Latitude;
        //    var centralMeridian = min.Longitude;
        //    int utmZone = GetUTMZone(min.Latitude, min.Longitude);

        //    // Setup our TO coordinate system (UTM but with modified units/origin to match Rhino)
        //    ProjectedCoordinateSystem utmCS;
        //    utmCS = MakeCustomisedUTM(latitudeOfOrigin, centralMeridian, utmZone, wgsCS, unitForCS);

        //    CoordinateTransformationFactory ctfac = new CoordinateTransformationFactory();
        //    ICoordinateTransformation trans = ctfac.CreateFromCoordinateSystems(wgsCS, utmCS);

        //    return trans;
        //}

        //private static ProjectedCoordinateSystem MakeCustomisedUTM(double latitudeOfOrigin, double centralMeridian, int zone, GeographicCoordinateSystem geoCS, LinearUnit units)
        //{
        //    var zoneIsNorth = latitudeOfOrigin > 0.0;
        //    CoordinateSystemFactory cFac = new ProjNet.CoordinateSystems.CoordinateSystemFactory();

        //    // Adapted from https://github.com/NetTopologySuite/ProjNet4GeoAPI/blob/34628b210ee1681d5d8cc8602f634e700a8058cf/src/ProjNet/CoordinateSystems/ProjectedCoordinateSystem.cs
        //    var pInfo = new List<ProjectionParameter>();
        //    pInfo.Add(new ProjectionParameter("latitude_of_origin", latitudeOfOrigin));
        //    pInfo.Add(new ProjectionParameter("central_meridian", centralMeridian));
        //    pInfo.Add(new ProjectionParameter("scale_factor", 0.9996));
        //    pInfo.Add(new ProjectionParameter("false_easting", 500000));
        //    pInfo.Add(new ProjectionParameter("false_northing", zoneIsNorth ? 0 : 10000000));

        //    IProjection projection = cFac.CreateProjection("Transverse_Mercator", "Transverse_Mercator", pInfo);
        //    ProjectedCoordinateSystem coordsys = cFac.CreateProjectedCoordinateSystem(
        //        "" + "UTM WGS84",
        //        geoCS, 
        //        projection, 
        //        units,
        //        new AxisInfo("East", AxisOrientationEnum.East), 
        //        new AxisInfo("North", AxisOrientationEnum.North));

        //    return coordsys;
        //}

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
        public static double[] MostSimpleExample(Coord min, double[] fromPointLongLat)
        {
            // See https://github.com/bozhink/ProcessingTools/blob/d83a57e955e6bae815bbfe7d886d109b08abd136/src/ProcessingTools/Infrastructure/Geo/Transformers/UtmCoordianesTransformer.cs
            int utmZone = GetUTMZone(min.Latitude, min.Longitude);
            bool zoneIsNorth = min.Latitude > 0.0;

            ProjNet.CoordinateSystems.ICoordinateSystem gcs_WGS84 = GeographicCoordinateSystem.WGS84;
            IProjectedCoordinateSystem pcs_UTM31N = ProjectedCoordinateSystem.WGS84_UTM(utmZone, zoneIsNorth);

            CoordinateTransformationFactory ctfac = new CoordinateTransformationFactory();
            ICoordinateTransformation trans = ctfac.CreateFromCoordinateSystems(gcs_WGS84, pcs_UTM31N);

            double[] toPoint = trans.MathTransform.Transform(fromPointLongLat);
            return toPoint;
        }
    }
}
