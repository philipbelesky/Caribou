namespace Caribou.Processing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class IdentifyBuildingHeight
    {
        public const double METERS_PER_LEVEL = 3.0;

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
            else if (tags.ContainsKey("levels"))
            {
                height = GetSanitisedLevels(tags["levels"]) * unitScale;
            }
            return height;
        }

        static double GetSanitisedHeight(string heightKey)
        {
            var rawHeight = heightKey.Replace("m", "").Trim();
            if (IsDigitsOnly(rawHeight))
            {
                return Double.Parse(rawHeight);
            }

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
