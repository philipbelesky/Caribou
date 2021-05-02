namespace Caribou.Processing
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Caribou.Data;
    using Grasshopper.Kernel;
    using Rhino.Geometry;

    public class LoadAndParseWorker : WorkerInstance
    {
        private string xmlFileContents;
        private List<string> requestedFeaturesRaw;
        private List<FeatureRequest> requestedFeatures;
        private List<string> debugOutput = new List<string>();
        private RequestResults foundItems;
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

            // Extra LatLon coords from XML tag that match the specified feature/subfeature
            this.foundItems = ParseViaXMLReader.FindByFeatures(this.requestedFeatures, this.xmlFileContents);

            if (this.CancellationToken.IsCancellationRequested)
            {
                return;
            }

            // Translate OSM nodes to Rhino points 
            this.foundNodes = TranslateToXYManually.NodePointsFromCoords(this.foundItems);

            if (this.CancellationToken.IsCancellationRequested)
            {
                return;
            }

            // Translate OSM ways to Rhino polylines
            this.foundWays = TranslateToXYManually.WayPolylinesFromCoords(this.foundItems);

            done();
        }

        public override WorkerInstance Duplicate() => new LoadAndParseWorker();

        public override void GetData(IGH_DataAccess da, GH_ComponentParamServer ghParams)
        {
            if (this.CancellationToken.IsCancellationRequested)
            {
                return;
            }

            var rawDataContensts = new List<string>();
            da.GetDataList(0, rawDataContensts);
            // To account for files being provided in per-line mode we just concat them
            if (rawDataContensts.Count > 1)
            {
                xmlFileContents = string.Join("", rawDataContensts);
                // this.Parent.AddRuntimeMessage(GH_RuntimeMessageLevel.Remark,
                //    "OSM file content was provided as a list of strings. When using the Read File component you should turn OFF Per-File parsing. If you are trying to read multiple OSM files at once, this is not supported.");
            }
            else
            {
                xmlFileContents = rawDataContensts[0];
            }

            var requestedFeaturesRaw = new List<string>();
            da.GetDataList(1, requestedFeaturesRaw);
            if (requestedFeaturesRaw.Count == 0)
            {
                // TODO: Raise error/remark if empty
            } 
            else
            {
                this.requestedFeatures = FeatureRequest.ParseFeatureRequestFromGrasshopper(requestedFeaturesRaw);
            }            
            // TODO: raise a remark or warning if they key:value is not predefined?
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
