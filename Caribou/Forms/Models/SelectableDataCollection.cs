namespace Caribou.Forms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Caribou.Models;
    using Grasshopper.Kernel.Data;

    // Translate a list of OSMMetaData items into OSMSelectableFeatures for showing in the form
    // E.g. like  OSMDefinedFeatures.GetDefinedFeaturesForForm() or for dynamic tags
    public struct SelectableDataCollection
    {
        public Dictionary<OSMSelectableData, List<OSMSelectableData>> tagHierarchy;

        // Create a selectable collection from a specific list of tags
        public SelectableDataCollection(List<OSMMetaData> requests)
        {
            // Basic setup
            tagHierarchy = new Dictionary<OSMSelectableData, List<OSMSelectableData>>();

            foreach (var tag in requests)
            {
                var selectableTag = new OSMSelectableData(tag.TagType, tag.Name, tag.Explanation, 0, 0, false);

                var parent = tag.ParentType;
                var selectableParent = new OSMSelectableData(parent.TagType, parent.Name, parent.Explanation, 0, 0, false);

                if (!tagHierarchy.ContainsKey(selectableParent))
                    tagHierarchy[selectableParent] = new List<OSMSelectableData>() { selectableTag };
                else
                    tagHierarchy[selectableParent].Add(selectableTag);
            }

            // Sort child items alphabetically (parents are sorted in MakeOSMCollection)
            foreach (var parentType in tagHierarchy)
                parentType.Value.Sort();
        }

        // If not providing a specific list of metadata, return defined features
        public SelectableDataCollection(bool usePredefined)
        {
            tagHierarchy = OSMDefinedFeatures.GetDefinedFeaturesForForm();
        }
    }
}
