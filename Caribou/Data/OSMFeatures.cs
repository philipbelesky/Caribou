using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caribou.Data
{
    public static class OSMFeatures
    {
        private static List<Feature> aerialwaySubFeatures = new List<Feature>();
        private static List<Feature> aerowaySubFeatures = new List<Feature>();
        private static List<Feature> amenitySubFeatures = new List<Feature>();
        private static List<Feature> barrierSubFeatures = new List<Feature>();
        private static List<Feature> boundarySubFeatures = new List<Feature>();
        private static List<Feature> buildingSubFeatures = new List<Feature>();
        private static List<Feature> craftSubFeatures = new List<Feature>();
        private static List<Feature> emergencySubFeatures = new List<Feature>();
        private static List<Feature> geologicalSubFeatures = new List<Feature>();
        private static List<Feature> healthcareSubFeatures = new List<Feature>();
        private static List<Feature> highwaySubFeatures = new List<Feature>();
        private static List<Feature> historicSubFeatures = new List<Feature>();
        private static List<Feature> landuseSubFeatures = new List<Feature>();
        private static List<Feature> leisureSubFeatures = new List<Feature>();
        private static List<Feature> man_madeSubFeatures = new List<Feature>();
        private static List<Feature> militarySubFeatures = new List<Feature>();
        private static List<Feature> naturalSubFeatures = new List<Feature>();
        private static List<Feature> officeSubFeatures = new List<Feature>();
        private static List<Feature> placeSubFeatures = new List<Feature>();
        private static List<Feature> powerSubFeatures = new List<Feature>();
        private static List<Feature> public_transportSubFeatures = new List<Feature>();
        private static List<Feature> railwaySubFeatures = new List<Feature>();
        private static List<Feature> routeSubFeatures = new List<Feature>();
        private static List<Feature> shopSubFeatures = new List<Feature>();
        private static List<Feature> sportSubFeatures = new List<Feature>();
        private static List<Feature> telecomSubFeatures = new List<Feature>();
        private static List<Feature> tourismSubFeatures = new List<Feature>();
        private static List<Feature> waterSubFeatures = new List<Feature>();
        private static List<Feature> waterwaySubFeatures = new List<Feature>();

        // From https://wiki.openstreetmap.org/wiki/Map_features
        public static readonly List<Feature> PrimaryFeatures = new List<Feature>()
        {
            new Feature("aerialway", "Aerialway", "Forms of transportation for people or goods that use aerial wires, e.g. cable-cars and chair-lifts.", aerialwaySubFeatures),
            new Feature("aeroway", "Aeroway", "Ground facilities  that support the operation of airplanes and helicopters, such as aerodromes and airfields.", aerowaySubFeatures),
            new Feature("amenity", "Amenity", "Facilities used by visitors and residents. For example: toilets, telephones, banks, pharmacies, cafes, parking and schools.", amenitySubFeatures),
            new Feature("barrier", "Barrier", "A physical structure which blocks or impedes movement. The barrier tag only covers on-the-ground barriers, it does not cover waterway barriers (dams, waterfalls...).", barrierSubFeatures),
            new Feature("boundary", "Boundary", "Administrative bounds as well as other types of territories, such as a municipal limits, a national park, or a post code.", boundarySubFeatures),
            new Feature("building", "Building", "Individual buildings (e.g. a house) or groups of connected buildings (e.g. terrace housing).", buildingSubFeatures),
            new Feature("craft", "Craft", "Paces that produce or process customised goods, such as bakeries, carpenters, photographers, and wineries. Unlike shops, these are places that produce small amounts of custmised goods on demand and by order.", craftSubFeatures),
            new Feature("emergency", "Emergency", "Emergency facilities and equipment, such as fire hydrants, sirens, lifeguard towers, or ambulance stations.", emergencySubFeatures),
            new Feature("geological", "Geological", "Describes the geological makup of an area or location, such as an outcrop or volcanic vent.", geologicalSubFeatures),
            new Feature("healthcare", "Healthcare", "Services delivered by a variety of different professionals in different types of facilities.", healthcareSubFeatures),
            new Feature("highway", "Highway", "Various forms of road and footpath, including different road types (e.g. motorway/cycleway), path types (e.g. stairs, sidewalks), and associated features (e.g. stop signs).", highwaySubFeatures),
            new Feature("historic", "Historic", "Features of historic interest and the cause of interest. For example: battlefields, buildings, monuments, tombs.", historicSubFeatures),
            new Feature("landuse", "Landuse", "The purpose for which an area of land is being used, such as commercial, farmyard, military, or port.", landuseSubFeatures),
            new Feature("leisure", "Leisure", "Places people go in their spare time. Includes fishing areas, dog parks, saunas, stadia, and dance halls.", leisureSubFeatures),
            new Feature("man_made", "Man Made", "Artificial structures that are distinctive landscape features, such as lighthouses, obelisks, breakwaters, and bridges.", man_madeSubFeatures),
            new Feature("military", "Military", "Facilities and land used by any branch of any military, such as a naval base, checkpoint, or barracks.", militarySubFeatures),
            new Feature("natural", "Natural", "Describes a variety of physical geography, geological, landcover, and biological features (even if they have been modified by humans). Includes trees, tundra, glaciers, and beaches.", naturalSubFeatures),
            new Feature("office", "Office", "A place of business for administrative or professional work. Classified by the type of worker, e.g. architects, educators, engineers, or lawyers.", officeSubFeatures),
            new Feature("place", "Place", "A type of populated settlement, such as town, village, suburb, as well as unoccupied but identifiable areas at a variety of scales. (e.g. an ocean or a plot of land).", placeSubFeatures),
            new Feature("power", "Power", "Systems and things used to generate and distribute elecctrical power, such as generators, cables, and pylons.", powerSubFeatures),
            new Feature("public_transport", "Public Transport", "Features that relate to public transport, such as bus stops and train stations.", public_transportSubFeatures),
            new Feature("railway", "Railway", "Linear tracks and their features for various types of rail-based transport, e.g. subway lines, tram lines, train platforms, level crossings, and subway entrances.", railwaySubFeatures),
            new Feature("route", "Route", "A customary or regular line of passage or travel of various different types. Includes: ferry routes, hiking routes, a sequences of railways.", routeSubFeatures),
            new Feature("shop", "Shop", "A business that has stocked goods for sale. Classified by the type of good (e.g. butcher, jetskis) or the type of building (e.g. mall)", shopSubFeatures),
            new Feature("sport", "Sport", "Notes which sports are played on/at a particular facility, e.g. akido or bobsleigh or yoga.", sportSubFeatures),
            new Feature("telecom", "Telecom", "Places and things that create telecommunication systems, such as exchanges and data centers.", telecomSubFeatures),
            new Feature("tourism", "Tourism", "Places and things of specific interest to tourists, such as different types of sights (e.g. zoos, galleries); forms of accomodation (e.g. hotels, hostels); and info centerss.", tourismSubFeatures),
            new Feature("water", "Water", "Describes an area or body of water, such as lakes, reservoirs and ponds.", waterSubFeatures),
            new Feature("waterway", "Waterway", "Describes linear water features, such as rivers, streams, drains, and ditches. Also includes related features, such as waterfalls and locks. Nominally aligned in the direction of the water flow.", waterwaySubFeatures),    };
    }
}
