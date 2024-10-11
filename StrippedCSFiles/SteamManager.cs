using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
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

		[MethodImpl(MethodImplOptions.NoInlining)]
		public UGCQuerySet()
		{
			throw null;
		}
	}

	private static SteamManager s_instance;

	[SerializeField]
	private static bool s_EverInitialized;

	[SerializeField]
	private bool m_bInitialized;

	private SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;

	[SerializeField]
	private uint KSPSteamAppID;

	[SerializeField]
	private static AppId_t _appID;

	private static int steamCloudFileLimit;

	[SerializeField]
	private uint KSPMakingHistorySteamAppID;

	[SerializeField]
	private static AppId_t _MHappID;

	[SerializeField]
	private uint KSPBreakingGroundSteamAppID;

	[SerializeField]
	private static AppId_t _BGappID;

	[SerializeField]
	private static string _KSPSteamAppFolder;

	private static string kspSteamWorkshopFolder;

	[SerializeField]
	private bool _validSteamPlatform;

	[SerializeField]
	private string _friendName;

	public int CurrentUGCQueryTotalPages;

	public const string SteamWorkshopTOS_URL = "https://steamcommunity.com/sharedfiles/workshoplegalagreement";

	public AppId_t[] CurrentUGCUpdateAppDependencies;

	private DictionaryValueList<ulong, UGCQuerySet> _UGCQueryList;

	private DictionaryValueList<ulong, TextMeshProUGUI> _UGCPersonaList;

	private Steamworks.Callback<PersonaStateChange_t> m_PersonaStateChanged;

	private CallResult<CreateItemResult_t> m_NewItemCreated;

	private Callback<CreateItemResult_t, bool> KSPCreateItemCallback;

	private CallResult<SubmitItemUpdateResult_t> m_ItemUpdated;

	private Callback<SubmitItemUpdateResult_t, bool> KSPUpdateItemCallback;

	private CallResult<RemoteStorageSubscribePublishedFileResult_t> m_SubscribeFile;

	private Callback<RemoteStorageSubscribePublishedFileResult_t, bool> KSPSubscribeItemCallback;

	private CallResult<RemoteStorageUnsubscribePublishedFileResult_t> m_UnsubscribeFile;

	private Callback<RemoteStorageUnsubscribePublishedFileResult_t, bool> KSPUnsubscribeItemCallback;

	private CallResult<AddAppDependencyResult_t> m_AddAppDependency;

	private CallResult<DeleteItemResult_t> m_DeleteItem;

	private Callback<DeleteItemResult_t, bool> KSPDeleteItemCallback;

	private CallResult<RemoteStoragePublishedFileSubscribed_t> m_RemoteFileSubscribed;

	private Callback<RemoteStoragePublishedFileSubscribed_t, bool> KSPRemoteFileSubscribedCallback;

	private CallResult<RemoteStoragePublishedFileUnsubscribed_t> m_RemoteFileUnSubscribed;

	private Callback<RemoteStoragePublishedFileUnsubscribed_t, bool> KSPRemoteFileUnsubscribedCallback;

	private CallResult<GetAppDependenciesResult_t> m_GetAppDependencies;

	private Callback<GetAppDependenciesResult_t, bool> KSPGetAppDependenciesCallback;

	private CallResult<RemoveAppDependencyResult_t> m_RemoveAppDependency;

	private Callback<RemoveAppDependencyResult_t, bool> KSPRemoveAppDependencyCallback;

	public static SteamManager Instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static bool Initialized
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static AppId_t AppID
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static int SteamCloudFileLimit
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static AppId_t MakingHistoryAppID
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static AppId_t BreakingGroundAppID
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static string KSPSteamAppFolder
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static string KSPSteamWorkshopFolder
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static bool ValidSteamPlatform
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string FriendName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public DictionaryValueList<ulong, UGCQuerySet> UGCQueryList
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public DictionaryValueList<ulong, TextMeshProUGUI> UGCPersonaList
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SteamManager()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static SteamManager()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupCallbacks()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupRemoteCallbacks()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RemoveRemoteCallbacks()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPersonaStateChanged(PersonaStateChange_t state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnRemoteFileSubscribed(RemoteStoragePublishedFileSubscribed_t subscribedResult, bool bIOFailure)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnRemoteFileUnsubscribed(RemoteStoragePublishedFileUnsubscribed_t unsubscribedResult, bool bIOFailure)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSubscribeFile(RemoteStorageSubscribePublishedFileResult_t subscribeResult, bool bIOFailure)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUnSubscribeFile(RemoteStorageUnsubscribePublishedFileResult_t subscribeResult, bool bIOFailure)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUGCQuery(SteamUGCQueryCompleted_t queryCallback, bool bIOFailure)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnNewItemCreated(CreateItemResult_t queryCallback, bool bIOFailure)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnItemUpdated(SubmitItemUpdateResult_t queryCallback, bool bIOFailure)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnItemAppDependenciesUpdated(AddAppDependencyResult_t queryCallback, bool bIOFailure)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnItemDeleted(DeleteItemResult_t deleteResult, bool bIOFailure)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGetAppDependencies(GetAppDependenciesResult_t result, bool bIOFailure)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnRemoveAppDependency(RemoveAppDependencyResult_t result, bool bIOFailure)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal int GetItemState(PublishedFileId_t fileID, out string stateText, out bool canBeUsed, out bool subscribed)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void RegisterRemoteSubUnsubEvents(Callback<RemoteStoragePublishedFileSubscribed_t, bool> remoteFileSubscribedCallback, Callback<RemoteStoragePublishedFileUnsubscribed_t, bool> remoteFileUnsubscribedCallback)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void RemoveRemoteSubUnsubEvents()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool CheckCloudQuota(string folderPath, out ulong totalRequired, out ulong availAmount, out int totalFilesRequired, out int availFileCount)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool subscribeItem(PublishedFileId_t fileId, Callback<RemoteStorageSubscribePublishedFileResult_t, bool> callBack)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool unsubscribeItem(PublishedFileId_t fileId, Callback<RemoteStorageUnsubscribePublishedFileResult_t, bool> callBack)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool deleteItem(PublishedFileId_t fileId, Callback<DeleteItemResult_t, bool> callBack)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal string getUserName(ulong steamId, TextMeshProUGUI textToUpdate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string getFriendName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool QueryUGCItems(Callback<SteamUGCQueryCompleted_t, bool> onCompleted, EUGCQuery eugcQuery, EUGCMatchingUGCType matchingType, string[] tags, uint page, bool returnMetadata, bool matchAllTags = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool QueryUserUGCItems(Callback<SteamUGCQueryCompleted_t, bool> onCompleted, EUserUGCList listType, EUGCMatchingUGCType matchingType, EUserUGCListSortOrder sortOrder, string[] tags, uint page)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool GetUGCItem(Callback<SteamUGCQueryCompleted_t, bool> onCompleted, PublishedFileId_t fileId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal SteamUGCDetails_t GetUGCItemDetails(UGCQueryHandle_t queryHandle, uint itemIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool GetAppDependencies(Callback<GetAppDependenciesResult_t, bool> onCompleted, PublishedFileId_t fileId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool RemoveAppDependency(Callback<RemoveAppDependencyResult_t, bool> onCompleted, PublishedFileId_t fileId, AppId_t appID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool CreateNewItem(Callback<CreateItemResult_t, bool> onCompleted, EWorkshopFileType fileType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool UpdateItem(Callback<SubmitItemUpdateResult_t, bool> onCompleted, PublishedFileId_t fileId, List<string> tags, string contentURL, string previewURL, string title, string description, string changeNote, ERemoteStoragePublishedFileVisibility visibility, string metaData, AppId_t[] appDependencies = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string checkWorkshopItemPreviewSize(string contentURL, string previewURL, ulong fileId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool AddAppDependency(Callback<AddAppDependencyResult_t, bool> onCompleted, PublishedFileId_t fileId, AppId_t appDependency)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal uint GetNumberSubscribedItems()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal PublishedFileId_t[] GetSubscribedItems(uint numSubscribedItems)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OpenSteamOverlayToWorkshopItem(PublishedFileId_t fileId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal string GetUGCFailureReason(EResult resultCode)
	{
		throw null;
	}
}
