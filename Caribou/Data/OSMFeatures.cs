namespace Caribou.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class OSMFeatures
    {
#pragma warning disable SA1310 // Field names should not contain underscore
        private static List<FeatureDefinition> aerialwaySubFeatures = new List<FeatureDefinition>();
        private static List<FeatureDefinition> aerowaySubFeatures = new List<FeatureDefinition>();
        private static List<FeatureDefinition> amenitySubFeatures = new List<FeatureDefinition>();
        private static List<FeatureDefinition> barrierSubFeatures = new List<FeatureDefinition>();
        private static List<FeatureDefinition> boundarySubFeatures = new List<FeatureDefinition>();
        private static List<FeatureDefinition> buildingSubFeatures = new List<FeatureDefinition>();
        private static List<FeatureDefinition> craftSubFeatures = new List<FeatureDefinition>();
        private static List<FeatureDefinition> emergencySubFeatures = new List<FeatureDefinition>();
        private static List<FeatureDefinition> geologicalSubFeatures = new List<FeatureDefinition>();
        private static List<FeatureDefinition> healthcareSubFeatures = new List<FeatureDefinition>();
        private static List<FeatureDefinition> highwaySubFeatures = new List<FeatureDefinition>();
        private static List<FeatureDefinition> historicSubFeatures = new List<FeatureDefinition>();
        private static List<FeatureDefinition> landuseSubFeatures = new List<FeatureDefinition>();
        private static List<FeatureDefinition> leisureSubFeatures = new List<FeatureDefinition>();
        private static List<FeatureDefinition> man_madeSubFeatures = new List<FeatureDefinition>();
        private static List<FeatureDefinition> militarySubFeatures = new List<FeatureDefinition>();
        private static List<FeatureDefinition> naturalSubFeatures = new List<FeatureDefinition>();
        private static List<FeatureDefinition> officeSubFeatures = new List<FeatureDefinition>();
        private static List<FeatureDefinition> placeSubFeatures = new List<FeatureDefinition>();
        private static List<FeatureDefinition> powerSubFeatures = new List<FeatureDefinition>();
        private static List<FeatureDefinition> public_transportSubFeatures = new List<FeatureDefinition>();
        private static List<FeatureDefinition> railwaySubFeatures = new List<FeatureDefinition>();
        private static List<FeatureDefinition> routeSubFeatures = new List<FeatureDefinition>();
        private static List<FeatureDefinition> shopSubFeatures = new List<FeatureDefinition>();
        private static List<FeatureDefinition> sportSubFeatures = new List<FeatureDefinition>();
        private static List<FeatureDefinition> telecomSubFeatures = new List<FeatureDefinition>();
        private static List<FeatureDefinition> tourismSubFeatures = new List<FeatureDefinition>();
        private static List<FeatureDefinition> waterSubFeatures = new List<FeatureDefinition>();
        private static List<FeatureDefinition> waterwaySubFeatures = new List<FeatureDefinition>();
#pragma warning restore SA1310 // Field names should not contain underscore

        // From https://wiki.openstreetmap.org/wiki/Map_features
        public static readonly List<FeatureDefinition> PrimaryFeatures = new List<FeatureDefinition>()
        {
            new FeatureDefinition("aerialway", "Aerialway", "Forms of transportation for people or goods that use aerial wires, e.g. cable-cars and chair-lifts.", aerialwaySubFeatures),
            new FeatureDefinition("aeroway", "Aeroway", "Ground facilities  that support the operation of airplanes and helicopters, such as aerodromes and airfields.", aerowaySubFeatures),
            new FeatureDefinition("amenity", "Amenity", "Facilities used by visitors and residents. For example: toilets, telephones, banks, pharmacies, cafes, parking and schools.", amenitySubFeatures),
            new FeatureDefinition("barrier", "Barrier", "A physical structure which blocks or impedes movement. The barrier tag only covers on-the-ground barriers, it does not cover waterway barriers (dams, waterfalls...).", barrierSubFeatures),
            new FeatureDefinition("boundary", "Boundary", "Administrative bounds as well as other types of territories, such as a municipal limits, a national park, or a post code.", boundarySubFeatures),
            new FeatureDefinition("building", "Building", "Individual buildings (e.g. a house) or groups of connected buildings (e.g. terrace housing).", buildingSubFeatures),
            new FeatureDefinition("craft", "Craft", "Paces that produce or process customised goods, such as bakeries, carpenters, photographers, and wineries. Unlike shops, these are places that produce small amounts of custmised goods on demand and by order.", craftSubFeatures),
            new FeatureDefinition("emergency", "Emergency", "Emergency facilities and equipment, such as fire hydrants, sirens, lifeguard towers, or ambulance stations.", emergencySubFeatures),
            new FeatureDefinition("geological", "Geological", "Describes the geological makup of an area or location, such as an outcrop or volcanic vent.", geologicalSubFeatures),
            new FeatureDefinition("healthcare", "Healthcare", "Services delivered by a variety of different professionals in different types of facilities.", healthcareSubFeatures),
            new FeatureDefinition("highway", "Highway", "Various forms of road and footpath, including different road types (e.g. motorway/cycleway), path types (e.g. stairs, sidewalks), and associated features (e.g. stop signs).", highwaySubFeatures),
            new FeatureDefinition("historic", "Historic", "Features of historic interest and the cause of interest. For example: battlefields, buildings, monuments, tombs.", historicSubFeatures),
            new FeatureDefinition("landuse", "Landuse", "The purpose for which an area of land is being used, such as commercial, farmyard, military, or port.", landuseSubFeatures),
            new FeatureDefinition("leisure", "Leisure", "Places people go in their spare time. Includes fishing areas, dog parks, saunas, stadia, and dance halls.", leisureSubFeatures),
            new FeatureDefinition("man_made", "Man Made", "Artificial structures that are distinctive landscape features, such as lighthouses, obelisks, breakwaters, and bridges.", man_madeSubFeatures),
            new FeatureDefinition("military", "Military", "Facilities and land used by any branch of any military, such as a naval base, checkpoint, or barracks.", militarySubFeatures),
            new FeatureDefinition("natural", "Natural", "Describes a variety of physical geography, geological, landcover, and biological features (even if they have been modified by humans). Includes trees, tundra, glaciers, and beaches.", naturalSubFeatures),
            new FeatureDefinition("office", "Office", "A place of business for administrative or professional work. Classified by the type of worker, e.g. architects, educators, engineers, or lawyers.", officeSubFeatures),
            new FeatureDefinition("place", "Place", "A type of populated settlement, such as town, village, suburb, as well as unoccupied but identifiable areas at a variety of scales. (e.g. an ocean or a plot of land).", placeSubFeatures),
            new FeatureDefinition("power", "Power", "Systems and things used to generate and distribute elecctrical power, such as generators, cables, and pylons.", powerSubFeatures),
            new FeatureDefinition("public_transport", "Public Transport", "Features that relate to public transport, such as bus stops and train stations.", public_transportSubFeatures),
            new FeatureDefinition("railway", "Railway", "Linear tracks and their features for various types of rail-based transport, e.g. subway lines, tram lines, train platforms, level crossings, and subway entrances.", railwaySubFeatures),
            new FeatureDefinition("route", "Route", "A customary or regular line of passage or travel of various different types. Includes: ferry routes, hiking routes, a sequences of railways.", routeSubFeatures),
            new FeatureDefinition("shop", "Shop", "A business that has stocked goods for sale. Classified by the type of good (e.g. butcher, jetskis) or the type of building (e.g. mall)", shopSubFeatures),
            new FeatureDefinition("sport", "Sport", "Notes which sports are played on/at a particular facility, e.g. akido or bobsleigh or yoga.", sportSubFeatures),
            new FeatureDefinition("telecom", "Telecom", "Places and things that create telecommunication systems, such as exchanges and data centers.", telecomSubFeatures),
            new FeatureDefinition("tourism", "Tourism", "Places and things of specific interest to tourists, such as different types of sights (e.g. zoos, galleries); forms of accomodation (e.g. hotels, hostels); and info centerss.", tourismSubFeatures),
            new FeatureDefinition("water", "Water", "Describes an area or body of water, such as lakes, reservoirs and ponds.", waterSubFeatures),
            new FeatureDefinition("waterway", "Waterway", "Describes linear water features, such as rivers, streams, drains, and ditches. Also includes related features, such as waterfalls and locks. Nominally aligned in the direction of the water flow.", waterwaySubFeatures),
        };
    }
}
