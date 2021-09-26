namespace Eto.Prototyping
{
    using Eto.Forms;
    using System;
    using Caribou.Forms;
    using Caribou.Models;

    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var mockOSMs = OSMDefinedFeatures.GetTreeCollection();
            // Note that this only works on Windows; for macOS seems to crash unless using Eto 2.5.11+. 
            new Application(Eto.Platforms.Wpf).Run(new SpecifyFeaturesForm(mockOSMs, true));
        }
    }
}
