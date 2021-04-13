using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Caribou.Processing;

namespace Caribou.Profiling
{

    public class BasicBenchmarks
    {
        private string simpleProfile = Properties.Resources.simpleXMLProfile;
        public BasicBenchmarks()
        {
        }

        [Benchmark]
        public void TestParseA()
        {
            var result = Caribou.Processing.XMLParsing.ParserA(simpleProfile);
        }
        
        [Benchmark]
        public void TestParseB()
        {
            var result = Caribou.Processing.XMLParsing.ParserB(simpleProfile);
        }

        [Benchmark]
        public void TestParseC()
        {
            var result = Caribou.Processing.XMLParsing.ParserC(simpleProfile);
        }
    }
}
