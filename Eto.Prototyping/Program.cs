namespace Eto.Prototyping
{
    using Eto.Forms;
    using System;
    using Caribou.Forms;
    using Caribou.Models;
    using Caribou.Forms.Models;

    class Program
    {
        const bool HideObscure = true;

        [STAThread]
        static void Main()
        {
            // Select Form
            ShowSpecifyForm();
            ShowFilterForm();
        }

        static void ShowSpecifyForm()
        {
            var mockOSMs = OSMPrimaryTypes.GetTreeCollection();
            // Note that this only works on Windows; for macOS seems to crash unless using Eto 2.5.11+. 
            new Application(Eto.Platforms.Wpf).Run(new SpecifyFeaturesForm(mockOSMs, HideObscure));
        }

        static void ShowFilterForm ()
        {
            var mockTags = new TreeGridItemCollection();

            var parentA = new OSMTag("website");
            var itemA = new OSMTreeGridItem(parentA, 0, 0, false, false);
            var AChildA = new OSMTreeGridItem(new OSMTag("website=http://www.google.com"), 1, 2, true, false);
            itemA.Children.Add(AChildA);
            var AChildB = new OSMTreeGridItem(new OSMTag("website=http://www.test.com"), 1, 2, true, false);
            itemA.Children.Add(AChildB);
            mockTags.Add(itemA);

            var parentB = new OSMTag("height");
            var itemB = new OSMTreeGridItem(parentB, 0, 0, false, false);
            var BChildA = new OSMTreeGridItem(new OSMTag("height=10m"), 2, 2, true, false);
            itemB.Children.Add(BChildA);
            var BChildB = new OSMTreeGridItem(new OSMTag("height=5"), 1, 1, true, false);
            itemB.Children.Add(BChildB);
            var CChildC = new OSMTreeGridItem(new OSMTag("height=11"), 3, 3, true, false);
            itemB.Children.Add(CChildC);
            mockTags.Add(itemB);

            new Application(Eto.Platforms.Wpf).Run(new FilterTagsForm(mockTags, HideObscure));
        }
    }
}
