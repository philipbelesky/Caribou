﻿namespace Caribou.Components
{
    using System;
    using Caribou.Properties;
    using Caribou.Workers;
    using Grasshopper.Kernel;

    /// <summary>Identifies and outputs all OSM way-type items that contain any of the requested metadata. Logic in worker.</summary>
    public class FindWaysComponent : BaseFindComponent
    {
        public FindWaysComponent()
            : base("Extract Ways", "Ways", "Load and parse way (e.g. polyline) data from an OSM file based on its metadata")
        {
            this.BaseWorker = new ParseWaysWorker(this);
        }

        protected override void CaribouRegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Ways", "W", "Ways; e.g. nodes linked in a linear order via a Polyline", GH_ParamAccess.tree);
            AddCommonOutputParams(pManager);
        }

        public override Guid ComponentGuid => new Guid("f677053e-0416-433b-9a8e-ce3124998b7d");

        protected override System.Drawing.Bitmap Icon => Resources.icons_ways;
    }
}
