namespace Caribou.Processing
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
    using Caribou.Data;

    /// <summary>
    /// Methods for parsing an XML file and extracting data that use XMLReader-based methods.
    /// </summary>
    public static class ParseViaXMLReader
    {
        public static void FindItemsByTag(ref RequestHandler request, OSMGeometryType typeToFind)
        {
            GetBounds(ref request);

            foreach (string providedXML in request.XmlCollection.ProvidedXMLs)
            {
                using (XmlReader reader = XmlReader.Create(new StringReader(providedXML)))
                {
                    if (typeToFind == OSMGeometryType.Way)
                    {
                        FindWaysInXML(reader, ref request);
                    }
                    else if (typeToFind == OSMGeometryType.Node)
                    {
                        FindNodesInXML(reader, ref request);
                    }
                }
            }
        }

        public static void FindNodesInXML(XmlReader reader, ref RequestHandler request)
        {
            string currentNodeId = "";
            double currentLat = 0;
            double currentLon = 0;
            var currentNodeMetaData = new Dictionary<string, string>();

            // Loop (linearly) through all tags. Keep track of each node's metadata and coords while looping through its tags.
            // When encountering the next node add the tracked data.
            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    if (reader.Name == "node")
                    {
                        currentNodeId = reader.GetAttribute("id");
                        currentLat = Convert.ToDouble(reader.GetAttribute("lat"));
                        currentLon = Convert.ToDouble(reader.GetAttribute("lon"));
                    }
                    else if (reader.Name == "tag")
                    {
                        currentNodeMetaData[reader.GetAttribute("k").ToLower()] = reader.GetAttribute("v");
                    }
                    else if (reader.Name == "way")
                    {
                         break;
                    }
                }
                else if (reader.Name == "node") // Closing out a node tag
                {
                    request.AddNodeIfMatchesRequest(currentNodeId, currentNodeMetaData, currentLat, currentLon);
                    currentNodeMetaData.Clear();
                }
            }
        }

        public static void FindWaysInXML(XmlReader reader, ref RequestHandler request)
        {
            string currentWayId = "";
            var currentWayMetaData = new Dictionary<string, string>();
            var currentWayNodes = new List<Coord>();
            var allNodes = new Dictionary<string, Coord>();
            var inANode = false; // Only needed for ways

            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    if (reader.Name == "way")
                    {
                        currentWayId = reader.GetAttribute("id");
                        inANode = true; // Add tag data
                    }
                    else if (reader.Name == "node")
                    {
                        var nodeId = reader.GetAttribute("id");
                        allNodes[nodeId] = new Coord(
                            Convert.ToDouble(reader.GetAttribute("lat")),
                            Convert.ToDouble(reader.GetAttribute("lon")));
                    }
                    else if (inANode && reader.Name == "nd")
                    {
                        var ndId = reader.GetAttribute("ref");
                        currentWayNodes.Add(allNodes[ndId]);
                    }
                    else if (inANode && reader.Name == "tag")
                    {
                        currentWayMetaData[reader.GetAttribute("k").ToLower()] = reader.GetAttribute("v");
                    }
                    else if (reader.Name == "relation")
                    {
                        break;
                    }
                }
                else
                {
                    inANode = false;
                    if (reader.Name == "way") // Closing out a way tag
                    {
                        request.AddWayIfMatchesRequest(currentWayId, currentWayMetaData, currentWayNodes); // If finished looping over a prior node                    currentWayMetaData.Clear();
                    }

                    currentWayMetaData.Clear();
                    currentWayNodes.Clear();
                }
            }
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
                                break;
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
