using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KawaSquad
{
    namespace Network
    {
        public class Debug
        {
            private static string filePath;
            private static string folderPath;
            private static bool isInit = false;
            private static FileStream logStream;
            private static string todayFormat;

            private enum LogType
            {
                LOG, WARNING, ERROR
            }

            private static void Inititialize()
            {
                folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                DateTime today = DateTime.Now;
                todayFormat = today.Day.ToString("D2") + "-" + today.Month.ToString("D2") + "-" + today.Year.ToString("D4");
                string fileName = "ServerLog_" + todayFormat;
                filePath = Path.Combine(folderPath, fileName + ".log");

                if (!File.Exists(filePath))
                {
                    logStream = File.Create(filePath);
                    logStream.Close();
                }
                isInit = true;
            }

            public static void Log(string log, bool consoleWrite = false)
            {
                if (!isInit)
                    Inititialize();

                WriteLog(log, LogType.LOG);
                if (consoleWrite)
                    Console.WriteLine(log);
            }
            public static void LogError(string error, bool consoleWrite = false)
            {
                if (!isInit)
                    Inititialize();

                WriteLog(error, LogType.ERROR);
                if (consoleWrite)
                    Console.WriteLine(error);
            }
            public static void LogWarning(string warning, bool consoleWrite = false)
            {
                if (!isInit)
                    Inititialize();

                WriteLog(warning, LogType.WARNING);
                if (consoleWrite)
                    Console.WriteLine(warning);
            }


            private static void WriteLog(string log, LogType logType)
            {
                DateTime now = DateTime.Now;

                // to recreate if the server exist today and write log tomorrow.
                string nowFormat = now.Day.ToString("D2") + "-" + now.Month.ToString("D2") + "-" + now.Year.ToString("D4");
                if (nowFormat != todayFormat)
                    Inititialize();

                string time = now.Hour.ToString("D2") + ":" + now.Minute.ToString("D2") + ":" + now.Second.ToString("D2");
                string logEncoded = string.Empty;
                switch (logType)
                {
                    case LogType.LOG:
                        logEncoded = "Log - " + time + " : " + log + "\n";
                        break;
                    case LogType.WARNING:
                        logEncoded = "Warning - " + time + " : " + log + "\n";
                        break;
                    case LogType.ERROR:
                        logEncoded = "Error - " + time + " : " + log + "\n";
                        break;
                    default:
                        logEncoded = "Default - " + time + " : " + log + "\n";
                        break;
                }

                File.AppendAllText(filePath, logEncoded);
            }
        }
    }
}