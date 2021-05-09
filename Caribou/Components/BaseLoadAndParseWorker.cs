namespace Caribou.Processing
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Caribou.Components;
    using Caribou.Data;
    using Grasshopper;
    using Grasshopper.Kernel;
    using Grasshopper.Kernel.Data;
    using Rhino.Geometry;

    /// <summary>The 'work' of a component that conforms to the asynchronous class features provided by WorkerInstance.
    /// Inherited by the workers that parse for specific geometries and provides their shared methods and parameters.</summary>
    public abstract class BaseLoadAndParseWorker : WorkerInstance
    {
        // Inputs
        protected List<string> providedXMLsRaw;
        protected List<string> requestedMetaDataRaw;
        // Parsed Inputs
        protected OSMXMLs providedXMLs;
        protected ParseRequest requestedMetaData;
        // Outputs
        protected RequestHandler foundOSMItems;
        protected List<string> debugOutput = new List<string>();

        public BaseLoadAndParseWorker(GH_Component parent)
            : base(parent) // Pass parent component back to base class so state (e.g. remarks) can bubble up
        {
        }

        public override void DoWork(Action<string, double> reportProgress, Action done)
        {
            // Checking for cancellation
            if (this.CancellationToken.IsCancellationRequested)
                return;

            // Extra LatLon coords from XML tag that match the specified feature/subfeature
            this.foundItems = ParseViaXMLReader.FindByFeatures(this.requestedMetaData, this.providedXMLsRaw);

            if (this.CancellationToken.IsCancellationRequested)
                return;

            // Translate OSM nodes to Rhino points
            this.foundNodes = TranslateToXYManually.NodePointsFromCoords(this.foundItems);

            if (this.CancellationToken.IsCancellationRequested)
                return;

            // Translate OSM ways to Rhino polylines
            this.foundWays = TranslateToXYManually.WayPolylinesFromCoords(this.foundItems);

            // Create output tree path for featues
            this.foundElementsReport = this.foundItems.ReportFoundFeatures(false);

            done();
        }

        public override void GetData(IGH_DataAccess da, GH_ComponentParamServer ghParams)
        {
            var parseMessages = new MessagesWrapper();

            if (this.CancellationToken.IsCancellationRequested)
                return;

            // PARSE XML Data
            this.providedXMLsRaw = new List<string>();
            da.GetDataList(0, this.providedXMLsRaw);
            this.providedXMLs = new OSMXMLs(this.providedXMLsRaw, ref parseMessages);

            // PARSE Feature Keys
            this.requestedMetaDataRaw = new List<string>();
            da.GetDataList(1, this.requestedMetaDataRaw);
            this.requestedMetaData = new ParseRequest(this.requestedMetaDataRaw, ref parseMessages);

            this.RuntimeMessages.Messages.AddRange(parseMessages.Messages);
        }
    }
}
