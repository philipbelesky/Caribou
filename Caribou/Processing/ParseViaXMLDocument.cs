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
        public static void GetBounds(ref RequestHandler result)
        {
            double? currentMinLat = null;
            double? currentMinLon = null;
            double? currentMaxLat = null;
            double? currentMaxLon = null;

            foreach (string providedXML in result.XmlPaths)
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(providedXML);
                XmlNode root = doc.DocumentElement;
                var boundsElement = root.SelectNodes("/osm/bounds").Item(0);
                CheckBounds(boundsElement, ref currentMinLat, ref currentMinLon, ref currentMaxLat, ref currentMaxLon);
            }

            result.MinBounds = new Coord(currentMinLat.Value, currentMinLon.Value);
            result.MaxBounds = new Coord(currentMaxLat.Value, currentMaxLon.Value);
        }

        private static void CheckBounds(XmlNode node, ref double? currentMinLat, ref double? currentMinLon,
                                                         ref double? currentMaxLat, ref double? currentMaxLon)
        {
            var boundsMinLat = Convert.ToDouble(node.Attributes.GetNamedItem("minlat").Value);
            if (!currentMinLat.HasValue || boundsMinLat < currentMinLat)
            {
                currentMinLat = boundsMinLat;
            }

            var boundsMinLon = Convert.ToDouble(node.Attributes.GetNamedItem("minlon").Value);
            if (!currentMinLon.HasValue || boundsMinLon < currentMinLon)
            {
                currentMinLon = boundsMinLon;
            }

            var boundsMaxLat = Convert.ToDouble(node.Attributes.GetNamedItem("maxlat").Value);
            if (!currentMaxLat.HasValue || boundsMaxLat > currentMaxLat)
            {
                currentMaxLat = boundsMaxLat;
            }

            var boundsMaxLon = Convert.ToDouble(node.Attributes.GetNamedItem("maxlon").Value);
            if (!currentMaxLon.HasValue || boundsMaxLon > currentMaxLon)
            {
                currentMaxLon = boundsMaxLon;
            }
        }
    }
}
