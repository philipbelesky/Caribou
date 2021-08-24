namespace Caribou.Models
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Caribou.Components;

    /// <summary>
    /// A wrapper around a list of OSMMetaData items. These are requested metadata tags to find.
    /// They are parsed from the provided raw key:value or key pairs (e.g. Grasshopper input).
    /// </summary>
    public struct ParseRequest
    {
        public List<OSMMetaData> Requests;
        public const char SplitChar = '='; // Can't use ":" because that is used within OSM keys, like addr:housenumber

        public ParseRequest(List<string> metaDataRawValues)
        {
            var cleanedGhInput = new List<string>();

            // Split up the big list of strings into an array of single key:values
            foreach (string inputString in metaDataRawValues)
            {
                string[] lines = inputString.Split(
                    new[] { "\r\n", "\r", "\n", "," }, // Split on new lines and on commas
                    StringSplitOptions.None);

                for (var i = 0; i < lines.Length; i++)
                {
                    cleanedGhInput.Add(lines[i].Trim().ToLower(CultureInfo.InvariantCulture));
                }
            }

            this.Requests = new List<OSMMetaData>();
            // Transform the key:value formatted strings into OSM items and assign them
            foreach (string inputString in cleanedGhInput)
            {
                var osmItem = ParseItemToOSMMetaData(inputString);
                if (osmItem != null)
                {
                    if (!this.Requests.Contains(osmItem)) // Prevent duplicates
                    {
                        this.Requests.Add(osmItem);
                    }
                }
            }
        }

        // This constructor is mostly just used to enable testing
        public ParseRequest(List<OSMMetaData> prepackagedData)
        {
            this.Requests = prepackagedData;
        }

        public static OSMMetaData ParseItemToOSMMetaData(string inputString)
        {
            List<OSMMetaData> foundItems = new List<OSMMetaData>();

            if (inputString.Length == 0)
            {
                return null;
            }

            var osmKey = inputString.Trim().Split(SplitChar)[0];
            if (inputString.Trim().Split(SplitChar).Length >= 2)
            {
                var osmValue = inputString.Trim().Split(SplitChar)[1];
                if (osmValue != "*" && !string.IsNullOrEmpty(osmValue))
                {
                    // If dealing with a pair, e.g. amenity:restaurant
                    return new OSMMetaData(osmValue, osmKey);
                }
            }

            // If dealing with a top level item, e.g. geological (or geological: or geological:*)
            return new OSMMetaData(osmKey);
        }
    }
}
