namespace Caribou.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    public struct FeatureDefinition
    {
        public FeatureDefinition(string id, string name, string explanation, List<FeatureDefinition> subFeatures = null)
        {
            this.Id = id;
            this.Name = name;
            this.Explanation = explanation;
            this.SubFeatures = subFeatures;
        }

        public string Id { get; }
        public string Name { get; }
        public string Explanation { get; }
        public List<FeatureDefinition> SubFeatures { get; }

        public override string ToString() => $"({this.Name}, {this.Explanation})";
    }
}
