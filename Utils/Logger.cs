using System;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace DocuShareIndexingWorker.Utils
{
    public static class Logger
    {
        /**
        * @notice static variable
        */
        private static String _dateFormat = "yyyy-MM-dd";
        private static CultureInfo _cultureInfo = new CultureInfo("en-US");


        /**
        * @dev The function will be write error message to log file.
        */
        public static void Error(string message)
        {
            WriteLog("ERROR " + message);
        }


        /**
        * @dev The function will be write information message to log file
        */
        public static void Info(string message)
        {
            WriteLog(message);
        }


        /**
        * @dev The function will be logging to text file.
        */ 
        private static void WriteLog(string message)
        {
            try
            {
                string path = Path.Combine(getAssemblyDirectory(), "logs");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string filePath = Path.Combine(
                    path, 
                    string.Format("{0}_events.log", DateTime.Now.ToString(_dateFormat, _cultureInfo))
                );

                if (!File.Exists(filePath))
                {
                    onWriteText(filePath, message);
                }
                else
                {
                    onAppendText(filePath, message);
                }
            }
            catch
            {
            }
        }


        /**
        * @dev The function will create new file and write.
        */
        private static void onWriteText(string filePath, string message)
        {
            using (var sw = File.CreateText(filePath))
            {
                sw.WriteLine(string.Format("{0} {1}", 
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", 
                _cultureInfo), 
                message));
            }
        }


        /**
        * @dev The function will append line to text file.
        */
        private static void onAppendText(string filePath, string message)
        {
            using (var sw = File.AppendText(filePath))
            {
                sw.WriteLine(string.Format("{0} {1}", 
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", 
                _cultureInfo), 
                message));
            }
        }


        /**
        * @dev Return the current path.
        */
        public static string getAssemblyDirectory()
        {
                string codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);

                return Path.GetDirectoryName(path);
        }
    }
}
