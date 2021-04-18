namespace Caribou.Profiling
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BenchmarkDotNet.Attributes;
    using Caribou.Processing;

    public class ChicagoBenchmarks
    {
        // These are benchmarks for a large XML case (100mbs)

        private string chicagoFile = Properties.Resources.ChicagoOSM;
        private DataRequestResult[] features = new DataRequestResult[]
        {
            new DataRequestResult("amenity", ""), new DataRequestResult("highway",  ""),
            new DataRequestResult("amenity", "restaurant"), new DataRequestResult("highway",  "residential")
        };

        public ChicagoBenchmarks()
        {
        }

        [Benchmark]
        public void TestParseViaXMLReader()
        {
            var result = Caribou.Processing.ParseViaXMLReader.FindByFeatures(features, chicagoFile);
        }
        
        [Benchmark]
        public void TestParseViaXMLDocument()
        {
            var result = Caribou.Processing.ParseViaXMLDocument.FindByFeatures(features, chicagoFile);
        }

        [Benchmark]
        public void TestParseViaLinq()
        {
            var result = Caribou.Processing.ParseViaLinq.FindByFeatures(features, chicagoFile);
        }
    }
}
