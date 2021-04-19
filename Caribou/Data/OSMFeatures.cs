using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caribou.Data
{
    public static class OSMFeatures
    {
        // From https://wiki.openstreetmap.org/wiki/Map_features
        public static readonly List<Feature> PrimaryFeatures = new List<Feature>()
        {
            new Feature("aerialway", "Aerialway", "This is used to tag different forms of transportation for people or goods by using aerial wires. For example these may include cable-cars, chair-lifts and drag-lifts."),
            new Feature("aeroway", "Aeroway", "These are mainly related to aerodromes, airfields other ground facilities that support the operation of airplanes and helicopters."),
            new Feature("amenity", "Amenity", "Used to map facilities used by visitors and residents. For example: toilets, telephones, banks, pharmacies, cafes, parking and schools."),
            new Feature("barrier", "Barrier", "A barrier is a physical structure which blocks or impedes movement. The barrier tag only covers on-the-ground barriers, it does not cover waterway barriers (dams, waterfalls...)."),
            new Feature("boundary", "Boundary", "Used to describe administrative and other designated areas, such as a political territory, a national park, or a post code."),
            new Feature("building", "Building", "Identifies individual buildings (e.g. a house) or groups of connected buildings (e.g. terrace housing)."),
            new Feature("craft", "Craft", "Identifies places that produce or process customised goods, such as bakeries, carpenters, photographers, and wineries. Unlike shops, these are places that produce small amounts of custmised goods on demand and by order."),
            new Feature("emergency", "Emergency", "Describes the location of emergency facilities and equipment, such as fire hydrants, sirens, lifeguard towers, or ambulance stations."),
            new Feature("geological", "Geological", "Describes the geological makup of an area or location, such as an outcrop or volcanic vent."),
            new Feature("healthcare", "Healthcare", "Describes services delivered by a variety of different professionals in different types of facilities."),
            new Feature("highway", "Highway", "Describes various forms of road and footpath, including different road types (e.g. motorway/cycleway), path types (e.g. stairs, sidewalks), and associated features (e.g. stop signs)."),
            new Feature("historic", "Historic", "Identifies features of historic interest and the cause of interest. For example: battlefields, buildings, monuments, tombs."),
            new Feature("landuse", "Landuse", ""),
            new Feature("leisure", "Leisure", ""),
            new Feature("man_made", "Man_made", ""),
            new Feature("military", "Military", ""),
            new Feature("natural", "Natural", ""),
            new Feature("office", "Office", ""),
            new Feature("place", "Place", ""),
            new Feature("power", "Power", ""),
            new Feature("public_transport", "Public_Transport", ""), 
            new Feature("railway", "Railway", ""),
            new Feature("route", "Route", ""),
            new Feature("shop", "Shop", ""),
            new Feature("sport", "Sport", ""),
            new Feature("telecom", "Telecom", ""),
            new Feature("tourism", "Tourism", ""),
            new Feature("water", "Water", "Describes an area or body of water, such as lakes, reservoirs and ponds."),
            new Feature("waterway", "Waterway", "Describes linear water features, such as rivers, streams, drains, and ditches. Also includes related features, such as waterfalls and locks. Nominally aligned in the direction of the water flow."),
        };
    }
}
