namespace Caribou.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
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
            this.ThisType = specifiedId.ToLower(CultureInfo.InvariantCulture);
            this.IsDefined = false;

            if (specifiedKey == null && OSMDefinedFeatures.Primary.ContainsKey(specifiedId))
            {
                // If providing a top-level defined primary feature like "natural" return that object
                this.IsDefined = true;
                this.Name = OSMDefinedFeatures.Primary[specifiedId].Name;
                this.Explanation = MakeNiceExplanation(OSMDefinedFeatures.Primary[specifiedId].Explanation);
                this.ParentType = null;
            }
            else
            {
                if (specifiedKey != null && OSMDefinedFeatures.Primary.ContainsKey(specifiedKey))
                {
                    // If providing a specific type of information to find/match, e.g. amenity:restaurant
                    // and the key (e.g. "amenity") is predefined then set parent from hardcoded data
                    this.ParentType = OSMDefinedFeatures.Primary[specifiedKey];
                }
                else if (specifiedKey != null)
                {
                    // If providing arbitrary key:value pairings then create the parent rather than referencing
                    this.ParentType = new OSMMetaData(specifiedKey);
                }

                this.Name = MakeNiceName(null, specifiedId, specifiedKey);
            }
        }

        // Constructing from hardcoded data
        public OSMMetaData(string id, string name, string explanation, OSMMetaData key = null)
        {
            this.ThisType = id;
            this.IsDefined = true;
            this.Explanation = MakeNiceExplanation(explanation);
            if (key != null)
                this.Name = MakeNiceName(name, id, key.Name);
            else
                this.Name = MakeNiceName(name, id, "");

            this.ParentType = key;
        }

        public string ThisType { get; } // The type of information; can either represent a KEY or a VALUE. A sanitised value.
        public OSMMetaData ParentType { get; } // If set, points back to the key this value is assocaited with.
        public bool IsDefined { get; } // Is a pre-defined feature or subfeature (as defined in OSMDefinedFeature)

        public string Name { get; } // Readable name; i.e. including spaces and so on
        public string Explanation { get; } // A description of what this represents

        public override string ToString() => this.IsFeature() ? this.SingleSearchNiceName() : this.MultiSearchNiceName();

        private static string MakeNiceName(string providedName, string providedID, string providedParent)
        {
            if (!string.IsNullOrEmpty(providedName))
            {
                return providedName;
            }
            else if (string.IsNullOrEmpty(providedID))
            {
                return "*";
            }

            // Try and make a nice name for layer bakes, etc
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            var name = providedID.Replace("_", " ");
            if (name == "yes")
            {
                name = providedParent + " (Untagged)";
            }

            return textInfo.ToTitleCase(name);
        }

        private static string MakeNiceExplanation(string explanation)
        {
            if (!string.IsNullOrEmpty(explanation))
            {
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                explanation = textInfo.ToTitleCase(explanation);
                if (explanation.Last() != '.')
                {
                    explanation += '.';
                }
            }

            return explanation;
        }

        private string SingleSearchNiceName()
        {
            return $"{this.ThisType}=*";
        }

        private string MultiSearchNiceName()
        {
            return $"{this.KeyNiceName()}={this.ThisType}";
        }

        private string KeyNiceName()
        {
            return this.ParentType == null ? "*" : this.ParentType.ThisType;
        }

        public bool IsFeature()
        {
            return this.IsDefined && this.ParentType == null;
        }

        public bool IsSubFeature()
        {
            return this.IsDefined && this.ParentType != null;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as OSMMetaData);
        }

        public bool Equals(OSMMetaData other)
        {
            return other != null &&
                   this.ThisType == other.ThisType &&
                   EqualityComparer<OSMMetaData>.Default.Equals(this.ParentType, other.ParentType);
        }

        // Need to provide a hash code as we are using this as a dictionary key in RequestHandler
        public override int GetHashCode()
        {
            if (this.ParentType == null)
            {
                return this.ThisType.GetHashCode();
            }
            else
            {
                return this.ThisType.GetHashCode() ^ this.ParentType.GetHashCode();
            }
        }
    }
}
