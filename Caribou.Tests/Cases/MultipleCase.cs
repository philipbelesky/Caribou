﻿namespace Caribou.Tests.Cases
{
    using System.Collections.Generic;
    using Caribou.Models;
    using Caribou.Tests.Parsing;

    public class MultipleCase : BaseParsingTest
    {
        protected static readonly List<string> OSMXMLs = new List<string>() {
            Properties.Resources.MultipleA,
            Properties.Resources.MultipleB,
            Properties.Resources.MultipleC,
        };

        protected static readonly ParseRequest mainFeatures = new ParseRequest(
            new List<OSMTag>() {
                new OSMTag("craft"), // x nodes x ways
                new OSMTag("Amenity"), // x nodes x ways
                new OSMTag("building")  // x nodes x ways
            }
        );

        protected static readonly ParseRequest miscSubFeatures = new ParseRequest(
            new List<string>() {
                "amenity=restaurant", // x nodes x ways
                "Craft=JEWELLER", // x nodes x ways
                "building=retail", // x nodes x ways
            });
    }
}
