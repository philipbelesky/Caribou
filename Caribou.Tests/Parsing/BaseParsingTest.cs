namespace Caribou.Tests.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Caribou.Models;
    using Caribou.Processing;

    public abstract class BaseParsingTest
    {
        public static Action<string, double> reportProgress;
        protected readonly OSMTag craftsData = new OSMTag("craft");
        protected readonly OSMTag amenitiesData = new OSMTag("amenity");
        protected readonly OSMTag buildingsData = new OSMTag("building");
        protected readonly OSMTag highwaysData = new OSMTag("highway");

        protected readonly OSMTag amenitiesRestaurantsData = new OSMTag("amenity", "restaurant");
        protected readonly OSMTag craftJewellersData = new OSMTag("craft", "jeweller");
        protected readonly OSMTag buildingsRetailData = new OSMTag("building", "retail");
        protected readonly OSMTag amenitiesWorshipData = new OSMTag("amenity", "place_of_worship");
        protected readonly OSMTag highwayResidentialData = new OSMTag("highway", "residential");

        protected readonly OSMTag namedThingsData = new OSMTag("name");
        protected readonly OSMTag wikiRelatedData = new OSMTag("wikipedia");
        protected readonly OSMTag tramRoutesData = new OSMTag("route_master", "tram");
        protected readonly OSMTag tramStopsData = new OSMTag("tram_stop", "yes");
        
        protected static RequestHandler fetchResultsViaXMLReader(List<string> xml, ParseRequest features, OSMGeometryType typeOfFeature)
        {
            var results = new RequestHandler(xml, features, OSMGeometryType.Node, reportProgress, "Test");
            ParseViaXMLReader.FindItemsByTag(ref results, typeOfFeature, true);
            return results;
        }

        protected static int CountNodesForMetaData(RequestHandler results, OSMTag request)
        {
            var allResults = results.FoundData[request];
            var nodeResults = allResults.Where(o => o.Kind == OSMGeometryType.Node);
            return nodeResults.Count();
        }

        protected static int CountWaysForMetaData(RequestHandler results, OSMTag request)
        {
            var allResults = results.FoundData[request];
            var nodeResults = allResults.Where(o => o.Kind == OSMGeometryType.Way);
            return nodeResults.Count();
        }
    }
}
