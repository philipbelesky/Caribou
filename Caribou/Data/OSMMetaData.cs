namespace Caribou.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>A key:value pairing that might be found within a node/way's tag in an OSM XML file.
    /// Applies to both keys and values (and thus features and subfeatures). E.g. if amenity=restaurant, then
    /// both "amenity" and "restaurant" are OSMMetaData items, with the later having the former is its Key.</summary>
    public class OSMMetaData : IEquatable<OSMMetaData>
    {
        public OSMMetaData(string id, string name, bool isDefined, string explanation = "", OSMMetaData key = null)
        {
            this.Id = id;
            this.Key = key;
            this.IsDefined = isDefined;
            this.Name = name;
            this.Explanation = explanation;
        }

        public string Id { get; } // The type of information; can either represent a KEY or a VALUE. A sanitised value.
        public OSMMetaData Key { get; } // If set, points back to the key this value is assocaited with.
        public bool IsDefined { get; } // Is a pre-defined feature or subfeature (as defined in OSMDefinedFeature)

        public string Name { get; } // Readable name; i.e. including spaces and so on
        public string Explanation { get; } // A description of what this represents

        public override string ToString() => $"{this.Id}:{this.Key}, (defined:  {this.IsDefined})";

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
