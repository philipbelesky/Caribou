namespace Caribou.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ExampleTests
    {
        /// <summary>
        /// Transform a brep using a translation
        /// </summary>
        [TestMethod]
        public void TestParseA()
        {
            var result = Caribou.Processing.XMLParsing.ParserA();
            Assert.AreEqual(result, 3);
        }

        [TestMethod]
        public void TestParseB()
        {
            var result = Caribou.Processing.XMLParsing.ParserB();
            Assert.AreEqual(result, 3);
        }

        [TestMethod]
        public void TestParseC()
        {
            var result = Caribou.Processing.XMLParsing.ParserC();
            Assert.AreEqual(result, 3);
        }
    }
}
