namespace Caribou
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using Caribou.Components;
    using GH_IO.Serialization;
    using Grasshopper.Kernel;
    using Rhino;

    public abstract class CaribouComponent : GH_Component
    {
        // This is a base class that can be used by all the plugin's components. This can allow for better code reuse for:
        // - Very commonly used functions (e.g. retrieving tolerances)
        // - Shared setup tasks (e.g. plugin category; or if wrapping SolveInstance in exception tracking (e.g. Sentry))
        protected LoggingHandler logger = new LoggingHandler();
        private static string pluginCategory = "Caribou";
        public static readonly double DocAngleTolerance = RhinoDoc.ActiveDoc.ModelAngleToleranceRadians;
        public static readonly double DocAbsTolerance = RhinoDoc.ActiveDoc.ModelAbsoluteTolerance;

        // Pass the constructor parameters up to the main GH_Component abstract class
        protected CaribouComponent(string name, string nickname, string description, string subCategory)
            : base(name, nickname, description, pluginCategory, subCategory)
        {
        }

        // Components must implement the method
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = "<Pending>")]
        protected abstract void CaribouRegisterOutputParams(GH_Component.GH_OutputParamManager pManager);

        // Override the output paramater registration. This allows for a debug logging output to be injected for DEBUG builds
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            CaribouRegisterOutputParams(pManager);
#if DEBUG
            pManager.AddTextParameter("Debug", "D", "Debug output logged from component - this parameter should be hidden in release builds", GH_ParamAccess.list); // Debugging affordance
            logger.indexOfDebugOutput = pManager.ParamCount - 1;
#endif
        }

        // Components must implement the method
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = "<Pending>")]
        protected abstract void CaribouSolveInstance(IGH_DataAccess da);

        // Override the main solve instance method. This allows it to be wrapped in a try/catch for error reporting purposes
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            RhinoDoc.ActiveDoc.Views.RedrawEnabled = false;
#if DEBUG
            logger.Reset();
#endif

            CaribouSolveInstance(DA);

#if DEBUG
            DA.SetDataList(logger.indexOfDebugOutput, logger.debugLogs);
#endif
            RhinoDoc.ActiveDoc.Views.RedrawEnabled = true;
        }

        // This is provided to all components so it can be passed along to error reporting
        public static string GetPluginVersion()
        {
            var v = Assembly.GetExecutingAssembly().GetName().Version;
            return v.Major.ToString() + '.' + v.Minor.ToString() + '.' + v.Build.ToString();
        }
    }
}
