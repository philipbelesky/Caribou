namespace Caribou.Processing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Caribou.Data;

    public static class ParseViaLinq
    {
        public static void FindItemsByTag(ref RequestHandler request)
        {
            GetBounds(ref request);

            //var matches = new RequestHandler(featuresSpecified); // Output
            //var xml = XDocument.Parse(xmlContents);
            //var matchAllKey = ParseRequest.SearchAllKey;
            //GetBounds(ref matches, xml); // Add minmax latlon to matches

            //foreach (var tagKey in matches.Nodes.Keys)
            //{
            //    System.Collections.Generic.IEnumerable<XElement> results;
            //    if (matches.Nodes[tagKey].ContainsKey(matchAllKey))
            //    {
            //        // If searching for all instances of a key regardless of the value
            //        results = from tag in xml.Descendants("tag")
            //                  where (string)tag.Attribute("k") == tagKey
            //                  select tag;

            //        foreach (var result in results)
            //        {
            //            if (result.Parent.Name != "node")
            //            {
            //                continue;
            //            }

            //            var tagValue = result.Attributes("v").First().Value;
            //            var lat = Convert.ToDouble(result.Parent.Attributes("lat").First().Value);
            //            var lon = Convert.ToDouble(result.Parent.Attributes("lon").First().Value);
            //            matches.AddNodeGivenFeature(tagKey, tagValue, new Coord(lat, lon));
            //        }
            //    }
            //    else
            //    {
            //        // If searching for all instances of a key regardless of the value
            //        foreach (string tagValue in matches.Nodes[tagKey].Keys)
            //        {
            //            results = from tag in xml.Descendants("tag")
            //                      where (string)tag.Attribute("k") == tagKey && (string)tag.Attribute("v") == tagValue
            //                      select tag.Parent;

            //            foreach (var result in results)
            //            {
            //                if (result.Name != "node")
            //                {
            //                    continue;
            //                }

            //                var lat = Convert.ToDouble(result.Attributes("lat").First().Value);
            //                var lon = Convert.ToDouble(result.Attributes("lon").First().Value);
            //                matches.AddNodeGivenFeatureAndSubFeature(tagKey, tagValue, new Coord(lat, lon));
            //            }
            //        }
            //    }
            //}

            //return matches;
        }

        // Identify a minimum and maximum boundary that encompasses all of the provided files' boundaries
        private static void GetBounds(ref RequestHandler result)
        {
            foreach (string providedXML in result.xmlCollection.ProvidedXMLs)
            {
                var xml = XDocument.Parse(providedXML);
                var boundsElement = (from el in xml.Descendants("bounds") select el).First();
                CheckBounds(boundsElement, ref result);
            }
        }

        private static void CheckBounds(XElement element, ref RequestHandler result)
        {
            var boundsMinLat = Convert.ToDouble(element.Attributes("minlat").First().Value);
            if (result.minBounds.Latitude > boundsMinLat)
            {
                result.minBounds.Latitude = boundsMinLat;
            }

            var boundsMinLon = Convert.ToDouble(element.Attributes("minlon").First().Value);
            if (result.minBounds.Longitude > boundsMinLon)
            {
                result.minBounds.Longitude = boundsMinLon;
            }

            var boundsMaxLat = Convert.ToDouble(element.Attributes("maxlat").First().Value);
            if (result.maxBounds.Latitude > boundsMaxLat)
            {
                result.maxBounds.Latitude = boundsMaxLat;
            }

            var boundsMaxLon = Convert.ToDouble(element.Attributes("maxlon").First().Value);
            if (result.maxBounds.Longitude > boundsMaxLon)
            {
                result.maxBounds.Longitude = boundsMaxLon;
            }
        }
    }
}
