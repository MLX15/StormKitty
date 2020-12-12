using System.IO;

namespace StormKitty
{
    internal sealed class Logging
    {
        private static string logfile = Path.Combine(Path.GetTempPath(), "StormKitty-Latest.log");
        public static bool Log(string text, bool ret = true)
        {
            string newline = "\n";
            if (text.Length > 40 && text.Contains("\n"))
                newline += "\n\n";
            System.Console.Write(text + newline);
            if (Config.DebugMode == "1")
                File.AppendAllText(logfile, text + newline);
            return ret;
        }

        public static void Save(string sSavePath)
        {
            if (Config.DebugMode == "1" && File.Exists(logfile))
                try { File.Copy(logfile, sSavePath); } catch { };
        }
    }
}
