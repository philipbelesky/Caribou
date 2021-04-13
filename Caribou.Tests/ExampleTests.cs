// File from https://github.com/tmakin/RhinoCommonUnitTesting
namespace Caribou.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rhino.Geometry;
    using Caribou.Processing;

    [TestClass]
    public class ExampleTests
    {
        /// <summary>
        /// Transform a brep using a translation
        /// </summary>
        [TestMethod]
        public void TestParseA()
        {
            var result = XMLParsing.ParserA();
            Assert.AreEqual(result, "OKA");
        }

        [TestMethod]
        public void TestParseB()
        {
            var result = XMLParsing.ParserB();
            Assert.AreEqual(result, "OKB");
        }
    }
}
