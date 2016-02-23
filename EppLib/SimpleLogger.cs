using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Globalization;

namespace EppLib
{
    class SimpleLogger : IDebugger
    {
        public static string LogFilename = "easyepplog.txt";

        public void Log(byte[] bytes)
        {
            LogMessageToFile(Encoding.UTF8.GetString(bytes));
        }

        public void Log(string str)
        {
            LogMessageToFile(str);
        }

        private static void LogMessageToFile(string msg)
        {
            var sw = File.AppendText(LogFilename);

            try
            {
                var logLine = System.String.Format( CultureInfo.InvariantCulture,"{0:G}: {1}.", System.DateTime.Now, msg);

                sw.WriteLine(logLine);
            }
            finally
            {
                sw.Close();
            }
        }
    }
}
