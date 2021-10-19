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
            { "attribution", "" },
            { "brand:wikipedia", "" },
            { "brand:wikidata", "" },
            { "check_date", "" },
            { "check_date:lit", "" },
            { "created_by", "" },
            { "description", "" },
            { "email", "" },
            { "linz:layer", ""},
            { "linz:source_version", ""},
            { "linz2osm:dataset", ""},
            { "linz2osm:layer", ""},
            { "linz2osm:objectid", ""},
            { "linz2osm:source_version", ""},
            { "name", ""},
            { "note", ""},
            { "old_name", ""},
            { "phone", ""},
            { "ref", ""},
            { "ref:linz:address_id", "" },
            {"source:date", ""},
            {"source:geometry", ""},
            {"source:maxspeed", ""},
            {"source:name", ""},
            {"source_ref", ""},
            {"source", ""},
            {"start", ""},
            {"start_date", ""},
            {"url", ""},
            { "website", "" },
            { "wikidata", "" },
            { "wikimedia_commmons", "" },
            { "wikipedia", "" }
        };
    }
}
