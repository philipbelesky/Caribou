namespace Caribou.Processing
{
    using System.Xml;
    public class FindNodesViaXMLDocument
    {
        public static ResultsForFeatures FindByFeatures(RequestedFeature[] featuresSpecified, string xmlContents)
        {
            var matches = new ResultsForFeatures(featuresSpecified); // Output


            int studentCount = 0;
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlContents);
            XmlNodeList itemRefList = doc.GetElementsByTagName("Student");
            foreach (XmlNode node in itemRefList)
            {
                XmlNode id = node.SelectSingleNode("Id");
                if (!string.IsNullOrEmpty(id.InnerText))
                {
                    studentCount++;
                }
            }

            return matches;
        }
    }
}
