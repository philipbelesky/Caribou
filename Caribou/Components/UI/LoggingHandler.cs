namespace Caribou.Components
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public class LoggingHandler
    {
        public List<string> debugLogs; // Debug parameter output
        protected Stopwatch debugTimer;
        public int indexOfDebugOutput; // Tracking where to output logs

        public void Reset()
        {
            debugTimer = Stopwatch.StartNew(); // Setup timer used for debugging
            debugLogs = new List<string>(); // The debugging log that would be output
        }

        public void NoteTiming(string eventDescription)
        {
            var logInfo = eventDescription + ": ";
            debugTimer.Stop();
            debugLogs.Add(logInfo.PadRight(28, ' ') + debugTimer.ElapsedMilliseconds.ToString() + " ms");
            debugTimer.Restart();
        }

        public void NoteGeneral(string eventDescription)
        {
            debugLogs.Add(eventDescription);
        }
    }
}
