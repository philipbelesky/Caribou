namespace Caribou.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Caribou.Data;
    using Grasshopper.Kernel.Data;
    using Grasshopper.Kernel.Types;
    using Rhino.Geometry;

    public static class TreeFormatters
    {
        public static GH_Structure<GH_String> MakeTreeForItemTags(RequestHandler result)
        {
            // TODO
            return new GH_Structure<GH_String>();
        }

        public static GH_Structure<GH_String> MakeTreeForMetaDataReport(RequestHandler result)
        {
            // TODO
            return new GH_Structure<GH_String>();
        }

        public static GH_Structure<GH_Point> MakeTreeForNodes(Dictionary<OSMMetaData, List<Point3d>> foundNodes)
        {
            // TODO
            return new GH_Structure<GH_Point>();
        }

        public static GH_Structure<GH_Curve> MakeTreeForWays(Dictionary<OSMMetaData, List<Polyline>> foundWays)
        {
            // TODO
            return new GH_Structure<GH_Curve>();
        }
    }
}
