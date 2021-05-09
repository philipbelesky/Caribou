namespace Caribou.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Grasshopper;
    using Grasshopper.Kernel.Data;
    using Grasshopper.Kernel.Types;

    // General purpose wrapped of the functionality required to output results to Grasshopper data
    public class RequestHandler
    {
        public OSMXMLs XmlCollection;
        private ParseRequest requestedMetaData;
        public Coord MinBounds;
        public Coord MaxBounds;

        public RequestHandler(OSMXMLs providedXMLs, ParseRequest requestedMetaData)
        {
            this.XmlCollection = providedXMLs;
            this.requestedMetaData = requestedMetaData;
        }

        public GH_Structure<GH_String> MakeTreeForItemTags()
        {
            // TODO
            return new GH_Structure<GH_String>();
        }

        public GH_Structure<GH_String> MakeTreeForMetaDataReport()
        {
            // TODO
            return new GH_Structure<GH_String>();
        }

    }
}
