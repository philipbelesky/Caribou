namespace Caribou.Models
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Drawing;
    using Caribou.Data;
    using Grasshopper.Kernel.Data;
    using Grasshopper.Kernel.Types;
    using Rhino.Geometry;
    using Hsluv;
    using System.Linq;

    public static class TreeFormatters
    {
        public static GH_Structure<GH_String> MakeTreeForItemTags(RequestHandler result)
        {
            var output = new GH_Structure<GH_String>();
            for (int i = 0; i < result.RequestedMetaData.Requests.Count; i++)
            {
                new GH_Path(i);
                var metaData = result.RequestedMetaData.Requests[i];
                var results = result.FoundData[metaData];
                for (int j = 0; j < results.Count; j++)
                {
                    var subPath = new GH_Path(i, j);
                    foreach (var tag in results[j].Tags)
                    {
                        var tagReadout = new GH_String(tag.Key + "=" + tag.Value);
                        output.Append(tagReadout, subPath);
                    }
                }
            }

            return output;
        }

        public static GH_Structure<GH_String> MakeTreeForMetaDataReport(RequestHandler result)
        {
            var output = new GH_Structure<GH_String>();
            var tInfo = CultureInfo.CurrentCulture.TextInfo;

            var requestMetaDataCount = result.RequestedMetaData.Requests.Count;
            for (int i = 0; i < requestMetaDataCount; i++)
            {
                GH_Path path = new GH_Path(i);
                var metaData = result.RequestedMetaData.Requests[i];
                var count = result.FoundData[metaData].Count;
                var colorForItem = GetPerceptualColorForTreeItem(requestMetaDataCount, i);

                output.Append(new GH_String(tInfo.ToTitleCase(metaData.Name)), path);
                output.Append(new GH_String(metaData.ToString()), path);
                output.Append(new GH_String($"{count} found"), path);
                output.Append(new GH_String(colorForItem.ToString()));
                if (metaData.ParentType != null)
                {
                    output.Append(new GH_String(tInfo.ToTitleCase(metaData.ParentType.Name)), path);
                }
                else
                {
                    output.Append(new GH_String("(No Parent Feature)"), path);
                }

                if (!string.IsNullOrEmpty(metaData.Explanation))
                {
                    output.Append(new GH_String($"Defined as: {metaData.Explanation}"), path);
                }
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

        /// <summary>/// Uses the HSLuv color space (via a package) to create maximally perceptually-distinct colors for use in legends.</summary>
        private static GH_Colour GetPerceptualColorForTreeItem(double treeCount, double itemPosition)
        {
            var HsluvValues = new double[] {
                (itemPosition / treeCount * 360.0), // Hue, from 0-360
                100.0, // Maximimise saturation
                50.0 // Halfway brightness
            };
            var RGBRaw = HsluvConverter.HsluvToRgb(HsluvValues); // Values are 0-1
            var RGBValues = RGBRaw.Select(value => value * 255).ToList(); // Values now 0-2
            var RGBColor = Color.FromArgb((int)RGBValues[0], (int)RGBValues[1], (int)RGBValues[2]);
            return new GH_Colour(RGBColor);
        }
    }
}
