using System;
using System.Collections;
using System.Collections.Generic;
using ns11;
using ns2;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPartActionController : MonoBehaviour
{
	public UIPartActionWindow windowPrefab;

	public List<UIPartActionFieldItem> fieldPrefabs;

	public List<Type> fieldControlTypes;

	public UIPartActionGroup groupPrefab;

	public UIPartActionButton eventItemPrefab;

	public UIPartActionResource resourceItemPrefab;

	public UIPartActionResourceTransfer resourceTransferItemPrefab;

	public UIPartActionResourceEditor resourceItemEditorPrefab;

	public UIPartActionResourcePriority resourcePriorityPrefab;

	public UIPartActionFuelFlowOverlay fuelFlowOverlayPrefab;

	public UIPartActionAeroDisplay debugAeroItemPrefab;

	public UIPartActionThermalDisplay debugThermalItemPrefab;

	public UIPartActionRoboticJointDisplay debugRoboticJointItemPrefab;

	public float zMin;

	public bool guiActive;

	public List<UIPartActionWindow> windows;

	public List<UIPartActionWindow> hiddenWindows;

	public List<UIPartActionWindow> hiddenResourceWindows;

	public bool allowControl = true;

	public UIPartActionControllerInventory partInventory;

	public bool showWindows = true;

	public bool isClicking;

	[SerializeField]
	public List<int> _resourcesShown = new List<int>();

	public bool resourcesShownDirty;

	public List<UIPartActionResourceTransfer> transfers = new List<UIPartActionResourceTransfer>();

	public static UIPartActionController Instance { get; set; }

	public List<int> resourcesShown => _resourcesShown;

	public void Awake()
	{
		if (Instance != null)
		{
			Debug.Log("Can only have one UIPartActionController in the scene!");
			UnityEngine.Object.DestroyImmediate(base.gameObject);
		}
		else
		{
			Instance = this;
		}
	}

	public void Start()
	{
		windows = new List<UIPartActionWindow>();
		hiddenWindows = new List<UIPartActionWindow>();
		hiddenResourceWindows = new List<UIPartActionWindow>();
		ResourceDisplay.AddOnShowResource(OnShowResource);
		ResourceDisplay.AddOnHideResource(OnHideResource);
		SetupItemControls();
		GameEvents.OnCameraChange.Add(OnCameraChange);
		showWindows = true;
	}

	public void OnCameraChange(CameraManager.CameraMode mode)
	{
		showWindows = mode != CameraManager.CameraMode.const_3 && mode != CameraManager.CameraMode.Internal && mode != CameraManager.CameraMode.Map;
		int count = windows.Count;
		while (count-- > 0)
		{
			if (windows[count].gameObject.activeSelf != showWindows)
			{
				windows[count].gameObject.SetActive(showWindows);
			}
		}
	}

	public void OnDestroy()
	{
		int count = windows.Count;
		while (count-- > 0)
		{
			GameEvents.onPartActionUIDismiss.Fire(windows[count].part);
			if (windows[count] != null && windows[count].gameObject != null)
			{
				UnityEngine.Object.DestroyImmediate(windows[count].gameObject);
			}
			windows.RemoveAt(count);
		}
		int count2 = hiddenWindows.Count;
		while (count2-- > 0)
		{
			if (hiddenWindows[count2] != null && hiddenWindows[count2].gameObject != null)
			{
				UnityEngine.Object.DestroyImmediate(hiddenWindows[count2].gameObject);
			}
			hiddenWindows.RemoveAt(count2);
		}
		int count3 = hiddenResourceWindows.Count;
		while (count3-- > 0)
		{
			if (hiddenResourceWindows[count3] != null && hiddenResourceWindows[count3].gameObject != null)
			{
				UnityEngine.Object.DestroyImmediate(hiddenResourceWindows[count3].gameObject);
			}
			hiddenResourceWindows.RemoveAt(count3);
		}
		ResourceDisplay.RemoveOnShowResource(OnShowResource);
		ResourceDisplay.RemoveOnHideResource(OnHideResource);
		GameEvents.OnCameraChange.Remove(OnCameraChange);
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void SetupItemControls()
	{
		fieldControlTypes = new List<Type>(fieldPrefabs.Count);
		int count = fieldPrefabs.Count;
		while (count-- > 0)
		{
			UIPartActionItem uIPartActionItem = fieldPrefabs[count];
			UI_Control[] array = (UI_Control[])uIPartActionItem.GetType().GetCustomAttributes(typeof(UI_Control), inherit: false);
			if (array != null && array.Length != 0)
			{
				Type type = array[0].GetType();
				if (fieldControlTypes.Contains(type))
				{
					Debug.LogError("ItemPrefab '" + uIPartActionItem.name + "' has same UI_Control type as '" + fieldPrefabs[fieldControlTypes.IndexOf(type)].name + "'");
					fieldPrefabs.RemoveAt(count);
				}
				else
				{
					fieldControlTypes.Insert(0, type);
				}
			}
			else
			{
				Debug.LogError("ItemPrefab '" + uIPartActionItem.name + "' has no UI_Control defined");
				fieldPrefabs.RemoveAt(count);
			}
		}
	}

	public UIPartActionFieldItem GetFieldControl(Type uiControlType)
	{
		int num = 0;
		int count = fieldControlTypes.Count;
		while (true)
		{
			if (num < count)
			{
				if (uiControlType == fieldControlTypes[num])
				{
					break;
				}
				num++;
				continue;
			}
			Debug.LogError("ItemPrefab for control type '" + uiControlType.Name + "' not found.");
			return null;
		}
		return fieldPrefabs[num];
	}

	public UIPartActionItem GetControl(Type uiControlType)
	{
		int num = 0;
		int count = fieldControlTypes.Count;
		while (true)
		{
			if (num < count)
			{
				if (uiControlType == fieldControlTypes[num])
				{
					break;
				}
				num++;
				continue;
			}
			Debug.LogError("ItemPrefab for control type '" + uiControlType.Name + "' not found.");
			return null;
		}
		return fieldPrefabs[num];
	}

	public void Update()
	{
		partInventory.UpdateCursorOverPAWs();
		if (HighLogic.LoadedSceneIsFlight)
		{
			UpdateFlight();
		}
		else if (HighLogic.LoadedSceneIsEditor)
		{
			UpdateEditor();
		}
	}

	public void UpdateFlight()
	{
		if (!(FlightDriver.fetch == null) && !FlightDriver.Pause && allowControl)
		{
			if (showWindows)
			{
				TrySelect(FlightCamera.fetch.mainCamera, allowMultiple: true);
			}
			if (GameSettings.PAUSE.GetKeyUp())
			{
				partInventory.ReturnHeldPart();
			}
			UpdateResourceWindows();
			UpdateActiveWindows();
			partInventory.UpdateCurrentCargoPart();
		}
	}

	public void UpdateEditor()
	{
		if (EditorLogic.fetch == null || EditorCamera.Instance == null)
		{
			return;
		}
		partInventory.UpdateSelectedPartVisibility();
		if (EditorLogic.SelectedPart != null)
		{
			Deselect(resourcesToo: true);
			return;
		}
		TrySelect(EditorCamera.Instance.GetComponent<Camera>(), allowMultiple: false);
		if (PhysicsGlobals.AeroDataDisplay && windows.Count > 0 && EditorLogic.fetch.ship != null && EditorLogic.fetch.ship.parts.Count > 0)
		{
			EditorLogic.fetch.ship.parts[0].DragCubes.ForceUpdate(weights: true, occlusion: true);
			EditorLogic.fetch.ship.parts[0].DragCubes.SetDragWeights();
			EditorLogic.fetch.ship.parts[0].DragCubes.SetPartOcclusion();
			for (int i = 1; i < EditorLogic.fetch.ship.parts.Count; i++)
			{
				if (!EditorLogic.fetch.ship.parts[i].DragCubes.None)
				{
					EditorLogic.fetch.ship.parts[i].DragCubes.ForceUpdate(weights: true, occlusion: true);
					EditorLogic.fetch.ship.parts[i].DragCubes.SetDragWeights();
					EditorLogic.fetch.ship.parts[i].DragCubes.SetPartOcclusion();
				}
			}
		}
		UpdateActiveWindows();
	}

	public void UpdateActiveWindows()
	{
		if (windows.Count <= 0)
		{
			return;
		}
		transfers.Clear();
		int count = windows.Count;
		while (count-- > 0)
		{
			windows[count].UpdateWindow();
		}
		int count2 = windows.Count;
		while (count2-- > 0)
		{
			if (!windows[count2].isValid)
			{
				GameEvents.onPartActionUIDismiss.Fire(windows[count2].part);
				if (windows[count2].Display == UIPartActionWindow.DisplayType.ResourceOnly)
				{
					hiddenResourceWindows.Add(windows[count2]);
					windows[count2].gameObject.SetActive(value: false);
				}
				else
				{
					UnityEngine.Object.DestroyImmediate(windows[count2].gameObject);
				}
				windows.RemoveAt(count2);
			}
		}
		if (windows.Count == 0)
		{
			Deselect(resourcesToo: false);
			return;
		}
		int count3 = windows.Count;
		while (count3-- > 0)
		{
			if (windows[count3].gameObject.activeSelf != showWindows)
			{
				windows[count3].gameObject.SetActive(showWindows);
			}
		}
	}

	public void UpdateResourceWindows()
	{
		if (!resourcesShownDirty)
		{
			return;
		}
		int i = 0;
		for (int count = FlightGlobals.ActiveVessel.parts.Count; i < count; i++)
		{
			Part part = FlightGlobals.ActiveVessel.parts[i];
			UIPartActionWindow item = GetItem(part, includeSymmetryCounterparts: false);
			if (item != null)
			{
				bool flag = false;
				int j = 0;
				for (int count2 = part.Resources.Count; j < count2; j++)
				{
					PartResource partResource = part.Resources[j];
					if (resourcesShown.Contains(partResource.info.id))
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					if (item.Display == UIPartActionWindow.DisplayType.Selected)
					{
						item.Display = UIPartActionWindow.DisplayType.ResourceSelected;
					}
				}
				else if (item.PreviousResourceOnly && item.Pinned)
				{
					item.Display = UIPartActionWindow.DisplayType.ResourceSelected;
				}
				item.displayDirty = true;
				continue;
			}
			int k = 0;
			for (int count3 = part.Resources.Count; k < count3; k++)
			{
				PartResource partResource2 = part.Resources[k];
				if (resourcesShown.Contains(partResource2.info.id))
				{
					item = HiddenResourceItemListGet(part);
					if (item != null)
					{
						item.isValid = true;
						item.gameObject.SetActive(value: true);
						hiddenResourceWindows.Remove(item);
						windows.Add(item);
						item.TrackPartPosition();
						GameEvents.onPartActionUIShown.Fire(item, part);
					}
					else
					{
						item = CreatePartUI(part, UIPartActionWindow.DisplayType.ResourceOnly, UI_Scene.Flight);
					}
					if (item != null)
					{
						item.displayDirty = true;
					}
					break;
				}
			}
		}
		resourcesShownDirty = false;
	}

	public void TrySelect(Camera cam, bool allowMultiple)
	{
		if (Input.GetMouseButtonDown(1) && !isClicking && Cursor.lockState != CursorLockMode.Locked && !EventSystem.current.IsPointerOverGameObject())
		{
			StartCoroutine(MouseClickCoroutine(cam, allowMultiple));
		}
	}

	public IEnumerator MouseClickCoroutine(Camera cam, bool allowMultiple)
	{
		isClicking = true;
		float time = Time.realtimeSinceStartup + 0.2f;
		while (!(Time.realtimeSinceStartup >= time))
		{
			if (Input.GetMouseButtonUp(1) && Cursor.lockState != CursorLockMode.Locked)
			{
				HandleMouseClick(cam, allowMultiple);
				break;
			}
			yield return null;
		}
		isClicking = false;
	}

	public void HandleMouseClick(Camera cam, bool allowMultiple)
	{
		if (!EventSystem.current.IsPointerOverGameObject())
		{
			Part hoveredPart = Mouse.HoveredPart;
			if (hoveredPart != null)
			{
				SelectPart(hoveredPart, allowMultiple, overrideSymmetry: false);
			}
			else if (!GetSelectionModifier())
			{
				Deselect(resourcesToo: false);
			}
		}
	}

	public void SpawnPartActionWindow(Part part)
	{
		SelectPart(part, HighLogic.LoadedSceneIsFlight ? true : false, overrideSymmetry: false);
	}

	public void SpawnPartActionWindow(Part part, bool overrideSymmetry)
	{
		SelectPart(part, HighLogic.LoadedSceneIsFlight ? true : false, overrideSymmetry);
	}

	public void SelectPart(Part part, bool allowMultiple, bool overrideSymmetry)
	{
		if (part == null)
		{
			Deselect(resourcesToo: false);
			return;
		}
		bool selectionModifier = GetSelectionModifier();
		if (!allowMultiple || !selectionModifier)
		{
			Deselect(resourcesToo: false);
		}
		if (ItemListContains(part, !selectionModifier) && !overrideSymmetry)
		{
			UIPartActionWindow item = GetItem(part, includeSymmetryCounterparts: false);
			if (item != null && item.Display == UIPartActionWindow.DisplayType.ResourceOnly)
			{
				item.Display = UIPartActionWindow.DisplayType.ResourceSelected;
				item.PreviousResourceOnly = true;
			}
			return;
		}
		UIPartActionWindow uIPartActionWindow = HiddenItemListGet(part);
		if (uIPartActionWindow != null)
		{
			uIPartActionWindow.gameObject.SetActive(value: true);
			hiddenWindows.Remove(uIPartActionWindow);
			windows.Add(uIPartActionWindow);
			if (partInventory != null && partInventory.referenceNextCreatedInventoryPAW)
			{
				partInventory.CurrentPawAutoOpened = uIPartActionWindow;
				partInventory.referenceNextCreatedInventoryPAW = false;
				partInventory.currentPawAutoOpening = true;
				if (partInventory.CurrentInventoryOnlyIcon != null)
				{
					partInventory.CurrentInventoryOnlyIcon.transform.SetAsLastSibling();
				}
			}
			uIPartActionWindow.TrackPartPosition();
			GameEvents.onPartActionUIShown.Fire(uIPartActionWindow, part);
		}
		else
		{
			uIPartActionWindow = CreatePartUI(part, UIPartActionWindow.DisplayType.Selected, (!HighLogic.LoadedSceneIsFlight) ? UI_Scene.Editor : UI_Scene.Flight);
		}
		UIPartActionResourceTransfer control = null;
		if (!(uIPartActionWindow != null) || !uIPartActionWindow.ContainsResourceTransferControl(null, out control))
		{
			return;
		}
		int i = 0;
		for (int count = windows.Count; i < count; i++)
		{
			UIPartActionResourceTransfer control2 = null;
			if (windows[i].ContainsResourceTransferControl(control.Resource, out control2))
			{
				windows[i].RefreshResourceTransferTargets();
			}
			else if (windows[i].ContainsResourceControl(control.Resource))
			{
				windows[i].CreatePartList(clearFirst: true);
			}
		}
	}

	public bool GetSelectionModifier()
	{
		return GameSettings.MODIFIER_KEY.GetKey();
	}

	public void Deselect(bool resourcesToo)
	{
		if (resourcesToo)
		{
			int count = windows.Count;
			while (count-- > 0)
			{
				if (!windows[count].Pinned && !PartWindowIsFromAnInventory(windows[count].part))
				{
					GameEvents.onPartActionUIDismiss.Fire(windows[count].part);
					windows[count].gameObject.SetActive(value: false);
					hiddenWindows.Add(windows[count]);
					windows.RemoveAt(count);
				}
			}
			return;
		}
		int count2 = windows.Count;
		while (count2-- > 0)
		{
			if (windows[count2].Display == UIPartActionWindow.DisplayType.ResourceSelected)
			{
				if (windows[count2].PreviousResourceOnly)
				{
					windows[count2].Display = UIPartActionWindow.DisplayType.ResourceOnly;
					windows[count2].PreviousResourceOnly = false;
				}
				else
				{
					windows[count2].Display = UIPartActionWindow.DisplayType.Selected;
				}
			}
			else if (windows[count2].Display != UIPartActionWindow.DisplayType.ResourceOnly && !windows[count2].Pinned)
			{
				if (!PartWindowIsFromAnInventory(windows[count2].part))
				{
					GameEvents.onPartActionUIDismiss.Fire(windows[count2].part);
					windows[count2].gameObject.SetActive(value: false);
					hiddenWindows.Add(windows[count2]);
					windows.RemoveAt(count2);
				}
				else if (partInventory != null && partInventory.CurrentCargoPart == null)
				{
					GameEvents.onPartActionUIDismiss.Fire(windows[count2].part);
					windows[count2].gameObject.SetActive(value: false);
					hiddenWindows.Add(windows[count2]);
					windows.RemoveAt(count2);
				}
			}
		}
	}

	public void Show(bool show)
	{
		showWindows = show;
	}

	public bool PartWindowIsFromAnInventory(Part p)
	{
		if (partInventory != null)
		{
			if (p.vessel != null && (p.vessel.isEVA || p.protoModuleCrew.Count > 0) && (bool)p.GetComponent<ModuleInventoryPart>())
			{
				return true;
			}
			if (partInventory.availableInventoryParts != null)
			{
				return partInventory.availableInventoryParts.Contains(p.partInfo.name);
			}
			return false;
		}
		return false;
	}

	public UIPartActionWindow CreatePartUI(Part part, UIPartActionWindow.DisplayType type, UI_Scene scene)
	{
		if (part == null)
		{
			return null;
		}
		UIPartActionWindow uIPartActionWindow = UnityEngine.Object.Instantiate(windowPrefab);
		uIPartActionWindow.transform.SetParent(UIMasterController.Instance.actionCanvas.transform, worldPositionStays: false);
		uIPartActionWindow.gameObject.SetActive(value: true);
		windows.Add(uIPartActionWindow);
		if (partInventory != null && partInventory.referenceNextCreatedInventoryPAW)
		{
			partInventory.CurrentPawAutoOpened = uIPartActionWindow;
			partInventory.referenceNextCreatedInventoryPAW = false;
			partInventory.currentPawAutoOpening = true;
			if (partInventory.CurrentInventoryOnlyIcon != null)
			{
				partInventory.CurrentInventoryOnlyIcon.transform.SetAsLastSibling();
			}
		}
		if (uIPartActionWindow.Setup(part, type, scene))
		{
			uIPartActionWindow.gameObject.name = part.name;
			return uIPartActionWindow;
		}
		windows.Remove(uIPartActionWindow);
		UnityEngine.Object.DestroyImmediate(uIPartActionWindow.gameObject);
		uIPartActionWindow = null;
		return null;
	}

	public bool ItemListContains(Part part, bool includeSymmetryCounterparts)
	{
		if (windows == null)
		{
			return false;
		}
		int num = 0;
		int count = windows.Count;
		while (true)
		{
			if (num < count)
			{
				UIPartActionWindow uIPartActionWindow = windows[num];
				if (!(uIPartActionWindow.part == part))
				{
					if (includeSymmetryCounterparts && !uIPartActionWindow.Pinned && uIPartActionWindow.part.symmetryCounterparts.Contains(part))
					{
						break;
					}
					num++;
					continue;
				}
				return true;
			}
			return false;
		}
		return true;
	}

	public bool ItemListContainsCounterparts(Part part)
	{
		if (windows == null)
		{
			return false;
		}
		int num = 0;
		int count = windows.Count;
		while (true)
		{
			if (num < count)
			{
				UIPartActionWindow uIPartActionWindow = windows[num];
				if (uIPartActionWindow.part != part && part.symmetryCounterparts.Contains(uIPartActionWindow.part))
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

	public UIPartActionWindow GetItem(Part part, bool includeSymmetryCounterparts = true)
	{
		if (windows == null)
		{
			return null;
		}
		int num = 0;
		int count = windows.Count;
		UIPartActionWindow uIPartActionWindow;
		while (true)
		{
			if (num < count)
			{
				uIPartActionWindow = windows[num];
				if (!(uIPartActionWindow.part == part))
				{
					if (includeSymmetryCounterparts && uIPartActionWindow.part.symmetryCounterparts.Contains(part))
					{
						break;
					}
					num++;
					continue;
				}
				return uIPartActionWindow;
			}
			return null;
		}
		return uIPartActionWindow;
	}

	public UIPartActionWindow ItemListGet(Part part)
	{
		if (windows == null)
		{
			return null;
		}
		int num = 0;
		int count = windows.Count;
		UIPartActionWindow uIPartActionWindow;
		while (true)
		{
			if (num < count)
			{
				uIPartActionWindow = windows[num];
				if (uIPartActionWindow.part == part)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return uIPartActionWindow;
	}

	public UIPartActionWindow HiddenItemListGet(Part part)
	{
		if (hiddenWindows == null)
		{
			return null;
		}
		int num = 0;
		int count = hiddenWindows.Count;
		UIPartActionWindow uIPartActionWindow;
		while (true)
		{
			if (num < count)
			{
				uIPartActionWindow = hiddenWindows[num];
				if (uIPartActionWindow.part == part)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return uIPartActionWindow;
	}

	public UIPartActionWindow HiddenResourceItemListGet(Part part)
	{
		if (hiddenResourceWindows == null)
		{
			return null;
		}
		int num = 0;
		int count = hiddenResourceWindows.Count;
		UIPartActionWindow uIPartActionWindow;
		while (true)
		{
			if (num < count)
			{
				uIPartActionWindow = hiddenResourceWindows[num];
				if (uIPartActionWindow.part == part)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return uIPartActionWindow;
	}

	public void Deactivate()
	{
		allowControl = false;
	}

	public void Activate()
	{
		allowControl = true;
		resourcesShownDirty = true;
	}

	public void OnShowResource(ResourceItem item)
	{
		if (resourcesShown.Contains(item.resourceID))
		{
			return;
		}
		resourcesShown.Add(item.resourceID);
		for (int i = 0; i < windows.Count; i++)
		{
			if (windows[i].Pinned && windows[i].Display == UIPartActionWindow.DisplayType.ResourceSelected && windows[i].PreviousResourceOnly)
			{
				windows[i].Display = UIPartActionWindow.DisplayType.ResourceOnly;
				windows[i].PreviousResourceOnly = false;
			}
		}
		resourcesShownDirty = true;
	}

	public void OnHideResource(ResourceItem item)
	{
		if (!resourcesShown.Contains(item.resourceID))
		{
			return;
		}
		resourcesShown.Remove(item.resourceID);
		for (int i = 0; i < windows.Count; i++)
		{
			if (windows[i].Pinned && windows[i].Display == UIPartActionWindow.DisplayType.ResourceOnly)
			{
				windows[i].Display = UIPartActionWindow.DisplayType.ResourceSelected;
				windows[i].PreviousResourceOnly = true;
			}
		}
		resourcesShownDirty = true;
	}

	public bool ShowTransfers(PartResource res)
	{
		int id = res.info.id;
		if (windows.Count < 2)
		{
			return false;
		}
		if (!GameVariables.Instance.UnlockedFuelTransfer(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.ResearchAndDevelopment)))
		{
			return false;
		}
		int count = windows.Count;
		do
		{
			if (count-- <= 0)
			{
				return true;
			}
		}
		while (windows[count].part.Resources.Contains(id) && (!HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().ResourceTransferObeyCrossfeed || res.part.CanCrossfeed(windows[count].part, id)));
		return false;
	}

	public void SetupResourceTransfer(UIPartActionResourceTransfer trans)
	{
		int i = 0;
		for (int count = transfers.Count; i < count; i++)
		{
			UIPartActionResourceTransfer uIPartActionResourceTransfer = transfers[i];
			if (uIPartActionResourceTransfer != null && uIPartActionResourceTransfer.Resource != null && trans != null && trans.Resource != null && uIPartActionResourceTransfer.Resource.info.id == trans.Resource.info.id && !(uIPartActionResourceTransfer.Part == trans.Part))
			{
				if (!uIPartActionResourceTransfer.Targets.Contains(trans))
				{
					uIPartActionResourceTransfer.Targets.Add(trans);
				}
				if (!trans.Targets.Contains(uIPartActionResourceTransfer))
				{
					trans.Targets.Add(uIPartActionResourceTransfer);
				}
			}
		}
		if (!transfers.Contains(trans))
		{
			transfers.Add(trans);
		}
	}

	public bool InventoryAndCargoPartExist()
	{
		if (partInventory != null && partInventory.CurrentCargoPart != null)
		{
			return true;
		}
		return false;
	}
}
