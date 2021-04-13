namespace Caribou.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ExampleTests
    {
        private string simpleFile = Properties.Resources.SimpleXMLParse;

        [TestMethod]
        public void TestParseA()
        {
            var result = Caribou.Processing.XMLParsing.ParserA(simpleFile);
            Assert.AreEqual(result, 3);
        }

        [TestMethod]
        public void TestParseB()
        {
            var result = Caribou.Processing.XMLParsing.ParserB(simpleFile);
            Assert.AreEqual(result, 3);
        }

        [TestMethod]
        public void TestParseC()
        {
            var result = Caribou.Processing.XMLParsing.ParserC(simpleFile);
            Assert.AreEqual(result, 3);
        }
    }
}
