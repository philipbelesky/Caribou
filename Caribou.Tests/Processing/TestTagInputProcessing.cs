﻿namespace Caribou.Tests.Processing
{
    using System.Collections.Generic;
    using System.Linq;
    using Caribou.Forms;
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
            AddItems(TagsTestClases.itemATags, ref expectedSimpleDataList);
            AddItems(TagsTestClases.itemBTags, ref expectedSimpleDataList);
            expectedSimpleDataList = expectedSimpleDataList.Distinct().ToList(); // Remove overlapping items

            expectedCaseDataList = new List<OSMMetaData>();
            var caseData = TagsTestClases.GetTagsCaseData().SelectMany(i => i).SelectMany(i => i).Distinct();
            AddItems(caseData, ref expectedCaseDataList);
        }

        [TestMethod]
        public void TestSimpleInput()
        {
            var testDataTree = MakeTreeFromListofLists(new List<List<List<string>>>() 
            {
                new List<List<string>>() { TagsTestClases.itemATags, TagsTestClases.itemBTags }
            });
            var resultMetaData = new OSMListWithPaths(testDataTree);

            CollectionAssert.AreEquivalent(resultMetaData.items, expectedSimpleDataList);

            var catholicPaths = GetExpectedPathIndicesForTag(resultMetaData, expectedSimpleDataList[2]); // denom=catholic
            Assert.IsTrue(catholicPaths.Contains(itemAPath.ToString()));

            var futunaPaths = GetExpectedPathIndicesForTag(resultMetaData, expectedSimpleDataList[0]); // name=futuna
            Assert.IsTrue(futunaPaths.Contains(itemAPath.ToString()));

            var amenityPaths = GetExpectedPathIndicesForTag(resultMetaData, expectedSimpleDataList[3]); // amenity=*
            Assert.IsTrue(amenityPaths.Contains(itemAPath.ToString()));
            Assert.IsTrue(amenityPaths.Contains(itemBPath.ToString()));

            var buildingPaths = GetExpectedPathIndicesForTag(resultMetaData, expectedSimpleDataList[6]); // building=yes
            Assert.IsTrue(buildingPaths.Contains(itemAPath.ToString()));
            Assert.IsTrue(buildingPaths.Contains(itemBPath.ToString()));
        }

        [TestMethod]
        public void TestCaseDataInput()
        {
            var testDataTree = MakeTreeFromListofLists(TagsTestClases.GetTagsCaseData());
            var resultMetaData = new OSMListWithPaths(testDataTree);

            CollectionAssert.AreEquivalent(resultMetaData.items, expectedCaseDataList);
        }

        private void AddItems(IEnumerable<string> tags, ref List<OSMMetaData> itemList)
        {
            foreach (string tagName in tags)
            {
                var tagData = new OSMMetaData(tagName);
                itemList.Add(tagData);
                if (!itemList.Contains(tagData.ParentType))
                    itemList.Add(tagData.ParentType);
            }
        }

        private List<string> GetExpectedPathIndicesForTag(OSMListWithPaths resultMetaData, OSMMetaData tag)
        {
            return resultMetaData.pathsPerItem[tag].Select(o => o.ToString()).ToList(); 
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
