using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caribou.Models
{
    /// <summary>Provides a hardcoded list of tags to hide, by default, from the filter form</summary>
    public static class OSMSpecialTags
    {
        public static Dictionary<string, string> numericTags = new Dictionary<string, string>
        {
            { "distance", "" },
            { "width", "" },
            { "building:levels", "" },
            { "height", "" },
            { "roof:levels", "" },
            { "level", "" },
            { "levels", "" },
            { "capacity", "" },
            { "maxheight", "" },
            { "maxspeed", "" }
        };

        public static Dictionary<string, string> uniqueTags =  new Dictionary<string, string> {
            { "addr:housename", "" },
            { "addr:housenumber", "" },
            { "brand:wikipedia", "" },
            { "brand:wikidata", "" },
            { "email", "" },
            { "name", "" },
            { "phone", "" },
            { "ref", "" },
            { "ref:linz:address_id", "" },
            { "website", "" },
            { "wikidata", "" },
            { "wikimedia_commmons", "" },
            { "wikipedia", "" }
        };
    }
}
