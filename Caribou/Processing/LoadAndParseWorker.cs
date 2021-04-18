namespace Caribou.Processing
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Grasshopper.Kernel;
    using Rhino.Geometry;

    public class LoadAndParseWorker : WorkerInstance
    {
        private string xmlFileContents;
        private List<string> debugOutput = new List<string>();
        private DataRequestedFeature[] featuresSpecified;
        private ResultsForFeatures foundItems;
        private List<Point3d> foundNodes;
        private List<Polyline> foundWays;
        public LoadAndParseWorker() : base(null) { }

        public override void DoWork(Action<string, double> reportProgress, Action done)
        {
            // Checking for cancellation
            if (CancellationToken.IsCancellationRequested) { return; }

            foundItems = ParseViaXMLReader.FindByFeatures(featuresSpecified, xmlFileContents);
            foundNodes = DataRhinoOutputs.GetNodesFromCoords(foundItems);
            foundWays = DataRhinoOutputs.GetWaysFromCoords(foundItems);

            done();
        }

        public override WorkerInstance Duplicate() => new LoadAndParseWorker();

        public override void GetData(IGH_DataAccess da, GH_ComponentParamServer ghParams)
        {
            if (CancellationToken.IsCancellationRequested) return;

            da.GetData(0, ref xmlFileContents);
            // TODO: validation of input

            // TODO: set below array based on input
            var featuresSpecified = new DataRequestedFeature[]
            {
                new DataRequestedFeature("amenity", "restaurant"), new DataRequestedFeature("craft", "jeweller")
            };
        }

        public override void SetData(IGH_DataAccess da)
        {
            if (CancellationToken.IsCancellationRequested) return;

            da.SetDataList(0, foundNodes);
            da.SetDataList(1, foundWays);

            // Can't use the GHBComponent approach to logging; so construct output for Debug param manually            
            da.SetDataList(2, debugOutput);
        }
    }
}
