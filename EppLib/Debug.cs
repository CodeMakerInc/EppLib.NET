using System.Globalization;
using System.IO;
using System.Text;

namespace EppLib
{
    public static class Debug
    {
        public static string LogFilename = "easyepplog.txt";

        public static void Log(byte[] bytes)
        {
            LogMessageToFile(Encoding.Default.GetString(bytes));
        }

        public static void Log(string str)
        {
            Log(Encoding.Default.GetBytes(str));
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
