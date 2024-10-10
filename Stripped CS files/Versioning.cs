using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Networking;

public class Versioning : VersioningBase
{
	public static Versioning fetch;

	public string titleShort;

	public string title;

	public int versionMajor;

	public int versionMinor;

	public int revision;

	public int experimental;

	public int buildID;

	public bool beta;

	public bool prerelease;

	public bool isReleaseBuild;

	public string versionString;

	[SerializeField]
	public List<string> distributionPlatformNames;

	public static string architecture = " x64";

	public static bool WinX64 = true;

	public string gameVersionUrl = "https://www.kerbalspaceprogram.com/files/KSP_LATEST_VERSION.vrs";

	public string patcherHashURL = "https://kerbalspaceprogram.com/kspstore/dp/patcher/index.php";

	public string patcherDLRoot = "https://kerbalspaceprogram.com/kspstore/dp/patcher/releases/{0}/{1}/Patcher";

	public string testUrl;

	public bool test;

	public static string Language = "";

	public static string distributionName = "";

	public int latestMajor;

	public int latestMinor;

	public int latestRev;

	public int latestExp;

	public bool latestBeta;

	public static bool promptNewVersion = false;

	public bool forcePrompt;

	public bool canUpdate;

	public bool isPatch;

	public string promptText = "";

	public GUISkin newVersionWindowSkin;

	public bool dontShowAgain;

	public bool doAnotherUpdate;

	public static int version_major => fetch.versionMajor;

	public static int version_minor => fetch.versionMinor;

	public static int Revision => fetch.revision;

	public static int Experimental => fetch.experimental;

	public static int BuildID => fetch.buildID;

	public static bool isBeta => fetch.beta;

	public static bool isPrerelease => fetch.prerelease;

	public static bool IsSteam => SteamManager.ValidSteamPlatform;

	public static string TitleShort => fetch.titleShort;

	public static string Title => fetch.title;

	public static bool IsReleaseBuild => fetch.isReleaseBuild;

	public static string VersionString => fetch.versionString;

	public static string DistributionName
	{
		get
		{
			if (fetch != null && fetch.distributionPlatformNames != null && fetch.distributionPlatformNames.Contains(distributionName))
			{
				return distributionName;
			}
			return "Unknown";
		}
	}

	public override string GetVersion()
	{
		return version_major + "." + version_minor + "." + Revision;
	}

	public static string GetVersionStringWithExperimental()
	{
		return version_major + "." + version_minor + "." + Revision + "." + BuildID + "x" + Experimental;
	}

	public static string GetVersionStringWithPrerelease()
	{
		return version_major + "." + version_minor + "." + Revision + "." + BuildID + "-pre";
	}

	public static string GetVersionStringFull()
	{
		string text = version_major + "." + version_minor + "." + Revision + "." + BuildID + " (" + Application.platform.ToString() + architecture + ")";
		if (Experimental > 0)
		{
			text = text + " x" + Experimental;
		}
		if (isBeta)
		{
			text += " BETA";
		}
		if (isPrerelease)
		{
			text += "-pre";
		}
		return text;
	}

	public static Version GetVersionFromString(string stringVersion)
	{
		return new Version(stringVersion);
	}

	public override void OnAwake()
	{
		fetch = this;
		string text = "buildID64.txt";
		isReleaseBuild = false;
		if (File.Exists(KSPUtil.ApplicationRootPath + "/" + text))
		{
			string[] array = File.ReadAllLines(KSPUtil.ApplicationRootPath + "/" + text);
			foreach (string text2 in array)
			{
				if (text2.Contains("prerelease"))
				{
					experimental = 0;
					prerelease = true;
					isReleaseBuild = false;
					UnityEngine.Debug.Log("Prerelease Build");
				}
				else if (text2.Contains("master"))
				{
					experimental = 0;
					isReleaseBuild = true;
					UnityEngine.Debug.Log("Release Build");
				}
				else if (text2.Contains("experimental"))
				{
					isReleaseBuild = false;
					experimental = 1;
					UnityEngine.Debug.Log("Experimental Build");
				}
				else if ((text2.Contains("build id") || text2.Contains("buildid")) && text2.Contains("="))
				{
					string[] array2 = text2.Split('=');
					if (array2.Length == 2)
					{
						buildID = int.Parse(array2[1].Trim());
					}
				}
				else if (text2.Contains("language") && text2.Contains("="))
				{
					string[] array3 = text2.Split('=');
					if (array3.Length == 2)
					{
						Language = array3[1].Trim();
					}
				}
				else if (text2.Contains("distribution name") && text2.Contains("="))
				{
					string[] array4 = text2.Split('=');
					if (array4.Length == 2)
					{
						distributionName = array4[1].Trim();
					}
				}
			}
		}
		versionString = GetVersionStringFull();
		UnityEngine.Debug.Log("[KSP Version]: " + versionString + " (x64) " + Language + " ==============================");
	}

	public void Start()
	{
		if (GameSettings.CHECK_FOR_UPDATES && !SteamManager.ValidSteamPlatform)
		{
			fetch.forcePrompt = false;
		}
	}

	public static void CheckForUpdates()
	{
		fetch.forcePrompt = true;
		fetch.StartCoroutine(fetch.checkForUpdate());
	}

	public void Update()
	{
		if (doAnotherUpdate)
		{
			doAnotherUpdate = false;
			CheckForUpdates();
		}
	}

	public IEnumerator checkForUpdate()
	{
		bool num = Application.platform.ToString().Contains("OSX");
		bool flag = Application.platform == RuntimePlatform.LinuxPlayer;
		string patcherPath = KSPUtil.ApplicationRootPath + "Patcher.exe";
		if (num || flag)
		{
			patcherPath = KSPUtil.ApplicationRootPath + "Patcher";
		}
		bool dlPatcher = !File.Exists(patcherPath);
		if (!dlPatcher)
		{
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("act", "patchermd5");
			wWWForm.AddField("experimental", experimental);
			wWWForm.AddField("major", versionMajor);
			wWWForm.AddField("minor", versionMinor);
			wWWForm.AddField("revision", revision);
			wWWForm.AddField("platID", GetOSCode());
			string text = patcherHashURL;
			PDebug.General("Checking patcher at " + text);
			UnityWebRequest patcherChecker = UnityWebRequest.Post(text, wWWForm);
			yield return patcherChecker.SendWebRequest();
			while (!patcherChecker.isDone)
			{
				yield return null;
			}
			if (patcherChecker.error != null)
			{
				PDebug.Error("Connecting to patcher updater: " + patcherChecker.error);
				yield break;
			}
			string text2 = patcherChecker.downloadHandler.text;
			string text3 = FileMD5String(patcherPath);
			if (text2.Contains("<"))
			{
				PDebug.Error("Server returned an error: " + text2);
			}
			else
			{
				PDebug.General("[Updater] Server reports that Patcher has MD5 of " + text2);
				PDebug.General("[Updater] Our own copy of " + patcherPath + " has MD5 of " + text3);
				if (!text2.Equals(text3))
				{
					PDebug.Warning("[Updater]  Well, they don't match.  Redownloading...");
					dlPatcher = true;
				}
			}
		}
		else
		{
			PDebug.Warning("[Updater] Patcher does NOT exist! Redownloading.");
			dlPatcher = true;
		}
		if (dlPatcher)
		{
			UpdatePatcher();
			yield break;
		}
		testUrl = KSPUtil.ApplicationRootPath + "KSP_LATEST_VERSION.vrs";
		if (File.Exists(testUrl))
		{
			test = true;
		}
		UnityWebRequest post = UnityWebRequest.Get(test ? (KSPUtil.ApplicationFileProtocol + testUrl) : gameVersionUrl);
		yield return post.SendWebRequest();
		while (!post.isDone)
		{
			yield return null;
		}
		if (post.error != null)
		{
			UnityEngine.Debug.Log("Error connecting to KSP home. Unable to fetch version info. \nmsg: " + post.error);
			yield break;
		}
		string[] array = post.downloadHandler.text.Split('\n');
		foreach (string text4 in array)
		{
			if (text4.Contains("="))
			{
				string text5 = text4.Split('=')[0].Trim();
				string text6 = text4.Split('=')[1].Trim();
				switch (text5)
				{
				case "ISPATCH":
					isPatch = bool.Parse(text6);
					break;
				case "EXP":
					latestExp = int.Parse(text6);
					break;
				case "BETA":
					latestBeta = bool.Parse(text6);
					break;
				case "REV":
					latestRev = int.Parse(text6);
					break;
				case "MINOR":
					latestMinor = int.Parse(text6);
					break;
				case "MAJOR":
					latestMajor = int.Parse(text6);
					break;
				}
			}
		}
		UnityEngine.Debug.Log("latest version from KSP site is: " + latestMajor + "." + latestMinor + "." + latestRev + ((latestExp != 0) ? ("x" + latestExp) : "") + (latestBeta ? " BETA" : ""));
		if (latestMajor > versionMajor)
		{
			promptText = "There is a major update to KSP available!";
		}
		else if (latestMajor == versionMajor && latestMinor > versionMinor)
		{
			promptText = "There is a new version out!";
		}
		else
		{
			if (latestMinor != versionMinor || latestRev <= revision)
			{
				promptText = "This is the latest version.";
				promptNewVersion = forcePrompt;
				canUpdate = false;
				yield break;
			}
			promptText = "There is a new update with bugfixes and improvements.";
		}
		promptNewVersion = true;
		canUpdate = true;
		InputLockManager.SetControlLock(ControlTypes.MAIN_MENU, "versioning");
	}

	public static byte[] FileMD5Bytes(string file)
	{
		return new MD5CryptoServiceProvider().ComputeHash(File.ReadAllBytes(file));
	}

	public static string FileMD5String(string file)
	{
		return BytesToHex(FileMD5Bytes(file));
	}

	public static string BytesToHex(byte[] b)
	{
		string text = string.Empty;
		for (int i = 0; i < b.Length; i++)
		{
			byte b2 = b[i];
			text = ((b2 >= 16) ? (text + b2.ToString("x")) : (text + "0" + b2.ToString("x")));
		}
		return text;
	}

	public void UpdatePatcher()
	{
		try
		{
			ServicePointManager.ServerCertificateValidationCallback = (object _003Cp0_003E, X509Certificate _003Cp1_003E, X509Chain _003Cp2_003E, SslPolicyErrors _003Cp3_003E) => true;
		}
		catch (Exception msg)
		{
			PDebug.Error(msg);
		}
		WebClient webClient = new WebClient();
		webClient.DownloadFileCompleted += PatcherDownloaded;
		string text = KSPUtil.ApplicationRootPath + "Patcher";
		string text2 = "";
		if (Application.platform.ToString().Contains("Windows"))
		{
			text2 = ".exe";
		}
		string text3 = string.Format(patcherDLRoot, GetUpdateChannel(), GetOSCode()) + text2;
		text += text2;
		PDebug.General(text3 + " -> " + text);
		webClient.DownloadFileAsync(new Uri(text3), text);
	}

	public string GetOSCode()
	{
		if (Application.platform.ToString().Contains("Windows"))
		{
			return "win";
		}
		if (Application.platform.ToString().Contains("OSX"))
		{
			return "osx";
		}
		return "linux";
	}

	public string GetUpdateChannel()
	{
		if (experimental != 0)
		{
			return "experimentals";
		}
		return "production";
	}

	public void PatcherDownloaded(object sender, AsyncCompletedEventArgs e)
	{
		if (e.Error != null)
		{
			PDebug.Error(e.Error);
		}
		else
		{
			PDebug.General("Downloaded fresh patcher!");
		}
		doAnotherUpdate = true;
	}

	public void drawNewVersionWindow(int id)
	{
		GUILayout.Label(promptText + " (" + latestMajor + "." + latestMinor + "." + latestRev + ((latestExp != 0) ? ("x" + latestExp) : "") + (latestBeta ? " BETA" : "") + ")");
		if (!isPatch)
		{
			GUILayout.Label("This update requires a full download. Press Update to open the KSP Store.");
		}
		GUILayout.BeginHorizontal();
		GUILayout.BeginVertical();
		GUI.enabled = canUpdate;
		if (GUILayout.Button("Update"))
		{
			UpdateGame();
		}
		GUI.enabled = true;
		GUILayout.EndVertical();
		GUILayout.BeginVertical();
		if (GUILayout.Button("Cancel"))
		{
			CancelUpdate();
		}
		dontShowAgain = GUILayout.Toggle(dontShowAgain, "Don't ask again");
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
	}

	public void CancelUpdate()
	{
		promptNewVersion = false;
		if (dontShowAgain)
		{
			GameSettings.CHECK_FOR_UPDATES = false;
			GameSettings.SaveSettings();
		}
		else
		{
			GameSettings.CHECK_FOR_UPDATES = true;
			GameSettings.SaveSettings();
		}
		InputLockManager.RemoveControlLock("versioning");
	}

	public void UpdateGame()
	{
		promptNewVersion = false;
		if (isPatch)
		{
			Screen.SetResolution(1280, 720, fullscreen: false);
			QuitAndStartPatcher();
		}
		else
		{
			Process.Start("https://www.kerbalspaceprogram.com/kspstore/");
			Application.Quit();
		}
	}

	public static void QuitAndStartPatcher()
	{
		try
		{
			Process process = new Process();
			if (!Application.platform.ToString().Contains("Windows"))
			{
				if (!Application.platform.ToString().Contains("OSX") && Application.platform != RuntimePlatform.LinuxPlayer)
				{
					process.StartInfo.FileName = "Patcher.exe";
				}
				else
				{
					process.StartInfo.FileName = "Patcher";
				}
			}
			else
			{
				process.StartInfo.FileName = "Patcher.exe";
			}
			process.Start();
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError("Could not QuitAndStartPatcher. The patch client could not be found or has no execution permission.\n\n" + ex);
		}
		Application.Quit();
	}
}
