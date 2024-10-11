using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using Expansions;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class VesselSpawnDialog : MonoBehaviour
{
	public delegate void CraftSelectedCallback(string fullFilename, string flagURL, VesselCrewManifest manifest);

	private enum ActiveToggle
	{
		TabSteam,
		TabVessels
	}

	private enum SteamQueryFilters
	{
		VOTE,
		FEATURED,
		NEWEST,
		SUBSCRIBERS
	}

	internal struct ListItem
	{
		public string name;

		public float mass;

		public float cost;

		public float size;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ListItem(string name, float mass, float cost, float size)
		{
			throw null;
		}
	}

	internal class VesselDataItem
	{
		public bool stock;

		public string name;

		public string description;

		public static string defaultDescription;

		public string fullFilePath;

		private ConfigNode _configNode;

		public bool steamItem;

		public int parts;

		public int stages;

		public VersionCompareResult compatibility;

		public UIListItem listItem;

		public bool isValid;

		public bool isExperimental;

		private ShipTemplate _template;

		public string thumbURL;

		public Texture2D thumbnail;

		public CraftProfileInfo craftProfileInfo;

		public ConfigNode configNode
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public ShipTemplate template
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public VesselDataItem(FileInfo fInfo, bool stock, bool steamItem)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		static VesselDataItem()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ulong GetSteamFileId()
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CLoadSteamItemPreviewURL_003Ed__104 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public CraftEntry selectedEntry;

		public VesselSpawnDialog _003C_003E4__this;

		private UnityWebRequest _003Cwww_003E5__2;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CLoadSteamItemPreviewURL_003Ed__104(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void _003C_003Em__Finally1()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CCreateVesselList_003Ed__114 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public VesselSpawnDialog _003C_003E4__this;

		public string craftSubfolder;

		public string profileName;

		private bool _003CfinalVesselListSort_003E5__2;

		private FileInfo[] _003CfileInfoList_003E5__3;

		private int _003CfileInfoListCount_003E5__4;

		private int _003Ci_003E5__5;

		private List<ExpansionsLoader.ExpansionInfo> _003CinstalledExps_003E5__6;

		private FileInfo[] _003CexpansionCraftList_003E5__7;

		private int _003Ci_003E5__8;

		private List<FileInfo> _003CfileList_003E5__9;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CCreateVesselList_003Ed__114(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CLateSelectFireThing_003Ed__116 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public VesselSpawnDialog _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CLateSelectFireThing_003Ed__116(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	private VesselSpawnSearch vesselSpawnSearch;

	public UIListSorter vesselListSorter;

	[SerializeField]
	private UIListItem listItemPrefab;

	public RectTransform showHodeTransform;

	public Transform vesselListAnchor;

	public ToggleGroup vesselListToggleGroup;

	[SerializeField]
	private FlagBrowserButton flagButtonController;

	[SerializeField]
	private RectTransform crewMgmtArea;

	[SerializeField]
	private RectTransform panelRightInfo;

	[SerializeField]
	private RectTransform panelRightSteamInfo;

	[SerializeField]
	private RectTransform tabVesselList;

	[SerializeField]
	private RectTransform tabSteam;

	[SerializeField]
	private RectTransform listSorting;

	[SerializeField]
	private RectTransform tabSteamToggles;

	[SerializeField]
	private RawImage shipThumbnail;

	[SerializeField]
	private Button buttonDelete;

	[SerializeField]
	private Button buttonLaunch;

	[SerializeField]
	private Button buttonEdit;

	[SerializeField]
	private Button buttonClose;

	[SerializeField]
	private GameObject launchSiteSelector;

	[SerializeField]
	private GameObject vesselListPanelFullTab;

	[SerializeField]
	private TextMeshProUGUI SteamLoadingText;

	[SerializeField]
	private TextMeshProUGUI btnSteamSubUnsubText;

	[SerializeField]
	private TextMeshProUGUI noSteamItemSelectedText;

	[SerializeField]
	private GameObject steamItemContent;

	[SerializeField]
	private RawImage steamItemPreview;

	[SerializeField]
	private Button btnSteamOverlay;

	[SerializeField]
	private Button btnSteamSubUnsub;

	[SerializeField]
	private Button btnSteamItem;

	[SerializeField]
	private TextMeshProUGUI steamItemKSPVersionText;

	[SerializeField]
	private TextMeshProUGUI steamItemVesselTypeText;

	[SerializeField]
	private TextMeshProUGUI steamItemDescriptionText;

	[SerializeField]
	private TextMeshProUGUI steamItemAuthorText;

	[SerializeField]
	private TextMeshProUGUI steamItemFavouriteText;

	[SerializeField]
	private TextMeshProUGUI steamItemModsText;

	[SerializeField]
	private TextMeshProUGUI steamItemFileSizeText;

	[SerializeField]
	private TextMeshProUGUI steamItemUpVotesText;

	[SerializeField]
	private TextMeshProUGUI steamItemDownVotesText;

	[SerializeField]
	private TextMeshProUGUI steamItemSubscribersText;

	[SerializeField]
	private Toggle SteamFilterToggleOne;

	[SerializeField]
	private Toggle SteamFilterToggleTwo;

	[SerializeField]
	private Toggle SteamFilterToggleThree;

	[SerializeField]
	private Toggle SteamFilterToggleFour;

	[SerializeField]
	private GameObject craftImageSteamOverlay;

	[SerializeField]
	private TextMeshProUGUI vesselName;

	[SerializeField]
	private TextMeshProUGUI steamItemInfovesselName;

	[SerializeField]
	private TextMeshProUGUI vesselDescription;

	private List<VesselDataItem> vesselDataItemList;

	private UIList<ListItem> scrollList;

	private string flagURL;

	private VesselCrewManifest crewManifest;

	private CraftSelectedCallback OnFileSelected;

	private VesselDataItem selectedDataItem;

	internal string siteName;

	internal EditorFacility facility;

	internal LaunchSiteFacility launchSiteFacility;

	private ActiveToggle activeToggle;

	[SerializeField]
	private PublishedFileId_t[] steamSubscribedItems;

	private SteamQueryFilters selectedSteamQueryFilter;

	protected List<CraftEntry> craftList;

	protected CraftEntry selectedEntry;

	private Toggle tabSteamToggle;

	private bool launchSiteSelectorIsAvailable;

	private Button btnCrewSteamSubUnsub;

	private TextMeshProUGUI crewSteamSubUnsubText;

	private static float maxFrameTime;

	private Coroutine createVesselCoroutine;

	internal string craftSubfolder;

	internal string profileName;

	private int sortButton;

	private bool sortAsc;

	private EditorFacility editorFacility;

	public static VesselSpawnDialog Instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public bool Visible
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public bool HasSearchText
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselSpawnDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static VesselSpawnDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AstronautComplexSpawn()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AstronautComplexDespawn()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void InitiateGUI(GameEvents.VesselSpawnInfo info)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onTabSteamToggle(bool toggle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onTabVesselListToggle(bool toggle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateSteamSubscribedItems()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool SteamItemSubscribed(ulong fileId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateSteamListOfCraft(SteamQueryFilters selectedQueryType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void getSteamWorkshopCraft(EUGCQuery queryType, string[] tags)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onSteamQueryItemsCallback(SteamUGCQueryCompleted_t results, bool bIOFailure)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuildSteamCraftList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onButtonSteamSubUnsub()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onUnsubscribeItemCallback(RemoteStorageUnsubscribePublishedFileResult_t callResult, bool bIOFailure)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onSubscribeItemCallback(RemoteStorageSubscribePublishedFileResult_t callResult, bool bIOFailure)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SaveCraftSteamUnsubscribeWarning(bool dontShowAgain)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onSteamFilterOne(bool value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onSteamFilterTwo(bool value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onSteamFilterThree(bool value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onSteamFilterFour(bool value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnBtnSteamOverlay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onBtnSteamItemDetails()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CLoadSteamItemPreviewURL_003Ed__104))]
	private IEnumerator LoadSteamItemPreviewURL(CraftEntry selectedEntry)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void AddCraftEntryWidget(CraftEntry entry, RectTransform listParent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ClearCraftList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ClearSelection()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnEntrySelected(CraftEntry entry)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnSelectionChanged(CraftEntry selectedEntry)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CCreateVesselList_003Ed__114))]
	private IEnumerator CreateVesselList(string craftSubfolder, string profileName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SelectVesselDataItem(VesselDataItem dataItem)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CLateSelectFireThing_003Ed__116))]
	private IEnumerator LateSelectFireThing()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddVesselListItem(string name, float mass, float cost, float size)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private VesselListItem AddVesselDataItem(List<VesselDataItem> list, VesselDataItem item)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private VesselDataItem GetVesselDataItem(int dataIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private VesselDataItem GetVesselDataItem(string dataName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string GetCostTextColorHex(VesselDataItem item)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string GetMassTextColorHex(VesselDataItem item)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string GetSizeTextColorHex(VesselDataItem item)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string GetPartCountTextColorHex(VesselDataItem item)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onVesselListItemTrue(PointerEventData data, UIRadioButton.CallType callType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onVesselListItemClick(PointerEventData data, UIRadioButton.State state, UIRadioButton.CallType callType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateVesselInfoPanel(VesselDataItem dataItem)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselListSort(int button, bool asc)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ButtonLaunch()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ConfirmLaunch()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ButtonDelete()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDeleteConfirm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDeleteDismiss()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ButtonEdit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onEditBtnProceed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onPipelineFailed(KSPUpgradePipeline.UpgradeFailOption opt, ConfigNode n, Callback onFinish)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onPipelineFinished(ConfigNode n, Callback onFinish)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onEditBtnPipelineFinish()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LaunchSelectedVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onEditBtnAbort()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ButtonClose()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFlagSelected(FlagBrowser.FlagEntry flag)
	{
		throw null;
	}
}
