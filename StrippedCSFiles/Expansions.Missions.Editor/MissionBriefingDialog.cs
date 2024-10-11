using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Missions.Flow;
using KSP.UI.TooltipTypes;
using KSP.UI.Util;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class MissionBriefingDialog : MonoBehaviour
{
	public enum TabEnum
	{
		Info,
		Objectives,
		Score,
		Awards
	}

	private static MissionBriefingDialog Instance;

	private FlagBrowserGUIButton flagBrowserGUIButton;

	public string DefaultFlagURL;

	private static string fURL;

	private FlagBrowser browser;

	private MEBannerBrowser bannerBrowser;

	public GameObject FlagBrowserPrefab;

	public GameObject BannerBrowserPrefab;

	public UISkinDefSO guiSkinDef;

	public RawImage btnFlagImage;

	public RawImage imageBannerMenu;

	public RawImage imageBannerSuccess;

	public RawImage imageBannerFail;

	public TMP_InputField titleText;

	public TMP_InputField briefingText;

	public TMP_InputField authorText;

	public TMP_Text objectivesText;

	public TMP_InputField addCustomTagText;

	public GridLayoutGroup tagLayoutGroup;

	public MissionBriefingTag missionBriefingTag;

	[SerializeField]
	private TooltipController_Text briefingTooltip;

	[SerializeField]
	private TooltipController_Text titleTooltip;

	[SerializeField]
	private TooltipController_Text authorTooltip;

	[SerializeField]
	private Button btnOpenFlag;

	[SerializeField]
	private Button btnExport;

	[SerializeField]
	private Button btnSteamExport;

	[SerializeField]
	private Button btnCancel;

	[SerializeField]
	private Button btnOK;

	[SerializeField]
	private Button btnAddTag;

	[SerializeField]
	private Button btnBannerMenu;

	[SerializeField]
	private Button btnBannerSuccess;

	[SerializeField]
	private Button btnBannerFail;

	[SerializeField]
	private Button btnModsList;

	[SerializeField]
	private TMP_InputField modsText;

	[SerializeField]
	private TMP_Dropdown missionPacks;

	[SerializeField]
	private Toggle hardIndicator;

	[SerializeField]
	private Slider difficultySlider;

	[SerializeField]
	private TextMeshProUGUI difficultyLabel;

	[SerializeField]
	private Image difficultyHead;

	[SerializeField]
	private List<Sprite> difficultyHeads;

	[SerializeField]
	private Toggle scoreEnable;

	[SerializeField]
	private TMP_InputField maxScore;

	[SerializeField]
	private ToggleGroup tabGroup;

	[SerializeField]
	private Toggle tabInfo;

	[SerializeField]
	private Toggle tabObjectives;

	[SerializeField]
	private Toggle tabScore;

	[SerializeField]
	private Toggle tabAwards;

	[SerializeField]
	private GameObject panelInfo;

	[SerializeField]
	private GameObject panelObjectives;

	[SerializeField]
	private GameObject panelScore;

	[SerializeField]
	private GameObject panelAwards;

	[SerializeField]
	private TMP_Text tabCaption;

	private TabEnum currentTab;

	public MEGUIParameterDynamicModuleList scoreParameterPrefab;

	public MEGUIParameterDynamicModuleList awardsParameterPrefab;

	public TMP_Text scoreDescriptionText;

	protected MEGUIParameterDynamicModuleList globalScoreParameter;

	public Transform globalScoreContentRoot;

	protected MEGUIParameterDynamicModuleList awardsParameter;

	public Transform awardsContentRoot;

	[MEGUI_DynamicModuleList(guiName = "#autoLOC_8006008", Tooltip = "#autoLOC_8001023")]
	private MissionScore globalScore;

	[MEGUI_DynamicModuleList(allowMultipleModuleInstances = true, guiName = "#autoLOC_8200084")]
	private MissionAwards awards;

	public MEGUIParameterAwardModule[] scoreAwards;

	[SerializeField]
	private Transform MEFlowObjectsParent;

	[SerializeField]
	private TMP_Text MEFlowEmptyObjectivesMessage;

	private MEFlowParser flowParser;

	private MEBannerEntry tempBannerMenu;

	private MEBannerEntry tempBannerSuccess;

	private MEBannerEntry tempBannerFail;

	private static List<MissionBriefingTag> missionBriefingTagList;

	private static Mission editorMissionReference;

	private Callback afterOKCallback;

	private Callback afterCancelCallback;

	private bool setSteamExportItemPublic;

	private string setSteamExportItemChangeLog;

	private double timeSteamCommsStarted;

	private PopupDialog steamCommsDialog;

	private bool steamUploadingNewItem;

	private List<AppId_t> appDependenciesToRemove;

	private string titleTextValue;

	private string briefingTextValue;

	private string authorTextValue;

	private static string flagURL
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MissionBriefingDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static MissionBriefingDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static MissionBriefingDialog Display(Mission currentMission, TabEnum selectedTab, Callback afterOKCallback = null, Callback afterCancelCallback = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static MissionBriefingDialog Display(Mission currentMission, Callback afterOKCallback = null, Callback afterCancelCallback = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void Hide(Mission currentMission)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void DisableExportButon()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LocalizationLock()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LocalizationLockTitle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LocalizationLockAuthor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LocalizationLockBriefing()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RefreshBanners()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LoadMissionPacks()
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
	private void OnBriefTextChanged(string newText)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnAuthorTextChanged(string newText)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnTitleTextChanged(string newText)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnMissionTagRemoved(string tagText)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onLocalizationLockOverriden()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCancel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnOK()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateMissionDetails()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnExport()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onSteamExport()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onSteamExportConfirm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onSteamExportAfterSave()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onQuerySteamComplete(SteamUGCQueryCompleted_t qryResults, bool bIOFailure)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onNewItemCreated(CreateItemResult_t qryResults, bool bIOFailure)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void updateSteamItem(PublishedFileId_t fileId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onGetAppDependency(GetAppDependenciesResult_t result, bool bIOFailure)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onAppDependencyRemoved(RemoveAppDependencyResult_t result, bool bIOFailure)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateSteamItem(Callback<SubmitItemUpdateResult_t, bool> onCompleted, PublishedFileId_t fileId, string contentURL, string previewURL, string title, string description, string changeNote, string metaData, AppId_t[] appDependencies = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onItemUpdated(SubmitItemUpdateResult_t updateResult, bool bIOFailure)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ExportMissionNameConfirmed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ExportMissionNameCancelled()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnExitAfterSave()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ExportMissionAfterChecksPassed(string exportFileName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SpawnNoMissionNameDialogue()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DestroyDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnTabInfoPress(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnTabValidationPress(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnTabScorePress(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnTabAwardsPress(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SetTab(TabEnum newTab)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetTabVisible(TabEnum tab, bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetTabCaption(TabEnum tab)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnEndEdit(string s)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnAddTag()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SpawnNewTagPrefab(string missionTag)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupGlobalScore()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupAwards()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnOpenflag()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFlagSelect(FlagBrowser.FlagEntry selected)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFlagCancel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnOpenBannerMenu()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnOpenBannerSuccess()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnOpenBannerFail()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OpenBannerSelector(MEBannerType bannerType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnBannerCancel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnBannerSelected(MEBannerEntry banner, MEBannerType bannerType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnBannerDeleted(MEBannerEntry banner, MEBannerType bannerType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetTempBanner(MEBannerEntry newBanner, MEBannerType bannerType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void GetTempBanner(ref MEBannerEntry tempBanner, MEBannerType bannerType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnModsList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDifficultySliderChanged(float value)
	{
		throw null;
	}
}
