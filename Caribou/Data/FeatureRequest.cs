namespace Caribou.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Grasshopper.Kernel;

    public readonly struct FeatureRequest
    {
        // A specific key:value pair represent feature/subfeatures to search for in an OSM file
        // Setup as a struct rather than dict for easier use of a specific hardcoded key to mean "all subfeatures"
        public const string SearchAllKey = "__ALL__"; // magic value; represents finding all subfeatures

        public FeatureRequest(string primaryFeature, string subFeature)
        {
            this.PimraryFeature = primaryFeature;
            this.SubFeature = subFeature; // "" means to not search by subfeature
            if (string.IsNullOrEmpty(this.SubFeature))
            {
                this.SubFeature = SearchAllKey;
            }
        }
        public static Tuple<List<FeatureRequest>, List<(GH_RuntimeMessageLevel, string)>> ParseFeatureRequestFromGrasshopper(List<string> ghInput)
        {
            var requestedFeatures = new List<FeatureRequest>();
            var requestMessages = new List<(GH_RuntimeMessageLevel, string)>();
            var cleanedGhInput = new List<string>();

            // If a multiline string has been provided via a Panel component without multiline input enabled
            foreach(string inputString in ghInput)
            {
                string[] lines = inputString.Split(
                    new[] { "\r\n", "\r", "\n", "," }, // Split on new lines and on commas
                    StringSplitOptions.None
                );
                for (var i = 0; i < lines.Length; i++)
                {
                    cleanedGhInput.Add(lines[i].Trim().ToLower());
                }
            }

            foreach (string inputString in cleanedGhInput)
            {
                string feature;
                string subFeature;
                if (inputString == "")
                {
                    continue;
                }

                if (inputString.Contains(':'))
                {
                    feature = inputString.Trim().Split(':')[0];
                    subFeature = inputString.Trim().Split(':')[1];
                } 
                else
                {
                    feature = inputString.Trim().Split(':')[0];
                    subFeature = FeatureRequest.SearchAllKey;
                }

                var requestedFeature = new FeatureRequest(feature, subFeature);
                // Prevent duplicates being added
                if (!requestedFeatures.Contains(requestedFeature))
                {
                    requestedFeatures.Add(requestedFeature);
                } else
                {
                    requestMessages.Add((GH_RuntimeMessageLevel.Remark, $"Found a duplicate request: {requestedFeature}"));
                }
            }
            return new Tuple<List<FeatureRequest>, List<(GH_RuntimeMessageLevel, string)>>(requestedFeatures, requestMessages);
        }

        public string PimraryFeature { get; }

        public string SubFeature { get; }

        public override string ToString() => $"({this.PimraryFeature}, {this.SubFeature})";
    }
}
