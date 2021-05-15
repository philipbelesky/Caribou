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
            var output = new GH_Structure<GH_String>();
            for (int i = 0; i < result.RequestedMetaData.Requests.Count; i++)
            {
                GH_Path path = new GH_Path(i);
                var metaData = result.RequestedMetaData.Requests[i];
                var results = result.FoundData[metaData];
                foreach (var foundItem in results)
                {
                    // Make a second level of path and add list of key:values
                }
            }

            return output;
        }

        public static GH_Structure<GH_String> MakeTreeForMetaDataReport(RequestHandler result)
        {
            var output = new GH_Structure<GH_String>();
            for (int i = 0; i < result.RequestedMetaData.Requests.Count; i++)
            {
                GH_Path path = new GH_Path(i);
                var metaData = result.RequestedMetaData.Requests[i];
                output.Append(new GH_String(metaData.Name), path);
                output.Append(new GH_String("count is"), path);
            }

            return output;
        }

        public static GH_Structure<GH_Point> MakeTreeForNodes(Dictionary<OSMMetaData, List<Point3d>> foundNodes)
        {
            var output = new GH_Structure<GH_Point>();
            var i = 0;

            foreach (var entry in foundNodes)
            {
                GH_Path path = new GH_Path(i);
                i++;
                foreach (var pt in entry.Value)
                {
                    output.Append(new GH_Point(pt), path);
                }
            }

            return output;
        }

        public static GH_Structure<GH_Curve> MakeTreeForWays(Dictionary<OSMMetaData, List<PolylineCurve>> foundWays)
        {
            var output = new GH_Structure<GH_Curve>();
            var i = 0;

            foreach (var entry in foundWays)
            {
                GH_Path path = new GH_Path(i);
                i++;
                foreach (var pLine in entry.Value)
                {
                    output.Append(new GH_Curve(pLine), path);
                }
            }

            return output;
        }
    }
}
