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
            var summary = BenchmarkRunner.Run<SimpleBenchmarks>();
            Console.Write("Finished Benchmarks. Press any key to exit.");
            Console.ReadLine(); // wait for input
        }
    }
}
