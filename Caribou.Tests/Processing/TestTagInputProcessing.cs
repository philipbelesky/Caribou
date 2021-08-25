namespace Caribou.Tests.Processing
{
    using System.Collections.Generic;
    using System.Linq;
    using Caribou.Models;
    using Grasshopper.Kernel.Data;
    using Grasshopper.Kernel.Types;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestTagInputProcessing
    {
        private List<OSMMetaData> expectedDataList;
        private GH_Path itemAPath = new GH_Path(0);
        private GH_Path itemBPath = new GH_Path(1);

        private List<string> itemATags = new List<string>()
        {
            "name=Futuna Chapel",
            "amenity=place_of_worship",
            "denomination=catholic",
            "building=yes",
        };
        private List<string> itemBTags = new List<string>()
        {
            "amenity=arts_centre",
            "building=yes",
        };

        public TestTagInputProcessing()
        {
            expectedDataList = new List<OSMMetaData>();
            foreach(var tag in itemATags)
            {
                expectedDataList.Add(new OSMMetaData(tag));
            }
            foreach (var tag in itemBTags)
            {
                expectedDataList.Add(new OSMMetaData(tag));
            }
            expectedDataList = expectedDataList.Distinct().ToList(); // Remove overlapping items
        }

        [TestMethod]
        public void TestSimpleInput()
        {
            var testDataTree = MakeTreeFromListofLists(new List<List<string>>() { itemATags, itemBTags });
            var resultMetaData = new FilterRequest(testDataTree);

            CollectionAssert.AreEquivalent(resultMetaData.Requests, expectedDataList);

            var catholicPaths = GetExpectedPathIndicesForTag(resultMetaData, expectedDataList[2]);
            Assert.IsTrue(catholicPaths.Contains(itemAPath.ToString()));

            var futunaPaths = GetExpectedPathIndicesForTag(resultMetaData, expectedDataList[0]);
            Assert.IsTrue(futunaPaths.Contains(itemAPath.ToString()));

            var buildingPaths = GetExpectedPathIndicesForTag(resultMetaData, expectedDataList[3]);
            Assert.IsTrue(buildingPaths.Contains(itemAPath.ToString()));
            Assert.IsTrue(buildingPaths.Contains(itemBPath.ToString()));
        }

        private List<string> GetExpectedPathIndicesForTag(FilterRequest resultMetaData, OSMMetaData tag)
        {
            return resultMetaData.PathsOfRequest[tag].Select(o => o.ToString()).ToList(); 
        }

        private GH_Structure<GH_String>  MakeTreeFromListofLists(List<List<string>> tagsByGeometryPath)
        {
            var tagsAsTree = new GH_Structure<GH_String>();
            for (var i = 0; i < tagsByGeometryPath.Count; i++)
            {
                var path = new GH_Path(i);
                for (var j = 0; j < tagsByGeometryPath[i].Count; j++)
                {
                    var tagReadout = new GH_String(tagsByGeometryPath[i][j]);
                    tagsAsTree.Append(tagReadout, path);
                }
            }
            return tagsAsTree;
        }
    }
}
