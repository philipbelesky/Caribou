﻿namespace Caribou.Processing
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Xml;
    using Caribou.Models;

    /// <summary>
    /// Methods for parsing an XML file and extracting data that use XMLReader-based methods.
    /// </summary>
    public static class ParseViaXMLReader
    {
        private delegate void DispatchDelegate(XmlReader reader, ref RequestHandler request, 
                                               int fileIndex, bool onlyBuildings = false);
        private static readonly CultureInfo CI = CultureInfo.InvariantCulture;

        public static void FindItemsByTag(ref RequestHandler request, OSMGeometryType typeToFind, bool pathIsContents = false)
        {
            var dispatchForType = GetDispatchForType(typeToFind);
            bool onlyBuildings = false;
            if (typeToFind == OSMGeometryType.Building)
                onlyBuildings = true;

            GetBounds(ref request, pathIsContents);
            for (var i = 0; i < request.XmlPaths.Count; i++)
            {
                var xmlPath = request.XmlPaths[i];
                if (pathIsContents)
                {
                    using (XmlReader reader = XmlReader.Create(new StringReader(xmlPath)))
                    {
                        dispatchForType(reader, ref request, i, onlyBuildings); // Only used in testing
                    }
                }
                else
                {
                    using (XmlReader reader = XmlReader.Create(xmlPath))
                    {
                        dispatchForType(reader, ref request, i);
                    }
                }
            }
        }

        public static void FindNodesInXML(XmlReader reader, ref RequestHandler request, int fileIndex, bool onlyBuildings)
        {
            string currentNodeId = "";
            double currentLat = 0;
            double currentLon = 0;
            var currentNodeMetaData = new Dictionary<string, string>();
            var xli = (IXmlLineInfo)reader; // Used to track read progress
            var nodesCollected = 0;

            // Loop (linearly) through all tags. Keep track of each node's metadata and coords while looping through its tags.
            // When encountering the next node add the tracked data.
            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    if (reader.Name == "node")
                    {
                        currentNodeId = reader.GetAttribute("id");
                        currentLat = Convert.ToDouble(reader.GetAttribute("lat"), CI);
                        currentLon = Convert.ToDouble(reader.GetAttribute("lon"), CI);
                    }
                    else if (reader.Name == "tag")
                    {
                        currentNodeMetaData[reader.GetAttribute("k").ToLower(CI)] = reader.GetAttribute("v");
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
                    nodesCollected++;
                    if (nodesCollected % 3000 == 0) // roughly every half a second
                        ProgressReporting.Ping(xli.LineNumber, fileIndex, request);
                }
            }
        }

        public static void FindWaysInXML(XmlReader reader, ref RequestHandler request, int fileIndex, bool onlyBuildings)
        {
            string currentWayId = "";
            var currentWayMetaData = new Dictionary<string, string>();
            var currentWayNodes = new List<Coord>();
            var allNodes = new Dictionary<string, Coord>();
            var inANode = false; // Only needed for ways
            var xli = (IXmlLineInfo)reader; // Used to track read progress
            var waysCollected = 0;

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
                            Convert.ToDouble(reader.GetAttribute("lat"), CI),
                            Convert.ToDouble(reader.GetAttribute("lon"), CI));
                    }
                    else if (inANode && reader.Name == "nd")
                    {
                        var ndId = reader.GetAttribute("ref");
                        currentWayNodes.Add(allNodes[ndId]);
                    }
                    else if (inANode && reader.Name == "tag")
                    {
                        currentWayMetaData[reader.GetAttribute("k").ToLower(CI)] = reader.GetAttribute("v");
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
                        // If finished looping over a prior node
                        if (onlyBuildings) 
                            request.AddBuildingIfMatchesRequest(currentWayId, currentWayMetaData, currentWayNodes); 
                        else
                            request.AddWayIfMatchesRequest(currentWayId, currentWayMetaData, currentWayNodes);

                        waysCollected += 1;
                        if (waysCollected % 2000 == 0)
                            ProgressReporting.Ping(xli.LineNumber, fileIndex, request);
                    }

                    currentWayMetaData.Clear();
                    currentWayNodes.Clear();
                }
            }
        }

        // Retur n correct parser for each geometry type
        private static DispatchDelegate GetDispatchForType(OSMGeometryType typeToFind)
        {
            DispatchDelegate dispatchForType;
            if (typeToFind == OSMGeometryType.Node)
                dispatchForType = FindNodesInXML;
            else if (typeToFind == OSMGeometryType.Way || typeToFind == OSMGeometryType.Building)
                dispatchForType = FindWaysInXML;
            else
                dispatchForType = null; // Necessary to prevent below paths thinking variable not set
            return dispatchForType;
        }

        // Identify a minimum and maximum boundary that encompasses all of the provided files' boundaries
        public static void GetBounds(ref RequestHandler result, bool readPathAsContents = false)
        {
            double? currentMinLat = null;
            double? currentMinLon = null;
            double? currentMaxLat = null;
            double? currentMaxLon = null;
            result.AllBounds = new List<Tuple<Coord, Coord>>();

            foreach (string xmlPath in result.XmlPaths)
            {
                XmlReader reader;
                if (readPathAsContents) 
                    reader = XmlReader.Create(new StringReader(xmlPath)); // Only used in testing
                else
                    reader = XmlReader.Create(xmlPath);
   
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        if (reader.Name == "bounds")
                        {
                            var minLat = Convert.ToDouble(reader.GetAttribute("minlat"), CI);
                            var minLon = Convert.ToDouble(reader.GetAttribute("minlon"), CI);
                            var maxLat = Convert.ToDouble(reader.GetAttribute("maxlat"), CI);
                            var maxLon = Convert.ToDouble(reader.GetAttribute("maxlon"), CI);
                            var bounds = new Tuple<Coord, Coord>(new Coord(minLat, minLon), new Coord(maxLat, maxLon));
                            result.AllBounds.Add(bounds);

                            CheckBounds(bounds, ref currentMinLat, ref currentMinLon, ref currentMaxLat, ref currentMaxLon);
                            break;
                        }
                    }
                }
            }

            result.MinBounds = new Coord(currentMinLat.Value, currentMinLon.Value);
            result.MaxBounds = new Coord(currentMaxLat.Value, currentMaxLon.Value);
        }

        public static void CheckBounds(Tuple<Coord, Coord> bounds, 
            ref double? currentMinLat, ref double? currentMinLon, ref double? currentMaxLat, ref double? currentMaxLon)
        {
            var boundsMinLat = bounds.Item1.Latitude;
            if (!currentMinLat.HasValue || boundsMinLat < currentMinLat)
            {
                currentMinLat = boundsMinLat;
            }

            var boundsMinLon = bounds.Item1.Longitude;
            if (!currentMinLon.HasValue || boundsMinLon < currentMinLon)
            {
                currentMinLon = boundsMinLon;
            }

            var boundsMaxLat = bounds.Item2.Latitude;
            if (!currentMaxLat.HasValue || boundsMaxLat > currentMaxLat)
            {
                currentMaxLat = boundsMaxLat;
            }

            var boundsMaxLon = bounds.Item2.Longitude;
            if (!currentMaxLon.HasValue || boundsMaxLon > currentMaxLon)
            {
                currentMaxLon = boundsMaxLon;
            }
        }
    }
}
