using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caribou.Data
{
    /// <summary>
    /// Represents a geometry form (way or a node) that has been succesfully matched to a requested OSM Data 
    /// It is stored within a RequestHandler and then parsed out to specific Grasshopper data (geometry/text)
    /// </summary>
    public struct FoundItem
    {
        public FoundItem(Dictionary<string, string> tags, List<Coord> coords)
        {
            this.Tags = tags;
            this.Coords = coords;
        }

        public List<Coord> Coords { get; } // If only a single item then represents a node
        public Dictionary<string, string> Tags { get; } // All key:value pairs attached to the item
    }
}
