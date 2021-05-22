using Eto.Drawing;
using Eto.Forms;
using System;
using Caribou.Components;

namespace Eto.Prototyping
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // new Application(Eto.Platform.Detect).Run(new MainForm());
            new Application(Eto.Platform.Detect).Run(new SpecifyFeaturesForm());
        }
    }
}
    