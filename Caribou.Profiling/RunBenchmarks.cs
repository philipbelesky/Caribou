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
        private const int N = 10000;
        private readonly byte[] data;

        private readonly SHA256 sha256 = SHA256.Create();
        private readonly MD5 md5 = MD5.Create();

        public BasicBenchmarks()
        {
        }

        [Benchmark]
        public void TestParseA()
        {
            var result = Caribou.Processing.XMLParsing.ParserA();
        }
        
        [Benchmark]
        public void TestParseB()
        {
            var result = Caribou.Processing.XMLParsing.ParserB();
        }

        [Benchmark]
        public void TestParseC()
        {
            var result = Caribou.Processing.XMLParsing.ParserC();
        }
    }
}
