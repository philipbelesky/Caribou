namespace Caribou.Models
{
    using System;
    using Caribou.Data;

    /// <summary>
    /// A OSMMetaData item that will be presented to the user for interaction. It thus has state information and more descriptive information.
    /// </summary>
    public class OSMSelectableFeature : OSMMetaData
    {
        public bool IsSelected { get; set; }
        public int NodeCount { get; set; }
        public int RelationCount { get; set; }
        public int WayCount { get; set; }

        public OSMSelectableFeature(string id, string name, string explanation,
                                    int nodeCount, int relationCount, int wayCount,
                                    OSMSelectableFeature key)
            : base(id, name, explanation, key)
        {
            this.IsSelected = false;
            this.NodeCount = nodeCount;
            this.RelationCount = relationCount;
            this.WayCount = wayCount;
        }
    }
}
