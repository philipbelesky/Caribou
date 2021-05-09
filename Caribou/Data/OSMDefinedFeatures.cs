namespace Caribou.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
#pragma warning disable SA1310 // Field names should not contain underscore

    /// <summary>A predefined list of major/common feature and subfeature pairings specified on the
    /// [OSM wiki](https://wiki.openstreetmap.org/wiki/Map_features).</summary>
    public static class OSMDefinedFeatures
    {
        public static readonly List<OSMMetaData> PrimaryFeatures = new List<OSMMetaData>()
        {
            new OSMMetaData("aerialway", "Aerialway", true, "Forms of transportation for people or goods that use aerial wires, e.g. cable-cars and chair-lifts."),
            new OSMMetaData("aeroway", "Aeroway", true, "Ground facilities  that support the operation of airplanes and helicopters, such as aerodromes and airfields."),
            new OSMMetaData("amenity", "Amenity", true, "Facilities used by visitors and residents. For example: toilets, telephones, banks, pharmacies, cafes, parking and schools."),
            new OSMMetaData("barrier", "Barrier", true, "A physical structure which blocks or impedes movement. The barrier tag only covers on-the-ground barriers, it does not cover waterway barriers (dams, waterfalls...)."),
            new OSMMetaData("boundary", "Boundary", true, "Administrative bounds as well as other types of territories, such as a municipal limits, a national park, or a post code."),
            new OSMMetaData("building", "Building", true, "Individual buildings (e.g. a house) or groups of connected buildings (e.g. terrace housing)."),
            new OSMMetaData("craft", "Craft", true, "Paces that produce or process customised goods, such as bakeries, carpenters, photographers, and wineries. Unlike shops, these are places that produce small amounts of custmised goods on demand and by order."),
            new OSMMetaData("emergency", "Emergency", true, "Emergency facilities and equipment, such as fire hydrants, sirens, lifeguard towers, or ambulance stations."),
            new OSMMetaData("geological", "Geological", true, "Describes the geological makup of an area or location, such as an outcrop or volcanic vent."),
            new OSMMetaData("healthcare", "Healthcare", true, "Services delivered by a variety of different professionals in different types of facilities."),
            new OSMMetaData("highway", "Highway", true, "Various forms of road and footpath, including different road types (e.g. motorway/cycleway), path types (e.g. stairs, sidewalks), and associated features (e.g. stop signs)."),
            new OSMMetaData("historic", "Historic", true, "Features of historic interest and the cause of interest. For example: battlefields, buildings, monuments, tombs."),
            new OSMMetaData("landuse", "Landuse", true, "The purpose for which an area of land is being used, such as commercial, farmyard, military, or port."),
            new OSMMetaData("leisure", "Leisure", true, "Places people go in their spare time. Includes fishing areas, dog parks, saunas, stadia, and dance halls."),
            new OSMMetaData("man_made", "Man Made", true, "Artificial structures that are distinctive landscape features, such as lighthouses, obelisks, breakwaters, and bridges."),
            new OSMMetaData("military", "Military", true, "Facilities and land used by any branch of any military, such as a naval base, checkpoint, or barracks."),
            new OSMMetaData("natural", "Natural", true, "Describes a variety of physical geography, geological, landcover, and biological features (even if they have been modified by humans). Includes trees, tundra, glaciers, and beaches."),
            new OSMMetaData("office", "Office", true, "A place of business for administrative or professional work. Classified by the type of worker, e.g. architects, educators, engineers, or lawyers."),
            new OSMMetaData("place", "Place", true, "A type of populated settlement, such as town, village, suburb, as well as unoccupied but identifiable areas at a variety of scales. (e.g. an ocean or a plot of land)."),
            new OSMMetaData("power", "Power", true, "Systems and things used to generate and distribute elecctrical power, such as generators, cables, and pylons."),
            new OSMMetaData("public_transport", "Public Transport", true, "Features that relate to public transport, such as bus stops and train stations."),
            new OSMMetaData("railway", "Railway", true, "Linear tracks and their features for various types of rail-based transport, e.g. subway lines, tram lines, train platforms, level crossings, and subway entrances."),
            new OSMMetaData("route", "Route", true, "A customary or regular line of passage or travel of various different types. Includes: ferry routes, hiking routes, a sequences of railways."),
            new OSMMetaData("shop", "Shop", true, "A business that has stocked goods for sale. Classified by the type of good (e.g. butcher, jetskis) or the type of building (e.g. mall)"),
            new OSMMetaData("sport", "Sport", true, "Notes which sports are played on/at a particular facility, e.g. akido or bobsleigh or yoga."),
            new OSMMetaData("telecom", "Telecom", true, "Places and things that create telecommunication systems, such as exchanges and data centers."),
            new OSMMetaData("tourism", "Tourism", true, "Places and things of specific interest to tourists, such as different types of sights (e.g. zoos, galleries); forms of accomodation (e.g. hotels, hostels); and info centerss."),
            new OSMMetaData("water", "Water", true, "Describes an area or body of water, such as lakes, reservoirs and ponds."),
            new OSMMetaData("waterway", "Waterway", true, "Describes linear water features, such as rivers, streams, drains, and ditches. Also includes related features, such as waterfalls and locks. Nominally aligned in the direction of the water flow."),
        };
    }
#pragma warning restore SA1310 // Field names should not contain underscore
}
