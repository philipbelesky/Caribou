namespace Caribou.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// A key or key:value pairing that might be found within a node or way's tags in an OSM XML file.
    /// Applies to both features:subfeatures and arbitrary key:values. E.g. if amenity=restaurant, then
    /// both "amenity" and "restaurant" are OSMMetaData items, with the later having the former is its Key.
    /// (This is because it is possible to search for all items with a key regardless of values (e.g. all buildings).
    /// </summary>
    public class OSMMetaData : IEquatable<OSMMetaData>
    {
        // Constructing from Grasshopper data
        public OSMMetaData(string specifiedId, string specifiedKey = null)
        {
            this.Id = specifiedId;
            this.IsDefined = false;

            // If providing a top-level defined primary feature like "natural" return that object
            if (specifiedKey == null && OSMDefinedFeatures.Primary.ContainsKey(specifiedId))
            {
                this.IsDefined = true;
                this.Name = OSMDefinedFeatures.Primary[specifiedId].Name;
                this.Explanation = OSMDefinedFeatures.Primary[specifiedId].Explanation;
                this.Key = null;
                return;
            }

            // If providing a specific type of information to find/match, e.g. amenity:restaurant
            // and the key (e.g. "amenity") is predefined then set parent from hardcoded data
            if (specifiedKey != null && OSMDefinedFeatures.Primary.ContainsKey(specifiedKey))
            {
                this.Key = OSMDefinedFeatures.Primary[specifiedKey];
            }
        }

        // Constructing from hardcoded data
        public OSMMetaData(string id, string name, string explanation, OSMMetaData key = null)
        {
            this.Id = id;
            this.Name = name;
            this.IsDefined = true;
            this.Explanation = explanation;
            this.Key = key;
        }

        public string Id { get; } // The type of information; can either represent a KEY or a VALUE. A sanitised value.
        public OSMMetaData Key { get; } // If set, points back to the key this value is assocaited with.
        public bool IsDefined { get; } // Is a pre-defined feature or subfeature (as defined in OSMDefinedFeature)

        public string Name { get; } // Readable name; i.e. including spaces and so on
        public string Explanation { get; } // A description of what this represents

        public override string ToString() => $"{this.Id}:{this.Key}, (defined: {this.IsDefined})";

        public bool IsFeature()
        {
            return this.IsDefined && this.Key == null;
        }

        public bool IsSubFeature()
        {
            return this.IsDefined && this.Key != null;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as OSMMetaData);
        }

        public bool Equals(OSMMetaData other)
        {
            return other != null &&
                   Id == other.Id &&
                   EqualityComparer<OSMMetaData>.Default.Equals(Key, other.Key) &&
                   IsDefined == other.IsDefined;
        }
    }
}
