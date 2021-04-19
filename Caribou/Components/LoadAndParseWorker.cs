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
        private DataRequestResult[] featuresSpecified;
        private ResultsForFeatures foundItems;
        private List<Point3d> foundNodes;
        private List<Polyline> foundWays;

        public LoadAndParseWorker()
            : base(null)
        {
        }

        public override void DoWork(Action<string, double> reportProgress, Action done)
        {
            // Checking for cancellation
            if (this.CancellationToken.IsCancellationRequested)
            {
                return;
            }

            this.foundItems = ParseViaXMLReader.FindByFeatures(this.featuresSpecified, this.xmlFileContents);
            this.foundNodes = DataRhinoOutputs.GetNodesFromCoords(this.foundItems);
            this.foundWays = DataRhinoOutputs.GetWaysFromCoords(this.foundItems);

            done();
        }

        public override WorkerInstance Duplicate() => new LoadAndParseWorker();

        public override void GetData(IGH_DataAccess da, GH_ComponentParamServer ghParams)
        {
            if (this.CancellationToken.IsCancellationRequested)
            {
                return;
            }

            da.GetData(0, ref this.xmlFileContents);

            // TODO: validation of input

            // TODO: set below array based on input
            var featuresSpecified = new DataRequestResult[]
            {
                new DataRequestResult("amenity", "restaurant"),
                new DataRequestResult("craft", "jeweller"),
            };
        }

        public override void SetData(IGH_DataAccess da)
        {
            if (this.CancellationToken.IsCancellationRequested)
            {
                return;
            }

            da.SetDataList(0, this.foundNodes);
            da.SetDataList(1, this.foundWays);

            // Can't use the GHBComponent approach to logging; so construct output for Debug param manually
            da.SetDataList(2, this.debugOutput);
        }
    }
}
