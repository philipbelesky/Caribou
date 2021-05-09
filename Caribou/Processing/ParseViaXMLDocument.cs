namespace Caribou.Processing
{
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using Caribou.Data;

    public static class ParseViaXMLDocument
    {
        public static void FindItemsByTag(ref RequestHandler request)
        {
            GetBounds(ref request); 

            //var matches = new RequestHandler(featuresSpecified); // Output
            //var matchAllKey = ParseRequest.SearchAllKey;
            //double lat = 0.0;
            //double lon = 0.0;

            //XmlDocument doc = new XmlDocument();
            //doc.LoadXml(xmlContents);
            //XmlNode root = doc.DocumentElement;
            //GetBounds(ref matches, root); // Add minmax latlon to matches

            //foreach (var tagKey in matches.Nodes.Keys)
            //{
            //    XmlNodeList nodeList;

            //    if (matches.Nodes[tagKey].ContainsKey(matchAllKey))
            //    {
            //        // If searching for all instances of a key regardless of the value
            //        nodeList = root.SelectNodes("/osm/node/tag[@k='" + tagKey + "']");
            //        foreach (XmlNode featureTag in nodeList)
            //        {
            //            var tagValue = featureTag.Attributes.GetNamedItem("v").Value;
            //            lat = Convert.ToDouble(featureTag.ParentNode.Attributes.GetNamedItem("lat").Value);
            //            lon = Convert.ToDouble(featureTag.ParentNode.Attributes.GetNamedItem("lon").Value);
            //            matches.AddNodeGivenFeature(tagKey, tagValue, new Coord(lat, lon));
            //        }
            //    }
            //    else
            //    {
            //        // If searching for a specific key and value attribute strings
            //        foreach (string tagValue in matches.Nodes[tagKey].Keys)
            //        {
            //            nodeList = root.SelectNodes("/osm/node/tag[@k='" + tagKey + "' and @v='" + tagValue + "']");
            //            foreach (XmlNode featureTag in nodeList)
            //            {
            //                lat = Convert.ToDouble(featureTag.ParentNode.Attributes.GetNamedItem("lat").Value);
            //                lon = Convert.ToDouble(featureTag.ParentNode.Attributes.GetNamedItem("lon").Value);
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
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(providedXML);
                XmlNode root = doc.DocumentElement;
                var boundsElement = root.SelectNodes("/osm/bounds").Item(0);
                CheckBounds(boundsElement, ref result);
            }
        }

        private static void CheckBounds(XmlNode element, ref RequestHandler result)
        {
            var boundsMinLat = Convert.ToDouble(element.Attributes.GetNamedItem("minlat").Value);
            if (result.minBounds.Latitude > boundsMinLat)
            {
                result.minBounds.Latitude = boundsMinLat;
            }

            var boundsMinLon = Convert.ToDouble(element.Attributes.GetNamedItem("minlon").Value);
            if (result.minBounds.Longitude > boundsMinLon)
            {
                result.minBounds.Longitude = boundsMinLon;
            }

            var boundsMaxLat = Convert.ToDouble(element.Attributes.GetNamedItem("maxlat").Value);
            if (result.maxBounds.Latitude > boundsMaxLat)
            {
                result.maxBounds.Latitude = boundsMaxLat;
            }

            var boundsMaxLon = Convert.ToDouble(element.Attributes.GetNamedItem("maxlon").Value);
            if (result.maxBounds.Longitude > boundsMaxLon)
            {
                result.maxBounds.Longitude = boundsMaxLon;
            }
        }
    }
}
