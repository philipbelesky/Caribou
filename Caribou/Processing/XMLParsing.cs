namespace Caribou.Processing
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;

    public class XMLParsing
    {
        public static int ParserA(string xmlContents)
        {
            int studentCount = 0;
            using (XmlReader reader = XmlReader.Create(new StringReader(xmlContents)))
            {
                while (reader.Read())
                {
                    // Loop through the starting elements (e.g. the opening tags)
                    if (reader.IsStartElement())
                    {
                        // Do different things for different tags
                        switch (reader.Name)
                        {
                            // If on a student tag
                            case "Id":
                                if (reader.Read()) // Need to load the element
                                {
                                    if (!string.IsNullOrEmpty(reader.Value)) // Check it has something
                                    {
                                        studentCount++;
                                    }
                                }
                                break;
                        }
                    }
                }
            }
            return studentCount;
        }

        public static int ParserB(string xmlContents)
        {
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
            return studentCount;
        }

        public static int ParserC(string xmlContents)
        {
            int studentCount = 0;
            var xml = XDocument.Parse(xmlContents);
            var results = from student in xml.Descendants("Student")
                          where (string)student.Element("Id") != ""
                          select student;

            studentCount = results.Count();
            return studentCount;
        }
    }
}
