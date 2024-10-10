using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Expansions.Missions;
using ns9;
using UnityEngine;
using UnityEngine.Networking;

namespace Expansions;

public class ExpansionsLoader : LoadingSystem
{
	public class ExpansionInfo
	{
		public string MasterBundleFilePath { get; set; }

		public ExpansionSO PersistentObject { get; set; }

		public bool Installed { get; set; }

		public string Name => PersistentObject.name;

		public string DisplayName => PersistentObject.DisplayName;

		public string FolderName => PersistentObject.FolderName;

		public string Version => PersistentObject.Version;

		public ProtoCrewMember.KerbalSuit[] KerbalSuits => PersistentObject.KerbalSuits;

		public string MasterBundleFileName => Path.GetFileName(MasterBundleFilePath);

		public string MasterBundleDirectory => Path.GetDirectoryName(MasterBundleFilePath);

		public ExpansionInfo(string masterBundleFilePath, ExpansionSO scriptableObject, bool isInstalled)
		{
			MasterBundleFilePath = masterBundleFilePath;
			PersistentObject = scriptableObject;
			Installed = isInstalled;
		}
	}

	[Serializable]
	public struct SupportedExpansion
	{
		public string expansionName;

		public string minimumVersion;

		public string maximumVersion;
	}

	public static readonly string expansionsMasterExtension = ".kspexpansion";

	public static readonly string expansionsPublicKey = "PFJTQUtleVZhbHVlPjxNb2R1bHVzPnM0VEkrWkk4S0pTMnI0eW5La2Z3ZUJOZ1V4enZabmpXQy85OEQ3bldtdEYrcStqYUFGK3NTdzB1MkhZbHFxQUZMK09kcEdUN2xMUzhDNzhtOGllSzFGK1gyUFhsbTYyVzBYUVJ4TERNRlJVODhVd3NKNFgyNk1DWVRXUDF5aU5yWXZEMXhiVzhlcjFNbStmNDMwMGhRb2JMMjlJRVlVT2d5dDZ6c2RNU0xyZHV2QWMzZFR4RElXZHpsV2NERElyWlJwM01wL3pTL1VoT0diR3FsZ1JjZU1aZHhIU0VpNHFBazhoL0tkdVpjVDRtNFJUaFNpMXgxaXRmVXEzVCtFRHRIVGdWMys0VFFya3dJZmROVUhTVENGb3NaTWpjZWttTEpHajhmdUpUbHRjTWZrckVTMFgweXEyOHk2WG5XUUE1dU43T2FUQmZWdHhWSCtndDRMN2duUT09PC9Nb2R1bHVzPjxFeHBvbmVudD5FUT09PC9FeHBvbmVudD48L1JTQUtleVZhbHVlPg==";

	public static Dictionary<string, ExpansionInfo> expansionsInfo = new Dictionary<string, ExpansionInfo>();

	public SupportedExpansion[] supportedExpansions = new SupportedExpansion[0];

	public List<string> expansionsThatFailedToLoad = new List<string>();

	public float progressDelta;

	public bool isReady;

	public string progressTitle = "";

	public float progressFraction;

	public static ExpansionsLoader Instance { get; set; }

	public void Awake()
	{
		if (Instance != null)
		{
			Debug.Log("ExpansionsLoader instance already exists, destroying!");
			UnityEngine.Object.Destroy(this);
		}
		Instance = this;
	}

	public void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public override bool IsReady()
	{
		return isReady;
	}

	public override string ProgressTitle()
	{
		return progressTitle;
	}

	public override float ProgressFraction()
	{
		return progressFraction;
	}

	public override void StartLoad()
	{
		StartCoroutine(LoadExpansions());
	}

	public IEnumerator LoadExpansions()
	{
		progressTitle = Localizer.Format("#autoLOC_8003147");
		progressFraction = 0f;
		float startTime = Time.realtimeSinceStartup;
		if (Directory.Exists(KSPExpansionsUtils.ExpansionsGameDataPath))
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(KSPExpansionsUtils.ExpansionsGameDataPath);
			FileInfo[] expansionFiles = directoryInfo.GetFiles("*" + expansionsMasterExtension, SearchOption.AllDirectories);
			progressDelta = 1f / (float)expansionFiles.Length;
			for (int i = 0; i < expansionFiles.Length; i++)
			{
				progressFraction += progressDelta;
				yield return StartCoroutine(InitializeExpansion(expansionFiles[i].FullName));
			}
		}
		else
		{
			progressFraction += 1f;
		}
		if (expansionsInfo.Count < 1)
		{
			Debug.Log("No Expansions detected");
		}
		else
		{
			ReportInstalledExpansions();
		}
		MissionsUtils.InitialiseAdjusterTypes();
		progressTitle = Localizer.Format("#autoLOC_8003148");
		progressFraction = 1f;
		yield return null;
		isReady = true;
		GameEvents.OnExpansionSystemLoaded.Fire();
		Debug.Log("ExpansionsLoader: Expansions loaded in " + (Time.realtimeSinceStartup - startTime).ToString("F3") + "s");
	}

	public IEnumerator InitializeExpansion(string expansionFile)
	{
		if (!InitPublicKeyCryptoProvider(out var verifier))
		{
			Debug.LogError("Unable to configure CryptoSigner.\nBreaking from Expansion Initializer for " + expansionFile);
			yield break;
		}
		progressTitle = Localizer.Format("#autoLOC_8003149", Path.GetFileNameWithoutExtension(expansionFile));
		byte[] hashBytes = null;
		AssetBundle expansionSOBundle = null;
		using (UnityWebRequest webRequest = new UnityWebRequest(KSPUtil.ApplicationFileProtocol + expansionFile))
		{
			webRequest.downloadHandler = new DownloadHandlerBuffer();
			yield return webRequest.SendWebRequest();
			while (!webRequest.isDone)
			{
				yield return null;
			}
			if (!webRequest.isNetworkError && !webRequest.isHttpError)
			{
				hashBytes = new MD5CryptoServiceProvider().ComputeHash(webRequest.downloadHandler.data);
				expansionSOBundle = AssetBundle.LoadFromMemory(webRequest.downloadHandler.data);
			}
		}
		if (expansionSOBundle == null)
		{
			string text = Localizer.Format("#autoLOC_8004231", expansionFile);
			Debug.LogError(text + "\nBreaking from Expansion Initializer!");
			expansionsThatFailedToLoad.Add(text);
			yield break;
		}
		string[] allAssetNames = expansionSOBundle.GetAllAssetNames();
		if (allAssetNames != null && allAssetNames.Length != 1)
		{
			string text2 = Localizer.Format("#autoLOC_8004232", expansionFile, allAssetNames.Length);
			Debug.LogError(text2 + "\nBreaking from Expansion Initializer!");
			expansionsThatFailedToLoad.Add(text2);
			yield break;
		}
		AssetBundleRequest requestSO = expansionSOBundle.LoadAssetAsync(allAssetNames[0], typeof(ExpansionSO));
		yield return requestSO;
		ExpansionSO masterBundleSO = requestSO.asset as ExpansionSO;
		if (masterBundleSO == null)
		{
			string text3 = Localizer.Format("#autoLOC_8004233", expansionFile);
			Debug.LogError(text3 + "\nBreaking from Expansion Initializer!");
			expansionsThatFailedToLoad.Add(text3);
			yield break;
		}
		progressTitle = Localizer.Format("#autoLOC_8003150", masterBundleSO.DisplayName);
		string signature = File.ReadAllText(Path.GetDirectoryName(expansionFile) + "/signature");
		if (!VerifyHashSignature(verifier, hashBytes, signature))
		{
			string text4 = Localizer.Format("#autoLOC_8004234", masterBundleSO.DisplayName);
			Debug.LogError(text4 + "\nBreaking from Expansion Initializer!");
			expansionsThatFailedToLoad.Add(text4);
			yield break;
		}
		bool flag = false;
		int num = 0;
		while (!flag && num < supportedExpansions.Length)
		{
			if (supportedExpansions[num].expansionName == masterBundleSO.DisplayName)
			{
				Version version = new Version(supportedExpansions[num].minimumVersion);
				Version version2 = new Version(supportedExpansions[num].maximumVersion);
				Version version3 = new Version(masterBundleSO.Version);
				if (version3 < version || version3 > version2)
				{
					string text5 = Localizer.Format("#autoLOC_8004235", masterBundleSO.DisplayName, version3.ToString(), version.ToString(), version2.ToString());
					Debug.LogError(text5 + "\nBreaking from Expansion Initializer!");
					expansionsThatFailedToLoad.Add(text5);
					yield break;
				}
				flag = true;
			}
			num++;
		}
		if (!flag)
		{
			string text6 = Localizer.Format("#autoLOC_8004236", masterBundleSO.DisplayName);
			Debug.LogError(text6 + "\nBreaking from Expansion Initializer!");
			expansionsThatFailedToLoad.Add(text6);
			yield break;
		}
		Version version4 = new Version(masterBundleSO.KSPVersion);
		Version version5 = new Version(VersioningBase.GetVersionString());
		if (version5 < version4)
		{
			string text7 = Localizer.Format("#autoLOC_8004237", masterBundleSO.DisplayName, version4.ToString(), version5.ToString());
			Debug.LogError(text7 + "\nBreaking from Expansion Initializer!");
			expansionsThatFailedToLoad.Add(text7);
			yield break;
		}
		bool allBundlesVerified = true;
		for (int index = 0; index < masterBundleSO.Bundles.Count; index++)
		{
			progressTitle = Localizer.Format("#autoLOC_8003151", masterBundleSO.DisplayName, index + 1, masterBundleSO.Bundles.Count);
			yield return BundleLoader.LoadAssetBundle(masterBundleSO.Bundles[index].name, KSPUtil.ApplicationFileProtocol + KSPExpansionsUtils.ExpansionsGameDataPath + masterBundleSO.FolderName + "/AssetBundles/");
			if (!VerifyHashSignature(hashBytes: BundleLoader.loadedBundles[masterBundleSO.Bundles[index].name].hash, verifier: verifier, signature: masterBundleSO.Bundles[index].fileSignature))
			{
				Debug.LogError("Expansion Bundle [" + masterBundleSO.Bundles[index].name + "] not able to be verified");
				allBundlesVerified = false;
			}
		}
		if (!allBundlesVerified)
		{
			string text8 = Localizer.Format("#autoLOC_8004238", masterBundleSO.DisplayName);
			Debug.LogError(text8 + "\nBreaking from Expansion Initializer!");
			expansionsThatFailedToLoad.Add(text8);
		}
		else
		{
			ExpansionInfo value = new ExpansionInfo(expansionFile, masterBundleSO, isInstalled: true);
			progressTitle = Localizer.Format("#autoLOC_8003150", masterBundleSO.DisplayName) + " SquadExpansion/" + masterBundleSO.FolderName;
			expansionsInfo.Add(masterBundleSO.FolderName, value);
		}
	}

	public bool InitPublicKeyCryptoProvider(out RSACryptoServiceProvider verifier)
	{
		verifier = null;
		try
		{
			RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
			string @string = Encoding.ASCII.GetString(Convert.FromBase64String(expansionsPublicKey));
			rSACryptoServiceProvider.FromXmlString(@string);
			verifier = rSACryptoServiceProvider;
			return true;
		}
		catch (Exception ex)
		{
			Debug.LogError("Error instantiating the verifier\n" + ex.Message);
			return false;
		}
	}

	public bool VerifyHashSignature(RSACryptoServiceProvider verifier, byte[] hashBytes, string signature)
	{
		byte[] signature2 = Convert.FromBase64String(signature);
		return verifier.VerifyData(hashBytes, new MD5CryptoServiceProvider(), signature2);
	}

	public bool VerifyHashSignature(byte[] hashBytes, string signature)
	{
		if (!InitPublicKeyCryptoProvider(out var verifier))
		{
			Debug.LogError("Unable to configure CryptoSigner.");
			return false;
		}
		if (VerifyHashSignature(verifier, hashBytes, signature))
		{
			return true;
		}
		return false;
	}

	public void ReportInstalledExpansions()
	{
		Dictionary<string, ExpansionInfo>.Enumerator enumerator = expansionsInfo.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				ExpansionInfo value = enumerator.Current.Value;
				if (value.Installed)
				{
					Debug.Log("Expansion " + value.Name + " detected in path " + value.MasterBundleDirectory);
				}
			}
		}
		finally
		{
			enumerator.Dispose();
		}
	}

	public static bool IsExpansionInstalled(string name)
	{
		if (expansionsInfo.TryGetValue(name, out var value))
		{
			return value.Installed;
		}
		return false;
	}

	public static bool IsExpansionInstalled(string name, byte[] hashBytes, string signature)
	{
		if (expansionsInfo.TryGetValue(name, out var value))
		{
			return value.Installed;
		}
		if (name == "MakingHistory" && hashBytes != null && !string.IsNullOrEmpty(signature))
		{
			return Instance.VerifyHashSignature(hashBytes, signature);
		}
		return false;
	}

	public static bool IsExpansionAnyKerbalSuitInstalled()
	{
		Dictionary<string, ExpansionInfo>.KeyCollection.Enumerator enumerator = expansionsInfo.Keys.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				ExpansionInfo expansionInfo = expansionsInfo[enumerator.Current];
				if (expansionInfo.Installed && expansionInfo.KerbalSuits.Length != 0)
				{
					return true;
				}
			}
		}
		finally
		{
			enumerator.Dispose();
		}
		return false;
	}

	public static bool IsExpansionKerbalSuitInstalled(ProtoCrewMember.KerbalSuit suit)
	{
		switch (suit)
		{
		case ProtoCrewMember.KerbalSuit.Default:
			return true;
		case ProtoCrewMember.KerbalSuit.Slim:
			return true;
		default:
		{
			Dictionary<string, ExpansionInfo>.KeyCollection.Enumerator enumerator = expansionsInfo.Keys.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					ExpansionInfo expansionInfo = expansionsInfo[enumerator.Current];
					if (!expansionInfo.Installed || expansionInfo.KerbalSuits.Length == 0)
					{
						continue;
					}
					ProtoCrewMember.KerbalSuit[] kerbalSuits = expansionInfo.KerbalSuits;
					for (int i = 0; i < kerbalSuits.Length; i++)
					{
						if (kerbalSuits[i] == suit)
						{
							return true;
						}
					}
				}
			}
			finally
			{
				enumerator.Dispose();
			}
			return false;
		}
		}
	}

	public static string GetExpansionDirectory(string expansionName)
	{
		if (expansionsInfo.TryGetValue(expansionName, out var value))
		{
			return value.MasterBundleDirectory;
		}
		return null;
	}

	public static bool IsInstalled(List<string> expansions)
	{
		int num = 0;
		while (true)
		{
			if (num < expansions.Count)
			{
				if (!expansionsInfo.TryGetValue(expansions[num], out var _))
				{
					break;
				}
				num++;
				continue;
			}
			return true;
		}
		return false;
	}

	public static bool IsAnyExpansionInstalled()
	{
		bool result = false;
		if (expansionsInfo.Count > 0)
		{
			result = true;
		}
		return result;
	}

	public static List<ExpansionInfo> GetInstalledExpansions()
	{
		List<ExpansionInfo> list = new List<ExpansionInfo>();
		Dictionary<string, ExpansionInfo>.KeyCollection.Enumerator enumerator = expansionsInfo.Keys.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				ExpansionInfo expansionInfo = expansionsInfo[enumerator.Current];
				if (expansionInfo.Installed)
				{
					list.Add(expansionInfo);
				}
			}
			return list;
		}
		finally
		{
			enumerator.Dispose();
		}
	}

	public static string GetInstalledExpansionsString(string separator = "\n", bool sepAtStart = true)
	{
		List<ExpansionInfo> installedExpansions = GetInstalledExpansions();
		string text = "";
		for (int i = 0; i < installedExpansions.Count; i++)
		{
			if (installedExpansions[i].Installed)
			{
				if (text != "" || sepAtStart)
				{
					text += separator;
				}
				text = text + installedExpansions[i].PersistentObject.Version + " " + installedExpansions[i].DisplayName + " ";
			}
		}
		return text;
	}

	public static string GetExpansionVersion(string expansionName)
	{
		if (expansionsInfo.TryGetValue(expansionName, out var value))
		{
			return value.Version;
		}
		return "";
	}

	public List<string> GetExpansionsThatFailedToLoad()
	{
		return Instance.expansionsThatFailedToLoad;
	}

	public static byte[] BuildMissionFileHash(string filePath, string fileName)
	{
		ConfigNode configNode = FindMissionNode(ConfigNode.Load(filePath + fileName));
		if (configNode == null)
		{
			Debug.LogError("Unable to parse File " + filePath + fileName);
			return null;
		}
		return BuildMissionFileHash(configNode);
	}

	public static byte[] GetMissionVerificationStrings(ConfigNode node, out string signature)
	{
		signature = "";
		ConfigNode configNode = FindMissionNode(node);
		if (configNode == null)
		{
			Debug.LogError("Unable to find Mission Validation information.");
			return null;
		}
		configNode.TryGetValue("signature", ref signature);
		return BuildMissionFileHash(configNode);
	}

	public static ConfigNode FindMissionNode(ConfigNode node)
	{
		ConfigNode node2 = null;
		if (!node.TryGetNode("MISSION", ref node2))
		{
			if (node.name == "GAME")
			{
				node2 = node;
			}
			else
			{
				node.TryGetNode("GAME", ref node2);
			}
			if (node2 == null)
			{
				return null;
			}
			try
			{
				node2 = ((!node2.HasNode("MISSIONTOSTART")) ? node.GetNode("SCENARIO", "name", "MissionSystem").GetNode("MISSIONS").GetNode("MISSION") : node2.GetNode("MISSIONTOSTART"));
			}
			catch
			{
				return null;
			}
		}
		return node2;
	}

	public static byte[] BuildMissionFileHash(ConfigNode missionNode)
	{
		List<string> list = new List<string>();
		list.Add(missionNode.GetValue("id"));
		list.Add(missionNode.GetValue("expansionVersion"));
		list.Add(missionNode.GetValue("seed"));
		list.Add(missionNode.GetValue("briefing"));
		ConfigNode node = new ConfigNode();
		if (missionNode.TryGetNode("SITUATION", ref node))
		{
			list.Add(node.GetValue("rocMissionSeed"));
			list.Add(node.GetValue("resourceSeed"));
		}
		ConfigNode node2 = new ConfigNode();
		if (missionNode.TryGetNode("NODES", ref node2))
		{
			ConfigNode[] nodes = node2.GetNodes("NODE");
			for (int i = 0; i < nodes.Length; i++)
			{
				list.Add(nodes[i].GetValue("title"));
				list.Add(nodes[i].GetValue("id"));
				ConfigNode node3 = new ConfigNode();
				if (nodes[i].TryGetNode("TONODEIDS", ref node3))
				{
					string[] values = node3.GetValues("nodeID");
					for (int j = 0; j < values.Length; j++)
					{
						list.Add(values[j]);
					}
				}
			}
		}
		string s = string.Join("$", list);
		byte[] bytes = Encoding.UTF8.GetBytes(s);
		return new MD5CryptoServiceProvider().ComputeHash(bytes);
	}
}
