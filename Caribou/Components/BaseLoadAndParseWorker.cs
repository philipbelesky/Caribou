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
    using Grasshopper.Kernel.Types;
    using Rhino.Geometry;

    /// <summary>
    /// Shared logic for doing the 'work' of each parsing component.
    /// Conforms  to the asynchronous class features provided by WorkerInstance.
    /// </summary>
    public abstract class BaseLoadAndParseWorker : WorkerInstance
    {
        // Inputs
        protected List<string> providedXMLsRaw;
        protected List<string> requestedMetaDataRaw;
        // Parsed Inputs
        protected OSMXMLFiles providedXMLs;
        protected ParseRequest requestedMetaData;
        // Outputs
        protected RequestHandler result;
        protected GH_Structure<GH_String> itemTags;
        protected GH_Structure<GH_String> itemMetaDatas;

        public BaseLoadAndParseWorker(GH_Component parent)
            : base(parent) // Pass parent component back to base class so state (e.g. remarks) can bubble up
        {
        }

        public override void DoWork(Action<string, double> reportProgress, Action done)
        {
            logger.Reset();
            logger.indexOfDebugOutput = 3;

            result = new RequestHandler(providedXMLs, requestedMetaData);
            logger.NoteTiming("Setup request handler");
            if (this.CancellationToken.IsCancellationRequested)
                return;

            this.ExtractCoordsForComponentType(); // Parse XML for lat/lon data
            logger.NoteTiming("Extract coords from data");
            if (this.CancellationToken.IsCancellationRequested)
                return;

            this.MakeGeometryForComponentType(); // Translate lat/lon data to Rhino geo
            logger.NoteTiming("Translate coords to geometry");
            if (this.CancellationToken.IsCancellationRequested)
                return;

            this.GetTreeForComponentType(); // Form tree structure for Rhino geo
            logger.NoteTiming("Output geometry");
            if (this.CancellationToken.IsCancellationRequested)
                return;

            this.itemTags = result.GetTreeForItemTags(); // Form tree structure for key:value data per geo
            logger.NoteTiming("Output tags");
            if (this.CancellationToken.IsCancellationRequested)
                return;

            this.itemMetaDatas = result.GetTreeForMetaDataReport(); // Form tree structure for found items
            logger.NoteTiming("Output metadata");
            if (this.CancellationToken.IsCancellationRequested)
               return;

            done();
        }

        // Parse the XML to extract component specific results
        public abstract void ExtractCoordsForComponentType();

        // Generate type-specific geometry (e.g. way or node)
        public abstract void MakeGeometryForComponentType();

        // Generate type-specific tree (e.g. way or node)
        public abstract void GetTreeForComponentType();

        public override void GetData(IGH_DataAccess da, GH_ComponentParamServer ghParams)
        {
            var parseMessages = new MessagesWrapper();

            if (this.CancellationToken.IsCancellationRequested)
                return;

            // PARSE XML Data
            this.providedXMLsRaw = new List<string>();
            da.GetDataList(0, this.providedXMLsRaw);
            this.providedXMLs = new OSMXMLFiles(this.providedXMLsRaw, ref parseMessages);

            // PARSE Feature Keys
            this.requestedMetaDataRaw = new List<string>();
            da.GetDataList(1, this.requestedMetaDataRaw);
            this.requestedMetaData = new ParseRequest(this.requestedMetaDataRaw, ref parseMessages);

            this.RuntimeMessages.Messages.AddRange(parseMessages.Messages);
        }

        public abstract void OutputTreeForComponentType(IGH_DataAccess da); // Output type-specific tree (e.g. way or node)

        protected override void WorkerSetData(IGH_DataAccess da)
        {
            if (this.CancellationToken.IsCancellationRequested)
                return;

            this.OutputTreeForComponentType(da); // Set component-specific outputs

            if (this.CancellationToken.IsCancellationRequested)
                return;

            da.SetDataTree(1, this.itemTags);

            if (this.CancellationToken.IsCancellationRequested)
                return;

            da.SetDataTree(2, this.itemMetaDatas);
        }
    }
}
