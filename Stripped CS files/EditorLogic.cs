using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using EditorGizmos;
using Expansions;
using Expansions.Missions;
using Expansions.Missions.Editor;
using Expansions.Missions.Runtime;
using Expansions.Serenity;
using Highlighting;
using ns11;
using ns2;
using ns9;
using PreFlightTests;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EditorLogic : EditorLogicBase
{
	public enum EditorModes
	{
		SIMPLE,
		ADVANCED
	}

	public EditorToolsUI toolsUI;

	public Camera editorCamera;

	public EditorCamera editorCam;

	public Camera editorCargoCamera;

	public List<string> availableCargoParts;

	public static string autoshipname;

	public string startPodId = "mk1pod.v2";

	public Quaternion vesselRotation = Quaternion.identity;

	public float dragSharpness = 0.3f;

	public float srfAttachAngleSnap = 15f;

	public float srfAttachAngleSnapFine = 5f;

	public bool disallowSave;

	public int sceneToLoad;

	public Part rootPart;

	public static EditorPartIcon iconRequestedVariantChange = null;

	public AudioClip attachClip;

	public AudioClip deletePartClip;

	public AudioClip partGrabClip;

	public AudioClip partReleaseClip;

	public AudioClip cannotPlaceClip;

	public AudioClip tweakGrabClip;

	public AudioClip tweakReleaseClip;

	public AudioClip reRootClip;

	public string launchSiteName = "LaunchPad";

	public PSystemSetup.SpaceCenterFacility selectedLaunchFacility;

	public LaunchSite selectedLaunchSite;

	public Ray ray;

	public RaycastHit hit;

	public Collider FairingHitCollider;

	public int undoLevel;

	public int layerMask;

	public int undoLimit;

	public ShipConstruct ship;

	public ConstructionMode constructionMode;

	public Attachment attachment;

	public Attachment[] cPartAttachments;

	public static EditorLogic fetch;

	public Button partPanelBtn;

	public Button actionPanelBtn;

	public Button crewPanelBtn;

	public Button switchEditorBtn;

	public Button cargoPanelBtn;

	public UIStateToggleButton editorBtnStateToggle;

	public RectTransform crewBtnOrigPos;

	public RectTransform cargoBtnOrigPos;

	public RectTransform editorBtnOrigPos;

	public Button saveBtn;

	public Button launchBtn;

	public Button exitBtn;

	public Button loadBtn;

	public Button newBtn;

	public Button steamBtn;

	[SerializeField]
	public GameObject launchSiteSelector;

	public Button coordSpaceBtn;

	public TextMeshProUGUI coordSpaceText;

	public Button radialSymmetryBtn;

	public TextMeshProUGUI radialSymmetryText;

	public UIOnClick symmetryButton;

	public UIStateImage symmetrySprite;

	public UIStateImage mirrorSprite;

	public UIOnClick angleSnapButton;

	public UIStateImage angleSnapSprite;

	public TMP_InputField shipNameField;

	public TMP_InputField shipDescriptionField;

	public FlagBrowserButton flagBrowserButton;

	public Collider[] modalAreas;

	public bool allowSrfAttachment = true;

	public bool allowNodeAttachment = true;

	public GUISkin shipBrowserSkin;

	public Texture2D shipFileImage;

	public List<MissionRecoveryDialog> missionDialogs;

	public SymmetryMethod symmetryMethod;

	public SymmetryMethod symmetryMethodTmp;

	public int symmetryMode;

	public int symmetryModeTmp;

	public int symmetryModeBeforeNodeAttachment = -1;

	public bool tmpSymMethodInUse;

	public EditorPartListFilter<AvailablePart> rootPartsOnlyFilter;

	public EditorPartListFilter<AvailablePart> inaccessiblePartsFilter;

	public Quaternion gizmoAttRotate;

	public Quaternion gizmoAttRotate0;

	public GizmoRotate gizmoRotate;

	public GizmoOffset gizmoOffset;

	public Space symmetryCoordSpace = Space.Self;

	public AudioSource audioSource;

	public bool setSteamExportItemPublic;

	public string setSteamExportItemChangeLog = Localizer.Format("#autoLOC_8002120");

	public string setSteamModsText = "";

	public string steamExportTempPath = "";

	public VesselType setSteamVesselType;

	public bool setSteamRobiticsTag;

	public List<AppId_t> appDependenciesToRemove;

	public List<string> steamTags;

	public string steamThumbURL;

	public ERemoteStoragePublishedFileVisibility steamVisibility;

	public float steamTotalCost;

	public int steamCrewCapacity;

	public double timeSteamCommsStarted;

	public PopupDialog steamCommsDialog;

	public bool steamUploadingNewItem;

	public static EditorModes Mode = EditorModes.SIMPLE;

	public bool skipPartAttach;

	public CraftBrowserDialog craftBrowserDialog;

	public static string FlagURL = "";

	public KFSMState st_podSelect;

	public KFSMState st_idle;

	public KFSMState st_place;

	public KFSMState st_offset_select;

	public KFSMState st_offset_tweak;

	public KFSMState st_rotate_select;

	public KFSMState st_rotate_tweak;

	public KFSMState st_root_unselected;

	public KFSMState st_root_select;

	public KFSMEvent on_podSelect;

	public KFSMEvent on_partCreated;

	public KFSMEvent on_partPicked;

	public KFSMEvent on_partCopied;

	public KFSMEvent on_partReveal;

	public KFSMEvent on_partDropped;

	public KFSMEvent on_partAttached;

	public KFSMEvent on_partDeleted;

	public KFSMEvent on_partLost;

	public KFSMEvent on_podDeleted;

	public KFSMEvent on_partOverInventoryPAW;

	public KFSMEvent on_goToModeOffset;

	public KFSMEvent on_offsetSelect;

	public KFSMEvent on_offsetDeselect;

	public KFSMEvent on_offsetReset;

	public KFSMEvent on_goToModeRotate;

	public KFSMEvent on_rotateSelect;

	public KFSMEvent on_rotateDeselect;

	public KFSMEvent on_rotateReset;

	public KFSMEvent on_goToModeRoot;

	public KFSMEvent on_rootPickSet;

	public KFSMEvent on_rootDeselect;

	public KFSMEvent on_rootSelect;

	public KFSMEvent on_rootSelectFail;

	public KFSMEvent on_goToModePlace;

	public KFSMEvent on_undoRedo;

	public KFSMEvent on_newShip;

	public KFSMEvent on_shipLoaded;

	public Quaternion attRot0;

	public int symUpdateMode;

	public Part symUpdateParent;

	public AttachNode symUpdateAttachNode;

	public Vector3 gizmoPivot;

	public Quaternion refRot;

	public Vector3 offsetGap;

	public float threshold;

	public AttachNode childToParent;

	public AttachNode parentToChild;

	public Vector3 diff;

	public List<PartSelector> rootCandidates;

	public int undoIndexAtLastSave;

	public string vesselNameAtLastSave;

	public string vesselNameAtLastSave_Sanitized;

	public string savedCraftPath;

	public LaunchSiteClear launchSiteClearTest;

	public bool panelButtonsLocked;

	public double startTime;

	public double modeStartTime;

	public Dictionary<string, double> timeInMode;

	public EditorScreen currentMode;

	public bool FSMStarted
	{
		get
		{
			if (fsm != null)
			{
				return fsm.Started;
			}
			return false;
		}
	}

	public string currentStateName => fsm.currentStateName;

	public string lastEventName => fsm.lastEventName;

	public static string autoShipName => autoshipname;

	public Vector3 initialPodPosition
	{
		get
		{
			if (!EditorBounds.Instance)
			{
				return new Vector3(0f, 5f, 0f);
			}
			return EditorBounds.Instance.rootPartSpawnPoint;
		}
	}

	public static Quaternion VesselRotation => fetch.vesselRotation;

	public bool AllowSave
	{
		get
		{
			if (!disallowSave)
			{
				return shipNameField.text.Length > 0;
			}
			return false;
		}
	}

	public Bounds editorBounds
	{
		get
		{
			if (!EditorBounds.Instance)
			{
				return new Bounds(Vector3.zero, Vector3.one * float.MaxValue);
			}
			return EditorBounds.Instance.constructionBounds;
		}
	}

	public static Part SelectedPart
	{
		get
		{
			if (!(fetch != null))
			{
				return null;
			}
			return fetch.selectedPart;
		}
	}

	public static Part RootPart
	{
		get
		{
			if (!(fetch != null))
			{
				return null;
			}
			return fetch.rootPart;
		}
	}

	public static int LayerMask
	{
		get
		{
			if (!fetch)
			{
				return 67109122 + LayerUtil.DefaultEquivalent;
			}
			return fetch.layerMask;
		}
	}

	public ConstructionMode EditorConstructionMode => constructionMode;

	public static Texture2D ShipFileImage
	{
		get
		{
			if (!fetch)
			{
				return null;
			}
			return fetch.shipFileImage;
		}
	}

	public static List<Part> SortedShipList => fetch.getSortedShipList();

	public void Awake()
	{
		if ((bool)fetch)
		{
			UnityEngine.Object.DestroyImmediate(this);
			return;
		}
		fetch = this;
		autoshipname = Localizer.Format("#autoLOC_123938");
		rootPartsOnlyFilter = new EditorPartListFilter<AvailablePart>("rootPartsOnly", delegate(AvailablePart aPart)
		{
			if (!(aPart.partPrefab != null))
			{
				return true;
			}
			return aPart.partPrefab.attachRules.allowStack && aPart.partPrefab.attachRules.allowRoot && aPart.partPrefab.physicalSignificance == Part.PhysicalSignificance.FULL && aPart.partPrefab.FindModuleImplementing<ModuleGroundPart>() == null;
		}, Localizer.Format("#autoLOC_124050"));
		inaccessiblePartsFilter = new EditorPartListFilter<AvailablePart>("inaccessibleParts", (AvailablePart aPart) => aPart.partPrefab != null && aPart.TechRequired != "Unresearcheable", "Cannot be used in the Editor");
		editorCam = UnityEngine.Object.FindObjectOfType<EditorCamera>();
		if (editorCam != null)
		{
			editorCamera = editorCam.GetComponent<Camera>();
			Camera[] componentsInChildren = editorCam.gameObject.GetComponentsInChildren<Camera>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].gameObject.name == "CargoCamera")
				{
					editorCargoCamera = componentsInChildren[i];
					break;
				}
			}
		}
		layerMask = 67109122 + LayerUtil.DefaultEquivalent;
		modeMsg = new ScreenMessage("", 5f, ScreenMessageStyle.LOWER_CENTER);
		audioSource = GetComponent<AudioSource>();
		GameEvents.onGUIAstronautComplexDespawn.Add(RefreshCrewDialog);
		GameEvents.onGUIAstronautComplexSpawn.Add(SpawningAC);
		missionDialogs = new List<MissionRecoveryDialog>();
		GameEvents.onGUIRecoveryDialogSpawn.Add(onMissionDialogUp);
		GameEvents.onGUIRecoveryDialogDespawn.Add(onMissionDialogDismiss);
		GameEvents.onInputLocksModified.Add(OnInputLocksModified);
		GameEvents.onEditorConstructionModeChange.Add(onConstructionModeChanged);
		GameEvents.onEditorSymmetryMethodChange.Add(onSymMethodChange);
		GameEvents.onPartVesselNamingChanged.Add(OnPartVesselNamingChanged);
		GameEvents.onEditorShipModified.Add(OnVesselNamingShipModified);
		GameEvents.OnPartPurchased.Add(OnPartPurchased);
		InputLockManager.DebugLockStack();
		if (steamBtn != null)
		{
			steamBtn.gameObject.SetActive(SteamManager.Initialized);
		}
	}

	public void OnDestroy()
	{
		GameEvents.onGUIAstronautComplexDespawn.Remove(RefreshCrewDialog);
		GameEvents.onGUIAstronautComplexSpawn.Remove(SpawningAC);
		GameEvents.onGUIRecoveryDialogSpawn.Remove(onMissionDialogUp);
		GameEvents.onGUIRecoveryDialogDespawn.Remove(onMissionDialogDismiss);
		GameEvents.onInputLocksModified.Remove(OnInputLocksModified);
		GameEvents.onEditorConstructionModeChange.Remove(onConstructionModeChanged);
		GameEvents.onEditorSymmetryMethodChange.Remove(onSymMethodChange);
		GameEvents.onPartVesselNamingChanged.Remove(OnPartVesselNamingChanged);
		GameEvents.onEditorShipModified.Remove(OnVesselNamingShipModified);
		GameEvents.OnPartPurchased.Remove(OnPartPurchased);
		InputLockManager.RemoveControlLock("Editor_outOfPartMode");
		InputLockManager.RemoveControlLock("EditorLogic_rootPartMode");
		shipNameField.onEndEdit.RemoveListener(onShipNameFieldSubmit);
		shipNameField.onValueChanged.RemoveListener(onShipNameFieldValueChanged);
		if (craftBrowserDialog != null)
		{
			craftBrowserDialog.Dismiss();
		}
		if (fetch != null && fetch == this)
		{
			fetch = null;
		}
	}

	public IEnumerator Start()
	{
		if (EditorDriver.CanRun)
		{
			Time.timeScale = 1f;
			audioSource.volume = GameSettings.UI_VOLUME;
			partPanelBtn.onClick.AddListener(SelectPanelParts);
			actionPanelBtn.onClick.AddListener(SelectPanelActions);
			crewPanelBtn.onClick.AddListener(SelectPanelCrew);
			cargoPanelBtn.onClick.AddListener(SelectPanelCargo);
			switchEditorBtn.onClick.AddListener(SwitchEditor);
			if (EditorDriver.editorFacility == EditorFacility.const_2)
			{
				editorBtnStateToggle.SetState(state: false);
			}
			else
			{
				editorBtnStateToggle.SetState(state: true);
			}
			symmetryButton.onClick.Add(symButton);
			symmetrySprite.SetState(symmetryMode);
			angleSnapButton.onClick.Add(snapButton);
			angleSnapSprite.SetState(GameSettings.VAB_USE_ANGLE_SNAP ? 1 : 0);
			saveBtn.onClick.AddListener(saveShip);
			exitBtn.onClick.AddListener(exitEditor);
			launchBtn.onClick.AddListener(launchVessel);
			loadBtn.onClick.AddListener(loadShip);
			newBtn.onClick.AddListener(NewShip);
			if (SteamManager.Initialized)
			{
				steamBtn.onClick.AddListener(steamExport);
			}
			coordSpaceBtn.onClick.AddListener(changeCoordSpace);
			radialSymmetryBtn.onClick.AddListener(changeRadialSymmetrySpace);
			shipNameField.onEndEdit.AddListener(onShipNameFieldSubmit);
			shipNameField.onValueChanged.AddListener(onShipNameFieldValueChanged);
			onSymMethodChange(symmetryMethod);
			SetupFSM();
			StartEditor(isRestart: false);
			if (rootPart != null)
			{
				rootPart.gameObject.SetLayerRecursive(0, filterTranslucent: true, 2097152);
				fsm.StartFSM(st_idle);
			}
			else
			{
				fsm.StartFSM(st_podSelect);
			}
			yield return null;
			Highlighter.HighlighterLimit = GameSettings.PART_HIGHLIGHTER_BRIGHTNESSFACTOR;
			shipNameField.ForceLabelUpdate();
			shipNameField.textComponent.gameObject.SetActive(value: false);
			shipNameField.textComponent.gameObject.SetActive(value: true);
			shipDescriptionField.ForceLabelUpdate();
			shipDescriptionField.textComponent.gameObject.SetActive(value: false);
			shipDescriptionField.textComponent.gameObject.SetActive(value: true);
			InitAnalytics();
			availableCargoParts = PartLoader.Instance.GetAvailableCargoPartNames();
		}
	}

	public void StartEditor(bool isRestart)
	{
		if (isRestart)
		{
			GameEvents.onEditorRestart.Fire();
		}
		WipeTooltips();
		if (!EditorDriver.fetch.restartingEditor)
		{
			InputLockManager.RemoveControlLock("Editor_outOfPartMode");
			InputLockManager.RemoveControlLock("EditorLogic_rootPartMode");
		}
		onSymMethodChange(symmetryMethod);
		angleSnapButton.Interactable = GameSettings.VAB_USE_ANGLE_SNAP;
		panelButtonsLocked = false;
		SelectPanelParts(isReset: true);
		actionPanelBtn.onClick.RemoveListener(SelectPanelActions);
		crewPanelBtn.onClick.RemoveListener(SelectPanelCrew);
		cargoPanelBtn.onClick.RemoveListener(SelectPanelCargo);
		actionPanelBtn.onClick.AddListener(SelectPanelActions);
		crewPanelBtn.onClick.AddListener(SelectPanelCrew);
		cargoPanelBtn.onClick.AddListener(SelectPanelCargo);
		switchEditorBtn.onClick.AddListener(SwitchEditor);
		if (EditorDriver.editorFacility == EditorFacility.const_2)
		{
			editorBtnStateToggle.SetState(state: false);
		}
		else
		{
			editorBtnStateToggle.SetState(state: true);
		}
		if (HighLogic.CurrentGame.Mode == Game.Modes.MISSION_BUILDER)
		{
			crewPanelBtn.gameObject.SetActive(value: false);
		}
		else
		{
			crewPanelBtn.gameObject.SetActive(value: true);
		}
		if (!GameVariables.Instance.UnlockedActionGroupsStock(ScenarioUpgradeableFacilities.GetFacilityLevel(EditorDriver.editorFacility.ToFacility()), EditorDriver.editorFacility.ToFacility() == SpaceCenterFacility.VehicleAssemblyBuilding))
		{
			if (actionPanelBtn.gameObject.activeSelf)
			{
				actionPanelBtn.gameObject.SetActive(value: false);
				switchEditorBtn.transform.position = (crewPanelBtn.gameObject.activeSelf ? cargoBtnOrigPos.transform.position : crewPanelBtn.transform.position);
				cargoPanelBtn.transform.position = (crewPanelBtn.gameObject.activeSelf ? crewBtnOrigPos.transform.position : actionPanelBtn.transform.position);
				crewPanelBtn.transform.position = actionPanelBtn.transform.position;
			}
		}
		else
		{
			actionPanelBtn.gameObject.SetActive(value: true);
			crewPanelBtn.transform.position = crewBtnOrigPos.position;
			if (!crewPanelBtn.gameObject.activeSelf)
			{
				cargoPanelBtn.transform.position = crewBtnOrigPos.position;
				switchEditorBtn.transform.position = cargoBtnOrigPos.position;
			}
			else
			{
				cargoPanelBtn.transform.position = cargoBtnOrigPos.position;
				switchEditorBtn.transform.position = editorBtnOrigPos.position;
			}
		}
		undoLevel = 0;
		undoLimit = GameSettings.EDITOR_UNDO_REDO_LIMIT;
		ShipConstruction.ClearBackups();
		if (ship != null && ship.vesselDeltaV != null)
		{
			ship.vesselDeltaV.gameObject.DestroyGameObject();
		}
		ship = new ShipConstruct(EditorDriver.editorFacility);
		UpdateUI();
		switch (EditorDriver.StartupBehaviour)
		{
		case EditorDriver.StartupBehaviours.LOAD_FROM_CACHE:
			if (ShipConstruction.ShipConfig != null)
			{
				if ((bool)FlightGlobals.fetch)
				{
					FlightGlobals.PersistentVesselIds.Remove(ship.persistentId);
				}
				if (ship != null && ship.vesselDeltaV != null)
				{
					ship.vesselDeltaV.gameObject.DestroyGameObject();
				}
				ship = ShipConstruction.LoadShip();
				if (ship != null && ship.parts.Count != 0)
				{
					rootPart = ship.parts[0].localRoot;
					shipNameField.text = Localizer.Format(ship.shipName);
					shipDescriptionField.text = Localizer.Format(ship.shipDescription);
					FlagURL = ship.missionFlag;
					editorScreen = EditorScreen.Parts;
					SetBackup();
					vesselNameAtLastSave = ship.shipName;
					SetLastSanitizedSaveName();
					bool flag = false;
					if (ShipConstruction.ShipManifest != null)
					{
						flag = ShipConstruction.ShipManifest.AnyCrewInState(ProtoCrewMember.RosterStatus.Available, notInState: true);
					}
					if (flag)
					{
						Debug.Log("[Crew Manifest]: Cannot resume cached manifest. The crew in it isn't available. Reverting to default.");
						ResetCrewAssignment(ShipConstruction.ShipConfig, allowAutoHire: false);
					}
					else if (CrewAssignmentDialog.Instance != null)
					{
						CrewAssignmentDialog.Instance.SetDefaultManifest(HighLogic.CurrentGame.CrewRoster.DefaultCrewForVessel(ShipConstruction.ShipConfig, null, autohire: false));
						CrewAssignmentDialog.Instance.RefreshCrewLists(ShipConstruction.ShipManifest, setAsDefault: false, updateUI: false);
					}
					if (ShipConstruction.ShipManifest == null)
					{
						ResetCrewAssignment(ShipConstruction.ShipConfig, allowAutoHire: false);
					}
					break;
				}
				if (ship != null && ship.vesselDeltaV != null)
				{
					ship.vesselDeltaV.gameObject.DestroyGameObject();
				}
				ship = new ShipConstruct(EditorDriver.editorFacility);
				ship.vesselDeltaV = VesselDeltaV.Create(ship);
			}
			goto default;
		case EditorDriver.StartupBehaviours.LOAD_FROM_FILE:
			FlagURL = "";
			if (!File.Exists(EditorDriver.filePathToLoad))
			{
				Debug.LogError(("[EditorDriver]: No file found at " + EditorDriver.filePathToLoad) ?? "");
			}
			else
			{
				if ((bool)FlightGlobals.fetch)
				{
					FlightGlobals.PersistentVesselIds.Remove(ship.persistentId);
				}
				if (ship != null && ship.vesselDeltaV != null)
				{
					ship.vesselDeltaV.gameObject.DestroyGameObject();
				}
				ship = ShipConstruction.LoadShip(EditorDriver.filePathToLoad);
				if (ship != null)
				{
					rootPart = ship.parts[0].localRoot;
					shipNameField.text = Localizer.Format(ship.shipName);
					shipDescriptionField.text = Localizer.Format(ship.shipDescription);
					FlagURL = ship.missionFlag;
					editorScreen = EditorScreen.Parts;
					SetBackup();
					vesselNameAtLastSave = ship.shipName;
					SetLastSanitizedSaveName();
					ResetCrewAssignment(ShipConstruction.ShipConfig, allowAutoHire: false);
					break;
				}
				ship = new ShipConstruct(EditorDriver.editorFacility);
				ship.vesselDeltaV = VesselDeltaV.Create(ship);
			}
			goto default;
		default:
			ship.shipName = Localizer.Format("#autoLOC_900530");
			FlagURL = string.Empty;
			if (HighLogic.CurrentGame != null && (HighLogic.CurrentGame.Mode == Game.Modes.MISSION_BUILDER || HighLogic.CurrentGame.Mode == Game.Modes.MISSION) && ExpansionsLoader.IsExpansionInstalled("MakingHistory") && MissionSystem.missions.Count > 0)
			{
				FlagURL = MissionSystem.missions[0].flagURL;
			}
			rootPart = null;
			shipNameField.text = Localizer.Format(ship.shipName);
			shipDescriptionField.text = Localizer.Format(ship.shipDescription);
			vesselNameAtLastSave = string.Empty;
			SetLastSanitizedSaveName();
			break;
		}
		ship.shipFacility = EditorDriver.editorFacility;
		EditorPanels.Instance.ShowPartsList();
		Input.ResetInputAxes();
		FlagSetup();
		undoIndexAtLastSave = undoLevel;
		if (isRestart)
		{
			if (rootPart != null)
			{
				fsm.RunEvent(on_shipLoaded);
			}
			else
			{
				fsm.RunEvent(on_newShip);
			}
		}
		GameEvents.onEditorStarted.Fire();
		for (int i = 0; i < ship.parts.Count; i++)
		{
			if (ship.parts[i].isSolarPanel(out var solarPanel) && solarPanel.deployState == ModuleDeployablePart.DeployState.EXTENDED)
			{
				solarPanel.playAnimationOnStart = true;
			}
			if (ship.parts[i].isRadiator(out var radiator) && radiator.deployState == ModuleDeployablePart.DeployState.EXTENDED)
			{
				radiator.playAnimationOnStart = true;
			}
			if (ship.parts[i].isAntenna(out var antenna) && antenna.deployState == ModuleDeployablePart.DeployState.EXTENDED)
			{
				antenna.playAnimationOnStart = true;
			}
		}
	}

	public void FlagSetup()
	{
		if (HighLogic.CurrentGame != null)
		{
			if (FlagURL == string.Empty)
			{
				FlagURL = HighLogic.CurrentGame.flagURL;
				if (HighLogic.CurrentGame != null && (HighLogic.CurrentGame.Mode == Game.Modes.MISSION_BUILDER || HighLogic.CurrentGame.Mode == Game.Modes.MISSION) && ExpansionsLoader.IsExpansionInstalled("MakingHistory") && MissionSystem.missions.Count > 0)
				{
					FlagURL = MissionSystem.missions[0].flagURL;
				}
			}
			if (GameDatabase.Instance != null)
			{
				Texture2D texture = GameDatabase.Instance.GetTexture(FlagURL, asNormalMap: false);
				flagBrowserButton.Setup(texture, delegate
				{
					Lock(lockLoad: true, lockExit: true, lockSave: true, "EditorLogic_flagDialog");
					InputLockManager.SetControlLock(ControlTypes.EDITOR_SOFT_LOCK, "EditorLogic_dialog_softLock");
					ScreenMessages.PostScreenMessage("", modeMsg);
				}, OnMissionFlagSelect, delegate
				{
					Unlock("EditorLogic_flagDialog");
					StartCoroutine(DelayedUnlock());
				});
			}
			else
			{
				Debug.LogError("[EditorLogic Error]: Flag Browser button is set, but GameDatabase has no instance!");
			}
		}
		else
		{
			Debug.LogError("[EditorLogic Error]: Flag Browser button is set, but HighLogic has no currentGame!");
		}
	}

	public void OnMissionFlagSelect(FlagBrowser.FlagEntry selected)
	{
		FlagURL = selected.textureInfo.name;
		StartCoroutine(DelayedUnlock());
		Unlock("EditorLogic_flagDialog");
		GameEvents.onMissionFlagSelect.Fire(FlagURL);
	}

	public void SelectPanelParts()
	{
		SelectPanelParts(isReset: false);
	}

	public void SelectPanelCargo()
	{
		SelectPanelParts(isReset: true);
		if (EditorPanels.Instance.ShowCargo(delegate
		{
			panelButtonsLocked = false;
			UpdateUI();
		}))
		{
			editorScreen = EditorScreen.Cargo;
			panelButtonsLocked = true;
			actionPanelBtn.interactable = false;
			crewPanelBtn.interactable = false;
			partPanelBtn.interactable = false;
			cargoPanelBtn.interactable = false;
			switchEditorBtn.interactable = false;
			UpdateCrewAssignment();
			EditorActionGroups.Instance.DeactivateInterface(ship);
			UIPartActionController.Instance.Activate();
			GameEvents.onEditorScreenChange.Fire(EditorScreen.Cargo);
			InputLockManager.SetControlLock(ControlTypes.EDITOR_ICON_HOVER | ControlTypes.EDITOR_TAB_SWITCH | ControlTypes.EDITOR_PAD_PICK_COPY, "Editor_outOfPartMode");
		}
	}

	public void SelectPanelParts(bool isReset)
	{
		if (EditorPanels.Instance.ShowPartsList(delegate
		{
			panelButtonsLocked = false;
			UpdateUI();
		}))
		{
			editorScreen = EditorScreen.Parts;
			panelButtonsLocked = true;
			actionPanelBtn.interactable = false;
			crewPanelBtn.interactable = false;
			partPanelBtn.interactable = false;
			cargoPanelBtn.interactable = false;
			switchEditorBtn.interactable = false;
			if (!isReset)
			{
				UpdateCrewAssignment();
				EditorActionGroups.Instance.DeactivateInterface(ship);
				UIPartActionController.Instance.Activate();
				GameEvents.onEditorScreenChange.Fire(EditorScreen.Parts);
				InputLockManager.RemoveControlLock("Editor_outOfPartMode");
			}
			else if (InventoryPanelController.Instance != null)
			{
				InventoryPanelController.Instance.Reset();
			}
		}
	}

	public void SelectPanelActions()
	{
		if ((!(selectedPart != null) || constructionMode != 0) && ship.Count > 0 && EditorPanels.Instance.ShowActionGroups(delegate
		{
			panelButtonsLocked = false;
			UpdateUI();
		}))
		{
			GameEvents.onGUIActionGroupShowing.Fire();
			editorScreen = EditorScreen.Actions;
			panelButtonsLocked = true;
			actionPanelBtn.interactable = false;
			crewPanelBtn.interactable = false;
			partPanelBtn.interactable = false;
			cargoPanelBtn.interactable = false;
			switchEditorBtn.interactable = false;
			EditorActionGroups.Instance.ActivateInterface(ship);
			UpdateCrewAssignment();
			toolsUI.SetMode(ConstructionMode.Place);
			UIPartActionController.Instance.Deactivate();
			GameEvents.onEditorScreenChange.Fire(EditorScreen.Actions);
			InputLockManager.SetControlLock(ControlTypes.EDITOR_ICON_HOVER | ControlTypes.EDITOR_ICON_PICK | ControlTypes.EDITOR_TAB_SWITCH | ControlTypes.EDITOR_PAD_PICK_PLACE | ControlTypes.EDITOR_PAD_PICK_COPY | ControlTypes.EDITOR_GIZMO_TOOLS | ControlTypes.EDITOR_ROOT_REFLOW | ControlTypes.EDITOR_SYM_SNAP_UI | ControlTypes.EDITOR_UNDO_REDO, "Editor_outOfPartMode");
		}
	}

	public void SwitchEditor()
	{
		if ((selectedPart != null && constructionMode == ConstructionMode.Place) || EditorDriver.fetch.restartingEditor)
		{
			return;
		}
		string text = ((EditorDriver.editorFacility == EditorFacility.const_1) ? "SpaceplaneHangar" : "VehicleAssemblyBuilding");
		PSystemSetup.SpaceCenterFacility spaceCenterFacility = PSystemSetup.Instance.GetSpaceCenterFacility(text);
		if (spaceCenterFacility != null && !(spaceCenterFacility.GetFacilityDamage() < 100f))
		{
			string text2 = "";
			text2 = ((EditorDriver.editorFacility != EditorFacility.const_1) ? Localizer.Format("#autoLOC_418766") : Localizer.Format("#autoLOC_418770"));
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_253289", text2), 5f);
			return;
		}
		SelectPanelParts(isReset: false);
		if (ship.Count > 0)
		{
			ShipConstruction.ShipConfig = ship.SaveShip();
		}
		else
		{
			ShipConstruction.ShipConfig = null;
		}
		panelButtonsLocked = true;
		actionPanelBtn.interactable = false;
		crewPanelBtn.interactable = false;
		partPanelBtn.interactable = false;
		cargoPanelBtn.interactable = false;
		switchEditorBtn.interactable = false;
		int facility = ((EditorDriver.editorFacility != EditorFacility.const_1) ? 1 : 2);
		EditorDriver.StartupBehaviour = EditorDriver.StartupBehaviours.LOAD_FROM_CACHE;
		EditorDriver.SwitchEditor((EditorFacility)facility);
	}

	public void SelectPanelCrew()
	{
		if ((!(selectedPart != null) || constructionMode != 0) && ship.Count > 0 && EditorPanels.Instance.ShowCrewAssignment(delegate
		{
			panelButtonsLocked = false;
			UpdateUI();
		}))
		{
			editorScreen = EditorScreen.Crew;
			panelButtonsLocked = true;
			actionPanelBtn.interactable = false;
			crewPanelBtn.interactable = false;
			partPanelBtn.interactable = false;
			cargoPanelBtn.interactable = false;
			switchEditorBtn.interactable = false;
			toolsUI.SetMode(ConstructionMode.Place);
			UIPartActionController.Instance.Deactivate();
			EditorActionGroups.Instance.DeactivateInterface(ship);
			RefreshCrewDialog();
			GameEvents.onEditorScreenChange.Fire(EditorScreen.Crew);
			InputLockManager.SetControlLock(ControlTypes.EDITOR_ICON_HOVER | ControlTypes.EDITOR_ICON_PICK | ControlTypes.EDITOR_TAB_SWITCH | ControlTypes.EDITOR_PAD_PICK_PLACE | ControlTypes.EDITOR_PAD_PICK_COPY | ControlTypes.EDITOR_GIZMO_TOOLS | ControlTypes.EDITOR_ROOT_REFLOW | ControlTypes.EDITOR_SYM_SNAP_UI | ControlTypes.EDITOR_UNDO_REDO, "Editor_outOfPartMode");
		}
	}

	public void symButton(PointerEventData evtData = null)
	{
		int num = symmetryMode;
		if (evtData != null && evtData.button != 0)
		{
			if (evtData.button == PointerEventData.InputButton.Middle)
			{
				RestoreSymmetryState();
				SymmetryMethod symmetryMethod = this.symmetryMethod;
				switch (this.symmetryMethod)
				{
				case SymmetryMethod.Mirror:
					this.symmetryMethod = SymmetryMethod.Radial;
					break;
				case SymmetryMethod.Radial:
					this.symmetryMethod = SymmetryMethod.Mirror;
					break;
				}
				if (fsm.CurrentState == st_place)
				{
					radialSymmetryBtn.gameObject.SetActive(this.symmetryMethod == SymmetryMethod.Radial);
				}
				Debug.LogFormat("SymmetryMethod Changed from {0} to {1}", symmetryMethod, this.symmetryMethod);
				GameEvents.onEditorSymmetryMethodChange.Fire(this.symmetryMethod);
			}
			else
			{
				prevSymMode();
				Debug.LogFormat("SymmetryMode Changed from {0} to {1}", num, symmetryMode);
			}
		}
		else
		{
			nextSymMode();
			Debug.LogFormat("SymmetryMode Changed from {0} to {1}", num, symmetryMode);
		}
	}

	public void nextSymMode()
	{
		if (symmetryMethod == SymmetryMethod.Radial)
		{
			symmetryMode++;
			if (symmetryMode == 4)
			{
				symmetryMode = 5;
			}
			if (symmetryMode == 6)
			{
				symmetryMode = 7;
			}
			if (symmetryMode >= 8)
			{
				symmetryMode = 0;
			}
		}
		else
		{
			symmetryMode++;
			if (symmetryMode >= 2)
			{
				symmetryMode = 0;
			}
		}
		GameEvents.onEditorSymmetryModeChange.Fire(symmetryMode);
		SetSymState();
	}

	public void prevSymMode()
	{
		if (symmetryMethod == SymmetryMethod.Radial)
		{
			symmetryMode--;
			if (symmetryMode == 4)
			{
				symmetryMode = 3;
			}
			if (symmetryMode == 6)
			{
				symmetryMode = 5;
			}
			if (symmetryMode <= -1)
			{
				symmetryMode = 7;
			}
		}
		else
		{
			symmetryMode--;
			if (symmetryMode <= -1)
			{
				symmetryMode = 1;
			}
		}
		GameEvents.onEditorSymmetryModeChange.Fire(symmetryMode);
		SetSymState();
	}

	public void SetSymState()
	{
		RestoreSymmetryState();
		switch (symmetryMethod)
		{
		case SymmetryMethod.Mirror:
			mirrorSprite.SetState(symmetryMode);
			break;
		case SymmetryMethod.Radial:
			if (symmetryMode == 7)
			{
				symmetrySprite.SetState(5);
			}
			else if (symmetryMode == 5)
			{
				symmetrySprite.SetState(4);
			}
			else
			{
				symmetrySprite.SetState(symmetryMode);
			}
			break;
		}
	}

	public void snapButton(PointerEventData evtData = null)
	{
		Debug.Log("SnapMode");
		GameSettings.VAB_USE_ANGLE_SNAP = !GameSettings.VAB_USE_ANGLE_SNAP;
		angleSnapSprite.SetState(GameSettings.VAB_USE_ANGLE_SNAP ? 1 : 0);
		GameEvents.onEditorSnapModeChange.Fire(GameSettings.VAB_USE_ANGLE_SNAP);
	}

	public void symInputUpdate()
	{
		if (!InputLockManager.IsUnlocked(ControlTypes.EDITOR_SYM_SNAP_UI) || AnyTextFieldHasFocus() || DeltaVApp.AnyTextFieldHasFocus() || RoboticControllerManager.AnyWindowTextFieldHasFocus())
		{
			return;
		}
		if (GameSettings.Editor_toggleSymMode.GetKeyDown())
		{
			RestoreSymmetryState();
			if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
			{
				nextSymMode();
			}
			else
			{
				prevSymMode();
			}
		}
		if (GameSettings.Editor_toggleSymMethod.GetKeyUp())
		{
			RestoreSymmetryState();
			switch (symmetryMethod)
			{
			case SymmetryMethod.Mirror:
				symmetryMethod = SymmetryMethod.Radial;
				break;
			case SymmetryMethod.Radial:
				symmetryMethod = SymmetryMethod.Mirror;
				break;
			}
			GameEvents.onEditorSymmetryMethodChange.Fire(symmetryMethod);
		}
		if (!GameSettings.Editor_coordSystem.GetKeyDown())
		{
			return;
		}
		RestoreSymmetryState();
		if (symmetryMethod == SymmetryMethod.Radial)
		{
			switch (symmetryCoordSpace)
			{
			case Space.Self:
				symmetryCoordSpace = Space.World;
				radialSymmetryText.text = EditorLogicBase.cacheAutoLOC_6001220;
				ScreenMessages.PostScreenMessage(EditorLogicBase.cacheAutoLOC_6001225, modeMsg);
				break;
			case Space.World:
				symmetryCoordSpace = Space.Self;
				radialSymmetryText.text = EditorLogicBase.cacheAutoLOC_6001219;
				ScreenMessages.PostScreenMessage(EditorLogicBase.cacheAutoLOC_6001226, modeMsg);
				break;
			}
			GameEvents.onEditorSymmetryCoordsChange.Fire(symmetryCoordSpace);
		}
	}

	public void snapInputUpdate()
	{
		if (InputLockManager.IsUnlocked(ControlTypes.EDITOR_SYM_SNAP_UI) && !AnyTextFieldHasFocus() && !DeltaVApp.AnyTextFieldHasFocus() && !RoboticControllerManager.AnyWindowTextFieldHasFocus() && GameSettings.Editor_toggleAngleSnap.GetKeyDown())
		{
			snapButton();
		}
	}

	public void onSymMethodChange(SymmetryMethod method)
	{
		switch (method)
		{
		case SymmetryMethod.Mirror:
			symmetrySprite.gameObject.SetActive(value: false);
			mirrorSprite.gameObject.SetActive(value: true);
			symmetryMode = (int)Mathf.Clamp01(symmetryMode);
			break;
		case SymmetryMethod.Radial:
			symmetrySprite.gameObject.SetActive(value: true);
			mirrorSprite.gameObject.SetActive(value: false);
			break;
		}
		SetSymState();
		if (fsm.Started)
		{
			ScreenMessages.PostScreenMessage(EditorLogicBase.cacheAutoLOC_6004038 + " " + EditorEnumExtensions.GetSymmetryMethodName(method), modeMsg);
		}
	}

	public void editorSwitchInput(bool keyOverride = false)
	{
		if ((Input.GetKeyUp(KeyCode.Insert) || keyOverride) && selectedPart == null)
		{
			switch (EditorDriver.editorFacility)
			{
			case EditorFacility.const_2:
				EditorDriver.SwitchEditor(EditorFacility.const_1);
				break;
			case EditorFacility.const_1:
				EditorDriver.SwitchEditor(EditorFacility.const_2);
				break;
			}
			ship.shipFacility = EditorDriver.editorFacility;
			Input.ResetInputAxes();
		}
	}

	public void SetupFSM()
	{
		fsm = new KerbalFSM();
		st_podSelect = new KFSMState("st_podSelect");
		st_podSelect.OnEnter = delegate(KFSMState from)
		{
			if (from == st_podSelect)
			{
				if (!EditorDriver.fetch.restartingEditor)
				{
					Debug.LogWarning("[EditorLogic Warning]: Trying to set rootPartMode, but mode is already set.", base.gameObject);
				}
			}
			else
			{
				EditorPartList.Instance.ExcludeFilters.AddFilter(inaccessiblePartsFilter);
				EditorPartList.Instance.GreyoutFilters.AddFilter(rootPartsOnlyFilter);
				EditorPartList.Instance.Refresh();
				editorScreen = EditorScreen.Parts;
				InputLockManager.SetControlLock(~(ControlTypes.flag_2 | ControlTypes.EDITOR_ICON_HOVER | ControlTypes.EDITOR_ICON_PICK | ControlTypes.EDITOR_TAB_SWITCH | ControlTypes.EDITOR_LOAD | ControlTypes.EDITOR_EXIT | ControlTypes.EDITOR_UNDO_REDO | ControlTypes.flag_53), "EditorLogic_rootPartMode");
				editorCamera.SendMessage("ResetCamera");
			}
		};
		KFSMState kFSMState = st_podSelect;
		kFSMState.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState.OnUpdate, new KFSMCallback(partSearchUpdate));
		KFSMState kFSMState2 = st_podSelect;
		kFSMState2.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState2.OnUpdate, new KFSMCallback(UndoRedoInputUpdate));
		st_podSelect.OnLeave = delegate(KFSMState to)
		{
			if (to == st_idle)
			{
				if (fsm.LastEvent == on_podSelect && rootPart != null && !ship.parts.Contains(rootPart))
				{
					rootPart.transform.position = initialPodPosition;
					rootPart.attPos0 = initialPodPosition;
					rootPart.transform.rotation = vesselRotation;
					rootPart.attRotation0 = vesselRotation;
					RestoreSymmetryState();
					rootPart.symMethod = symmetryMethod;
					addToShip(rootPart);
					bool num8 = undoLevel == 0;
					SetBackup();
					if (ship != null && ship.vesselDeltaV != null)
					{
						ship.vesselDeltaV.gameObject.DestroyGameObject();
					}
					ship.vesselDeltaV = VesselDeltaV.Create(ship);
					GameEvents.onEditorPodPicked.Fire(rootPart);
					if (UIPartActionControllerInventory.Instance != null)
					{
						UIPartActionControllerInventory.Instance.CurrentCargoPart = null;
						UIPartActionControllerInventory.Instance.DestroyHeldPartAsIcon();
					}
					if (num8)
					{
						ResetCrewAssignment(ShipConstruction.ShipConfig, allowAutoHire: true);
					}
					else
					{
						ShipConstruction.ShipManifest.Filter(GetPartExistsFilter());
						VesselCrewManifest.MergeInto(ShipConstruction.ShipManifest, HighLogic.CurrentGame.CrewRoster.DefaultCrewForVessel(ShipConstruction.ShipConfig, ShipConstruction.ShipManifest), GetPartExistsFilter());
						CrewAssignmentDialog.Instance.RefreshCrewLists(ShipConstruction.ShipManifest, setAsDefault: false, updateUI: false);
					}
					audioSource.PlayOneShot(attachClip);
				}
				InputLockManager.RemoveControlLock("EditorLogic_rootPartMode");
				actionPanelBtn.interactable = InputLockManager.IsUnlocked(ControlTypes.EDITOR_MODE_SWITCH);
				crewPanelBtn.interactable = InputLockManager.IsUnlocked(ControlTypes.EDITOR_MODE_SWITCH | ControlTypes.EDITOR_CREW);
				switchEditorBtn.interactable = true;
				EditorPanels.Instance.ShowPartsList();
				EditorPartList.Instance.GreyoutFilters.RemoveFilter(rootPartsOnlyFilter);
				EditorPartList.Instance.Refresh();
			}
		};
		fsm.AddState(st_podSelect);
		st_idle = new KFSMState("st_idle");
		st_idle.OnEnter = delegate
		{
		};
		st_idle.OnUpdate = delegate
		{
		};
		KFSMState kFSMState3 = st_idle;
		kFSMState3.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState3.OnUpdate, new KFSMCallback(symInputUpdate));
		KFSMState kFSMState4 = st_idle;
		kFSMState4.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState4.OnUpdate, new KFSMCallback(snapInputUpdate));
		KFSMState kFSMState5 = st_idle;
		kFSMState5.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState5.OnUpdate, new KFSMCallback(UndoRedoInputUpdate));
		KFSMState kFSMState6 = st_idle;
		kFSMState6.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState6.OnUpdate, new KFSMCallback(partSearchUpdate));
		st_idle.OnLeave = delegate
		{
		};
		fsm.AddState(st_idle);
		st_place = new KFSMState("st_place");
		st_place.OnEnter = delegate
		{
			selectedPart.gameObject.SetLayerRecursive(1, filterTranslucent: true, 2097152);
			deleteSymmetryParts();
			RestoreSymmetryState();
			if (UIPartActionControllerInventory.Instance != null && UIPartActionControllerInventory.Instance.editorPartPickedBlockSfx)
			{
				UIPartActionControllerInventory.Instance.editorPartPickedBlockSfx = false;
			}
			else
			{
				audioSource.PlayOneShot(partGrabClip);
			}
			if (selectedPart == RootPart)
			{
				GameEvents.onEditorPodSelected.Fire(RootPart);
			}
			if (symmetryMethod == SymmetryMethod.Radial && editorScreen != EditorScreen.Cargo)
			{
				radialSymmetryBtn.gameObject.SetActive(value: true);
			}
			radialSymmetryText.text = ((symmetryCoordSpace == Space.World) ? EditorLogicBase.cacheAutoLOC_6001220 : EditorLogicBase.cacheAutoLOC_6001219);
		};
		KFSMState kFSMState7 = st_place;
		kFSMState7.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState7.OnUpdate, new KFSMCallback(symInputUpdate));
		KFSMState kFSMState8 = st_place;
		kFSMState8.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState8.OnUpdate, new KFSMCallback(snapInputUpdate));
		KFSMState kFSMState9 = st_place;
		kFSMState9.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState9.OnUpdate, new KFSMCallback(UndoRedoInputUpdate));
		KFSMState kFSMState10 = st_place;
		kFSMState10.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState10.OnUpdate, new KFSMCallback(partSearchUpdate));
		KFSMState kFSMState11 = st_place;
		kFSMState11.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState11.OnUpdate, (KFSMCallback)delegate
		{
			if (Mouse.Right.GetButtonUp() && Mouse.Right.WasDragging())
			{
				srfAttachCursorOffset = Vector3.zero;
			}
			if (selectedPart != null)
			{
				Vector3 position = Vector3.zero;
				if (dragOverPlane(out position))
				{
					selectedPart.transform.position = Vector3.Lerp(selectedPart.transform.position, position, dragSharpness * Time.deltaTime);
				}
				selectedPart.transform.rotation = vesselRotation * selectedPart.initRotation * selectedPart.attRotation;
				if (!skipPartAttach)
				{
					attachment = checkAttach(selectedPart);
				}
				else if (attachment != null)
				{
					attachment.possible = false;
				}
				if (HeldPartIsStacked())
				{
					attachment.possible = false;
				}
				selectedPart.attRotation0 = attachment.rotation;
				selectedPart.attPos0 = attachment.position;
				if (attachment.possible)
				{
					selectedPart.transform.rotation = attachment.rotation * selectedPart.attRotation;
				}
				selectedPart.transform.position = attachment.position;
				if (selectedPart.isCargoPart() && !selectedPart.attachRules.StackOrSurfaceAttachable)
				{
					bool flag3 = false;
					if (Mouse.HoveredPart != null)
					{
						ModuleInventoryPart moduleInventoryPart = Mouse.HoveredPart.FindModuleImplementing<ModuleInventoryPart>();
						if (moduleInventoryPart != null && moduleInventoryPart.TotalEmptySlots() > 0)
						{
							highlightSelected(attach: true, selectedPart);
							flag3 = true;
						}
					}
					if (!flag3)
					{
						highlightSelected(attach: false, selectedPart);
					}
				}
				else
				{
					highlightSelected(attachment.possible, selectedPart);
				}
				if (selectedPart.potentialParent != null && attachment.potentialParent != selectedPart.potentialParent)
				{
					deleteSymmetryParts();
					wipeSymmetry(selectedPart);
				}
				selectedPart.potentialParent = attachment.potentialParent;
				if (attachment.potentialParent != null)
				{
					createSymmetry(symmetryMode, attachment);
				}
				else
				{
					deleteSymmetryParts();
					if (symmetryModeBeforeNodeAttachment >= 0)
					{
						RestoreSymmetryModeBeforeNodeAttachment();
					}
				}
				if (attachment != null)
				{
					cPartAttachments = CheckSymPartsAttach(attachment);
				}
				displayAttachNodeIcons(ship.parts, selectedPart, selectedPart.attachRules.stack, selectedPart.attachRules.srfAttach, selectedPart.attachRules.allowDock);
				GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartDragging, selectedPart);
			}
		});
		KFSMState kFSMState12 = st_place;
		kFSMState12.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState12.OnUpdate, new KFSMCallback(base.partRotationInputUpdate));
		KFSMState kFSMState13 = st_place;
		kFSMState13.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState13.OnUpdate, new KFSMCallback(deleteInputUpdate));
		st_place.OnLeave = delegate(KFSMState to)
		{
			if (selectedPart != null)
			{
				displayAttachNodeIcons(ship.parts, selectedPart, stackNodes: false, srfNodes: false, dockNodes: false);
				if (attachment.possible)
				{
					selectedPart.gameObject.SetLayerRecursive(0, filterTranslucent: true, 2097152);
				}
			}
			cleanSymmetry();
			CheckCopiedSymmetryPersistentId(selectedPart);
			GameEvents.onEditorPartPlaced.Fire(selectedPart);
			if (to == st_idle && selectedPart != null)
			{
				selectedPart = null;
				if ((bool)UIPartActionControllerInventory.Instance)
				{
					UIPartActionControllerInventory.Instance.CurrentCargoPart = null;
				}
			}
			if (to != st_rotate_tweak)
			{
				srfAttachCursorOffset = Vector3.zero;
			}
			radialSymmetryBtn.gameObject.SetActive(value: false);
		};
		fsm.AddState(st_place);
		st_offset_select = new KFSMState("st_offset_select");
		KFSMState kFSMState14 = st_offset_select;
		kFSMState14.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState14.OnUpdate, new KFSMCallback(UndoRedoInputUpdate));
		KFSMState kFSMState15 = st_offset_select;
		kFSMState15.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState15.OnUpdate, new KFSMCallback(snapInputUpdate));
		KFSMState kFSMState16 = st_offset_select;
		kFSMState16.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState16.OnUpdate, new KFSMCallback(partSearchUpdate));
		fsm.AddState(st_offset_select);
		st_offset_tweak = new KFSMState("st_offset_tweak");
		st_offset_tweak.OnEnter = delegate
		{
			selectedPart.onEditorStartTweak();
			Transform referenceTransform2 = selectedPart.GetReferenceTransform();
			symUpdateMode = selectedPart.symmetryCounterparts.Count;
			if (ship.Contains(selectedPart))
			{
				symUpdateParent = selectedPart.parent;
				symUpdateAttachNode = selectedPart.FindAttachNodeByPart(symUpdateParent);
			}
			else
			{
				symUpdateParent = attachment.potentialParent;
				symUpdateAttachNode = attachment.callerPartNode;
			}
			gizmoOffset = GizmoOffset.Attach(referenceTransform2, selectedPart.initRotation, onOffsetGizmoUpdate, onOffsetGizmoUpdated, editorCamera);
			coordSpaceBtn.gameObject.SetActive(value: true);
			coordSpaceText.text = ((gizmoOffset.CoordSpace == Space.World) ? EditorLogicBase.cacheAutoLOC_6001217 : EditorLogicBase.cacheAutoLOC_6001218);
			audioSource.PlayOneShot(tweakGrabClip);
		};
		KFSMState kFSMState17 = st_offset_tweak;
		kFSMState17.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState17.OnUpdate, new KFSMCallback(partOffsetInputUpdate));
		KFSMState kFSMState18 = st_offset_tweak;
		kFSMState18.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState18.OnUpdate, new KFSMCallback(UndoRedoInputUpdate));
		KFSMState kFSMState19 = st_offset_tweak;
		kFSMState19.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState19.OnUpdate, new KFSMCallback(snapInputUpdate));
		KFSMState kFSMState20 = st_offset_tweak;
		kFSMState20.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState20.OnUpdate, new KFSMCallback(deleteInputUpdate));
		KFSMState kFSMState21 = st_offset_tweak;
		kFSMState21.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState21.OnUpdate, new KFSMCallback(partSearchUpdate));
		KFSMState kFSMState22 = st_offset_tweak;
		kFSMState22.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState22.OnUpdate, (KFSMCallback)delegate
		{
			if (GameSettings.Editor_coordSystem.GetKeyUp() && !gizmoOffset.IsDragging)
			{
				switch (gizmoOffset.CoordSpace)
				{
				case Space.Self:
					gizmoOffset.SetCoordSystem(Space.World);
					coordSpaceText.text = EditorLogicBase.cacheAutoLOC_6001217;
					ScreenMessages.PostScreenMessage(EditorLogicBase.cacheAutoLOC_6001221, modeMsg);
					break;
				case Space.World:
					gizmoOffset.SetCoordSystem(Space.Self);
					coordSpaceText.text = EditorLogicBase.cacheAutoLOC_6001218;
					ScreenMessages.PostScreenMessage(EditorLogicBase.cacheAutoLOC_6001222, modeMsg);
					break;
				}
			}
		});
		st_offset_tweak.OnLeave = delegate(KFSMState to)
		{
			gizmoOffset.Detach();
			symUpdateMode = 0;
			symUpdateParent = null;
			symUpdateAttachNode = null;
			if (to != st_offset_tweak && to != st_rotate_tweak && selectedPart != null)
			{
				selectedPart.onEditorEndTweak();
				if (to == st_idle)
				{
					selectedPart = null;
				}
			}
			audioSource.PlayOneShot(tweakReleaseClip);
			coordSpaceBtn.gameObject.SetActive(value: false);
		};
		fsm.AddState(st_offset_tweak);
		st_rotate_select = new KFSMState("st_rotate_select");
		KFSMState kFSMState23 = st_rotate_select;
		kFSMState23.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState23.OnUpdate, new KFSMCallback(UndoRedoInputUpdate));
		KFSMState kFSMState24 = st_rotate_select;
		kFSMState24.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState24.OnUpdate, new KFSMCallback(snapInputUpdate));
		KFSMState kFSMState25 = st_rotate_select;
		kFSMState25.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState25.OnUpdate, new KFSMCallback(partSearchUpdate));
		fsm.AddState(st_rotate_select);
		st_rotate_tweak = new KFSMState("st_rotate_tweak");
		st_rotate_tweak.OnEnter = delegate
		{
			selectedPart.onEditorStartTweak();
			Transform referenceTransform = selectedPart.GetReferenceTransform();
			symUpdateMode = selectedPart.symmetryCounterparts.Count;
			if (ship.Contains(selectedPart))
			{
				symUpdateParent = selectedPart.parent;
				symUpdateAttachNode = selectedPart.FindAttachNodeByPart(symUpdateParent);
			}
			else
			{
				symUpdateParent = attachment.potentialParent;
				symUpdateAttachNode = attachment.callerPartNode;
			}
			if (symUpdateAttachNode != null)
			{
				gizmoPivot = referenceTransform.TransformPoint(symUpdateAttachNode.position);
			}
			else
			{
				gizmoPivot = referenceTransform.transform.position;
			}
			gizmoRotate = GizmoRotate.Attach(referenceTransform, gizmoPivot, selectedPart.initRotation, onRotateGizmoUpdate, onRotateGizmoUpdated, editorCamera);
			gizmoAttRotate = selectedPart.attRotation;
			gizmoAttRotate0 = selectedPart.attRotation0;
			coordSpaceBtn.gameObject.SetActive(value: true);
			coordSpaceText.text = ((gizmoRotate.CoordSpace == Space.World) ? EditorLogicBase.cacheAutoLOC_6001217 : EditorLogicBase.cacheAutoLOC_6001218);
			audioSource.PlayOneShot(tweakGrabClip);
		};
		KFSMState kFSMState26 = st_rotate_tweak;
		kFSMState26.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState26.OnUpdate, new KFSMCallback(base.partRotationInputUpdate));
		KFSMState kFSMState27 = st_rotate_tweak;
		kFSMState27.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState27.OnUpdate, new KFSMCallback(UndoRedoInputUpdate));
		KFSMState kFSMState28 = st_rotate_tweak;
		kFSMState28.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState28.OnUpdate, new KFSMCallback(snapInputUpdate));
		KFSMState kFSMState29 = st_rotate_tweak;
		kFSMState29.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState29.OnUpdate, new KFSMCallback(deleteInputUpdate));
		KFSMState kFSMState30 = st_rotate_tweak;
		kFSMState30.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState30.OnUpdate, new KFSMCallback(partSearchUpdate));
		KFSMState kFSMState31 = st_rotate_tweak;
		kFSMState31.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState31.OnUpdate, (KFSMCallback)delegate
		{
			if (GameSettings.Editor_coordSystem.GetKeyUp() && !gizmoRotate.IsDragging)
			{
				switch (gizmoRotate.CoordSpace)
				{
				case Space.Self:
					gizmoRotate.SetCoordSystem(Space.World);
					coordSpaceText.text = EditorLogicBase.cacheAutoLOC_6001217;
					ScreenMessages.PostScreenMessage(EditorLogicBase.cacheAutoLOC_6001223, modeMsg);
					break;
				case Space.World:
					gizmoRotate.SetCoordSystem(Space.Self);
					coordSpaceText.text = EditorLogicBase.cacheAutoLOC_6001218;
					ScreenMessages.PostScreenMessage(EditorLogicBase.cacheAutoLOC_6001224, modeMsg);
					break;
				}
			}
		});
		st_rotate_tweak.OnLeave = delegate(KFSMState to)
		{
			if (to == st_place && !ship.Contains(selectedPart) && attachment != null && attachment.possible && attachment.mode == AttachModes.SRF_ATTACH)
			{
				srfAttachCursorOffset = Input.mousePosition - editorCamera.WorldToScreenPoint(selectedPart.GetReferenceTransform().TransformPoint(attachment.callerPartNode.position));
			}
			else
			{
				srfAttachCursorOffset = Vector3.zero;
			}
			gizmoRotate.Detach();
			symUpdateMode = 0;
			symUpdateParent = null;
			symUpdateAttachNode = null;
			if (to != st_offset_tweak && to != st_rotate_tweak && selectedPart != null)
			{
				selectedPart.onEditorEndTweak();
				if (to == st_idle)
				{
					selectedPart = null;
				}
			}
			audioSource.PlayOneShot(tweakReleaseClip);
			coordSpaceBtn.gameObject.SetActive(value: false);
		};
		fsm.AddState(st_rotate_tweak);
		st_root_unselected = new KFSMState("st_root_unselected");
		st_root_unselected.OnEnter = delegate
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_125058"), modeMsg);
		};
		KFSMState kFSMState32 = st_root_unselected;
		kFSMState32.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState32.OnUpdate, new KFSMCallback(UndoRedoInputUpdate));
		KFSMState kFSMState33 = st_root_unselected;
		kFSMState33.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState33.OnUpdate, new KFSMCallback(snapInputUpdate));
		KFSMState kFSMState34 = st_root_unselected;
		kFSMState34.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState34.OnUpdate, new KFSMCallback(partSearchUpdate));
		st_root_unselected.OnUpdate = delegate
		{
		};
		st_root_unselected.OnLeave = delegate
		{
		};
		fsm.AddState(st_root_unselected);
		st_root_select = new KFSMState("st_root_select");
		st_root_select.OnEnter = delegate
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_125079"), modeMsg);
			rootCandidates = new List<PartSelector>();
			Part part4 = null;
			List<Part> list3 = EditorLogicBase.FindPartsInChildren(selectedPart);
			int num6 = 0;
			for (int count5 = list3.Count; num6 < count5; num6++)
			{
				part4 = list3[num6];
				part4.SetHighlight(active: false, recursive: false);
				part4.SetHighlightType(Part.HighlightType.Disabled);
			}
			list3 = EditorReRootUtil.GetRootCandidates(EditorLogicBase.FindPartsInChildren(selectedPart));
			int num7 = 0;
			for (int count6 = list3.Count; num7 < count6; num7++)
			{
				part4 = list3[num7];
				rootCandidates.Add(PartSelector.Create(part4, OnNewRootSelect, Highlighter.colorPartRootToolHighlight, Highlighter.colorPartRootToolHover, Highlighter.colorPartRootToolHighlightEdge, Highlighter.colorPartRootToolHoverEdge));
			}
		};
		KFSMState kFSMState35 = st_root_select;
		kFSMState35.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState35.OnUpdate, new KFSMCallback(UndoRedoInputUpdate));
		KFSMState kFSMState36 = st_root_select;
		kFSMState36.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState36.OnUpdate, new KFSMCallback(snapInputUpdate));
		KFSMState kFSMState37 = st_root_select;
		kFSMState37.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState37.OnUpdate, new KFSMCallback(partSearchUpdate));
		st_root_select.OnLeave = delegate(KFSMState to)
		{
			if (selectedPart != null)
			{
				List<Part> list2 = EditorLogicBase.FindPartsInChildren(selectedPart.localRoot);
				int m = 0;
				for (int count3 = list2.Count; m < count3; m++)
				{
					list2[m].SetHighlightDefault();
				}
			}
			int n = 0;
			for (int count4 = rootCandidates.Count; n < count4; n++)
			{
				rootCandidates[n].Dismiss();
			}
			rootCandidates.Clear();
			if (to == st_place)
			{
				toolsUI.SetMode(ConstructionMode.Place);
			}
			else if (to == st_offset_tweak)
			{
				toolsUI.SetMode(ConstructionMode.Move);
			}
			else if (to == st_rotate_tweak)
			{
				toolsUI.SetMode(ConstructionMode.Rotate);
			}
		};
		fsm.AddState(st_root_select);
		on_podSelect = new KFSMEvent("on_podSelect");
		on_podSelect.GoToStateOnEvent = st_idle;
		on_podSelect.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		on_podSelect.OnCheckCondition = (KFSMState st) => rootPart != null;
		fsm.AddEvent(on_podSelect, st_podSelect);
		on_podDeleted = new KFSMEvent("on_podDeleted");
		on_podDeleted.GoToStateOnEvent = st_podSelect;
		on_podDeleted.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		on_podDeleted.OnCheckCondition = (KFSMState st) => rootPart == null;
		on_podDeleted.OnEvent = delegate
		{
			SetBackup();
			if (ship != null && ship.vesselDeltaV != null)
			{
				ship.vesselDeltaV.gameObject.DestroyGameObject();
			}
			if (editorScreen == EditorScreen.Cargo)
			{
				SelectPanelParts();
			}
			GameEvents.onEditorPodDeleted.Fire();
		};
		fsm.AddEventExcluding(on_podDeleted);
		on_partCreated = new KFSMEvent("on_partCreated");
		on_partCreated.GoToStateOnEvent = st_place;
		on_partCreated.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		on_partCreated.OnEvent = delegate
		{
			audioSource.PlayOneShot(partGrabClip);
			GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartCreated, selectedPart);
			isCurrentPartFlag = selectedPart != null && selectedPart.GetComponent<FlagDecalBackground>() != null;
			CreateSelectedPartIcon();
		};
		fsm.AddEventExcluding(on_partCreated, st_place);
		on_partPicked = new KFSMEvent("on_partPicked");
		on_partPicked.updateMode = KFSMUpdateMode.UPDATE;
		on_partPicked.GoToStateOnEvent = st_place;
		on_partPicked.OnCheckCondition = delegate
		{
			if (InputLockManager.IsUnlocked(ControlTypes.EDITOR_PAD_PICK_PLACE) && Input.GetMouseButtonUp(0) && !Mouse.Left.WasDragging(25f) && fsm.FramesInCurrentState > 1 && !EventSystem.current.IsPointerOverGameObject() && !HeldPartIsStacked())
			{
				selectedPart = pickPart(layerMask | 0x200000, Input.GetKey(KeyCode.LeftShift), !Input.GetKey(KeyCode.LeftShift));
				isCurrentPartFlag = selectedPart != null && selectedPart.GetComponent<FlagDecalBackground>() != null;
				CreateSelectedPartIcon();
				if (selectedPart != null && !GameSettings.MODIFIER_KEY.GetKey())
				{
					return !Input.GetKey(KeyCode.LeftControl);
				}
				return false;
			}
			return false;
		};
		on_partPicked.OnEvent = delegate
		{
			if (selectedPart != selectedPart.localRoot)
			{
				bool num5 = ship.Contains(selectedPart);
				detachPart(selectedPart);
				deleteSymmetryParts();
				if (num5)
				{
					GameEvents.onEditorPartPicked.Fire(selectedPart);
					SetBackup();
					if (selectedPart.CrewCapacity > 0)
					{
						RefreshCrewAssignment(ShipConstruction.ShipConfig, GetPartExistsFilter());
					}
					GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartDetached, selectedPart);
					return;
				}
			}
			else
			{
				SetBackup();
			}
			if (selectedPart.frozen)
			{
				selectedPart.unfreeze();
			}
			isCurrentPartFlag = selectedPart != null && selectedPart.GetComponent<FlagDecalBackground>() != null;
			if (selectedPart != null && selectedPart.FindModuleImplementing<ModuleCargoPart>() != null && UIPartActionControllerInventory.Instance != null)
			{
				UIPartActionControllerInventory.Instance.CurrentCargoPart = selectedPart;
			}
			GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartPicked, selectedPart);
		};
		fsm.AddEvent(on_partPicked, st_idle);
		on_partCopied = new KFSMEvent("on_partCopied");
		on_partCopied.GoToStateOnEvent = st_place;
		on_partCopied.updateMode = KFSMUpdateMode.UPDATE;
		on_partCopied.OnCheckCondition = delegate
		{
			if (InputLockManager.IsUnlocked(ControlTypes.EDITOR_PAD_PICK_PLACE) && Input.GetMouseButtonUp(0) && !Mouse.Left.WasDragging(25f) && fsm.FramesInCurrentState > 1 && !EventSystem.current.IsPointerOverGameObject())
			{
				selectedPart = pickPart(layerMask | 0x200000, Input.GetKey(KeyCode.LeftShift), pickRootIfFrozen: false);
				if (selectedPart != null && selectedPart != rootPart && GameSettings.MODIFIER_KEY.GetKey())
				{
					return !Input.GetKey(KeyCode.LeftControl);
				}
				return false;
			}
			return false;
		};
		on_partCopied.OnEvent = delegate
		{
			selectedPart.OnWillBeCopied(asSymCounterpart: false);
			Part part3 = DuplicatePart(selectedPart);
			selectedPart.OnWasCopied(part3, asSymCounterpart: false);
			part3.OnCopy(selectedPart, asSymCounterpart: false);
			selectedPart = part3;
			EditorLogicBase.clearAttachNodes(selectedPart, selectedPart.parent);
			selectedPart.clearParent();
			wipeSymmetry(selectedPart);
			if (selectedPart.frozen)
			{
				selectedPart.unfreeze();
			}
			CheckCopiedSymmetryPersistentId(selectedPart);
			selPartGrabOffset = Vector3.zero;
			GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartCopied, selectedPart);
		};
		fsm.AddEvent(on_partCopied, st_idle);
		on_partReveal = new KFSMEvent("on_partReveal");
		on_partReveal.GoToStateOnEvent = st_idle;
		on_partReveal.updateMode = KFSMUpdateMode.UPDATE;
		on_partReveal.OnCheckCondition = delegate
		{
			if (InputLockManager.IsUnlocked(ControlTypes.EDITOR_PAD_PICK_PLACE) && Input.GetMouseButtonUp(0) && !Mouse.Left.WasDragging(25f) && fsm.FramesInCurrentState > 1 && !EventSystem.current.IsPointerOverGameObject())
			{
				selectedPart = pickPart(layerMask | 0x200000, Input.GetKey(KeyCode.LeftShift), pickRootIfFrozen: false);
				if (selectedPart != null)
				{
					return Input.GetKey(KeyCode.LeftControl);
				}
				return false;
			}
			return false;
		};
		on_partReveal.OnEvent = delegate
		{
			EditorPartList.Instance.RevealPart(selectedPart.partInfo, revealToolip: true);
			selectedPart = null;
		};
		fsm.AddEvent(on_partReveal, st_idle);
		on_partDropped = new KFSMEvent("on_partDropped");
		on_partDropped.updateMode = KFSMUpdateMode.LATEUPDATE;
		on_partDropped.OnCheckCondition = delegate
		{
			if (selectedPart != null && fsm.FramesInCurrentState > 0 && Input.GetMouseButtonUp(0) && !attachment.possible && !mouseOverModalArea(modalAreas) && !EventSystem.current.IsPointerOverGameObject() && !HeldPartIsStacked())
			{
				bool flag2 = true;
				int num4 = cPartAttachments.Length;
				while (num4-- > 0)
				{
					if (cPartAttachments[num4].collision)
					{
						flag2 = false;
						break;
					}
				}
				if (!attachment.collision && flag2)
				{
					if (selectedPart == rootPart)
					{
						SetBackup();
					}
					return true;
				}
				audioSource.PlayOneShot(cannotPlaceClip);
				return false;
			}
			return false;
		};
		on_partDropped.OnEvent = delegate
		{
			audioSource.PlayOneShot(partReleaseClip);
			isCurrentPartFlag = false;
			selectedPart.gameObject.SetLayerRecursive(0, filterTranslucent: true, 2097152);
			selectedPart.highlighter.ReinitMaterials();
			selectedPart.highlighter.UpdateHighlighting(!(editorCam != null) || !(editorCam.highLightingSystem != null) || editorCam.highLightingSystem.IsDepthAvailable);
			if (editorCam != null && editorCam.highLightingSystem != null)
			{
				editorCam.highLightingSystem.IsDirty = true;
			}
			if (selectedPart == rootPart)
			{
				selectedPart.gameObject.SetLayerRecursive(0, filterTranslucent: true, 2097152);
			}
			else
			{
				selectedPart.gameObject.SetLayerRecursive(1, filterTranslucent: true, 2097152);
			}
			deleteSymmetryParts();
			if (selectedPart != rootPart)
			{
				selectedPart.freeze();
			}
			if (UIPartActionControllerInventory.Instance != null)
			{
				UIPartActionControllerInventory.Instance.CurrentCargoPart = null;
				UIPartActionControllerInventory.Instance.DestroyHeldPartAsIcon();
			}
			GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartDropped, selectedPart);
			on_partDropped.GoToStateOnEvent = st_idle;
		};
		fsm.AddEvent(on_partDropped, st_place);
		on_partAttached = new KFSMEvent("on_partAttached");
		on_partAttached.updateMode = KFSMUpdateMode.UPDATE;
		on_partAttached.OnCheckCondition = (KFSMState st) => selectedPart != null && Input.GetMouseButtonUp(0) && fsm.FramesInCurrentState > 0 && attachment.possible && !EventSystem.current.IsPointerOverGameObject();
		on_partAttached.OnEvent = delegate
		{
			isCurrentPartFlag = false;
			if (selectedPart.symmetryCounterparts.Count > 0)
			{
				RestoreSymmetryState();
				bool flag = true;
				int num3 = cPartAttachments.Length;
				while (num3-- > 0)
				{
					if (!cPartAttachments[num3].possible)
					{
						flag = false;
						break;
					}
				}
				if (!flag)
				{
					audioSource.PlayOneShot(cannotPlaceClip);
					on_partAttached.GoToStateOnEvent = st_place;
					if (UIPartActionControllerInventory.Instance != null)
					{
						UIPartActionControllerInventory.Instance.DestroyHeldPartAsIcon();
					}
					return;
				}
				attachPart(selectedPart, attachment);
				attachSymParts(cPartAttachments);
			}
			else
			{
				attachPart(selectedPart, attachment);
				if (symmetryModeBeforeNodeAttachment >= 0)
				{
					RestoreSymmetryModeBeforeNodeAttachment();
				}
			}
			SetBackup();
			if (selectedPart.CrewCapacity > 0)
			{
				RefreshCrewAssignment(ShipConstruction.ShipConfig, GetPartExistsFilter());
			}
			ModuleCargoPart moduleCargoPart = selectedPart.FindModuleImplementing<ModuleCargoPart>();
			if (UIPartActionControllerInventory.Instance != null)
			{
				if (moduleCargoPart != null && !moduleCargoPart.IsDeployedSciencePart())
				{
					UIPartActionControllerInventory.Instance.CurrentCargoPart = null;
					UIPartActionControllerInventory.Instance.CurrentInventory = null;
				}
				UIPartActionControllerInventory.Instance.DestroyHeldPartAsIcon();
			}
			audioSource.PlayOneShot(attachClip);
			on_partAttached.GoToStateOnEvent = st_idle;
			CenterDragPlane(selectedPart.transform.position + selPartGrabOffset);
			GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartAttached, selectedPart);
		};
		fsm.AddEvent(on_partAttached, st_place);
		on_partDeleted = new KFSMEvent("on_partDeleted");
		on_partDeleted.GoToStateOnEvent = st_idle;
		on_partDeleted.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		on_partDeleted.OnEvent = delegate
		{
			if (selectedPart != null)
			{
				DestroySelectedPart();
			}
			if (UIPartActionControllerInventory.Instance != null)
			{
				UIPartActionControllerInventory.Instance.DestroyHeldPartAsIcon();
			}
			isCurrentPartFlag = false;
			GameEvents.onEditorPartDeleted.Fire(selectedPart);
		};
		fsm.AddEvent(on_partDeleted, st_place, st_offset_tweak, st_rotate_tweak);
		on_partLost = new KFSMEvent("on_partLost");
		on_partLost.GoToStateOnEvent = st_idle;
		on_partLost.updateMode = KFSMUpdateMode.UPDATE;
		on_partLost.OnCheckCondition = (KFSMState st) => selectedPart == null;
		on_partLost.OnEvent = delegate
		{
			Debug.LogWarning("[EditorLogic]: Selected Part was lost for unknown reasons! This is possibly unwanted behaviour.");
			isCurrentPartFlag = false;
			GameEvents.onEditorPartEvent.Fire(ConstructionEventType.Unknown, selectedPart);
		};
		fsm.AddEvent(on_partLost, st_place);
		on_partOverInventoryPAW = new KFSMEvent("on_partOverInventoryPAW");
		on_partOverInventoryPAW.GoToStateOnEvent = st_idle;
		on_partOverInventoryPAW.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		on_partOverInventoryPAW.OnEvent = delegate
		{
			if (!(selectedPart == null))
			{
				isCurrentPartFlag = false;
				selectedPart.gameObject.SetLayerRecursive(0, filterTranslucent: true, 2097152);
				selectedPart.highlighter.ReinitMaterials();
				selectedPart.highlighter.UpdateHighlighting(!(editorCam != null) || !(editorCam.highLightingSystem != null) || editorCam.highLightingSystem.IsDepthAvailable);
				if (editorCam != null && editorCam.highLightingSystem != null)
				{
					editorCam.highLightingSystem.IsDirty = true;
				}
				if (selectedPart == rootPart)
				{
					selectedPart.gameObject.SetLayerRecursive(0, filterTranslucent: true, 2097152);
				}
				else
				{
					selectedPart.gameObject.SetLayerRecursive(1, filterTranslucent: true, 2097152);
				}
				if (UIPartActionControllerInventory.Instance != null)
				{
					if (UIPartActionControllerInventory.Instance.CurrentInventoryOnlyIcon != null)
					{
						UIPartActionControllerInventory.Instance.CurrentInventoryOnlyIcon.gameObject.SetActive(value: true);
					}
					if (UIPartActionControllerInventory.Instance.CurrentCargoPart != null)
					{
						UIPartActionControllerInventory.Instance.CurrentCargoPart.gameObject.SetActive(value: false);
					}
				}
				deleteSymmetryParts();
				if (selectedPart != rootPart)
				{
					selectedPart.freeze();
				}
				GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartOverInventoryGrid, selectedPart);
			}
		};
		fsm.AddEventExcluding(on_partOverInventoryPAW);
		on_goToModeOffset = new KFSMEvent("on_goToModeOffset");
		on_goToModeOffset.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		on_goToModeOffset.OnEvent = delegate
		{
			if (selectedPart == null)
			{
				ScreenMessages.PostScreenMessage(EditorLogicBase.cacheAutoLOC_125488, modeMsg);
				on_goToModeOffset.GoToStateOnEvent = st_offset_select;
			}
			else if (!ship.Contains(selectedPart))
			{
				on_goToModeOffset.GoToStateOnEvent = st_place;
				on_partPicked.OnEvent();
			}
			else
			{
				on_goToModeOffset.GoToStateOnEvent = st_offset_tweak;
			}
		};
		fsm.AddEvent(on_goToModeOffset, st_idle, st_rotate_select, st_rotate_tweak, st_root_unselected, st_root_select);
		on_offsetSelect = new KFSMEvent("on_offsetSelect");
		on_offsetSelect.updateMode = KFSMUpdateMode.UPDATE;
		on_offsetSelect.OnCheckCondition = delegate
		{
			if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
			{
				selectedPart = pickPart(layerMask | 4 | 0x200000, Input.GetKey(KeyCode.LeftShift), pickRootIfFrozen: false);
				if (selectedPart != null)
				{
					if (!ship.Contains(selectedPart))
					{
						on_offsetSelect.GoToStateOnEvent = st_place;
						on_partPicked.OnEvent();
						return false;
					}
					on_offsetSelect.GoToStateOnEvent = st_offset_tweak;
					return true;
				}
			}
			return false;
		};
		fsm.AddEvent(on_offsetSelect, st_offset_select);
		on_offsetDeselect = new KFSMEvent("on_offsetDeselect");
		on_offsetDeselect.GoToStateOnEvent = st_offset_select;
		on_offsetDeselect.updateMode = KFSMUpdateMode.UPDATE;
		on_offsetDeselect.OnCheckCondition = delegate
		{
			if (Mouse.Left.GetButtonDown() && !Mouse.Left.WasDragging() && !gizmoOffset.GetMouseOverGizmo && !EventSystem.current.IsPointerOverGameObject())
			{
				Part part2 = pickPart(layerMask | 4 | 0x200000, Input.GetKey(KeyCode.LeftShift), pickRootIfFrozen: false);
				if (part2 == null)
				{
					selectedPart.onEditorEndTweak();
					selectedPart.gameObject.SetLayerRecursive(0, filterTranslucent: true, 2097152);
					selectedPart = null;
					return true;
				}
				if (EditorGeometryUtil.GetPixelDistance(gizmoOffset.transform.position, Input.mousePosition, editorCamera) > 75f)
				{
					selectedPart.onEditorEndTweak();
					selectedPart.gameObject.SetLayerRecursive(0, filterTranslucent: true, 2097152);
					selectedPart = part2;
					return true;
				}
			}
			return false;
		};
		fsm.AddEvent(on_offsetDeselect, st_offset_tweak);
		on_offsetReset = new KFSMEvent("on_offsetReset");
		on_offsetReset.GoToStateOnEvent = st_offset_tweak;
		on_offsetReset.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		fsm.AddEvent(on_offsetReset, st_offset_tweak);
		on_goToModeRotate = new KFSMEvent("on_goToModeRotate");
		on_goToModeRotate.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		on_goToModeRotate.OnEvent = delegate
		{
			if (selectedPart == null)
			{
				ScreenMessages.PostScreenMessage(EditorLogicBase.cacheAutoLOC_125583, modeMsg);
				on_goToModeRotate.GoToStateOnEvent = st_rotate_select;
			}
			else if (!ship.Contains(selectedPart))
			{
				on_goToModeRotate.GoToStateOnEvent = st_place;
				on_partPicked.OnEvent();
			}
			else
			{
				on_goToModeRotate.GoToStateOnEvent = st_rotate_tweak;
			}
		};
		fsm.AddEvent(on_goToModeRotate, st_idle, st_offset_select, st_offset_tweak, st_root_unselected, st_root_select);
		on_rotateSelect = new KFSMEvent("on_rotateSelect");
		on_rotateSelect.GoToStateOnEvent = st_rotate_tweak;
		on_rotateSelect.updateMode = KFSMUpdateMode.UPDATE;
		on_rotateSelect.OnCheckCondition = delegate
		{
			if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
			{
				selectedPart = pickPart(layerMask | 4 | 0x200000, Input.GetKey(KeyCode.LeftShift), pickRootIfFrozen: false);
				if (selectedPart != null)
				{
					if (!ship.Contains(selectedPart))
					{
						on_rotateSelect.GoToStateOnEvent = st_place;
						on_partPicked.OnEvent();
						return false;
					}
					on_rotateSelect.GoToStateOnEvent = st_rotate_tweak;
					return true;
				}
			}
			return false;
		};
		fsm.AddEvent(on_rotateSelect, st_rotate_select);
		on_rotateDeselect = new KFSMEvent("on_rotateDeselect");
		on_rotateDeselect.GoToStateOnEvent = st_rotate_select;
		on_rotateDeselect.updateMode = KFSMUpdateMode.UPDATE;
		on_rotateDeselect.OnCheckCondition = delegate
		{
			if (Mouse.Left.GetButtonDown() && !Mouse.Left.WasDragging() && !gizmoRotate.GetMouseOverGizmo && !EventSystem.current.IsPointerOverGameObject())
			{
				Part part = pickPart(layerMask | 4 | 0x200000, Input.GetKey(KeyCode.LeftShift), pickRootIfFrozen: false);
				if (part == null)
				{
					selectedPart.onEditorEndTweak();
					selectedPart.gameObject.SetLayerRecursive(0, filterTranslucent: true, 2097152);
					selectedPart = null;
					return true;
				}
				if (EditorGeometryUtil.GetPixelDistance(gizmoRotate.transform.position, Input.mousePosition, editorCamera) > 75f)
				{
					selectedPart.onEditorEndTweak();
					selectedPart.gameObject.SetLayerRecursive(0, filterTranslucent: true, 2097152);
					selectedPart = part;
					return true;
				}
			}
			return false;
		};
		fsm.AddEvent(on_rotateDeselect, st_rotate_tweak);
		on_rotateReset = new KFSMEvent("on_rotateReset");
		on_rotateReset.GoToStateOnEvent = st_rotate_tweak;
		on_rotateReset.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		fsm.AddEvent(on_rotateReset, st_rotate_tweak);
		on_goToModeRoot = new KFSMEvent("on_goToModeRoot");
		on_goToModeRoot.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		on_goToModeRoot.OnEvent = delegate
		{
			if (selectedPart != null && (fsm.CurrentState == st_place || fsm.CurrentState == st_offset_tweak || fsm.CurrentState == st_rotate_tweak))
			{
				selectedPart.gameObject.SetLayerRecursive(0, filterTranslucent: true, 2097152);
				on_goToModeRoot.GoToStateOnEvent = st_root_select;
			}
			else
			{
				on_goToModeRoot.GoToStateOnEvent = st_root_unselected;
			}
		};
		fsm.AddEventExcluding(on_goToModeRoot, st_root_select, st_root_unselected, st_podSelect);
		on_rootPickSet = new KFSMEvent("on_rootPickSet");
		on_rootPickSet.updateMode = KFSMUpdateMode.UPDATE;
		on_rootPickSet.OnCheckCondition = delegate
		{
			if (Input.GetMouseButtonUp(0) && !Mouse.Left.WasDragging(25f) && !EventSystem.current.IsPointerOverGameObject())
			{
				selectedPart = pickPart(layerMask | 4 | 0x200000, pickRoot: true, pickRootIfFrozen: true);
				return selectedPart != null;
			}
			return false;
		};
		on_rootPickSet.OnEvent = delegate
		{
			if (selectedPart.children.Count > 0)
			{
				on_rootPickSet.GoToStateOnEvent = st_root_select;
			}
			else
			{
				on_rootPickSet.GoToStateOnEvent = st_root_unselected;
				ScreenMessages.PostScreenMessage(EditorLogicBase.cacheAutoLOC_125724, 5f, ScreenMessageStyle.UPPER_CENTER);
				audioSource.PlayOneShot(cannotPlaceClip);
				selectedPart = null;
			}
		};
		fsm.AddEvent(on_rootPickSet, st_root_unselected);
		on_rootDeselect = new KFSMEvent("on_rootDeselect");
		on_rootDeselect.GoToStateOnEvent = st_root_unselected;
		on_rootDeselect.updateMode = KFSMUpdateMode.LATEUPDATE;
		on_rootDeselect.OnCheckCondition = delegate
		{
			if (Mouse.Left.GetButtonUp() && !Mouse.Left.WasDragging() && !pickPart(layerMask | 4 | 0x200000, pickRoot: false, pickRootIfFrozen: false))
			{
				selectedPart.gameObject.SetLayerRecursive(0, filterTranslucent: true, 2097152);
				List<Part> list = EditorLogicBase.FindPartsInChildren(selectedPart.localRoot);
				int l = 0;
				for (int count2 = list.Count; l < count2; l++)
				{
					list[l].SetHighlightDefault();
				}
				selectedPart = null;
				return true;
			}
			return false;
		};
		fsm.AddEvent(on_rootDeselect, st_root_select);
		on_rootSelect = new KFSMEvent("on_rootSelect");
		on_rootSelect.GoToStateOnEvent = st_place;
		on_rootSelect.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		on_rootSelect.OnEvent = delegate
		{
			audioSource.PlayOneShot(reRootClip);
			GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartRootSelected, selectedPart);
		};
		fsm.AddEvent(on_rootSelect, st_root_select);
		on_rootSelectFail = new KFSMEvent("on_rootSelectFail");
		on_rootSelectFail.updateMode = KFSMUpdateMode.UPDATE;
		on_rootSelectFail.GoToStateOnEvent = st_root_unselected;
		on_rootSelectFail.OnCheckCondition = (KFSMState st) => fsm.FramesInCurrentState > 0 && rootCandidates.Count == 0;
		on_rootSelectFail.OnEvent = delegate
		{
			ScreenMessages.PostScreenMessage(EditorLogicBase.cacheAutoLOC_125784, 5f, ScreenMessageStyle.UPPER_CENTER);
			audioSource.PlayOneShot(cannotPlaceClip);
			selectedPart = null;
		};
		fsm.AddEvent(on_rootSelectFail, st_root_select);
		on_goToModePlace = new KFSMEvent("on_goToModePlace");
		on_goToModePlace.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		on_goToModePlace.OnEvent = delegate
		{
			ScreenMessages.PostScreenMessage(EditorLogicBase.cacheAutoLOC_125798, modeMsg);
			if (!(selectedPart == null) && !ship.Contains(selectedPart))
			{
				selectedPart.unfreeze();
				on_goToModePlace.GoToStateOnEvent = st_place;
			}
			else
			{
				on_goToModePlace.GoToStateOnEvent = st_idle;
			}
		};
		fsm.AddEvent(on_goToModePlace, st_offset_select, st_offset_tweak, st_rotate_select, st_rotate_tweak, st_root_unselected, st_root_select);
		on_undoRedo = new KFSMEvent("on_undoRedo");
		on_undoRedo.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		on_undoRedo.OnEvent = delegate
		{
			if (fsm.CurrentState == st_rotate_tweak)
			{
				on_undoRedo.GoToStateOnEvent = st_rotate_select;
			}
			else if (fsm.CurrentState == st_offset_tweak)
			{
				on_undoRedo.GoToStateOnEvent = st_offset_select;
			}
		};
		fsm.AddEvent(on_undoRedo, st_offset_tweak, st_rotate_tweak);
		on_newShip = new KFSMEvent("on_newShip");
		on_newShip.GoToStateOnEvent = st_podSelect;
		on_newShip.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		on_newShip.OnEvent = delegate
		{
			if (ship != null)
			{
				int j = 0;
				for (int count = ship.parts.Count; j < count; j++)
				{
					if (ship.parts[j] != null)
					{
						UnityEngine.Object.DestroyImmediate(ship.parts[j].gameObject);
					}
				}
			}
			Part[] array2 = Part.allParts.ToArray();
			int k = 0;
			for (int num2 = array2.Length; k < num2; k++)
			{
				UnityEngine.Object.Destroy(array2[k].gameObject);
			}
			selectedPart = null;
		};
		fsm.AddEventExcluding(on_newShip);
		on_shipLoaded = new KFSMEvent("on_shipLoaded");
		on_shipLoaded.GoToStateOnEvent = st_idle;
		on_shipLoaded.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		on_shipLoaded.OnEvent = delegate
		{
			Part[] array = Part.allParts.ToArray();
			int i = 0;
			for (int num = array.Length; i < num; i++)
			{
				if (!ship.Contains(array[i]))
				{
					UnityEngine.Object.Destroy(array[i].gameObject);
				}
			}
			selectedPart = null;
			if (rootPart != null)
			{
				rootPart.gameObject.SetLayerRecursive(0, filterTranslucent: true, 2097152);
			}
			ResetCrewAssignment(ShipConstruction.ShipConfig, allowAutoHire: false);
		};
		fsm.AddEventExcluding(on_shipLoaded);
	}

	public void FixedUpdate()
	{
		if (fsm.Started)
		{
			fsm.FixedUpdateFSM();
		}
	}

	public void Update()
	{
		if (fsm.Started)
		{
			fsm.UpdateFSM();
			if (GameSettings.PAUSE.GetKeyDown(ignoreInputLock: true) && selectedCompoundPart != null && selectedCompoundPart.attachState == CompoundPart.AttachState.Attaching)
			{
				selectedPart = selectedCompoundPart;
				fsm.RunEvent(on_partPicked);
				CreateSelectedPartIcon();
				InputLockManager.RemoveControlLock("CompoundPart_Placement");
			}
			if (Application.isEditor && Input.GetKeyDown(KeyCode.Pause))
			{
				Debug.Break();
			}
		}
		if ((HighLogic.CurrentGame != null && HighLogic.CurrentGame.Mode == Game.Modes.MISSION) || HighLogic.CurrentGame.Mode == Game.Modes.MISSION_BUILDER)
		{
			editorSwitchInput();
		}
		if (steamCommsDialog != null && (double)Time.time - timeSteamCommsStarted > 60.0)
		{
			steamCommsDialog.Dismiss();
			steamCommsDialog = null;
		}
		if (editorCargoCamera != null)
		{
			editorCargoCamera.gameObject.SetActive(selectedPart != null && selectedPart.isCargoPart() && !selectedPart.attachRules.StackOrSurfaceAttachable);
		}
	}

	public void LateUpdate()
	{
		if (fsm.Started)
		{
			fsm.LateUpdateFSM();
		}
	}

	public void OnPartPurchased(AvailablePart part)
	{
		availableCargoParts = PartLoader.Instance.GetAvailableCargoPartNames();
	}

	public void onConstructionModeChanged(ConstructionMode mode)
	{
		if (mode != constructionMode)
		{
			coordSpaceBtn.gameObject.SetActive(value: false);
			radialSymmetryBtn.gameObject.SetActive(value: false);
			switch (mode)
			{
			default:
				fsm.RunEvent(on_goToModePlace);
				break;
			case ConstructionMode.Move:
				fsm.RunEvent(on_goToModeOffset);
				break;
			case ConstructionMode.Rotate:
				fsm.RunEvent(on_goToModeRotate);
				break;
			case ConstructionMode.Root:
				fsm.RunEvent(on_goToModeRoot);
				break;
			}
			constructionMode = mode;
		}
	}

	public void onRotateGizmoUpdate(Quaternion dRot)
	{
		Transform referenceTransform = selectedPart.GetReferenceTransform();
		switch (gizmoRotate.CoordSpace)
		{
		case Space.Self:
			referenceTransform.rotation = gizmoRotate.transform.rotation * selectedPart.initRotation;
			break;
		case Space.World:
			referenceTransform.rotation = dRot * gizmoRotate.HostRot0;
			break;
		}
		selectedPart.attRotation0 = selectedPart.transform.localRotation;
		selectedPart.attRotation = gizmoAttRotate * Quaternion.Inverse(gizmoAttRotate0) * selectedPart.transform.localRotation;
		if (symUpdateAttachNode != null)
		{
			referenceTransform.position += getPivotOffset(gizmoPivot, referenceTransform.TransformPoint(symUpdateAttachNode.position));
			gizmoRotate.transform.position = gizmoPivot;
		}
		if (symUpdateMode != 0)
		{
			UpdateSymmetry(selectedPart, symUpdateMode, symUpdateParent, symUpdateAttachNode);
		}
		GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartRotating, selectedPart);
	}

	public void onRotateGizmoUpdated(Quaternion dRot)
	{
		if (ship.Contains(selectedPart))
		{
			SetBackup();
		}
		GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartRotated, selectedPart);
	}

	public override void partRotationResetUpdate()
	{
		if (!GameSettings.Editor_resetRotation.GetKeyDown() || !(selectedPart.transform == selectedPart.GetReferenceTransform()))
		{
			return;
		}
		srfAttachCursorOffset = Vector3.zero;
		if (fsm.CurrentState == st_rotate_tweak)
		{
			selectedPart.attRotation = gizmoAttRotate;
			selectedPart.transform.localRotation = gizmoAttRotate0;
			selectedPart.attRotation0 = gizmoAttRotate0;
			if (symUpdateAttachNode != null)
			{
				selectedPart.transform.position += getPivotOffset(gizmoPivot, selectedPart.transform.TransformPoint(symUpdateAttachNode.position));
				gizmoRotate.transform.position = selectedPart.transform.position;
			}
		}
		else
		{
			selectedPart.attRotation = Quaternion.identity;
			selectedPart.transform.localRotation = selectedPart.attRotation0;
		}
		if (symUpdateMode != 0)
		{
			UpdateSymmetry(selectedPart, symUpdateMode, symUpdateParent, symUpdateAttachNode);
		}
		if (fsm.CurrentState == st_rotate_tweak)
		{
			fsm.RunEvent(on_rotateReset);
		}
		partTweaked = true;
	}

	public Vector3 getPivotOffset(Vector3 pivot0, Vector3 pivot)
	{
		return pivot0 - pivot;
	}

	public void onOffsetGizmoUpdate(Vector3 dPos)
	{
		Transform referenceTransform = selectedPart.GetReferenceTransform();
		referenceTransform.position = gizmoOffset.transform.position;
		if (selectedPart != rootPart)
		{
			threshold = (GameSettings.Editor_fineTweak.GetKey() ? GameSettings.VAB_FINE_OFFSET_THRESHOLD : 0.1f);
			if (selectedPart.surfaceAttachGO != null)
			{
				selectedPart.surfaceAttachGO.SetActive(value: true);
			}
			Part referenceParent = selectedPart.GetReferenceParent();
			if (EditorGeometryUtil.TestPartBoundsSeparate(selectedPart, referenceParent, threshold, out offsetGap))
			{
				if (selectedPart.attachMode == AttachModes.STACK)
				{
					childToParent = selectedPart.FindAttachNodeByPart(selectedPart.parent);
					parentToChild = selectedPart.parent.FindAttachNodeByPart(selectedPart);
					diff = parentToChild.position - (referenceTransform.localPosition + selectedPart.attRotation * childToParent.position);
					if (diff.sqrMagnitude > threshold * (float)(parentToChild.size + 1))
					{
						gizmoOffset.transform.position -= offsetGap;
						referenceTransform.position = gizmoOffset.transform.position;
					}
				}
				else
				{
					gizmoOffset.transform.position -= offsetGap;
					referenceTransform.position = gizmoOffset.transform.position;
				}
			}
			if (selectedPart.surfaceAttachGO != null)
			{
				selectedPart.surfaceAttachGO.SetActive(value: false);
			}
		}
		selectedPart.attPos = referenceTransform.localPosition - selectedPart.attPos0;
		if (symUpdateMode != 0)
		{
			UpdateSymmetry(selectedPart, symUpdateMode, symUpdateParent, symUpdateAttachNode);
		}
		GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartOffsetting, selectedPart);
	}

	public void onOffsetGizmoUpdated(Vector3 dPos)
	{
		if (ship.Contains(selectedPart))
		{
			SetBackup();
		}
		GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartOffset, selectedPart);
	}

	public void onOffsetGizmoBoundsUpdate()
	{
	}

	public void partOffsetInputUpdate()
	{
		if (InputLockManager.IsUnlocked(ControlTypes.EDITOR_GIZMO_TOOLS) && GameSettings.Editor_resetRotation.GetKeyDown() && selectedPart.transform == selectedPart.GetReferenceTransform())
		{
			selectedPart.attPos = Vector3.zero;
			selectedPart.transform.localPosition = selectedPart.attPos0;
			srfAttachCursorOffset = Vector3.zero;
			if (symUpdateMode != 0)
			{
				UpdateSymmetry(selectedPart, symUpdateMode, symUpdateParent, symUpdateAttachNode);
			}
			if (fsm.CurrentState == st_offset_tweak)
			{
				fsm.RunEvent(on_offsetReset);
			}
			GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartOffset, selectedPart);
		}
		Part part = null;
		int i = 0;
		for (int count = selectedPart.symmetryCounterparts.Count; i < count; i++)
		{
			part = selectedPart.symmetryCounterparts[i];
			part.attPos = part.transform.localPosition - part.attPos0;
		}
	}

	public void OnNewRootSelect(Part newRoot)
	{
		if (EditorReRootUtil.MakeRoot(newRoot, selectedPart))
		{
			audioSource.PlayOneShot(partGrabClip);
			selectedPart = newRoot;
			if (ship.Contains(newRoot))
			{
				rootPart = newRoot;
			}
			pickPart(LayerUtil.DefaultEquivalent | 4 | 0x200000, pickRoot: true, pickRootIfFrozen: true);
			fsm.RunEvent(on_rootSelect);
		}
		else
		{
			audioSource.PlayOneShot(cannotPlaceClip);
		}
	}

	public Part pickPart(int layerMask, bool pickRoot, bool pickRootIfFrozen)
	{
		if (EventSystem.current.IsPointerOverGameObject())
		{
			return null;
		}
		ray = editorCamera.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit, 1000f, layerMask))
		{
			Part part = FlightGlobals.GetPartUpwardsCached(hit.collider.gameObject);
			if (part == null)
			{
				return null;
			}
			if (part.frozen ? pickRootIfFrozen : pickRoot)
			{
				part = part.localRoot;
			}
			Vector3 normalized = (editorCamera.transform.position - part.transform.position).normalized;
			normalized = new Vector3(normalized.x, 0f, normalized.z);
			selPartGrabOffset = hit.point - part.transform.position;
			selPartGrabOffset = Quaternion.Inverse(Quaternion.LookRotation(normalized - (normalized - ray.direction))) * selPartGrabOffset;
			selPartGrabOffset = new Vector3(selPartGrabOffset.x, selPartGrabOffset.y, 0f);
			selPartGrabOffset = Quaternion.LookRotation(normalized - (normalized - ray.direction)) * selPartGrabOffset;
			CenterDragPlane(part.transform.position + selPartGrabOffset);
			return part;
		}
		return null;
	}

	public void CenterDragPlane(Vector3 pos)
	{
		dragPlaneCenter = new Vector3(pos.x, 0f, pos.z);
	}

	public bool dragOverPlane(out Vector3 position)
	{
		ray = editorCamera.ScreenPointToRay(Input.mousePosition);
		float num = Vector3.Distance(editorCamera.transform.position, dragPlaneCenter);
		Vector3 normalized = (dragPlaneCenter - editorCamera.transform.position).normalized;
		float y = Mathf.Atan2(dragPlaneCenter.x - editorCamera.transform.position.x, dragPlaneCenter.z - editorCamera.transform.position.z) * 57.29578f;
		float f = (float)Math.PI / 2f - Mathf.Acos(Vector3.Dot(normalized, Quaternion.Euler(0f, y, 0f) * Vector3.forward));
		float num2 = num * Mathf.Sin(f);
		float f2 = Mathf.Acos(Vector3.Dot(ray.direction, Quaternion.Euler(0f, y, 0f) * Vector3.forward));
		float num3 = num2 * Mathf.Tan(f2);
		float num4 = Mathf.Sqrt(num2 * num2 + num3 * num3);
		position = editorCamera.transform.position + ray.direction * num4 - selPartGrabOffset;
		if (!editorBounds.Contains(position))
		{
			return false;
		}
		return true;
	}

	public void SetBackup()
	{
		if (ship.parts.Count != 0)
		{
			if (undoLevel < ShipConstruction.backups.Count)
			{
				Debug.Log("Clearing undo states from #" + undoLevel + " forward (" + (ShipConstruction.backups.Count - undoLevel) + " entries)");
				ShipConstruction.backups.RemoveRange(undoLevel, ShipConstruction.backups.Count - undoLevel);
			}
			ship.shipName = shipNameField.text;
			ship.shipDescription = shipDescriptionField.text;
			ship.missionFlag = FlagURL;
			if (ShipConstruction.backups.Count >= undoLimit)
			{
				ShipConstruction.ShiftAndCreateBackup(ship);
			}
			else
			{
				ShipConstruction.CreateBackup(ship);
			}
			undoLevel = ShipConstruction.backups.Count;
			GameEvents.onEditorSetBackup.Fire(ship);
			GameEvents.onEditorShipModified.Fire(ship);
		}
	}

	public void ResetBackup()
	{
		ShipConstruction.backups.RemoveAt(ShipConstruction.backups.Count - 1);
		undoLevel = ShipConstruction.backups.Count;
		SetBackup();
	}

	public void UndoRedoInputUpdate()
	{
		if (!InputLockManager.IsUnlocked(ControlTypes.EDITOR_UNDO_REDO))
		{
			return;
		}
		bool num;
		if (!Application.isEditor)
		{
			if (Application.platform == RuntimePlatform.OSXPlayer)
			{
				if (Input.GetKey(KeyCode.LeftCommand))
				{
					num = Input.GetKey(KeyCode.Z);
					goto IL_0059;
				}
			}
			else if (Input.GetKey(KeyCode.LeftControl))
			{
				num = Input.GetKeyDown(KeyCode.Z);
				goto IL_0059;
			}
			goto IL_0062;
		}
		num = Input.GetKeyDown(KeyCode.Z);
		goto IL_0059;
		IL_0059:
		if (num)
		{
			RestoreState(-1);
		}
		goto IL_0062;
		IL_0062:
		bool num2;
		if (!Application.isEditor)
		{
			if (Application.platform == RuntimePlatform.OSXPlayer)
			{
				if (!Input.GetKey(KeyCode.LeftCommand))
				{
					return;
				}
				num2 = Input.GetKey(KeyCode.Y);
			}
			else
			{
				if (!Input.GetKey(KeyCode.LeftControl))
				{
					return;
				}
				num2 = Input.GetKeyDown(KeyCode.Y);
			}
		}
		else
		{
			num2 = Input.GetKeyDown(KeyCode.Y);
		}
		if (num2)
		{
			RestoreState(1);
		}
	}

	public void RestoreState(int offset)
	{
		if (offset == 0 || ShipConstruction.backups.Count == 0 || (offset == -1 && ship.parts.Count == 1 && ShipConstruction.backups[undoLevel - 1].GetNodes("PART").Length == 0))
		{
			return;
		}
		int num = Mathf.Clamp(undoLevel + offset, 1, ShipConstruction.backups.Count);
		if (num == undoLevel && fsm.CurrentState != st_podSelect)
		{
			return;
		}
		if (selectedPart != null)
		{
			displayAttachNodeIcons(ship.parts, selectedPart, stackNodes: false, srfNodes: false, dockNodes: false);
		}
		if (ship.Count > 0)
		{
			UnityEngine.Object.DestroyImmediate(rootPart.gameObject);
		}
		undoLevel = num;
		if (ship != null && ship.vesselDeltaV != null)
		{
			ship.vesselDeltaV.gameObject.DestroyGameObject();
		}
		ship = ShipConstruction.RestoreBackup(undoLevel - 1);
		rootPart = ship.parts[0].localRoot;
		ShipConstruction.SanitizeCraftIDs(ship.parts, preserveIDsOnGivenParts: false);
		ShipConstruction.ShipConfig = ship.SaveShip();
		GameEvents.onEditorRestoreState.Fire();
		RefreshCrewAssignment(ShipConstruction.ShipConfig, GetPartExistsFilter());
		GameEvents.onEditorShipModified.Fire(ship);
		GameEvents.onEditorUndo.Fire(ship);
		fsm.RunEvent(on_undoRedo);
		if (selectedPart != null)
		{
			selectedPart.persistentId = FlightGlobals.CheckPartpersistentId(selectedPart.persistentId, selectedPart, removeOldId: true, addNewId: true);
			if (selectedPart.children.Count > 0)
			{
				for (int i = 0; i < selectedPart.children.Count; i++)
				{
					selectedPart.children[i].persistentId = FlightGlobals.CheckPartpersistentId(selectedPart.children[i].persistentId, selectedPart.children[i], removeOldId: true, addNewId: true);
				}
			}
		}
		if (fsm.CurrentState == st_podSelect)
		{
			fsm.RunEvent(on_podSelect);
		}
		if (fsm.Started && SelectedPart != null)
		{
			skipPartAttach = true;
			StartCoroutine(CallbackUtil.DelayedCallback(1, delegate
			{
				skipPartAttach = false;
			}));
		}
	}

	public void OnSubassemblyDialogDismiss(Part p)
	{
		selectedPart = p;
	}

	public void OnPartListIconTap(AvailablePart p)
	{
		if (fsm.CurrentState == st_place)
		{
			OnPartListBackgroundTap();
		}
		else
		{
			SpawnPart(p);
		}
	}

	public void OnPartListIconTap(ShipTemplate st)
	{
		if (fsm.CurrentState == st_place)
		{
			OnPartListBackgroundTap();
		}
		else
		{
			SpawnTemplate(st);
		}
	}

	public void OnPartListBackgroundTap()
	{
		if (fsm.CurrentState == st_place)
		{
			DestroySelectedPart();
		}
	}

	public void OnPodSpawn(AvailablePart pod)
	{
		rootPart = UnityEngine.Object.Instantiate(pod.partPrefab);
		rootPart.gameObject.SetActive(value: true);
		rootPart.name = pod.name;
		rootPart.partInfo = pod;
		rootPart.persistentId = FlightGlobals.CheckPartpersistentId(RootPart.persistentId, rootPart, removeOldId: false, addNewId: true);
		if (rootPart.variants != null && pod.variant != null && pod.variant.Name != null)
		{
			rootPart.variants.SetVariant(pod.variant.Name);
		}
		ModuleInventoryPart moduleInventoryPart = rootPart.FindModuleImplementing<ModuleInventoryPart>();
		if (moduleInventoryPart != null)
		{
			moduleInventoryPart.SetInventoryDefaults();
		}
		ConfigNode node = null;
		for (int i = 0; i < pod.partConfig.nodes.Count; i++)
		{
			if (pod.partConfig.nodes[i].HasValue("name") && pod.partConfig.nodes[i].GetValue("name") == "PartStatsUpgradeModule")
			{
				node = pod.partConfig.nodes[i];
				break;
			}
		}
		if (PartModule.UpgradesAvailable(rootPart, node) == PartModule.PartUpgradeState.AVAILABLE)
		{
			for (int j = 0; j < rootPart.Modules.Count; j++)
			{
				if (rootPart.Modules[j].moduleName == "PartStatsUpgradeModule")
				{
					rootPart.Modules[j].OnLoad(node);
					break;
				}
			}
		}
		GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartCreated, rootPart);
		fsm.RunEvent(on_podSelect);
	}

	public void SpawnPart(AvailablePart partInfo)
	{
		if (ship.parts.Count == 0)
		{
			OnPodSpawn(partInfo);
			return;
		}
		Part part = UnityEngine.Object.Instantiate(partInfo.partPrefab);
		part.gameObject.SetActive(value: true);
		part.name = partInfo.name;
		part.persistentId = FlightGlobals.CheckPartpersistentId(part.persistentId, part, removeOldId: false, addNewId: true);
		if (part.variants != null && partInfo.variant != null && partInfo.variant.Name != null)
		{
			part.variants.SetVariant(partInfo.variant.Name);
		}
		part.InitializeModules();
		ModuleInventoryPart moduleInventoryPart = part.FindModuleImplementing<ModuleInventoryPart>();
		if (moduleInventoryPart != null)
		{
			moduleInventoryPart.SetInventoryDefaults();
		}
		selectedPart = part;
		selectedPart.transform.rotation = (rootPart ? rootPart.transform.rotation : selectedPart.initRotation);
		Vector3 position = Vector3.zero;
		if (dragOverPlane(out position))
		{
			selectedPart.transform.position = position;
		}
		selPartGrabOffset = Vector3.zero;
		selectedPart.attRotation = Quaternion.identity;
		selectedPart.attRotation0 = selectedPart.transform.localRotation;
		selectedPart.attPos0 = selectedPart.transform.localPosition;
		selectedPart.gameObject.SetLayerRecursive(1, filterTranslucent: true, 2097152);
		selectedPart.partInfo = partInfo;
		if (fsm.Started)
		{
			fsm.RunEvent(on_partCreated);
		}
	}

	public void SetIconAsPart(Part part)
	{
		part.transform.SetParent(null);
		SceneManager.MoveGameObjectToScene(part.gameObject, SceneManager.GetActiveScene());
		selectedPart = part;
		selPartGrabOffset = Vector3.zero;
		dragPlaneCenter = Vector3.zero;
		selectedPart.transform.rotation = (rootPart ? rootPart.transform.rotation : selectedPart.initRotation);
		selectedPart.transform.localScale = Vector3.one;
		selectedPart.attRotation = Quaternion.identity;
		selectedPart.attRotation0 = selectedPart.transform.localRotation;
		selectedPart.attPos0 = selectedPart.transform.localPosition;
		selectedPart.gameObject.SetLayerRecursive(1, filterTranslucent: true, 2097152);
		part.highlighter.ReinitMaterials();
		part.SetHighlight(active: true, recursive: true);
		if (fsm.Started)
		{
			fsm.RunEvent(on_partPicked);
		}
	}

	[Obsolete("Please use UIPartActionControllerInventory.Instance.CreatePartFromInventory")]
	public Part CreatePartForInventoryUse(AvailablePart partInfo)
	{
		if ((bool)UIPartActionControllerInventory.Instance)
		{
			return UIPartActionControllerInventory.Instance.CreatePartFromInventory(partInfo);
		}
		return null;
	}

	public void ReleasePartToIcon()
	{
		if (fsm.Started)
		{
			fsm.RunEvent(on_partOverInventoryPAW);
		}
	}

	public void SpawnTemplate(ShipTemplate st)
	{
		ShipConstruction.CreateConstructFromTemplate(st, delegate(ShipConstruct construct)
		{
			if (construct != null)
			{
				SpawnConstruct(construct);
			}
		});
	}

	public void SpawnConstruct(ShipConstruct construct)
	{
		if (construct != null)
		{
			if (ship.parts.Count == 0)
			{
				rootPart = construct.parts[0].localRoot;
				fsm.RunEvent(on_podSelect);
				return;
			}
			ShipConstruction.SanitizeCraftIDs(construct.parts, preserveIDsOnGivenParts: false);
			selectedPart = construct.parts[0].localRoot;
			selectedPart.transform.rotation = (rootPart ? rootPart.transform.rotation : selectedPart.initRotation);
			Vector3 position = Vector3.zero;
			if (dragOverPlane(out position))
			{
				selectedPart.transform.position = position;
			}
			selPartGrabOffset = Vector3.zero;
			selectedPart.attRotation = Quaternion.identity;
			selectedPart.attRotation0 = selectedPart.transform.localRotation;
			selectedPart.attPos0 = selectedPart.transform.localPosition;
			GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartCreated, selectedPart);
			if (fsm.Started)
			{
				fsm.RunEvent(on_partCreated);
			}
			return;
		}
		throw new ArgumentNullException("construct", "[EditorLogic]: The Construct you are attempting to load is null!");
	}

	public Part DuplicatePart(Part p)
	{
		Part part = UnityEngine.Object.Instantiate(p);
		part.gameObject.SetActive(value: true);
		part.name = p.partInfo.name;
		part.persistentId = FlightGlobals.GetUniquepersistentId();
		if ((bool)FlightGlobals.fetch)
		{
			FlightGlobals.PersistentLoadedPartIds.Add(part.persistentId, part);
		}
		part.InitializeModules();
		BaseAction baseAction = null;
		int i = 0;
		for (int count = p.Actions.Count; i < count; i++)
		{
			baseAction = p.Actions[i];
			part.Actions[baseAction.name].actionGroup = baseAction.actionGroup;
		}
		return part;
	}

	public static void DeletePart(Part part)
	{
		if ((bool)fetch)
		{
			if (part == SelectedPart)
			{
				fetch.DestroySelectedPart();
			}
			else
			{
				fetch.deletePartAndSymmetryParts(part);
			}
		}
	}

	public void deletePart(Part part, bool symmetry = false, bool printConfirmation = false)
	{
		List<Part> list = EditorLogicBase.FindPartsInChildren(part);
		if (part == selectedPart)
		{
			audioSource.PlayOneShot(deletePartClip);
		}
		if (printConfirmation)
		{
			if (list.Count == 1)
			{
				Debug.Log("deleting " + (symmetry ? "symmetryPart " : "part ") + part.name);
			}
			else if (list.Count > 1)
			{
				Debug.Log("deleting " + (symmetry ? "symmetryPart " : "part ") + part.name + " and all children");
			}
			else
			{
				Debug.Log("delete what " + (symmetry ? "symmetryPart" : "part") + "?");
			}
		}
		Part part2 = null;
		int count = list.Count;
		while (count-- > 0)
		{
			part2 = list[count];
			if (ship.Contains(part2))
			{
				ship.Remove(part2);
			}
			part2.OnDelete();
			UnityEngine.Object.Destroy(part2.gameObject);
		}
		isCurrentPartFlag = false;
	}

	public void DestroySelectedPart()
	{
		if (selectedPart != null)
		{
			if (ship.Contains(selectedPart) && selectedPart != rootPart)
			{
				detachPart(selectedPart);
				SetBackup();
			}
			GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartDeleted, selectedPart);
			displayAttachNodeIcons(ship.parts, selectedPart, stackNodes: false, srfNodes: false, dockNodes: false);
			deletePartAndSymmetryParts(selectedPart);
			RefreshCrewAssignment(ShipConstruction.ShipConfig, GetPartExistsFilter());
			selectedPart = null;
			isCurrentPartFlag = false;
			if (ship.Count == 0)
			{
				fsm.RunEvent(on_podDeleted);
			}
			else
			{
				fsm.RunEvent(on_partDeleted);
			}
			if (CameraMouseLook.MouseLocked)
			{
				CameraMouseLook.SetMouseLook(mLock: false);
			}
		}
	}

	public void deleteInputUpdate()
	{
		bool flag = constructionMode == ConstructionMode.Place || (selectedPart != rootPart && (constructionMode == ConstructionMode.Move || constructionMode == ConstructionMode.Rotate));
		if (InputLockManager.IsUnlocked(ControlTypes.EDITOR_PAD_PICK_PLACE) && selectedPart != null && flag && Input.GetKeyDown(KeyCode.Delete))
		{
			DestroySelectedPart();
		}
	}

	public void RestoreSymmetryState()
	{
		if (tmpSymMethodInUse)
		{
			tmpSymMethodInUse = false;
			symmetryMethod = symmetryMethodTmp;
			if (symmetryMethod == SymmetryMethod.Mirror)
			{
				symmetryMode = Mathf.Min(1, symmetryModeTmp);
			}
			else
			{
				symmetryMode = symmetryModeTmp;
			}
		}
	}

	public void RestoreSymmetryModeBeforeNodeAttachment()
	{
		symmetryMode = symmetryModeBeforeNodeAttachment;
		SetSymState();
		symmetryModeBeforeNodeAttachment = -1;
	}

	public void createSymmetry(int mode, Attachment attach)
	{
		Part part = selectedPart;
		if (!tmpSymMethodInUse && (part.symmetryCounterparts.Count != mode || part.symMethod != symmetryMethod))
		{
			deleteSymmetryParts();
		}
		part.symMethod = symmetryMethod;
		if (mode == 0)
		{
			if (attach.mode == AttachModes.SRF_ATTACH && symmetryModeBeforeNodeAttachment >= 0 && part.symmetryCounterparts.Count == 0)
			{
				RestoreSymmetryModeBeforeNodeAttachment();
			}
			return;
		}
		if (part.potentialParent.symmetryCounterparts.Count == 0)
		{
			if (new Vector3(part.transform.position.x, 0f, part.transform.position.z).Equals(new Vector3(part.potentialParent.transform.position.x, 0f, part.potentialParent.transform.position.z)))
			{
				deleteSymmetryParts();
				return;
			}
			if (attach.mode == AttachModes.STACK && (part.potentialParent.stackSymmetry != mode || (EditorDriver.editorFacility == EditorFacility.const_1 && symmetryMethod == SymmetryMethod.Mirror)))
			{
				symmetryModeBeforeNodeAttachment = symmetryMode;
				symmetryMode = part.potentialParent.stackSymmetry;
				SetSymState();
				return;
			}
		}
		if (part.symmetryCounterparts.Count == 0)
		{
			Part part2 = null;
			for (int i = 0; i < mode; i++)
			{
				part.OnWillBeCopied(asSymCounterpart: true);
				part2 = DuplicatePart(selectedPart);
				part.OnWasCopied(part2, asSymCounterpart: true);
				part.symmetryCounterparts.Add(part2);
				part2.symmetryCounterparts.Clear();
				part2.symmetryCounterparts.Add(selectedPart);
				part2.symMethod = symmetryMethod;
				part2.OnCopy(part, asSymCounterpart: true);
				if (part.potentialParent.symmetryCounterparts.Count == 0)
				{
					part2.potentialParent = part.potentialParent;
					continue;
				}
				if (part.potentialParent.symmetryCounterparts.Count == mode && part.potentialParent.symMethod == symmetryMethod)
				{
					part2.potentialParent = part.potentialParent.getSymmetryCounterPart(i + 1);
					continue;
				}
				symmetryMode = part.potentialParent.symmetryCounterparts.Count;
				if (part.symMethod != part.potentialParent.symMethod)
				{
					RestoreSymmetryState();
					symmetryMethod = part.potentialParent.symMethod;
					GameEvents.onEditorSymmetryMethodChange.Fire(symmetryMethod);
				}
				SetSymState();
				return;
			}
			Part part3 = null;
			Part part4 = null;
			int j = 0;
			for (int count = part.symmetryCounterparts.Count; j < count; j++)
			{
				part3 = part.symmetryCounterparts[j];
				int k = 0;
				for (int count2 = part.symmetryCounterparts.Count; k < count2; k++)
				{
					part4 = part.symmetryCounterparts[k];
					if (!(part3 == part4) && !part3.symmetryCounterparts.Contains(part4))
					{
						part3.symmetryCounterparts.Add(part4);
					}
				}
			}
			int l = 0;
			for (int count3 = part.symmetryCounterparts.Count; l < count3; l++)
			{
				part4 = part.symmetryCounterparts[l];
				UpdatePartAndChildren(part, part4, symmetryMethod);
			}
		}
		UpdateSymmetry(part, symmetryMode, attach.potentialParent, attach.callerPartNode);
		if (part.symMethod != symmetryMethod)
		{
			symmetryMethodTmp = symmetryMethod;
			symmetryModeTmp = symmetryMode;
			symmetryMethod = part.symMethod;
			tmpSymMethodInUse = true;
		}
		int num = 0;
		while (true)
		{
			if (num < symmetryMode)
			{
				if (part.symmetryCounterparts[num].transform.position.Equals(part.transform.position))
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		deleteSymmetryParts();
	}

	public void UpdatePartAndChildren(Part SourcePart, Part UpdatePart, SymmetryMethod SymMethod)
	{
		if (null == SourcePart || null == UpdatePart || SourcePart == UpdatePart)
		{
			return;
		}
		if (!SourcePart.symmetryCounterparts.Contains(UpdatePart))
		{
			SourcePart.symmetryCounterparts.Add(UpdatePart);
		}
		Part part = null;
		int i = 0;
		for (int count = SourcePart.symmetryCounterparts.Count; i < count; i++)
		{
			part = SourcePart.symmetryCounterparts[i];
			part.symmetryCounterparts.Clear();
			for (int j = i + 1; j < count; j++)
			{
				part.symmetryCounterparts.Add(SourcePart.symmetryCounterparts[j]);
			}
			part.symmetryCounterparts.Add(SourcePart);
			for (int k = 0; k < i; k++)
			{
				part.symmetryCounterparts.Add(SourcePart.symmetryCounterparts[k]);
			}
		}
		if (SourcePart.symMethod == SymmetryMethod.Radial && SourcePart.symmetryCounterparts.Count > 1)
		{
			SymMethod = SymmetryMethod.Radial;
		}
		SourcePart.symMethod = SymMethod;
		UpdatePart.symMethod = SymMethod;
		int l = 0;
		for (int count2 = UpdatePart.children.Count; l < count2; l++)
		{
			UpdatePartAndChildren(SourcePart.children[l], UpdatePart.children[l], SymMethod);
		}
	}

	public void UpdateSymmetry(Part selPart, int symMode, Part partParent, AttachNode selPartNode)
	{
		int num = 1;
		int count = selPart.symmetryCounterparts.Count;
		while (count-- > 0)
		{
			if (selPart.symmetryCounterparts[count].potentialParent == selPart.potentialParent)
			{
				num++;
			}
		}
		for (int i = 0; i < symMode; i++)
		{
			Quaternion quaternion = default(Quaternion);
			Vector3 vector = default(Vector3);
			Quaternion quaternion2 = default(Quaternion);
			Transform referenceTransform = selPart.GetReferenceTransform();
			int num2 = (i + 1) % num;
			switch (selPart.symMethod)
			{
			case SymmetryMethod.Mirror:
			{
				Part part = FirstNonSymmetricalParentFrom(partParent);
				Vector3 vector2 = referenceTransform.position - part.transform.position;
				Vector3 vector3 = Vector3.ProjectOnPlane(vector2, rootPart.transform.up);
				Vector3 newPosition = part.transform.position + (vector2 - vector3) + Quaternion.AngleAxis(180f, -rootPart.transform.forward) * vector3;
				Vector3 direction = rootPart.transform.InverseTransformDirection(referenceTransform.up);
				Vector3 direction2 = rootPart.transform.InverseTransformDirection(referenceTransform.forward);
				direction.x *= -1f;
				direction2.x *= -1f;
				direction = rootPart.transform.TransformDirection(direction);
				direction2 = rootPart.transform.TransformDirection(direction2);
				quaternion = Quaternion.LookRotation(direction2, direction);
				bool flag = selPart.symmetryCounterparts[i].OnWillBeMirrored(ref quaternion, selPartNode, partParent);
				if (selPartNode != null && selPartNode.nodeType == AttachNode.NodeType.Surface && !flag && selPartNode.orientation.x != 0f)
				{
					quaternion = Quaternion.AngleAxis(180f, direction) * quaternion;
				}
				selPart.symmetryCounterparts[i].SetSymmetryValues(newPosition, quaternion);
				UpdateChildMirrorSymmetry(selPart, part, i);
				break;
			}
			case SymmetryMethod.Radial:
				if (selPart.symmetryCounterparts[i].potentialParent == selPart.potentialParent)
				{
					Space space = symmetryCoordSpace;
					quaternion = ((space == Space.World || space != Space.Self) ? Quaternion.AngleAxis((float)num2 * 360f / (float)num, rootPart.transform.up) : Quaternion.AngleAxis((float)num2 * 360f / (float)num, selPart.potentialParent.transform.up));
					vector = quaternion * (referenceTransform.position - selPart.potentialParent.transform.position) + selPart.symmetryCounterparts[i].potentialParent.transform.position;
					quaternion2 = quaternion * referenceTransform.rotation;
					selPart.symmetryCounterparts[i].SetSymmetryValues(vector, quaternion2);
				}
				else if (selPart.symmetryCounterparts[i].potentialParent != null)
				{
					if (num2 == 0)
					{
						vector = selPart.symmetryCounterparts[i].potentialParent.transform.TransformPoint(selPart.potentialParent.transform.InverseTransformPoint(referenceTransform.position));
						quaternion2 = selPart.symmetryCounterparts[i].potentialParent.transform.rotation * (Quaternion.Inverse(selPart.potentialParent.GetReferenceTransform().rotation) * referenceTransform.rotation);
						selPart.symmetryCounterparts[i].SetSymmetryValues(vector, quaternion2);
					}
					else
					{
						vector = selPart.symmetryCounterparts[i].potentialParent.transform.TransformPoint(selPart.symmetryCounterparts[num2 - 1].potentialParent.transform.InverseTransformPoint(selPart.symmetryCounterparts[num2 - 1].transform.position));
						quaternion2 = selPart.symmetryCounterparts[i].potentialParent.transform.rotation * (Quaternion.Inverse(selPart.symmetryCounterparts[num2 - 1].potentialParent.transform.rotation) * selPart.symmetryCounterparts[num2 - 1].transform.rotation);
						selPart.symmetryCounterparts[i].SetSymmetryValues(vector, quaternion2);
					}
				}
				break;
			}
			AttachModes attachMode = selPart.attachMode;
			if (attachMode == AttachModes.SRF_ATTACH)
			{
				selPart.symmetryCounterparts[i].srfAttachNode.srfAttachMeshName = selPart.srfAttachNode.srfAttachMeshName;
			}
		}
	}

	public void UpdateChildMirrorSymmetry(Part part, Part mirrorRoot, int symIdx)
	{
		int i = 0;
		for (int count = part.children.Count; i < count; i++)
		{
			Part part2 = part.children[i];
			if (part2.symmetryCounterparts.Count > symIdx && !(part2.symmetryCounterparts[symIdx] == null) && !PartInHierarchy(selectedPart, part2.symmetryCounterparts[symIdx]))
			{
				Vector3 vector = part2.transform.position - mirrorRoot.transform.position;
				Vector3 vector2 = Vector3.ProjectOnPlane(vector, rootPart.transform.up);
				Vector3 position = mirrorRoot.transform.position + (vector - vector2) + Quaternion.AngleAxis(180f, -rootPart.transform.forward) * vector2;
				Vector3 direction = rootPart.transform.InverseTransformDirection(part2.transform.up);
				Vector3 direction2 = rootPart.transform.InverseTransformDirection(part2.transform.forward);
				direction.x *= -1f;
				direction2.x *= -1f;
				direction = rootPart.transform.TransformDirection(direction);
				direction2 = rootPart.transform.TransformDirection(direction2);
				Quaternion rotation = Quaternion.LookRotation(direction2, direction);
				AttachNode attachNode = part2.FindAttachNodeByPart(part);
				bool flag = part2.OnWillBeMirrored(ref rotation, attachNode, part);
				if (attachNode != null && !flag && attachNode.orientation.x != 0f)
				{
					rotation = Quaternion.AngleAxis(180f, direction) * rotation;
				}
				part2.symmetryCounterparts[symIdx].transform.position = position;
				part2.symmetryCounterparts[symIdx].transform.rotation = rotation;
				UpdateChildMirrorSymmetry(part2, mirrorRoot, symIdx);
			}
		}
	}

	public void cleanSymmetry()
	{
		Part part = null;
		int i = 0;
		for (int count = ship.Count; i < count; i++)
		{
			part = ship[i];
			int j = 0;
			for (int num = part.symmetryCounterparts.Count; j < num; j++)
			{
				if (part.symmetryCounterparts[j] == null)
				{
					Debug.LogWarning("Symmetry counterpart found missing on " + part.name, part);
					part.symmetryCounterparts.RemoveAt(j);
					j--;
					num--;
				}
			}
		}
	}

	public void CheckCopiedSymmetryPersistentId(Part selectedPart)
	{
		if (selectedPart == null)
		{
			return;
		}
		List<Part> list = EditorLogicBase.FindPartsInChildren(selectedPart);
		for (int i = 0; i < list.Count; i++)
		{
			list[i].persistentId = FlightGlobals.CheckPartpersistentId(list[i].persistentId, list[i], removeOldId: false, addNewId: true);
		}
		for (int j = 0; j < selectedPart.symmetryCounterparts.Count; j++)
		{
			List<Part> list2 = EditorLogicBase.FindPartsInChildren(selectedPart.symmetryCounterparts[j]);
			for (int k = 0; k < list2.Count; k++)
			{
				list2[k].persistentId = FlightGlobals.CheckPartpersistentId(list2[k].persistentId, list2[k], removeOldId: false, addNewId: true);
			}
		}
	}

	public void wipeSymmetry(Part selPart)
	{
		selPart.symmetryCounterparts = new List<Part>();
		List<Part> list = EditorLogicBase.FindPartsInChildren(selPart);
		HashSet<Part> hashSet = new HashSet<Part>(list);
		Part part = null;
		int i = 0;
		for (int count = list.Count; i < count; i++)
		{
			part = list[i];
			int j = 0;
			for (int num = part.symmetryCounterparts.Count; j < num; j++)
			{
				if (!hashSet.Contains(part.symmetryCounterparts[j]))
				{
					part.symmetryCounterparts.RemoveAt(j);
					j--;
					num--;
				}
			}
		}
	}

	public void deleteSymmetryParts()
	{
		RestoreSymmetryState();
		if (selectedPart == null)
		{
			return;
		}
		Part part = null;
		int i = 0;
		for (int count = selectedPart.symmetryCounterparts.Count; i < count; i++)
		{
			part = selectedPart.symmetryCounterparts[i];
			if (part != null && selectedPart != part)
			{
				GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartSymmetryDeleted, part);
				detachPart(part);
				deletePart(part, symmetry: true);
			}
		}
		wipeSymmetry(selectedPart);
	}

	public void deletePartAndSymmetryParts(Part part, bool symmetry = false)
	{
		if (part != selectedPart)
		{
			displayAttachNodeIcons(ship.parts, selectedPart, stackNodes: false, srfNodes: false, dockNodes: false);
		}
		deleteSymmetryParts();
		deletePart(part, symmetry: false, printConfirmation: true);
	}

	public static Part FirstNonSymmetricalParentFrom(Part p)
	{
		if (p.symmetryCounterparts.Count != 0)
		{
			return FirstNonSymmetricalParentFrom(p.parent);
		}
		return p;
	}

	public Attachment checkAttach(Part selPart)
	{
		Attachment attachment = new Attachment();
		attachment.caller = selPart;
		attachment.position = selPart.transform.position;
		attachment.rotation = vesselRotation;
		attachment.potentialParent = null;
		bool flag = false;
		bool flag2 = false;
		if (selPart.attachRules.stack && allowNodeAttachment)
		{
			Part part = null;
			int i = 0;
			for (int count = ship.Count; i < count; i++)
			{
				part = ship[i];
				if (part == selPart || PartInHierarchy(selPart, part) || (!part.attachRules.allowStack && !part.attachRules.allowDock))
				{
					continue;
				}
				AttachNode attachNode = null;
				AttachNode attachNode2 = null;
				AttachNode attachNode3 = null;
				int j = 0;
				for (int count2 = selPart.attachNodes.Count; j < count2; j++)
				{
					attachNode3 = selPart.attachNodes[j];
					if (attachNode3.attachedPart != null || attachNode3.icon == null)
					{
						continue;
					}
					Vector3 normalized = selPart.transform.TransformDirection(attachNode3.orientation).normalized;
					AttachNode attachNode4 = null;
					int k = 0;
					for (int count3 = part.attachNodes.Count; k < count3; k++)
					{
						attachNode4 = part.attachNodes[k];
						if (((attachNode4.icon == null || attachNode4.attachedPart != null) && !CheatOptions.AllowPartClipping) || attachNode3.nodeType != attachNode4.nodeType || (!attachNode4.icon.GetComponent<Renderer>().bounds.IntersectRay(editorCamera.ScreenPointToRay(editorCamera.WorldToScreenPoint(selPart.transform.TransformPoint(attachNode3.position)))) && !attachNode4.icon.GetComponent<Renderer>().bounds.Contains(selPart.transform.TransformPoint(attachNode3.position))))
						{
							continue;
						}
						Vector3 normalized2 = part.transform.TransformDirection(attachNode4.orientation).normalized;
						float num = Vector3.Dot(normalized, normalized2);
						if (!CheatOptions.NonStrictAttachmentOrientation)
						{
							if (num < -0.7f)
							{
								attachNode2 = attachNode4;
								attachNode = attachNode3;
								attachment.potentialParent = part;
								stackRotation = Quaternion.FromToRotation(normalized, -normalized2);
								attachment.rotation = stackRotation * vesselRotation;
								attachment.position = part.transform.TransformPoint(attachNode2.position + attachNode2.offset) - attachment.rotation * selPart.attRotation * attachNode.position;
								break;
							}
						}
						else if (!(Mathf.Abs(num) <= 0.7f))
						{
							attachNode2 = attachNode4;
							attachNode = attachNode3;
							attachment.potentialParent = part;
							stackRotation = Quaternion.FromToRotation(normalized * Mathf.Sign(num), normalized2);
							attachment.rotation = stackRotation * vesselRotation;
							attachment.position = part.transform.TransformPoint(attachNode2.position + attachNode2.offset) - attachment.rotation * selPart.attRotation * attachNode.position;
							break;
						}
					}
				}
				if (attachNode != null && attachNode2 != null)
				{
					flag = true;
					attachment.callerPartNode = attachNode;
					attachment.otherPartNode = attachNode2;
					selPart.attachMode = AttachModes.STACK;
				}
			}
		}
		if ((selPart.attachRules.srfAttach && allowSrfAttachment && !GameSettings.MODIFIER_KEY.GetKey()) || (GameSettings.MODIFIER_KEY.GetKey() && isCurrentPartFlag))
		{
			this.ray = editorCamera.ScreenPointToRay(Input.mousePosition - srfAttachCursorOffset);
			if (Physics.Raycast(this.ray, out hit, 1000f, LayerUtil.DefaultEquivalent))
			{
				Part part2 = FlightGlobals.GetPartUpwardsCached(hit.collider.gameObject);
				if (isCurrentPartFlag)
				{
					FairingHitCollider = hit.collider;
				}
				else
				{
					FairingHitCollider = null;
					ModuleProceduralFairing moduleProceduralFairing = null;
					if (part2 != null)
					{
						moduleProceduralFairing = part2.FindModuleImplementing<ModuleProceduralFairing>();
						if (moduleProceduralFairing != null)
						{
							for (int l = 0; l < moduleProceduralFairing.Panels.Count; l++)
							{
								if (hit.collider.transform.FindParent(moduleProceduralFairing.Panels[l].ColliderContainer.name, findActiveParent: false) != null)
								{
									attachment.possible = false;
									return attachment;
								}
							}
						}
					}
				}
				if (part2 != null)
				{
					if (!ship.Contains(part2))
					{
						part2 = null;
					}
					if (hit.collider.CompareTag("NoAttach"))
					{
						part2 = null;
					}
					if ((bool)part2 && part2.attachRules.allowSrfAttach)
					{
						attachment.callerPartNode = selPart.srfAttachNode;
						attachment.otherPartNode = null;
						selPart.attachMode = AttachModes.SRF_ATTACH;
						selPart.srfAttachNode.srfAttachMeshName = hit.collider.name;
						attachment.potentialParent = part2;
						flag2 = true;
						attachment.rotation = Quaternion.LookRotation(hit.normal, part2.transform.rotation * Vector3.up) * Quaternion.LookRotation(selPart.srfAttachNode.orientation, Vector3.up);
						attachment.position = hit.point - attachment.rotation * selPart.attRotation * selPart.srfAttachNode.position;
						if (GameSettings.VAB_USE_ANGLE_SNAP)
						{
							Vector3 vector = hit.point - part2.transform.position;
							Ray ray = new Ray(part2.transform.position, part2.transform.up);
							Vector3 vector2 = Vector3.ProjectOnPlane(vector, ray.direction);
							Vector3 forward = part2.transform.forward;
							float num2 = Vector3.Dot(vector, ray.direction);
							float num3 = KSPUtil.BearingDegrees(forward, vector2.normalized, ray.direction);
							float num4 = (GameSettings.Editor_fineTweak.GetKey() ? srfAttachAngleSnapFine : srfAttachAngleSnap);
							float angle = ((!(num3 / num4 - Mathf.Floor(num3 / num4) > 0.5f)) ? ((float)Mathf.FloorToInt(num3 / num4) * num4) : ((float)Mathf.CeilToInt(num3 / num4) * num4));
							Vector3 vector3 = Quaternion.AngleAxis(angle, ray.direction) * forward;
							Vector3 vector4 = ray.origin + ray.direction * num2;
							vector.Normalize();
							vector4 += vector3 * FindPartSurface(part2, ray.origin, ray.direction, vector, hit, out var srfNormal);
							if (Mathf.Abs(Vector3.Dot(srfNormal, ray.direction)) < 1f)
							{
								srfNormal = Quaternion.FromToRotation(Vector3.ProjectOnPlane(srfNormal, ray.direction).normalized, vector3) * srfNormal;
							}
							attachment.rotation = Quaternion.LookRotation(srfNormal, part2.transform.rotation * Vector3.up) * Quaternion.LookRotation(selPart.srfAttachNode.orientation, Vector3.up);
							attachment.position = vector4 - attachment.rotation * selPart.attRotation * selPart.srfAttachNode.position;
						}
					}
				}
			}
		}
		attachment.mode = selPart.attachMode;
		attachment.collision = false;
		attachment.possible = (flag2 || flag) && !attachment.collision;
		return attachment;
	}

	public bool checkEditorCollision(Attachment attach)
	{
		if (CheatOptions.AllowPartClipping)
		{
			return false;
		}
		if ((bool)attach.potentialParent)
		{
			if (attach.caller.editorCollision != null)
			{
				if (attach.caller.editorCollision == attach.potentialParent)
				{
					attach.caller.editorCollision = null;
				}
				if (attach.caller.symmetryCounterparts.Contains(attach.caller.editorCollision))
				{
					attach.caller.editorCollision = null;
				}
			}
			if (!attach.caller.attachRules.allowCollision && !attach.potentialParent.attachRules.allowCollision && (bool)attach.caller.editorCollision)
			{
				return !PartInHierarchy(attach.caller, attach.caller.editorCollision);
			}
			return false;
		}
		if (!attach.caller.attachRules.allowCollision && (bool)attach.caller.editorCollision)
		{
			return !PartInHierarchy(attach.caller, attach.caller.editorCollision);
		}
		return false;
	}

	public Attachment[] CheckSymPartsAttach(Attachment oAttach)
	{
		Attachment[] array = new Attachment[selectedPart.symmetryCounterparts.Count];
		int i = 0;
		for (int count = selectedPart.symmetryCounterparts.Count; i < count; i++)
		{
			Part part = selectedPart.symmetryCounterparts[i];
			Attachment attachment = new Attachment();
			switch (oAttach.mode)
			{
			case AttachModes.SRF_ATTACH:
				attachment.caller = part;
				attachment.position = part.transform.position;
				attachment.rotation = part.transform.rotation;
				attachment.callerPartNode = part.srfAttachNode;
				attachment.potentialParent = part.potentialParent;
				attachment.collision = false;
				attachment.possible = true;
				highlightSelected(attachment.possible, part);
				array[i] = attachment;
				break;
			case AttachModes.STACK:
				attachment.caller = part;
				attachment.collision = false;
				if (oAttach.callerPartNode != null)
				{
					attachment.callerPartNode = part.FindAttachNode(oAttach.callerPartNode.id);
					AttachNode attachNode = null;
					float num = float.MaxValue;
					AttachNode attachNode2 = null;
					Part part2 = null;
					int count2 = ship.Count;
					for (int j = 0; j < count2; j++)
					{
						part2 = ship[j];
						if (!(oAttach.potentialParent == part2) && !oAttach.potentialParent.symmetryCounterparts.Contains(part2))
						{
							continue;
						}
						int k = 0;
						for (int count3 = part2.attachNodes.Count; k < count3; k++)
						{
							attachNode2 = part2.attachNodes[k];
							float sqrMagnitude = (part.transform.TransformPoint(oAttach.callerPartNode.position) - part2.transform.TransformPoint(attachNode2.position)).sqrMagnitude;
							if (sqrMagnitude < num)
							{
								attachNode = attachNode2;
								num = sqrMagnitude;
								attachment.potentialParent = part2;
							}
						}
					}
					AttachNode attachNode3 = attachment.callerPartNode;
					num = float.MaxValue;
					int l = 0;
					for (int count4 = part.attachNodes.Count; l < count4; l++)
					{
						attachNode2 = part.attachNodes[l];
						float sqrMagnitude2 = (attachment.potentialParent.transform.TransformPoint(attachNode.position) - part.transform.TransformPoint(attachNode2.position)).sqrMagnitude;
						if (sqrMagnitude2 < num)
						{
							num = sqrMagnitude2;
							attachNode3 = attachNode2;
						}
					}
					attachment.callerPartNode = attachNode3;
					if (attachNode != null)
					{
						attachment.otherPartNode = attachNode;
						attachment.position = attachment.potentialParent.transform.TransformPoint(attachNode.position) - part.transform.rotation * attachNode3.position;
					}
					attachment.possible = attachNode != null && !attachment.collision && attachNode3 != null;
					highlightSelected(attachment.possible, part);
					if (attachment.possible)
					{
						part.transform.position = attachment.position;
					}
					array[i] = attachment;
				}
				else
				{
					attachment.callerPartNode = null;
					attachment.possible = false;
					attachment.position = part.transform.position;
					attachment.rotation = part.transform.rotation;
					attachment.potentialParent = null;
					array[i] = attachment;
				}
				break;
			}
		}
		if (oAttach.mode == AttachModes.STACK)
		{
			Attachment attachment2 = null;
			Attachment attachment3 = null;
			int m = 0;
			for (int num2 = array.Length; m < num2; m++)
			{
				attachment2 = array[m];
				int n = 0;
				for (int num3 = array.Length; n < num3; n++)
				{
					attachment3 = array[n];
					if (!(attachment2.caller == attachment3.caller) && !(attachment2.potentialParent != attachment3.potentialParent) && attachment2.otherPartNode == attachment3.otherPartNode)
					{
						attachment2.possible = false;
					}
				}
				if (!(attachment2.potentialParent != oAttach.potentialParent) && attachment2.otherPartNode == oAttach.otherPartNode)
				{
					attachment2.possible = false;
				}
			}
		}
		return array;
	}

	public void attachPart(Part part, Attachment attach)
	{
		if (part == rootPart)
		{
			return;
		}
		part.isAttached = true;
		addToShip(part);
		MonoBehaviour.print(part.name + " added to ship - part count: " + ship.Count);
		highlightSelected(attach: true, part, reset: true);
		if (attach.potentialParent != null)
		{
			part.setParent(attach.potentialParent);
			part.transform.parent = attach.potentialParent.transform;
		}
		if (attach.callerPartNode != null)
		{
			attach.callerPartNode.attachedPart = attach.potentialParent;
		}
		if (attach.otherPartNode != null)
		{
			attach.otherPartNode.attachedPart = part;
		}
		part.attPos0 = part.transform.localPosition;
		part.attRotation0 = part.transform.localRotation;
		part.onAttach(attach.potentialParent);
		if (part != null)
		{
			for (int i = 0; i < part.Modules.Count; i++)
			{
				part.Modules[i].enabled = true;
			}
		}
		if (part.isCompund)
		{
			selectedCompoundPart = part as CompoundPart;
		}
	}

	public void detachPart(Part part)
	{
		if (!(part == rootPart))
		{
			if (ship.Contains(part))
			{
				removeFromShip(part);
			}
			EditorLogicBase.clearAttachNodes(part, part.parent);
			part.setParent();
			part.transform.parent = null;
			part.onDetach();
		}
	}

	public void CopyActionGroups(Part sPart, Part cPart)
	{
		if (sPart == null || cPart == null || sPart == cPart)
		{
			return;
		}
		int count = cPart.Modules.Count;
		while (count-- > 0)
		{
			if (sPart.Modules[count].Actions.Count == cPart.Modules[count].Actions.Count)
			{
				for (int i = 0; i < cPart.Modules[count].Actions.Count; i++)
				{
					cPart.Modules[count].Actions[i].actionGroup = sPart.Modules[count].Actions[i].actionGroup;
				}
			}
		}
		int count2 = sPart.children.Count;
		while (count2-- > 0)
		{
			CopyActionGroups(sPart.children[count2], cPart.children[count2]);
		}
	}

	public bool attachSymParts(Attachment[] cAttaches)
	{
		int i = 0;
		for (int count = selectedPart.symmetryCounterparts.Count; i < count; i++)
		{
			Part part = selectedPart.symmetryCounterparts[i];
			attachPart(part, cAttaches[i]);
			part.gameObject.SetLayerRecursive(0, filterTranslucent: true, 2097152);
		}
		return true;
	}

	public void addToShip(Part part)
	{
		ship.Add(part);
		if ((bool)FlightGlobals.fetch)
		{
			FlightGlobals.PersistentLoadedPartIds.Add(part.persistentId, part);
		}
		Part part2 = null;
		List<Part> list = EditorLogicBase.FindPartsInChildren(part);
		int i = 0;
		for (int count = list.Count; i < count; i++)
		{
			part2 = list[i];
			if (part2 != part)
			{
				ship.Add(part2);
				if ((bool)FlightGlobals.fetch)
				{
					FlightGlobals.PersistentLoadedPartIds.Add(part2.persistentId, part2);
				}
			}
		}
	}

	public void removeFromShip(Part part)
	{
		ship.Remove(part);
		if ((bool)FlightGlobals.fetch)
		{
			FlightGlobals.PersistentLoadedPartIds.Remove(part.persistentId);
		}
		List<Part> list = EditorLogicBase.FindPartsInChildren(part);
		int i = 0;
		for (int count = list.Count; i < count; i++)
		{
			ship.Remove(list[i]);
			if ((bool)FlightGlobals.fetch)
			{
				FlightGlobals.PersistentLoadedPartIds.Remove(list[i].persistentId);
			}
		}
	}

	public void highlightSelected(bool attach, Part part, bool reset = false)
	{
		if (part == rootPart)
		{
			return;
		}
		if (part.hasHeiarchyModel)
		{
			if (reset)
			{
				part.SetHighlightDefault();
			}
			else if (attach)
			{
				part.SetHighlightColor(Highlighter.colorPartEditorAttached);
				part.SetHighlight(active: true, recursive: true);
			}
			else
			{
				part.SetHighlightColor(Highlighter.colorPartEditorDetached);
				part.SetHighlight(active: true, recursive: true);
			}
			part.SetOpacity(reset ? 1f : (attach ? 0.6f : 0.4f));
		}
		else
		{
			MeshRenderer[] componentsInChildren = part.transform.Find("model").GetComponentsInChildren<MeshRenderer>();
			int i = 0;
			for (int num = componentsInChildren.Length; i < num; i++)
			{
				MeshRenderer obj = componentsInChildren[i];
				obj.material.SetColor(PropertyIDs._RimColor, reset ? new Color(0f, 0f, 0f) : (attach ? new Color(0f, 1f, 0f) : new Color(1f, 0f, 0f)));
				obj.material.SetFloat(PropertyIDs._Opacity, reset ? 1f : (attach ? 0.6f : 0.4f));
			}
		}
		if (part.children.Count > 0)
		{
			int j = 0;
			for (int count = part.children.Count; j < count; j++)
			{
				Part part2 = part.children[j];
				highlightSelected(attach, part2, reset);
			}
		}
	}

	public void SetLastSanitizedSaveName()
	{
		vesselNameAtLastSave_Sanitized = KSPUtil.SanitizeString(vesselNameAtLastSave, '_', replaceEmpty: false);
		if (string.IsNullOrEmpty(vesselNameAtLastSave_Sanitized))
		{
			vesselNameAtLastSave_Sanitized = Localizer.Format("#autoLOC_900530");
		}
	}

	public void NewShip()
	{
		toolsUI.SetMode(ConstructionMode.Place);
		if (undoIndexAtLastSave != undoLevel && fetch.ship.parts.Count > 0)
		{
			WipeTooltips();
			InputLockManager.SetControlLock("SaveConfirmationDialog");
			ScreenMessages.PostScreenMessage("", modeMsg);
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("NewCraft", Localizer.Format("#autoLOC_6004037"), Localizer.Format("#autoLOC_127810"), UISkinManager.GetSkin("KSP window 7"), new DialogGUIButton(AllowSave ? Localizer.Format("#autoLOC_127975") : Localizer.Format("#autoLOC_6004036"), delegate
			{
				if (AllowSave)
				{
					OnNewShipDialogDismiss();
					saveShip(OnNewShipConfirm);
				}
			}, AllowSave), new DialogGUIButton(Localizer.Format("#autoLOC_127984"), OnNewShipDialogDismiss), new DialogGUIButton(Localizer.Format("#autoLOC_127985"), OnNewShipConfirm)), persistAcrossScenes: false, UISkinManager.GetSkin("KSP window 7")).OnDismiss = OnNewShipDialogDismiss;
		}
		else
		{
			OnNewShipConfirm();
		}
	}

	public void OnNewShipConfirm()
	{
		OnNewShipDialogDismiss();
		if ((bool)FlightGlobals.fetch)
		{
			FlightGlobals.PersistentVesselIds.Remove(ship.persistentId);
		}
		EditorDriver.StartupBehaviour = EditorDriver.StartupBehaviours.START_CLEAN;
		ShipConstruction.ShipConfig = null;
		StartEditor(isRestart: true);
	}

	public void OnNewShipDialogDismiss()
	{
		InputLockManager.RemoveControlLock("SaveConfirmationDialog");
		GameEvents.onEditorNewShipDialogDismiss.Fire();
	}

	public void onShipNameFieldSubmit(string name)
	{
		ship.shipName = name;
		if (ship.vesselNamedBy != null)
		{
			ship.vesselNamedBy.vesselNaming.vesselName = ship.shipName;
			ship.vesselNamedBy.RefreshVesselNamingPAWDisplay();
		}
	}

	public void onShipNameFieldValueChanged(string name)
	{
		if (name.Length > GameSettings.VAB_CRAFTNAME_CHAR_LIMIT)
		{
			shipNameField.text = name.Substring(0, GameSettings.VAB_CRAFTNAME_CHAR_LIMIT);
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_6003111", GameSettings.VAB_CRAFTNAME_CHAR_LIMIT), 2f, ScreenMessageStyle.UPPER_CENTER);
		}
		if (name.Length <= 0)
		{
			InputLockManager.SetControlLock(ControlTypes.EDITOR_SAVE, "EmptyName");
		}
		else
		{
			InputLockManager.RemoveControlLock("EmptyName");
		}
	}

	public void steamExport()
	{
		setSteamExportItemPublic = false;
		Part part = VesselNaming.FindPriorityNamePart(ship);
		if (part != null)
		{
			ship.RunVesselNamingUpdates(part, noGameEvent: true);
		}
		else
		{
			int count = ship.parts.Count;
			for (int i = 0; i < count; i++)
			{
				Part part2 = ship.parts[i];
				if (part2.vesselType > ship.vesselType)
				{
					ship.vesselType = part2.vesselType;
				}
			}
		}
		SteamWorkshopExportDialog.Spawn(Localizer.Format("#autoLOC_8002154"), Localizer.Format("#autoLOC_8002122"), Localizer.Format("#autoLOC_226976"), Localizer.Format("#autoLOC_226975"), Localizer.Format("#autoLOC_8002123"), delegate(SteamWorkshopExportDialog.ReturnItems returnItems)
		{
			setSteamExportItemPublic = returnItems.setPublic;
			setSteamExportItemChangeLog = returnItems.changeLog;
			setSteamModsText = returnItems.modsText;
			setSteamVesselType = returnItems.vesselType;
			setSteamRobiticsTag = returnItems.roboticTag;
			onSteamExportConfirm();
		}, null, showCancelBtn: true, showVisibilityOption: true, showChangeLog: true, showModsSection: true, showVesselTypeSection: true, ship.vesselType);
	}

	public void onSteamExportConfirm()
	{
		saveShip(onSteamExportAfterSave);
	}

	public void onSteamExportAfterSave()
	{
		timeSteamCommsStarted = Time.time;
		steamCommsDialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("ExportMissionSteamComms", "", Localizer.Format("#autoLOC_8002139"), UISkinManager.GetSkin("KSP window 7"), 300f, new DialogGUISpace(6f), new DialogGUILabel(Localizer.Format("#autoLOC_8002158"))), persistAcrossScenes: true, null);
		if (ship.steamPublishedFileId == 0L)
		{
			SteamManager.Instance.CreateNewItem(onNewItemCreated, EWorkshopFileType.k_EWorkshopFileTypeFirst);
		}
		else
		{
			SteamManager.Instance.GetUGCItem(onQuerySteamComplete, new PublishedFileId_t(ship.steamPublishedFileId));
		}
	}

	public void onQuerySteamComplete(SteamUGCQueryCompleted_t qryResults, bool bIOFailure)
	{
		if (bIOFailure)
		{
			AnalyticsUtil.LogSteamError(AnalyticsUtil.steamActions.query, AnalyticsUtil.steamItemTypes.craft, 0uL, qryResults.m_eResult);
			Debug.LogFormat("[EditorLogic]: Failed to create new item on steam. I/O error.");
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
					preupdateSteamItem(uGCItemDetails.m_nPublishedFileId, newItem: false);
				}
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
			AnalyticsUtil.LogSteamError(AnalyticsUtil.steamActions.update, AnalyticsUtil.steamItemTypes.craft, 0uL, qryResults.m_eResult);
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("SteamCraftError", Localizer.Format("#autoLOC_8002124", SteamManager.Instance.GetUGCFailureReason(qryResults.m_eResult)), Localizer.Format("#autoLOC_8002125"), UISkinManager.GetSkin("KSP window 7"), 350f, new DialogGUIButton(Localizer.Format("#autoLOC_226975"), null, dismissOnSelect: true)), persistAcrossScenes: false, UISkinManager.GetSkin("KSP window 7"));
			Debug.LogFormat("[EditorLogic]: Failed to export craft ({0}) to steam. Reason:{1}", ship.shipName, SteamManager.Instance.GetUGCFailureReason(qryResults.m_eResult));
		}
		SteamUGC.ReleaseQueryUGCRequest(qryResults.m_handle);
	}

	public void onNewItemCreated(CreateItemResult_t qryResults, bool bIOFailure)
	{
		if (bIOFailure)
		{
			AnalyticsUtil.LogSteamError(AnalyticsUtil.steamActions.create, AnalyticsUtil.steamItemTypes.craft, 0uL, qryResults.m_eResult);
			Debug.LogFormat("[EditorLogic]: Failed to create new item on steam. I/O error.");
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
			preupdateSteamItem(qryResults.m_nPublishedFileId, newItem: true);
			return;
		}
		if (steamCommsDialog != null)
		{
			steamCommsDialog.Dismiss();
			steamCommsDialog = null;
		}
		AnalyticsUtil.LogSteamError(AnalyticsUtil.steamActions.create, AnalyticsUtil.steamItemTypes.craft, qryResults.m_nPublishedFileId, qryResults.m_eResult);
		PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("SteamCraftError", Localizer.Format("#autoLOC_8002126", SteamManager.Instance.GetUGCFailureReason(qryResults.m_eResult)), Localizer.Format("#autoLOC_8002125"), UISkinManager.GetSkin("KSP window 7"), 350f, new DialogGUIButton(Localizer.Format("#autoLOC_226975"), null, dismissOnSelect: true)), persistAcrossScenes: false, UISkinManager.GetSkin("KSP window 7"));
		Debug.LogFormat("[EditorLogic]: Failed to create new item on steam. Steam Error:{0}", SteamManager.Instance.GetUGCFailureReason(qryResults.m_eResult));
	}

	public void preupdateSteamItem(PublishedFileId_t fileId, bool newItem)
	{
		ship.steamPublishedFileId = fileId.m_PublishedFileId;
		steamTags = new List<string>();
		steamTags.Add("Craft");
		steamTags.Add(setSteamVesselType.ToString());
		if (setSteamRobiticsTag)
		{
			steamTags.Add("Robotics");
		}
		steamVisibility = ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPrivate;
		if (setSteamExportItemPublic)
		{
			steamVisibility = ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic;
		}
		string savePath = ShipConstruction.GetSavePath(shipNameField.text);
		string text = KSPUtil.SanitizeString(shipNameField.text, '_', replaceEmpty: true);
		saveShip();
		steamExportTempPath = KSPUtil.ApplicationRootPath + "steamExport/" + text + "/";
		if (Directory.Exists(steamExportTempPath))
		{
			Directory.Delete(steamExportTempPath);
		}
		Directory.CreateDirectory(steamExportTempPath);
		steamThumbURL = string.Concat(KSPUtil.ApplicationRootPath, "thumbs/", HighLogic.SaveFolder, "_", EditorDriver.editorFacility, "_", Path.GetFileNameWithoutExtension(savePath), ".png");
		File.Copy(savePath, steamExportTempPath + Path.GetFileName(savePath));
		File.Copy(steamThumbURL, steamExportTempPath + text + ".png");
		ulong availAmount = 0uL;
		ulong totalRequired = 0uL;
		int totalFilesRequired = 0;
		int availFileCount = 0;
		if (!SteamManager.Instance.CheckCloudQuota(steamExportTempPath, out totalRequired, out availAmount, out totalFilesRequired, out availFileCount))
		{
			if (newItem)
			{
				SteamManager.Instance.deleteItem(fileId, null);
			}
			string text2 = "";
			if (totalRequired > availAmount)
			{
				AnalyticsUtil.LogSteamError(AnalyticsUtil.steamActions.update, AnalyticsUtil.steamItemTypes.craft, fileId, "k_EResultCustom_SteamCloudSpaceExceeded");
				text2 = Localizer.Format("#autoLOC_8002159", (totalRequired / 1024L).ToString("N0"), (availAmount / 1024L).ToString("N0"));
			}
			if (totalFilesRequired > availFileCount)
			{
				AnalyticsUtil.LogSteamError(AnalyticsUtil.steamActions.update, AnalyticsUtil.steamItemTypes.craft, fileId, "k_EResultCustom_SteamCloudFileLimitExceeded");
				text2 = text2 + "\n" + Localizer.Format("#autoLOC_8002160", totalFilesRequired, availFileCount);
			}
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("ExportCraftSteamNoSpace", text2, Localizer.Format("#autoLOC_8002139"), UISkinManager.GetSkin("KSP window 7"), 300f, new DialogGUISpace(6f), new DialogGUIButton(Localizer.Format("#autoLOC_226975"), null, dismissOnSelect: true)), persistAcrossScenes: true, null);
			if (steamCommsDialog != null)
			{
				steamCommsDialog.Dismiss();
				steamCommsDialog = null;
			}
			steamUploadingNewItem = false;
			Debug.LogFormat("[MissionBriefing]: Unable to export Mission as there is not enough space on Steam Cloud.");
		}
		else
		{
			float dryCost = 0f;
			float fuelCost = 0f;
			steamTotalCost = ship.GetShipCosts(out dryCost, out fuelCost);
			steamCrewCapacity = 0;
			for (int i = 0; i < ship.parts.Count; i++)
			{
				steamCrewCapacity += ship.parts[i].CrewCapacity;
			}
			if (!newItem)
			{
				SteamManager.Instance.GetAppDependencies(onGetAppDependency, fileId);
			}
			else
			{
				updateSteamItem(fileId);
			}
		}
	}

	public void onGetAppDependency(GetAppDependenciesResult_t result, bool bIOFailure)
	{
		if (bIOFailure)
		{
			AnalyticsUtil.LogSteamError(AnalyticsUtil.steamActions.update, AnalyticsUtil.steamItemTypes.mission, result.m_nPublishedFileId, result.m_eResult);
			Debug.LogFormat("[EditorLogic]: Failed to Get App Dependencies to update a Steam Item. I/O error.");
			if (steamCommsDialog != null)
			{
				steamCommsDialog.Dismiss();
				steamCommsDialog = null;
			}
			steamUploadingNewItem = false;
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
				updateSteamItem(result.m_nPublishedFileId);
			}
			return;
		}
		if (steamCommsDialog != null)
		{
			steamCommsDialog.Dismiss();
			steamCommsDialog = null;
		}
		AnalyticsUtil.LogSteamError(AnalyticsUtil.steamActions.update, AnalyticsUtil.steamItemTypes.mission, result.m_nPublishedFileId, result.m_eResult);
		PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("SteamError", Localizer.Format("#autoLOC_8002128", SteamManager.Instance.GetUGCFailureReason(result.m_eResult)), Localizer.Format("#autoLOC_8002125"), UISkinManager.GetSkin("KSP window 7"), 350f, new DialogGUIButton(Localizer.Format("#autoLOC_226975"), null, dismissOnSelect: true)), persistAcrossScenes: false, UISkinManager.GetSkin("KSP window 7"));
		Debug.LogFormat("[EditorLogic]: Failed to update item on steam. Unable to determine Steam App dependencies. Steam Error:{0}", SteamManager.Instance.GetUGCFailureReason(result.m_eResult));
		steamUploadingNewItem = false;
	}

	public void onAppDependencyRemoved(RemoveAppDependencyResult_t result, bool bIOFailure)
	{
		if (bIOFailure)
		{
			AnalyticsUtil.LogSteamError(AnalyticsUtil.steamActions.update, AnalyticsUtil.steamItemTypes.mission, result.m_nPublishedFileId, result.m_eResult);
			Debug.LogFormat("[EditorLogic]: Failed to remove App Dependency when updating an item on steam. I/O error.");
			if (steamCommsDialog != null)
			{
				steamCommsDialog.Dismiss();
				steamCommsDialog = null;
			}
			steamUploadingNewItem = false;
			return;
		}
		if (result.m_eResult == EResult.k_EResultOK)
		{
			appDependenciesToRemove.Remove(result.m_nAppID);
			if (appDependenciesToRemove.Count == 0)
			{
				updateSteamItem(result.m_nPublishedFileId);
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
		PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("SteamError", Localizer.Format("#autoLOC_8002128", SteamManager.Instance.GetUGCFailureReason(result.m_eResult)), Localizer.Format("#autoLOC_8002125"), UISkinManager.GetSkin("KSP window 7"), 350f, new DialogGUIButton(Localizer.Format("#autoLOC_226975"), null, dismissOnSelect: true)), persistAcrossScenes: false, UISkinManager.GetSkin("KSP window 7"));
		Debug.LogFormat("[EditorLogic]: Failed to update item on steam. Unable to remove Steam App dependencies. Steam Error:{0}", SteamManager.Instance.GetUGCFailureReason(result.m_eResult));
	}

	public void updateSteamItem(PublishedFileId_t fileId)
	{
		if (setSteamRobiticsTag)
		{
			SteamManager.Instance.UpdateItem(onItemUpdated, fileId, steamTags, steamExportTempPath, steamThumbURL, ship.shipName, ship.shipDescription, setSteamExportItemChangeLog, steamVisibility, SteamCraftInfo.CreateMetaData(setSteamModsText, setSteamVesselType.ToString(), ship.GetTotalMass(), steamTotalCost, ship.parts.Count, StageManager.Instance.Stages.Count, steamCrewCapacity), new AppId_t[1] { SteamManager.BreakingGroundAppID });
		}
		else
		{
			SteamManager.Instance.UpdateItem(onItemUpdated, fileId, steamTags, steamExportTempPath, steamThumbURL, ship.shipName, ship.shipDescription, setSteamExportItemChangeLog, steamVisibility, SteamCraftInfo.CreateMetaData(setSteamModsText, setSteamVesselType.ToString(), ship.GetTotalMass(), steamTotalCost, ship.parts.Count, StageManager.Instance.Stages.Count, steamCrewCapacity));
		}
	}

	public void onItemUpdated(SubmitItemUpdateResult_t updateResult, bool bIOFailure)
	{
		if (steamCommsDialog != null)
		{
			steamCommsDialog.Dismiss();
			steamCommsDialog = null;
		}
		if (!string.IsNullOrEmpty(steamExportTempPath))
		{
			Directory.Delete(steamExportTempPath, recursive: true);
		}
		if (bIOFailure)
		{
			AnalyticsUtil.LogSteamError(AnalyticsUtil.steamActions.update, AnalyticsUtil.steamItemTypes.craft, 0uL, updateResult.m_eResult);
			Debug.LogFormat("[EditorLogic]: Failed to create new item on steam. I/O error.");
			steamUploadingNewItem = false;
			return;
		}
		if (updateResult.m_eResult == EResult.k_EResultOK)
		{
			if (steamUploadingNewItem)
			{
				AnalyticsUtil.LogSteamItemCreated(AnalyticsUtil.steamItemTypes.craft, updateResult.m_nPublishedFileId);
			}
			else
			{
				AnalyticsUtil.LogSteamItemUpdated(AnalyticsUtil.steamItemTypes.craft, updateResult.m_nPublishedFileId);
			}
			Debug.LogFormat("[EditorLogic]: Item ({0}) has been successfully Updated to Steam Workshop", updateResult.m_nPublishedFileId);
			saveShip();
			if (updateResult.m_bUserNeedsToAcceptWorkshopLegalAgreement)
			{
				SteamWorkshopExportDialog.Spawn(Localizer.Format("#autoLOC_8002154"), Localizer.Format("#autoLOC_8002127"), "", Localizer.Format("#autoLOC_226975"), "", delegate
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
			AnalyticsUtil.LogSteamError(AnalyticsUtil.steamActions.update, AnalyticsUtil.steamItemTypes.craft, updateResult.m_nPublishedFileId, updateResult.m_eResult);
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("SteamError", Localizer.Format("#autoLOC_8002128", SteamManager.Instance.GetUGCFailureReason(updateResult.m_eResult)), Localizer.Format("#autoLOC_8002125"), UISkinManager.GetSkin("KSP window 7"), 350f, new DialogGUIButton(Localizer.Format("#autoLOC_226975"), null, dismissOnSelect: true)), persistAcrossScenes: false, UISkinManager.GetSkin("KSP window 7"));
			Debug.LogFormat("[EditorLogic]: Failed to Update item ({0}) on steam. Steam Error:{1}", updateResult.m_nPublishedFileId, SteamManager.Instance.GetUGCFailureReason(updateResult.m_eResult));
		}
		steamUploadingNewItem = false;
	}

	public void saveShip()
	{
		InputLockManager.SetControlLock(ControlTypes.EDITOR_SOFT_LOCK, "Saving");
		StartCoroutine(CallbackUtil.DelayedCallback(1, delegate
		{
			saveShip(delegate
			{
			});
		}));
	}

	public bool saveShip(Callback afterSave)
	{
		bool saveCompleted = false;
		string text = KSPUtil.SanitizeString(shipNameField.text, '_', replaceEmpty: false);
		if (string.IsNullOrEmpty(text))
		{
			text = Localizer.Format("#autoLOC_900530");
		}
		if (text != vesselNameAtLastSave_Sanitized && File.Exists(ShipConstruction.GetSavePath(text)))
		{
			WipeTooltips();
			InputLockManager.SetControlLock("SaveConfirmationDialog");
			ScreenMessages.PostScreenMessage("", modeMsg);
			if (HighLogic.CurrentGame.Mode == Game.Modes.MISSION && MissionCraftIsCreators(text))
			{
				PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("CreatorCraftName", Localizer.Format("#autoLOC_8002098", text), Localizer.Format("#autoLOC_8002099"), UISkinManager.GetSkin("KSP window 7"), 350f, new DialogGUIButton("<color=orange>" + Localizer.Format("#autoLOC_417274") + "</color>", onSaveConfirmDismiss)), persistAcrossScenes: false, UISkinManager.GetSkin("KSP window 7")).OnDismiss = onSaveConfirmDismiss;
			}
			else
			{
				PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("SaveCraftOverwrite", Localizer.Format("#autoLOC_127865", text), Localizer.Format("#autoLOC_127866"), UISkinManager.GetSkin("KSP window 7"), new DialogGUIButton(Localizer.Format("#autoLOC_127867"), delegate
				{
					saveCompleted = true;
					onSaveConfirm();
					afterSave();
				}), new DialogGUIButton(Localizer.Format("#autoLOC_127984"), onSaveConfirmDismiss)), persistAcrossScenes: false, UISkinManager.GetSkin("KSP window 7")).OnDismiss = onSaveConfirmDismiss;
			}
		}
		else if ((HighLogic.CurrentGame.Mode == Game.Modes.MISSION || HighLogic.CurrentGame.Mode == Game.Modes.MISSION_BUILDER) && MissionCraftInOtherFolder(shipNameField.text))
		{
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("CreatorCraftName", Localizer.Format("#autoLOC_6011149", text, (EditorDriver.editorFacility == EditorFacility.const_1) ? Localizer.Format("#autoLOC_6002119") : Localizer.Format("#autoLOC_6002108")), Localizer.Format("#autoLOC_8002099"), UISkinManager.GetSkin("KSP window 7"), 350f, new DialogGUIButton("<color=orange>" + Localizer.Format("#autoLOC_417274") + "</color>", onSaveConfirmDismiss)), persistAcrossScenes: false, UISkinManager.GetSkin("KSP window 7")).OnDismiss = onSaveConfirmDismiss;
		}
		else
		{
			saveCompleted = true;
			onSaveConfirm();
			afterSave();
			InputLockManager.RemoveControlLock("Saving");
		}
		return saveCompleted;
	}

	public bool MissionCraftIsCreators(string shipName)
	{
		if (MissionSystem.missions.Count > 0)
		{
			string text = KSPUtil.SanitizeString(shipName, '_', replaceEmpty: true) + ".craft";
			DictionaryValueList<VesselSituation, Guid> allVesselSituationsGuid = MissionSystem.missions[0].GetAllVesselSituationsGuid();
			for (int i = 0; i < allVesselSituationsGuid.Count; i++)
			{
				VesselSituation vesselSituation = allVesselSituationsGuid.KeyAt(i);
				if (!vesselSituation.playerCreated && vesselSituation.craftFile == text)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool MissionCraftInOtherFolder(string shipName)
	{
		string text = KSPUtil.SanitizeString(shipName, '_', replaceEmpty: true) + ".craft";
		if (HighLogic.CurrentGame.Mode == Game.Modes.MISSION)
		{
			if (File.Exists(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/Ships/" + ShipConstruction.GetShipsSubfolderFor((EditorDriver.editorFacility != EditorFacility.const_1) ? EditorFacility.const_1 : EditorFacility.const_2) + "/" + text))
			{
				return true;
			}
		}
		else if (HighLogic.CurrentGame.Mode == Game.Modes.MISSION_BUILDER && MissionSystem.missions.Count > 0 && File.Exists(MissionSystem.missions[0].MissionInfo.FolderPath + "Ships/" + ShipConstruction.GetShipsSubfolderFor((EditorDriver.editorFacility != EditorFacility.const_1) ? EditorFacility.const_1 : EditorFacility.const_2) + "/" + text))
		{
			return true;
		}
		return false;
	}

	public void onSaveConfirm()
	{
		onSaveConfirmDismiss();
		if (shipNameField.text != vesselNameAtLastSave)
		{
			ship.steamPublishedFileId = 0uL;
		}
		ship.shipName = shipNameField.text;
		ship.shipDescription = shipDescriptionField.text;
		ship.missionFlag = FlagURL;
		ShipConstruction.CreateBackup(ship);
		undoIndexAtLastSave = undoLevel;
		vesselNameAtLastSave = ship.shipName;
		SetLastSanitizedSaveName();
		ShipConstruction.SaveShipToPath(shipNameField.text, EditorDriver.DefaultCraftSavePath);
	}

	public void onSaveConfirmDismiss()
	{
		InputLockManager.RemoveControlLock("SaveConfirmationDialog");
		InputLockManager.RemoveControlLock("Saving");
	}

	public void exitEditor()
	{
		if (HighLogic.CurrentGame.Mode == Game.Modes.SCENARIO_NON_RESUMABLE)
		{
			FinalizeAnalytics();
			HighLogic.LoadScene(GameScenes.MAINMENU);
		}
		if (HighLogic.CurrentGame.Mode == Game.Modes.MISSION && ExpansionsLoader.IsExpansionInstalled("MakingHistory") && !MissionSystem.missions[0].isStarted)
		{
			FinalizeAnalytics();
			if (MissionsApp.Instance != null)
			{
				MissionsApp.Instance.ExitEditor();
			}
			GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE, HighLogic.CurrentGame.startScene);
			if (MissionSystem.IsTestMode)
			{
				FlightGlobals.ClearpersistentIdDictionaries();
				MissionEditorLogic.StartUpMissionEditor(MissionSystem.missions[0].MissionInfo.FilePath);
			}
			else
			{
				HighLogic.LoadScene(GameScenes.MAINMENU);
			}
		}
		else
		{
			onExitContinue();
		}
	}

	public void onExitContinue()
	{
		if (fetch.ship == null)
		{
			onExitConfirm();
		}
		else if (undoIndexAtLastSave != undoLevel && fetch.ship.parts.Count > 0)
		{
			WipeTooltips();
			InputLockManager.SetControlLock("ExitConfirmationDialog");
			ScreenMessages.PostScreenMessage("", modeMsg);
			string text = Localizer.Format(ScenarioUpgradeableFacilities.GetFacilityName(EditorDriver.editorFacility.ToFacility()));
			PopupDialog popupDialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("Leave" + text, Localizer.Format("#autoLOC_6004037"), Localizer.Format("#autoLOC_127918", text), UISkinManager.GetSkin("KSP window 7"), new DialogGUIButton(AllowSave ? Localizer.Format("#autoLOC_127975") : Localizer.Format("#autoLOC_6004036"), delegate
			{
				if (AllowSave)
				{
					onExitConfirmDismiss();
					saveShip(onExitConfirm);
				}
			}, AllowSave), new DialogGUIButton(Localizer.Format("#autoLOC_127984"), onExitConfirmDismiss), new DialogGUIButton(Localizer.Format("#autoLOC_127985"), onExitConfirm)), persistAcrossScenes: false, UISkinManager.GetSkin("KSP window 7"));
			popupDialog.OnDismiss = onExitConfirmDismiss;
			MenuNavigation.SpawnMenuNavigation(popupDialog.gameObject, Navigation.Mode.Vertical, limitCheck: true);
		}
		else
		{
			onExitConfirm();
		}
	}

	public void onExitConfirm()
	{
		onExitConfirmDismiss();
		ShipConstruction.CreateBackup(ship);
		if (ExpansionsLoader.IsExpansionInstalled("MakingHistory") && HighLogic.CurrentGame != null && HighLogic.CurrentGame.Mode == Game.Modes.MISSION_BUILDER)
		{
			FlightGlobals.ClearpersistentIdDictionaries();
			MissionEditorLogic.StartUpMissionEditor(MissionSystem.missions[0].MissionInfo.FilePath);
			return;
		}
		if (HighLogic.CurrentGame.Parameters.Editor.CanLeaveToSpaceCenter && HighLogic.CurrentGame.Mode == Game.Modes.MISSION && MissionsApp.Instance != null && MissionsApp.Instance.Mode != MissionAppMode.EditorFreeBuild)
		{
			VesselSituation vesselSituation = null;
			if (MissionsApp.Instance.CurrentVessel != null && MissionsApp.Instance.CurrentVessel.vesselSituation != null)
			{
				vesselSituation = MissionsApp.Instance.CurrentVessel.vesselSituation;
				vesselSituation.mission.situation.VesselSituationRevertLaunch(vesselSituation);
			}
		}
		ShipConstruction.ShipManifest = CrewAssignmentDialog.Instance.GetManifest();
		EditorDriver.saveselectedLaunchSite();
		GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
		if (!HighLogic.CurrentGame.Parameters.Editor.CanLeaveToSpaceCenter && HighLogic.CurrentGame.Parameters.Editor.CanLeaveToMainMenu)
		{
			FinalizeAnalytics();
			HighLogic.LoadScene(GameScenes.MAINMENU);
		}
		else if (HighLogic.CurrentGame.Parameters.Editor.CanLeaveToSpaceCenter)
		{
			FinalizeAnalytics();
			HighLogic.LoadScene(GameScenes.SPACECENTER);
		}
	}

	public void onExitConfirmDismiss()
	{
		InputLockManager.RemoveControlLock("ExitConfirmationDialog");
	}

	public void loadShip()
	{
		WipeTooltips();
		toolsUI.SetMode(ConstructionMode.Place);
		if (undoIndexAtLastSave != undoLevel && fetch.ship.parts.Count > 0)
		{
			InputLockManager.SetControlLock("LoadConfirmationDialog");
			ScreenMessages.PostScreenMessage("", modeMsg);
			PopupDialog popupDialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("LoadCraft", Localizer.Format("#autoLOC_6004037"), Localizer.Format("#autoLOC_127973"), UISkinManager.GetSkin("KSP window 7"), new DialogGUIButton(AllowSave ? Localizer.Format("#autoLOC_127975") : Localizer.Format("#autoLOC_6004036"), delegate
			{
				if (AllowSave)
				{
					onLoadConfirmDismiss();
					saveShip(onLoadConfirm);
				}
			}, AllowSave), new DialogGUIButton(Localizer.Format("#autoLOC_127984"), onLoadConfirmDismiss), new DialogGUIButton(Localizer.Format("#autoLOC_127985"), onLoadConfirm)), persistAcrossScenes: false, UISkinManager.GetSkin("KSP window 7"));
			popupDialog.OnDismiss = onLoadConfirmDismiss;
			MenuNavigation.SpawnMenuNavigation(popupDialog.gameObject, Navigation.Mode.Automatic, limitCheck: true);
		}
		else
		{
			onLoadConfirm();
		}
	}

	public void changeCoordSpace()
	{
		if (gizmoOffset != null)
		{
			switch (gizmoOffset.CoordSpace)
			{
			case Space.Self:
				gizmoOffset.SetCoordSystem(Space.World);
				coordSpaceText.text = EditorLogicBase.cacheAutoLOC_6001217;
				ScreenMessages.PostScreenMessage(EditorLogicBase.cacheAutoLOC_6001221, modeMsg);
				break;
			case Space.World:
				gizmoOffset.SetCoordSystem(Space.Self);
				coordSpaceText.text = EditorLogicBase.cacheAutoLOC_6001218;
				ScreenMessages.PostScreenMessage(EditorLogicBase.cacheAutoLOC_6001222, modeMsg);
				break;
			}
		}
		else if (gizmoRotate != null)
		{
			switch (gizmoRotate.CoordSpace)
			{
			case Space.Self:
				gizmoRotate.SetCoordSystem(Space.World);
				coordSpaceText.text = EditorLogicBase.cacheAutoLOC_6001217;
				ScreenMessages.PostScreenMessage(EditorLogicBase.cacheAutoLOC_6001223, modeMsg);
				break;
			case Space.World:
				gizmoRotate.SetCoordSystem(Space.Self);
				coordSpaceText.text = EditorLogicBase.cacheAutoLOC_6001218;
				ScreenMessages.PostScreenMessage(EditorLogicBase.cacheAutoLOC_6001224, modeMsg);
				break;
			}
		}
	}

	public void changeRadialSymmetrySpace()
	{
		if (symmetryMethod != 0)
		{
			return;
		}
		RestoreSymmetryState();
		if (symmetryMethod == SymmetryMethod.Radial)
		{
			switch (symmetryCoordSpace)
			{
			case Space.Self:
				symmetryCoordSpace = Space.World;
				radialSymmetryText.text = EditorLogicBase.cacheAutoLOC_6001220;
				ScreenMessages.PostScreenMessage(EditorLogicBase.cacheAutoLOC_6001225, modeMsg);
				break;
			case Space.World:
				symmetryCoordSpace = Space.Self;
				radialSymmetryText.text = EditorLogicBase.cacheAutoLOC_6001219;
				ScreenMessages.PostScreenMessage(EditorLogicBase.cacheAutoLOC_6001226, modeMsg);
				break;
			}
			GameEvents.onEditorSymmetryCoordsChange.Fire(symmetryCoordSpace);
		}
	}

	public void onLoadConfirm()
	{
		onLoadConfirmDismiss();
		string profile = HighLogic.SaveFolder;
		if (HighLogic.CurrentGame != null && HighLogic.CurrentGame.Mode == Game.Modes.MISSION_BUILDER && ExpansionsLoader.IsExpansionInstalled("MakingHistory") && MissionSystem.missions.Count > 0)
		{
			profile = MissionSystem.missions[0].MissionInfo.ShipFolderPath;
		}
		if (craftBrowserDialog == null)
		{
			craftBrowserDialog = CraftBrowserDialog.Spawn(EditorDriver.editorFacility, profile, (CraftBrowserDialog.SelectConfigNodeCallback)ShipToLoadSelected, (CraftBrowserDialog.CancelledCallback)CraftBrowseCancelled, ship != null && ship.parts.Count > 0);
		}
		else
		{
			craftBrowserDialog.ReDisplay(EditorDriver.editorFacility, ship != null && ship.parts.Count > 0);
		}
		GameEvents.onTooltipDestroyRequested.Fire();
		Lock(lockLoad: true, lockExit: true, lockSave: true, "EditorLogic_loadDialog");
		InputLockManager.SetControlLock(ControlTypes.EDITOR_SOFT_LOCK, "EditorLogic_dialog_softLock");
		ScreenMessages.PostScreenMessage("", modeMsg);
	}

	public void onLoadConfirmDismiss()
	{
		InputLockManager.RemoveControlLock("LoadConfirmationDialog");
	}

	public static void LoadShipFromFile(string path)
	{
		if ((bool)fetch)
		{
			fetch.ShipToLoadSelected(path, CraftBrowserDialog.LoadType.Normal);
		}
	}

	public void ShipToLoadSelected(string path, CraftBrowserDialog.LoadType loadType)
	{
		InputLockManager.RemoveControlLock("EditorLogic_dialog_softLock");
		Unlock("EditorLogic_loadDialog");
		if (path == null)
		{
			return;
		}
		if ((bool)FlightGlobals.fetch)
		{
			FlightGlobals.PersistentVesselIds.Remove(ship.persistentId);
		}
		switch (loadType)
		{
		case CraftBrowserDialog.LoadType.Normal:
			EditorDriver.StartupBehaviour = EditorDriver.StartupBehaviours.LOAD_FROM_FILE;
			EditorDriver.filePathToLoad = path;
			StartEditor(isRestart: true);
			GameEvents.onEditorLoad.Fire(ship, loadType);
			break;
		case CraftBrowserDialog.LoadType.Merge:
		{
			ShipConstruct shipConstruct = new ShipConstruct();
			ConfigNode root = ConfigNode.Load(path);
			shipConstruct.LoadShip(root);
			if (shipConstruct.steamPublishedFileId == 0L)
			{
				shipConstruct.steamPublishedFileId = KSPSteamUtils.GetSteamIDFromSteamFolder(path);
			}
			SpawnConstruct(shipConstruct);
			GameEvents.onEditorLoad.Fire(shipConstruct, loadType);
			break;
		}
		}
	}

	public void ShipToLoadSelected(ConfigNode node, CraftBrowserDialog.LoadType loadType)
	{
		InputLockManager.RemoveControlLock("EditorLogic_dialog_softLock");
		Unlock("EditorLogic_loadDialog");
		if (node != null)
		{
			if ((bool)FlightGlobals.fetch)
			{
				FlightGlobals.PersistentVesselIds.Remove(ship.persistentId);
			}
			switch (loadType)
			{
			case CraftBrowserDialog.LoadType.Normal:
				EditorDriver.StartupBehaviour = EditorDriver.StartupBehaviours.LOAD_FROM_CACHE;
				ShipConstruction.ShipConfig = node;
				StartEditor(isRestart: true);
				GameEvents.onEditorLoad.Fire(ship, loadType);
				break;
			case CraftBrowserDialog.LoadType.Merge:
			{
				ShipConstruct shipConstruct = new ShipConstruct();
				shipConstruct.LoadShip(node);
				SpawnConstruct(shipConstruct);
				GameEvents.onEditorLoad.Fire(shipConstruct, loadType);
				break;
			}
			}
			onShipNameFieldValueChanged(ship.shipName);
			shipNameField.text = Localizer.Format(ship.shipName);
		}
	}

	public void CraftBrowseCancelled()
	{
		StartCoroutine(DelayedUnlock());
		Unlock("EditorLogic_loadDialog");
	}

	public void OnPartVesselNamingChanged(Part p)
	{
		ship.UpdateVesselNaming();
	}

	public void OnVesselNamingShipModified(ShipConstruct ship)
	{
		if (this.ship == ship)
		{
			ship.UpdateVesselNaming();
		}
	}

	public void launchVessel()
	{
		launchVessel(launchSiteName);
	}

	public void launchVessel(string siteName)
	{
		launchSiteName = siteName;
		if (HighLogic.CurrentGame.Mode != Game.Modes.MISSION && !EditorDriver.ValidLaunchSite(siteName))
		{
			EditorDriver.setDefaultLaunchSite();
			siteName = launchSiteName;
		}
		GameEvents.onTooltipDestroyRequested.Fire();
		ScreenMessages.PostScreenMessage("", modeMsg);
		FinalizeAnalytics();
		if (HighLogic.CurrentGame.Mode == Game.Modes.SCENARIO_NON_RESUMABLE)
		{
			HighLogic.LoadScene(GameScenes.MAINMENU);
			return;
		}
		ship.shipName = shipNameField.text;
		ship.shipDescription = shipDescriptionField.text;
		ship.missionFlag = FlagURL;
		ShipConstruction.CreateBackup(ship);
		if (HighLogic.CurrentGame.Mode == Game.Modes.MISSION && MissionsApp.Instance != null && MissionsApp.Instance.Mode != MissionAppMode.EditorFreeBuild)
		{
			VesselSituation vesselSituation = null;
			if (MissionsApp.Instance.CurrentVessel != null && MissionsApp.Instance.CurrentVessel.vesselSituation != null)
			{
				vesselSituation = MissionsApp.Instance.CurrentVessel.vesselSituation;
			}
			if (vesselSituation != null && vesselSituation.location.situation == MissionSituation.VesselStartSituations.ORBITING)
			{
				ShipConstruct shipOut = null;
				if (!MissionCheckLaunchClamps(ship, rootPart, vesselSituation, out shipOut))
				{
					return;
				}
			}
			GetMissionPreFlightCheck(siteName).RunTests();
		}
		else
		{
			Lock(lockLoad: true, lockExit: true, lockSave: true, "EditorLogic_launchSequence");
			string shipFilename = KSPUtil.SanitizeString(autoShipName, '_', replaceEmpty: true);
			savedCraftPath = ShipConstruction.SaveShip(shipFilename);
			InputLockManager.RemoveControlLock("Saving");
			VesselCrewManifest manifest = CrewAssignmentDialog.Instance.GetManifest();
			if (manifest != null)
			{
				ShipConstruction.ShipManifest = manifest;
				RefreshCrewAssignment(ShipConstruction.ShipConfig, partIsAttached);
			}
			else
			{
				ShipConstruction.ShipManifest = HighLogic.CurrentGame.CrewRoster.DefaultCrewForVessel(ShipConstruction.ShipConfig);
			}
			GetStockPreFlightCheck(siteName).RunTests();
			ShipConstruction.ClearBackups();
		}
	}

	public static bool MissionCheckLaunchClamps(ShipConstruct ship, Part rootPart, VesselSituation vesselSituation, out ShipConstruct shipOut, bool overrideSituationCheck = false)
	{
		shipOut = ship;
		if (ship != null && !(rootPart == null) && vesselSituation != null)
		{
			if (vesselSituation.location.situation != MissionSituation.VesselStartSituations.ORBITING && !overrideSituationCheck)
			{
				return true;
			}
			bool flag = false;
			List<Part> list = new List<Part>();
			int i = 0;
			for (int count = ship.parts.Count; i < count; i++)
			{
				Part part = ship.parts[i];
				if (part.name.Contains("launchClamp1"))
				{
					list.Add(part);
					if (part == rootPart)
					{
						flag = true;
					}
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				List<Part> list2 = EditorLogicBase.FindPartsInChildren(list[j]);
				int k = 0;
				for (int count2 = list2.Count; k < count2; k++)
				{
					EditorLogicBase.clearAttachNodes(list2[k], list2[k].parent);
					for (int num = list2[k].parent.children.Count - 1; num >= 0; num--)
					{
						if (list2[k].parent.children[num] == list2[k])
						{
							list2[k].parent.children.Remove(list2[k]);
						}
					}
					Debug.LogFormat("[EditorLogic]: Removed LaunchLamp from orbiting vessel: {0}", ship.shipName);
					ship.Remove(list2[k]);
					if (list2[k].gameObject != null)
					{
						list2[k].gameObject.DestroyGameObjectImmediate();
					}
				}
			}
			if (!flag && ship.parts.Count != 0)
			{
				string shipFilename = KSPUtil.SanitizeString(ship.shipName, '_', replaceEmpty: true);
				if (!string.IsNullOrEmpty(vesselSituation.vesselName))
				{
					shipFilename = KSPUtil.SanitizeString(vesselSituation.vesselName, '_', replaceEmpty: true);
				}
				if (HighLogic.CurrentGame != null && (HighLogic.CurrentGame.Mode == Game.Modes.MISSION || HighLogic.CurrentGame.Mode == Game.Modes.MISSION_BUILDER))
				{
					ShipConstruction.SaveShip(ship, shipFilename);
				}
				shipOut = ship;
				return true;
			}
			if (HighLogic.LoadedSceneIsEditor)
			{
				if (fetch != null)
				{
					fetch.RestoreState(-1);
				}
				PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("LaunchClampsOrbitShip", Localizer.Format("#autoLOC_8002096"), Localizer.Format("#autoLOC_8002097"), UISkinManager.GetSkin("KSP window 7"), 350f, new DialogGUIButton("<color=orange>" + Localizer.Format("#autoLOC_417274") + "</color>", delegate
				{
				})), persistAcrossScenes: false, UISkinManager.GetSkin("KSP window 7"));
			}
			return false;
		}
		return true;
	}

	public PreFlightCheck GetStockPreFlightCheck(string siteName)
	{
		PreFlightCheck preFlightCheck = new PreFlightCheck(proceedWithVesselLaunch, abortLaunch);
		switch (EditorDriver.editorFacility)
		{
		case EditorFacility.const_2:
			preFlightCheck.AddTest(new CraftWithinPartCountLimit(ship, SpaceCenterFacility.SpaceplaneHangar, GameVariables.Instance.GetPartCountLimit(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.SpaceplaneHangar), isVAB: false)));
			break;
		case EditorFacility.const_1:
			preFlightCheck.AddTest(new CraftWithinPartCountLimit(ship, SpaceCenterFacility.VehicleAssemblyBuilding, GameVariables.Instance.GetPartCountLimit(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.VehicleAssemblyBuilding), isVAB: true)));
			break;
		}
		switch (siteName)
		{
		case "LaunchPad":
		case "Woomerang_Launch_Site":
		case "Desert_Launch_Site":
			preFlightCheck.AddTest(new CraftWithinSizeLimits(ship, SpaceCenterFacility.LaunchPad, GameVariables.Instance.GetCraftSizeLimit(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.LaunchPad), isPad: true)));
			preFlightCheck.AddTest(new CraftWithinMassLimits(ship, SpaceCenterFacility.LaunchPad, GameVariables.Instance.GetCraftMassLimit(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.LaunchPad), isPad: true)));
			break;
		}
		switch (siteName)
		{
		case "Runway":
		case "Island_Airfield":
		case "Desert_Airfield":
			preFlightCheck.AddTest(new CraftWithinSizeLimits(ship, SpaceCenterFacility.Runway, GameVariables.Instance.GetCraftSizeLimit(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.Runway), isPad: false)));
			preFlightCheck.AddTest(new CraftWithinMassLimits(ship, SpaceCenterFacility.Runway, GameVariables.Instance.GetCraftMassLimit(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.Runway), isPad: false)));
			break;
		}
		preFlightCheck.AddTest(new ExperimentalPartsAvailable(ship));
		preFlightCheck.AddTest(new CanAffordLaunchTest(ship, Funding.Instance));
		if (PSystemSetup.Instance.GetSpaceCenterFacility(siteName) != null)
		{
			preFlightCheck.AddTest(new FacilityOperational(siteName, siteName));
		}
		preFlightCheck.AddTest(new NoControlSources(ShipConstruction.ShipManifest));
		if (string.IsNullOrEmpty(siteName))
		{
			launchSiteClearTest = null;
		}
		else
		{
			launchSiteClearTest = new LaunchSiteClear(siteName, PSystemSetup.Instance.GetLaunchSiteDisplayName(siteName));
			preFlightCheck.AddTest(launchSiteClearTest);
		}
		return preFlightCheck;
	}

	public PreFlightCheck GetMissionPreFlightCheck(string siteName)
	{
		PreFlightCheck preFlightCheck = new PreFlightCheck(proceedWithMissionLaunch, abortLaunch);
		VesselSituation vesselSituation = null;
		if (MissionsApp.Instance != null && MissionsApp.Instance.CurrentVessel != null && MissionsApp.Instance.CurrentVessel.vesselSituation != null)
		{
			vesselSituation = MissionsApp.Instance.CurrentVessel.vesselSituation;
		}
		if (vesselSituation == null)
		{
			preFlightCheck = GetStockPreFlightCheck(siteName);
			preFlightCheck.AddTest(new MissionBlackListPreFlightCheck(null, ship));
			return preFlightCheck;
		}
		preFlightCheck.AddTest(new MissionBlackListPreFlightCheck(vesselSituation, ship));
		List<VesselRestriction> activeRestrictions = vesselSituation.vesselRestrictionList.ActiveRestrictions;
		for (int i = 0; i < activeRestrictions.Count; i++)
		{
			preFlightCheck.AddTest(activeRestrictions[i].GetPreflightCheck());
		}
		SpaceCenterFacility facility = ((EditorDriver.editorFacility == EditorFacility.const_1) ? SpaceCenterFacility.VehicleAssemblyBuilding : SpaceCenterFacility.SpaceplaneHangar);
		preFlightCheck.AddTest(new CraftWithinPartCountLimit(ship, facility, GameVariables.Instance.GetPartCountLimit(ScenarioUpgradeableFacilities.GetFacilityLevel(facility), isVAB: true)));
		switch (siteName)
		{
		case "LaunchPad":
		case "Woomerang_Launch_Site":
		case "Desert_Launch_Site":
			preFlightCheck.AddTest(new CraftWithinSizeLimits(ship, SpaceCenterFacility.LaunchPad, GameVariables.Instance.GetCraftSizeLimit(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.LaunchPad), isPad: true)));
			preFlightCheck.AddTest(new CraftWithinMassLimits(ship, SpaceCenterFacility.LaunchPad, GameVariables.Instance.GetCraftMassLimit(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.LaunchPad), isPad: true)));
			break;
		}
		switch (siteName)
		{
		case "Runway":
		case "Island_Airfield":
		case "Desert_Airfield":
			preFlightCheck.AddTest(new CraftWithinSizeLimits(ship, SpaceCenterFacility.Runway, GameVariables.Instance.GetCraftSizeLimit(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.Runway), isPad: false)));
			preFlightCheck.AddTest(new CraftWithinMassLimits(ship, SpaceCenterFacility.Runway, GameVariables.Instance.GetCraftMassLimit(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.Runway), isPad: false)));
			break;
		}
		if (string.IsNullOrEmpty(siteName))
		{
			launchSiteClearTest = null;
		}
		else
		{
			launchSiteClearTest = new LaunchSiteClear(siteName, PSystemSetup.Instance.GetLaunchSiteDisplayName(siteName));
			preFlightCheck.AddTest(launchSiteClearTest);
		}
		preFlightCheck.AddTest(new NoControlSources(ShipConstruction.ShipManifest));
		return preFlightCheck;
	}

	public void processMissionAppChecks()
	{
		if (MissionSystem.Instance != null && MissionsApp.Instance != null)
		{
			VesselCrewManifest manifest = CrewAssignmentDialog.Instance.GetManifest();
			string craftFile = KSPUtil.SanitizeString(ship.shipName, '_', replaceEmpty: true) + ".craft";
			MissionsApp.Instance.EditorVesselCompleted(craftFile, manifest, delegate
			{
				GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE, GameScenes.EDITOR);
			});
		}
		else
		{
			Debug.LogError("[EditorLogic]: MissionSystem Instance not active");
		}
	}

	public void proceedWithMissionLaunch()
	{
		if (launchSiteClearTest != null)
		{
			List<ProtoVessel> obstructingVessels = launchSiteClearTest.GetObstructingVessels();
			int i = 0;
			for (int count = obstructingVessels.Count; i < count; i++)
			{
				ShipConstruction.RecoverVesselFromFlight(obstructingVessels[i], HighLogic.CurrentGame.flightState, quick: true);
			}
		}
		saveShip(processMissionAppChecks);
	}

	public void proceedWithVesselLaunch()
	{
		if (launchSiteClearTest != null)
		{
			List<ProtoVessel> obstructingVessels = launchSiteClearTest.GetObstructingVessels();
			int i = 0;
			for (int count = obstructingVessels.Count; i < count; i++)
			{
				ShipConstruction.RecoverVesselFromFlight(obstructingVessels[i], HighLogic.CurrentGame.flightState);
			}
		}
		goForLaunch();
	}

	public void onMissionDialogUp(MissionRecoveryDialog dialog)
	{
		missionDialogs.Add(dialog);
	}

	public void onMissionDialogDismiss(MissionRecoveryDialog dialog)
	{
		missionDialogs.Remove(dialog);
		if (missionDialogs.Count == 0)
		{
			goForLaunch();
		}
	}

	public void goForLaunch()
	{
		if (missionDialogs.Count <= 0)
		{
			EditorDriver.saveselectedLaunchSite();
			StartCoroutine(CallbackUtil.DelayedCallback(1, delegate
			{
				Unlock("EditorLogic_launchSequence");
				EditorActionGroups.Instance.DeactivateInterface(ship);
				FlightDriver.StartWithNewLaunch(savedCraftPath, FlagURL, launchSiteName, ShipConstruction.ShipManifest);
			}));
		}
	}

	public void abortLaunch()
	{
		StartCoroutine(DelayedAbort());
	}

	public IEnumerator DelayedAbort()
	{
		yield return null;
		Unlock("EditorLogic_launchSequence");
	}

	public void ResetCrewAssignment(ConfigNode craftNode, bool allowAutoHire)
	{
		if (!(CrewAssignmentDialog.Instance == null))
		{
			ShipConstruction.ShipManifest = HighLogic.CurrentGame.CrewRoster.DefaultCrewForVessel(craftNode, null, allowAutoHire);
			CrewAssignmentDialog.Instance.RefreshCrewLists(ShipConstruction.ShipManifest, setAsDefault: true, updateUI: false);
			GameEvents.onEditorShipCrewModified.Fire(ShipConstruction.ShipManifest);
		}
	}

	public void RefreshCrewAssignment(ConfigNode craftNode, Func<PartCrewManifest, bool> persistFilter)
	{
		if (!(CrewAssignmentDialog.Instance == null))
		{
			ShipConstruction.ShipManifest = ShipConstruction.ShipManifest.UpdateCrewForVessel(craftNode, persistFilter);
			CrewAssignmentDialog.Instance.RefreshCrewLists(ShipConstruction.ShipManifest, setAsDefault: false, updateUI: false);
			GameEvents.onEditorShipCrewModified.Fire(ShipConstruction.ShipManifest);
		}
	}

	public void RefreshCrewDialog()
	{
		if (!(CrewAssignmentDialog.Instance == null))
		{
			VesselCrewManifest.MergeInto(ShipConstruction.ShipManifest, CrewAssignmentDialog.Instance.GetManifest(), GetPartExistsFilter());
			CrewAssignmentDialog.Instance.RefreshCrewLists(ShipConstruction.ShipManifest, setAsDefault: false, updateUI: true, partIsAttached);
			GameEvents.onEditorShipCrewModified.Fire(ShipConstruction.ShipManifest);
		}
	}

	public void SpawningAC()
	{
		if (RoboticControllerManager.Instance != null)
		{
			RoboticControllerManager.Instance.CloseAllWindows();
		}
	}

	public bool partIsAttached(PartCrewManifest pcm)
	{
		int count = ship.parts.Count;
		do
		{
			if (count-- <= 0)
			{
				return false;
			}
		}
		while (ship.parts[count].craftID != pcm.PartID);
		return true;
	}

	public Func<PartCrewManifest, bool> GetPartExistsFilter()
	{
		return partExists;
	}

	public bool partExists(PartCrewManifest pcm)
	{
		int count = Part.allParts.Count;
		do
		{
			if (count-- <= 0)
			{
				return false;
			}
		}
		while (Part.allParts[count].craftID != pcm.PartID);
		return true;
	}

	public void UpdateCrewAssignment()
	{
		VesselCrewManifest.MergeInto(ShipConstruction.ShipManifest, CrewAssignmentDialog.Instance.GetManifest(), GetPartExistsFilter());
		CrewAssignmentDialog.Instance.RefreshCrewLists(ShipConstruction.ShipManifest, setAsDefault: false, updateUI: false);
		GameEvents.onEditorShipCrewModified.Fire(ShipConstruction.ShipManifest);
	}

	public void UpdateCrewManifest()
	{
		VesselCrewManifest.MergeInto(ShipConstruction.ShipManifest, CrewAssignmentDialog.Instance.GetManifest(), GetPartExistsFilter());
		GameEvents.onEditorShipCrewModified.Fire(ShipConstruction.ShipManifest);
	}

	public IEnumerator DelayedUnlock()
	{
		yield return null;
		yield return null;
		InputLockManager.RemoveControlLock("EditorLogic_dialog_softLock");
	}

	public void OnInputLocksModified(GameEvents.FromToAction<ControlTypes, ControlTypes> ct)
	{
		UpdateUI();
	}

	public void UpdateUI()
	{
		loadBtn.interactable = InputLockManager.IsUnlocked(ControlTypes.EDITOR_LOAD);
		saveBtn.interactable = InputLockManager.IsUnlocked(ControlTypes.EDITOR_SAVE);
		steamBtn.interactable = InputLockManager.IsUnlocked(ControlTypes.EDITOR_SAVE);
		newBtn.interactable = InputLockManager.IsUnlocked(ControlTypes.EDITOR_NEW);
		launchBtn.interactable = InputLockManager.IsUnlocked(ControlTypes.EDITOR_LAUNCH);
		if (launchSiteSelector != null)
		{
			launchSiteSelector.SetActive(InputLockManager.IsUnlocked(ControlTypes.EDITOR_LAUNCH));
		}
		exitBtn.interactable = InputLockManager.IsUnlocked(ControlTypes.EDITOR_EXIT);
		symmetryButton.Interactable = InputLockManager.IsUnlocked(ControlTypes.EDITOR_SYM_SNAP_UI);
		angleSnapButton.Interactable = InputLockManager.IsUnlocked(ControlTypes.EDITOR_SYM_SNAP_UI);
		actionPanelBtn.interactable = InputLockManager.IsUnlocked(ControlTypes.EDITOR_MODE_SWITCH) && editorScreen != EditorScreen.Actions && !panelButtonsLocked;
		partPanelBtn.interactable = InputLockManager.IsUnlocked(ControlTypes.EDITOR_MODE_SWITCH) && editorScreen != 0 && !panelButtonsLocked;
		crewPanelBtn.interactable = InputLockManager.IsUnlocked(ControlTypes.EDITOR_MODE_SWITCH | ControlTypes.EDITOR_CREW) && editorScreen != EditorScreen.Crew && !panelButtonsLocked;
		cargoPanelBtn.interactable = InputLockManager.IsUnlocked(ControlTypes.EDITOR_MODE_SWITCH) && editorScreen != EditorScreen.Cargo && !panelButtonsLocked;
		switchEditorBtn.interactable = !panelButtonsLocked;
		flagBrowserButton.button.interactable = InputLockManager.IsUnlocked(ControlTypes.EDITOR_EDIT_NAME_FIELDS);
		shipNameField.interactable = InputLockManager.IsUnlocked(ControlTypes.EDITOR_EDIT_NAME_FIELDS);
		shipDescriptionField.interactable = InputLockManager.IsUnlocked(ControlTypes.EDITOR_EDIT_NAME_FIELDS);
	}

	public void Lock(bool lockLoad, bool lockExit, bool lockSave, string lockID)
	{
		ControlTypes controlTypes = ControlTypes.EDITOR_LOCK | ControlTypes.EDITOR_NEW | ControlTypes.EDITOR_LAUNCH | ControlTypes.EDITOR_EDIT_STAGES;
		if (lockLoad)
		{
			controlTypes |= ControlTypes.EDITOR_LOAD;
		}
		if (lockExit)
		{
			controlTypes |= ControlTypes.EDITOR_EXIT;
		}
		if (lockSave)
		{
			controlTypes |= ControlTypes.EDITOR_SAVE;
		}
		InputLockManager.SetControlLock(controlTypes, lockID);
	}

	public void Unlock(string lockID)
	{
		InputLockManager.RemoveControlLock(lockID);
	}

	public bool NameOrDescriptionFocused()
	{
		if (!shipNameField.isFocused)
		{
			return shipDescriptionField.isFocused;
		}
		return true;
	}

	public bool AnyTextFieldHasFocus()
	{
		if (!shipNameField.isFocused && !shipDescriptionField.isFocused)
		{
			return PartCategorizer.Instance.searchField.isFocused;
		}
		return true;
	}

	public bool mouseOverModalArea(Collider[] areas)
	{
		int num = 0;
		int num2 = areas.Length;
		while (true)
		{
			if (num < num2)
			{
				if (areas[num].bounds.IntersectRay(UIMasterController.Instance.uiCamera.ScreenPointToRay(Input.mousePosition)))
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

	public void WipeTooltips()
	{
		UIMasterController.Instance.DestroyCurrentTooltip();
	}

	public void partSearchUpdate()
	{
		if (GameSettings.Editor_partSearch.GetKeyDown() && !NameOrDescriptionFocused() && !DeltaVApp.AnyTextFieldHasFocus() && !RoboticControllerManager.AnyWindowTextFieldHasFocus())
		{
			if (craftBrowserDialog == null && PartCategorizer.Instance != null)
			{
				PartCategorizer.Instance.FocusSearchField();
			}
		}
		else if (Input.GetKeyUp(KeyCode.Escape) && craftBrowserDialog == null && PartCategorizer.Instance != null)
		{
			PartCategorizer.Instance.searchField.text = string.Empty;
		}
	}

	public List<Part> getSortedShipList()
	{
		List<Part> list = new List<Part>();
		recurseShipList(rootPart, list);
		return list;
	}

	public void recurseShipList(Part part, List<Part> shipList)
	{
		if (!(part == null))
		{
			shipList.Add(part);
			int i = 0;
			for (int count = part.children.Count; i < count; i++)
			{
				Part part2 = part.children[i];
				recurseShipList(part2, shipList);
			}
		}
	}

	public void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(editorBounds.center, editorBounds.size);
	}

	public bool AreAllPartsConnected()
	{
		int count = Part.allParts.Count;
		do
		{
			if (count-- <= 0)
			{
				return true;
			}
		}
		while (ship.Contains(Part.allParts[count]));
		return false;
	}

	public int CountAllSceneParts(bool includeSelected)
	{
		if (selectedPart != null)
		{
			return Part.allParts.Count - 1;
		}
		return Part.allParts.Count;
	}

	public bool ShipIsValid(ShipConstruct ship)
	{
		if (ship != null)
		{
			return ship.parts.Count > 0;
		}
		return false;
	}

	public bool HeldPartIsStacked()
	{
		bool result = false;
		if (UIPartActionControllerInventory.Instance == null)
		{
			return result;
		}
		if (UIPartActionControllerInventory.Instance.CurrentCargoPart != null && (UIPartActionControllerInventory.heldPartIsStack || UIPartActionControllerInventory.stackSize > 1))
		{
			result = true;
		}
		return result;
	}

	public void InitAnalytics()
	{
		timeInMode = new Dictionary<string, double>();
		startTime = HighLogic.gameRunTimeAdditive;
		modeStartTime = HighLogic.gameRunTimeAdditive;
		currentMode = EditorScreen.Parts;
		GameEvents.onEditorScreenChange.Add(OnEditorScreenChange);
	}

	public void FinalizeAnalytics()
	{
		GameEvents.onEditorScreenChange.Remove(OnEditorScreenChange);
		string key = currentMode.ToString();
		if (!timeInMode.ContainsKey(key))
		{
			timeInMode.Add(key, 0.0);
		}
		timeInMode[key] += HighLogic.gameRunTimeAdditive - modeStartTime;
		AnalyticsUtil.LogExitEditor(HighLogic.gameRunTimeAdditive - startTime, timeInMode);
	}

	public void OnEditorScreenChange(EditorScreen screen)
	{
		OnModeChange(screen);
	}

	public void OnModeChange(EditorScreen mode)
	{
		string key = currentMode.ToString();
		if (!timeInMode.ContainsKey(key))
		{
			timeInMode.Add(key, 0.0);
		}
		timeInMode[key] += HighLogic.gameRunTimeAdditive - modeStartTime;
		modeStartTime = HighLogic.gameRunTimeAdditive;
		currentMode = mode;
	}
}
