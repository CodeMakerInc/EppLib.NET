// Copyright 2012 Code Maker Inc. (http://codemaker.net)
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
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
