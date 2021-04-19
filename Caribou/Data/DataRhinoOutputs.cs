namespace Caribou.Processing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Rhino;
    using Rhino.Geometry;

    public class DataRhinoOutputs
    {
        private UnitSystem docUnits = RhinoDoc.ActiveDoc.ModelUnitSystem;

        public static List<Point3d> GetNodesFromCoords(ResultsForFeatures foundItems)
        {
            var results = new List<Point3d>();
            foreach (var featureType in foundItems.Nodes.Keys)
            {
                foreach (var subfeatureType in foundItems.Nodes[featureType].Keys)
                {
                    foreach (var coord in foundItems.Nodes[featureType][subfeatureType])
                    {
                        results.Add(GetPointFromLatLong(coord));
                    }
                }
            }

            return results;
        }

        public static List<Polyline> GetWaysFromCoords(ResultsForFeatures foundItems)
        {
            var linePoints = new List<Point3d>();
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
                            linePoints.Add(GetPointFromLatLong(coord));
                        }

                        results.Add(new Polyline(linePoints));
                    }
                }
            }

            return results;
        }

        private static Point3d GetPointFromLatLong(Coord coord)
        {
            return new Point3d(coord.Latitude, coord.Longitude, 0);
        }
    }
}
