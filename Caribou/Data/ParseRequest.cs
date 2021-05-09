namespace Caribou.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Caribou.Components;

    /// <summary>
    /// A wrapper around a list of OSMMetaData items that are parsed from the provided raw key:value or key pairs
    /// </summary>
    public struct ParseRequest
    {
        public List<OSMMetaData> RequestedMetaData;

        public ParseRequest(List<string> metaDataRawValues, ref MessagesWrapper messages)
        {
            if (metaDataRawValues.Count == 0)
            {
                messages.AddWarning("No feature keys provided. Please provide them via:\n" +
                    "- Via text parameters 'key:value' format separated by commas or newlines" +
                    "- Via one of the Specify components.");
            }

            var cleanedGhInput = new List<string>();

            // Split up the big list of strings into an array of single key:values
            foreach (string inputString in metaDataRawValues)
            {
                string[] lines = inputString.Split(
                    new[] { "\r\n", "\r", "\n", "," }, // Split on new lines and on commas
                    StringSplitOptions.None);

                for (var i = 0; i < lines.Length; i++)
                {
                    cleanedGhInput.Add(lines[i].Trim().ToLower());
                }
            }

            this.RequestedMetaData = new List<OSMMetaData>();
            // Transform the key:value formatted strings into OSM items and assign them
            foreach (string inputString in cleanedGhInput)
            { 
                var osmItems = ParseRawKeyToOSMMetaData(inputString);
                foreach (OSMMetaData osmItem in osmItems)
                {
                    if (osmItem != null && !this.RequestedMetaData.Contains(osmItem))
                    {
                        this.RequestedMetaData.Add(osmItem);
                    }
                }
            }
        }

        public static List<OSMMetaData> ParseRawKeyToOSMMetaData(string inputString)
        {
            string osmKey;
            string osmValue = null;
            OSMMetaData keyItem = null;
            List<OSMMetaData> foundItems = new List<OSMMetaData>();

            if (inputString.Length == 0)
            {
                return foundItems;
            }

            if (inputString.Contains(':'))
            {
                osmKey = inputString.Trim().Split(':')[0];
                osmValue = inputString.Trim().Split(':')[1];
                // Create and return the parent key
                keyItem = new OSMMetaData(osmValue, "", false, "");
                foundItems.Add(keyItem);
            }
            else
            {
                osmKey = inputString.Trim().Split(':')[0];
            }

            // Add the key as an item
            OSMMetaData valueItem = new OSMMetaData(osmKey, "", false, "", keyItem);
            foundItems.Add(valueItem);

            return foundItems;
        }
    }
}
