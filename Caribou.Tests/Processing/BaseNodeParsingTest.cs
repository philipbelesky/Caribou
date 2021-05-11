namespace Caribou.Tests.Processing
{
    using Caribou.Components;
    using Caribou.Data;
    using Caribou.Processing;

    public abstract class BaseNodeParsingTest
    {
        protected static MessagesWrapper messages = new MessagesWrapper();
        protected readonly OSMMetaData craftsData = new OSMMetaData("craft");
        protected readonly OSMMetaData amenitiesData = new OSMMetaData("amenity");
        protected readonly OSMMetaData buildingsData = new OSMMetaData("building");
        protected readonly OSMMetaData highwaysData = new OSMMetaData("highway");

        protected readonly OSMMetaData amenitiesRestaurantsData = new OSMMetaData("restaurant", "amenity");
        protected readonly OSMMetaData craftJewellersData = new OSMMetaData("jeweller", "craft");
        protected readonly OSMMetaData buildingsRetailData = new OSMMetaData("retail", "building");
        protected readonly OSMMetaData amenitiesWorshipData = new OSMMetaData("place_of_worship", "amenity");
        protected readonly OSMMetaData highwayResidentialData = new OSMMetaData("residential", "highway");

        protected static RequestHandler fetchResultsViaXMLReader(OSMXMLFiles xml, ParseRequest features, string typeOfFeature)
        {
            var results = new RequestHandler(xml, features);
            ParseViaXMLReader.FindItemsByTag(ref results, typeOfFeature);
            return results;
        }
    }
}
