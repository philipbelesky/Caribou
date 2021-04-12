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
            data = new byte[N];
            new Random(42).NextBytes(data);
        }

        [Benchmark]
        public void ProfilingA()
        {
            sha256.ComputeHash(data);
            XMLParsing.ParserA();
        }

        [Benchmark]
        public void ProfilingB()
        {
            sha256.ComputeHash(data);
            XMLParsing.ParserB();
        }
    }
}
