namespace Caribou.Processing
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
    using Caribou.Data;

    public class ParseViaXMLReader
    {
        public static RequestResults FindByFeatures(List<FeatureRequest> featuresSpecified, string xmlContents)
        {
            var matches = new RequestResults(featuresSpecified); // Output
            var matchAllKey = FeatureRequest.SearchAllKey;
            var allNodes = new Dictionary<string, Coord>(); // Dict used to lookup a way's nodes
            GetBounds(ref matches, xmlContents); // Add minmax latlon to matches

            using (XmlReader reader = XmlReader.Create(new StringReader(xmlContents)))
            {
                // XmlReader is forward-only so need to track the lat/long of the parent element to use if we find a match
                string currentNodeId = ""; // Current node; need to track it for when parsing inside-nodes
                string tagKey; 
                string tagValue;
                bool inAWay = false; // Need to keep track of when parsing inside Ways so we know wether to add tag info to node or way lists
                List<string> wayNodesIds = new List<string>(); // If parsing inside Ways need to track the different nodes that make it up

                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        if (reader.Name == "node")
                        {
                            wayNodesIds.Clear();
                            inAWay = false;
                            currentNodeId = reader.GetAttribute("id");
                            allNodes[currentNodeId] = new Coord(
                                Convert.ToDouble(reader.GetAttribute("lat")),
                                Convert.ToDouble(reader.GetAttribute("lon"))
                            );
                        }
                        else if (reader.Name == "way")
                        {
                            wayNodesIds.Clear();
                            inAWay = true;
                        }
                        else if (reader.Name == "nd")
                        {
                            wayNodesIds.Add(reader.GetAttribute("ref"));
                        }
                        else if (reader.Name == "tag")
                        {
                            tagKey = reader.GetAttribute("k");
                            if (matches.PrimaryFeaturesToFind.Contains(tagKey))
                            {
                                tagValue = reader.GetAttribute("v");
                                if (inAWay) {
                                    // Parsing a collections of nodes references by a way out
                                    var ndsForWay = new Coord[wayNodesIds.Count];
                                    for (int i = 0; i < wayNodesIds.Count; i++)
                                    {
                                        ndsForWay[i] = allNodes[wayNodesIds[i]];
                                    }

                                    if (matches.Ways[tagKey].ContainsKey(matchAllKey))
                                    {
                                        matches.AddWayGivenFeature(tagKey, tagValue, ndsForWay);
                                    }
                                    else if (matches.Ways[tagKey].ContainsKey(tagValue))
                                    {
                                        matches.AddWayGivenFeatureAndSubFeature(tagKey, tagValue, ndsForWay);
                                    }
                                    inAWay = false;
                                } 
                                else
                                {
                                    // Parsing a node out
                                    if (matches.Nodes[tagKey].ContainsKey(matchAllKey))
                                    {
                                        // If we are searching for all items within a feature then add it regardless
                                        matches.AddNodeGivenFeature(tagKey, tagValue, allNodes[currentNodeId]);
                                    }
                                    else if (matches.Nodes[tagKey].ContainsKey(tagValue))
                                    {
                                        // If searching for a particular key:value only add if there is
                                        matches.AddNodeGivenFeatureAndSubFeature(tagKey, tagValue, allNodes[currentNodeId]);
                                    }
                                }

                            }
                        }
                    }
                }
            }

            return matches;
        }

        private static void GetBounds(ref RequestResults matches, string xmlContents)
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
