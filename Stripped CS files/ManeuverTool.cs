using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using ns11;
using ns2;
using ns9;
using UnityEngine;
using UnityEngine.EventSystems;

public class ManeuverTool : UIApp
{
	public static ManeuverTool Instance;

	public static bool Ready = false;

	[SerializeField]
	public ManeuverToolUIFrame appFramePrefab;

	public ManeuverToolUIFrame appFrame;

	public RectTransform appRectTransform;

	[SerializeField]
	public int appWidth = 230;

	[SerializeField]
	public int appHeight = 161;

	public DictionaryValueList<string, TransferTypeBase> transferTypes;

	public TransferTypeBase currentSelectedTransferType;

	public bool currentTimeDisplayUT = true;

	public bool currentTimeDisplaySeconds;

	public DictionaryValueList<string, ManeuverNode> positions;

	public bool creatingManNode;

	[SerializeField]
	public GameObject[] transferVisualPrefabs;

	public bool priorManeuversDeleteReq;

	public bool initialHeightSet;

	public bool createAlarm;

	public static Thread mainThread;

	public static object lockObject = new object();

	public static readonly Queue actions = new Queue();

	public bool isMainThread => Thread.CurrentThread == mainThread;

	public void Update()
	{
		lock (lockObject)
		{
			while (actions.Count > 0)
			{
				((Action)actions.Dequeue())();
			}
		}
		if (currentSelectedTransferType != null && currentSelectedTransferType.currentSelectedTransfer != null)
		{
			currentSelectedTransferType.OnUpdate();
		}
		else if (appFrame != null)
		{
			appFrame.UpdateTimeText("");
		}
	}

	public override void OnAppInitialized()
	{
		Instance = this;
		mainThread = Thread.CurrentThread;
		appFrame = UnityEngine.Object.Instantiate(appFramePrefab);
		appFrame.transform.SetParent(base.transform, worldPositionStays: false);
		appFrame.transform.localPosition = Vector3.zero;
		appRectTransform = appFrame.transform as RectTransform;
		appFrame.Setup(base.appLauncherButton, base.name, Localizer.Format("#autoLOC_6002646", appWidth, appHeight), appWidth, appHeight);
		appFrame.AddGlobalInputDelegate(base.MouseInput_PointerEnter, Initial_MouseInput_PointerExit);
		appFrame.maneuverApp = this;
		GenerateTransferTypes();
		PopulateTransferTypes();
		if (appFrame.transferTypeDropDown != null)
		{
			appFrame.transferTypeDropDown.onValueChanged.AddListener(OnTransferTypeChanged);
			appFrame.transferTypeDropDown.value = 0;
			OnTransferTypeChanged(0);
		}
		PopulatePositionList();
		appFrame.positionDropDown.onValueChanged.AddListener(OnPositionChanged);
		GameEvents.onVesselSwitching.Add(OnVesselSwitching);
		GameEvents.onManeuverAdded.Add(OnManeuverChanged);
		GameEvents.onManeuverRemoved.Add(OnManeuverChanged);
		ApplicationLauncher.Instance.AddOnRepositionCallback(Reposition);
		ReadjustPosition();
		HideApp();
		Ready = true;
		GameEvents.onGUIManeuverToolReady.Fire();
	}

	public override void OnAppDestroy()
	{
		if (appFrame != null && appFrame.transferTypeDropDown != null)
		{
			appFrame.transferTypeDropDown.onValueChanged.RemoveListener(OnTransferTypeChanged);
		}
		if ((bool)ApplicationLauncher.Instance)
		{
			ApplicationLauncher.Instance.RemoveOnRepositionCallback(Reposition);
		}
		if (appFrame != null)
		{
			if ((bool)ApplicationLauncher.Instance)
			{
				ApplicationLauncher.Instance.RemoveOnRepositionCallback(appFrame.Reposition);
			}
			appFrame.gameObject.DestroyGameObject();
		}
		Ready = false;
		GameEvents.onGUIManeuverToolDestroy.Fire();
		GameEvents.onVesselSwitching.Remove(OnVesselSwitching);
		GameEvents.onManeuverAdded.Remove(OnManeuverChanged);
		GameEvents.onManeuverRemoved.Remove(OnManeuverChanged);
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public override bool OnAppAboutToStart()
	{
		if (HighLogic.CurrentGame != null && (HighLogic.CurrentGame.Mode == Game.Modes.SCENARIO || HighLogic.CurrentGame.Mode == Game.Modes.SCENARIO_NON_RESUMABLE))
		{
			return false;
		}
		if (GameVariables.Instance.ManeuverToolAvailable(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.TrackingStation)) && GameVariables.Instance.UnlockedFlightPlanning(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.MissionControl)))
		{
			return HighLogic.LoadedSceneIsFlight;
		}
		return false;
	}

	public override void DisplayApp()
	{
		if (appFrame != null)
		{
			if (!initialHeightSet)
			{
				appRectTransform.sizeDelta = new Vector2(appWidth, appHeight);
				initialHeightSet = true;
			}
			appFrame.gameObject.SetActive(value: true);
			if (currentSelectedTransferType != null)
			{
				currentSelectedTransferType.DrawTypeControls(appFrame, TransferTypeBase.DrawReason.AppShown);
			}
			if (appFrame.TopPanelTransitioning)
			{
				appFrame.TransitionTopPanelImmediate();
			}
			if (appFrame.VisualPanelTransitioning)
			{
				appFrame.TransitionVisualPanelImmediate();
			}
		}
	}

	public override void HideApp()
	{
		if (currentSelectedTransferType != null)
		{
			currentSelectedTransferType.HideApp();
		}
		if (appFrame != null)
		{
			if (appFrame.TopPanelTransitioning)
			{
				appFrame.TransitionTopPanelImmediate();
			}
			if (appFrame.VisualPanelTransitioning)
			{
				appFrame.TransitionVisualPanelImmediate();
			}
			appFrame.gameObject.SetActive(value: false);
		}
	}

	public override ApplicationLauncher.AppScenes GetAppScenes()
	{
		if (GameVariables.Instance.ManeuverToolAvailable(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.TrackingStation)))
		{
			return ApplicationLauncher.AppScenes.FLIGHT | ApplicationLauncher.AppScenes.MAPVIEW;
		}
		return ApplicationLauncher.AppScenes.NEVER;
	}

	public override Vector3 GetAppScreenPos(Vector3 defaultAnchorPos)
	{
		return defaultAnchorPos;
	}

	public override void Reposition()
	{
		appFrame.Reposition();
		ReadjustPosition();
	}

	public void ReadjustPosition()
	{
		appRectTransform.anchoredPosition = new Vector2(appRectTransform.anchoredPosition.x, -180f);
	}

	public void Initial_MouseInput_PointerExit(PointerEventData eventData)
	{
		if (!IsAnyDropdownOpen())
		{
			MouseInput_PointerExit(eventData);
		}
	}

	public GameObject GetTransferVisualPrefab(string name)
	{
		int num = 0;
		while (true)
		{
			if (num < transferVisualPrefabs.Length)
			{
				if (transferVisualPrefabs[num].name == name)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return transferVisualPrefabs[num];
	}

	public void OnManeuverChanged(Vessel vsl, PatchedConicSolver solver)
	{
		if (!creatingManNode && vsl.persistentId == FlightGlobals.ActiveVessel.persistentId)
		{
			PopulatePositionList();
			if (currentSelectedTransferType != null)
			{
				currentSelectedTransferType.DrawTypeControls(appFrame, TransferTypeBase.DrawReason.ManeuverChanged);
			}
		}
	}

	public void OnVesselSwitching(Vessel oldVsl, Vessel newVsl)
	{
		PopulatePositionList();
		if (currentSelectedTransferType != null)
		{
			currentSelectedTransferType.DrawTypeControls(appFrame, TransferTypeBase.DrawReason.VesselChanged);
		}
	}

	public void GenerateTransferTypes()
	{
		if (transferTypes != null)
		{
			return;
		}
		transferTypes = new DictionaryValueList<string, TransferTypeBase>();
		AssemblyLoader.loadedAssemblies.TypeOperation(delegate(Type t)
		{
			if (t.IsSubclassOf(typeof(TransferTypeBase)) && !(t == typeof(TransferTypeBase)))
			{
				TransferTypeBase transferTypeBase = (TransferTypeBase)Activator.CreateInstance(t);
				transferTypes.Add(transferTypeBase.DisplayName(), transferTypeBase);
			}
		});
		Debug.Log("[ManeuverTool]: Found " + transferTypes.Count + " transfer types");
	}

	public void PopulateTransferTypes()
	{
		if (!(appFrame != null) || !(appFrame.transferTypeDropDown != null))
		{
			return;
		}
		appFrame.transferTypeDropDown.ClearOptions();
		List<string> list = new List<string>();
		for (int i = 0; i < transferTypes.ValuesList.Count; i++)
		{
			string text = transferTypes.ValuesList[i].DisplayName();
			if (!string.IsNullOrEmpty(text))
			{
				list.Add(text);
			}
		}
		appFrame.transferTypeDropDown.AddOptions(list);
		if (appFrame.transferTypeDropDown.value >= list.Count)
		{
			appFrame.transferTypeDropDown.value = list.Count - 1;
		}
		if (transferTypes.ValuesList.Count <= 1 && appFrame.transferTypeParent != null)
		{
			appFrame.transferTypeParent.showGuiName = false;
			appFrame.transferTypeParent.gameObject.SetActive(value: false);
		}
	}

	public void PopulatePositionList()
	{
		if (appFrame != null && appFrame.positionDropDown != null && FlightGlobals.ActiveVessel != null && FlightGlobals.ActiveVessel.patchedConicSolver != null)
		{
			appFrame.positionDropDown.ClearOptions();
			positions = new DictionaryValueList<string, ManeuverNode>();
			List<string> list = new List<string>();
			positions.Add(Localizer.Format("#autoLOC_6002647"), null);
			list.Add(Localizer.Format("#autoLOC_6002647"));
			for (int i = 0; i < FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes.Count; i++)
			{
				string text = Localizer.Format("#autoLOC_6100053", Localizer.Format("#autoLOC_198614"), (i + 1).ToString()).ToUpper();
				positions.Add(text, FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes[i]);
				list.Add(text);
			}
			appFrame.positionDropDown.AddOptions(list);
			if (appFrame.positionDropDown.value >= list.Count)
			{
				appFrame.positionDropDown.value = list.Count - 1;
			}
			appFrame.positionDropDown.value = 0;
			OnPositionChanged(0);
		}
	}

	public void OnTransferTypeChanged(int itemIndex)
	{
		if (appFrame != null && transferTypes.ContainsKey(appFrame.transferTypeDropDown.options[itemIndex].text))
		{
			currentSelectedTransferType = transferTypes[appFrame.transferTypeDropDown.options[itemIndex].text];
			currentSelectedTransferType.DrawTypeControls(appFrame, TransferTypeBase.DrawReason.TransferTypeChanged);
			appFrame.SetHeaderText(currentSelectedTransferType.DisplayName());
			appFrame.UpdateDVText("");
			appFrame.UpdateTimeText("");
		}
	}

	public void OnPositionChanged(int itemIndex)
	{
		if (!(appFrame != null) || currentSelectedTransferType == null)
		{
			return;
		}
		string text = appFrame.positionDropDown.options[itemIndex].text;
		if (positions.ContainsKey(text))
		{
			string errorString = "";
			appFrame.SetErrorState(state: false, errorString, currentSelectedTransferType.currentSelectedTransfer, toggleInputPanel: false, currentSelectedTransferType.UpdateTransferData);
			if (currentSelectedTransferType.ChangePosition(positions[text], out errorString))
			{
				appFrame.SetCreateManeuverButton(interactable: true);
				appFrame.SetErrorState(state: false, errorString, currentSelectedTransferType.currentSelectedTransfer, toggleInputPanel: false, currentSelectedTransferType.UpdateTransferData);
			}
			else
			{
				appFrame.SetCreateManeuverButton(interactable: false);
				appFrame.SetErrorState(state: true, errorString, currentSelectedTransferType.currentSelectedTransfer, toggleInputPanel: false, currentSelectedTransferType.UpdateTransferData);
			}
		}
	}

	public void ChangeTime(bool bool_0, bool seconds)
	{
		currentTimeDisplayUT = bool_0;
		currentTimeDisplaySeconds = seconds;
	}

	public void CreateManeuver(bool createAlarm)
	{
		if (!InputLockManager.IsUnlocked(ControlTypes.MANNODE_ADDEDIT) && Application.isFocused)
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_465476"), 3f, ScreenMessageStyle.UPPER_CENTER);
			return;
		}
		priorManeuversDeleteReq = false;
		this.createAlarm = createAlarm;
		if (currentSelectedTransferType != null)
		{
			creatingManNode = true;
			appFrame.SetCreateManeuverButton(interactable: false);
			if (!currentSelectedTransferType.PriorManeuversCheck(currentSelectedTransferType.currentSelectedTransfer, OnPriorCheckOk, OnPriorCheckCancel))
			{
				FollowingManevuersCheck();
			}
		}
	}

	public void OnPriorCheckOk()
	{
		priorManeuversDeleteReq = true;
		FollowingManevuersCheck();
	}

	public void OnPriorCheckCancel()
	{
		creatingManNode = false;
		appFrame.SetCreateManeuverButton(interactable: true);
	}

	public void FollowingManevuersCheck()
	{
		if (!currentSelectedTransferType.FollowingManeuversCheck(currentSelectedTransferType.currentSelectedTransfer, OnFollowingCheckOk, OnFollowingLeave, OnFollowingCheckCancel))
		{
			if (priorManeuversDeleteReq)
			{
				DeletePriorManeuvers();
			}
			CreateManeuverChecksComplete();
		}
	}

	public void OnFollowingCheckOk()
	{
		if (priorManeuversDeleteReq)
		{
			DeletePriorManeuvers();
		}
		int count = currentSelectedTransferType.currentSelectedTransfer.vessel.patchedConicSolver.maneuverNodes.Count;
		while (count-- > 0)
		{
			if (currentSelectedTransferType.currentSelectedTransfer.vessel.patchedConicSolver.maneuverNodes[count].double_0 > currentSelectedTransferType.currentSelectedTransfer.startBurnTime)
			{
				currentSelectedTransferType.currentSelectedTransfer.vessel.patchedConicSolver.maneuverNodes[count].RemoveSelf();
			}
			currentSelectedTransferType.currentSelectedTransfer.vessel.patchedConicSolver.UpdateFlightPlan();
		}
		CreateManeuverChecksComplete();
	}

	public void OnFollowingLeave()
	{
		CreateManeuverChecksComplete();
	}

	public void OnFollowingCheckCancel()
	{
		creatingManNode = false;
		appFrame.SetCreateManeuverButton(interactable: true);
	}

	public void CreateManeuverChecksComplete()
	{
		string alarmTitle = "";
		string alarmDesc = "";
		if (currentSelectedTransferType.CreateManeuver(out var newNode, out alarmTitle, out alarmDesc) && createAlarm)
		{
			AlarmTypeManeuver obj = new AlarmTypeManeuver
			{
				Maneuver = newNode
			};
			Vessel vessel = currentSelectedTransferType.currentSelectedTransfer.vessel;
			obj.vesselId = vessel.persistentId;
			obj.title = alarmTitle;
			obj.description = alarmDesc;
			AlarmClockScenario.AddAlarm(obj);
		}
		appFrame.SetCreateManeuverButton(interactable: true);
		creatingManNode = false;
		OnPositionChanged(appFrame.positionDropDown.value);
	}

	public bool IsAnyDropdownOpen()
	{
		if (appFrame.positionDropDown.IsExpanded)
		{
			return true;
		}
		if (currentSelectedTransferType.currentSelectedTransfer.IsAnyDropdownOpen(appFrame.inputPanel))
		{
			return true;
		}
		return false;
	}

	public void DeletePriorManeuvers()
	{
		int count = currentSelectedTransferType.currentSelectedTransfer.vessel.patchedConicSolver.maneuverNodes.Count;
		while (count-- > 0)
		{
			if (count >= currentSelectedTransferType.currentSelectedTransfer.positionNodeIdx && currentSelectedTransferType.currentSelectedTransfer.vessel.patchedConicSolver.maneuverNodes[count].double_0 < currentSelectedTransferType.currentSelectedTransfer.startBurnTime)
			{
				currentSelectedTransferType.currentSelectedTransfer.vessel.patchedConicSolver.maneuverNodes[count].RemoveSelf();
			}
			currentSelectedTransferType.currentSelectedTransfer.vessel.patchedConicSolver.UpdateFlightPlan();
		}
	}

	public void InvokeAsync(Action action)
	{
		if (isMainThread)
		{
			action();
			return;
		}
		lock (lockObject)
		{
			actions.Enqueue(action);
		}
	}
}
