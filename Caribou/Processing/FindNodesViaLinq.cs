namespace Caribou.Processing
{
    using System.Linq;
    using System.Xml.Linq;

    public class FindNodesViaLinq
    {
        public static ResultsForFeatures FindByFeatures(RequestedFeature[] featuresSpecified, string xmlContents)
        {
            var matches = new ResultsForFeatures(featuresSpecified); // Output


            int studentCount = 0;
            var xml = XDocument.Parse(xmlContents);
            var results = from student in xml.Descendants("Student")
                          where (string)student.Element("Id") != ""
                          select student;

            studentCount = results.Count();

            return matches;
        }
    }
}
