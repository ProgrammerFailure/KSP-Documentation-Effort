using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Expansions.Missions.Flow;
using ns12;
using ns16;
using ns2;
using ns9;
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

	public static MissionBriefingDialog Instance;

	public FlagBrowserGUIButton flagBrowserGUIButton;

	public string DefaultFlagURL;

	public static string fURL = null;

	public FlagBrowser browser;

	public MEBannerBrowser bannerBrowser;

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
	public TooltipController_Text briefingTooltip;

	[SerializeField]
	public TooltipController_Text titleTooltip;

	[SerializeField]
	public TooltipController_Text authorTooltip;

	[SerializeField]
	public Button btnOpenFlag;

	[SerializeField]
	public Button btnExport;

	[SerializeField]
	public Button btnSteamExport;

	[SerializeField]
	public Button btnCancel;

	[SerializeField]
	public Button btnOK;

	[SerializeField]
	public Button btnAddTag;

	[SerializeField]
	public Button btnBannerMenu;

	[SerializeField]
	public Button btnBannerSuccess;

	[SerializeField]
	public Button btnBannerFail;

	[SerializeField]
	public Button btnModsList;

	[SerializeField]
	public TMP_InputField modsText;

	[SerializeField]
	public TMP_Dropdown missionPacks;

	[SerializeField]
	public Toggle hardIndicator;

	[SerializeField]
	public Slider difficultySlider;

	[SerializeField]
	public TextMeshProUGUI difficultyLabel;

	[SerializeField]
	public Image difficultyHead;

	[SerializeField]
	public List<Sprite> difficultyHeads;

	[SerializeField]
	public Toggle scoreEnable;

	[SerializeField]
	public TMP_InputField maxScore;

	[SerializeField]
	public ToggleGroup tabGroup;

	[SerializeField]
	public Toggle tabInfo;

	[SerializeField]
	public Toggle tabObjectives;

	[SerializeField]
	public Toggle tabScore;

	[SerializeField]
	public Toggle tabAwards;

	[SerializeField]
	public GameObject panelInfo;

	[SerializeField]
	public GameObject panelObjectives;

	[SerializeField]
	public GameObject panelScore;

	[SerializeField]
	public GameObject panelAwards;

	[SerializeField]
	public TMP_Text tabCaption;

	public TabEnum currentTab;

	public MEGUIParameterDynamicModuleList scoreParameterPrefab;

	public MEGUIParameterDynamicModuleList awardsParameterPrefab;

	public TMP_Text scoreDescriptionText;

	public MEGUIParameterDynamicModuleList globalScoreParameter;

	public Transform globalScoreContentRoot;

	public MEGUIParameterDynamicModuleList awardsParameter;

	public Transform awardsContentRoot;

	[MEGUI_DynamicModuleList(guiName = "#autoLOC_8006008", Tooltip = "#autoLOC_8001023")]
	public MissionScore globalScore;

	[MEGUI_DynamicModuleList(allowMultipleModuleInstances = true, guiName = "#autoLOC_8200084")]
	public MissionAwards awards;

	public MEGUIParameterAwardModule[] scoreAwards;

	[SerializeField]
	public Transform MEFlowObjectsParent;

	[SerializeField]
	public TMP_Text MEFlowEmptyObjectivesMessage;

	public MEFlowParser flowParser;

	public MEBannerEntry tempBannerMenu;

	public MEBannerEntry tempBannerSuccess;

	public MEBannerEntry tempBannerFail;

	public static List<MissionBriefingTag> missionBriefingTagList = new List<MissionBriefingTag>();

	public static Mission editorMissionReference;

	public Callback afterOKCallback;

	public Callback afterCancelCallback;

	public bool setSteamExportItemPublic;

	public string setSteamExportItemChangeLog = Localizer.Format("#autoLOC_8002120");

	public double timeSteamCommsStarted;

	public PopupDialog steamCommsDialog;

	public bool steamUploadingNewItem;

	public List<AppId_t> appDependenciesToRemove;

	public string titleTextValue;

	public string briefingTextValue;

	public string authorTextValue;

	public static string flagURL
	{
		get
		{
			return fURL;
		}
		set
		{
			fURL = value;
		}
	}

	public void Update()
	{
		if (Input.GetKeyUp(KeyCode.Escape) && btnCancel.gameObject.activeSelf)
		{
			OnCancel();
		}
		if (steamCommsDialog != null && (double)Time.time - timeSteamCommsStarted > 60.0)
		{
			steamCommsDialog.Dismiss();
			steamCommsDialog = null;
		}
	}

	public static MissionBriefingDialog Display(Mission currentMission, TabEnum selectedTab, Callback afterOKCallback = null, Callback afterCancelCallback = null)
	{
		MissionBriefingDialog missionBriefingDialog = Display(currentMission, afterOKCallback, afterCancelCallback);
		missionBriefingDialog.SetTab(selectedTab);
		return missionBriefingDialog;
	}

	public static MissionBriefingDialog Display(Mission currentMission, Callback afterOKCallback = null, Callback afterCancelCallback = null)
	{
		if (Instance == null)
		{
			UnityEngine.Object @object = MissionsUtils.MEPrefab("_UI5/Dialogs/MissionBriefingDialog/prefabs/MissionBriefingDialog.prefab");
			if (@object == null)
			{
				Debug.LogError("[MissionBriefingDialog]: Unable to load the Asset");
				return null;
			}
			Instance = ((GameObject)UnityEngine.Object.Instantiate(@object)).GetComponent<MissionBriefingDialog>();
		}
		Instance.transform.SetParent(DialogCanvasUtil.DialogCanvasRect, worldPositionStays: false);
		editorMissionReference = currentMission;
		missionBriefingTagList.Clear();
		Instance.titleTooltip.enabled = false;
		Instance.briefingTooltip.enabled = false;
		Instance.titleTextValue = currentMission.title;
		Instance.briefingTextValue = currentMission.briefing;
		Instance.authorTextValue = currentMission.author;
		Instance.LocalizationLock();
		Instance.modsText.text = currentMission.modsBriefing;
		Instance.scoreEnable.isOn = currentMission.isScoreEnabled;
		Instance.maxScore.text = currentMission.maxScore.ToString("F0");
		flagURL = currentMission.flagURL;
		if (string.IsNullOrEmpty(flagURL))
		{
			flagURL = Instance.DefaultFlagURL;
		}
		Texture2D texture = GameDatabase.Instance.GetTexture(flagURL, asNormalMap: false);
		Instance.scoreDescriptionText.text = currentMission.PrintScoreObjectives(onlyPrintActivatedNodes: false, startWithActiveNode: false, onlyAwardedScores: false);
		Instance.awards = currentMission.awards.Clone() as MissionAwards;
		Instance.globalScore = currentMission.globalScore.Clone() as MissionScore;
		Instance.flagBrowserGUIButton = new FlagBrowserGUIButton(texture, Instance.OnOpenflag, Instance.OnFlagSelect, Instance.OnFlagCancel);
		Instance.flagBrowserGUIButton.flagURL = flagURL;
		Instance.btnFlagImage.texture = texture;
		Instance.btnSteamExport.gameObject.SetActive(SteamManager.Initialized);
		Instance.afterOKCallback = afterOKCallback;
		Instance.afterCancelCallback = afterCancelCallback;
		Instance.LoadMissionPacks();
		Instance.hardIndicator.isOn = currentMission.hardIcon;
		Instance.difficultySlider.value = (float)currentMission.difficulty;
		Instance.tempBannerMenu = currentMission.GetBanner(MEBannerType.Menu);
		Instance.tempBannerSuccess = currentMission.GetBanner(MEBannerType.Success);
		Instance.tempBannerFail = currentMission.GetBanner(MEBannerType.Fail);
		Instance.RefreshBanners();
		GameEvents.Mission.onMissionBriefingCreated.Fire(Instance);
		return Instance;
	}

	public static void Hide(Mission currentMission)
	{
		if (!(Instance == null) && !editorMissionReference != (bool)currentMission)
		{
			Instance.OnCancel();
		}
	}

	public void DisableExportButon()
	{
		btnExport.interactable = false;
	}

	public void LocalizationLock()
	{
		Instance.LocalizationLockBriefing();
		Instance.LocalizationLockAuthor();
		Instance.LocalizationLockTitle();
	}

	public void LocalizationLockTitle()
	{
		Instance.titleText.text = Localizer.Format(titleTextValue);
		if (Localizer.Tags.ContainsKey(titleTextValue) && !Localizer.OverrideMELock)
		{
			Instance.titleText.interactable = false;
			Instance.titleTooltip.enabled = true;
		}
		else
		{
			Instance.titleText.interactable = true;
			Instance.titleTooltip.enabled = false;
		}
	}

	public void LocalizationLockAuthor()
	{
		Instance.authorText.text = Localizer.Format(authorTextValue);
		if (Localizer.Tags.ContainsKey(authorTextValue) && !Localizer.OverrideMELock)
		{
			Instance.authorText.interactable = false;
			Instance.authorTooltip.enabled = true;
		}
		else
		{
			Instance.authorText.interactable = true;
			Instance.authorTooltip.enabled = false;
		}
	}

	public void LocalizationLockBriefing()
	{
		Instance.briefingText.text = Localizer.Format(briefingTextValue);
		if (Localizer.Tags.ContainsKey(briefingTextValue) && !Localizer.OverrideMELock)
		{
			Instance.briefingText.interactable = false;
			Instance.briefingTooltip.enabled = true;
		}
		else
		{
			Instance.briefingText.interactable = true;
			Instance.briefingTooltip.enabled = false;
		}
	}

	public void RefreshBanners()
	{
		imageBannerMenu.texture = tempBannerMenu.texture;
		imageBannerSuccess.texture = tempBannerSuccess.texture;
		imageBannerFail.texture = tempBannerFail.texture;
	}

	public void LoadMissionPacks()
	{
		if (!(missionPacks != null))
		{
			return;
		}
		missionPacks.options.Clear();
		missionPacks.options.Add(new TMP_Dropdown.OptionData(Localizer.Format("#autoLOC_6003083")));
		int value = 0;
		if (GameSettings.MISSION_SHOW_STOCK_PACKS_IN_BRIEFING)
		{
			for (int i = 0; i < MissionEditorLogic.Instance.StockMissionPacks.Count; i++)
			{
				TMP_Dropdown.OptionData item = new TMP_Dropdown.OptionData(MissionEditorLogic.Instance.StockMissionPacks[i].displayName);
				missionPacks.options.Add(item);
				if (editorMissionReference.packName == MissionEditorLogic.Instance.StockMissionPacks[i].name)
				{
					value = i + 1;
				}
			}
		}
		if (MissionEditorLogic.Instance != null && MissionEditorLogic.Instance.userMissionPacks != null)
		{
			for (int j = 0; j < MissionEditorLogic.Instance.userMissionPacks.Count; j++)
			{
				TMP_Dropdown.OptionData item2 = new TMP_Dropdown.OptionData(MissionEditorLogic.Instance.userMissionPacks[j].displayName);
				missionPacks.options.Add(item2);
				if (editorMissionReference.packName == MissionEditorLogic.Instance.userMissionPacks[j].name)
				{
					value = j + 1;
				}
			}
		}
		missionPacks.value = value;
		missionPacks.RefreshShownValue();
	}

	public void Start()
	{
		GameEvents.Mission.onMissionTagRemoved.Add(OnMissionTagRemoved);
		GameEvents.Mission.onLocalizationLockOverriden.Add(onLocalizationLockOverriden);
		btnAddTag.onClick.AddListener(OnAddTag);
		btnOpenFlag.onClick.AddListener(OnOpenflag);
		btnCancel.onClick.AddListener(OnCancel);
		btnOK.onClick.AddListener(OnOK);
		btnExport.onClick.AddListener(OnExport);
		if (SteamManager.Initialized)
		{
			btnSteamExport.onClick.AddListener(onSteamExport);
		}
		btnBannerMenu.onClick.AddListener(OnOpenBannerMenu);
		btnBannerSuccess.onClick.AddListener(OnOpenBannerSuccess);
		btnBannerFail.onClick.AddListener(OnOpenBannerFail);
		briefingText.onEndEdit.AddListener(OnBriefTextChanged);
		authorText.onEndEdit.AddListener(OnAuthorTextChanged);
		titleText.onEndEdit.AddListener(OnTitleTextChanged);
		tabInfo.onValueChanged.AddListener(OnTabInfoPress);
		tabObjectives.onValueChanged.AddListener(OnTabValidationPress);
		tabScore.onValueChanged.AddListener(OnTabScorePress);
		tabAwards.onValueChanged.AddListener(OnTabAwardsPress);
		btnModsList.onClick.AddListener(OnModsList);
		OnDifficultySliderChanged(difficultySlider.value);
		difficultySlider.onValueChanged.AddListener(OnDifficultySliderChanged);
		addCustomTagText.onEndEdit.AddListener(delegate
		{
			if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
			{
				OnEndEdit(addCustomTagText.text);
			}
		});
		UIMasterController.Instance.RegisterNonModalDialog(GetComponent<CanvasGroup>());
		for (int i = 0; i < editorMissionReference.tags.Count; i++)
		{
			SpawnNewTagPrefab(editorMissionReference.tags[i]);
		}
		SetupGlobalScore();
		SetupAwards();
		if (flowParser == null)
		{
			flowParser = MEFlowParser.Create(base.gameObject.transform, MEFlowObjectsParent, MEFlowEmptyObjectivesMessage);
		}
		flowParser.CreateMissionFlowUI_Button(editorMissionReference, MEFlowUINode.ButtonAction.None, null, showEvents: true);
		MenuNavigation.SpawnMenuNavigation(base.gameObject, Navigation.Mode.Automatic);
	}

	public void OnDestroy()
	{
		btnAddTag.onClick.RemoveListener(OnAddTag);
		btnOpenFlag.onClick.RemoveListener(OnOpenflag);
		btnCancel.onClick.RemoveListener(OnCancel);
		btnOK.onClick.RemoveListener(OnOK);
		addCustomTagText.onEndEdit.RemoveListener(OnEndEdit);
	}

	public void OnBriefTextChanged(string newText)
	{
		briefingTextValue = newText;
		Instance.LocalizationLockBriefing();
	}

	public void OnAuthorTextChanged(string newText)
	{
		authorTextValue = newText;
		Instance.LocalizationLockAuthor();
	}

	public void OnTitleTextChanged(string newText)
	{
		titleTextValue = newText;
		editorMissionReference.name = newText;
		LocalizationLockTitle();
	}

	public void OnMissionTagRemoved(string tagText)
	{
		MissionBriefingTag missionBriefingTag = null;
		for (int i = 0; i < missionBriefingTagList.Count; i++)
		{
			if (missionBriefingTagList[i].tagText.text == tagText)
			{
				missionBriefingTag = missionBriefingTagList[i];
				break;
			}
		}
		if (missionBriefingTag != null)
		{
			missionBriefingTagList.Remove(missionBriefingTag);
			UnityEngine.Object.Destroy(missionBriefingTag.gameObject);
		}
	}

	public void onLocalizationLockOverriden()
	{
		LocalizationLock();
	}

	public void OnCancel()
	{
		if (afterCancelCallback != null)
		{
			afterCancelCallback();
		}
		DestroyDialog();
	}

	public void OnOK()
	{
		if (titleText.text == string.Empty)
		{
			SpawnNoMissionNameDialogue();
			return;
		}
		UpdateMissionDetails();
		if (afterOKCallback != null)
		{
			afterOKCallback();
		}
		DestroyDialog();
	}

	public void UpdateMissionDetails()
	{
		editorMissionReference.title = titleTextValue;
		editorMissionReference.flagURL = flagURL;
		editorMissionReference.briefing = briefingTextValue;
		editorMissionReference.author = authorTextValue;
		editorMissionReference.modsBriefing = modsText.text;
		editorMissionReference.isScoreEnabled = scoreEnable.isOn;
		float.TryParse(maxScore.text, out editorMissionReference.maxScore);
		editorMissionReference.awards = awards.Clone() as MissionAwards;
		editorMissionReference.globalScore = globalScore.Clone() as MissionScore;
		LocalizationLock();
		if (missionPacks.value < 1)
		{
			editorMissionReference.packName = "";
		}
		else if (MissionEditorLogic.Instance != null && MissionEditorLogic.Instance.userMissionPacks != null && missionPacks.value - 1 < MissionEditorLogic.Instance.userMissionPacks.Count)
		{
			editorMissionReference.packName = MissionEditorLogic.Instance.userMissionPacks[missionPacks.value - 1].name;
		}
		editorMissionReference.hardIcon = hardIndicator.isOn;
		editorMissionReference.difficulty = (MissionDifficulty)difficultySlider.value;
		editorMissionReference.tags.Clear();
		for (int i = 0; i < missionBriefingTagList.Count; i++)
		{
			if (missionBriefingTagList[i].tagText.text != string.Empty)
			{
				editorMissionReference.tags.Add(missionBriefingTagList[i].tagText.text);
			}
		}
		GameEvents.Mission.onMissionBriefingChanged.Fire(editorMissionReference);
		editorMissionReference.SetBanner(tempBannerMenu, MEBannerType.Menu);
		editorMissionReference.SetBanner(tempBannerSuccess, MEBannerType.Success);
		editorMissionReference.SetBanner(tempBannerFail, MEBannerType.Fail);
	}

	public void OnExport()
	{
		if (string.IsNullOrEmpty(titleText.text))
		{
			SpawnNoMissionNameDialogue();
			return;
		}
		if (string.IsNullOrEmpty(editorMissionReference.exportName) || editorMissionReference.missionNameAtLastExport != editorMissionReference.name)
		{
			editorMissionReference.exportName = "Mission-" + titleText.text;
		}
		PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("ExportMissionFileName", Localizer.Format("#autoLOC_8200064"), Localizer.Format("#autoLOC_8200065"), HighLogic.UISkin, 350f, new DialogGUITextInput(editorMissionReference.exportName, multiline: false, 64, delegate(string n)
		{
			editorMissionReference.exportName = n;
			return editorMissionReference.exportName;
		}, 24f), new DialogGUISpace(6f), new DialogGUILabel(Localizer.Format("#autoLOC_8005057", editorMissionReference.exportName)), new DialogGUISpace(6f), new DialogGUIHorizontalLayout(new DialogGUIFlexibleSpace(), new DialogGUIButton(Localizer.Format("#autoLOC_226975"), delegate
		{
			ExportMissionNameConfirmed();
		}, 120f, 30f, true), new DialogGUIButton(Localizer.Format("#autoLOC_226976"), delegate
		{
			ExportMissionNameCancelled();
		}, 120f, 30f, true))), persistAcrossScenes: true, null).OnDismiss = ExportMissionNameCancelled;
	}

	public void onSteamExport()
	{
		if (string.IsNullOrEmpty(titleText.text))
		{
			SpawnNoMissionNameDialogue();
			return;
		}
		setSteamExportItemPublic = false;
		SteamWorkshopExportDialog.Spawn(Localizer.Format("#autoLOC_8002121"), Localizer.Format("#autoLOC_8002122"), Localizer.Format("#autoLOC_226976"), Localizer.Format("#autoLOC_226975"), Localizer.Format("#autoLOC_8002123"), delegate(SteamWorkshopExportDialog.ReturnItems returnItems)
		{
			setSteamExportItemPublic = returnItems.setPublic;
			setSteamExportItemChangeLog = returnItems.changeLog;
			onSteamExportConfirm();
		}, null);
	}

	public void onSteamExportConfirm()
	{
		UpdateMissionDetails();
		MissionEditorLogic.Instance.SaveMission(onSteamExportAfterSave);
	}

	public void onSteamExportAfterSave()
	{
		timeSteamCommsStarted = Time.time;
		steamCommsDialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("ExportMissionSteamComms", "", Localizer.Format("#autoLOC_8002139"), guiSkinDef.SkinDef, 300f, new DialogGUISpace(6f), new DialogGUILabel(Localizer.Format("#autoLOC_8002158"))), persistAcrossScenes: true, null);
		ulong availAmount = 0uL;
		ulong totalRequired = 0uL;
		int totalFilesRequired = 0;
		int availFileCount = 0;
		if (!SteamManager.Instance.CheckCloudQuota(editorMissionReference.MissionInfo.FolderPath, out totalRequired, out availAmount, out totalFilesRequired, out availFileCount))
		{
			string text = "";
			if (totalRequired > availAmount)
			{
				AnalyticsUtil.LogSteamError(AnalyticsUtil.steamActions.update, AnalyticsUtil.steamItemTypes.mission, editorMissionReference.steamPublishedFileId, "k_EResultCustom_SteamCloudSpaceExceeded");
				text = Localizer.Format("#autoLOC_8002159", (totalRequired / 1024L).ToString("N0"), (availAmount / 1024L).ToString("N0"));
			}
			if (totalFilesRequired > availFileCount)
			{
				AnalyticsUtil.LogSteamError(AnalyticsUtil.steamActions.update, AnalyticsUtil.steamItemTypes.mission, editorMissionReference.steamPublishedFileId, "k_EResultCustom_SteamCloudFileLimitExceeded");
				text = text + "\n" + Localizer.Format("#autoLOC_8002160", totalFilesRequired, availFileCount);
			}
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("ExportMissionSteamNoSpace", text, Localizer.Format("#autoLOC_8002139"), guiSkinDef.SkinDef, 300f, new DialogGUISpace(6f), new DialogGUIButton(Localizer.Format("#autoLOC_226975"), null, dismissOnSelect: true)), persistAcrossScenes: true, null);
			if (steamCommsDialog != null)
			{
				steamCommsDialog.Dismiss();
				steamCommsDialog = null;
			}
			Debug.LogFormat("[MissionBriefing]: Unable to export Mission as there is not enough space on Steam Cloud.");
		}
		else if (editorMissionReference.steamPublishedFileId == 0L)
		{
			SteamManager.Instance.CreateNewItem(onNewItemCreated, EWorkshopFileType.k_EWorkshopFileTypeFirst);
		}
		else
		{
			SteamManager.Instance.GetUGCItem(onQuerySteamComplete, new PublishedFileId_t(editorMissionReference.steamPublishedFileId));
		}
	}

	public void onQuerySteamComplete(SteamUGCQueryCompleted_t qryResults, bool bIOFailure)
	{
		if (bIOFailure)
		{
			AnalyticsUtil.LogSteamError(AnalyticsUtil.steamActions.query, AnalyticsUtil.steamItemTypes.mission, 0uL, qryResults.m_eResult);
			Debug.LogFormat("[MissionBriefing]: Failed to create new item on steam. I/O error.");
			if (steamCommsDialog != null)
			{
				steamCommsDialog.Dismiss();
				steamCommsDialog = null;
			}
			SteamUGC.ReleaseQueryUGCRequest(qryResults.m_handle);
			return;
		}
		if (qryResults.m_eResult == EResult.k_EResultOK)
		{
			if (qryResults.m_unNumResultsReturned != 0)
			{
				SteamUGCDetails_t uGCItemDetails = SteamManager.Instance.GetUGCItemDetails(qryResults.m_handle, 0u);
				if (uGCItemDetails.m_eResult == EResult.k_EResultFileNotFound)
				{
					SteamManager.Instance.CreateNewItem(onNewItemCreated, EWorkshopFileType.k_EWorkshopFileTypeFirst);
				}
				else
				{
					updateSteamItem(uGCItemDetails.m_nPublishedFileId);
				}
			}
			else
			{
				SteamManager.Instance.CreateNewItem(onNewItemCreated, EWorkshopFileType.k_EWorkshopFileTypeFirst);
			}
		}
		else if (qryResults.m_eResult == EResult.k_EResultNoMatch)
		{
			SteamManager.Instance.CreateNewItem(onNewItemCreated, EWorkshopFileType.k_EWorkshopFileTypeFirst);
		}
		else
		{
			if (steamCommsDialog != null)
			{
				steamCommsDialog.Dismiss();
				steamCommsDialog = null;
			}
			AnalyticsUtil.LogSteamError(AnalyticsUtil.steamActions.update, AnalyticsUtil.steamItemTypes.mission, 0uL, qryResults.m_eResult);
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("SteamError", Localizer.Format("#autoLOC_8002124", SteamManager.Instance.GetUGCFailureReason(qryResults.m_eResult)), Localizer.Format("#autoLOC_8002125"), guiSkinDef.SkinDef, 350f, new DialogGUIButton(Localizer.Format("#autoLOC_226975"), null, dismissOnSelect: true)), persistAcrossScenes: false, guiSkinDef.SkinDef);
			Debug.LogFormat("[MissionBriefing]: Failed to export mission ({0}) to steam. Reason:{1}", editorMissionReference.title, SteamManager.Instance.GetUGCFailureReason(qryResults.m_eResult));
		}
		SteamUGC.ReleaseQueryUGCRequest(qryResults.m_handle);
	}

	public void onNewItemCreated(CreateItemResult_t qryResults, bool bIOFailure)
	{
		if (bIOFailure)
		{
			AnalyticsUtil.LogSteamError(AnalyticsUtil.steamActions.create, AnalyticsUtil.steamItemTypes.mission, qryResults.m_nPublishedFileId, qryResults.m_eResult);
			Debug.LogFormat("[MissionBriefing]: Failed to create new item on steam. I/O error.");
			if (steamCommsDialog != null)
			{
				steamCommsDialog.Dismiss();
				steamCommsDialog = null;
			}
			return;
		}
		if (qryResults.m_eResult == EResult.k_EResultOK)
		{
			steamUploadingNewItem = true;
			updateSteamItem(qryResults.m_nPublishedFileId);
			return;
		}
		if (steamCommsDialog != null)
		{
			steamCommsDialog.Dismiss();
			steamCommsDialog = null;
		}
		AnalyticsUtil.LogSteamError(AnalyticsUtil.steamActions.create, AnalyticsUtil.steamItemTypes.mission, qryResults.m_nPublishedFileId, qryResults.m_eResult);
		PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("SteamError", Localizer.Format("#autoLOC_8002126", SteamManager.Instance.GetUGCFailureReason(qryResults.m_eResult)), Localizer.Format("#autoLOC_8002125"), guiSkinDef.SkinDef, 350f, new DialogGUIButton(Localizer.Format("#autoLOC_226975"), null, dismissOnSelect: true)), persistAcrossScenes: false, guiSkinDef.SkinDef);
		Debug.LogFormat("[MissionBriefing]: Failed to create new item on steam. Steam Error:{0}", SteamManager.Instance.GetUGCFailureReason(qryResults.m_eResult));
	}

	public void updateSteamItem(PublishedFileId_t fileId)
	{
		editorMissionReference.steamPublishedFileId = fileId.m_PublishedFileId;
		if (!steamUploadingNewItem)
		{
			SteamManager.Instance.GetAppDependencies(onGetAppDependency, fileId);
			return;
		}
		UpdateSteamItem(onItemUpdated, fileId, editorMissionReference.MissionInfo.FolderPath, editorMissionReference.GetBanner(MEBannerType.Menu).FullPath, editorMissionReference.title, editorMissionReference.briefing, setSteamExportItemChangeLog, SteamMissionFileInfo.CreateMetaData(editorMissionReference), new AppId_t[1] { SteamManager.MakingHistoryAppID });
	}

	public void onGetAppDependency(GetAppDependenciesResult_t result, bool bIOFailure)
	{
		if (bIOFailure)
		{
			AnalyticsUtil.LogSteamError(AnalyticsUtil.steamActions.update, AnalyticsUtil.steamItemTypes.mission, result.m_nPublishedFileId, result.m_eResult);
			Debug.LogFormat("[MissionBriefing]: Failed to Get App Dependencies to update a Steam Item. I/O error.");
			if (steamCommsDialog != null)
			{
				steamCommsDialog.Dismiss();
				steamCommsDialog = null;
			}
			return;
		}
		if (result.m_eResult == EResult.k_EResultOK)
		{
			if (result.m_nNumAppDependencies != 0)
			{
				appDependenciesToRemove = new List<AppId_t>();
				for (int i = 0; i < result.m_nNumAppDependencies; i++)
				{
					appDependenciesToRemove.Add(result.m_rgAppIDs[i]);
				}
				SteamManager.Instance.RemoveAppDependency(onAppDependencyRemoved, result.m_nPublishedFileId, appDependenciesToRemove[0]);
			}
			else
			{
				UpdateSteamItem(onItemUpdated, result.m_nPublishedFileId, editorMissionReference.MissionInfo.FolderPath, editorMissionReference.GetBanner(MEBannerType.Menu).FullPath, editorMissionReference.title, editorMissionReference.briefing, setSteamExportItemChangeLog, SteamMissionFileInfo.CreateMetaData(editorMissionReference), new AppId_t[1] { SteamManager.MakingHistoryAppID });
			}
			return;
		}
		if (steamCommsDialog != null)
		{
			steamCommsDialog.Dismiss();
			steamCommsDialog = null;
		}
		AnalyticsUtil.LogSteamError(AnalyticsUtil.steamActions.update, AnalyticsUtil.steamItemTypes.mission, result.m_nPublishedFileId, result.m_eResult);
		PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("SteamError", Localizer.Format("#autoLOC_8002128", SteamManager.Instance.GetUGCFailureReason(result.m_eResult)), Localizer.Format("#autoLOC_8002125"), guiSkinDef.SkinDef, 350f, new DialogGUIButton(Localizer.Format("#autoLOC_226975"), null, dismissOnSelect: true)), persistAcrossScenes: false, guiSkinDef.SkinDef);
		Debug.LogFormat("[MissionBriefing]: Failed to update item on steam. Unable to determine Steam App dependencies. Steam Error:{0}", SteamManager.Instance.GetUGCFailureReason(result.m_eResult));
	}

	public void onAppDependencyRemoved(RemoveAppDependencyResult_t result, bool bIOFailure)
	{
		if (bIOFailure)
		{
			AnalyticsUtil.LogSteamError(AnalyticsUtil.steamActions.update, AnalyticsUtil.steamItemTypes.mission, result.m_nPublishedFileId, result.m_eResult);
			Debug.LogFormat("[MissionBriefing]: Failed to remove App Dependency when updating an item on steam. I/O error.");
			if (steamCommsDialog != null)
			{
				steamCommsDialog.Dismiss();
				steamCommsDialog = null;
			}
			return;
		}
		if (result.m_eResult == EResult.k_EResultOK)
		{
			appDependenciesToRemove.Remove(result.m_nAppID);
			if (appDependenciesToRemove.Count == 0)
			{
				UpdateSteamItem(onItemUpdated, result.m_nPublishedFileId, editorMissionReference.MissionInfo.FolderPath, editorMissionReference.GetBanner(MEBannerType.Menu).FullPath, editorMissionReference.title, editorMissionReference.briefing, setSteamExportItemChangeLog, SteamMissionFileInfo.CreateMetaData(editorMissionReference), new AppId_t[1] { SteamManager.MakingHistoryAppID });
			}
			else
			{
				SteamManager.Instance.RemoveAppDependency(onAppDependencyRemoved, result.m_nPublishedFileId, appDependenciesToRemove[0]);
			}
			return;
		}
		if (steamCommsDialog != null)
		{
			steamCommsDialog.Dismiss();
			steamCommsDialog = null;
		}
		AnalyticsUtil.LogSteamError(AnalyticsUtil.steamActions.update, AnalyticsUtil.steamItemTypes.mission, result.m_nPublishedFileId, result.m_eResult);
		PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("SteamError", Localizer.Format("#autoLOC_8002128", SteamManager.Instance.GetUGCFailureReason(result.m_eResult)), Localizer.Format("#autoLOC_8002125"), guiSkinDef.SkinDef, 350f, new DialogGUIButton(Localizer.Format("#autoLOC_226975"), null, dismissOnSelect: true)), persistAcrossScenes: false, guiSkinDef.SkinDef);
		Debug.LogFormat("[MissionBriefing]: Failed to update item on steam. Unable to remove Steam App dependencies. Steam Error:{0}", SteamManager.Instance.GetUGCFailureReason(result.m_eResult));
	}

	public void UpdateSteamItem(Callback<SubmitItemUpdateResult_t, bool> onCompleted, PublishedFileId_t fileId, string contentURL, string previewURL, string title, string description, string changeNote, string metaData, AppId_t[] appDependencies = null)
	{
		List<string> list = new List<string>();
		list.Add("Mission");
		ERemoteStoragePublishedFileVisibility visibility = ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPrivate;
		if (setSteamExportItemPublic)
		{
			visibility = ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic;
		}
		SteamManager.Instance.UpdateItem(onCompleted, fileId, list, contentURL, previewURL, title, description, changeNote, visibility, metaData, appDependencies);
	}

	public void onItemUpdated(SubmitItemUpdateResult_t updateResult, bool bIOFailure)
	{
		if (bIOFailure)
		{
			AnalyticsUtil.LogSteamError(AnalyticsUtil.steamActions.update, AnalyticsUtil.steamItemTypes.mission, updateResult.m_nPublishedFileId, updateResult.m_eResult);
			Debug.LogFormat("[MissionBriefing]: Failed to create new item on steam. I/O error.");
			if (steamCommsDialog != null)
			{
				steamCommsDialog.Dismiss();
				steamCommsDialog = null;
			}
			return;
		}
		if (updateResult.m_eResult == EResult.k_EResultOK)
		{
			if (steamUploadingNewItem)
			{
				AnalyticsUtil.LogSteamItemCreated(AnalyticsUtil.steamItemTypes.mission, updateResult.m_nPublishedFileId);
			}
			else
			{
				AnalyticsUtil.LogSteamItemUpdated(AnalyticsUtil.steamItemTypes.mission, updateResult.m_nPublishedFileId);
			}
			Debug.LogFormat("[MissionBriefing]: Item ({0}) has been successfully Updated to Steam Workshop", updateResult.m_nPublishedFileId);
			MissionEditorLogic.Instance.SaveMission(null);
			if (steamCommsDialog != null)
			{
				steamCommsDialog.Dismiss();
				steamCommsDialog = null;
			}
			if (updateResult.m_bUserNeedsToAcceptWorkshopLegalAgreement)
			{
				SteamWorkshopExportDialog.Spawn(Localizer.Format("#autoLOC_8002121"), Localizer.Format("#autoLOC_8002127"), "", Localizer.Format("#autoLOC_226975"), "", delegate
				{
					SteamManager.Instance.OpenSteamOverlayToWorkshopItem(updateResult.m_nPublishedFileId);
				}, null, showCancelBtn: false, showVisibilityOption: false, showChangeLog: false);
			}
			else
			{
				SteamManager.Instance.OpenSteamOverlayToWorkshopItem(updateResult.m_nPublishedFileId);
			}
		}
		else
		{
			if (steamCommsDialog != null)
			{
				steamCommsDialog.Dismiss();
				steamCommsDialog = null;
			}
			AnalyticsUtil.LogSteamError(AnalyticsUtil.steamActions.update, AnalyticsUtil.steamItemTypes.mission, updateResult.m_nPublishedFileId, updateResult.m_eResult);
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("SteamError", Localizer.Format("#autoLOC_8002128", SteamManager.Instance.GetUGCFailureReason(updateResult.m_eResult)), Localizer.Format("#autoLOC_8002125"), guiSkinDef.SkinDef, 350f, new DialogGUIButton(Localizer.Format("#autoLOC_226975"), null, dismissOnSelect: true)), persistAcrossScenes: false, guiSkinDef.SkinDef);
			Debug.LogFormat("[MissionBriefing]: Failed to Update item ({0}) on steam. Steam Error:{1}", updateResult.m_nPublishedFileId, SteamManager.Instance.GetUGCFailureReason(updateResult.m_eResult));
		}
		steamUploadingNewItem = false;
	}

	public void ExportMissionNameConfirmed()
	{
		UpdateMissionDetails();
		MissionEditorLogic.Instance.SaveMission(OnExitAfterSave);
	}

	public void ExportMissionNameCancelled()
	{
	}

	public void OnExitAfterSave()
	{
		string sanitisedMissionExportName = KSPUtil.SanitizeFilename(editorMissionReference.exportName) + ".zip";
		if (File.Exists(MissionsUtils.MissionExportsPath + sanitisedMissionExportName))
		{
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("ConfirmFileOverwrite", Localizer.Format("#autoLOC_8200066", sanitisedMissionExportName), Localizer.Format("#autoLOC_8200067"), HighLogic.UISkin, 350f, new DialogGUISpace(6f), new DialogGUIButton(Localizer.Format("#autoLOC_127867"), delegate
			{
				ExportMissionAfterChecksPassed(sanitisedMissionExportName);
			}), new DialogGUIButton(Localizer.Format("#autoLOC_8200069"), delegate
			{
			})), persistAcrossScenes: false, null);
		}
		else
		{
			ExportMissionAfterChecksPassed(sanitisedMissionExportName);
		}
	}

	public void ExportMissionAfterChecksPassed(string exportFileName)
	{
		editorMissionReference.Export(exportFileName, overwrite: true);
	}

	public void SpawnNoMissionNameDialogue()
	{
		PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("MissionNameEmpty", Localizer.Format("#autoLOC_8200070"), Localizer.Format("#autoLOC_8200071"), HighLogic.UISkin, new DialogGUISpace(6f), new DialogGUIButton(Localizer.Format("#autoLOC_418748"), delegate
		{
		})), persistAcrossScenes: false, null);
	}

	public void DestroyDialog()
	{
		difficultySlider.onValueChanged.RemoveListener(OnDifficultySliderChanged);
		UIMasterController.Instance.UnregisterNonModalDialog(GetComponent<CanvasGroup>());
		UnityEngine.Object.Destroy(base.gameObject);
		editorMissionReference = null;
	}

	public void OnTabInfoPress(bool state)
	{
		if (state)
		{
			SetTab(TabEnum.Info);
		}
	}

	public void OnTabValidationPress(bool state)
	{
		if (state)
		{
			SetTab(TabEnum.Objectives);
		}
	}

	public void OnTabScorePress(bool state)
	{
		if (state)
		{
			SetTab(TabEnum.Score);
		}
	}

	public void OnTabAwardsPress(bool state)
	{
		if (state)
		{
			SetTab(TabEnum.Awards);
		}
	}

	public void SetTab(TabEnum newTab)
	{
		if (currentTab != newTab)
		{
			SetTabVisible(currentTab, state: false);
			tabGroup.SetAllTogglesOff();
			currentTab = newTab;
			SetTabVisible(currentTab, state: true);
			SetTabCaption(currentTab);
		}
	}

	public void SetTabVisible(TabEnum tab, bool state)
	{
		switch (currentTab)
		{
		case TabEnum.Info:
			tabInfo.isOn = state;
			panelInfo.gameObject.SetActive(state);
			break;
		case TabEnum.Objectives:
			tabObjectives.isOn = state;
			panelObjectives.gameObject.SetActive(state);
			break;
		case TabEnum.Score:
			tabScore.isOn = state;
			panelScore.SetActive(state);
			break;
		case TabEnum.Awards:
			tabAwards.isOn = state;
			panelAwards.SetActive(state);
			break;
		}
	}

	public void SetTabCaption(TabEnum tab)
	{
		switch (tab)
		{
		default:
			tabCaption.text = "";
			break;
		case TabEnum.Info:
			tabCaption.text = Localizer.Format("#autoLOC_8200073");
			break;
		case TabEnum.Objectives:
			tabCaption.text = Localizer.Format("#autoLOC_8200074");
			break;
		case TabEnum.Score:
			tabCaption.text = Localizer.Format("#autoLOC_8200075");
			break;
		case TabEnum.Awards:
			tabCaption.text = Localizer.Format("#autoLOC_8200076");
			break;
		}
	}

	public void OnEndEdit(string s)
	{
		OnAddTag();
		addCustomTagText.ActivateInputField();
		addCustomTagText.Select();
	}

	public void OnAddTag()
	{
		if (addCustomTagText.placeholder.isActiveAndEnabled)
		{
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("InvalidTagValue", Localizer.Format("#autoLOC_8200077"), Localizer.Format("#autoLOC_8200078"), HighLogic.UISkin, 350f, new DialogGUISpace(6f), new DialogGUIButton(Localizer.Format("#autoLOC_418748"), delegate
			{
			})), persistAcrossScenes: false, null);
			return;
		}
		int num = 0;
		while (true)
		{
			if (num < missionBriefingTagList.Count)
			{
				if (missionBriefingTagList[num].tagText.text == addCustomTagText.text)
				{
					break;
				}
				num++;
				continue;
			}
			SpawnNewTagPrefab(addCustomTagText.text);
			addCustomTagText.text = "";
			return;
		}
		PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("DuplicatedTagValue", Localizer.Format("#autoLOC_8200079", addCustomTagText.text), Localizer.Format("#autoLOC_8200080"), HighLogic.UISkin, 350f, new DialogGUISpace(6f), new DialogGUIButton(Localizer.Format("#autoLOC_418748"), delegate
		{
		})), persistAcrossScenes: false, null);
		addCustomTagText.text = "";
	}

	public void SpawnNewTagPrefab(string missionTag)
	{
		MissionBriefingTag missionBriefingTag = this.missionBriefingTag.Spawn(missionTag);
		missionBriefingTag.transform.SetParent(tagLayoutGroup.transform);
		missionBriefingTag.transform.localScale = new Vector3(1f, 1f, 1f);
		missionBriefingTag.transform.localPosition = Vector3.zero;
		missionBriefingTagList.Add(missionBriefingTag);
	}

	public void SetupGlobalScore()
	{
		FieldInfo field = GetType().GetField("globalScore", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		MEGUI_DynamicModuleList[] array = (MEGUI_DynamicModuleList[])field.GetCustomAttributes(typeof(MEGUI_DynamicModuleList), inherit: true);
		if (array == null || array.Length == 0)
		{
			return;
		}
		BaseAPField field2 = new BaseAPField(array[0], field, this);
		MEGUIParameter mEGUIParameter = scoreParameterPrefab;
		if (mEGUIParameter != null)
		{
			globalScoreParameter = mEGUIParameter.Create(field2, globalScoreContentRoot) as MEGUIParameterDynamicModuleList;
			Canvas[] componentsInChildren = globalScoreParameter.GetComponentsInChildren<Canvas>(includeInactive: true);
			int num = componentsInChildren.Length;
			while (num-- > 0)
			{
				componentsInChildren[num].sortingLayerName = "Dialogs";
			}
		}
	}

	public void SetupAwards()
	{
		awards.FillAwardsList();
		FieldInfo field = GetType().GetField("awards", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		MEGUI_DynamicModuleList[] array = (MEGUI_DynamicModuleList[])field.GetCustomAttributes(typeof(MEGUI_DynamicModuleList), inherit: true);
		if (array != null && array.Length != 0)
		{
			BaseAPField field2 = new BaseAPField(array[0], field, this);
			MEGUIParameter mEGUIParameter = awardsParameterPrefab;
			if (mEGUIParameter != null)
			{
				awardsParameter = mEGUIParameter.Create(field2, awardsContentRoot) as MEGUIParameterDynamicModuleList;
				Canvas[] componentsInChildren = awardsParameter.GetComponentsInChildren<Canvas>(includeInactive: true);
				int num = componentsInChildren.Length;
				while (num-- > 0)
				{
					componentsInChildren[num].sortingLayerName = "Dialogs";
				}
			}
		}
		int count = awards.activeModules.Count;
		int num2 = 0;
		while (count-- > 0 || num2 < 3)
		{
			if (awards.activeModules[count].GetType() == typeof(AwardModule_Score))
			{
				scoreAwards[num2++].SetupAwardModule(awards.activeModules[count] as AwardModule);
			}
		}
	}

	public void OnOpenflag()
	{
		browser = UnityEngine.Object.Instantiate(FlagBrowserPrefab).GetComponent<FlagBrowser>();
		browser.OnDismiss = OnFlagCancel;
		browser.OnFlagSelected = OnFlagSelect;
		base.gameObject.SetActive(value: false);
	}

	public void OnFlagSelect(FlagBrowser.FlagEntry selected)
	{
		flagURL = selected.textureInfo.name;
		GameEvents.onFlagSelect.Fire(selected.textureInfo.name);
		btnFlagImage.texture = selected.textureInfo.texture;
		base.gameObject.SetActive(value: true);
	}

	public void OnFlagCancel()
	{
		base.gameObject.SetActive(value: true);
	}

	public void OnOpenBannerMenu()
	{
		OpenBannerSelector(MEBannerType.Menu);
	}

	public void OnOpenBannerSuccess()
	{
		OpenBannerSelector(MEBannerType.Success);
	}

	public void OnOpenBannerFail()
	{
		OpenBannerSelector(MEBannerType.Fail);
	}

	public void OpenBannerSelector(MEBannerType bannerType)
	{
		bannerBrowser = new GameObject().AddComponent<MEBannerBrowser>();
		bannerBrowser.Setup(bannerType);
		bannerBrowser.OnDismiss = OnBannerCancel;
		bannerBrowser.OnBannerSelected = OnBannerSelected;
		bannerBrowser.OnBannerDeleted = OnBannerDeleted;
		base.gameObject.SetActive(value: false);
	}

	public void OnBannerCancel()
	{
		base.gameObject.SetActive(value: true);
	}

	public void OnBannerSelected(MEBannerEntry banner, MEBannerType bannerType)
	{
		base.gameObject.SetActive(value: true);
		SetTempBanner(banner, bannerType);
		RefreshBanners();
	}

	public void OnBannerDeleted(MEBannerEntry banner, MEBannerType bannerType)
	{
		MEBannerEntry tempBanner = null;
		GetTempBanner(ref tempBanner, bannerType);
		if (tempBanner != null && banner.FullPath == tempBanner.FullPath)
		{
			tempBanner.SetToDefault(bannerType);
			RefreshBanners();
		}
	}

	public void SetTempBanner(MEBannerEntry newBanner, MEBannerType bannerType)
	{
		switch (bannerType)
		{
		case MEBannerType.Menu:
			tempBannerMenu = newBanner;
			break;
		case MEBannerType.Success:
			tempBannerSuccess = newBanner;
			break;
		case MEBannerType.Fail:
			tempBannerFail = newBanner;
			break;
		}
	}

	public void GetTempBanner(ref MEBannerEntry tempBanner, MEBannerType bannerType)
	{
		switch (bannerType)
		{
		case MEBannerType.Menu:
			tempBanner = tempBannerMenu;
			break;
		case MEBannerType.Success:
			tempBanner = tempBannerSuccess;
			break;
		case MEBannerType.Fail:
			tempBanner = tempBannerFail;
			break;
		}
	}

	public void OnModsList()
	{
		TMP_InputField tMP_InputField = modsText;
		tMP_InputField.text = tMP_InputField.text + "\n" + Localizer.Format("#autoLOC_8001027");
		int i = 0;
		for (int count = GameDatabase.loadedModsInfo.Count; i < count; i++)
		{
			TMP_InputField tMP_InputField2 = modsText;
			tMP_InputField2.text = tMP_InputField2.text + "\n" + GameDatabase.loadedModsInfo[i];
		}
	}

	public void OnDifficultySliderChanged(float value)
	{
		int num = (int)value;
		if (Enum.IsDefined(typeof(MissionDifficultyWithColor), num))
		{
			difficultyLabel.text = ((MissionDifficultyWithColor)num).displayDescription();
			difficultyHead.sprite = difficultyHeads[num];
		}
	}
}
