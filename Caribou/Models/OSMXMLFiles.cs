namespace Caribou.Data
{
    using System.Collections.Generic;
    using Caribou.Components;

    /// <summary>
    /// A wrapper around a list of strings representing the XML files that are provided.
    /// </summary>
    public struct OSMXMLFiles
    {
        public List<string> ProvidedXMLs;

        public OSMXMLFiles(List<string> ghInput, ref MessagesWrapper messages)
        {
            // When provided with a list of strings it can either be a request to parse multiple files, OR it
            // could be a single file that is being read per-line. To guess, we check the length of the first item
            // which will either be a whole-file, or a single line
            if (ghInput.Count > 1 && ghInput[0].Length < 100)
            {
                // To account for files being provided in per-line mode we just concat them and re-add into a new list
                this.ProvidedXMLs = new List<string>()
                {
                    string.Join("", ghInput)
                };
                messages.AddRemark(
                    "OSM file content was provided as a list of per-line strings. \n" +
                    "When using the Read File component you should turn OFF Per-File parsing via the right-click menu. \n" +
                    "If you are trying to read multiple OSM files at once, please use separate components to do so.");
            }
            else
            {
                this.ProvidedXMLs = ghInput;
            }
        }
    }
}
