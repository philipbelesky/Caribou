namespace Caribou.Forms.Models
{
    using Caribou.Models;
    using Eto.Forms;

    /// <summary>
    /// Extension of the TreeGridItem class to contain the OSM item related to it and custom presentation logic
    /// </summary>
    public class CaribouTreeGridItem: TreeGridItem
    {
        private const int OBSCURITY_THRESHOLD = 5000;
        public OSMTag OSMData;
        public bool IsObscure { get; set; }

        public CaribouTreeGridItem(OSMTag osmItem, int nodeCount, int wayCount, bool selected, bool expanded = false)
        {
            OSMData = osmItem;
            IsObscure = GetObscurity(); // Defined features;
            Values = this.GetColumnData(selected);
            Expanded = expanded;
            osmItem.NodeCount = nodeCount;
            osmItem.WayCount = wayCount;
        }

        public bool IsSelected()
        {
            if ((this.Values[1] as string) == "True")
                return true;

            return false;
        }

        private bool GetObscurity()
        {
            if (this.OSMData.Value == "yes") // Untagged items, e.g. building=yes, always shown
                return false;
            else if (this.OSMData.NodeCount > 0 || this.OSMData.WayCount > 0)
                return this.OSMData.NodeCount + this.OSMData.WayCount < OBSCURITY_THRESHOLD;
            else if (OSMUniqueTags.names.ContainsKey(this.OSMData.Value))
                return true;

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
            // TODO: wire up some sort of flag for when to return raw vs actual counts
            // E.g. for filter form vs picker form
            //if (!this.OSMData.IsDefined) // When using the filter form
            //    return (OSMData.NodeCount + OSMData.WayCount).ToString();

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
