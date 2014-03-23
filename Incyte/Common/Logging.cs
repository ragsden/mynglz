using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Configuration;

namespace Incyte
{
    public static class Logging
    {
        static bool shudDebug = false;
        static string cs = "INCYTE";
        static string debugLocation = string.Empty;
        
        static Logging()
        {
            if (ConfigurationManager.AppSettings["Debug"].Equals("true", StringComparison.InvariantCultureIgnoreCase))
                shudDebug = true;

            debugLocation = ConfigurationManager.AppSettings["DebugLocation"];
            EventLog elog = new EventLog();

            if (!EventLog.SourceExists(cs))
                EventLog.CreateEventSource(cs, "Application");
        }
        public static void WriteToEventLog(string message)
        {
            EventLog.WriteEntry(cs, message, EventLogEntryType.Error);
        }

        public static void WriteToFileLog(string message)
        {
            if (!shudDebug)
                return;

            using (TextWriter wr = File.AppendText(debugLocation))
            {
                wr.WriteLine(message);
                wr.Close();
            }

        }

    }
}
