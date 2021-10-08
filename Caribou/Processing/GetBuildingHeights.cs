namespace Caribou.Processing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class GetBuildingHeights
    {
        public const double METERS_PER_LEVEL = 3.0;
        public const double FT_TO_M = 0.3048;
        public const double INCHES_TO_M = 0.0254;

        public static double ParseHeight(Dictionary<string, string> tags, double unitScale)
        {
            var height = 0.0; // null result
            if (tags.ContainsKey("height"))
            {
                height = GetSanitisedHeight(tags["height"]) * unitScale;
            }
            else if (tags.ContainsKey("building:height"))
            {
                height = GetSanitisedHeight(tags["building:height"]) * unitScale;
            }
            else if (tags.ContainsKey("building:levels"))
            {
                height = GetSanitisedLevels(tags["building:levels"]) * unitScale;
            }
            else if (tags.ContainsKey("building:level"))
            {
                height = GetSanitisedLevels(tags["building:level"]) * unitScale;
            }
            else if (tags.ContainsKey("stories"))
            {
                height = GetSanitisedLevels(tags["stories"]) * unitScale;
            }
            else if (tags.ContainsKey("levels"))
            {
                height = GetSanitisedLevels(tags["levels"]) * unitScale;
            }
            return height;
        }

        static double GetSanitisedHeight(string heightKey)
        {
            double parsedHeight;

            if (heightKey.Contains('\'') || heightKey.Contains('\"')) // Imperial is valid per OSM
            {
                string rawFeet = heightKey.Split('\'')[0].Trim();
                string remainingHeight = heightKey.Replace('\'', ' ').Replace(heightKey.Split('\'')[0], "");

                string rawInches;
                if (remainingHeight.Contains('\"'))
                    rawInches = remainingHeight.Split('\"')[0].Trim();
                else
                    rawInches = "0";

                parsedHeight = (int.Parse(rawFeet) * FT_TO_M) + (int.Parse(rawInches) * INCHES_TO_M);
            }
            else
            {
                var rawHeight = heightKey.Replace("m", "").Trim();
                parsedHeight = Double.Parse(rawHeight);
            }

            if (parsedHeight > 0)
                return parsedHeight;

            return 0.0;
        }

        static double GetSanitisedLevels(string levelKey)
        {
            var rawLevels = levelKey.Trim();
            if (IsDigitsOnly(rawLevels))
            {
                return Double.Parse(rawLevels) * METERS_PER_LEVEL;
            }

            return 0.0;
        }

        static bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }
    }
}
