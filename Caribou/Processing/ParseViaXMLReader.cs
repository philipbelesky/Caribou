namespace Caribou.Processing
{
    using System;
    using System.IO;
    using System.Xml;

    public class ParseViaXMLReader
    {
        public static ResultsForFeatures FindByFeatures(DataRequestResult[] featuresSpecified, string xmlContents)
        {
            var matches = new ResultsForFeatures(featuresSpecified); // Output
            var matchAllKey = DataRequestResult.SearchAllKey;
            GetBounds(ref matches, xmlContents); // Add minmax latlon to matches

            using (XmlReader reader = XmlReader.Create(new StringReader(xmlContents)))
            {
                // XmlReader is forward-only so need to track the lat/long of the parent element to use if we find a match
                double latitude = 0.0;
                double longitude = 0.0;
                string tagKey;
                string tagValue;

                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        if (reader.Name == "node")
                        {
                            latitude = Convert.ToDouble(reader.GetAttribute("lat"));
                            longitude = Convert.ToDouble(reader.GetAttribute("lon"));
                        }
                        else if (reader.Name == "way")
                        {
                            reader.Skip();
                        }
                        else if (reader.Name == "tag")
                        {
                            tagKey = reader.GetAttribute("k");
                            if (matches.Nodes.ContainsKey(tagKey))
                            {
                                tagValue = reader.GetAttribute("v");

                                if (matches.Nodes[tagKey].ContainsKey(matchAllKey))
                                {
                                    // If we are searching for all items within a feature then add it regardless
                                    matches.AddNodeGivenFeature(tagKey, tagValue, latitude, longitude);
                                }
                                else if (matches.Nodes[tagKey].ContainsKey(tagValue))
                                {
                                    // If searching for a particular key:value only add if there is
                                    matches.AddNodeGivenFeatureAndSubFeature(tagKey, tagValue, latitude, longitude);
                                }
                            }
                        }
                    }
                }
            }

            return matches;
        }

        private static void GetBounds(ref ResultsForFeatures matches, string xmlContents)
        {
            using (XmlReader reader = XmlReader.Create(new StringReader(xmlContents)))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        if (reader.Name == "bounds")
                        {
                            matches.SetLatLonBounds(
                                Convert.ToDouble(reader.GetAttribute("minlat")),
                                Convert.ToDouble(reader.GetAttribute("minlon")),
                                Convert.ToDouble(reader.GetAttribute("maxlat")),
                                Convert.ToDouble(reader.GetAttribute("maxlon")));
                        }
                    }
                }
            }
        }
    }
}
