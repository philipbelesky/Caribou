using Eto.Drawing;
using Eto.Forms;
using System;
using Caribou.Forms;
using Caribou.Components;

namespace Eto.Prototyping
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var mockSelectionState = SelectionCollection.GetCollection(false);
            // Note that this only works on Windows; for macOS seems to crash unless using Eto 2.5.11+. 
            new Application(Eto.Platforms.Wpf).Run(new SpecifyFeaturesForm(mockSelectionState, true));
        }
    }
}
