using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Attributes;

namespace Caribou.Profiling
{

    class Program
    {
        static void Main(string[] args)
        {
            var summaryA = BenchmarkRunner.Run<MelbourneBenchmarks>();
            var summaryB = BenchmarkRunner.Run<ChicagoBenchmarks>();
            Console.Write("Finished Benchmarks. Press any key to exit.");
            Console.ReadLine(); // wait for input
        }
    }
}
