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
    using Caribou.Data;

    public class MelbourneBenchmarks
    {
        // These are benchmarks for a medium sized XML case (10mbs)

        private string melbourneFile = Properties.Resources.MelbourneOSM;
        private List<FeatureRequest> features = new List<FeatureRequest>()
        {
            new FeatureRequest("amenity", ""), new FeatureRequest("highway",  ""),
            new FeatureRequest("amenity", "restaurant"), new FeatureRequest("highway",  "residential")
        };

        public MelbourneBenchmarks()
        {
        }

        [Benchmark]
        public void TestParseViaXMLReader()
        {
            var result = Caribou.Processing.ParseViaXMLReader.FindByFeatures(features, melbourneFile);
        }
        
        [Benchmark]
        public void TestParseViaXMLDocument()
        {
            var result = Caribou.Processing.ParseViaXMLDocument.FindByFeatures(features, melbourneFile);
        }

        [Benchmark]
        public void TestParseViaLinq()
        {
            var result = Caribou.Processing.ParseViaLinq.FindByFeatures(features, melbourneFile);
        }
    }
}
