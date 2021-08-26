using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caribou.Tests.Cases
{
    // Query from amenity=parking and building:levels=5 on singapore top/left/right OSM files
    public static class TagsTestClases
    {
        public static List<string> itemATags = new List<string>()
        {
            "name=Futuna Chapel",
            "amenity=place_of_worship",
            "denomination=catholic",
            "building=yes",
        };

        public static List<string> itemBTags = new List<string>()
        {
            "amenity=arts_centre",
            "building=yes",
        };

        public static List<List<List<string>>> GetTagsCaseData()
        {
            var branch0 = new List<List<string>>() { };
            var branch1 = new List<List<string>>() { };

            branch0.Add(new List<string>() {
                "amenity=parking",
                "fee=yes",
                "name=Maritime Square D",
                "parking=surface",
                "surface=paved",
            });

            branch0.Add(new List<string>() {
                "amenity=parking",
            });

            branch0.Add(new List<string>() {
                "amenity=parking",
            });

            branch0.Add(new List<string>() {
                "amenity=parking",
            });

            branch0.Add(new List<string>() {
                "amenity=parking",
            });

            branch0.Add(new List<string>() {
                "access=yes",
                "amenity=parking",
                "building=parking",
                "name=Keppel Club",
                "parking=multi-storey",
            });

            branch0.Add(new List<string>() {
                "amenity=parking",
            });

            branch0.Add(new List<string>() {
                "access=yes",
                "addr:city=Singapore",
                "addr:country=SG",
                "addr:housenumber=52A",
                "addr:neighbourhood=Blangah View",
                "addr:postcode=101052",
                "addr:street=Telok Blangah Drive",
                "amenity=parking",
                "building=garage",
                "building:levels=7",
                "fee=yes",
            });

            branch0.Add(new List<string>() {
                "access=yes",
                "addr:city=Singapore",
                "addr:country=SG",
                "addr:housenumber=78A",
                "addr:postcode=101078",
                "addr:street=Telok Blangah Street 32",
                "amenity=parking",
                "building=parking",
                "building:levels=6",
                "fee=yes",
                "parking=multi-storey",
            });

            branch0.Add(new List<string>() {
                "addr:city=Singapore",
                "addr:country=SG",
                "addr:housenumber=57A",
                "addr:neighbourhood=Blangah View",
                "addr:postcode=101057",
                "addr:street=Telok Blangah Heights",
                "amenity=parking",
                "building=garage",
                "building:levels=5",
                "parking=multi-storey",
            });

            branch0.Add(new List<string>() {
                "access=yes",
                "amenity=parking",
            });

            branch1.Add(new List<string>(){
                "building:colour=#b7b4c0",
                "building:levels=5",
                "building:material=metal",
                "building:part=yes",
                "roof:colour=#858b7d",
            });

            branch1.Add(new List<string>(){
                "building:colour=#b7b4c0",
                "building:levels=5",
                "building:material=metal",
                "building:part=yes",
                "roof:colour=#858b7d",
            });

            branch1.Add(new List<string>(){
                "building:colour=#b7b4c0",
                "building:levels=5",
                "building:material=metal",
                "building:part=yes",
                "roof:colour=#858b7d",
            });

            branch1.Add(new List<string>(){
                "building=yes",
                "building:levels=5",
            });

            branch1.Add(new List<string>(){
                "building:levels=5",
                "building:part=yes",
            });

            branch1.Add(new List<string>(){
                "building:colour=#d4d6cb",
                "building:levels=5",
                "building:material=concrete",
                "building:part=yes",
            });

            branch1.Add(new List<string>(){
                "addr:city=Singapore",
                "addr:country=SG",
                "addr:housenumber=57A",
                "addr:neighbourhood=Blangah View",
                "addr:postcode=101057",
                "addr:street=Telok Blangah Heights",
                "amenity=parking",
                "building=garage",
                "building:levels=5",
                "parking=multi-storey",
            });

            branch1.Add(new List<string>(){
                "addr:city=Singapore",
                "addr:country=SG",
                "addr:housenumber=303",
                "addr:postcode=108925",
                "addr:street=Henderson Road",
                "amenity=social_facility",
                "branch=Henderson",
                "building=residential",
                "building:levels=5",
                "email=hnhenquiries@sanh.org.sg",
                "name=St. Andrew's Nursing Home",
                "phone=+65 6430 9449",
                "short_name=SANH (Henderson)",
                "social_facility=nursing_home",
                "website=https://www.samh.org.sg/st-andrews-nursing-home/",
            });

            var allTags = new List<List<List<string>>>() { branch0, branch1 };
            return allTags;
        }
    }
}
