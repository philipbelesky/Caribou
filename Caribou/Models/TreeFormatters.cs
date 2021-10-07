namespace Caribou.Models
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Drawing;
    using Caribou.Models;
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

        public static GH_Structure<GH_String> MakeReportForRequests(
            Dictionary<OSMTag, int> foundItemsForResult)
        {
            var output = new GH_Structure<GH_String>();
            var tInfo = CultureInfo.CurrentCulture.TextInfo;

            var requestMetaDataItems = foundItemsForResult.Keys.ToList();
            for (int i = 0; i < requestMetaDataItems.Count; i++)
            {
                var metaData = requestMetaDataItems[i];
                GH_Path path = new GH_Path(i);
                var count = foundItemsForResult[metaData];
                var colorForItem = GetPerceptualColorForTreeItem(requestMetaDataItems.Count, i);
                var metaDataTitle = tInfo.ToTitleCase(metaData.Name);

                output.Append(new GH_String(metaDataTitle), path);
                output.Append(new GH_String(metaData.ToString()), path);
                output.Append(new GH_String($"{count} found"), path);
                output.Append(new GH_String(colorForItem.ToString()));
                if (metaData.Key != null)
                {
                    var titleName = tInfo.ToTitleCase(metaData.Key.Name);
                    output.Append(new GH_String(titleName), path);
                    var layerName = titleName + "::" + metaDataTitle;
                    output.Append(new GH_String(layerName), path); // Layer path helper
                }
                else
                {
                    output.Append(new GH_String("(No Parent Feature)"), path);
                    output.Append(new GH_String(metaDataTitle + "::"), path); // Layer path helper
                }

                if (!string.IsNullOrEmpty(metaData.Description))
                {
                    output.Append(new GH_String($"Defined as: {metaData.Description}"), path);
                }
            }

            return output;
        }

        public static GH_Structure<GH_Point> MakeTreeForNodes(Dictionary<OSMTag, List<Point3d>> foundNodes)
        {
            var output = new GH_Structure<GH_Point>();
            var i = 0;

            foreach (var entry in foundNodes)
            {
                for (int j = 0; j < entry.Value.Count; j++)
                {
                    GH_Path path = new GH_Path(i, j); // Need to ensure even an empty path exists to enable data matching
                    output.EnsurePath(path); // Need to ensure even an empty path exists to enable data matching
                    GH_Point pointForPath = new GH_Point(entry.Value[j]);
                    output.Append(pointForPath, path);
                }
                i++;
            }

            return output;
        }

        public static GH_Structure<GH_Curve> MakeTreeForWays(Dictionary<OSMTag, List<PolylineCurve>> foundWays)
        {
            var output = new GH_Structure<GH_Curve>();
            var i = 0;

            foreach (var entry in foundWays)
            {
                for (int j = 0; j < entry.Value.Count; j++)
                {
                    GH_Path path = new GH_Path(i, j); // Need to ensure even an empty path exists to enable data matching
                    output.EnsurePath(path); // Need to ensure even an empty path exists to enable data matching
                    GH_Curve lineForPath = new GH_Curve(entry.Value[j]);
                    output.Append(lineForPath, path);
                }
                i++;
            }

            return output;
        }

        public static GH_Structure<GH_Brep> MakeTreeForBuildings(Dictionary<OSMTag, List<Brep>> foundBuildings)
        {
            var output = new GH_Structure<GH_Brep>();
            var i = 0;

            foreach (var entry in foundBuildings)
            { 
                for (int j = 0; j < entry.Value.Count; j++)
                {
                    GH_Path path = new GH_Path(i, j); // Need to ensure even an empty path exists to enable data matching
                    output.EnsurePath(path); // Need to ensure even an empty path exists to enable data matching
                    GH_Brep brepForPath = new GH_Brep(entry.Value[j]);
                    output.Append(brepForPath, path);
                }
                i++;
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
