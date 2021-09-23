namespace Caribou.Models
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Grasshopper.Kernel.Types;

    /// <summary>
    /// A key or key:value pairing that might be found within a node or way's tags in an OSM XML file.
    /// Applies to both features:subfeatures and arbitrary key:values. E.g. if amenity=restaurant, then
    /// both "amenity" and "restaurant" are OSMMetaData items, with the later having the former is its Key.
    /// (This is because it is possible to search for all items with a key regardless of values (e.g. all buildings).
    /// </summary>
    public class OSMMetaData : IEquatable<OSMMetaData>
    {
        protected const char SplitChar = '='; // Can't use ":" because that is used within OSM keys, like addr:housenumber
        public string TagType { get; } // The type of information; can either represent a KEY or a VALUE. A sanitised value.
        public OSMMetaData ParentType { get; } // If set, points back to the key this value is associated with.
        public bool IsDefined { get; } // Is a pre-defined feature or subfeature (as defined in OSMDefinedFeature)
        public string Name { get; } // Readable name; i.e. including spaces and so on
        public string Explanation { get; } // A description of what this represents

        // Constructing from a raw keyValue string; e.g. "highway=residential or highway=yes or highway=* or highway"
        public OSMMetaData(string rawKeyValue)
        {  
            this.ParentType = null;
            this.IsDefined = false;

            if (!rawKeyValue.Contains(SplitChar)) // If the text is just "something"
                this.TagType = rawKeyValue.ToLower(CultureInfo.InvariantCulture); // Split at = and take left-side
            else 
            {
                // If the text is something=something
                var rawComponents = rawKeyValue.Trim().Split(SplitChar);

                // Make a parent if there is a "=something" (e.g. not a sole key) and if that =value is not null or  =*
                if (rawComponents.Length >= 2 && rawComponents[1] != "*" && !string.IsNullOrEmpty(rawComponents[1]))
                {
                    // E.g. PARENT=CHILD
                    this.TagType = rawComponents[1].ToLower(CultureInfo.InvariantCulture);
                    this.ParentType = new OSMMetaData(rawComponents[0]);
                }
                else
                {
                    // E.g. CHILD=* or CHILD
                    this.TagType = rawComponents[0].ToLower(CultureInfo.InvariantCulture); // Split at = and take left-side
                }
            }

            // Lookup if the type is known (e.g. a defined Feature/Subfeature)
            if (this.ParentType == null && OSMDefinedFeatures.Primary.ContainsKey(this.TagType))
            {
                // If providing a top-level defined primary feature like "natural" return that object
                this.IsDefined = true;
                this.Explanation = MakeNiceExplanation(OSMDefinedFeatures.Primary[this.TagType].Explanation);
                this.Name = OSMDefinedFeatures.Primary[this.TagType].Name;
            }
            else
            {
                if (this.ParentType != null)
                {
                    // If providing arbitrary key:value pairings then create the parent rather than referencing
                    this.Name = MakeNiceName(null, this.TagType, this.ParentType.ToString());
                }
                else
                {
                    this.Name = MakeNiceName(null, this.TagType, null);
                }
            }
        }

        // If providing an explicit key and value, just call the other constructor using the standard format
        public OSMMetaData(string tagValue, string tagType) : this($"{tagValue}={tagType}") { }

        // Constructing from hardcoded data; e.g. loading from library of feature definitionss
        public OSMMetaData(string id, string name, string explanation, OSMMetaData parent = null)
        {
            this.TagType = id;
            this.IsDefined = true;
            this.Explanation = MakeNiceExplanation(explanation);
            this.ParentType = parent;

            if (parent != null)
                this.Name = MakeNiceName(name, id, parent.Name);
            else
                this.Name = MakeNiceName(name, id, "");
        }

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

        private string SingleSearchNiceName() => $"{this.TagType}=*";

        private string MultiSearchNiceName() => $"{(this.ParentType == null ? "*" : this.ParentType.TagType)}={this.TagType}";

        public bool IsFeature() => this.IsDefined && this.ParentType == null;

        public bool IsSubFeature() => this.IsDefined && this.ParentType != null;

        // IEquitable implementations
        public override bool Equals(object obj) => Equals(obj as OSMMetaData);

        public bool Equals(OSMMetaData other)
        {
            return other != null &&
                   this.TagType == other.TagType &&
                   EqualityComparer<OSMMetaData>.Default.Equals(this.ParentType, other.ParentType);
        }

        // Need to provide a hash code as we are using this as a dictionary key in RequestHandler
        public override int GetHashCode()
        {
            if (this.ParentType == null)
                return this.TagType.GetHashCode();
            else
                return this.TagType.GetHashCode() ^ this.ParentType.GetHashCode();
        }
    }
}
