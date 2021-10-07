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
    public class OSMTag : IEquatable<OSMTag>
    {
        #region Instance
        protected const char SplitChar = '='; // Can't use ":" because that is used within OSM keys, like addr:housenumber
        public readonly OSMTag Key = null; // If set, points back to the key this value is associated with.
        public readonly string Value; // The type of information; can represent a keyless value; e.g. "building"
        public readonly string Name;  // Readable name; i.e. including spaces and so on
        public readonly string Description; // A description of what this represents
        public int NodeCount { get; set; }
        public int RelationCount { get; set; }
        public int WayCount { get; set; }
        private TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
        #endregion

        #region Constructors

        // Constructing from a raw keyValue string; e.g. "highway=residential or highway=yes or highway=* or highway"
        public OSMTag(string rawKeyValue)
        {
            string key, value;
            GetKeyValueComponents(rawKeyValue, out key, out value);
            // Possible options (string) > key:value
            // building             -> building:null        -> type=building, no parent
            // building=*           -> building:null        -> type=building, no parent
            // building=hospital    -> building:hospital    -> type=hospital, building=parent
            string explanation = "";
            string name = "";
            string tagType = "";
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            if (String.IsNullOrEmpty(value) || value == "*") // If providing a top-level feature like "natural"
            {
                if (OSMPrimaryTypes.Keys.ContainsKey(key)) // If that top-level feature is defined
                {
                    name = OSMPrimaryTypes.Keys[key].Name;
                    explanation = OSMPrimaryTypes.Keys[key].Description;
                    tagType = key;
                }
                else if (OSMArbitraryTypes.Keys.ContainsKey(key))
                {
                    name = OSMArbitraryTypes.Keys[key]["key"];
                    explanation = OSMArbitraryTypes.Keys[key]["description"];
                    tagType = key;
                }
                else
                    tagType = key;
            }
            else // If providing a key value pair
            {
                // Create object for the parent/key
                if (OSMPrimaryTypes.Keys.ContainsKey(key)) 
                    Key = OSMPrimaryTypes.Keys[key];
                else if (OSMArbitraryTypes.Keys.ContainsKey(key))
                {
                    var item = OSMArbitraryTypes.Keys[key];
                    Key = new OSMTag(item["key"], textInfo.ToTitleCase(item["key"]), item["description"]);
                }
                else
                    Key = new OSMTag(key);

                // Create object for the current item/child/value
                if (OSMPrimaryTypes.Values.ContainsKey(rawKeyValue))
                {
                    var item = OSMPrimaryTypes.Values[rawKeyValue];
                    name = textInfo.ToTitleCase(item["value"]);
                    explanation = item["description"];
                    NodeCount = int.Parse(item["nodes"]);
                    WayCount = int.Parse(item["ways"]);
                    RelationCount = int.Parse(item["relations"]);
                    tagType = value;
                }
                else if (OSMArbitraryTypes.Values.ContainsKey(rawKeyValue))
                {
                    var item = OSMArbitraryTypes.Values[rawKeyValue];
                    name = textInfo.ToTitleCase(item["value"]);
                    explanation = item["description"];
                    NodeCount = int.Parse(item["nodes"]);
                    WayCount = int.Parse(item["ways"]);
                    RelationCount = int.Parse(item["relations"]);
                    tagType = value;
                }
                else
                {
                    tagType = value;
                }
            }

            Value = tagType;
            Name = MakeNiceName(name, tagType, this.Key);
            Description = explanation;
        }

        // If providing an explicit key and value, just call the other constructor using the standard format
        public OSMTag(string tagKey, string tagValue) : this($"{tagKey}={tagValue}") { }

        // Constructing from hardcoded data; e.g. loading from library of feature definitions (only used by primary keys)
        public OSMTag(string value, string name, string description, OSMTag parent = null)
        {
            Value = value;
            Name = MakeNiceName(name, value, parent);
            Description = description;
            this.Key = parent;
        }

        private void GetKeyValueComponents(string rawKeyValue, out string key, out string value)
        {
            // If the text is just "something"; not something=something
            if (!rawKeyValue.Contains(SplitChar))
            {
                key = rawKeyValue.ToLower(CultureInfo.InvariantCulture);
                value = null;
                return;
            }

            var rawComponents = rawKeyValue.Trim().ToLower(CultureInfo.InvariantCulture).Split(SplitChar);

            // Make a parent if there is a "=something" (e.g. not a sole key) and if that =value is not null or  =*
            if (rawComponents.Length >= 2 && rawComponents[1] != "*" && !string.IsNullOrEmpty(rawComponents[1]))
            {
                key = rawComponents[0];
                value = rawComponents[1];
            }
            else // If the item is something=
            {
                key = rawComponents[0];
                value = null;
            }

            return;
        }

        #endregion

        #region Display
        public override string ToString() => this.IsParent() ? 
            $"{this.Value}=*" : 
            $"{this.Key.Value}={this.Value}";

        private static string MakeNiceName(string providedName, string providedID, OSMTag parent)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            if (!string.IsNullOrEmpty(providedName))
                return textInfo.ToTitleCase(providedName);
            else if (string.IsNullOrEmpty(providedID))
                return "*";

            // Try and make a nice name for layer bakes, etc
            var name = providedID.Replace("_", " ");
            if (name == "yes" && parent != null)
                name = parent.Name + " (Untagged)";

            return textInfo.ToTitleCase(name);
        }

        #endregion

        public bool IsParent() =>  this.Key == null;

        #region IEquitable implementation
        public override bool Equals(object obj) => Equals(obj as OSMTag);

        public bool Equals(OSMTag other)
        {
            return other != null &&
                   this.Value == other.Value &&
                   EqualityComparer<OSMTag>.Default.Equals(this.Key, other.Key);
        }

        // Need to provide a hash code as we are using this as a dictionary key in RequestHandler
        public override int GetHashCode()
        {
            if (this.IsParent())
                return this.Value.GetHashCode();
            else
                return this.Value.GetHashCode() ^ this.Key.GetHashCode();
        }
        #endregion
    }
}
