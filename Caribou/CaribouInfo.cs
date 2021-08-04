namespace Caribou
{
    using System;
    using System.Drawing;
    using Caribou.Properties;
    using Grasshopper.Kernel;

    public class CaribouInfo : GH_AssemblyInfo
    {
        public override string Name => "Caribou";

        public override Bitmap Icon => Resources.icons_plugin;

        // GHB note: if using the provided "About" component this text is output there
        public override string Description => "Caribou is a fast and resilient OSM data parser";

        public override Guid Id => new Guid("f7d1db23-92ab-4f95-bb7a-2370ae5e67f6");

        public override string AuthorName => "Philip Belesky";

        public override string AuthorContact => "contact@philipbelesky.com";

        // GHB note: if using the provided "About" component this URL is output there
        public virtual Uri PluginURL => new Uri("https://caribou.philipbelesky.com");

        // GHB note: if using the provided "About" component this File is output there
        public virtual Uri ChangeLogURL => new Uri("https://raw.githubusercontent.com/philipbelesky/Caribou/main/CHANGELOG.md");

        // GHB note: if using the provided "About" component this URL is parsed there
        public virtual Uri ReleasesFeed => new Uri("https://github.com/philipbelesky/caribou/releases.atom");
    }
}
