using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Expansions.Missions.Flow;
using KSP.UI.Util;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Expansions.Missions.Runtime;

public class MissionPlayDialog : MonoBehaviour
{
	public class dialogList
	{
		public bool steamItem;

		public MissionFileInfo missionFileInfo;

		public SteamMissionFileInfo steamMissionFileInfo;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public dialogList()
		{
			throw null;
		}
	}

	public delegate void FinishedCallback(MissionFileInfo missionInfo);

	private enum SteamQueryFilters
	{
		VOTE,
		FEATURED,
		NEWEST,
		SUBSCRIBERS
	}

	public class MissionProfileInfo : IConfigNode
	{
		public Guid id;

		internal string idName;

		public string title;

		public string briefing;

		public string author;

		public string modsBriefing;

		public string packName;

		public int order;

		public bool hardIcon;

		public MissionDifficulty difficulty;

		public int vesselCount;

		public double startUT;

		public int nodeCount;

		public List<string> tags;

		public bool errorAccess;

		public string errorDetails;

		public string saveMD5;

		public long lastWriteTime;

		public string missionExpansionVersion;

		public ulong steamPublishedFileId;

		public List<string> testModules;

		public List<string> actionModules;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public MissionProfileInfo()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public MissionProfileInfo(string filename, string saveFolder, Mission mission)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public MissionProfileInfo LoadDetailsFromMission(Mission mission)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Load(ConfigNode node)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Save(ConfigNode node)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void LoadFromMetaFile(string filename, string saveFolder)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void SaveToMetaFile(string filename, string saveFolder)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static string GetSFSMD5(string filename, string saveFolder)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static long GetLastWriteTime(string filename, string saveFolder)
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CBuildList_003Ed__106 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public MissionPlayDialog _003C_003E4__this;

		private int _003Cc_003E5__2;

		private int _003Cj_003E5__3;

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
		public _003CBuildList_003Ed__106(int _003C_003E1__state)
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
	private sealed class _003CLoadSteamItemPreviewURL_003Ed__142 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public MissionPlayDialog _003C_003E4__this;

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
		public _003CLoadSteamItemPreviewURL_003Ed__142(int _003C_003E1__state)
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

	private static MissionPlayDialog Instance;

	private Texture2D currentIcon;

	public Material iconMaterial;

	public Color scnIconColor;

	[SerializeField]
	[Header("UI Components")]
	private Button btnLoad;

	[SerializeField]
	private Button btnCancel;

	[SerializeField]
	private Button btnRestart;

	[SerializeField]
	private Button btnEdit;

	[SerializeField]
	private Button btnCreate;

	[SerializeField]
	private Button btnSteamOverlay;

	[SerializeField]
	private Button btnSteamItemDetails;

	[SerializeField]
	private Button btnSteamSubUnSub;

	[SerializeField]
	private Button btnMainSteamSubUnSub;

	[SerializeField]
	private TextMeshProUGUI MainSteamSubUnsubText;

	[SerializeField]
	private Toggle tabCommunity;

	[SerializeField]
	private Toggle tabStock;

	[SerializeField]
	private Toggle tabSteam;

	[SerializeField]
	private TextMeshProUGUI textTitle;

	[SerializeField]
	private TextMeshProUGUI textDescription;

	[SerializeField]
	private TextMeshProUGUI btnLoadCaption;

	[SerializeField]
	private TextMeshProUGUI textModsDescription;

	[SerializeField]
	private TextMeshProUGUI textAuthor;

	[SerializeField]
	private RawImage imgScnBanner;

	[SerializeField]
	private RectTransform dialogRectTransform;

	[SerializeField]
	private RectTransform containerDefault;

	[SerializeField]
	private RectTransform containerInfo;

	[SerializeField]
	private RectTransform scrollListContent;

	[SerializeField]
	private ToggleGroup listGroup;

	[SerializeField]
	private GameObject modsSection;

	[SerializeField]
	private AwardWidget awardWidgetPrefab;

	[SerializeField]
	private Transform awardsContent;

	[SerializeField]
	private TextMeshProUGUI textHighScore;

	[SerializeField]
	private TextMeshProUGUI textMaxScore;

	[SerializeField]
	private GameObject maxScoreField;

	[SerializeField]
	private GameObject highScoreField;

	[SerializeField]
	private GameObject awardsHeader;

	[SerializeField]
	private GameObject awardsHolder;

	[SerializeField]
	private GameObject objectivesHeader;

	[SerializeField]
	private GameObject progressField;

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
	private GameObject SteamSection;

	[SerializeField]
	private TextMeshProUGUI AuthorTitleText;

	[SerializeField]
	private TextMeshProUGUI SteamFileSizeText;

	[SerializeField]
	private TextMeshProUGUI SteamUpVotesText;

	[SerializeField]
	private TextMeshProUGUI SteamSubscribersText;

	[SerializeField]
	private TextMeshProUGUI SteamDownVotesText;

	[SerializeField]
	private TextMeshProUGUI SteamSubUnsubText;

	[SerializeField]
	private TextMeshProUGUI SteamLoadingText;

	[SerializeField]
	private GameObject SteamButtonSection;

	[SerializeField]
	private GameObject ButtonSection;

	[SerializeField]
	private Transform MEFlowObjectsParent;

	[SerializeField]
	private TMP_Text MEFlowEmptyObjectivesMessage;

	[SerializeField]
	private GameObject difficultyField;

	[SerializeField]
	private TextMeshProUGUI textDifficulty;

	[SerializeField]
	private Image difficultyHead;

	public Texture2D missionNormalIcon;

	public Texture2D missionHardIcon;

	public Texture2D steamIcon;

	public Texture2D steamSub;

	public Texture2D steamUnsub;

	[Header("Other Variables")]
	private DictionaryValueList<DialogGUIToggleButton, dialogList> items;

	private DictionaryValueList<MissionSteamItemWidget, dialogList> steamItems;

	[SerializeField]
	private PublishedFileId_t[] steamSubscribedItems;

	private FinishedCallback OnFinishedCallback;

	private Callback OnDialogDismissCallback;

	private UISkinDef skin;

	private bool confirmGameRestart;

	private List<MissionFileInfo> missionList;

	private List<MissionPack> missionPacks;

	private MissionFileInfo selectedMission;

	private SteamMissionFileInfo _selectedSteamMissionItem;

	private MissionTypes selectedType;

	private SteamQueryFilters selectedSteamQueryFilter;

	private MissionScoreInfo missionScoreInfo;

	private MEFlowParser flowParser;

	public UISkinDefSO guiSkinDef;

	[SerializeField]
	private List<Texture2D> missionDifficultyButtons;

	[SerializeField]
	private List<Sprite> difficultyHeads;

	private string authorNormalTitle;

	private string authorSteamTitle;

	private Coroutine buildListCoroutine;

	private DictionaryValueList<string, MissionListGroup> packGroups;

	private static float maxFrameTime;

	private bool dialogFullExpansionMode;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MissionPlayDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static MissionPlayDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static MissionPlayDialog Create(Callback onDismiss)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnMissionLoadDismiss(MissionFileInfo missionInfo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onMissionLoadSuccess()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onMissionLoadFail()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
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
	private void AddTutorialLock()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGameSettingsWritten()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RemoveTutorialLock()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Terminate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void UpdateListOfMissions()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void UpdateSteamListOfMissions(SteamQueryFilters selectedQueryType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onMissionImported()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CBuildList_003Ed__106))]
	private IEnumerator BuildList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DisplayAddedMissions()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuildSteamList(SteamUGCQueryCompleted_t results, bool bIOFailure, bool morePages)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuildPacksList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ClearPacksList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void CreateWidget(MissionFileInfo mission, DialogGUIVerticalLayout vLayout)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void CreateSteamWidget(SteamUGCDetails_t itemDetails, SteamMissionFileInfo steamMissionFileInfo, MissionSteamItemWidget toggle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void useMissionData(MissionFileInfo mission, DialogGUIVerticalLayout vLayout, DialogGUILabel.TextLabelOptions labelOptsForSaveDetails)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ClearListItems()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetHidden(bool hide)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetLocked(bool locked)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnTabStock(bool value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnTabCommunity(bool value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnTabSteam(bool value)
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
	private void onBtnSteamSubUnSub()
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
	private void SaveMissionSteamUnsubscribeWarning(bool dontShowAgain)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnTabStockInCode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnButtonEdit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnButtonCreate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnButtonRestart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnBtnCancel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnButtonLoad()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DismissRestartGame()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ConfirmLoadGame()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CancelLoadGame()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCloseWindow()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ConfirmRestart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnSelectionChanged(bool haveSelection, bool steamView = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CLoadSteamItemPreviewURL_003Ed__142))]
	private IEnumerator LoadSteamItemPreviewURL()
	{
		throw null;
	}
}
