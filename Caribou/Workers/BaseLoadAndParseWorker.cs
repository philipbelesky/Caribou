namespace Caribou.Workers
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Threading;
    using Caribou.Components;
    using Caribou.Models;
    using Grasshopper;
    using Grasshopper.Kernel;
    using Grasshopper.Kernel.Data;
    using Grasshopper.Kernel.Types;
    using Rhino.Geometry;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using System.Linq;
    using Rhino;
    using Caribou.Processing;

    /// <summary>
    /// Shared logic for doing the 'work' of each parsing component.
    /// Conforms  to the asynchronous class features provided by WorkerInstance.
    /// </summary>
    public abstract class BaseLoadAndParseWorker : WorkerInstance
    {
        // Inputs
        protected List<string> providedFilePaths;
        protected List<string> requestedMetaDataRaw;
        // Parsed Inputs
        protected ParseRequest requestedMetaData;
        // Outputs
        protected RequestHandler result;
        protected GH_Structure<GH_String> itemTags;
        protected GH_Structure<GH_String> requestReport;
        protected List<Rectangle3d> boundaries;

        public BaseLoadAndParseWorker(GH_Component parent)
            : base(parent) // Pass parent component back to base class so state (e.g. remarks) can bubble up
        {
        }

        // Subclasses must specify their type
        protected abstract OSMGeometryType WorkerType();

        // Parse the XML to extract component specific results
        public void ExtractCoordsForComponentType(Action<string, double> reportProgress)
        {
            ParseViaXMLReader.FindItemsByTag(ref this.result, this.WorkerType());
        }

        public override void DoWork(Action<string, double> reportProgress, Action done)
        {
            logger.Reset();
            logger.indexOfDebugOutput = 4; // Dynamic nature of class params requires manually specifying debug log output index
            string typeName = Enum.GetName(typeof(OSMGeometryType), this.WorkerType());

            // Validate filepaths
            for (var i = 0; i < this.providedFilePaths.Count; i++)
            {
                var path = this.providedFilePaths[i];
                path = path.Replace("\n", "").Replace("\r", "").Trim(); // paths from Panels are messy
                bool isFilePresent = File.Exists(path);
                if (!isFilePresent)
                    this.RuntimeMessages.Add(new Message($"Could not find file located at '{path}'", Message.Level.Warning));

                this.providedFilePaths[i] = path;
            }

            this.providedFilePaths.RemoveAll(path => !File.Exists(path));
            if (this.providedFilePaths.Count == 0)
            {
                this.RuntimeMessages.Add(new Message("No valid file paths provided", Message.Level.Error));
                done();
                return;
            }

            // Validate feature keys
            if (this.requestedMetaDataRaw.Count == 0)
            {
                var text = "Features parameter connected but no feature keys were specified. Please provide them via:\n" +
                    "- Using Caribou's Specify Features component (click the button!)\n" +
                    "- Text in a 'key=value' or 'key=*' format separated by commas or newlines";
                this.RuntimeMessages.Add(new Message(text, Message.Level.Warning));
                done(); return;
            }
            this.requestedMetaData = new ParseRequest(this.requestedMetaDataRaw);

            reportProgress(Id, 0.02); // Report something in case there is a long node-collection hang when extracting ways 

            result = new RequestHandler(providedFilePaths, requestedMetaData, this.WorkerType(), reportProgress, Id);
            logger.NoteTiming("Setup request handler");
            if (this.CancellationToken.IsCancellationRequested) { done(); return; }

            if (result.LinesPerFile.Contains(-1))
            {
                var text = "One of the files provided could not be read.\n" +
                    "Check that the file is in the provided location.\n" +
                    "If providing a file path via a Panel, check it does not contain spaces, new lines, etc";
                this.RuntimeMessages.Add(new Message(text, Message.Level.Error));
                done(); return;
            }

            reportProgress(Id, 0.03); // Report something in case there is a long node-collection hang when extracting ways 
            this.ExtractCoordsForComponentType(reportProgress); // Parse XML for lat/lon data
            logger.NoteTiming($"Extract {typeName}s from data");
            if (this.CancellationToken.IsCancellationRequested) { done(); return; }

            this.MakeGeometryForComponentType(); // Translate lat/lon data to Rhino geo
            logger.NoteTiming("Convert to geometry");
            if (this.CancellationToken.IsCancellationRequested) { done(); return; }

            this.GetTreeForComponentType(); // Form tree structure for Rhino geo
            boundaries = GetOSMBoundaries.GetBoundariesFromResult(result);
            logger.NoteTiming("Output geometry");
            if (this.CancellationToken.IsCancellationRequested) { done(); return; }

            this.itemTags = result.GetTreeForItemTags(); // Form tree structure for key:value data per geo
            logger.NoteTiming("Output tags");
            if (this.CancellationToken.IsCancellationRequested) { done(); return; }

            this.requestReport = result.GetTreeForMetaDataReport(); // Form tree structure for found items
            logger.NoteTiming("Output metadata");

            done(); // Must be called to trigger outputs and report messages!
        }

        // Generate type-specific geometry (e.g. way or node)
        public abstract void MakeGeometryForComponentType();

        // Generate type-specific tree (e.g. way or node)
        public abstract void GetTreeForComponentType();

        public override void GetData(IGH_DataAccess da, GH_ComponentParamServer ghParams)
        {
            if (this.CancellationToken.IsCancellationRequested)
                return;

            // PARSE XML Data
            this.providedFilePaths = new List<string>();
            da.GetDataList(0, this.providedFilePaths);

            // PARSE Feature Keys
            this.requestedMetaDataRaw = new List<string>();
            da.GetDataList(1, this.requestedMetaDataRaw);

            GetExtraData(da);
        }

        protected virtual void GetExtraData(IGH_DataAccess da) { }

        public abstract void OutputTreeForComponentType(IGH_DataAccess da); // Output type-specific tree (e.g. way or node)

        protected override void WorkerSetData(IGH_DataAccess da)
        {
            if (this.CancellationToken.IsCancellationRequested)
                return;
            this.OutputTreeForComponentType(da); // Set component-specific outputs

            if (this.CancellationToken.IsCancellationRequested)
                return;
            if (this.itemTags != null)
                da.SetDataTree(1, this.itemTags);

            if (this.CancellationToken.IsCancellationRequested)
                return;
            if (this.requestReport != null)
                da.SetDataTree(2, this.requestReport);

            if (this.boundaries != null)
                da.SetDataList(3, this.boundaries);
        }
    }
}
