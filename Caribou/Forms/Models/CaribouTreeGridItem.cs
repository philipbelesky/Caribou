namespace Caribou.Forms.Models
{
    using Caribou.Models;
    using Eto.Forms;

    /// <summary>
    /// Extension of the TreeGridItem class to contain the OSM item related to it
    /// </summary>
    public class CaribouTreeGridItem: TreeGridItem
    {
        private const int OBSCURITY_THRESHOLD = 5000;
        public OSMMetaData OSMData;

        public int NodeCount { get; set; }
        //public int RelationCount { get; set; } // Not useful as not filterable
        public int WayCount { get; set; }
        public bool IsObscure { get; set; }

        public CaribouTreeGridItem(OSMMetaData osmItem, int nodeCount, int wayCount, bool selected)
        {
            OSMData = osmItem;
            NodeCount = nodeCount;
            WayCount = wayCount;
            IsObscure = SetObscurity(); // Defined features;
            Values = this.GetColumnData(selected);
        }
        public bool IsSelected()
        {
            if ((this.Values[1] as string) == "True")
                return true;

            return false;
        }

        private bool SetObscurity()
        {
            if (this.OSMData.IsFeature() || this.OSMData.TagType == "yes")
                return false;

            return this.NodeCount + this.WayCount < OBSCURITY_THRESHOLD;
        }

        private string[] GetColumnData(bool selectedState) // For the form table
        {
            return new string[]
            {
                OSMData.Name, selectedState.ToString(),
                PresentCount(this.NodeCount), PresentCount(this.WayCount),
                OSMData.ToString(), "View", OSMData.Explanation, this.GetLink(),
            };
        }

        private string GetLink()
        {
            if (OSMData.IsFeature())
                return $"https://wiki.openstreetmap.org/wiki/Key:{this.OSMData.TagType}";

            return $"https://wiki.openstreetmap.org/wiki/Tag:{this.OSMData.ParentType}={this.OSMData.TagType}";
        }

        private string PresentCount(int countType)
        {
            if (this.OSMData.IsFeature())
                return "";

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
