namespace Caribou.Tests.Processing
{
    using System.Collections.Generic;
    using System.Linq;
    using Caribou.Models;
    using Caribou.Tests.Cases;
    using Grasshopper.Kernel.Data;
    using Grasshopper.Kernel.Types;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestTagInputProcessing
    {
        private List<OSMMetaData> expectedSimpleDataList;
        private List<OSMMetaData> expectedCaseDataList;
        private GH_Path itemAPath = new GH_Path(0, 0);
        private GH_Path itemBPath = new GH_Path(0, 1);

        public TestTagInputProcessing()
        {
            expectedSimpleDataList = new List<OSMMetaData>();
            foreach(string tag in TagsTestClases.itemATags)
            {
                expectedSimpleDataList.Add(new OSMMetaData(tag));
            }
            foreach (string tag in TagsTestClases.itemBTags)
            {
                expectedSimpleDataList.Add(new OSMMetaData(tag));
            }
            expectedSimpleDataList = expectedSimpleDataList.Distinct().ToList(); // Remove overlapping items

            expectedCaseDataList = new List<OSMMetaData>();
            var caseData = TagsTestClases.GetTagsCaseData().SelectMany(i => i).SelectMany(i => i).Distinct();
            foreach (string tag in caseData)
            {
                expectedCaseDataList.Add(new OSMMetaData(tag));
            }
        }

        [TestMethod]
        public void TestSimpleInput()
        {
            var testDataTree = MakeTreeFromListofLists(new List<List<List<string>>>() 
            {
                new List<List<string>>() { TagsTestClases.itemATags, TagsTestClases.itemBTags }
            });
            var resultMetaData = new FilterRequest(testDataTree);

            CollectionAssert.AreEquivalent(resultMetaData.Requests, expectedSimpleDataList);

            var catholicPaths = GetExpectedPathIndicesForTag(resultMetaData, expectedSimpleDataList[2]);
            Assert.IsTrue(catholicPaths.Contains(itemAPath.ToString()));

            var futunaPaths = GetExpectedPathIndicesForTag(resultMetaData, expectedSimpleDataList[0]);
            Assert.IsTrue(futunaPaths.Contains(itemAPath.ToString()));

            var buildingPaths = GetExpectedPathIndicesForTag(resultMetaData, expectedSimpleDataList[3]);
            Assert.IsTrue(buildingPaths.Contains(itemAPath.ToString()));
            Assert.IsTrue(buildingPaths.Contains(itemBPath.ToString()));
        }

        [TestMethod]
        public void TestCaseDataInput()
        {
            var testDataTree = MakeTreeFromListofLists(TagsTestClases.GetTagsCaseData());
            var resultMetaData = new FilterRequest(testDataTree);

            CollectionAssert.AreEquivalent(resultMetaData.Requests, expectedCaseDataList);
        }

        private List<string> GetExpectedPathIndicesForTag(FilterRequest resultMetaData, OSMMetaData tag)
        {
            return resultMetaData.PathsOfRequest[tag].Select(o => o.ToString()).ToList(); 
        }

        private GH_Structure<GH_String>  MakeTreeFromListofLists(List<List<List<string>>> tagsByGeometryPath)
        {
            var tagsAsTree = new GH_Structure<GH_String>();
            for (var i = 0; i < tagsByGeometryPath.Count; i++)
            {
                for (var j = 0; j < tagsByGeometryPath[i].Count; j++)
                {
                    var path = new GH_Path(i, j);
                    for (var k = 0; k < tagsByGeometryPath[i][j].Count; k++)
                    {
                        var tagReadout = new GH_String(tagsByGeometryPath[i][j][k]);
                        tagsAsTree.Append(tagReadout, path);
                    }
                }
            }
            return tagsAsTree;
        }
    }
}
