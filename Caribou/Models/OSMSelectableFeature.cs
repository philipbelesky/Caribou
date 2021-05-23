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
        public bool ShowCounts { get; set; }

        // Full constructor
        public OSMSelectableFeature(string subfeature, string name, string description,
                                    int nodes, int ways, bool showCounts,
                                    OSMSelectableFeature key = null)
            : base(subfeature, name, description, key)
        {
            this.IsSelected = false;
            this.NodeCount = nodes;
            this.WayCount = ways;
            this.ShowCounts = showCounts;
        }

        public string[] GetColumnData() // For the form table
        {
            return new string[]
            {
                this.Name, this.IsSelected.ToString(),
                GetCount(this.NodeCount), GetCount(this.WayCount),
                this.ToString(), "View", this.Explanation
            };
        }

        private Uri GetLink()
        {
            if (this.IsFeature())
            {
                return new Uri($"//wiki.openstreetmap.org/wiki/Key:{this.ThisType}");
            }
            return new Uri($"//wiki.openstreetmap.org/wiki/Tag:{this.ParentType}={this.ThisType}");
        } 

        private string GetCount(int countType)
        {
            if (this.IsFeature() || !this.ShowCounts)
            {
                return "";
            }

            if (countType < 100)
                return "Very few";
            else if (countType < 1000)
                return "Few";
            else if (countType < 10000)
                return "Some";
            else if (countType < 100000)
                return "Many";
            else if (countType < 1000000)
                return "Heaps";

            return "?";
        }

        public bool IsRare()
        {
            return (this.NodeCount < 100 && this.WayCount < 100);
        }

        // Used in the UI form to sort by alphabetical 
        public int CompareTo(OSMSelectableFeature other)
        {
            return this.Name.CompareTo(other.Name);
        }
    }
}
