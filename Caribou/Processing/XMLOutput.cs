namespace Caribou.Processing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Rhino.Geometry;

    public class XMLOutput
    {
        public static List<Point3d> GetNodesFromCoords(ResultsForFeatures foundItems)
        {
            var results = new List<Point3d>();
            foreach (var featureType in foundItems.Results.Keys)
            {
                foreach (var subfeatureType in foundItems.Results[featureType].Keys)
                {
                    foreach (var coord in foundItems.Results[featureType][subfeatureType])
                    {
                        results.Add(GetPointFromLatLong(coord));
                    }
                }
            }
            return results;
        }

        public static List<Polyline> GetWaysFromCoords(ResultsForFeatures foundItems)
        {
            var results = new List<Polyline>();
            foreach (var featureType in foundItems.Results.Keys)
            {
                // TODO: implementation
            }
            return results;
        }

        private static Point3d GetPointFromLatLong(Coords coord)
        {
            return new Point3d(coord.Latitude, coord.Longitude, 0);
        }
    }
}
