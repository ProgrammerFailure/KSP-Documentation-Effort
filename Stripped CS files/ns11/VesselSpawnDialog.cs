using System.Collections;
using System.Collections.Generic;
using System.IO;
using Expansions;
using ns2;
using ns9;
using PreFlightTests;
using SaveUpgradePipeline;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace ns11;

public class VesselSpawnDialog : MonoBehaviour
{
	public delegate void CraftSelectedCallback(string fullFilename, string flagURL, VesselCrewManifest manifest);

	public enum ActiveToggle
	{
		TabSteam,
		TabVessels
	}

	public enum SteamQueryFilters
	{
		VOTE,
		FEATURED,
		NEWEST,
		SUBSCRIBERS
	}

	public struct ListItem
	{
		public string name;

		public float mass;

		public float cost;

		public float size;

		public ListItem(string name, float mass, float cost, float size)
		{
			this.name = name;
			this.mass = mass;
			this.cost = cost;
			this.size = size;
		}
	}

	public class VesselDataItem
	{
		public bool stock;

		public string name;

		public string description;

		public static string defaultDescription = string.Empty;

		public string fullFilePath;

		public ConfigNode _configNode;

		public bool steamItem;

		public int parts;

		public int stages;

		public VersionCompareResult compatibility;

		public UIListItem listItem;

		public bool isValid;

		public bool isExperimental;

		public ShipTemplate _template;

		public string thumbURL;

		public Texture2D thumbnail;

		public CraftProfileInfo craftProfileInfo;

		public ConfigNode configNode
		{
			get
			{
				if (_configNode == null)
				{
					_configNode = ConfigNode.Load(fullFilePath);
				}
				return _configNode;
			}
		}

		public ShipTemplate template
		{
			get
			{
				if (_template == null)
				{
					_template = new ShipTemplate();
					_template.LoadShip(configNode);
				}
				return _template;
			}
		}

		public VesselDataItem(FileInfo fInfo, bool stock, bool steamItem)
		{
			this.stock = stock;
			this.steamItem = steamItem;
			fullFilePath = fInfo.FullName;
			string text = fInfo.FullName.Replace(fInfo.Extension, ".loadmeta");
			if (steamItem)
			{
				text = KSPSteamUtils.GetSteamCacheLocation(text);
			}
			craftProfileInfo = CraftProfileInfo.GetSaveData(fullFilePath, text);
			isValid = craftProfileInfo.shipPartsUnlocked;
			isExperimental = craftProfileInfo.shipPartsExperimental;
			parts = craftProfileInfo.partCount;
			stages = craftProfileInfo.stageCount;
			compatibility = craftProfileInfo.compatibility;
			name = Localizer.Format(craftProfileInfo.shipName);
			if (stock)
			{
				name += Localizer.Format("#autoLOC_482705");
			}
			description = craftProfileInfo.description;
			if (description == null || description == string.Empty)
			{
				if (defaultDescription == string.Empty)
				{
					defaultDescription = Localizer.Format("#autoLOC_8004254");
				}
				description = defaultDescription;
			}
			string text2 = new FileInfo(fullFilePath).Directory.Name;
			if (stock)
			{
				thumbURL = "Ships/@thumbs/" + text2 + "/" + KSPUtil.SanitizeFilename(Path.GetFileNameWithoutExtension(fullFilePath));
			}
			else
			{
				thumbURL = "thumbs/" + HighLogic.SaveFolder + "_" + text2 + "_" + KSPUtil.SanitizeFilename(Path.GetFileNameWithoutExtension(fullFilePath));
			}
			if (steamItem)
			{
				thumbURL = fInfo.DirectoryName + "/" + Path.GetFileNameWithoutExtension(fullFilePath);
				thumbnail = ShipConstruction.GetThumbnail(thumbURL, fullPath: true, addFileExt: true);
				name = name + " (" + Localizer.Format("#autoLOC_8002139") + ")";
			}
			else if (fullFilePath.Contains("SquadExpansion"))
			{
				List<ExpansionsLoader.ExpansionInfo> installedExpansions = ExpansionsLoader.GetInstalledExpansions();
				string text3 = "";
				for (int i = 0; i < installedExpansions.Count; i++)
				{
					text3 = KSPExpansionsUtils.ExpansionsGameDataPath + installedExpansions[i].FolderName + "/" + thumbURL;
					if (fullFilePath.Contains(installedExpansions[i].FolderName))
					{
						thumbnail = ShipConstruction.GetThumbnail(text3, fullPath: true, addFileExt: true);
					}
				}
			}
			else
			{
				thumbnail = ShipConstruction.GetThumbnail(thumbURL);
			}
		}

		public ulong GetSteamFileId()
		{
			if (craftProfileInfo != null)
			{
				return craftProfileInfo.steamPublishedFileId;
			}
			ulong value = 0uL;
			if (configNode != null)
			{
				configNode.TryGetValue("steamPublishedFileId", ref value);
				if (value == 0L)
				{
					value = KSPSteamUtils.GetSteamIDFromSteamFolder(fullFilePath);
				}
			}
			return value;
		}
	}

	public VesselSpawnSearch vesselSpawnSearch;

	public UIListSorter vesselListSorter;

	[SerializeField]
	public UIListItem listItemPrefab;

	public RectTransform showHodeTransform;

	public Transform vesselListAnchor;

	public ToggleGroup vesselListToggleGroup;

	[SerializeField]
	public FlagBrowserButton flagButtonController;

	[SerializeField]
	public RectTransform crewMgmtArea;

	[SerializeField]
	public RectTransform panelRightInfo;

	[SerializeField]
	public RectTransform panelRightSteamInfo;

	[SerializeField]
	public RectTransform tabVesselList;

	[SerializeField]
	public RectTransform tabSteam;

	[SerializeField]
	public RectTransform listSorting;

	[SerializeField]
	public RectTransform tabSteamToggles;

	[SerializeField]
	public RawImage shipThumbnail;

	[SerializeField]
	public Button buttonDelete;

	[SerializeField]
	public Button buttonLaunch;

	[SerializeField]
	public Button buttonEdit;

	[SerializeField]
	public Button buttonClose;

	[SerializeField]
	public GameObject launchSiteSelector;

	[SerializeField]
	public GameObject vesselListPanelFullTab;

	[SerializeField]
	public TextMeshProUGUI SteamLoadingText;

	[SerializeField]
	public TextMeshProUGUI btnSteamSubUnsubText;

	[SerializeField]
	public TextMeshProUGUI noSteamItemSelectedText;

	[SerializeField]
	public GameObject steamItemContent;

	[SerializeField]
	public RawImage steamItemPreview;

	[SerializeField]
	public Button btnSteamOverlay;

	[SerializeField]
	public Button btnSteamSubUnsub;

	[SerializeField]
	public Button btnSteamItem;

	[SerializeField]
	public TextMeshProUGUI steamItemKSPVersionText;

	[SerializeField]
	public TextMeshProUGUI steamItemVesselTypeText;

	[SerializeField]
	public TextMeshProUGUI steamItemDescriptionText;

	[SerializeField]
	public TextMeshProUGUI steamItemAuthorText;

	[SerializeField]
	public TextMeshProUGUI steamItemFavouriteText;

	[SerializeField]
	public TextMeshProUGUI steamItemModsText;

	[SerializeField]
	public TextMeshProUGUI steamItemFileSizeText;

	[SerializeField]
	public TextMeshProUGUI steamItemUpVotesText;

	[SerializeField]
	public TextMeshProUGUI steamItemDownVotesText;

	[SerializeField]
	public TextMeshProUGUI steamItemSubscribersText;

	[SerializeField]
	public Toggle SteamFilterToggleOne;

	[SerializeField]
	public Toggle SteamFilterToggleTwo;

	[SerializeField]
	public Toggle SteamFilterToggleThree;

	[SerializeField]
	public Toggle SteamFilterToggleFour;

	[SerializeField]
	public GameObject craftImageSteamOverlay;

	[SerializeField]
	public TextMeshProUGUI vesselName;

	[SerializeField]
	public TextMeshProUGUI steamItemInfovesselName;

	[SerializeField]
	public TextMeshProUGUI vesselDescription;

	public List<VesselDataItem> vesselDataItemList = new List<VesselDataItem>();

	public UIList<ListItem> scrollList;

	public string flagURL;

	public VesselCrewManifest crewManifest;

	public CraftSelectedCallback OnFileSelected;

	public VesselDataItem selectedDataItem;

	public string siteName;

	public EditorFacility facility;

	public LaunchSiteFacility launchSiteFacility;

	public ActiveToggle activeToggle = ActiveToggle.TabVessels;

	[SerializeField]
	public PublishedFileId_t[] steamSubscribedItems;

	public SteamQueryFilters selectedSteamQueryFilter;

	public List<CraftEntry> craftList;

	public CraftEntry selectedEntry;

	public Toggle tabSteamToggle;

	public bool launchSiteSelectorIsAvailable;

	public Button btnCrewSteamSubUnsub;

	public TextMeshProUGUI crewSteamSubUnsubText;

	public static float maxFrameTime = 0.25f;

	public Coroutine createVesselCoroutine;

	public string craftSubfolder;

	public string profileName;

	public int sortButton;

	public bool sortAsc;

	public EditorFacility editorFacility;

	public static VesselSpawnDialog Instance { get; set; }

	public bool Visible { get; set; }

	public bool HasSearchText
	{
		get
		{
			if (vesselSpawnSearch != null)
			{
				return vesselSpawnSearch.HasSearchText;
			}
			return false;
		}
	}

	public void Awake()
	{
		if (Instance != null)
		{
			Debug.LogError("VesselSpawnDialog: Instance already exists.");
			base.gameObject.DestroyGameObject();
			return;
		}
		Instance = this;
		if ((bool)GetComponent<UIRectScaler>())
		{
			GetComponent<UIRectScaler>().enabled = true;
		}
		GameEvents.onGUIAstronautComplexSpawn.Add(AstronautComplexSpawn);
		GameEvents.onGUIAstronautComplexDespawn.Add(AstronautComplexDespawn);
		vesselSpawnSearch = GetComponent<VesselSpawnSearch>();
		scrollList = new UIList<ListItem>(vesselListAnchor);
	}

	public void Start()
	{
		if (SteamManager.Initialized)
		{
			Transform transform = crewMgmtArea.transform.Find("CrewAssignmentDialog(Clone)/VL 1/Buttons/buttonSteamSubUnsub");
			if (transform != null)
			{
				btnCrewSteamSubUnsub = transform.GetComponent<Button>();
				if (btnCrewSteamSubUnsub != null)
				{
					btnCrewSteamSubUnsub.onClick.AddListener(onButtonSteamSubUnsub);
					crewSteamSubUnsubText = transform.GetComponentInChildren<TextMeshProUGUI>();
				}
			}
		}
		vesselListSorter.AddOnSortCallback(OnVesselListSort);
		sortButton = vesselListSorter.StartSortingIndex;
		sortAsc = vesselListSorter.startSortingAsc;
		OnVesselListSort(vesselListSorter.StartSortingIndex, vesselListSorter.startSortingAsc);
		Texture2D texture = GameDatabase.Instance.GetTexture(HighLogic.CurrentGame.flagURL, asNormalMap: false);
		flagButtonController.Setup(texture, delegate
		{
		}, OnFlagSelected, delegate
		{
		});
		buttonLaunch.onClick.AddListener(ButtonLaunch);
		buttonDelete.onClick.AddListener(ButtonDelete);
		buttonEdit.onClick.AddListener(ButtonEdit);
		buttonClose.onClick.AddListener(ButtonClose);
		if (SteamManager.Initialized)
		{
			vesselListPanelFullTab.SetActive(value: true);
			tabSteam.gameObject.SetActive(value: true);
			tabSteamToggle = tabSteam.gameObject.GetComponent<Toggle>();
			tabSteamToggle.interactable = true;
			tabSteamToggle.isOn = false;
			tabSteamToggle.onValueChanged.AddListener(onTabSteamToggle);
			Toggle component = tabVesselList.gameObject.GetComponent<Toggle>();
			component.interactable = true;
			component.onValueChanged.AddListener(onTabVesselListToggle);
			component.isOn = true;
			btnSteamSubUnsub.onClick.AddListener(onButtonSteamSubUnsub);
			SteamFilterToggleOne.onValueChanged.AddListener(onSteamFilterOne);
			SteamFilterToggleTwo.onValueChanged.AddListener(onSteamFilterTwo);
			SteamFilterToggleThree.onValueChanged.AddListener(onSteamFilterThree);
			SteamFilterToggleFour.onValueChanged.AddListener(onSteamFilterFour);
			btnSteamOverlay.onClick.AddListener(OnBtnSteamOverlay);
			btnSteamItem.onClick.AddListener(onBtnSteamItemDetails);
			UpdateSteamSubscribedItems();
		}
		launchSiteSelectorIsAvailable = launchSiteSelector.activeInHierarchy;
	}

	public void OnDestroy()
	{
		if (createVesselCoroutine != null)
		{
			StopCoroutine(createVesselCoroutine);
		}
		GameEvents.onGUIAstronautComplexSpawn.Remove(AstronautComplexSpawn);
		GameEvents.onGUIAstronautComplexDespawn.Remove(AstronautComplexDespawn);
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void AstronautComplexSpawn()
	{
		showHodeTransform.gameObject.SetActive(value: false);
	}

	public void AstronautComplexDespawn()
	{
		showHodeTransform.gameObject.SetActive(value: true);
	}

	public void InitiateGUI(GameEvents.VesselSpawnInfo info)
	{
		siteName = info.callingFacility.launchSiteName;
		facility = (EditorDriver.editorFacility = info.callingFacility.facilityType);
		launchSiteFacility = info.callingFacility;
		profileName = info.profileName;
		createVesselCoroutine = StartCoroutine(CreateVesselList(info.craftSubfolder, profileName));
		OnFileSelected = info.OnFileSelected;
		if (GameDatabase.Instance != null)
		{
			if (HighLogic.CurrentGame != null)
			{
				flagURL = HighLogic.CurrentGame.flagURL;
				flagButtonController.SetFlag(GameDatabase.Instance.GetTexture(HighLogic.CurrentGame.flagURL, asNormalMap: false));
			}
			else
			{
				Debug.LogError("[Craft Browser Error]: Flag Browser is enabled, but HighLogic has no currentGame!");
			}
		}
		else
		{
			Debug.LogError("[Craft Browser Error]: Flag Browser is enabled, but GameDatabase has no running instance!");
		}
		Visible = true;
	}

	public void onTabSteamToggle(bool toggle)
	{
		if (toggle && activeToggle != 0)
		{
			panelRightInfo.gameObject.SetActive(value: false);
			panelRightSteamInfo.gameObject.SetActive(value: true);
			listSorting.gameObject.SetActive(value: false);
			tabSteamToggles.gameObject.SetActive(value: true);
			if (launchSiteSelectorIsAvailable)
			{
				launchSiteSelector.SetActive(value: false);
			}
			activeToggle = ActiveToggle.TabSteam;
			buttonLaunch.interactable = false;
			buttonDelete.interactable = false;
			buttonEdit.interactable = false;
			BuildSteamCraftList();
		}
	}

	public void onTabVesselListToggle(bool toggle)
	{
		if (toggle && activeToggle != ActiveToggle.TabVessels)
		{
			panelRightInfo.gameObject.SetActive(value: true);
			panelRightSteamInfo.gameObject.SetActive(value: false);
			listSorting.gameObject.SetActive(value: true);
			tabSteamToggles.gameObject.SetActive(value: false);
			if (launchSiteSelectorIsAvailable)
			{
				launchSiteSelector.SetActive(value: true);
			}
			activeToggle = ActiveToggle.TabVessels;
			ClearCraftList();
			craftList = new List<CraftEntry>();
			if (createVesselCoroutine != null)
			{
				StopCoroutine(createVesselCoroutine);
			}
			createVesselCoroutine = StartCoroutine(CreateVesselList(craftSubfolder, profileName));
		}
	}

	public void UpdateSteamSubscribedItems()
	{
		if (SteamManager.Initialized)
		{
			uint numberSubscribedItems = SteamManager.Instance.GetNumberSubscribedItems();
			steamSubscribedItems = SteamManager.Instance.GetSubscribedItems(numberSubscribedItems);
		}
	}

	public bool SteamItemSubscribed(ulong fileId)
	{
		int num = 0;
		while (true)
		{
			if (num < steamSubscribedItems.Length)
			{
				if (steamSubscribedItems[num].m_PublishedFileId == fileId)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public void UpdateSteamListOfCraft(SteamQueryFilters selectedQueryType)
	{
		scrollList.Clear(destroyElements: true);
		vesselDataItemList.Clear();
		ClearCraftList();
		craftList = new List<CraftEntry>();
		EUGCQuery eUGCQuery = EUGCQuery.k_EUGCQuery_RankedByVote;
		string[] tags = new string[1] { "Craft" };
		switch (selectedQueryType)
		{
		default:
			eUGCQuery = EUGCQuery.k_EUGCQuery_RankedByVote;
			break;
		case SteamQueryFilters.FEATURED:
			eUGCQuery = EUGCQuery.k_EUGCQuery_RankedByVote;
			tags = new string[2] { "Craft", "Featured" };
			break;
		case SteamQueryFilters.NEWEST:
			eUGCQuery = EUGCQuery.k_EUGCQuery_RankedByPublicationDate;
			break;
		case SteamQueryFilters.SUBSCRIBERS:
			eUGCQuery = EUGCQuery.k_EUGCQuery_RankedByTotalUniqueSubscriptions;
			break;
		}
		getSteamWorkshopCraft(eUGCQuery, tags);
	}

	public void getSteamWorkshopCraft(EUGCQuery queryType, string[] tags)
	{
		SteamManager.Instance.QueryUGCItems(onSteamQueryItemsCallback, queryType, EUGCMatchingUGCType.k_EUGCMatchingUGCType_Items_ReadyToUse, tags, 1u, returnMetadata: true);
	}

	public void onSteamQueryItemsCallback(SteamUGCQueryCompleted_t results, bool bIOFailure)
	{
		if (bIOFailure && results.m_eResult == EResult.k_EResultOK)
		{
			AnalyticsUtil.LogSteamError(AnalyticsUtil.steamActions.query, AnalyticsUtil.steamItemTypes.craft, 0uL, results.m_eResult);
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("SteamError", Localizer.Format("#autoLOC_8002168", SteamManager.Instance.GetUGCFailureReason(results.m_eResult)), Localizer.Format("#autoLOC_8002125"), HighLogic.UISkin, 350f, new DialogGUIButton(Localizer.Format("#autoLOC_226975"), null, dismissOnSelect: true)), persistAcrossScenes: false, HighLogic.UISkin);
			SteamUGC.ReleaseQueryUGCRequest(results.m_handle);
			return;
		}
		SteamLoadingText.gameObject.SetActive(value: false);
		for (int i = 0; i < results.m_unNumResultsReturned; i++)
		{
			SteamUGCDetails_t uGCItemDetails = SteamManager.Instance.GetUGCItemDetails(results.m_handle, (uint)i);
			if (uGCItemDetails.m_rgchTags.Contains("Craft") && (selectedSteamQueryFilter != SteamQueryFilters.FEATURED || uGCItemDetails.m_rgchTags.Contains("Featured")))
			{
				SteamCraftInfo steamCraftInfo = new SteamCraftInfo(uGCItemDetails);
				SteamUGC.GetQueryUGCStatistic(results.m_handle, (uint)i, EItemStatistic.k_EItemStatistic_NumFavorites, out steamCraftInfo.totalFavorites);
				SteamUGC.GetQueryUGCStatistic(results.m_handle, (uint)i, EItemStatistic.k_EItemStatistic_NumFollowers, out steamCraftInfo.totalFollowers);
				SteamUGC.GetQueryUGCStatistic(results.m_handle, (uint)i, EItemStatistic.k_EItemStatistic_NumSubscriptions, out steamCraftInfo.totalSubscriptions);
				SteamUGC.GetQueryUGCPreviewURL(results.m_handle, (uint)i, out steamCraftInfo.previewURL, (uint)uGCItemDetails.m_nPreviewFileSize);
				string pchMetadata = "";
				SteamUGC.GetQueryUGCMetadata(results.m_handle, (uint)i, out pchMetadata, 5000u);
				steamCraftInfo.ProcessMetaData(pchMetadata);
				steamCraftInfo.UpdateSteamState();
				CraftEntry item = CraftEntry.Create(null, stock: false, OnEntrySelected, steamItem: true, steamCraftInfo);
				craftList.Add(item);
			}
		}
		int count = craftList.Count;
		for (int j = 0; j < count; j++)
		{
			AddCraftEntryWidget(craftList[j], vesselListAnchor.gameObject.GetComponent<RectTransform>());
		}
		OnSelectionChanged(null);
		SteamUGC.ReleaseQueryUGCRequest(results.m_handle);
		base.gameObject.SendMessage("SteamCraftListReceived", SendMessageOptions.DontRequireReceiver);
	}

	public void BuildSteamCraftList()
	{
		scrollList.Clear(destroyElements: true);
		vesselDataItemList.Clear();
		craftList = new List<CraftEntry>();
		UpdateSteamSubscribedItems();
		UpdateSteamListOfCraft(selectedSteamQueryFilter);
	}

	public void onButtonSteamSubUnsub()
	{
		ulong fileId = 0uL;
		if (tabSteamToggle.isOn && selectedEntry != null)
		{
			fileId = selectedEntry.GetSteamFileId();
		}
		else if (!tabSteamToggle.isOn && selectedDataItem != null)
		{
			fileId = selectedDataItem.GetSteamFileId();
		}
		if (fileId == 0L)
		{
			return;
		}
		if (SteamItemSubscribed(fileId))
		{
			UIConfirmDialog.Spawn(Localizer.Format("#autoLOC_8002132"), Localizer.Format("#autoLOC_8002133"), Localizer.Format("#autoLOC_226976"), Localizer.Format("#autoLOC_226975"), Localizer.Format("#autoLOC_360842"), delegate(bool b)
			{
				SaveCraftSteamUnsubscribeWarning(b);
				SteamManager.Instance.unsubscribeItem(new PublishedFileId_t(fileId), onUnsubscribeItemCallback);
			}, delegate
			{
			});
		}
		else
		{
			SteamManager.Instance.subscribeItem(new PublishedFileId_t(fileId), onSubscribeItemCallback);
		}
	}

	public void onUnsubscribeItemCallback(RemoteStorageUnsubscribePublishedFileResult_t callResult, bool bIOFailure)
	{
		if (!bIOFailure && callResult.m_eResult == EResult.k_EResultOK)
		{
			AnalyticsUtil.LogSteamItemUnsubscribed(AnalyticsUtil.steamItemTypes.craft, callResult.m_nPublishedFileId);
			btnSteamSubUnsubText.text = Localizer.Format("#autoLOC_8002134");
			if (crewSteamSubUnsubText != null)
			{
				crewSteamSubUnsubText.text = Localizer.Format("#autoLOC_8002134");
			}
			UpdateSteamSubscribedItems();
			if (tabSteamToggle.isOn)
			{
				UpdateSteamListOfCraft(selectedSteamQueryFilter);
				return;
			}
			if (createVesselCoroutine != null)
			{
				StopCoroutine(createVesselCoroutine);
			}
			createVesselCoroutine = StartCoroutine(CreateVesselList(craftSubfolder, profileName));
		}
		else
		{
			AnalyticsUtil.LogSteamError(AnalyticsUtil.steamActions.unsubscribe, AnalyticsUtil.steamItemTypes.craft, callResult.m_nPublishedFileId, callResult.m_eResult);
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("SteamError", Localizer.Format("#autoLOC_8002136", SteamManager.Instance.GetUGCFailureReason(callResult.m_eResult)), Localizer.Format("#autoLOC_8002125"), HighLogic.UISkin, 350f, new DialogGUIButton(Localizer.Format("#autoLOC_226975"), null, dismissOnSelect: true)), persistAcrossScenes: false, HighLogic.UISkin);
		}
	}

	public void onSubscribeItemCallback(RemoteStorageSubscribePublishedFileResult_t callResult, bool bIOFailure)
	{
		if (!bIOFailure && callResult.m_eResult == EResult.k_EResultOK)
		{
			AnalyticsUtil.LogSteamItemSubscribed(AnalyticsUtil.steamItemTypes.craft, callResult.m_nPublishedFileId);
			btnSteamSubUnsubText.text = Localizer.Format("#autoLOC_8002135");
			if (crewSteamSubUnsubText != null)
			{
				crewSteamSubUnsubText.text = Localizer.Format("#autoLOC_8002135");
			}
			UpdateSteamSubscribedItems();
			if (tabSteamToggle.isOn)
			{
				UpdateSteamListOfCraft(selectedSteamQueryFilter);
				return;
			}
			if (createVesselCoroutine != null)
			{
				StopCoroutine(createVesselCoroutine);
			}
			createVesselCoroutine = StartCoroutine(CreateVesselList(craftSubfolder, profileName));
		}
		else
		{
			AnalyticsUtil.LogSteamError(AnalyticsUtil.steamActions.subscribe, AnalyticsUtil.steamItemTypes.craft, callResult.m_nPublishedFileId, callResult.m_eResult);
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("SteamError", Localizer.Format("#autoLOC_8002137", SteamManager.Instance.GetUGCFailureReason(callResult.m_eResult)), Localizer.Format("#autoLOC_8002125"), HighLogic.UISkin, 350f, new DialogGUIButton(Localizer.Format("#autoLOC_226975"), null, dismissOnSelect: true)), persistAcrossScenes: false, HighLogic.UISkin);
		}
	}

	public void SaveCraftSteamUnsubscribeWarning(bool dontShowAgain)
	{
		if (dontShowAgain == GameSettings.CRAFT_STEAM_UNSUBSCRIBE_WARNING)
		{
			GameSettings.CRAFT_STEAM_UNSUBSCRIBE_WARNING = !dontShowAgain;
			GameSettings.SaveGameSettingsOnly();
		}
	}

	public void onSteamFilterOne(bool value)
	{
		if (selectedSteamQueryFilter != 0)
		{
			SteamLoadingText.gameObject.SetActive(value: true);
			ClearCraftList();
			selectedSteamQueryFilter = SteamQueryFilters.VOTE;
			UpdateSteamSubscribedItems();
			UpdateSteamListOfCraft(selectedSteamQueryFilter);
		}
	}

	public void onSteamFilterTwo(bool value)
	{
		if (selectedSteamQueryFilter != SteamQueryFilters.FEATURED)
		{
			SteamLoadingText.gameObject.SetActive(value: true);
			ClearCraftList();
			selectedSteamQueryFilter = SteamQueryFilters.FEATURED;
			UpdateSteamSubscribedItems();
			UpdateSteamListOfCraft(selectedSteamQueryFilter);
		}
	}

	public void onSteamFilterThree(bool value)
	{
		if (selectedSteamQueryFilter != SteamQueryFilters.NEWEST)
		{
			SteamLoadingText.gameObject.SetActive(value: true);
			ClearCraftList();
			selectedSteamQueryFilter = SteamQueryFilters.NEWEST;
			UpdateSteamSubscribedItems();
			UpdateSteamListOfCraft(selectedSteamQueryFilter);
		}
	}

	public void onSteamFilterFour(bool value)
	{
		if (selectedSteamQueryFilter != SteamQueryFilters.SUBSCRIBERS)
		{
			SteamLoadingText.gameObject.SetActive(value: true);
			ClearCraftList();
			selectedSteamQueryFilter = SteamQueryFilters.SUBSCRIBERS;
			UpdateSteamSubscribedItems();
			UpdateSteamListOfCraft(selectedSteamQueryFilter);
		}
	}

	public void OnBtnSteamOverlay()
	{
		string text = "https://steamcommunity.com/workshop/browse/?appid=220200&searchtext=&childpublishedfileid=0&section=readytouseitems&days=-1";
		switch (selectedSteamQueryFilter)
		{
		case SteamQueryFilters.VOTE:
			text += "&section=readytouseitems&days=-1&browsesort=trend&requiredtags%5B%5D=Craft";
			break;
		case SteamQueryFilters.FEATURED:
			text += "&browsesort=trend&requiredtags%5B%5D=Craft&requiredtags%5B%5D=Featured";
			break;
		case SteamQueryFilters.NEWEST:
			text += "&section=readytouseitems&days=-1&browsesort=mostrecent&requiredtags%5B%5D=Craft";
			break;
		case SteamQueryFilters.SUBSCRIBERS:
			text += "&section=readytouseitems&days=-1&browsesort=totaluniquesubscribers&requiredtags%5B%5D=Craft";
			break;
		}
		SteamFriends.ActivateGameOverlayToWebPage(text);
	}

	public void onBtnSteamItemDetails()
	{
		string text = "steam://url/CommunityFilePage/" + selectedEntry.steamCraftInfo.itemDetails.m_nPublishedFileId.m_PublishedFileId;
		Debug.Log("Opening: " + text);
		SteamFriends.ActivateGameOverlayToWebPage(text);
	}

	public IEnumerator LoadSteamItemPreviewURL(CraftEntry selectedEntry)
	{
		using UnityWebRequest www = UnityWebRequestTexture.GetTexture(selectedEntry.steamCraftInfo.previewURL);
		yield return www.SendWebRequest();
		while (!www.isDone)
		{
			yield return null;
		}
		if (!www.isNetworkError && !www.isHttpError && string.IsNullOrEmpty(www.error))
		{
			steamItemPreview.texture = DownloadHandlerTexture.GetContent(www);
			steamItemPreview.gameObject.SetActive(value: true);
			yield break;
		}
		Debug.LogError("Texture load error for craft " + selectedEntry.craftName + ", error: " + www.error);
	}

	public void AddCraftEntryWidget(CraftEntry entry, RectTransform listParent)
	{
		vesselListToggleGroup.RegisterToggle(entry.Toggle);
		entry.Toggle.group = vesselListToggleGroup;
		entry.transform.SetParent(listParent, worldPositionStays: false);
	}

	public void ClearCraftList()
	{
		if (craftList != null)
		{
			int count = craftList.Count;
			while (count-- > 0)
			{
				CraftEntry craftEntry = craftList[count];
				vesselListToggleGroup.UnregisterToggle(craftEntry.Toggle);
				craftEntry.Terminate();
			}
			craftList.Clear();
			selectedEntry = null;
		}
		OnSelectionChanged(null);
	}

	public void ClearSelection()
	{
		selectedEntry = null;
		OnSelectionChanged(null);
		vesselListToggleGroup.SetAllTogglesOff();
	}

	public void OnEntrySelected(CraftEntry entry)
	{
		OnSelectionChanged(entry);
		selectedEntry = entry;
	}

	public void OnSelectionChanged(CraftEntry selectedEntry)
	{
		if (selectedEntry != null)
		{
			if (tabSteamToggle.isOn && selectedEntry.steamItem && selectedEntry.steamCraftInfo != null)
			{
				noSteamItemSelectedText.gameObject.SetActive(value: false);
				steamItemContent.SetActive(value: true);
				StartCoroutine(LoadSteamItemPreviewURL(selectedEntry));
				bool flag = SteamItemSubscribed(selectedEntry.steamCraftInfo.itemDetails.m_nPublishedFileId.m_PublishedFileId);
				btnSteamSubUnsub.interactable = true;
				btnSteamItem.interactable = true;
				btnSteamSubUnsubText.text = (flag ? Localizer.Format("#autoLOC_8002135") : Localizer.Format("#autoLOC_8002134"));
				steamItemInfovesselName.text = selectedEntry.craftName;
				steamItemKSPVersionText.text = selectedEntry.steamCraftInfo.KSPversion;
				steamItemVesselTypeText.text = selectedEntry.steamCraftInfo.vesselType;
				steamItemDescriptionText.text = selectedEntry.steamCraftInfo.itemDetails.m_rgchDescription;
				steamItemAuthorText.text = SteamManager.Instance.getUserName(selectedEntry.steamCraftInfo.itemDetails.m_ulSteamIDOwner, steamItemAuthorText);
				steamItemFavouriteText.text = selectedEntry.steamCraftInfo.totalFavorites.ToString();
				float num = (float)selectedEntry.steamCraftInfo.itemDetails.m_nFileSize / 1024f / 1024f;
				steamItemFileSizeText.text = num.ToString("N2");
				steamItemModsText.text = selectedEntry.steamCraftInfo.modsBriefing;
				steamItemDownVotesText.text = selectedEntry.steamCraftInfo.itemDetails.m_unVotesDown.ToString();
				steamItemUpVotesText.text = selectedEntry.steamCraftInfo.itemDetails.m_unVotesUp.ToString();
				steamItemSubscribersText.text = selectedEntry.steamCraftInfo.totalSubscriptions.ToString();
			}
			if (!(btnCrewSteamSubUnsub != null) || !(crewSteamSubUnsubText != null))
			{
				return;
			}
			if (selectedEntry.steamItem && !tabSteamToggle.isOn)
			{
				btnCrewSteamSubUnsub.gameObject.SetActive(value: true);
				ulong steamFileId = selectedEntry.GetSteamFileId();
				if (steamFileId == 0L)
				{
					btnCrewSteamSubUnsub.interactable = false;
					return;
				}
				btnCrewSteamSubUnsub.interactable = true;
				bool flag2 = SteamItemSubscribed(steamFileId);
				crewSteamSubUnsubText.text = (flag2 ? Localizer.Format("#autoLOC_8002135") : Localizer.Format("#autoLOC_8002134"));
			}
			else
			{
				btnCrewSteamSubUnsub.gameObject.SetActive(value: false);
				btnCrewSteamSubUnsub.interactable = false;
			}
		}
		else
		{
			steamItemInfovesselName.text = "";
			btnSteamSubUnsub.interactable = false;
			noSteamItemSelectedText.gameObject.SetActive(value: true);
			steamItemContent.SetActive(value: false);
			btnSteamItem.interactable = false;
			if (btnCrewSteamSubUnsub != null)
			{
				btnCrewSteamSubUnsub.gameObject.SetActive(value: false);
				btnCrewSteamSubUnsub.interactable = false;
			}
		}
	}

	public IEnumerator CreateVesselList(string craftSubfolder, string profileName)
	{
		UpdateSteamSubscribedItems();
		yield return null;
		bool finalVesselListSort = false;
		float num = Time.realtimeSinceStartup + maxFrameTime;
		this.craftSubfolder = craftSubfolder;
		scrollList.Clear(destroyElements: true);
		vesselDataItemList.Clear();
		CraftProfileInfo.PrepareCraftMetaFileLoad();
		if (!Directory.Exists(KSPUtil.ApplicationRootPath + "Ships/" + craftSubfolder))
		{
			Directory.CreateDirectory(KSPUtil.ApplicationRootPath + "Ships/" + craftSubfolder);
		}
		if (ExpansionsLoader.IsAnyExpansionInstalled())
		{
			List<ExpansionsLoader.ExpansionInfo> installedExpansions = ExpansionsLoader.GetInstalledExpansions();
			for (int m = 0; m < installedExpansions.Count; m++)
			{
				string path = KSPExpansionsUtils.ExpansionsGameDataPath + installedExpansions[m].FolderName + "/Ships/" + craftSubfolder;
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
			}
		}
		if (HighLogic.CurrentGame.Parameters.Difficulty.AllowStockVessels)
		{
			FileInfo[] fileInfoList2 = new DirectoryInfo(KSPUtil.ApplicationRootPath + "Ships/" + craftSubfolder).GetFiles("*.craft");
			int fileInfoListCount4 = fileInfoList2.Length;
			int l = 0;
			while (l < fileInfoListCount4)
			{
				FileInfo fInfo = fileInfoList2[l];
				try
				{
					AddVesselDataItem(vesselDataItemList, new VesselDataItem(fInfo, stock: true, steamItem: false)).radioButton.Interactable = false;
					finalVesselListSort = true;
				}
				catch
				{
				}
				if (Time.realtimeSinceStartup > num)
				{
					finalVesselListSort = false;
					OnVesselListSort(sortButton, sortAsc);
					yield return null;
					num = Time.realtimeSinceStartup + maxFrameTime;
				}
				int num2 = l + 1;
				l = num2;
			}
			if (ExpansionsLoader.IsAnyExpansionInstalled())
			{
				List<ExpansionsLoader.ExpansionInfo> installedExps = ExpansionsLoader.GetInstalledExpansions();
				l = 0;
				while (l < installedExps.Count)
				{
					string path2 = KSPExpansionsUtils.ExpansionsGameDataPath + installedExps[l].FolderName + "/Ships/" + craftSubfolder;
					FileInfo[] expansionCraftList = new DirectoryInfo(path2).GetFiles("*.craft");
					fileInfoListCount4 = expansionCraftList.Length;
					int num2;
					for (int i = 0; i < fileInfoListCount4; i = num2)
					{
						FileInfo fInfo = expansionCraftList[i];
						try
						{
							AddVesselDataItem(vesselDataItemList, new VesselDataItem(fInfo, stock: true, steamItem: false)).radioButton.Interactable = false;
							finalVesselListSort = true;
						}
						catch
						{
						}
						if (Time.realtimeSinceStartup > num)
						{
							finalVesselListSort = false;
							OnVesselListSort(sortButton, sortAsc);
							yield return null;
							num = Time.realtimeSinceStartup + maxFrameTime;
						}
						num2 = i + 1;
					}
					num2 = l + 1;
					l = num2;
				}
			}
		}
		if (!string.IsNullOrEmpty(profileName))
		{
			if (!Directory.Exists(KSPUtil.ApplicationRootPath + "saves/" + profileName + "/Ships/" + craftSubfolder))
			{
				Directory.CreateDirectory(KSPUtil.ApplicationRootPath + "saves/" + profileName + "/Ships/" + craftSubfolder);
			}
			FileInfo[] fileInfoList2 = new DirectoryInfo(KSPUtil.ApplicationRootPath + "saves/" + profileName + "/Ships/" + craftSubfolder).GetFiles("*.craft");
			int fileInfoListCount4 = fileInfoList2.Length;
			int l = 0;
			while (l < fileInfoListCount4)
			{
				FileInfo fInfo2 = fileInfoList2[l];
				try
				{
					AddVesselDataItem(vesselDataItemList, new VesselDataItem(fInfo2, stock: false, steamItem: false)).radioButton.Interactable = false;
					finalVesselListSort = true;
				}
				catch
				{
				}
				if (Time.realtimeSinceStartup > num)
				{
					finalVesselListSort = false;
					OnVesselListSort(sortButton, sortAsc);
					yield return null;
					num = Time.realtimeSinceStartup + maxFrameTime;
				}
				int num2 = l + 1;
				l = num2;
			}
		}
		if (SteamManager.Initialized && !tabSteamToggle.isOn)
		{
			List<FileInfo> fileList = KSPSteamUtils.GatherCraftFilesFileInfo();
			int fileInfoListCount4 = 0;
			while (fileInfoListCount4 < fileList.Count)
			{
				try
				{
					AddVesselDataItem(vesselDataItemList, new VesselDataItem(fileList[fileInfoListCount4], stock: false, steamItem: true)).radioButton.Interactable = false;
					finalVesselListSort = true;
				}
				catch
				{
				}
				if (Time.realtimeSinceStartup > num)
				{
					finalVesselListSort = false;
					OnVesselListSort(sortButton, sortAsc);
					yield return null;
					num = Time.realtimeSinceStartup + maxFrameTime;
				}
				int num2 = fileInfoListCount4 + 1;
				fileInfoListCount4 = num2;
			}
		}
		if (finalVesselListSort)
		{
			OnVesselListSort(sortButton, sortAsc);
		}
		if (scrollList.Count > 0)
		{
			int count = scrollList.Items.Count;
			for (int n = 0; n < count; n++)
			{
				scrollList.Items[n].listItem.GetComponent<UIRadioButton>().Interactable = true;
			}
			SelectVesselDataItem(GetVesselDataItem(EditorLogic.autoShipName) ?? GetVesselDataItem(0));
		}
		else
		{
			buttonLaunch.interactable = false;
			buttonDelete.interactable = false;
			buttonEdit.interactable = false;
			vesselDescription.text = "";
		}
	}

	public void SelectVesselDataItem(VesselDataItem dataItem)
	{
		if (dataItem != null)
		{
			UIRadioButton component = dataItem.listItem.GetComponent<UIRadioButton>();
			if (component != null)
			{
				component.Value = true;
			}
			selectedDataItem = dataItem;
			UpdateVesselInfoPanel(dataItem);
			StartCoroutine(LateSelectFireThing());
		}
	}

	public IEnumerator LateSelectFireThing()
	{
		yield return null;
		GameEvents.onGUILaunchScreenVesselSelected.Fire(selectedDataItem.template);
	}

	public void AddVesselListItem(string name, float mass, float cost, float size)
	{
		UIListItem uIListItem = Object.Instantiate(listItemPrefab);
		VesselListItem component = uIListItem.GetComponent<VesselListItem>();
		component.Setup(name, mass, cost, size);
		component.GetComponent<Toggle>().group = vesselListToggleGroup;
		scrollList.AddItem(new UIListData<ListItem>(new ListItem(name, mass, cost, size), uIListItem));
	}

	public VesselListItem AddVesselDataItem(List<VesselDataItem> list, VesselDataItem item)
	{
		list.Add(item);
		item.listItem = Object.Instantiate(listItemPrefab);
		VesselListItem component = item.listItem.GetComponent<VesselListItem>();
		component.radioButton.onTrue.AddListener(onVesselListItemTrue);
		component.radioButton.onClick.AddListener(onVesselListItemClick);
		string parts = Localizer.Format("#autoLOC_482977", GetPartCountTextColorHex(item), item.parts, item.stages);
		string warnings = string.Empty;
		bool flag = false;
		if (item.steamItem)
		{
			string stateText = "";
			bool canBeUsed = false;
			bool subscribed = false;
			SteamManager.Instance.GetItemState(new PublishedFileId_t(item.GetSteamFileId()), out stateText, out canBeUsed, out subscribed);
			warnings = ((!subscribed) ? Localizer.Format("#autoLOC_8002131") : Localizer.Format("#autoLOC_8002130"));
			if (!canBeUsed)
			{
				warnings = stateText;
			}
		}
		if (item.compatibility != VersionCompareResult.COMPATIBLE)
		{
			warnings = "<color=#db6227>" + Localizer.Format("#autoLOC_8004246") + "</color>";
			flag = true;
		}
		else if (item.isValid)
		{
			if (item.isExperimental)
			{
				warnings = Localizer.Format("#autoLOC_482982");
			}
			else if (!item.craftProfileInfo.shipPartModulesAvailable)
			{
				warnings = Localizer.Format("#autoLOC_8004266");
			}
		}
		else
		{
			warnings = Localizer.Format("#autoLOC_482988");
		}
		string text = item.name;
		string mass = Localizer.Format("#autoLOC_482996", GetMassTextColorHex(item), item.craftProfileInfo.totalMass.ToString("N1"));
		string cost = "<color=" + GetCostTextColorHex(item) + "><sprite=\"CurrencySpriteAsset\" name=\"Funds\" tint=1>  " + KSPUtil.LocalizeNumber(item.craftProfileInfo.totalCost, "0.0") + "</color>";
		string size = string.Empty;
		Vector3 shipSize = item.craftProfileInfo.shipSize;
		if (!flag)
		{
			size = ((!(shipSize == Vector3.zero)) ? Localizer.Format("#autoLOC_483009", GetSizeTextColorHex(item), shipSize.y.ToString("0.0"), shipSize.x.ToString("0.0"), shipSize.z.ToString("0.0")) : Localizer.Format("#autoLOC_483005", XKCDColors.HexFormat.KSPNotSoGoodOrange));
		}
		item.listItem.Data = item;
		component.Setup(text, parts, warnings, mass, cost, size);
		scrollList.AddItem(new UIListData<ListItem>(new ListItem(item.name, item.craftProfileInfo.totalMass, item.craftProfileInfo.totalCost, shipSize.magnitude), item.listItem));
		return component;
	}

	public VesselDataItem GetVesselDataItem(int dataIndex)
	{
		UIListItem uilistItemAt = scrollList.GetUilistItemAt(dataIndex);
		if (!uilistItemAt)
		{
			return null;
		}
		return uilistItemAt.Data as VesselDataItem;
	}

	public VesselDataItem GetVesselDataItem(string dataName)
	{
		int count = scrollList.Count;
		VesselDataItem vesselDataItem;
		do
		{
			if (count-- > 0)
			{
				vesselDataItem = GetVesselDataItem(count);
				continue;
			}
			return null;
		}
		while (vesselDataItem == null || vesselDataItem.name != dataName);
		return vesselDataItem;
	}

	public string GetCostTextColorHex(VesselDataItem item)
	{
		if (Funding.CanAfford(item.craftProfileInfo.totalCost))
		{
			return XKCDColors.HexFormat.KSPBadassGreen;
		}
		return XKCDColors.HexFormat.KSPNotSoGoodOrange;
	}

	public string GetMassTextColorHex(VesselDataItem item)
	{
		EditorFacility shipFacility = item.craftProfileInfo.shipFacility;
		CraftWithinMassLimits craftWithinMassLimits = ((shipFacility == EditorFacility.const_1 || shipFacility != EditorFacility.const_2) ? new CraftWithinMassLimits(item.craftProfileInfo.totalMass, item.craftProfileInfo.shipName, SpaceCenterFacility.LaunchPad, GameVariables.Instance.GetCraftMassLimit(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.LaunchPad), isPad: true)) : new CraftWithinMassLimits(item.craftProfileInfo.totalMass, item.craftProfileInfo.shipName, SpaceCenterFacility.Runway, GameVariables.Instance.GetCraftMassLimit(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.Runway), isPad: false)));
		if (craftWithinMassLimits.Test())
		{
			return XKCDColors.HexFormat.KSPBadassGreen;
		}
		return XKCDColors.HexFormat.KSPNotSoGoodOrange;
	}

	public string GetSizeTextColorHex(VesselDataItem item)
	{
		EditorFacility shipFacility = item.craftProfileInfo.shipFacility;
		CraftWithinSizeLimits craftWithinSizeLimits = ((shipFacility == EditorFacility.const_1 || shipFacility != EditorFacility.const_2) ? new CraftWithinSizeLimits(item.craftProfileInfo.shipSize, item.craftProfileInfo.shipName, SpaceCenterFacility.LaunchPad, GameVariables.Instance.GetCraftSizeLimit(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.LaunchPad), isPad: true)) : new CraftWithinSizeLimits(item.craftProfileInfo.shipSize, item.craftProfileInfo.shipName, SpaceCenterFacility.Runway, GameVariables.Instance.GetCraftSizeLimit(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.Runway), isPad: false)));
		if (craftWithinSizeLimits.Test())
		{
			return XKCDColors.HexFormat.KSPNeutralUIGrey;
		}
		return XKCDColors.HexFormat.KSPNotSoGoodOrange;
	}

	public string GetPartCountTextColorHex(VesselDataItem item)
	{
		EditorFacility shipFacility = item.craftProfileInfo.shipFacility;
		CraftWithinPartCountLimit craftWithinPartCountLimit = ((shipFacility == EditorFacility.const_1 || shipFacility != EditorFacility.const_2) ? new CraftWithinPartCountLimit(item.craftProfileInfo.partCount, SpaceCenterFacility.VehicleAssemblyBuilding, GameVariables.Instance.GetPartCountLimit(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.VehicleAssemblyBuilding), isVAB: true)) : new CraftWithinPartCountLimit(item.craftProfileInfo.partCount, SpaceCenterFacility.SpaceplaneHangar, GameVariables.Instance.GetPartCountLimit(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.SpaceplaneHangar), isVAB: false)));
		if (craftWithinPartCountLimit.Test())
		{
			return XKCDColors.HexFormat.KSPNeutralUIGrey;
		}
		return XKCDColors.HexFormat.KSPNotSoGoodOrange;
	}

	public void onVesselListItemTrue(PointerEventData data, UIRadioButton.CallType callType)
	{
		if (callType == UIRadioButton.CallType.USER)
		{
			VesselDataItem vesselDataItem = data.pointerPress.GetComponent<UIRadioButton>().GetComponent<UIListItem>().Data as VesselDataItem;
			UpdateVesselInfoPanel(vesselDataItem);
			selectedDataItem = vesselDataItem;
			GameEvents.onGUILaunchScreenVesselSelected.Fire(vesselDataItem.template);
		}
	}

	public void onVesselListItemClick(PointerEventData data, UIRadioButton.State state, UIRadioButton.CallType callType)
	{
		if (callType == UIRadioButton.CallType.USER && data.clickCount == 2)
		{
			ButtonLaunch();
		}
	}

	public void UpdateVesselInfoPanel(VesselDataItem dataItem)
	{
		crewManifest = HighLogic.CurrentGame.CrewRoster.DefaultCrewForVessel(dataItem.configNode);
		if (CrewAssignmentDialog.Instance != null)
		{
			CrewAssignmentDialog.Instance.RefreshCrewLists(crewManifest, setAsDefault: true, updateUI: true);
		}
		buttonDelete.interactable = !dataItem.stock;
		vesselName.text = dataItem.name;
		vesselDescription.text = dataItem.description;
		shipThumbnail.texture = dataItem.thumbnail;
		selectedDataItem = dataItem;
		if (!dataItem.isValid)
		{
			buttonLaunch.interactable = false;
		}
		else
		{
			buttonLaunch.interactable = true;
		}
		if (dataItem.steamItem)
		{
			craftImageSteamOverlay.SetActive(value: true);
		}
		else
		{
			craftImageSteamOverlay.SetActive(value: false);
		}
		if (!(btnCrewSteamSubUnsub != null) || !(crewSteamSubUnsubText != null))
		{
			return;
		}
		if (selectedDataItem.steamItem && !tabSteamToggle.isOn)
		{
			UpdateSteamSubscribedItems();
			btnCrewSteamSubUnsub.gameObject.SetActive(value: true);
			ulong steamFileId = selectedDataItem.GetSteamFileId();
			if (steamFileId == 0L)
			{
				btnCrewSteamSubUnsub.interactable = false;
				return;
			}
			btnCrewSteamSubUnsub.interactable = true;
			bool flag = SteamItemSubscribed(steamFileId);
			crewSteamSubUnsubText.text = (flag ? Localizer.Format("#autoLOC_8002135") : Localizer.Format("#autoLOC_8002134"));
		}
		else
		{
			btnCrewSteamSubUnsub.gameObject.SetActive(value: false);
			btnCrewSteamSubUnsub.interactable = false;
		}
	}

	public void OnVesselListSort(int button, bool asc)
	{
		switch (button)
		{
		case 0:
			scrollList.Sort(new RUIutils.FuncComparer<UIListData<ListItem>>((UIListData<ListItem> left, UIListData<ListItem> right) => RUIutils.SortAscDescPrimarySecondary(asc, left.data.name.CompareTo(right.data.name), left.data.mass.CompareTo(right.data.mass))));
			break;
		case 1:
			scrollList.Sort(new RUIutils.FuncComparer<UIListData<ListItem>>((UIListData<ListItem> left, UIListData<ListItem> right) => RUIutils.SortAscDescPrimarySecondary(asc, left.data.mass.CompareTo(right.data.mass), left.data.name.CompareTo(right.data.name))));
			break;
		case 2:
			scrollList.Sort(new RUIutils.FuncComparer<UIListData<ListItem>>((UIListData<ListItem> left, UIListData<ListItem> right) => RUIutils.SortAscDescPrimarySecondary(asc, left.data.cost.CompareTo(right.data.cost), left.data.name.CompareTo(right.data.name))));
			break;
		case 3:
			scrollList.Sort(new RUIutils.FuncComparer<UIListData<ListItem>>((UIListData<ListItem> left, UIListData<ListItem> right) => RUIutils.SortAscDescPrimarySecondary(asc, left.data.size.CompareTo(right.data.size), left.data.name.CompareTo(right.data.name))));
			break;
		}
	}

	public void ButtonLaunch()
	{
		if (selectedDataItem.compatibility == VersionCompareResult.COMPATIBLE && selectedDataItem.isValid)
		{
			if (selectedDataItem.craftProfileInfo.shipPartsUnlocked && selectedDataItem.craftProfileInfo.shipPartModulesAvailable)
			{
				ConfirmLaunch();
				return;
			}
			DialogGUILabel dialogGUILabel = new DialogGUILabel(selectedDataItem.craftProfileInfo.GetErrorMessage());
			dialogGUILabel.bypassTextStyleColor = true;
			DialogGUIVerticalLayout dialogGUIVerticalLayout = new DialogGUIVerticalLayout(true, false, 0f, new RectOffset(4, 11, 4, 4), TextAnchor.UpperLeft, dialogGUILabel);
			dialogGUIVerticalLayout.AddChild(new DialogGUIContentSizer(ContentSizeFitter.FitMode.Unconstrained, ContentSizeFitter.FitMode.PreferredSize));
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("Confirmation Needed", Localizer.Format("#autoLOC_8004267"), Localizer.Format("#autoLOC_464288"), HighLogic.UISkin, 350f, new DialogGUISpace(6f), new DialogGUIScrollList(new Vector2(-1f, 80f), hScroll: false, vScroll: true, dialogGUIVerticalLayout), new DialogGUISpace(6f), new DialogGUIHorizontalLayout(TextAnchor.UpperLeft, new DialogGUIFlexibleSpace(), new DialogGUIButton(Localizer.Format("#autoLOC_417274"), ConfirmLaunch, 100f, -1f, true), new DialogGUIButton(Localizer.Format("#autoLOC_226976"), null, 150f, -1f, true))), persistAcrossScenes: false, HighLogic.UISkin);
		}
		else
		{
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("Confirmation Needed", Localizer.Format("#autoLOC_8004239"), Localizer.Format("#autoLOC_464288"), HighLogic.UISkin, 350f, new DialogGUIButton(Localizer.Format("#autoLOC_417274"), ConfirmLaunch, dismissOnSelect: true), new DialogGUIButton(Localizer.Format("#autoLOC_226976"), null, dismissOnSelect: true)), persistAcrossScenes: false, HighLogic.UISkin);
		}
	}

	public void ConfirmLaunch()
	{
		if (!selectedDataItem.craftProfileInfo.shipPartsUnlocked || !selectedDataItem.craftProfileInfo.shipPartModulesAvailable)
		{
			Debug.LogWarning("Loading craft with the following issues:\n" + selectedDataItem.craftProfileInfo.GetErrorMessage());
		}
		KSPUpgradePipeline.Process(selectedDataItem.configNode, selectedDataItem.name, LoadContext.Craft, delegate(ConfigNode n)
		{
			onPipelineFinished(n, LaunchSelectedVessel);
		}, delegate(KSPUpgradePipeline.UpgradeFailOption opt, ConfigNode n)
		{
			onPipelineFailed(opt, n, LaunchSelectedVessel);
		});
	}

	public void ButtonDelete()
	{
		PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("DeleteCraft", Localizer.Format("#autoLOC_483228"), Localizer.Format("#autoLOC_464288"), HighLogic.UISkin, new DialogGUIButton(Localizer.Format("#autoLOC_483229"), OnDeleteConfirm), new DialogGUIButton(Localizer.Format("#autoLOC_483230"), OnDeleteDismiss)), persistAcrossScenes: false, HighLogic.UISkin).OnDismiss = OnDeleteDismiss;
	}

	public void OnDeleteConfirm()
	{
		if (File.Exists(KSPUtil.ApplicationRootPath + selectedDataItem.thumbURL + ".png"))
		{
			File.Delete(KSPUtil.ApplicationRootPath + selectedDataItem.thumbURL + ".png");
		}
		File.Delete(selectedDataItem.fullFilePath);
		createVesselCoroutine = StartCoroutine(CreateVesselList(craftSubfolder, HighLogic.SaveFolder));
		OnDeleteDismiss();
	}

	public void OnDeleteDismiss()
	{
	}

	public void ButtonEdit()
	{
		string value = selectedDataItem.configNode.GetValue("type");
		string facilityName;
		string facilityTitle;
		if (!(value == "VAB") && value == "SPH")
		{
			facilityName = "SPH";
			facilityTitle = "SpacePlane Hangar";
			editorFacility = EditorFacility.const_2;
		}
		else
		{
			facilityName = "VAB";
			facilityTitle = "Vehicle Assembly Building";
			editorFacility = EditorFacility.const_1;
		}
		PreFlightCheck preFlightCheck = new PreFlightCheck(onEditBtnProceed, onEditBtnAbort);
		preFlightCheck.AddTest(new FacilityOperational(facilityName, facilityTitle));
		preFlightCheck.RunTests();
	}

	public void onEditBtnProceed()
	{
		KSPUpgradePipeline.Process(selectedDataItem.configNode, selectedDataItem.name, LoadContext.Craft, delegate(ConfigNode n)
		{
			onPipelineFinished(n, onEditBtnPipelineFinish);
		}, delegate(KSPUpgradePipeline.UpgradeFailOption opt, ConfigNode n)
		{
			onPipelineFailed(opt, n, onEditBtnPipelineFinish);
		});
	}

	public void onPipelineFailed(KSPUpgradePipeline.UpgradeFailOption opt, ConfigNode n, Callback onFinish)
	{
		switch (opt)
		{
		case KSPUpgradePipeline.UpgradeFailOption.LoadAnyway:
			onPipelineFinished(n, onFinish);
			break;
		case KSPUpgradePipeline.UpgradeFailOption.Cancel:
			ButtonClose();
			break;
		}
	}

	public void onPipelineFinished(ConfigNode n, Callback onFinish)
	{
		if (n != selectedDataItem.configNode)
		{
			selectedDataItem.configNode.Save(selectedDataItem.fullFilePath + ".original");
			n.Save(selectedDataItem.fullFilePath);
		}
		onFinish();
	}

	public void onEditBtnPipelineFinish()
	{
		GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
		EditorDriver.StartAndLoadVessel(selectedDataItem.fullFilePath, editorFacility);
	}

	public void LaunchSelectedVessel()
	{
		GameEvents.onGUILaunchScreenDespawn.Fire();
		VesselCrewManifest manifest = CrewAssignmentDialog.Instance.GetManifest();
		if (manifest != null)
		{
			crewManifest = manifest;
		}
		launchSiteFacility.launchSiteName = siteName;
		OnFileSelected(selectedDataItem.fullFilePath, flagURL, crewManifest);
	}

	public void onEditBtnAbort()
	{
	}

	public void ButtonClose()
	{
		Visible = false;
		CrewAssignmentDialog.Instance.ClearLists();
		GameEvents.onGUILaunchScreenDespawn.Fire();
	}

	public void OnFlagSelected(FlagBrowser.FlagEntry flag)
	{
		flagURL = flag.textureInfo.name;
	}
}
