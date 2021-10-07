namespace Caribou.Models
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// A wrapper around a list of OSMMetaData items. These are requested metadata tags to find.
    /// They are parsed from the provided raw key:value or key pairs (e.g. Grasshopper input).
    /// </summary>
    public struct ParseRequest
    {
        public List<OSMTag> Requests;

        // Requests coming to a list input in Grasshopper, e.g. the Find components via Specify or Panel outputs
        public ParseRequest(List<string> metaDataRawValues)
        {
            var cleanedGhInput = SplitTextStreamIntoIndividualItems(metaDataRawValues);

            // Transform the key:value formatted strings into OSM items and assign them
            this.Requests = new List<OSMTag>();
            foreach (string inputString in cleanedGhInput)
            {
                var osmItem = new OSMTag(inputString);
                if (osmItem != null)
                {
                    if (!this.Requests.Contains(osmItem)) // Prevent duplicates
                        this.Requests.Add(osmItem);
                }
            }
        }

        // This constructor is mostly just used to enable testing
        public ParseRequest(List<OSMTag> prepackagedData)
        {
            this.Requests = prepackagedData;
        }

        /// <summary>Take a list of strings that may or may not be individual items and/or list items</summary>
        public static List<string> SplitTextStreamIntoIndividualItems(List<string> rawText)
        {
            var cleanedItems = new List<string>();
            // Split up the big list of strings into an array of single key:values
            foreach (string inputString in rawText)
            {
                string[] lines = inputString.ToString().Split(
                    new[] { "\r\n", "\r", "\n", "," }, // Split on new lines and on commas
                    StringSplitOptions.None);

                for (var i = 0; i < lines.Length; i++)
                {
                    var cleanedKeyValue = lines[i].Trim().Replace(",", string.Empty);
                    if (!string.IsNullOrEmpty(lines[i]))
                        cleanedItems.Add(cleanedKeyValue.ToLower(CultureInfo.InvariantCulture));
                }
            }
            return cleanedItems;
        }
    }
}
