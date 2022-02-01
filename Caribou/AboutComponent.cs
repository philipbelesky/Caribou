namespace Caribou
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.ServiceModel.Syndication;
    using System.Xml;
    using Caribou.Properties;
    using Grasshopper.Kernel;

    public class AboutComponent : CaribouComponent
    {
        public AboutComponent() : base(
            "About Caribou", "AB", "Displays information about this plugin, including " +
            "documentation sources and current/latest versions.", "About")
        { }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            // No input parameters needed
        }

        protected override void CaribouRegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Current Version", "cV", "The version of the installed plugin.", GH_ParamAccess.item);
            pManager.AddTextParameter("Latest Version", "lV", "The latest released version of the installed plugin.", GH_ParamAccess.item);
            pManager.AddTextParameter("Latest Changes", "cL", "The list of changes to date of the installed plugin.", GH_ParamAccess.item);
            pManager.AddTextParameter("About", "A", "Information about this plugin.", GH_ParamAccess.item);
            pManager.AddTextParameter("URL", "U", "A link to this plugin's documentation.", GH_ParamAccess.item);
        }

        protected override void CaribouSolveInstance(IGH_DataAccess da)
        {
            var assemblyInfo = new CaribouInfo();
            // Do the non-fetch based outputs first
            da.SetData(0, GetPluginVersion());
            da.SetData(3, assemblyInfo.Description);
            da.SetData(4, assemblyInfo.PluginURL.ToString());
            logger.NoteTiming("Setup"); // Debug Info
            da.SetData(1, GetLatestVersion(assemblyInfo.ReleasesFeed));
            da.SetData(2, GetLatestChanges(assemblyInfo.ChangeLogURL));
            logger.NoteTiming("URL fetching"); // Debug Info
        }

        private static string GetLatestVersion(Uri feedURL)
        {
            var version = "A latest release could not be found";
            var client = new HttpClient();
            System.IO.Stream result;

            try
            {
                result = client.GetStreamAsync(feedURL).Result; // Will throw during debug; should be fine in release
            }
            catch (HttpRequestException)
            {
                client.Dispose();
                return version;
            }

            using (var xmlReader = XmlReader.Create(result))
            {
                SyndicationFeed feed = SyndicationFeed.Load(xmlReader);
                if (feed != null)
                {
                    version = feed.Items.First().Title.Text;
                }
            }

            client.Dispose();
            return version;
        }

        private static string GetLatestChanges(Uri changelogURL)
        {
            var changes = "A CHANGELOG could not be found";
            var client = new HttpClient();
            changes = client.GetStringAsync(changelogURL).Result;
            client.Dispose();
            return changes;
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

        public override Guid ComponentGuid => new Guid("4ccad95b-ec0d-4559-a26f-07dc5dc18a32");

        protected override System.Drawing.Bitmap Icon => Resources.icons_about;
    }
}
