﻿namespace Caribou.Forms
{
    using System;
    using Caribou.Models;

    /// <summary>A OSMMetaData item that will be presented to the user for interaction. It thus has state information and more descriptive information.</summary>
    public class OSMSelectableData : OSMMetaData, IComparable<OSMSelectableData>
    {
        private const int OBSCURITY_THRESHOLD = 5000;
        public bool IsSelected { get; set; }
        public int NodeCount { get; set; }
        //public int RelationCount { get; set; } // Not useful as not filterable
        public int WayCount { get; set; }
        public bool ShowCounts { get; set; }

        // Full constructor
        public OSMSelectableData(string subfeature, string name, string description,
                                    int nodes, int ways, bool showCounts,
                                    OSMSelectableData key = null)
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
                this.ToString(), "View", this.Explanation, this.GetLink(),
            };
        }

        #region Dynamic Properties
        private string GetLink()
        {
            if (this.IsFeature())
            {
                return $"https://wiki.openstreetmap.org/wiki/Key:{this.TagType}";
            }

            return $"https://wiki.openstreetmap.org/wiki/Tag:{this.ParentType}={this.TagType}";
        }

        private string GetCount(int countType)
        {
            if (this.IsFeature() || !this.ShowCounts)
            {
                return "";
            }
            
            if (countType < 100)
                return "Very Rare";
            else if (countType < 1000)
                return "Rare";
            else if (countType < 10000)
                return "Uncommon";
            else if (countType < 100000)
                return "Common";
            else if (countType < 1000000)
                return "Very Common";
            else if (countType < 10000000)
                return "Abundant";
            else
                return "Heaps";
        }

        public bool IsObscure()
        {
            return this.NodeCount + this.WayCount < OBSCURITY_THRESHOLD; // Defined features
        }
        #endregion

        // Used in the UI form to sort by alphabetical
        public int CompareTo(OSMSelectableData other)
        {
            return this.Name.CompareTo(other.Name);
        }
    }
}
