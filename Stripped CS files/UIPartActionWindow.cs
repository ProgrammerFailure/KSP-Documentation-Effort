using System;
using System.Collections.Generic;
using Expansions;
using ns2;
using ns3;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPartActionWindow : MonoBehaviour, IDragHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler
{
	public enum DisplayType
	{
		Selected,
		ResourceOnly,
		ResourceSelected
	}

	public struct ListItemCache
	{
		public UIPartActionFieldItem listFieldItem;

		public UIPartActionButton listButton;

		public UIPartActionResource listResource;

		public UIPartActionResourceEditor listResourceEditor;

		public UIPartActionResourceTransfer listResourceTransfer;

		public UIPartActionResourcePriority listResourcePriority;

		public UIPartActionAeroDisplay listAeroDisplay;

		public UIPartActionThermalDisplay listThermalDisplay;

		public UIPartActionRoboticJointDisplay listRoboticJointDisplay;

		public UIPartActionFuelFlowOverlay listFuelFlowOverlay;

		public ListItemCache(UIPartActionItem item)
		{
			listFieldItem = item as UIPartActionFieldItem;
			listButton = item as UIPartActionButton;
			listResource = item as UIPartActionResource;
			listResourceEditor = item as UIPartActionResourceEditor;
			listResourceTransfer = item as UIPartActionResourceTransfer;
			listResourcePriority = item as UIPartActionResourcePriority;
			listAeroDisplay = item as UIPartActionAeroDisplay;
			listThermalDisplay = item as UIPartActionThermalDisplay;
			listRoboticJointDisplay = item as UIPartActionRoboticJointDisplay;
			listFuelFlowOverlay = item as UIPartActionFuelFlowOverlay;
		}

		public void ClearReferences()
		{
			listFieldItem = null;
			listButton = null;
			listResource = null;
			listResourceEditor = null;
			listResourceTransfer = null;
			listResourcePriority = null;
			listAeroDisplay = null;
			listThermalDisplay = null;
			listRoboticJointDisplay = null;
			listFuelFlowOverlay = null;
		}
	}

	public LayerMask partLayerMask;

	public float zOffset;

	public RectTransform titleBar;

	public TextMeshProUGUI titleText;

	public VerticalLayoutGroup layoutGroup;

	public Toggle toggleNumeric;

	public Toggle togglePinned;

	public Material lineMaterial;

	public Color lineColor;

	public float lineCornerRadius = 10f;

	public float lineWidth = 5f;

	public bool _dragging;

	[SerializeField]
	public float screenEdgeOffset = 20f;

	public Vector2 screenEdgeOffsetVector;

	[SerializeField]
	public RectTransform windowTransform;

	public Vector2 windowDimensions;

	[SerializeField]
	public RectTransform titleBarTransform;

	[SerializeField]
	public RectTransform itemsContentTransform;

	public Vector2 itemsContentDimensions;

	public Vector2 contentAnchoredDimensions;

	[SerializeField]
	public ScrollRect itemsScrollRect;

	[SerializeField]
	public Scrollbar itemsScrollBar;

	[SerializeField]
	public float windowPixelMargin = 60f;

	public float windowHeight;

	public float maxWindowHeight;

	public float scrollBarWidth;

	public int Seed;

	public KSPRandom Generator;

	public Part _part;

	[SerializeField]
	public bool _isValid;

	[SerializeField]
	public bool _displayDirty;

	[SerializeField]
	public DisplayType displayType;

	public List<ListItemCache> listItemCache = new List<ListItemCache>();

	public List<int> listItemPositions = new List<int>();

	public List<UIPartActionItem> listItems = new List<UIPartActionItem>();

	public UI_Scene scene;

	public RectTransform rectTransform;

	public float previousUiScale;

	public bool PreviousResourceOnly;

	public bool usingNumericValue;

	public DictionaryValueList<string, UIPartActionGroup> parameterGroups;

	public Vector3 rootPartScreenPosition;

	public Vector3 onSidePosition;

	public List<UIPartActionColorPicker> colorPickers;

	public int controlIndex;

	public UIWorldPointer pointer;

	public bool pinned;

	public bool hover;

	public bool numericSliders;

	public bool dragging => _dragging;

	public Part part => _part;

	public bool isValid
	{
		get
		{
			return _isValid;
		}
		set
		{
			_isValid = value;
		}
	}

	public bool displayDirty
	{
		get
		{
			return _displayDirty;
		}
		set
		{
			_displayDirty = value;
		}
	}

	public DisplayType Display
	{
		get
		{
			return displayType;
		}
		set
		{
			if (displayType != value)
			{
				displayType = value;
				displayDirty = true;
			}
		}
	}

	public float targetDistanceFromCamera { get; set; }

	public List<UIPartActionItem> ListItems => listItems;

	public UIWorldPointer Pointer => pointer;

	public bool Pinned => pinned;

	public bool Hover => hover;

	public bool NumericSliders => numericSliders;

	public void Awake()
	{
		rectTransform = (RectTransform)base.transform;
		titleBar.gameObject.SetActive(value: false);
		if (togglePinned != null)
		{
			togglePinned.onValueChanged.AddListener(OnPin);
		}
		if (toggleNumeric != null)
		{
			toggleNumeric.isOn = GameSettings.PAW_NUMERIC_SLIDERS;
			toggleNumeric.onValueChanged.AddListener(OnNumericSwap);
		}
		previousUiScale = UIMasterController.Instance.uiScale;
		colorPickers = new List<UIPartActionColorPicker>();
		parameterGroups = new DictionaryValueList<string, UIPartActionGroup>();
		GameEvents.onTimeWarpRateChanged.Add(RecyclePartList);
		GameEvents.onPartActionNumericSlider.Add(OnNumericSwapped);
		GameEvents.onCrewOnEva.Add(OnKerbalEVA);
		GameEvents.onCrewBoardVessel.Add(OnCrewBoardVessel);
		GameEvents.onEditorShipCrewModified.Add(OnEditorShipCrewModified);
		GameEvents.onEditorPartEvent.Add(OnEditorPartEvent);
		GameEvents.onCrewTransferred.Add(OnCrewTransferred);
	}

	public void Start()
	{
		GameEvents.OnGameSettingsApplied.Add(OnGameSettingsApplied);
		UpdateColorPickers();
		GameEvents.onPartActionInitialized.Fire(part);
	}

	public void OnEnable()
	{
		UpdateWindow();
		UpdateColorPickers();
	}

	public void OnDestroy()
	{
		if (pointer != null)
		{
			pointer.Terminate();
		}
		if (_part._partActionWindow != null)
		{
			_part._partActionWindow = null;
		}
		GameEvents.onTimeWarpRateChanged.Remove(RecyclePartList);
		GameEvents.OnGameSettingsApplied.Remove(OnGameSettingsApplied);
		GameEvents.onPartActionNumericSlider.Remove(OnNumericSwapped);
		GameEvents.onCrewOnEva.Remove(OnKerbalEVA);
		GameEvents.onCrewBoardVessel.Remove(OnCrewBoardVessel);
		GameEvents.onEditorShipCrewModified.Remove(OnEditorShipCrewModified);
		GameEvents.onEditorPartEvent.Remove(OnEditorPartEvent);
		GameEvents.onCrewTransferred.Remove(OnCrewTransferred);
	}

	public void OnGameSettingsApplied()
	{
		float num = UIMasterController.Instance.uiScale - previousUiScale;
		rectTransform.anchoredPosition *= 1f - num;
		previousUiScale = UIMasterController.Instance.uiScale;
		UpdateCacheVariables();
	}

	public bool Setup(Part part, DisplayType type, UI_Scene scene)
	{
		_part = part;
		this.scene = scene;
		if (Generator == null)
		{
			Seed = Environment.TickCount ^ Guid.NewGuid().GetHashCode();
			Generator = new KSPRandom(Seed);
		}
		numericSliders = GameSettings.PAW_NUMERIC_SLIDERS;
		if ((!CreatePartList(clearFirst: true) && !pinned) || (HighLogic.LoadedSceneIsFlight && ActionGroupsFlightController.Instance != null && ActionGroupsFlightController.Instance.IsOpen))
		{
			isValid = false;
			return false;
		}
		if (part.partInfo.name == "flag" && part.vessel != null)
		{
			titleText.text = part.vessel.GetDisplayName();
		}
		else
		{
			titleText.text = part.partInfo.title;
		}
		titleBar.gameObject.SetActive(value: true);
		displayType = type;
		isValid = true;
		Canvas.ForceUpdateCanvases();
		UpdateCacheVariables();
		ResizeWindowDimensions();
		TrackPartPosition();
		_part._partActionWindow = this;
		GameEvents.onPartActionUIShown.Fire(this, part);
		toggleNumeric.gameObject.SetActive(usingNumericValue);
		return true;
	}

	public void UpdateCacheVariables()
	{
		windowDimensions = new Vector2(windowTransform.sizeDelta.x, windowTransform.sizeDelta.y);
		maxWindowHeight = ((float)Screen.height - windowPixelMargin * 2f) / GameSettings.UI_SCALE;
		screenEdgeOffsetVector = new Vector2(screenEdgeOffset, screenEdgeOffset);
		scrollBarWidth = (itemsScrollBar.gameObject.transform as RectTransform).sizeDelta.x;
		UpdateWindowHeight(GameSettings.PAW_PREFERRED_HEIGHT);
	}

	public void UpdateWindowHeight(float newHeight)
	{
		windowHeight = Mathf.Min(maxWindowHeight, newHeight);
	}

	public bool CreatePartList(bool clearFirst)
	{
		if (part == null)
		{
			return false;
		}
		if (clearFirst)
		{
			ClearList();
		}
		else
		{
			int count = listItems.Count;
			while (count-- > 0)
			{
				if (!listItems[count].IsItemValid())
				{
					if (listItems[count] != null)
					{
						UnityEngine.Object.DestroyImmediate(listItems[count].gameObject);
					}
					RemoveItemAt(count);
				}
			}
		}
		GameEvents.onPartActionUICreate.Fire(part);
		controlIndex = 0;
		if (displayType != DisplayType.ResourceOnly)
		{
			int i = 0;
			for (int count2 = part.Fields.Count; i < count2; i++)
			{
				BaseField baseField = part.Fields[i];
				if (CanActivateField(baseField, part, scene))
				{
					if (clearFirst || !TrySetFieldControl(baseField, part, null))
					{
						AddFieldControl(baseField, part, null);
					}
				}
				else if (!clearFirst && !pinned)
				{
					RemoveFieldControl(baseField, part, null);
				}
			}
			int j = 0;
			for (int count3 = part.Modules.Count; j < count3; j++)
			{
				PartModule partModule = part.Modules[j];
				if (!partModule.isEnabled)
				{
					continue;
				}
				int k = 0;
				for (int count4 = partModule.Fields.Count; k < count4; k++)
				{
					BaseField baseField = partModule.Fields[k];
					if (CanActivateField(baseField, part, scene))
					{
						if (clearFirst || !TrySetFieldControl(baseField, part, partModule))
						{
							AddFieldControl(baseField, part, partModule);
						}
					}
					else if (!clearFirst && !pinned)
					{
						RemoveFieldControl(baseField, part, partModule);
					}
				}
			}
			if (HasKerbalInventory(part))
			{
				bool flag = FlightGlobals.ActiveVessel == part.vessel || (part.transform.position - FlightGlobals.ActiveVessel.transform.position).magnitude < GameSettings.EVA_INVENTORY_RANGE;
				List<ProtoCrewMember> partCrew = GetPartCrew(part);
				for (int l = 0; l < partCrew.Count; l++)
				{
					ProtoCrewMember protoCrewMember = partCrew[l];
					if (flag)
					{
						if (clearFirst || !TrySetFieldControl(protoCrewMember.KerbalInventoryModule.Fields["InventorySlots"], part, protoCrewMember.KerbalInventoryModule))
						{
							AddCrewInventory(partCrew[l]);
						}
					}
					else if (!clearFirst && !pinned)
					{
						RemoveCrewInventory(part);
					}
				}
			}
			int m = 0;
			for (int count5 = part.Events.Count; m < count5; m++)
			{
				BaseEvent byIndex = part.Events.GetByIndex(m);
				if (CanActivateEvent(byIndex, part, scene))
				{
					if (clearFirst || !TrySetEventControl(byIndex, part, null))
					{
						AddEventControl(byIndex, part, null);
					}
				}
				else if (!clearFirst && !pinned)
				{
					RemoveEventControl(byIndex, part, null);
				}
			}
			int n = 0;
			for (int count6 = part.Modules.Count; n < count6; n++)
			{
				PartModule partModule = part.Modules[n];
				if (!partModule.isEnabled)
				{
					continue;
				}
				int num = 0;
				for (int count7 = partModule.Events.Count; num < count7; num++)
				{
					BaseEvent byIndex = partModule.Events.GetByIndex(num);
					if (CanActivateEvent(byIndex, part, scene))
					{
						if (clearFirst || !TrySetEventControl(byIndex, part, partModule))
						{
							AddEventControl(byIndex, part, partModule);
						}
					}
					else if (!clearFirst && !pinned)
					{
						RemoveEventControl(byIndex, part, partModule);
					}
				}
			}
			int num2 = 0;
			for (int count8 = part.Resources.Count; num2 < count8; num2++)
			{
				PartResource r = part.Resources[num2];
				SetupResourceControls(r, clearFirst, scene, ref controlIndex);
			}
			if (CanActivateResourcePriorityDisplay(part))
			{
				if (!TrySetResourcePriorityControl(part))
				{
					AddResourcePriorityControl(part);
				}
			}
			else
			{
				RemoveResourcePriorityControl(part);
			}
			if (CanActivateFuelFlowOverlay(part))
			{
				if (!TrySetFuelFlowOverlay(part))
				{
					AddFuelFlowOverlay(part);
				}
			}
			else
			{
				RemoveFuelFlowOverlay(part);
			}
			if (CanActivateAeroDisplay(part))
			{
				if (!TrySetAeroControl(part))
				{
					AddAeroControl(part);
				}
			}
			else
			{
				RemoveAeroControl(part);
			}
			if (CanActivateThermalDisplay(part))
			{
				if (!TrySetThermalControl(part))
				{
					AddThermalControl(part);
				}
			}
			else
			{
				RemoveThermalControl(part);
			}
			if (CanActivateRoboticJointDisplay(part))
			{
				if (!TrySetRoboticJointControl(part))
				{
					AddRoboticJointControl(part);
				}
			}
			else
			{
				RemoveRoboticJointControl(part);
			}
		}
		else
		{
			int num3 = 0;
			for (int count9 = part.Resources.Count; num3 < count9; num3++)
			{
				PartResource r = part.Resources[num3];
				if (UIPartActionController.Instance.resourcesShown.Contains(r.info.id))
				{
					SetupResourceControls(r, clearFirst: false, scene, ref controlIndex);
				}
				else
				{
					RemoveResourceControlFlight(r);
				}
			}
		}
		if (listItems.Count == 0 && !pinned)
		{
			return false;
		}
		int num4 = 0;
		for (int count10 = listItems.Count; num4 < count10; num4++)
		{
			listItems[num4].UpdateItem();
		}
		return true;
	}

	public bool CanActivateField(BaseField fld, Part p, UI_Scene scene)
	{
		if (!GameSettings.ADVANCED_TWEAKABLES && fld.advancedTweakable)
		{
			return false;
		}
		if (scene == UI_Scene.Flight)
		{
			if (!(FlightGlobals.ActiveVessel == part.vessel) && (!fld.guiActiveUnfocused || (p.transform.position - FlightGlobals.ActiveVessel.transform.position).sqrMagnitude >= fld.guiUnfocusedRange * fld.guiUnfocusedRange))
			{
				return false;
			}
			return fld.guiActive;
		}
		return fld.guiActiveEditor;
	}

	public UIPartActionFieldItem TrySetFieldControl(BaseField field, Part part, PartModule module)
	{
		UI_Control uI_Control = ((scene == UI_Scene.Flight) ? field.uiControlFlight : field.uiControlEditor);
		int count = listItems.Count;
		int num = 0;
		UIPartActionFieldItem listFieldItem;
		while (true)
		{
			if (num < count)
			{
				listFieldItem = listItemCache[num].listFieldItem;
				if (!(listFieldItem == null) && listFieldItem.Part == part && listFieldItem.PartModule == module && listFieldItem.Control == uI_Control && listFieldItem.Field == field)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		if (listItemPositions[num] != controlIndex)
		{
			listItemPositions[num] = controlIndex;
			listFieldItem.transform.SetSiblingIndex(controlIndex);
		}
		controlIndex++;
		return listFieldItem;
	}

	public bool RemoveFieldControl(BaseField field, Part part, PartModule module)
	{
		UI_Control uI_Control = ((scene == UI_Scene.Flight) ? field.uiControlFlight : field.uiControlEditor);
		int count = listItems.Count;
		int num = 0;
		while (true)
		{
			if (num < count)
			{
				UIPartActionFieldItem listFieldItem = listItemCache[num].listFieldItem;
				if (!(listFieldItem == null) && listFieldItem.Part == part && listFieldItem.PartModule == module && listFieldItem.Control == uI_Control && listFieldItem.Field == field)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		UnityEngine.Object.DestroyImmediate(listItems[num].gameObject);
		RemoveItemAt(num);
		return true;
	}

	public void AddFieldControl(BaseField field, Part part, PartModule module)
	{
		UI_Control uI_Control = ((scene == UI_Scene.Flight) ? field.uiControlFlight : field.uiControlEditor);
		UIPartActionFieldItem fieldControl = UIPartActionController.Instance.GetFieldControl(uI_Control.GetType());
		if (!(fieldControl == null))
		{
			UIPartActionFieldItem uIPartActionFieldItem = UnityEngine.Object.Instantiate(fieldControl);
			uIPartActionFieldItem.gameObject.SetActive(value: true);
			uIPartActionFieldItem.Setup(this, part, module, scene, uI_Control, field);
			AddParameterGroup(uIPartActionFieldItem);
		}
	}

	public List<ProtoCrewMember> GetPartCrew(Part p)
	{
		List<ProtoCrewMember> list = new List<ProtoCrewMember>();
		if (p == null)
		{
			return list;
		}
		if (HighLogic.LoadedSceneIsEditor)
		{
			if (part.FindModuleImplementing<KerbalSeat>() == null && part.FindModuleImplementing<ModuleCommand>() == null && part.CrewCapacity <= 0)
			{
				return list;
			}
			List<ProtoCrewMember> list2 = new List<ProtoCrewMember>();
			if (ShipConstruction.ShipManifest != null)
			{
				list2.AddRange(ShipConstruction.ShipManifest.GetAllCrew(includeNulls: false));
			}
			for (int i = 0; i < list2.Count; i++)
			{
				PartCrewManifest partForCrew = ShipConstruction.ShipManifest.GetPartForCrew(list2[i]);
				if (partForCrew == null)
				{
					continue;
				}
				for (int j = 0; j < partForCrew.partCrew.Length; j++)
				{
					if (part.craftID == partForCrew.PartID)
					{
						list.AddUnique(list2[i]);
					}
				}
			}
		}
		else if (HighLogic.LoadedSceneIsFlight)
		{
			if (part.FindModuleImplementing<KerbalEVA>() != null || part.protoModuleCrew == null || part.protoModuleCrew.Count <= 0)
			{
				return list;
			}
			list.AddRange(part.protoModuleCrew);
		}
		return list;
	}

	public void AddCrewInventory(List<ProtoCrewMember> crewMembers)
	{
		for (int i = 0; i < crewMembers.Count; i++)
		{
			AddCrewInventory(crewMembers[i]);
		}
	}

	public void AddCrewInventory(ProtoCrewMember crewMember)
	{
		if (crewMember == null || !(crewMember.KerbalInventoryModule != null))
		{
			return;
		}
		ModuleInventoryPart kerbalInventoryModule = crewMember.KerbalInventoryModule;
		if (kerbalInventoryModule == null)
		{
			return;
		}
		UI_Control uI_Control = ((scene == UI_Scene.Flight) ? kerbalInventoryModule.Fields["InventorySlots"].uiControlFlight : kerbalInventoryModule.Fields["InventorySlots"].uiControlEditor);
		if (uI_Control != null)
		{
			UIPartActionFieldItem fieldControl = UIPartActionController.Instance.GetFieldControl(uI_Control.GetType());
			if (!(fieldControl == null))
			{
				UIPartActionFieldItem uIPartActionFieldItem = UnityEngine.Object.Instantiate(fieldControl);
				uIPartActionFieldItem.gameObject.SetActive(value: true);
				uIPartActionFieldItem.Setup(this, part, kerbalInventoryModule, scene, uI_Control, kerbalInventoryModule.Fields["InventorySlots"]);
				string displayName = crewMember.displayName;
				int index = controlIndex;
				AddGroup(uIPartActionFieldItem.transform, displayName, startCollapsed: false);
				AddItem(uIPartActionFieldItem, index);
				kerbalInventoryModule.Fields["InventorySlots"].guiActive = true;
			}
		}
	}

	public void RemoveCrewInventory(Part crewPart)
	{
		if (crewPart == null)
		{
			return;
		}
		int num = 0;
		ModuleInventoryPart moduleInventoryPart;
		while (true)
		{
			if (num >= listItems.Count)
			{
				return;
			}
			if (listItems[num].PartModule != null)
			{
				moduleInventoryPart = listItems[num].PartModule as ModuleInventoryPart;
				if (moduleInventoryPart != null && string.Equals(crewPart.vessel.vesselName, moduleInventoryPart.kerbalReference.name))
				{
					break;
				}
			}
			num++;
		}
		string displayName = crewPart.vessel.GetDisplayName();
		RemoveGroup(displayName);
		moduleInventoryPart.Fields["InventorySlots"].guiActive = false;
	}

	public void OnKerbalEVA(GameEvents.FromToAction<Part, Part> fv)
	{
		if (fv.from != null && fv.from.persistentId == part.persistentId)
		{
			RemoveCrewInventory(fv.to);
		}
	}

	public void OnCrewBoardVessel(GameEvents.FromToAction<Part, Part> fp)
	{
		if (fp.from != null && fp.to != null && fp.to.persistentId == part.persistentId)
		{
			displayDirty = true;
		}
	}

	public void OnEditorPartEvent(ConstructionEventType evt, Part p)
	{
		if (HighLogic.LoadedSceneIsFlight && p != null && part != null && evt == ConstructionEventType.PartDetached && p.persistentId == part.persistentId)
		{
			isValid = false;
		}
	}

	public void OnEditorShipCrewModified(VesselCrewManifest vcm)
	{
		displayDirty = true;
	}

	public void OnCrewTransferred(GameEvents.HostedFromToAction<ProtoCrewMember, Part> data)
	{
		displayDirty = true;
	}

	public bool CanActivateEvent(BaseEvent evt, Part p, UI_Scene scene)
	{
		if (!GameSettings.ADVANCED_TWEAKABLES && evt.advancedTweakable)
		{
			return false;
		}
		if (scene == UI_Scene.Flight)
		{
			if (FlightGlobals.ActiveVessel == part.vessel)
			{
				if (evt.guiActive && evt.active && !evt.EventIsDisabledByVariant)
				{
					if ((InputLockManager.IsUnlocked(ControlTypes.ACTIONS_SHIP) || NumericInputModeAndFocusedItem()) && part.vessel.IsControllable && (!evt.requireFullControl || InputLockManager.IsUnlocked(ControlTypes.TWEAKABLES_FULLONLY)))
					{
						return true;
					}
					return evt.guiActiveUncommand;
				}
				return false;
			}
			if (evt.guiActiveUnfocused && InputLockManager.IsUnlocked(ControlTypes.ACTIONS_EXTERNAL) && evt.active && !evt.EventIsDisabledByVariant && FlightGlobals.ActiveVessel.IsControllable && (!evt.externalToEVAOnly || FlightGlobals.ActiveVessel.isEVA))
			{
				return (p.transform.position - FlightGlobals.ActiveVessel.transform.position).sqrMagnitude < evt.unfocusedRange * evt.unfocusedRange;
			}
			return false;
		}
		if (evt.guiActiveEditor && evt.active)
		{
			return !evt.EventIsDisabledByVariant;
		}
		return false;
	}

	public UIPartActionButton TrySetEventControl(BaseEvent evt, Part part, PartModule module)
	{
		int count = listItems.Count;
		int num = 0;
		UIPartActionButton listButton;
		while (true)
		{
			if (num < count)
			{
				listButton = listItemCache[num].listButton;
				if (!(listButton == null) && listButton.Part == part && listButton.PartModule == module && listButton.Evt == evt)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		if (listItemPositions[num] != controlIndex)
		{
			listItemPositions[num] = controlIndex;
			listButton.transform.SetSiblingIndex(controlIndex);
		}
		controlIndex++;
		return listButton;
	}

	public bool RemoveEventControl(BaseEvent evt, Part part, PartModule module)
	{
		int count = listItems.Count;
		int num = 0;
		while (true)
		{
			if (num < count)
			{
				UIPartActionButton listButton = listItemCache[num].listButton;
				if (!(listButton == null) && listButton.Part == part && listButton.PartModule == module && listButton.Evt == evt)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		UnityEngine.Object.DestroyImmediate(listItems[num].gameObject);
		RemoveItemAt(num);
		return true;
	}

	public void AddEventControl(BaseEvent evt, Part part, PartModule module)
	{
		UIPartActionButton uIPartActionButton = UnityEngine.Object.Instantiate(UIPartActionController.Instance.eventItemPrefab);
		uIPartActionButton.gameObject.SetActive(value: true);
		uIPartActionButton.Setup(this, part, module, scene, null, evt);
		AddParameterGroup(uIPartActionButton);
	}

	public void AddParameterGroup(UIPartActionItem pawFieldItem)
	{
		Transform transform = pawFieldItem.transform;
		bool flag = pawFieldItem is UIPartActionFieldItem;
		bool flag2 = pawFieldItem is UIPartActionButton;
		UIPartActionFieldItem uIPartActionFieldItem = pawFieldItem as UIPartActionFieldItem;
		UIPartActionButton uIPartActionButton = pawFieldItem as UIPartActionButton;
		string value = "";
		int index = controlIndex;
		if (flag)
		{
			if (uIPartActionFieldItem.Field != null)
			{
				value = uIPartActionFieldItem.Field.group.name;
			}
		}
		else if (flag2 && uIPartActionButton.Evt != null)
		{
			value = uIPartActionButton.Evt.group.name;
		}
		if (!string.IsNullOrEmpty(value))
		{
			if (flag)
			{
				if (uIPartActionFieldItem.Field != null)
				{
					AddGroup(transform, uIPartActionFieldItem.Field.group);
				}
			}
			else if (flag2 && uIPartActionButton.Evt != null)
			{
				AddGroup(transform, uIPartActionButton.Evt.group);
			}
		}
		else
		{
			transform.transform.SetParent(layoutGroup.transform, worldPositionStays: false);
			transform.transform.SetSiblingIndex(controlIndex++);
		}
		AddItem(pawFieldItem, index);
	}

	public void AddGroup(Transform t, BasePAWGroup group)
	{
		UIPartActionGroup val = null;
		if (parameterGroups.TryGetValue(group.name, out val))
		{
			val.AddItemToContent(t);
			controlIndex++;
			return;
		}
		val = UnityEngine.Object.Instantiate(UIPartActionController.Instance.groupPrefab);
		val.gameObject.SetActive(value: true);
		val.transform.SetParent(layoutGroup.transform, worldPositionStays: false);
		val.transform.SetSiblingIndex(controlIndex++);
		parameterGroups.Add(group.name, val);
		val.AddItemToContent(t);
		val.Initialize(group.name, group.displayName, group.startCollapsed, this);
	}

	public void AddGroup(Transform t, string groupName, bool startCollapsed)
	{
		UIPartActionGroup val = null;
		if (parameterGroups.TryGetValue(groupName, out val))
		{
			val.AddItemToContent(t);
			controlIndex++;
			return;
		}
		val = UnityEngine.Object.Instantiate(UIPartActionController.Instance.groupPrefab);
		val.gameObject.SetActive(value: true);
		val.transform.SetParent(layoutGroup.transform, worldPositionStays: false);
		val.transform.SetSiblingIndex(controlIndex++);
		parameterGroups.Add(groupName, val);
		val.AddItemToContent(t);
		val.Initialize(groupName, groupName, startCollapsed, this);
	}

	public void RemoveGroup(string groupName)
	{
		UIPartActionGroup val = null;
		if (parameterGroups.TryGetValue(groupName, out val))
		{
			parameterGroups.Remove(groupName);
			UnityEngine.Object.Destroy(val.gameObject);
		}
	}

	public void SetupResourceControls(PartResource r, bool clearFirst, UI_Scene scene, ref int controlIndex)
	{
		if (scene == UI_Scene.Flight)
		{
			if (!CanActivateResource(r, part, scene))
			{
				if (!clearFirst)
				{
					RemoveResourceControlFlight(r);
				}
				return;
			}
			if (!TrySetResourceControlFlight(r))
			{
				AddResourceFlightControl(r);
			}
		}
		else
		{
			if (!CanActivateResource(r, part, scene))
			{
				if (!clearFirst)
				{
					RemoveResourceControlEditor(r);
				}
				return;
			}
			if (!TrySetResourceControlEditor(r))
			{
				AddResourceEditorControl(r);
			}
		}
		if (scene == UI_Scene.Flight && r.info.resourceTransferMode != 0 && UIPartActionController.Instance.ShowTransfers(r))
		{
			if (!TrySetResourceTransferControl(r))
			{
				AddResourceTransferControl(r);
			}
		}
		else
		{
			RemoveResourceTransferControl(r);
		}
	}

	public bool CanActivateResource(PartResource rsrc, Part p, UI_Scene scene)
	{
		if (rsrc.maxAmount == 0.0)
		{
			return false;
		}
		if (scene == UI_Scene.Flight)
		{
			if (FlightGlobals.ActiveVessel == part.vessel && rsrc.isVisible)
			{
				return true;
			}
			return false;
		}
		if (rsrc.isTweakable && rsrc.isVisible)
		{
			return true;
		}
		return false;
	}

	public UIPartActionResource TrySetResourceControlFlight(PartResource r)
	{
		int count = listItems.Count;
		int num = 0;
		UIPartActionResource listResource;
		while (true)
		{
			if (num < count)
			{
				listResource = listItemCache[num].listResource;
				if (!(listResource == null) && listResource.Resource == r)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		if (listItemPositions[num] != controlIndex)
		{
			listItemPositions[num] = controlIndex;
			listResource.transform.SetSiblingIndex(controlIndex);
		}
		controlIndex++;
		return listResource;
	}

	public UIPartActionResourceEditor TrySetResourceControlEditor(PartResource r)
	{
		int count = listItems.Count;
		int num = 0;
		UIPartActionResourceEditor listResourceEditor;
		while (true)
		{
			if (num < count)
			{
				listResourceEditor = listItemCache[num].listResourceEditor;
				if (!(listResourceEditor == null) && listResourceEditor.Resource == r)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		if (listItemPositions[num] != controlIndex)
		{
			listItemPositions[num] = controlIndex;
			listResourceEditor.transform.SetSiblingIndex(controlIndex);
			controlIndex++;
		}
		return listResourceEditor;
	}

	public UIPartActionResourceTransfer TrySetResourceTransferControl(PartResource r)
	{
		int count = listItems.Count;
		int num = 0;
		UIPartActionResourceTransfer listResourceTransfer;
		while (true)
		{
			if (num < count)
			{
				listResourceTransfer = listItemCache[num].listResourceTransfer;
				if (!(listResourceTransfer == null) && listResourceTransfer.Resource == r)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		if (listItemPositions[num] != controlIndex)
		{
			listItemPositions[num] = controlIndex;
			listResourceTransfer.transform.SetSiblingIndex(controlIndex);
		}
		controlIndex++;
		return listResourceTransfer;
	}

	public bool RemoveResourceControlFlight(PartResource r)
	{
		int count = listItems.Count;
		int num = 0;
		while (true)
		{
			if (num < count)
			{
				UIPartActionResource listResource = listItemCache[num].listResource;
				if (!(listResource == null) && listResource.Resource == r)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		UnityEngine.Object.DestroyImmediate(listItems[num].gameObject);
		RemoveItemAt(num);
		return true;
	}

	public bool RemoveResourceControlEditor(PartResource r)
	{
		int count = listItems.Count;
		int num = 0;
		while (true)
		{
			if (num < count)
			{
				UIPartActionResourceEditor listResourceEditor = listItemCache[num].listResourceEditor;
				if (!(listResourceEditor == null) && listResourceEditor.Resource == r)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		UnityEngine.Object.DestroyImmediate(listItems[num].gameObject);
		RemoveItemAt(num);
		return true;
	}

	public bool RemoveResourceTransferControl(PartResource r)
	{
		int count = listItems.Count;
		int num = 0;
		while (true)
		{
			if (num < count)
			{
				UIPartActionResourceTransfer listResourceTransfer = listItemCache[num].listResourceTransfer;
				if (!(listResourceTransfer == null) && listResourceTransfer.Resource == r)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		UnityEngine.Object.DestroyImmediate(listItems[num].gameObject);
		RemoveItemAt(num);
		return true;
	}

	public void AddResourceTransferControl(PartResource r)
	{
		UIPartActionResourceTransfer uIPartActionResourceTransfer = UnityEngine.Object.Instantiate(UIPartActionController.Instance.resourceTransferItemPrefab);
		uIPartActionResourceTransfer.gameObject.SetActive(value: true);
		uIPartActionResourceTransfer.transform.SetParent(layoutGroup.transform, worldPositionStays: false);
		uIPartActionResourceTransfer.transform.SetSiblingIndex(controlIndex++);
		uIPartActionResourceTransfer.Setup(this, part, scene, null, r);
		UIPartActionController.Instance.SetupResourceTransfer(uIPartActionResourceTransfer);
		AddItem(uIPartActionResourceTransfer, controlIndex - 1);
	}

	public void AddResourceFlightControl(PartResource r)
	{
		UIPartActionResource uIPartActionResource = UnityEngine.Object.Instantiate(UIPartActionController.Instance.resourceItemPrefab);
		uIPartActionResource.gameObject.SetActive(value: true);
		uIPartActionResource.transform.SetParent(layoutGroup.transform, worldPositionStays: false);
		uIPartActionResource.transform.SetSiblingIndex(controlIndex++);
		uIPartActionResource.Setup(this, part, scene, null, r);
		AddItem(uIPartActionResource, controlIndex - 1);
	}

	public void AddResourceEditorControl(PartResource r)
	{
		UIPartActionResourceEditor uIPartActionResourceEditor = UnityEngine.Object.Instantiate(UIPartActionController.Instance.resourceItemEditorPrefab);
		uIPartActionResourceEditor.gameObject.SetActive(value: true);
		uIPartActionResourceEditor.transform.SetParent(layoutGroup.transform, worldPositionStays: false);
		uIPartActionResourceEditor.transform.SetSiblingIndex(controlIndex++);
		uIPartActionResourceEditor.Setup(this, part, scene, null, r);
		AddItem(uIPartActionResourceEditor, controlIndex - 1);
	}

	public bool ContainsResourceControl(PartResource r)
	{
		int count = listItems.Count;
		int num = 0;
		while (true)
		{
			if (num < count)
			{
				UIPartActionResource uIPartActionResource = listItems[num] as UIPartActionResource;
				if (!(uIPartActionResource == null) && uIPartActionResource.Resource.resourceName == r.resourceName)
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

	public bool ContainsResourceTransferControl(PartResource resource, out UIPartActionResourceTransfer control)
	{
		int count = listItems.Count;
		control = null;
		int num = 0;
		while (true)
		{
			if (num < count)
			{
				control = listItems[num] as UIPartActionResourceTransfer;
				if (!(control == null) && (resource == null || resource.resourceName == control.Resource.resourceName))
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

	public void RefreshResourceTransferTargets()
	{
		int count = listItems.Count;
		for (int i = 0; i < count; i++)
		{
			UIPartActionResourceTransfer uIPartActionResourceTransfer = listItems[i] as UIPartActionResourceTransfer;
			if (!(uIPartActionResourceTransfer == null))
			{
				UIPartActionController.Instance.SetupResourceTransfer(uIPartActionResourceTransfer);
			}
		}
	}

	public bool CanActivateAeroDisplay(Part p)
	{
		if (PhysicsGlobals.AeroDataDisplay)
		{
			return p.dragModel == Part.DragModel.CUBE;
		}
		return false;
	}

	public UIPartActionAeroDisplay TrySetAeroControl(Part r)
	{
		int count = listItems.Count;
		int num = 0;
		UIPartActionAeroDisplay listAeroDisplay;
		while (true)
		{
			if (num < count)
			{
				listAeroDisplay = listItemCache[num].listAeroDisplay;
				if (!(listAeroDisplay == null))
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		if (listItemPositions[num] != controlIndex)
		{
			listItemPositions[num] = controlIndex;
			listAeroDisplay.transform.SetSiblingIndex(controlIndex);
		}
		controlIndex++;
		return listAeroDisplay;
	}

	public void AddAeroControl(Part p)
	{
		UIPartActionAeroDisplay debugAeroItemPrefab = UIPartActionController.Instance.debugAeroItemPrefab;
		if (!(debugAeroItemPrefab == null))
		{
			UIPartActionAeroDisplay uIPartActionAeroDisplay = UnityEngine.Object.Instantiate(debugAeroItemPrefab);
			uIPartActionAeroDisplay.gameObject.SetActive(value: true);
			if (uIPartActionAeroDisplay.pawGroup != null)
			{
				AddGroup(uIPartActionAeroDisplay.transform, uIPartActionAeroDisplay.pawGroup);
			}
			else
			{
				uIPartActionAeroDisplay.transform.SetParent(layoutGroup.transform, worldPositionStays: false);
			}
			uIPartActionAeroDisplay.transform.SetSiblingIndex(controlIndex++);
			uIPartActionAeroDisplay.Setup(this, part, scene);
			AddItem(uIPartActionAeroDisplay, controlIndex - 1);
		}
	}

	public bool RemoveAeroControl(Part p)
	{
		int count = listItems.Count;
		int num = 0;
		UIPartActionAeroDisplay listAeroDisplay;
		while (true)
		{
			if (num < count)
			{
				listAeroDisplay = listItemCache[num].listAeroDisplay;
				if (!(listAeroDisplay == null))
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		UnityEngine.Object.DestroyImmediate(listItems[num].gameObject);
		RemoveItemAt(num);
		if (listAeroDisplay.pawGroup != null)
		{
			UIPartActionGroup val = null;
			parameterGroups.TryGetValue(listAeroDisplay.pawGroup.name, out val);
			if (val != null)
			{
				val.UpdateContentSize();
			}
		}
		return true;
	}

	public bool CanActivateThermalDisplay(Part p)
	{
		if (!HighLogic.LoadedSceneIsEditor)
		{
			return PhysicsGlobals.ThermalDataDisplay;
		}
		return false;
	}

	public UIPartActionThermalDisplay TrySetThermalControl(Part r)
	{
		int count = listItems.Count;
		int num = 0;
		UIPartActionThermalDisplay listThermalDisplay;
		while (true)
		{
			if (num < count)
			{
				listThermalDisplay = listItemCache[num].listThermalDisplay;
				if (!(listThermalDisplay == null))
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		if (listItemPositions[num] != controlIndex)
		{
			listItemPositions[num] = controlIndex;
			listThermalDisplay.transform.SetSiblingIndex(controlIndex);
		}
		controlIndex++;
		return listThermalDisplay;
	}

	public void AddThermalControl(Part p)
	{
		UIPartActionThermalDisplay debugThermalItemPrefab = UIPartActionController.Instance.debugThermalItemPrefab;
		if (!(debugThermalItemPrefab == null))
		{
			UIPartActionThermalDisplay uIPartActionThermalDisplay = UnityEngine.Object.Instantiate(debugThermalItemPrefab);
			uIPartActionThermalDisplay.gameObject.SetActive(value: true);
			if (uIPartActionThermalDisplay.pawGroup != null)
			{
				AddGroup(uIPartActionThermalDisplay.transform, uIPartActionThermalDisplay.pawGroup);
			}
			else
			{
				uIPartActionThermalDisplay.transform.SetParent(layoutGroup.transform, worldPositionStays: false);
			}
			uIPartActionThermalDisplay.transform.SetSiblingIndex(controlIndex++);
			uIPartActionThermalDisplay.Setup(this, part, scene);
			AddItem(uIPartActionThermalDisplay, controlIndex - 1);
		}
	}

	public bool RemoveThermalControl(Part p)
	{
		int count = listItems.Count;
		int num = 0;
		UIPartActionThermalDisplay listThermalDisplay;
		while (true)
		{
			if (num < count)
			{
				listThermalDisplay = listItemCache[num].listThermalDisplay;
				if (!(listThermalDisplay == null))
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		UnityEngine.Object.DestroyImmediate(listItems[num].gameObject);
		RemoveItemAt(num);
		if (listThermalDisplay.pawGroup != null)
		{
			UIPartActionGroup val = null;
			parameterGroups.TryGetValue(listThermalDisplay.pawGroup.name, out val);
			if (val != null)
			{
				val.UpdateContentSize();
			}
		}
		return true;
	}

	public bool CanActivateRoboticJointDisplay(Part p)
	{
		if (!ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			return false;
		}
		if (!HighLogic.LoadedSceneIsEditor && PhysicsGlobals.RoboticJointDataDisplay)
		{
			return p.isRobotic();
		}
		return false;
	}

	public UIPartActionRoboticJointDisplay TrySetRoboticJointControl(Part r)
	{
		int count = listItems.Count;
		int num = 0;
		UIPartActionRoboticJointDisplay listRoboticJointDisplay;
		while (true)
		{
			if (num < count)
			{
				listRoboticJointDisplay = listItemCache[num].listRoboticJointDisplay;
				if (!(listRoboticJointDisplay == null))
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		if (listItemPositions[num] != controlIndex)
		{
			listItemPositions[num] = controlIndex;
			listRoboticJointDisplay.transform.SetSiblingIndex(controlIndex);
		}
		controlIndex++;
		return listRoboticJointDisplay;
	}

	public void AddRoboticJointControl(Part p)
	{
		UIPartActionRoboticJointDisplay debugRoboticJointItemPrefab = UIPartActionController.Instance.debugRoboticJointItemPrefab;
		if (!(debugRoboticJointItemPrefab == null))
		{
			UIPartActionRoboticJointDisplay uIPartActionRoboticJointDisplay = UnityEngine.Object.Instantiate(debugRoboticJointItemPrefab);
			uIPartActionRoboticJointDisplay.gameObject.SetActive(value: true);
			if (uIPartActionRoboticJointDisplay.pawGroup != null)
			{
				AddGroup(uIPartActionRoboticJointDisplay.transform, uIPartActionRoboticJointDisplay.pawGroup);
			}
			else
			{
				uIPartActionRoboticJointDisplay.transform.SetParent(layoutGroup.transform, worldPositionStays: false);
			}
			uIPartActionRoboticJointDisplay.transform.SetSiblingIndex(controlIndex++);
			uIPartActionRoboticJointDisplay.Setup(this, part, scene);
			AddItem(uIPartActionRoboticJointDisplay, controlIndex - 1);
		}
	}

	public bool RemoveRoboticJointControl(Part p)
	{
		int count = listItems.Count;
		int num = 0;
		UIPartActionRoboticJointDisplay listRoboticJointDisplay;
		while (true)
		{
			if (num < count)
			{
				listRoboticJointDisplay = listItemCache[num].listRoboticJointDisplay;
				if (!(listRoboticJointDisplay == null))
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		UnityEngine.Object.DestroyImmediate(listItems[num].gameObject);
		RemoveItemAt(num);
		if (listRoboticJointDisplay.pawGroup != null)
		{
			UIPartActionGroup val = null;
			parameterGroups.TryGetValue(listRoboticJointDisplay.pawGroup.name, out val);
			if (val != null)
			{
				val.UpdateContentSize();
			}
		}
		return true;
	}

	public bool CanActivateResourcePriorityDisplay(Part p)
	{
		if (GameSettings.ADVANCED_TWEAKABLES)
		{
			if (p.Resources != null && p.Resources.HasFlowableUnhidden() && (!HighLogic.LoadedSceneIsFlight || !(p.vessel != null) || (p.vessel.vesselType != VesselType.const_11 && !(p.vessel != FlightGlobals.ActiveVessel))))
			{
				return true;
			}
			return p.alwaysShowResourcePriority;
		}
		return false;
	}

	public UIPartActionResourcePriority TrySetResourcePriorityControl(Part r)
	{
		int count = listItems.Count;
		int num = 0;
		UIPartActionResourcePriority listResourcePriority;
		while (true)
		{
			if (num < count)
			{
				listResourcePriority = listItemCache[num].listResourcePriority;
				if (!(listResourcePriority == null))
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		if (listItemPositions[num] != controlIndex)
		{
			listItemPositions[num] = controlIndex;
			listResourcePriority.transform.SetSiblingIndex(controlIndex);
		}
		controlIndex++;
		return listResourcePriority;
	}

	public void AddResourcePriorityControl(Part p)
	{
		UIPartActionResourcePriority resourcePriorityPrefab = UIPartActionController.Instance.resourcePriorityPrefab;
		if (!(resourcePriorityPrefab == null))
		{
			UIPartActionResourcePriority uIPartActionResourcePriority = UnityEngine.Object.Instantiate(resourcePriorityPrefab);
			uIPartActionResourcePriority.gameObject.SetActive(value: true);
			uIPartActionResourcePriority.transform.SetParent(layoutGroup.transform, worldPositionStays: false);
			uIPartActionResourcePriority.transform.SetSiblingIndex(controlIndex++);
			uIPartActionResourcePriority.Setup(this, part, scene);
			AddItem(uIPartActionResourcePriority, controlIndex - 1);
		}
	}

	public bool RemoveResourcePriorityControl(Part p)
	{
		int count = listItems.Count;
		int num = 0;
		while (true)
		{
			if (num < count)
			{
				if (!(listItemCache[num].listResourcePriority == null))
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		UnityEngine.Object.DestroyImmediate(listItems[num].gameObject);
		RemoveItemAt(num);
		return true;
	}

	public bool CanActivateFuelFlowOverlay(Part p)
	{
		if (FuelFlowOverlay.instance != null && GameSettings.ADVANCED_TWEAKABLES && (!HighLogic.LoadedSceneIsFlight || p.vessel.vesselType != VesselType.const_11))
		{
			return SCCFlowGraphUCFinder.IsEntryPoint(p);
		}
		return false;
	}

	public UIPartActionFuelFlowOverlay TrySetFuelFlowOverlay(Part r)
	{
		int count = listItems.Count;
		int num = 0;
		UIPartActionFuelFlowOverlay listFuelFlowOverlay;
		while (true)
		{
			if (num < count)
			{
				listFuelFlowOverlay = listItemCache[num].listFuelFlowOverlay;
				if (!(listFuelFlowOverlay == null))
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		if (listItemPositions[num] != controlIndex)
		{
			listItemPositions[num] = controlIndex;
			listFuelFlowOverlay.transform.SetSiblingIndex(controlIndex);
		}
		controlIndex++;
		return listFuelFlowOverlay;
	}

	public void AddFuelFlowOverlay(Part p)
	{
		UIPartActionFuelFlowOverlay fuelFlowOverlayPrefab = UIPartActionController.Instance.fuelFlowOverlayPrefab;
		if (!(fuelFlowOverlayPrefab == null))
		{
			UIPartActionFuelFlowOverlay uIPartActionFuelFlowOverlay = UnityEngine.Object.Instantiate(fuelFlowOverlayPrefab);
			uIPartActionFuelFlowOverlay.gameObject.SetActive(value: true);
			uIPartActionFuelFlowOverlay.transform.SetParent(layoutGroup.transform, worldPositionStays: false);
			uIPartActionFuelFlowOverlay.transform.SetSiblingIndex(controlIndex++);
			uIPartActionFuelFlowOverlay.Setup(this, part, scene);
			AddItem(uIPartActionFuelFlowOverlay, controlIndex - 1);
		}
	}

	public bool RemoveFuelFlowOverlay(Part p)
	{
		int count = listItems.Count;
		int num = 0;
		while (true)
		{
			if (num < count)
			{
				if (!(listItemCache[num].listFuelFlowOverlay == null))
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		UnityEngine.Object.DestroyImmediate(listItems[num].gameObject);
		RemoveItemAt(num);
		return true;
	}

	public bool HasKerbalInventory(Part p)
	{
		if (p.CrewCapacity <= 0 && !p.isKerbalSeat())
		{
			return false;
		}
		return true;
	}

	public void RemoveItemAt(int index)
	{
		ListItems.RemoveAt(index);
		listItemPositions.RemoveAt(index);
		listItemCache[index].ClearReferences();
		listItemCache.RemoveAt(index);
	}

	public void AddItem(UIPartActionItem item, int index)
	{
		ListItems.Add(item);
		listItemPositions.Add(index);
		listItemCache.Add(new ListItemCache(item));
	}

	public void ClearList()
	{
		if (listItems.Count > 0)
		{
			int count = listItems.Count;
			while (count-- > 0)
			{
				if (listItems[count] != null)
				{
					UnityEngine.Object.Destroy(listItems[count].gameObject);
				}
				listItems[count] = null;
				listItemCache[count].ClearReferences();
			}
			listItems.Clear();
			listItemPositions.Clear();
			listItemCache.Clear();
		}
		for (int i = 0; i < parameterGroups.Count; i++)
		{
			UIPartActionGroup val = null;
			parameterGroups.TryGetValue(parameterGroups.KeyAt(i), out val);
			if (val != null)
			{
				UnityEngine.Object.Destroy(val.gameObject);
			}
		}
		parameterGroups.Clear();
	}

	public void RecyclePartList()
	{
		isValid = CreatePartList(clearFirst: false);
	}

	public void UpdateWindow()
	{
		if (!(part == null) && part.State != PartStates.DEAD)
		{
			if (scene == UI_Scene.Flight && part.vessel != FlightGlobals.ActiveVessel && displayType == DisplayType.ResourceOnly)
			{
				isValid = false;
			}
			else if (displayDirty)
			{
				displayDirty = false;
				if (!CreatePartList(clearFirst: true))
				{
					isValid = false;
				}
				UpdateColorPickers();
			}
			else if (listItems.Count <= 0 || !InputLockManager.IsAllLocked(ControlTypes.All))
			{
				if (listItems.Count == 0 && !pinned)
				{
					isValid = false;
					return;
				}
				if (!CreatePartList(clearFirst: false))
				{
					isValid = false;
					return;
				}
				ResizeWindowDimensions();
				PointerUpdate();
			}
		}
		else
		{
			isValid = false;
		}
	}

	public void CloseAllColorPickers(UIPartActionColorPicker caller)
	{
		int count = colorPickers.Count;
		while (count-- > 0)
		{
			if (colorPickers[count] != caller && colorPickers[count].showingColorSelectWindow)
			{
				colorPickers[count].CloseColorPickWindow();
			}
		}
	}

	public void ResizeWindowDimensions()
	{
		if (itemsContentTransform.sizeDelta.y + titleBarTransform.sizeDelta.y > windowHeight)
		{
			windowDimensions.y = windowHeight;
			if (!itemsScrollRect.enabled)
			{
				itemsScrollRect.enabled = true;
			}
			if (!itemsScrollBar.enabled)
			{
				itemsScrollBar.enabled = true;
				itemsScrollBar.gameObject.SetActive(value: true);
			}
		}
		else
		{
			windowDimensions.y = itemsContentTransform.sizeDelta.y + titleBarTransform.sizeDelta.y;
			if (itemsScrollRect.enabled)
			{
				itemsScrollRect.enabled = false;
			}
			if (itemsScrollBar.enabled)
			{
				itemsScrollBar.enabled = false;
				itemsScrollBar.gameObject.SetActive(value: false);
			}
		}
		itemsContentDimensions.x = itemsScrollRect.viewport.rect.size.x - (itemsScrollBar.enabled ? scrollBarWidth : 0f);
		itemsContentDimensions.y = itemsContentTransform.sizeDelta.y;
		contentAnchoredDimensions.x = itemsContentTransform.anchoredPosition.x;
		contentAnchoredDimensions.y = (itemsScrollBar.enabled ? Mathf.Clamp(itemsContentTransform.anchoredPosition.y, 0f, Mathf.Abs(windowHeight - titleBarTransform.sizeDelta.y - itemsContentTransform.sizeDelta.y)) : 0f);
		itemsContentTransform.anchoredPosition = contentAnchoredDimensions;
		itemsContentTransform.sizeDelta = itemsContentDimensions;
		windowTransform.sizeDelta = windowDimensions;
		UIMasterController.ClampToScreen(windowTransform, screenEdgeOffsetVector);
	}

	public void UpdateColorPickers()
	{
		colorPickers.Clear();
		colorPickers.AddRange(GetComponentsInChildren<UIPartActionColorPicker>());
	}

	public void PointerUpdate()
	{
		if (pointer == null)
		{
			pointer = UIWorldPointer.Create(rectTransform, part.transform, (scene == UI_Scene.Flight) ? FlightCamera.fetch.mainCamera : EditorCamera.Instance.cam, lineMaterial);
			pointer.transform.SetParent(titleBar, worldPositionStays: true);
			pointer.chamferDistance = lineCornerRadius;
			pointer.lineColor = lineColor;
			pointer.lineWidth = lineWidth;
		}
		bool flag = !pinned || Hover || GetPartHover();
		if (flag && !pointer.gameObject.activeSelf)
		{
			pointer.gameObject.SetActive(value: true);
		}
		if (!flag && pointer.gameObject.activeSelf)
		{
			pointer.gameObject.SetActive(value: false);
		}
	}

	public void TrackPartPosition()
	{
		switch (scene)
		{
		case UI_Scene.Flight:
			RepositionWindow(part.transform, FlightCamera.fetch.mainCamera);
			break;
		case UI_Scene.Editor:
			RepositionWindow(part.transform, EditorCamera.Instance.cam);
			break;
		}
	}

	public void RepositionWindow(Transform partTransform, Camera cam)
	{
		if (!(partTransform == null) && !(cam == null))
		{
			Vector3 vector = cam.WorldToScreenPoint(partTransform.position) / UIMasterController.Instance.uiScale;
			Vector2 vector2 = new Vector2((float)UIMasterController.Instance.uiCamera.pixelWidth / 2f, (float)UIMasterController.Instance.uiCamera.pixelHeight / 2f) / UIMasterController.Instance.uiScale;
			if (HighLogic.LoadedScene == GameScenes.EDITOR && EditorLogic.RootPart != null)
			{
				rootPartScreenPosition = EditorLogic.fetch.editorCamera.WorldToScreenPoint(EditorLogic.RootPart.transform.position) / UIMasterController.Instance.uiScale;
			}
			else if (HighLogic.LoadedScene == GameScenes.FLIGHT && FlightCamera.fetch.Target != null)
			{
				rootPartScreenPosition = vector2;
			}
			else
			{
				rootPartScreenPosition = new Vector3(vector2.x, vector2.y, 0f);
			}
			bool flag = false;
			if ((Mathf.Abs(vector.x - rootPartScreenPosition.x) < 0.5f) ? (Generator.Next(1, 3) == 2) : ((vector.x > rootPartScreenPosition.x) ? true : false))
			{
				onSidePosition = new Vector3(vector.x + GameSettings.PAW_SCREEN_OFFSET_X, vector.y, 0f);
				vector.x = onSidePosition.x;
			}
			else
			{
				onSidePosition = new Vector3(vector.x - rectTransform.sizeDelta.x - GameSettings.PAW_SCREEN_OFFSET_X, vector.y, 0f);
				vector.x = onSidePosition.x;
			}
			vector.y += rectTransform.sizeDelta.y / 2f * GameSettings.UI_SCALE;
			rectTransform.anchoredPosition = vector;
			UIMasterController.ClampToWindow(UIMasterController.Instance.actionCanvas.GetComponent<RectTransform>(), rectTransform, Vector3.one * screenEdgeOffset);
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		_dragging = true;
	}

	public void OnDrag(PointerEventData eventData)
	{
		UIMasterController.DragTooltip(rectTransform, eventData.delta, Vector3.one * screenEdgeOffset);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		_dragging = false;
	}

	public void OnPin(bool pinned)
	{
		this.pinned = pinned;
		if (Display == DisplayType.ResourceOnly && pinned)
		{
			Display = DisplayType.ResourceSelected;
			PreviousResourceOnly = true;
		}
		else if (PreviousResourceOnly && !pinned)
		{
			Display = DisplayType.ResourceOnly;
			PreviousResourceOnly = false;
		}
	}

	public void OnNumericSwap(bool numeric)
	{
		numericSliders = numeric;
		GameSettings.PAW_NUMERIC_SLIDERS = numeric;
		GameEvents.onPartActionNumericSlider.Fire(numeric);
		GameSettings.SaveSettingsOnNextGameSave();
	}

	public void OnNumericSwapped(bool numeric)
	{
		toggleNumeric.isOn = numeric;
	}

	public bool NumericInputModeAndFocusedItem()
	{
		if (numericSliders && EventSystem.current != null)
		{
			return EventSystem.current.currentSelectedGameObject != null;
		}
		return false;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		hover = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		hover = false;
	}

	public bool GetPartHover()
	{
		if (!(part != null))
		{
			return false;
		}
		return part.MouseOver;
	}
}
