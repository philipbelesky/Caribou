using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using System.IO;

namespace Caribou.Processing
{
    public class FindNodes
    {
        public static Dictionary<string, List<Coords>> FindByFeaturesA(Dictionary<string, string> featuresSpecified, string xmlContents)
        {
            // Output
            var matches = new Dictionary<string, List<Coords>>();
            foreach (string key in featuresSpecified.Keys)
            {
                matches[key + ":" + featuresSpecified[key]] = new List<Coords>();
            }

            using (XmlReader reader = XmlReader.Create(new StringReader(xmlContents)))
            {
                // XmlReader is forward-only so need to track the lat/long of the parent element to use if we find a match
                double latitude = 0.0;
                double longitude = 0.0;
                string tagKey;
                string tagValue;
                string[] featureKeys = featuresSpecified.Keys.ToArray(); // TODO: is this actually faster than just checking the dict?
                
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        if (reader.Name == "node")
                        {
                            latitude = Convert.ToDouble(reader.GetAttribute("lat"));
                            longitude = Convert.ToDouble(reader.GetAttribute("lon"));
                        }
                        else if (reader.Name == "tag")
                        {
                            tagKey = reader.GetAttribute("k");
                            if (featureKeys.Contains(tagKey))
                            {
                                tagValue = reader.GetAttribute("v");
                                if (tagValue == featuresSpecified[tagKey])
                                {
                                    matches[tagKey + ":" + tagValue].Add(new Coords(latitude, longitude));
                                }
                                // TODO: skip read to next <node> value rather than continue reading subsequent sibling tags
                            }
                        }
                    }
                }
            }
            return matches;
        }
    }
}
