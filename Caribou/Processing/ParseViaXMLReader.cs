namespace Caribou.Processing
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
    using Caribou.Data;

    public static class ParseViaXMLReader
    {
        public static void FindItemsByTag(ref RequestHandler request)
        {
            GetBounds(ref request);

            //var matches = new RequestHandler(featuresSpecified); // Output
            //var matchAllKey = ParseRequest.SearchAllKey;
            //var allNodes = new Dictionary<string, Coord>(); // Dict used to lookup a way's nodes
            //

            //using (XmlReader reader = XmlReader.Create(new StringReader(xmlContents)))
            //{
            //    // XmlReader is forward-only so need to track the lat/long of the parent element to use if we find a match
            //    string currentNodeId = ""; // Current node; need to track it for when parsing inside-nodes
            //    string tagKey;
            //    string tagValue;
            //    bool inAWay = false; // Need to keep track of when parsing inside Ways so we know wether to add tag info to node or way lists
            //    List<string> wayNodesIds = new List<string>(); // If parsing inside Ways need to track the different nodes that make it up

            //    while (reader.Read())
            //    {
            //        if (reader.IsStartElement())
            //        {
            //            if (reader.Name == "node")
            //            {
            //                wayNodesIds.Clear();
            //                inAWay = false;
            //                currentNodeId = reader.GetAttribute("id");
            //                allNodes[currentNodeId] = new Coord(
            //                    Convert.ToDouble(reader.GetAttribute("lat")),
            //                    Convert.ToDouble(reader.GetAttribute("lon")));
            //            }
            //            else if (reader.Name == "way")
            //            {
            //                wayNodesIds.Clear();
            //                inAWay = true;
            //            }
            //            else if (reader.Name == "nd")
            //            {
            //                wayNodesIds.Add(reader.GetAttribute("ref"));
            //            }
            //            else if (reader.Name == "tag")
            //            {
            //                tagKey = reader.GetAttribute("k");
            //                if (!matches.PrimaryFeaturesToFind.Contains(tagKey))
            //                {
            //                    continue;
            //                }

            //                tagValue = reader.GetAttribute("v");
            //                if (inAWay)
            //                {
            //                    // Parsing a collections of nodes references by a way out
            //                    var ndsForWay = new Coord[wayNodesIds.Count];
            //                    for (int i = 0; i < wayNodesIds.Count; i++)
            //                    {
            //                        ndsForWay[i] = allNodes[wayNodesIds[i]];
            //                    }

            //                    if (matches.Ways[tagKey].ContainsKey(matchAllKey))
            //                    {
            //                        matches.AddWayGivenFeature(tagKey, tagValue, ndsForWay);
            //                    }
            //                    else if (matches.Ways[tagKey].ContainsKey(tagValue))
            //                    {
            //                        matches.AddWayGivenFeatureAndSubFeature(tagKey, tagValue, ndsForWay);
            //                    }
            //                }
            //                else
            //                {
            //                    // Parsing a node out
            //                    if (matches.Nodes[tagKey].ContainsKey(matchAllKey))
            //                    {
            //                        // If we are searching for all items within a feature then add it regardless
            //                        matches.AddNodeGivenFeature(tagKey, tagValue, allNodes[currentNodeId]);
            //                    }
            //                    else if (matches.Nodes[tagKey].ContainsKey(tagValue))
            //                    {
            //                        // If searching for a particular key:value only add if there is
            //                        matches.AddNodeGivenFeatureAndSubFeature(tagKey, tagValue, allNodes[currentNodeId]);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
        }

        // Identify a minimum and maximum boundary that encompasses all of the provided files' boundaries
        public static void GetBounds(ref RequestHandler result)
        {
            double? currentMinLat = null;
            double? currentMinLon = null;
            double? currentMaxLat = null;
            double? currentMaxLon = null;

            foreach (string providedXML in result.XmlCollection.ProvidedXMLs)
            {
                using (XmlReader reader = XmlReader.Create(new StringReader(providedXML)))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            if (reader.Name == "bounds")
                            {
                                CheckBounds(reader, ref currentMinLat, ref currentMinLon, ref currentMaxLat, ref currentMaxLon);
                            }
                        }
                    }
                }
            }

            result.MinBounds = new Coord(currentMinLat.Value, currentMinLon.Value);
            result.MaxBounds = new Coord(currentMaxLat.Value, currentMaxLon.Value);
        }

        private static void CheckBounds(XmlReader reader, ref double? currentMinLat, ref double? currentMinLon,
                                                          ref double? currentMaxLat, ref double? currentMaxLon)
        {
            var boundsMinLat = Convert.ToDouble(reader.GetAttribute("minlat"));
            if (!currentMinLat.HasValue || boundsMinLat < currentMinLat)
            {
                currentMinLat = boundsMinLat;
            }

            var boundsMinLon = Convert.ToDouble(reader.GetAttribute("minlon"));
            if (!currentMinLon.HasValue || boundsMinLon < currentMinLon)
            {
                currentMinLon = boundsMinLon;
            }

            var boundsMaxLat = Convert.ToDouble(reader.GetAttribute("maxlat"));
            if (!currentMaxLat.HasValue || boundsMaxLat > currentMaxLat)
            {
                currentMaxLat = boundsMaxLat;
            }

            var boundsMaxLon = Convert.ToDouble(reader.GetAttribute("maxlon"));
            if (!currentMaxLon.HasValue || boundsMaxLon > currentMaxLon)
            {
                currentMaxLon = boundsMaxLon;
            }
        }
    }
}
