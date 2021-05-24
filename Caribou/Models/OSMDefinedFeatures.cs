namespace Caribou.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.Json;
    using Caribou.Models;
    using Caribou.Properties;

#pragma warning disable SA1310 // Field names should not contain underscore

    /// <summary>
    /// A predefined list of major/common feature & subfeature pairings specified on the
    /// [OSM wiki](https://wiki.openstreetmap.org/wiki/Map_features).
    /// </summary>
    public static class OSMDefinedFeatures
    {
        // Take the hardcoded feature types below, combine with the subfeatures in JSON, & report them back as a tree
        public static Dictionary<OSMSelectableFeature, List<OSMSelectableFeature>> GetDefinedFeaturesForForm()
        {
            var allDataInHierarchy = new Dictionary<OSMSelectableFeature, List<OSMSelectableFeature>>();

            foreach (var primaryFeature in Primary)
            {
                var pf = primaryFeature.Value;
                var spf = new OSMSelectableFeature(pf.ThisType, pf.Name, pf.Explanation, 0, 0, false);
                allDataInHierarchy[spf] = new List<OSMSelectableFeature>() { };
            }

            var docString = Encoding.UTF8.GetString(Resources.SubFeatureData);
            using (JsonDocument document = JsonDocument.Parse(docString))
            {
                JsonElement root = document.RootElement;
                foreach (JsonElement element in root.EnumerateArray())
                {
                    if (element.TryGetProperty("feature", out JsonElement feature))
                    {
                        var nameID = element.GetProperty("subfeature").ToString();
                        var parent = allDataInHierarchy.Keys.First(k => k.ThisType == feature.ToString());

                        OSMSelectableFeature subfeature = new OSMSelectableFeature(nameID, null,
                            element.GetProperty("description").ToString(),
                            int.Parse(element.GetProperty("nodes").ToString()),
                            int.Parse(element.GetProperty("ways").ToString()),
                            true, parent
                        );
                        allDataInHierarchy[parent].Add(subfeature);
                    }
                }
            }


            foreach (var primaryFeature in allDataInHierarchy)
            {
                primaryFeature.Value.Sort();
                var description = $"Items that are specified as {primaryFeature.Key.Name}, but without more specific subfeature information";
                primaryFeature.Value.Insert(0, new OSMSelectableFeature(
                    "yes", "UNTAGGED", description, 0, 0, false, primaryFeature.Key
                ));
            }
                
            return allDataInHierarchy;
        }

        public static readonly Dictionary<string, OSMMetaData> Primary = new Dictionary<string, OSMMetaData>()
        {
            { "aerialway",  new OSMMetaData("aerialway", "Aerialway", "Forms of transportation for people or goods that use aerial wires, e.g. cable-cars & chair-lifts.") },
            { "aeroway",    new OSMMetaData("aeroway", "Aeroway", "Ground facilities  that support the operation of airplanes & helicopters, such as aerodromes & airfields.") },
            { "amenity",    new OSMMetaData("amenity", "Amenity", "Facilities used by visitors & residents. For example: toilets, telephones, banks, pharmacies, cafes, parking & schools.") },
            { "barrier",    new OSMMetaData("barrier", "Barrier", "A physical structure which blocks or impedes movement. The barrier tag only covers on-the-ground barriers, it does not cover waterway barriers (dams, waterfalls...).") },
            { "boundary",   new OSMMetaData("boundary", "Boundary", "Administrative bounds as well as other types of territories, such as a municipal limits, a national park, or a post code.") },
            { "building",   new OSMMetaData("building", "Building", "Individual buildings (e.g. a house) or groups of connected buildings (e.g. terrace housing).") },
            { "craft",      new OSMMetaData("craft", "Craft", "Produces or processors of small amounts of customised goods on demand, such as bakeries, carpenters, photographers, & wineries.") },
            { "emergency",  new OSMMetaData("emergency", "Emergency", "Emergency facilities & equipment, such as fire hydrants, sirens, lifeguard towers, or ambulance stations.") },
            { "geological", new OSMMetaData("geological", "Geological", "The geological makup of an area or location, such as an outcrop or volcanic vent.") },
            { "healthcare", new OSMMetaData("healthcare", "Healthcare", "Services delivered by a variety of different professionals in different types of facilities.") },
            { "highway",    new OSMMetaData("highway", "Highway", "Various forms of road & footpath, including different road types (e.g. motorway/cycleway), path types (e.g. stairs, sidewalks), & associated features (e.g. stop signs).") },
            { "historic",   new OSMMetaData("historic", "Historic", "Features of historic interest & the cause of interest. For example: battlefields, buildings, monuments, tombs.") },
            { "landuse",    new OSMMetaData("landuse", "Landuse", "The purpose for which an area of land is being used, such as commercial, farmyard, military, or port.") },
            { "leisure",    new OSMMetaData("leisure", "Leisure", "Places people go in their spare time. Includes fishing areas, dog parks, saunas, stadia, & dance halls.") },
            { "man_made",   new OSMMetaData("man_made", "Man Made", "Artificial structures that are distinctive landscape features, such as lighthouses, obelisks, breakwaters, & bridges.") },
            { "military",   new OSMMetaData("military", "Military", "Facilities & land used by any branch of any military, such as a naval base, checkpoint, or barracks.") },
            { "natural",    new OSMMetaData("natural", "Natural", "A variety of physical geography, geological, landcover, & biological features (even if they have been modified by humans). Includes trees, tundra, glaciers, & beaches.") },
            { "office",     new OSMMetaData("office", "Office", "A place of business for administrative or professional work. Classified by the type of worker, e.g. architects, educators, engineers, or lawyers.") },
            { "place",      new OSMMetaData("place", "Place", "A type of populated settlement, such as town, village, suburb, as well as unoccupied but identifiable areas at a variety of scales (e.g. an ocean or a plot of land).") },
            { "power",      new OSMMetaData("power", "Power", "Systems & things used to generate & distribute elecctrical power, such as generators, cables, & pylons.") },
            { "public_transport", new OSMMetaData("public_transport", "Public Transport", "Features that relate to public transport, such as bus stops & train stations.") },
            { "railway",    new OSMMetaData("railway", "Railway", "Linear tracks & their features for various types of rail-based transport, e.g. subway lines, tram lines, train platforms, level crossings, & subway entrances.") },
            { "route",      new OSMMetaData("route", "Route", "A customary or regular line of passage or travel of various different types. Includes: ferry routes, hiking routes, a sequences of railways.") },
            { "shop",       new OSMMetaData("shop", "Shop", "A business that has stocked goods for sale. Classified by the type of good (e.g. butcher, jetskis) or the type of building (e.g. mall)") },
            { "sport",      new OSMMetaData("sport", "Sport", "Notes which sports are played on/at a particular facility, e.g. akido or bobsleigh or yoga.") },
            { "telecom",    new OSMMetaData("telecom", "Telecom", "Places & things that create telecommunication systems, such as exchanges & data centers.") },
            { "tourism",    new OSMMetaData("tourism", "Tourism", "Places & things of specific interest to tourists, such as different types of sights (e.g. zoos, galleries); forms of accomodation (e.g. hotels, hostels); & info centers.") },
            { "water",      new OSMMetaData("water", "Water", "An area or body of water, such as lakes, reservoirs & ponds.") },
            { "waterway",   new OSMMetaData("waterway", "Waterway", "Linear water features, such as rivers, streams, drains, & ditches. Also includes related features, such as waterfalls & locks. Nominally aligned to direction of flow.") },
        };
    }
#pragma warning restore SA1310 // Field names should not contain underscore
}
