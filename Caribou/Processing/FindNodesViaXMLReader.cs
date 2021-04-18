namespace Caribou.Processing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Threading.Tasks;
    using System.IO;

    public class FindNodesViaXMLReader
    {
        public static ResultsForFeatures FindByFeatures(RequestedFeature[] featuresSpecified, string xmlContents)
        {
            var matches = new ResultsForFeatures(featuresSpecified); // Output
            var matchAllKey = RequestedFeature.SearchAllKey;

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
                            if (matches.Results.ContainsKey(tagKey))
                            {
                                tagValue = reader.GetAttribute("v");

                                if (matches.Results[tagKey].ContainsKey(matchAllKey))
                                {
                                    // If we are searching for all items within a feature then add it regardless
                                    matches.AddCoordForFeature(tagKey, tagValue, latitude, longitude);
                                } 
                                else if (matches.Results[tagKey].ContainsKey(tagValue))
                                {
                                    // If searching for a particular key:value only add if there is 
                                    matches.AddCoordForFeatureAndSubFeature(tagKey, tagValue, latitude, longitude);
                                }
                            }
                        }
                    }
                }
            }
            return matches;
        }
    }
}
