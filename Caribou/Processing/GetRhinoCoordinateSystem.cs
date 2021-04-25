namespace Caribou.Processing
{
    using System;
    using GeoAPI.CoordinateSystems.Transformations;
    using Caribou.Data;
    using ProjNet.CoordinateSystems.Transformations;
    using ProjNet.CoordinateSystems;
    using GeoAPI.CoordinateSystems;
    using System.Collections.Generic;

    public static class GetRhinoCoordinateSystem
    {
        public static ICoordinateTransformation GetTransformation(Coord minLatLon, double unitScale, string unitName)
        {
            // Setup our FROM coordinate system (WSG84 per OSM)
            GeoAPI.CoordinateSystems.IGeographicCoordinateSystem wgsCS;
            wgsCS = ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84;

            // Customise our TO coordinate system's units given Rhino doc
            var unitForCS = new LinearUnit(unitScale, unitName, string.Empty, 0, string.Empty, string.Empty, string.Empty);
 
            // Customise our TO coordinate system's origin given bounds of OSM file
            var latitudeOfOrigin = minLatLon.Latitude;
            var centralMeridian = minLatLon.Longitude;
            int utmZone = GetUTMZone(minLatLon.Longitude);

            // Setup our TO coordinate system (UTM but with modified units/origin to match Rhino)
            GeoAPI.CoordinateSystems.IProjectedCoordinateSystem utmCS;
            utmCS = MakeCustomisedUTM(latitudeOfOrigin, centralMeridian, utmZone, wgsCS, unitForCS);

            CoordinateTransformationFactory ctfac = new CoordinateTransformationFactory();
            ICoordinateTransformation trans = ctfac.CreateFromCoordinateSystems(wgsCS, utmCS);

            return trans;
        }

        private static IProjectedCoordinateSystem MakeCustomisedUTM(double latitudeOfOrigin, double centralMeridian, int zone, GeographicCoordinateSystem geoCS, LinearUnit units)
        {
            var zoneIsNorth = latitudeOfOrigin > 0.0;
            GeoAPI.CoordinateSystems.ICoordinateSystemFactory cFac = new ProjNet.CoordinateSystems.CoordinateSystemFactory();

            // Adapted from https://github.com/NetTopologySuite/ProjNet4GeoAPI/blob/34628b210ee1681d5d8cc8602f634e700a8058cf/src/ProjNet/CoordinateSystems/ProjectedCoordinateSystem.cs
            var pInfo = new List<ProjectionParameter>();
            pInfo.Add(new ProjectionParameter("latitude_of_origin", latitudeOfOrigin));
            pInfo.Add(new ProjectionParameter("central_meridian", centralMeridian));
            pInfo.Add(new ProjectionParameter("scale_factor", 0.9996));
            pInfo.Add(new ProjectionParameter("false_easting", 500000));
            pInfo.Add(new ProjectionParameter("false_northing", zoneIsNorth ? 0 : 10000000));

            IProjection projection = cFac.CreateProjection("Transverse_Mercator", "Transverse_Mercator", pInfo);
            IProjectedCoordinateSystem coordsys = cFac.CreateProjectedCoordinateSystem(
                "" + "UTM WGS84",
                geoCS, 
                projection, 
                units,
                new AxisInfo("East", AxisOrientationEnum.East), 
                new AxisInfo("North", AxisOrientationEnum.North));

            return coordsys;
        }

        public static int GetUTMZone(double lon)
        {
            var a = (lon + 180) / 6;
            return (int)(Math.Floor(a % 60) + 1);
        }
    }
}
