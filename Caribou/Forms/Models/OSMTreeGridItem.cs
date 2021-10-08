namespace Caribou.Forms.Models
{
    using Caribou.Models;
    using Eto.Forms;

    /// <summary>
    /// Extension of the TreeGridItem class to contain the OSM item related to it and custom presentation logic
    /// </summary>
    public class OSMTreeGridItem: TreeGridItem
    {
        private const int OBSCURITY_THRESHOLD = 5000;
        public OSMTag OSMData;
        public bool IsObscure { get; set; }
        public bool IsParsed { get; set; } // Coming from already-parsed tag lists; not a predefined list

        public OSMTreeGridItem(OSMTag osmItem, int nodeCount, int wayCount,
            bool showCounts, bool selected, bool expanded = false)
        {
            osmItem.NodeCount = nodeCount;
            osmItem.WayCount = wayCount;
            OSMData = osmItem;

            Expanded = expanded;
            IsParsed = showCounts;
            IsObscure = GetObscurity(); // Defined features;
            Values = this.GetColumnData(selected);
        }

        public bool IsSelected()
        {
            if ((this.Values[1] as string) == "True")
                return true;

            return false;
        }

        private bool GetObscurity()
        {
            if (!IsParsed && this.OSMData.Value == "yes") // Untagged items, e.g. building=yes, always shown
                return false;
            if (IsParsed)
                if (OSMUniqueTags.names.ContainsKey(this.OSMData.Value))
                    return true;
                else if (this.OSMData.Key != null && OSMUniqueTags.names.ContainsKey(this.OSMData.Key.Value))
                    return true;
                else
                    return false;
            else if (this.OSMData.NodeCount > 0 || this.OSMData.WayCount > 0)
                return this.OSMData.NodeCount + this.OSMData.WayCount < OBSCURITY_THRESHOLD;

            return false;
        }

        private string[] GetColumnData(bool selectedState) // For the form table
        {
            return new string[]
            {
                OSMData.Name, selectedState.ToString(),
                GetCount(this.OSMData.NodeCount), GetCount(this.OSMData.WayCount),
                OSMData.ToString(), "View", OSMData.Description, this.GetLink(),
            };
        }

        private string GetLink()
        {
            if (OSMData.IsParent())
                return $"https://wiki.openstreetmap.org/wiki/Key:{this.OSMData.Value}";

            return $"https://wiki.openstreetmap.org/wiki/Tag:{this.OSMData.Key}={this.OSMData.Value}";
        }

        private string GetCount(int countType)
        {
            if (this.OSMData.IsParent()) // Top levels have no count
                return "";
            else if(this.IsParsed)
                return countType.ToString();

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
    }
}
