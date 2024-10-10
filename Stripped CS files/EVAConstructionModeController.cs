using System;
using System.Collections.Generic;
using ns11;
using ns12;
using ns2;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EVAConstructionModeController : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public class InventoryDisplayItem
	{
		public ModuleInventoryPart inventoryModule;

		public GameObject uiObject;

		public UIPartActionInventory uiInventory;
	}

	public enum PanelMode
	{
		Construction,
		Cargo
	}

	public static EVAConstructionModeController Instance;

	public EVAConstructionModeEditor evaEditor;

	public EVAConstructionToolsUI evaToolsUI;

	[SerializeField]
	public LayerMask markerCamCullingMask;

	public Camera markerCam;

	[SerializeField]
	public UIPanelTransition constructionModeTransition;

	[SerializeField]
	public UIPanelTransition navballTransition;

	public bool navballPreviouslyOpen;

	public bool isOpen;

	public KerbalEVA activeEVA;

	[SerializeField]
	public GameObject uiPartActionInventoryContainerPrefab;

	[SerializeField]
	public GameObject uiPartActionInventoryParent;

	public Dictionary<uint, ModuleInventoryPart> loadedModuleInventoryPart;

	public List<InventoryDisplayItem> displayedInventories;

	[SerializeField]
	public RectTransform partList;

	public Vector2 partListVector;

	[SerializeField]
	public RectTransform footerConstruction;

	[SerializeField]
	public RectTransform footerCargo;

	public bool hover;

	public ApplicationLauncherButton applauncherConstruction;

	public ApplicationLauncherButton applauncherCargo;

	[SerializeField]
	public Button exitButton;

	[SerializeField]
	public TooltipController_Text exitTooltipText;

	[SerializeField]
	public RectTransform exitButtonOriginalPos;

	[SerializeField]
	public PanelMode panelMode;

	public TextMeshProUGUI AssistingKerbalsLabel;

	public TextMeshProUGUI AssistingKerbalsNumber;

	public TextMeshProUGUI MaxMassLimitLabel;

	public TextMeshProUGUI MaxMassLimitNumber;

	public double constructionGravity;

	public double lastConstructionGravity;

	public InventoryDisplayItem displayItem;

	public Vector3 maxVector = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

	public bool IsOpen => isOpen;

	public bool Hover => hover;

	public static bool MovementRestricted
	{
		get
		{
			if (Instance != null && Instance.evaEditor != null)
			{
				if (Instance.IsOpen)
				{
					return Instance.evaEditor.SelectedPart != null;
				}
				return false;
			}
			return false;
		}
	}

	public void Awake()
	{
		if ((bool)Instance)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		Instance = this;
		loadedModuleInventoryPart = new Dictionary<uint, ModuleInventoryPart>();
		displayedInventories = new List<InventoryDisplayItem>();
		GameEvents.onLevelWasLoaded.Add(OnLoadedScene);
	}

	public void Start()
	{
		if (footerConstruction != null)
		{
			footerConstruction.anchoredPosition = new Vector2(0f, 0f);
		}
		GameEvents.onGameSceneLoadRequested.Add(OnGameSceneLoadRequested);
		GameEvents.onVesselChange.Add(OnVesselChange);
		GameEvents.onEditorPartEvent.Add(OnEditorPartEvent);
		GameEvents.OnMapExited.Add(SetAppLauncherButtonVisibility);
		GameEvents.onGUIActionGroupFlightShowing.Add(OnActionGroupsOpened);
		GameEvents.OnCombinedConstructionWeightLimitChanged.Add(UpdateInfoLabels);
		exitButton.onClick.AddListener(ClosePanel);
		footerCargo.gameObject.SetActive(value: false);
		partListVector = partList.offsetMin;
		SetAppLauncherButtonVisibility();
		AssistingKerbalsLabel.transform.parent.gameObject.SetActive(GameSettings.EVA_CONSTRUCTION_COMBINE_ENABLED);
	}

	public void OnDestroy()
	{
		GameEvents.onGameSceneLoadRequested.Remove(OnGameSceneLoadRequested);
		GameEvents.onVesselChange.Remove(OnVesselChange);
		GameEvents.onEditorPartEvent.Remove(OnEditorPartEvent);
		GameEvents.OnMapExited.Remove(SetAppLauncherButtonVisibility);
		GameEvents.onLevelWasLoaded.Remove(OnLoadedScene);
		GameEvents.onGUIActionGroupFlightShowing.Remove(OnActionGroupsOpened);
		GameEvents.OnCombinedConstructionWeightLimitChanged.Remove(UpdateInfoLabels);
		if (markerCam != null && markerCam.gameObject != null)
		{
			UnityEngine.Object.Destroy(markerCam.gameObject);
		}
		exitButton.onClick.RemoveListener(ClosePanel);
	}

	public void SetAppLauncherButtonVisibility()
	{
		if (applauncherConstruction != null)
		{
			applauncherConstruction.VisibleInScenes = (CanOpenConstructionPanel() ? ApplicationLauncher.AppScenes.FLIGHT : ApplicationLauncher.AppScenes.NEVER);
		}
		if (applauncherCargo != null)
		{
			applauncherCargo.VisibleInScenes = (CanOpenCargoPanel() ? ApplicationLauncher.AppScenes.FLIGHT : ApplicationLauncher.AppScenes.NEVER);
		}
	}

	public void Update()
	{
		if (GameSettings.EVA_CONSTRUCTION_MODE_TOGGLE.GetKeyUp())
		{
			if (IsOpen)
			{
				ClosePanel();
			}
			else
			{
				OpenConstructionPanel();
			}
		}
		if (!isOpen)
		{
			return;
		}
		SearchForInventoryParts();
		UpdateDisplayedInventories();
		if (panelMode == PanelMode.Construction)
		{
			constructionGravity = EVAConstructionUtil.GetConstructionGee(FlightGlobals.ActiveVessel);
			if (Math.Abs(lastConstructionGravity - constructionGravity) > 0.001)
			{
				UpdateInfoLabels();
			}
			lastConstructionGravity = constructionGravity;
		}
	}

	public void RegisterAppButtonConstruction(ApplicationLauncherButton button)
	{
		applauncherConstruction = button;
		SetAppLauncherButtonVisibility();
	}

	public void RegisterAppButtonCargo(ApplicationLauncherButton button)
	{
		applauncherCargo = button;
		SetAppLauncherButtonVisibility();
	}

	public void OpenConstructionPanel()
	{
		if (CanOpenConstructionPanel() && !IsOpen)
		{
			activeEVA = FlightGlobals.ActiveVessel.evaController;
			if (activeEVA != null)
			{
				activeEVA.EnterConstructionMode();
			}
			navballPreviouslyOpen = navballTransition.State == "In";
			if (navballPreviouslyOpen)
			{
				navballTransition.Transition("Out");
			}
			InputLockManager.SetControlLock(ControlTypes.MAP_TOGGLE, "EVAConstructionModeController");
			constructionModeTransition.Transition("In");
			exitButton.transform.position = exitButtonOriginalPos.position;
			evaToolsUI.ShowModeTools();
			exitTooltipText.textString = Localizer.Format("#autoLOC_8003410");
			footerCargo.gameObject.SetActive(value: false);
			footerConstruction.gameObject.SetActive(value: true);
			partListVector.y = footerConstruction.offsetMax.y;
			partList.offsetMin = partListVector;
			panelMode = PanelMode.Construction;
			isOpen = true;
			if (InputLockManager.IsLocked(ControlTypes.EDITOR_SOFT_LOCK))
			{
				InputLockManager.RemoveControlLock("EVACompoundPart_Placement");
			}
			if ((bool)FlightCamera.fetch)
			{
				FlightCamera.fetch.DisableCameraHighlighter();
			}
			SearchForInventoryParts();
			UpdateInfoLabels();
			if (applauncherConstruction != null)
			{
				applauncherConstruction.SetTrue(makeCall: false);
			}
			GameEvents.OnEVAConstructionMode.Fire(data: true);
		}
	}

	public void OpenCargoPanel()
	{
		if (CanOpenCargoPanel() && !IsOpen)
		{
			StageManager.ShowHideStageStack(state: false);
			InputLockManager.SetControlLock(ControlTypes.MAP_TOGGLE, "EVAConstructionModeController");
			constructionModeTransition.Transition("In");
			exitButton.transform.position = evaToolsUI.placeButton.transform.position;
			evaToolsUI.HideModeTools();
			exitTooltipText.textString = Localizer.Format("#autoLOC_8003419");
			footerCargo.gameObject.SetActive(value: true);
			footerConstruction.gameObject.SetActive(value: false);
			partListVector.y = footerCargo.offsetMax.y;
			partList.offsetMin = partListVector;
			panelMode = PanelMode.Cargo;
			isOpen = true;
			SearchForInventoryParts();
			if (applauncherCargo != null)
			{
				applauncherCargo.SetTrue(makeCall: false);
			}
			GameEvents.OnEVACargoMode.Fire(data: true);
		}
	}

	public void ClosePanel()
	{
		if (!IsOpen)
		{
			return;
		}
		evaEditor.ForceDrop();
		if (activeEVA != null)
		{
			activeEVA.ExitConstructionMode();
		}
		if (panelMode == PanelMode.Construction && navballPreviouslyOpen)
		{
			navballTransition.Transition("In");
		}
		if (constructionModeTransition != null)
		{
			constructionModeTransition.Transition("Out");
		}
		InputLockManager.RemoveControlLock("EVAConstructionModeController");
		isOpen = false;
		if (panelMode == PanelMode.Construction && (bool)FlightCamera.fetch)
		{
			FlightCamera.fetch.CycleCameraHighlighter();
		}
		loadedModuleInventoryPart.Clear();
		int count = displayedInventories.Count;
		while (count-- > 0)
		{
			if (displayedInventories[count].uiObject != null)
			{
				UnityEngine.Object.Destroy(displayedInventories[count].uiObject);
			}
		}
		displayedInventories.Clear();
		if (panelMode == PanelMode.Construction && applauncherConstruction != null)
		{
			applauncherConstruction.SetFalse();
		}
		if (panelMode == PanelMode.Cargo && applauncherCargo != null)
		{
			applauncherCargo.SetFalse();
		}
		if (panelMode == PanelMode.Construction)
		{
			GameEvents.OnEVAConstructionMode.Fire(data: false);
		}
		if (panelMode == PanelMode.Cargo)
		{
			StageManager.ShowHideStageStack(state: true);
			GameEvents.OnEVACargoMode.Fire(data: false);
		}
	}

	public bool CanOpenConstructionPanel()
	{
		if (HighLogic.LoadedSceneIsFlight && !MapView.MapIsEnabled && !(FlightGlobals.ActiveVessel == null) && (!(FlightGlobals.ActiveVessel != null) || FlightGlobals.ActiveVessel.isEVA) && (!(FlightGlobals.ActiveVessel != null) || !FlightGlobals.ActiveVessel.isEVA || FlightGlobals.ActiveVessel.VesselValues.RepairSkill.value >= 0) && !FlightDriver.Pause && !ActionGroupsFlightController.Instance.IsOpen)
		{
			return true;
		}
		return false;
	}

	public bool CanOpenCargoPanel()
	{
		if (HighLogic.LoadedSceneIsFlight && !MapView.MapIsEnabled && !(FlightGlobals.ActiveVessel == null) && !CanOpenConstructionPanel())
		{
			return true;
		}
		return false;
	}

	public void OnGameSceneLoadRequested(GameScenes scene)
	{
		if (isOpen)
		{
			ClosePanel();
		}
		SetAppLauncherButtonVisibility();
	}

	public void OnVesselChange(Vessel vessel)
	{
		if ((bool)activeEVA && activeEVA.InConstructionMode)
		{
			activeEVA.ExitConstructionMode();
		}
		if (isOpen)
		{
			if (!CanOpenConstructionPanel())
			{
				ClosePanel();
				activeEVA = null;
			}
			else
			{
				activeEVA = vessel.evaController;
				if ((bool)activeEVA)
				{
					activeEVA.EnterConstructionMode();
				}
			}
		}
		SetAppLauncherButtonVisibility();
	}

	public void OnEditorPartEvent(ConstructionEventType eventType, Part p)
	{
		switch (eventType)
		{
		case ConstructionEventType.PartPicked:
		{
			RemoveInvDisplayItem(p);
			for (int j = 0; j < displayedInventories.Count; j++)
			{
				displayedInventories[j].uiInventory.SetAllSlotsNotSelected();
			}
			break;
		}
		case ConstructionEventType.PartDragging:
			break;
		case ConstructionEventType.PartAttached:
		{
			for (int k = 0; k < displayedInventories.Count; k++)
			{
				displayedInventories[k].uiInventory.SetAllSlotsNotSelected();
			}
			break;
		}
		case ConstructionEventType.PartDropped:
		case ConstructionEventType.PartDetached:
		{
			RemoveInvDisplayItem(p);
			for (int i = 0; i < displayedInventories.Count; i++)
			{
				displayedInventories[i].uiInventory.SetAllSlotsNotSelected();
			}
			break;
		}
		}
	}

	public void RemoveInvDisplayItem(Part p)
	{
		if (!(p != null) || !loadedModuleInventoryPart.ContainsKey(p.persistentId))
		{
			return;
		}
		if (TryGetDisplayedInventory(loadedModuleInventoryPart[p.persistentId], out var item))
		{
			int num = displayedInventories.IndexOf(item);
			if (num >= 0)
			{
				UnityEngine.Object.Destroy(displayedInventories[num].uiObject);
				displayedInventories.RemoveAt(num);
			}
		}
		UnityEngine.Object.Destroy(loadedModuleInventoryPart[p.persistentId]);
		loadedModuleInventoryPart.Remove(p.persistentId);
	}

	public void OnActionGroupsOpened()
	{
		ClosePanel();
	}

	public void SearchForInventoryParts()
	{
		List<Vessel> vesselsLoaded = FlightGlobals.VesselsLoaded;
		for (int i = 0; i < vesselsLoaded.Count; i++)
		{
			Vessel vessel = vesselsLoaded[i];
			List<Part> parts = vessel.parts;
			for (int j = 0; j < parts.Count; j++)
			{
				ModuleInventoryPart moduleInventoryPart = parts[j].FindModuleImplementing<ModuleInventoryPart>();
				bool flag = parts[j].isKerbalEVA() && parts.Count > 1;
				if (moduleInventoryPart != null && !flag && !loadedModuleInventoryPart.ContainsKey(parts[j].persistentId))
				{
					loadedModuleInventoryPart.Add(parts[j].persistentId, moduleInventoryPart);
				}
				if (vessel.isEVA || parts[j].protoModuleCrew == null)
				{
					continue;
				}
				for (int k = 0; k < parts[j].protoModuleCrew.Count; k++)
				{
					ModuleInventoryPart kerbalInventoryModule = parts[j].protoModuleCrew[k].KerbalInventoryModule;
					if (kerbalInventoryModule != null)
					{
						if (!loadedModuleInventoryPart.ContainsKey(parts[j].protoModuleCrew[k].persistentID))
						{
							kerbalInventoryModule.transform.position = parts[j].transform.position;
							loadedModuleInventoryPart.Add(parts[j].protoModuleCrew[k].persistentID, kerbalInventoryModule);
						}
						else
						{
							kerbalInventoryModule.transform.position = parts[j].transform.position;
						}
					}
				}
			}
		}
	}

	public void UpdateDisplayedInventories()
	{
		Vector3 position = maxVector;
		if (FlightGlobals.ActiveVessel != null)
		{
			position = FlightGlobals.ActiveVessel.transform.position;
		}
		Dictionary<uint, ModuleInventoryPart>.KeyCollection.Enumerator enumerator = loadedModuleInventoryPart.Keys.GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModuleInventoryPart moduleInventoryPart = loadedModuleInventoryPart[enumerator.Current];
			if (moduleInventoryPart == null)
			{
				loadedModuleInventoryPart.Remove(enumerator.Current);
			}
			float num = 0f;
			num = ((!moduleInventoryPart.kerbalMode) ? Vector3.Distance(moduleInventoryPart.part.transform.position, position) : Vector3.Distance(moduleInventoryPart.transform.position, position));
			if (!TryGetDisplayedInventory(moduleInventoryPart, out displayItem))
			{
				if (!(GameSettings.EVA_INVENTORY_RANGE > num))
				{
					continue;
				}
				displayItem = new InventoryDisplayItem();
				displayItem.inventoryModule = moduleInventoryPart;
				displayItem.uiObject = UnityEngine.Object.Instantiate(uiPartActionInventoryContainerPrefab);
				if (displayItem.uiObject != null)
				{
					displayItem.uiObject.transform.SetParent(uiPartActionInventoryParent.transform);
					displayItem.uiObject.transform.localPosition = Vector3.zero;
					displayItem.uiObject.transform.localScale = Vector3.one;
					displayItem.uiInventory = displayItem.uiObject.GetComponentInChildren<UIPartActionInventory>(includeInactive: true);
					if (displayItem.uiInventory != null)
					{
						displayItem.uiInventory.SetupConstruction(moduleInventoryPart);
					}
				}
				displayedInventories.Add(displayItem);
			}
			else if (GameSettings.EVA_INVENTORY_RANGE < num)
			{
				UnityEngine.Object.Destroy(displayItem.uiObject);
				displayedInventories.Remove(displayItem);
			}
		}
		enumerator.Dispose();
	}

	public void UpdateInfoLabels()
	{
		AssistingKerbalsLabel.text = Localizer.Format(GameSettings.EVA_CONSTRUCTION_COMBINE_NONENGINEERS ? "#autoLOC_8014163" : "#autoLOC_8014164");
		AssistingKerbalsNumber.text = evaEditor.AssistingKerbals.ToString();
		double val = constructionGravity;
		val = Math.Max(val, 1E-06);
		double num = evaEditor.CombinedConstructionWeightLimit / val;
		if (num < 100000.0)
		{
			MaxMassLimitNumber.text = StringBuilderCache.Format("{0:F2}t", (float)num * 0.001f);
		}
		else
		{
			MaxMassLimitNumber.text = Localizer.Format("#autoLOC_8014166");
		}
	}

	public void DestroyHeldIcons(UIPartActionInventory callingInventory)
	{
		for (int i = 0; i < displayedInventories.Count; i++)
		{
			if (displayedInventories[i].uiInventory != callingInventory)
			{
				displayedInventories[i].uiInventory.DestroyHeldPart(fromConstructionController: true);
			}
		}
	}

	public bool TryGetDisplayedInventory(ModuleInventoryPart inventoryModule, out InventoryDisplayItem displayItem)
	{
		int num = 0;
		while (true)
		{
			if (num < displayedInventories.Count)
			{
				if (displayedInventories[num].inventoryModule == inventoryModule)
				{
					break;
				}
				num++;
				continue;
			}
			displayItem = null;
			return false;
		}
		displayItem = displayedInventories[num];
		return true;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		hover = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		hover = false;
	}

	public void SpawnMarkerCamera()
	{
		markerCam = new GameObject("markerCam").AddComponent<Camera>();
		markerCam.cullingMask = markerCamCullingMask;
		markerCam.orthographic = false;
		markerCam.nearClipPlane = 0.3f;
		markerCam.farClipPlane = 1000f;
		markerCam.depth = 1f;
		markerCam.fieldOfView = 60f;
		markerCam.clearFlags = CameraClearFlags.Depth;
		markerCam.usePhysicalProperties = false;
		markerCam.useOcclusionCulling = true;
		markerCam.allowHDR = false;
		markerCam.transform.SetParent(Camera.main.transform);
		markerCam.transform.localPosition = Vector3.zero;
		markerCam.transform.localRotation = Quaternion.identity;
	}

	public void OnLoadedScene(GameScenes loadedScene)
	{
		if (loadedScene == GameScenes.FLIGHT)
		{
			SpawnMarkerCamera();
		}
		SetAppLauncherButtonVisibility();
	}
}
