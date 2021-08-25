namespace Caribou.Tests.Processing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Caribou.Components;
    using Caribou.Models;
    using Caribou.Processing;

    public abstract class BaseParsingTest
    {
        public static Action<string, double> reportProgress;
        protected readonly OSMMetaData craftsData = new OSMMetaData("craft");
        protected readonly OSMMetaData amenitiesData = new OSMMetaData("amenity");
        protected readonly OSMMetaData buildingsData = new OSMMetaData("building");
        protected readonly OSMMetaData highwaysData = new OSMMetaData("highway");

        protected readonly OSMMetaData amenitiesRestaurantsData = new OSMMetaData("amenity", "restaurant");
        protected readonly OSMMetaData craftJewellersData = new OSMMetaData("craft", "jeweller");
        protected readonly OSMMetaData buildingsRetailData = new OSMMetaData("building", "retail");
        protected readonly OSMMetaData amenitiesWorshipData = new OSMMetaData("amenity", "place_of_worship");
        protected readonly OSMMetaData highwayResidentialData = new OSMMetaData("highway", "residential");

        protected readonly OSMMetaData namedThingsData = new OSMMetaData("name");
        protected readonly OSMMetaData wikiRelatedData = new OSMMetaData("wikipedia");
        protected readonly OSMMetaData tramRoutesData = new OSMMetaData("route_master", "tram");
        protected readonly OSMMetaData tramStopsData = new OSMMetaData("tram_stop", "yes");
        
        protected static RequestHandler fetchResultsViaXMLReader(List<string> xml, ParseRequest features, OSMGeometryType typeOfFeature)
        {
            var results = new RequestHandler(xml, features, OSMGeometryType.Node, reportProgress, "Test");
            ParseViaXMLReader.FindItemsByTag(ref results, typeOfFeature, true);
            return results;
        }

        protected static int CountNodesForMetaData(RequestHandler results, OSMMetaData request)
        {
            var allResults = results.FoundData[request];
            var nodeResults = allResults.Where(o => o.Kind == OSMGeometryType.Node);
            return nodeResults.Count();
        }

        protected static int CountWaysForMetaData(RequestHandler results, OSMMetaData request)
        {
            var allResults = results.FoundData[request];
            var nodeResults = allResults.Where(o => o.Kind == OSMGeometryType.Way);
            return nodeResults.Count();
        }
    }
}
