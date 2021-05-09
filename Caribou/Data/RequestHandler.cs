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
        public OSMXMLs xmlCollection;
        private ParseRequest requestedMetaData;
        public Coord minBounds;
        public Coord maxBounds;

        public RequestHandler(OSMXMLs providedXMLs, ParseRequest requestedMetaData)
        {
            this.xmlCollection = providedXMLs;
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
