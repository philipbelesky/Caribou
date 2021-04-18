namespace Caribou.Processing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    public class FindNodesViaLinq
    {
        public static ResultsForFeatures FindByFeatures(RequestedFeature[] featuresSpecified, string xmlContents)
        {
            var matches = new ResultsForFeatures(featuresSpecified); // Output
            var xml = XDocument.Parse(xmlContents);
            var matchAllKey = RequestedFeature.SearchAllKey;

            var boundsElement = (from el in xml.Descendants("bounds") select el).First();
            matches.SetLatLonBounds(
                Convert.ToDouble(boundsElement.Attributes("minlat").First().Value),
                Convert.ToDouble(boundsElement.Attributes("minlon").First().Value),
                Convert.ToDouble(boundsElement.Attributes("maxlat").First().Value),
                Convert.ToDouble(boundsElement.Attributes("maxlon").First().Value)
            );

            foreach (var tagKey in matches.Results.Keys)
            {
                System.Collections.Generic.IEnumerable<XElement> results;
                if (matches.Results[tagKey].ContainsKey(matchAllKey))
                {
                    // If searching for all instances of a key regardless of the value
                    results = from tag in xml.Descendants("tag")
                              where (string)tag.Attribute("k") == tagKey
                              select tag;

                    foreach (var result in results)
                    {
                        if (result.Parent.Name != "node")
                        {
                            continue;
                        }
                        var tagValue = result.Attributes("v").First().Value;
                        var lat = Convert.ToDouble(result.Parent.Attributes("lat").First().Value);
                        var lon = Convert.ToDouble(result.Parent.Attributes("lon").First().Value);
                        matches.AddCoordForFeature(tagKey, tagValue, lat, lon);
                    }
                }
                else
                {
                    // If searching for all instances of a key regardless of the value
                    foreach (string tagValue in matches.Results[tagKey].Keys)
                    {
                        results = from tag in xml.Descendants("tag")
                                  where (string)tag.Attribute("k") == tagKey && (string)tag.Attribute("v") == tagValue
                                  select tag.Parent;

                        foreach (var result in results)
                        {
                            if (result.Name != "node")
                            {
                                continue;
                            }
                            var lat = Convert.ToDouble(result.Attributes("lat").First().Value);
                            var lon = Convert.ToDouble(result.Attributes("lon").First().Value);
                            matches.AddCoordForFeatureAndSubFeature(tagKey, tagValue, lat, lon);
                        }
                    }

                }
            }

            return matches;
        }
    }
}
