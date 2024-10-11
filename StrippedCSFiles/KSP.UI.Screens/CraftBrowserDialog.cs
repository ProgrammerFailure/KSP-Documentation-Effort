using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using KSP.UI.Util;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class CraftBrowserDialog : MonoBehaviour
{
	public delegate void SelectFileCallback(string fullPath, LoadType t);

	public delegate void SelectConfigNodeCallback(ConfigNode n, LoadType t);

	public delegate void CancelledCallback();

	private enum SteamQueryFilters
	{
		VOTE,
		FEATURED,
		NEWEST,
		SUBSCRIBERS
	}

	public enum LoadType
	{
		Normal,
		Merge
	}

	[CompilerGenerated]
	private sealed class _003CLoadSteamItemPreviewURL_003Ed__129 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public CraftEntry selectedEntry;

		public CraftBrowserDialog _003C_003E4__this;

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
		public _003CLoadSteamItemPreviewURL_003Ed__129(int _003C_003E1__state)
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

	private DirectoryController directoryController;

	protected List<CraftEntry> craftList;

	protected string craftSubfolder;

	protected EditorFacility facility;

	protected CraftEntry selectedEntry;

	public SelectFileCallback OnFileSelected;

	public SelectConfigNodeCallback OnConfigNodeSelected;

	public CancelledCallback OnBrowseCancelled;

	protected string profile;

	protected bool showMergeOption;

	protected string title;

	[Tooltip("UI to toggle searching in subdirectories")]
	[SerializeField]
	public Toggle subDirectorySearch;

	[SerializeField]
	[Tooltip("UI to toggle searching in all game folders")]
	public Toggle allGameSearch;

	[SerializeField]
	private TextMeshProUGUI header;

	[SerializeField]
	private Toggle tabVAB;

	[SerializeField]
	private Toggle tabSPH;

	[SerializeField]
	private Toggle tabSteam;

	[SerializeField]
	private Button btnCancel;

	[SerializeField]
	private Button btnLoad;

	[SerializeField]
	private Button btnMerge;

	[SerializeField]
	private Button btnDelete;

	[SerializeField]
	private GameObject SteamButtons;

	[SerializeField]
	private Button btnSteamOverlay;

	[SerializeField]
	private Button btnSteamItem;

	[SerializeField]
	private Button btnSteamSubUnsub;

	[SerializeField]
	private TextMeshProUGUI btnSteamSubUnsubText;

	[SerializeField]
	private Button btnMainSteamSubUnsub;

	[SerializeField]
	private TextMeshProUGUI btnMainSteamSubUnsubText;

	[SerializeField]
	private TextMeshProUGUI SteamLoadingText;

	[SerializeField]
	private GameObject SteamFilterHolder;

	[SerializeField]
	private Toggle SteamFilterToggleOne;

	[SerializeField]
	private Toggle SteamFilterToggleTwo;

	[SerializeField]
	private Toggle SteamFilterToggleThree;

	[SerializeField]
	private Toggle SteamFilterToggleFour;

	[SerializeField]
	private UIPanelTransition SteamDetailsPanel;

	[SerializeField]
	private TextMeshProUGUI noSteamItemSelectedText;

	[SerializeField]
	private GameObject SteamItemContent;

	[SerializeField]
	private TextMeshProUGUI AuthorTitleText;

	[SerializeField]
	private TextMeshProUGUI steamItemKSPVersionText;

	[SerializeField]
	private TextMeshProUGUI steamItemVesselTypeText;

	[SerializeField]
	private TextMeshProUGUI steamItemDescriptionText;

	[SerializeField]
	private TextMeshProUGUI steamItemAuthorText;

	[SerializeField]
	private TextMeshProUGUI steamItemModsText;

	[SerializeField]
	private TextMeshProUGUI steamItemFavouriteText;

	[SerializeField]
	private TextMeshProUGUI steamItemFileSizeText;

	[SerializeField]
	private TextMeshProUGUI steamItemUpVotesText;

	[SerializeField]
	private TextMeshProUGUI steamItemDownVotesText;

	[SerializeField]
	private TextMeshProUGUI steamItemSubscribersText;

	[SerializeField]
	private RawImage steamItemPreview;

	[SerializeField]
	private RectTransform scrollView;

	[SerializeField]
	private ToggleGroup listGroup;

	[SerializeField]
	private RectTransform listContainer;

	[SerializeField]
	protected UISkinDefSO uiSkin;

	protected UISkinDef skin;

	private PopupDialog window;

	private PopupDialog unableToSubscribeToSteam;

	private PopupDialog steamError;

	private PopupDialog vesselIsIncompatible;

	private UIConfirmDialog steamWorkshopItemUnsubscribe;

	private List<Selectable> baseSelectables;

	private List<Selectable> totalSelectables;

	private List<Selectable> loadedVesselsSelectables;

	private List<Selectable> filteredLoadedVessels;

	private bool filterNextLoadedVessels;

	[SerializeField]
	private Transform directoryContent;

	private MenuNavigation menuNavigation;

	private Navigation tabVabNav;

	private Navigation tabSphNav;

	private Navigation secondVesselNav;

	private Navigation steamToggleOneNav;

	private Navigation steamToggleTwoNav;

	private Navigation steamToggleThreeNav;

	private Navigation steamToggleFourNav;

	private Navigation btnCancelNav;

	private Navigation btnLoadNav;

	private Navigation btnMergeNav;

	private Navigation btnDeleteNav;

	private Navigation btnSteamOverlayNav;

	[SerializeField]
	private PublishedFileId_t[] steamSubscribedItems;

	private string[] nonGameSaveDirectories;

	private SteamQueryFilters selectedSteamQueryFilter;

	private bool steamTabOpen;

	private string authorNormalTitle;

	private string authorSteamTitle;

	private const string folderSeparator = "/";

	private const string craftFiles = "*.craft";

	public static ScrollRect ScrollView
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

	public bool IsSubdirectorySearch
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsAllGameSearch
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CraftBrowserDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static CraftBrowserDialog Spawn(EditorFacility facility, string profile, SelectFileCallback onFileSelected, CancelledCallback onCancel, bool showMergeOption)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static CraftBrowserDialog Spawn(EditorFacility facility, string profile, SelectConfigNodeCallback onConfigNodeSelected, CancelledCallback onCancel, bool showMergeOption)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Dismiss()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void HideForLater()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void ReDisplay(EditorFacility newFacility, bool showMergeOption)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDisable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Show()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void Hide()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static CraftEntry RemoveCreateCraftEntry(List<CraftEntry> craftEntries, FileInfo file, bool stock, Callback<CraftEntry> OnSelected, bool steamItem, SteamCraftInfo steamCraftInfo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void BuildPlayerCraftList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private FileInfo[] GetMissionCraftFiles()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private FileInfo[] GetPlayerCraftFiles()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private FileInfo[] GetAllGameFiles(string searchPattern, SearchOption searchOption)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void BuildStockCraftList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void BuildSteamSubscribedCraftList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateCraftListUI(List<CraftEntry> craftList)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FilterUpdateSelectables(bool filtered)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateSelectables()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InitializeMenuNavigation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateExplicitNavigation()
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
	protected void ClearOldCraftEntries(List<CraftEntry> oldCraftList)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ClearCraftEntry(CraftEntry craftEntry)
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
	[IteratorStateMachine(typeof(_003CLoadSteamItemPreviewURL_003Ed__129))]
	private IEnumerator LoadSteamItemPreviewURL(CraftEntry selectedEntry)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void onButtonLoad()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ConfirmLoadCraft()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onButtonMerge()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void onButtonCancel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SearchOption_OnChanged(bool value)
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
	private void remoteFileSubscribedCallback(RemoteStoragePublishedFileSubscribed_t result, bool bIOFailure)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void remoteFileUnsubscribedCallback(RemoteStoragePublishedFileUnsubscribed_t result, bool bIOFailure)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void pipeSelectedItem(CraftEntry sItem, LoadType loadType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onPipelineFailed(KSPUpgradePipeline.UpgradeFailOption opt, ConfigNode n, CraftEntry sItem, LoadType loadType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onPipelineFinished(ConfigNode n, CraftEntry sItem, LoadType loadType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void onButtonDelete()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void PromptDeleteFileConfirm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDeleteConfirm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void onVABtabToggle(bool isSelected)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void onSPHtabToggle(bool isSelected)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void onSteamtabToggle(bool isSelected)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void setbottomButtons(CraftEntry selectedEntry, bool steamMode)
	{
		throw null;
	}
}
