namespace Caribou.Processing
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Caribou.Models;

    /// <summary>
    /// Utilities for determining the initial line counts and during-processing progress for % reporting
    /// </summary>
    public static class ProgressReporting
    {        
        public static void Ping(int currentLineNumber, int currentFileIndex, RequestHandler request)
        {
            var progress = GetProgressAcrossLines(currentLineNumber, currentFileIndex, request.LinesPerFile);
            request.ReportProgress(request.WorkerId, progress);
        }

        public static double GetProgressAcrossLines(int currentLine, int currentFileIndex, List<int> totalLines)
        {
            var count = 0.0;
            for (var i = 0; i < totalLines.Count; i++)
            {
                if (currentFileIndex > i)
                    count += totalLines[i];
                else if (currentFileIndex == i)
                    count += currentLine;
            }
            var allLinesFromAllFiles = totalLines.Sum();
            return count / (double)allLinesFromAllFiles;
        }

        public static List<int> GetLineLengthsForFiles(List<string> xmlFilePaths, OSMGeometryType typeToFind, bool readPathAsContents = false)
        {
            if (readPathAsContents)
                return Enumerable.Repeat(1000, xmlFilePaths.Count).ToList(); // With tests we pass strings, not paths, so just return arbitrary values

            var count = new List<int>();
            foreach (var filePath in xmlFilePaths)
            {
                int linesForFile = File.ReadLines(filePath).Count();
                if (typeToFind == OSMGeometryType.Node)
                    linesForFile = Convert.ToInt32(linesForFile * 0.55); // Only half of the amount of a file is nodes

                count.Add(linesForFile);
            }
            return count;
        }

    }
}
