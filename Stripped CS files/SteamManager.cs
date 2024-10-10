using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ns9;
using Steamworks;
using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
public class SteamManager : MonoBehaviour
{
	public class UGCQuerySet
	{
		public CallResult<SteamUGCQueryCompleted_t> UGCQuery;

		public UGCQueryHandle_t Handle;

		public Callback<SteamUGCQueryCompleted_t, bool> QueryCallback;
	}

	public static SteamManager s_instance;

	[SerializeField]
	public static bool s_EverInitialized = false;

	[SerializeField]
	public bool m_bInitialized;

	public SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;

	[SerializeField]
	public uint KSPSteamAppID;

	[SerializeField]
	public static AppId_t _appID = AppId_t.Invalid;

	public static int steamCloudFileLimit = 10000;

	[SerializeField]
	public uint KSPMakingHistorySteamAppID;

	[SerializeField]
	public static AppId_t _MHappID = AppId_t.Invalid;

	[SerializeField]
	public uint KSPBreakingGroundSteamAppID;

	[SerializeField]
	public static AppId_t _BGappID = AppId_t.Invalid;

	[SerializeField]
	public static string _KSPSteamAppFolder = null;

	public static string kspSteamWorkshopFolder = string.Empty;

	[SerializeField]
	public bool _validSteamPlatform;

	[SerializeField]
	public string _friendName = "";

	public int CurrentUGCQueryTotalPages;

	public const string SteamWorkshopTOS_URL = "https://steamcommunity.com/sharedfiles/workshoplegalagreement";

	public AppId_t[] CurrentUGCUpdateAppDependencies;

	public DictionaryValueList<ulong, UGCQuerySet> _UGCQueryList;

	public DictionaryValueList<ulong, TextMeshProUGUI> _UGCPersonaList;

	public Steamworks.Callback<PersonaStateChange_t> m_PersonaStateChanged;

	public CallResult<CreateItemResult_t> m_NewItemCreated;

	public Callback<CreateItemResult_t, bool> KSPCreateItemCallback;

	public CallResult<SubmitItemUpdateResult_t> m_ItemUpdated;

	public Callback<SubmitItemUpdateResult_t, bool> KSPUpdateItemCallback;

	public CallResult<RemoteStorageSubscribePublishedFileResult_t> m_SubscribeFile;

	public Callback<RemoteStorageSubscribePublishedFileResult_t, bool> KSPSubscribeItemCallback;

	public CallResult<RemoteStorageUnsubscribePublishedFileResult_t> m_UnsubscribeFile;

	public Callback<RemoteStorageUnsubscribePublishedFileResult_t, bool> KSPUnsubscribeItemCallback;

	public CallResult<AddAppDependencyResult_t> m_AddAppDependency;

	public CallResult<DeleteItemResult_t> m_DeleteItem;

	public Callback<DeleteItemResult_t, bool> KSPDeleteItemCallback;

	public CallResult<RemoteStoragePublishedFileSubscribed_t> m_RemoteFileSubscribed;

	public Callback<RemoteStoragePublishedFileSubscribed_t, bool> KSPRemoteFileSubscribedCallback;

	public CallResult<RemoteStoragePublishedFileUnsubscribed_t> m_RemoteFileUnSubscribed;

	public Callback<RemoteStoragePublishedFileUnsubscribed_t, bool> KSPRemoteFileUnsubscribedCallback;

	public CallResult<GetAppDependenciesResult_t> m_GetAppDependencies;

	public Callback<GetAppDependenciesResult_t, bool> KSPGetAppDependenciesCallback;

	public CallResult<RemoveAppDependencyResult_t> m_RemoveAppDependency;

	public Callback<RemoveAppDependencyResult_t, bool> KSPRemoveAppDependencyCallback;

	public static SteamManager Instance => s_instance;

	public static bool Initialized
	{
		get
		{
			if ((bool)Instance)
			{
				return Instance.m_bInitialized;
			}
			return false;
		}
	}

	public static AppId_t AppID => _appID;

	public static int SteamCloudFileLimit => steamCloudFileLimit;

	public static AppId_t MakingHistoryAppID => _MHappID;

	public static AppId_t BreakingGroundAppID => _BGappID;

	public static string KSPSteamAppFolder => _KSPSteamAppFolder;

	public static string KSPSteamWorkshopFolder
	{
		get
		{
			if (kspSteamWorkshopFolder == string.Empty)
			{
				kspSteamWorkshopFolder = Path.GetFullPath(KSPSteamAppFolder + "/../../workshop/content/" + AppID.m_AppId + "/");
			}
			return kspSteamWorkshopFolder;
		}
	}

	public static bool ValidSteamPlatform
	{
		get
		{
			if ((bool)Instance)
			{
				return Instance._validSteamPlatform;
			}
			return false;
		}
	}

	public string FriendName => _friendName;

	public DictionaryValueList<ulong, UGCQuerySet> UGCQueryList => _UGCQueryList;

	public DictionaryValueList<ulong, TextMeshProUGUI> UGCPersonaList => _UGCPersonaList;

	public static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
	{
		Debug.LogWarning(pchDebugText);
	}

	public void Awake()
	{
		_UGCQueryList = new DictionaryValueList<ulong, UGCQuerySet>();
		_UGCPersonaList = new DictionaryValueList<ulong, TextMeshProUGUI>();
		if (s_instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		s_instance = this;
		bool flag = true;
		_appID = new AppId_t(KSPSteamAppID);
		_MHappID = new AppId_t(KSPMakingHistorySteamAppID);
		_BGappID = new AppId_t(KSPBreakingGroundSteamAppID);
		if (!_validSteamPlatform)
		{
			_validSteamPlatform = File.Exists(KSPUtil.ApplicationRootPath + "/../../appmanifest_220200.acf");
			if (!_validSteamPlatform)
			{
				flag = false;
			}
		}
		if (s_EverInitialized)
		{
			throw new Exception("[SteamManager]: Tried to Initialize the SteamAPI twice in one session!");
		}
		if (_validSteamPlatform)
		{
			if (!Packsize.Test())
			{
				Debug.LogError("[SteamManager]: Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
				flag = false;
			}
			if (!DllCheck.Test())
			{
				Debug.LogError("[SteamManager]: DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", this);
				flag = false;
			}
			try
			{
				if (SteamAPI.RestartAppIfNecessary(AppId_t.Invalid))
				{
					Application.Quit();
					return;
				}
			}
			catch (DllNotFoundException ex)
			{
				Debug.LogError("[SteamManager]: Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + ex, this);
				flag = false;
			}
			if (flag)
			{
				m_bInitialized = SteamAPI.Init();
				if (!m_bInitialized)
				{
					Debug.LogError("[SteamManager]: SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", this);
					flag = false;
				}
				SteamApps.GetAppInstallDir(AppID, out _KSPSteamAppFolder, 5242880u);
			}
		}
		s_EverInitialized = true;
		if (!flag)
		{
			_validSteamPlatform = false;
		}
		else
		{
			_friendName = getFriendName();
		}
	}

	public void OnEnable()
	{
		if (s_instance == null)
		{
			s_instance = this;
		}
		if (m_bInitialized)
		{
			if (m_SteamAPIWarningMessageHook == null)
			{
				m_SteamAPIWarningMessageHook = SteamAPIDebugTextHook;
				SteamClient.SetWarningMessageHook(m_SteamAPIWarningMessageHook);
			}
			SetupCallbacks();
		}
	}

	public void OnDestroy()
	{
		if (!(s_instance != this))
		{
			s_instance = null;
			if (m_bInitialized)
			{
				SteamAPI.Shutdown();
			}
		}
	}

	public void Update()
	{
		if (m_bInitialized)
		{
			SteamAPI.RunCallbacks();
		}
	}

	public void SetupCallbacks()
	{
		if (Initialized)
		{
			m_PersonaStateChanged = Steamworks.Callback<PersonaStateChange_t>.Create(OnPersonaStateChanged);
			m_NewItemCreated = CallResult<CreateItemResult_t>.Create(OnNewItemCreated);
			m_ItemUpdated = CallResult<SubmitItemUpdateResult_t>.Create(OnItemUpdated);
			m_SubscribeFile = CallResult<RemoteStorageSubscribePublishedFileResult_t>.Create(OnSubscribeFile);
			m_UnsubscribeFile = CallResult<RemoteStorageUnsubscribePublishedFileResult_t>.Create(OnUnSubscribeFile);
			m_AddAppDependency = CallResult<AddAppDependencyResult_t>.Create(OnItemAppDependenciesUpdated);
			m_DeleteItem = CallResult<DeleteItemResult_t>.Create(OnItemDeleted);
			m_GetAppDependencies = CallResult<GetAppDependenciesResult_t>.Create(OnGetAppDependencies);
			m_RemoveAppDependency = CallResult<RemoveAppDependencyResult_t>.Create(OnRemoveAppDependency);
		}
	}

	public void SetupRemoteCallbacks()
	{
		m_RemoteFileSubscribed = CallResult<RemoteStoragePublishedFileSubscribed_t>.Create(OnRemoteFileSubscribed);
		m_RemoteFileUnSubscribed = CallResult<RemoteStoragePublishedFileUnsubscribed_t>.Create(OnRemoteFileUnsubscribed);
	}

	public void RemoveRemoteCallbacks()
	{
		if (m_RemoteFileSubscribed != null && m_RemoteFileSubscribed.IsActive())
		{
			m_RemoteFileSubscribed.Cancel();
		}
		if (m_RemoteFileUnSubscribed != null && m_RemoteFileUnSubscribed.IsActive())
		{
			m_RemoteFileUnSubscribed.Cancel();
		}
	}

	public void OnPersonaStateChanged(PersonaStateChange_t state)
	{
		if (_UGCPersonaList.ContainsKey(state.m_ulSteamID) && _UGCPersonaList[state.m_ulSteamID].text != null)
		{
			_UGCPersonaList[state.m_ulSteamID].text = SteamFriends.GetFriendPersonaName(new CSteamID(state.m_ulSteamID));
			_UGCPersonaList.Remove(state.m_ulSteamID);
		}
	}

	public void OnRemoteFileSubscribed(RemoteStoragePublishedFileSubscribed_t subscribedResult, bool bIOFailure)
	{
		if (subscribedResult.m_nAppID == AppID && KSPRemoteFileSubscribedCallback != null)
		{
			KSPRemoteFileSubscribedCallback(subscribedResult, bIOFailure);
		}
	}

	public void OnRemoteFileUnsubscribed(RemoteStoragePublishedFileUnsubscribed_t unsubscribedResult, bool bIOFailure)
	{
		if (unsubscribedResult.m_nAppID == AppID && KSPRemoteFileUnsubscribedCallback != null)
		{
			KSPRemoteFileUnsubscribedCallback(unsubscribedResult, bIOFailure);
		}
	}

	public void OnSubscribeFile(RemoteStorageSubscribePublishedFileResult_t subscribeResult, bool bIOFailure)
	{
		if (!bIOFailure && subscribeResult.m_eResult == EResult.k_EResultOK)
		{
			Debug.LogFormat("[SteamManager]: Subscribe to steam file ({0}) Results: {1}", subscribeResult.m_nPublishedFileId, subscribeResult.m_eResult);
		}
		else
		{
			Debug.LogFormat("[SteamManager]: There was an error Subscribing to steam file ({0})", subscribeResult.m_eResult.ToString());
		}
		if (KSPSubscribeItemCallback != null)
		{
			KSPSubscribeItemCallback(subscribeResult, bIOFailure);
			KSPSubscribeItemCallback = null;
		}
	}

	public void OnUnSubscribeFile(RemoteStorageUnsubscribePublishedFileResult_t subscribeResult, bool bIOFailure)
	{
		if (!bIOFailure && subscribeResult.m_eResult == EResult.k_EResultOK)
		{
			Debug.LogFormat("[SteamManager]: UnSubscribe from steam file ({0}) Results: {1}", subscribeResult.m_nPublishedFileId, subscribeResult.m_eResult);
		}
		else
		{
			Debug.LogFormat("[SteamManager]: There was an error unsubscribing from steam file ({0})", subscribeResult.m_eResult.ToString());
		}
		if (KSPUnsubscribeItemCallback != null)
		{
			KSPUnsubscribeItemCallback(subscribeResult, bIOFailure);
			KSPUnsubscribeItemCallback = null;
		}
	}

	public void OnUGCQuery(SteamUGCQueryCompleted_t queryCallback, bool bIOFailure)
	{
		if (!bIOFailure && queryCallback.m_eResult == EResult.k_EResultOK)
		{
			CurrentUGCQueryTotalPages = (int)(queryCallback.m_unTotalMatchingResults - 1) / 50 + 1;
			Debug.Log("[SteamManager]: Query ITEMS - Total results: " + queryCallback.m_unTotalMatchingResults + " Number results: " + queryCallback.m_unNumResultsReturned + " Total Pages: " + CurrentUGCQueryTotalPages);
		}
		else
		{
			Debug.LogFormat("[SteamManager]: There was an error retrieving the UGC - {0}", queryCallback.m_eResult.ToString());
			CurrentUGCQueryTotalPages = 0;
		}
		if (_UGCQueryList.ContainsKey((ulong)queryCallback.m_handle))
		{
			UGCQuerySet uGCQuerySet = _UGCQueryList[(ulong)queryCallback.m_handle];
			if (uGCQuerySet != null && uGCQuerySet.QueryCallback != null)
			{
				uGCQuerySet.QueryCallback(queryCallback, bIOFailure);
				_UGCQueryList.Remove((ulong)queryCallback.m_handle);
				return;
			}
			_UGCQueryList.Remove((ulong)queryCallback.m_handle);
		}
		SteamUGC.ReleaseQueryUGCRequest(queryCallback.m_handle);
	}

	public void OnNewItemCreated(CreateItemResult_t queryCallback, bool bIOFailure)
	{
		if (!bIOFailure && queryCallback.m_eResult == EResult.k_EResultOK)
		{
			Debug.Log(string.Concat("[SteamManager]: New Item Created. Steam Legal Agreement Accepted: ", (!queryCallback.m_bUserNeedsToAcceptWorkshopLegalAgreement).ToString(), " Result: ", queryCallback.m_eResult, " id: ", queryCallback.m_nPublishedFileId));
		}
		else
		{
			Debug.LogFormat("[SteamManager]: There was an error creating new Item FileId({0}) - {1}", queryCallback.m_nPublishedFileId, queryCallback.m_eResult.ToString());
			if (queryCallback.m_nPublishedFileId != PublishedFileId_t.Invalid)
			{
				deleteItem(queryCallback.m_nPublishedFileId, null);
			}
		}
		if (KSPCreateItemCallback != null)
		{
			KSPCreateItemCallback(queryCallback, bIOFailure);
			KSPCreateItemCallback = null;
		}
	}

	public void OnItemUpdated(SubmitItemUpdateResult_t queryCallback, bool bIOFailure)
	{
		if (!bIOFailure && queryCallback.m_eResult == EResult.k_EResultOK)
		{
			Debug.Log(string.Concat("[SteamManager]: Item Updated. Steam Legal Agreement Accepted: ", queryCallback.m_bUserNeedsToAcceptWorkshopLegalAgreement.ToString(), " Result: ", queryCallback.m_eResult, " id: ", queryCallback.m_nPublishedFileId));
			if (CurrentUGCUpdateAppDependencies != null)
			{
				for (int i = 0; i < CurrentUGCUpdateAppDependencies.Length; i++)
				{
					AddAppDependency(null, queryCallback.m_nPublishedFileId, CurrentUGCUpdateAppDependencies[i]);
				}
			}
		}
		else
		{
			Debug.LogFormat("[SteamManager]: There was an error updating the item FileId({0}) - {1}", queryCallback.m_nPublishedFileId, queryCallback.m_eResult.ToString());
		}
		if (KSPUpdateItemCallback != null)
		{
			KSPUpdateItemCallback(queryCallback, bIOFailure);
			KSPUpdateItemCallback = null;
		}
	}

	public void OnItemAppDependenciesUpdated(AddAppDependencyResult_t queryCallback, bool bIOFailure)
	{
		if (!bIOFailure && queryCallback.m_eResult == EResult.k_EResultOK)
		{
			Debug.LogFormat("[SteamManager]: Workshop Item App Dependencies Updated for item FileId({0})", queryCallback.m_nPublishedFileId);
		}
		else
		{
			Debug.LogFormat("[SteamManager]: There was an error updating App dependencies for item FileId({0}) - {1}", queryCallback.m_nPublishedFileId, queryCallback.m_eResult.ToString());
		}
	}

	public void OnItemDeleted(DeleteItemResult_t deleteResult, bool bIOFailure)
	{
		if (!bIOFailure && deleteResult.m_eResult == EResult.k_EResultOK)
		{
			Debug.LogFormat("[SteamManager]: Delete steam file ({0}) Results: {1}", deleteResult.m_nPublishedFileId, deleteResult.m_eResult);
		}
		else
		{
			Debug.LogFormat("[SteamManager]: Delete steam file ({0} Failed. Results: {1})", deleteResult.m_nPublishedFileId, deleteResult.m_eResult);
		}
		if (KSPDeleteItemCallback != null)
		{
			KSPDeleteItemCallback(deleteResult, bIOFailure);
			KSPDeleteItemCallback = null;
		}
	}

	public void OnGetAppDependencies(GetAppDependenciesResult_t result, bool bIOFailure)
	{
		if (!bIOFailure && result.m_eResult == EResult.k_EResultOK)
		{
			Debug.LogFormat("[SteamManager]: Get App Dependencies steam file ({0}) Results: {1}", result.m_nPublishedFileId, result.m_eResult);
		}
		else
		{
			Debug.LogFormat("[SteamManager]: Get App Dependencies steam file ({0} Failed. Results: {1})", result.m_nPublishedFileId, result.m_eResult);
		}
		if (KSPGetAppDependenciesCallback != null)
		{
			KSPGetAppDependenciesCallback(result, bIOFailure);
			KSPGetAppDependenciesCallback = null;
		}
	}

	public void OnRemoveAppDependency(RemoveAppDependencyResult_t result, bool bIOFailure)
	{
		if (!bIOFailure && result.m_eResult == EResult.k_EResultOK)
		{
			Debug.LogFormat("[SteamManager]: Remove App Dependency from steam file ({0}) Results: {1}", result.m_nPublishedFileId, result.m_eResult);
		}
		else
		{
			Debug.LogFormat("[SteamManager]: Remove App Dependency from steam file ({0} Failed. Results: {1})", result.m_nPublishedFileId, result.m_eResult);
		}
		if (KSPRemoveAppDependencyCallback != null)
		{
			KSPRemoveAppDependencyCallback(result, bIOFailure);
			KSPRemoveAppDependencyCallback = null;
		}
	}

	public int GetItemState(PublishedFileId_t fileID, out string stateText, out bool canBeUsed, out bool subscribed)
	{
		int itemState = (int)SteamUGC.GetItemState(fileID);
		stateText = "";
		canBeUsed = true;
		subscribed = false;
		if (((uint)itemState & (true ? 1u : 0u)) != 0)
		{
			subscribed = true;
		}
		if (((uint)itemState & 0x10u) != 0)
		{
			stateText = Localizer.Format("#autoLOC_8002163");
			canBeUsed = false;
		}
		else if (((uint)itemState & 4u) != 0 && ((uint)itemState & 8u) != 0)
		{
			stateText = Localizer.Format("#autoLOC_8002164");
		}
		else if (((uint)itemState & 0x20u) != 0)
		{
			stateText = Localizer.Format("#autoLOC_8002165");
			canBeUsed = false;
		}
		else if (((uint)itemState & 4u) != 0)
		{
			stateText = Localizer.Format("#autoLOC_8002166");
		}
		else if (((uint)itemState & (true ? 1u : 0u)) != 0)
		{
			stateText = Localizer.Format("#autoLOC_8002167");
			canBeUsed = false;
		}
		return itemState;
	}

	public void RegisterRemoteSubUnsubEvents(Callback<RemoteStoragePublishedFileSubscribed_t, bool> remoteFileSubscribedCallback, Callback<RemoteStoragePublishedFileUnsubscribed_t, bool> remoteFileUnsubscribedCallback)
	{
		SetupRemoteCallbacks();
		KSPRemoteFileSubscribedCallback = remoteFileSubscribedCallback;
		KSPRemoteFileUnsubscribedCallback = remoteFileUnsubscribedCallback;
	}

	public void RemoveRemoteSubUnsubEvents()
	{
		RemoveRemoteCallbacks();
		KSPRemoteFileSubscribedCallback = null;
		KSPRemoteFileUnsubscribedCallback = null;
	}

	public bool CheckCloudQuota(string folderPath, out ulong totalRequired, out ulong availAmount, out int totalFilesRequired, out int availFileCount)
	{
		ulong pnTotalBytes = 0uL;
		totalFilesRequired = 0;
		SteamRemoteStorage.GetQuota(out pnTotalBytes, out availAmount);
		availFileCount = steamCloudFileLimit - SteamRemoteStorage.GetFileCount();
		float num = KSPUtil.CalculateFolderSize(folderPath, out totalFilesRequired);
		totalRequired = (ulong)num;
		Debug.LogFormat("[SteamManager]: Cloud space required: {0} Cloud Space Available: {1}", totalRequired, availAmount);
		Debug.LogFormat("[SteamManager]: Total files to upload: {0} Cloud File Total Available: {1}", totalFilesRequired, availFileCount);
		if (totalRequired > availAmount)
		{
			return false;
		}
		if (totalFilesRequired > availFileCount)
		{
			return false;
		}
		return true;
	}

	public bool subscribeItem(PublishedFileId_t fileId, Callback<RemoteStorageSubscribePublishedFileResult_t, bool> callBack)
	{
		KSPSubscribeItemCallback = callBack;
		SteamAPICall_t hAPICall = SteamUGC.SubscribeItem(fileId);
		m_SubscribeFile.Set(hAPICall);
		return true;
	}

	public bool unsubscribeItem(PublishedFileId_t fileId, Callback<RemoteStorageUnsubscribePublishedFileResult_t, bool> callBack)
	{
		KSPUnsubscribeItemCallback = callBack;
		SteamAPICall_t hAPICall = SteamUGC.UnsubscribeItem(fileId);
		m_UnsubscribeFile.Set(hAPICall);
		return true;
	}

	public bool deleteItem(PublishedFileId_t fileId, Callback<DeleteItemResult_t, bool> callBack)
	{
		KSPDeleteItemCallback = callBack;
		SteamAPICall_t hAPICall = SteamUGC.DeleteItem(fileId);
		m_DeleteItem.Set(hAPICall);
		return true;
	}

	public string getUserName(ulong steamId, TextMeshProUGUI textToUpdate)
	{
		CSteamID cSteamID = new CSteamID(steamId);
		if (SteamFriends.RequestUserInformation(cSteamID, bRequireNameOnly: true) && textToUpdate != null)
		{
			_UGCPersonaList.Add(steamId, textToUpdate);
		}
		return SteamFriends.GetFriendPersonaName(cSteamID);
	}

	public string getFriendName()
	{
		if (Initialized)
		{
			string personaName = SteamFriends.GetPersonaName();
			Debug.Log("[SteamManager]: Running under steam name: " + personaName);
			return personaName;
		}
		return "";
	}

	public bool QueryUGCItems(Callback<SteamUGCQueryCompleted_t, bool> onCompleted, EUGCQuery eugcQuery, EUGCMatchingUGCType matchingType, string[] tags, uint page, bool returnMetadata, bool matchAllTags = true)
	{
		UGCQuerySet uGCQuerySet = new UGCQuerySet();
		uGCQuerySet.QueryCallback = onCompleted;
		uGCQuerySet.Handle = SteamUGC.CreateQueryAllUGCRequest(eugcQuery, matchingType, AppID, AppID, page);
		bool flag = true;
		if (tags.Length != 0)
		{
			SteamUGC.SetMatchAnyTag(uGCQuerySet.Handle, !matchAllTags);
		}
		for (int i = 0; i < tags.Length; i++)
		{
			if (!SteamUGC.AddRequiredTag(uGCQuerySet.Handle, tags[i]))
			{
				flag = false;
				Debug.LogFormat("[SteamManager]: Steam Missions query failed to add Tag {0}", tags[i]);
				break;
			}
		}
		if (flag)
		{
			SteamUGC.SetReturnMetadata(uGCQuerySet.Handle, returnMetadata);
			SteamAPICall_t hAPICall = SteamUGC.SendQueryUGCRequest(uGCQuerySet.Handle);
			uGCQuerySet.UGCQuery = CallResult<SteamUGCQueryCompleted_t>.Create(OnUGCQuery);
			_UGCQueryList.Add((ulong)uGCQuerySet.Handle, uGCQuerySet);
			uGCQuerySet.UGCQuery.Set(hAPICall);
			Debug.Log("[SteamManager]: call " + hAPICall.m_SteamAPICall);
			return true;
		}
		return false;
	}

	public bool QueryUserUGCItems(Callback<SteamUGCQueryCompleted_t, bool> onCompleted, EUserUGCList listType, EUGCMatchingUGCType matchingType, EUserUGCListSortOrder sortOrder, string[] tags, uint page)
	{
		UGCQuerySet uGCQuerySet = new UGCQuerySet();
		uGCQuerySet.QueryCallback = onCompleted;
		uGCQuerySet.Handle = SteamUGC.CreateQueryUserUGCRequest(SteamUser.GetSteamID().GetAccountID(), listType, matchingType, sortOrder, AppID, AppID, page);
		bool flag = true;
		for (int i = 0; i < tags.Length; i++)
		{
			if (!SteamUGC.AddRequiredTag(uGCQuerySet.Handle, tags[i]))
			{
				flag = false;
				Debug.LogFormat("[SteamManager]: Steam Missions query failed to add Tag {0}", tags[i]);
				break;
			}
		}
		if (flag)
		{
			if (tags.Length != 0)
			{
				SteamUGC.SetMatchAnyTag(uGCQuerySet.Handle, bMatchAnyTag: true);
			}
			SteamAPICall_t hAPICall = SteamUGC.SendQueryUGCRequest(uGCQuerySet.Handle);
			uGCQuerySet.UGCQuery = CallResult<SteamUGCQueryCompleted_t>.Create(OnUGCQuery);
			_UGCQueryList.Add((ulong)uGCQuerySet.Handle, uGCQuerySet);
			uGCQuerySet.UGCQuery.Set(hAPICall);
			Debug.Log("[SteamManager]: call " + hAPICall.m_SteamAPICall);
			return true;
		}
		return false;
	}

	public bool GetUGCItem(Callback<SteamUGCQueryCompleted_t, bool> onCompleted, PublishedFileId_t fileId)
	{
		UGCQuerySet uGCQuerySet = new UGCQuerySet();
		uGCQuerySet.QueryCallback = onCompleted;
		uGCQuerySet.Handle = SteamUGC.CreateQueryUGCDetailsRequest(new PublishedFileId_t[1] { fileId }, 1u);
		SteamAPICall_t hAPICall = SteamUGC.SendQueryUGCRequest(uGCQuerySet.Handle);
		uGCQuerySet.UGCQuery = CallResult<SteamUGCQueryCompleted_t>.Create(OnUGCQuery);
		_UGCQueryList.Add((ulong)uGCQuerySet.Handle, uGCQuerySet);
		uGCQuerySet.UGCQuery.Set(hAPICall);
		return true;
	}

	public SteamUGCDetails_t GetUGCItemDetails(UGCQueryHandle_t queryHandle, uint itemIndex)
	{
		SteamUGC.GetQueryUGCResult(queryHandle, itemIndex, out var pDetails);
		return pDetails;
	}

	public bool GetAppDependencies(Callback<GetAppDependenciesResult_t, bool> onCompleted, PublishedFileId_t fileId)
	{
		KSPGetAppDependenciesCallback = onCompleted;
		SteamAPICall_t appDependencies = SteamUGC.GetAppDependencies(fileId);
		m_GetAppDependencies.Set(appDependencies);
		return true;
	}

	public bool RemoveAppDependency(Callback<RemoveAppDependencyResult_t, bool> onCompleted, PublishedFileId_t fileId, AppId_t appID)
	{
		KSPRemoveAppDependencyCallback = onCompleted;
		SteamAPICall_t hAPICall = SteamUGC.RemoveAppDependency(fileId, appID);
		m_RemoveAppDependency.Set(hAPICall);
		return true;
	}

	public bool CreateNewItem(Callback<CreateItemResult_t, bool> onCompleted, EWorkshopFileType fileType)
	{
		KSPCreateItemCallback = onCompleted;
		SteamAPICall_t hAPICall = SteamUGC.CreateItem(AppID, fileType);
		m_NewItemCreated.Set(hAPICall);
		return true;
	}

	public bool UpdateItem(Callback<SubmitItemUpdateResult_t, bool> onCompleted, PublishedFileId_t fileId, List<string> tags, string contentURL, string previewURL, string title, string description, string changeNote, ERemoteStoragePublishedFileVisibility visibility, string metaData, AppId_t[] appDependencies = null)
	{
		KSPUpdateItemCallback = onCompleted;
		UGCUpdateHandle_t uGCUpdateHandle_t = SteamUGC.StartItemUpdate(AppID, fileId);
		SteamUGC.SetItemContent(uGCUpdateHandle_t, contentURL);
		SteamUGC.SetItemDescription(uGCUpdateHandle_t, description);
		previewURL = checkWorkshopItemPreviewSize(contentURL, previewURL, fileId.m_PublishedFileId);
		SteamUGC.SetItemPreview(uGCUpdateHandle_t, previewURL);
		SteamUGC.SetItemTags(uGCUpdateHandle_t, tags);
		SteamUGC.SetItemTitle(uGCUpdateHandle_t, title);
		SteamUGC.SetItemVisibility(uGCUpdateHandle_t, visibility);
		SteamUGC.SetItemMetadata(uGCUpdateHandle_t, metaData);
		CurrentUGCUpdateAppDependencies = appDependencies;
		SteamAPICall_t hAPICall = SteamUGC.SubmitItemUpdate(uGCUpdateHandle_t, changeNote);
		m_ItemUpdated.Set(hAPICall);
		return true;
	}

	public string checkWorkshopItemPreviewSize(string contentURL, string previewURL, ulong fileId)
	{
		FileInfo[] files = new DirectoryInfo(contentURL).GetFiles("steamPreview*.png");
		for (int i = 0; i < files.Length; i++)
		{
			File.Delete(files[i].FullName);
		}
		if (File.Exists(previewURL))
		{
			FileInfo fileInfo = new FileInfo(previewURL);
			while (fileInfo.Length > 1048576L)
			{
				Texture2D texture2D = null;
				byte[] data = File.ReadAllBytes(previewURL);
				texture2D = new Texture2D(2, 2);
				texture2D.LoadImage(data);
				TextureScale.Bilinear(texture2D, texture2D.width / 2, texture2D.height / 2);
				byte[] bytes = texture2D.EncodeToPNG();
				previewURL = contentURL + "steamPreview" + fileId + ".png";
				File.WriteAllBytes(previewURL, bytes);
				fileInfo = new FileInfo(previewURL);
			}
		}
		return previewURL;
	}

	public bool AddAppDependency(Callback<AddAppDependencyResult_t, bool> onCompleted, PublishedFileId_t fileId, AppId_t appDependency)
	{
		SteamAPICall_t hAPICall = SteamUGC.AddAppDependency(fileId, appDependency);
		m_AddAppDependency.Set(hAPICall);
		return true;
	}

	public uint GetNumberSubscribedItems()
	{
		return SteamUGC.GetNumSubscribedItems();
	}

	public PublishedFileId_t[] GetSubscribedItems(uint numSubscribedItems)
	{
		PublishedFileId_t[] array = new PublishedFileId_t[numSubscribedItems];
		SteamUGC.GetSubscribedItems(array, numSubscribedItems);
		return array;
	}

	public void OpenSteamOverlayToWorkshopItem(PublishedFileId_t fileId)
	{
		string text = "steam://url/CommunityFilePage/" + fileId.ToString();
		Debug.Log("[SteamManager]: Opening Steam Game Overlay for Item: " + text);
		SteamFriends.ActivateGameOverlayToWebPage(text);
	}

	public string GetUGCFailureReason(EResult resultCode)
	{
		return resultCode.ToString();
	}
}
