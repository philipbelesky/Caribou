namespace Caribou.Models
{
    using System;
    using Caribou.Data;

    /// <summary>
    /// A OSMMetaData item that will be presented to the user for interaction. It thus has state information and more descriptive information.
    /// </summary>
    public class OSMSelectableFeature : OSMMetaData, IComparable<OSMSelectableFeature>
    {
        public bool IsSelected { get; set; }
        public int NodeCount { get; set; }
        //public int RelationCount { get; set; } // Not useful as not filterable
        public int WayCount { get; set; }

        // Full constructor
        public OSMSelectableFeature(string subfeature, string name, string description,
                                    int nodes, int ways,
                                    OSMSelectableFeature key = null)
            : base(subfeature, name, description, key)
        {
            this.IsSelected = false;
            this.NodeCount = nodes;
            this.WayCount = ways;
        }

        // Used in the UI form to sort by alphabetical 
        public int CompareTo(OSMSelectableFeature other)
        {
            return this.Name.CompareTo(other.Name);
        }
    }
}
