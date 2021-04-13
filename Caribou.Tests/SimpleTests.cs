namespace Caribou.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SimpleTests
    {
        private string simpleFile = Properties.Resources.SimpleXMLParse;

        [TestMethod]
        public void SimpleParserA()
        {
            var result = Caribou.Processing.XMLParsing.ParserA(simpleFile);
            Assert.AreEqual(result, 3);
        }

        [TestMethod]
        public void SimpleParserB()
        {
            var result = Caribou.Processing.XMLParsing.ParserB(simpleFile);
            Assert.AreEqual(result, 3);
        }

        [TestMethod]
        public void SimpleParserC()
        {
            var result = Caribou.Processing.XMLParsing.ParserC(simpleFile);
            Assert.AreEqual(result, 3);
        }
    }
}
