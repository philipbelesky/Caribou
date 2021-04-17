using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using System.IO;

namespace Caribou.Processing
{
    public class FindNodes
    {
        public static ResultsForFeatures FindByFeaturesA(RequestedFeature[] featuresSpecified, string xmlContents)
        {
            // Output
            var matches = new ResultsForFeatures(featuresSpecified);
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
                        else if (reader.Name == "tag")
                        {
                            tagKey = reader.GetAttribute("k");
                            if (matches.Results.ContainsKey(tagKey))
                            {
                                tagValue = reader.GetAttribute("v");
                                if (matches.Results[tagKey].ContainsKey(matchAllKey))
                                {
                                    // If we are searching for all items within a feature then add it regardless
                                    if (matches.Results[tagKey].ContainsKey(tagValue)) {
                                        // If we have already found items of this sub-type, add it to an existing list
                                        matches.Results[tagKey][tagValue].Add(new Coords(latitude, longitude));
                                    } 
                                    else
                                    {
                                        // If not then need to add the key and init the list
                                        matches.Results[tagKey][tagValue] = new List<Coords>() {
                                            new Coords(latitude, longitude)
                                        };
                                    }
                                } 
                                else if (matches.Results[tagKey].ContainsKey(tagValue))
                                {
                                    // If searching for a particular key:value only add if there is 
                                    matches.Results[tagKey][tagValue].Add(new Coords(latitude, longitude));
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
