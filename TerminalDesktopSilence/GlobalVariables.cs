using RasheedTag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalDesktopSilence
{
    public static class GlobalVariables
    {
        //  global variables here
        public static int Version = 18;
        public static string defaultOutFolderPath = "Output";
        public static string defaultExe = "Tools\\TerminalDesktop.exe";
        static public int TmpRFFailedCounter = 0;
        public static string configFilePath = "";



         
        public static void LogInFile(string LogStr)
        {
            if (string.IsNullOrWhiteSpace(LogStr))
            {
                return;
            }

            try
            {
                // Append the log entry to the file
                string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} :TerminalVer{Version.ToString()}: {LogStr}{Environment.NewLine}";
                string currentDate = DateTime.Now.ToString("yyyyMMdd");
                string logFilePath = Configuration.XMLFolder + "\\LogFile" + currentDate + ".txt";

                // Ensure the directory exists
                string directory = Path.GetDirectoryName(logFilePath) ?? "";
                if (Directory.Exists(directory))
                {
                    // Open the file with FileStream and allow multiple processes to write simultaneously
                    using (FileStream fs = new FileStream(logFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.Write(logEntry);
                        // Console.WriteLine(logEntry);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Logging failed: {ex.Message}");

            }
        }
    }
}
