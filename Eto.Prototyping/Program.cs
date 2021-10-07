﻿namespace Eto.Prototyping
{
    using Eto.Forms;
    using System;
    using Caribou.Forms;
    using Caribou.Models;
    using Caribou.Forms.Models;

    class Program
    {
        const bool hideObscure = true;

        [STAThread]
        static void Main(string[] args)
        {
            // Select Form
            ShowSpecifyForm();
            //ShowFilterForm();
        }

        static void ShowSpecifyForm()
        {
            var mockOSMs = OSMPrimaryTypes.GetTreeCollection();
            // Note that this only works on Windows; for macOS seems to crash unless using Eto 2.5.11+. 
            new Application(Eto.Platforms.Wpf).Run(new SpecifyFeaturesForm(mockOSMs, hideObscure));
        }

        static void ShowFilterForm ()
        {
            var mockTags = new TreeGridItemCollection();

            var itemA = new CaribouTreeGridItem(new OSMTag("website"), 0, 0, false);
            var AChildA = new CaribouTreeGridItem(new OSMTag("website=http://www.google.com"), 1, 2, false);
            itemA.Children.Add(AChildA);
            var AChildB = new CaribouTreeGridItem(new OSMTag("website=http://www.test.com"), 1, 2, false);
            itemA.Children.Add(AChildB);
            mockTags.Add(itemA);

            var itemB = new CaribouTreeGridItem(new OSMTag("height"), 0, 0, false);
            var BChildA = new CaribouTreeGridItem(new OSMTag("height=10m"), 2, 2, false);
            itemB.Children.Add(BChildA);
            var BChildB = new CaribouTreeGridItem(new OSMTag("height=5"), 1, 1, false);
            itemB.Children.Add(BChildB);
            var CChildC = new CaribouTreeGridItem(new OSMTag("height=11"), 3, 3, false);
            itemB.Children.Add(CChildC);
            mockTags.Add(itemB);

            new Application(Eto.Platforms.Wpf).Run(new FilterFeaturesForm(mockTags, hideObscure));
        }
    }
}
