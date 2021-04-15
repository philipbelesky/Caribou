using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace Caribou.Processing
{
    public class XMLOutput
    {
        public static List<Point3d> GetNodesFromCoords(Dictionary<string, List<Coords>> foundItems)
        {
            var results = new List<Point3d>();
            foreach (var featureType in foundItems)
            {
                foreach (var coord in featureType.Value)
                {
                    results.Add(GetPointFromLatLong(coord));
                }
            }
            return results;
        }

        public static List<Polyline> GetWaysFromCoords(Dictionary<string, List<Coords>> foundItems)
        {
            var results = new List<Polyline>();
            foreach (var featureType in foundItems)
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
