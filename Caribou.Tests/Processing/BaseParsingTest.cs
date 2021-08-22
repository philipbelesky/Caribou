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
        protected static MessagesWrapper messages = new MessagesWrapper();
        public static Action<string, double> reportProgress;
        protected readonly OSMMetaData craftsData = new OSMMetaData("craft");
        protected readonly OSMMetaData amenitiesData = new OSMMetaData("amenity");
        protected readonly OSMMetaData buildingsData = new OSMMetaData("building");
        protected readonly OSMMetaData highwaysData = new OSMMetaData("highway");

        protected readonly OSMMetaData amenitiesRestaurantsData = new OSMMetaData("restaurant", "amenity");
        protected readonly OSMMetaData craftJewellersData = new OSMMetaData("jeweller", "craft");
        protected readonly OSMMetaData buildingsRetailData = new OSMMetaData("retail", "building");
        protected readonly OSMMetaData amenitiesWorshipData = new OSMMetaData("place_of_worship", "amenity");
        protected readonly OSMMetaData highwayResidentialData = new OSMMetaData("residential", "highway");

        protected readonly OSMMetaData namedThingsData = new OSMMetaData("name");
        protected readonly OSMMetaData wikiRelatedData = new OSMMetaData("wikipedia");
        protected readonly OSMMetaData tramRoutesData = new OSMMetaData("tram", "route_master");
        protected readonly OSMMetaData tramStopsData = new OSMMetaData("yes", "tram_stop");
        
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
