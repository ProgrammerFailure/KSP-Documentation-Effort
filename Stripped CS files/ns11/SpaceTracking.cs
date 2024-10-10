using System;
using System.Collections;
using System.Collections.Generic;
using CommNet;
using Expansions;
using Expansions.Missions;
using Expansions.Missions.Actions;
using Expansions.Missions.Editor;
using Expansions.Missions.Flow;
using Expansions.Missions.Runtime;
using FinePrint;
using ns12;
using ns2;
using ns23;
using ns8;
using ns9;
using UnityEngine;
using UnityEngine.UI;

namespace ns11;

public class SpaceTracking : MonoBehaviour
{
	public class MissionObjective
	{
		public Waypoint waypoint;

		public MENode node;

		public MapNode mapNode;

		public MissionOrbitRenderer missionOrbitRenderer;

		public CelestialBody body;
	}

	public static SpaceTracking Instance;

	public Button LeaveBtn;

	public Button FlyButton;

	public Button DeleteButton;

	public Button RecoverButton;

	public Button TrackButton;

	public TrackingStationWidget listItemPrefab;

	public Transform listContainer;

	public ToggleGroup listToggleGroup;

	public List<TrackingStationWidget> vesselWidgets;

	public Toggle tglTrackedObjects;

	public Toggle tglMissionObjectives;

	public ToggleGroup tabsToggleGroup;

	public Transform missionsListContainer;

	public ToggleGroup missionsListToggleGroup;

	[SerializeField]
	public GameObject TimeWarpWidget;

	[SerializeField]
	public GameObject TimeScrubberWidget;

	[SerializeField]
	public ScrollRect sideBarScrollRect;

	public Game st;

	public List<Vessel> trackedVessels;

	[SerializeField]
	public Dictionary<Vessel, MapObject> scaledTargets;

	public Vessel selectedVessel;

	public PlanetariumCamera mainCamera;

	public bool unownedTrackingUnlocked;

	public MissionRecoveryDialog summaryScreen;

	public Mission mission;

	public MEFlowParser flowParser;

	public static string missionFilePath;

	public CommNetUI.DisplayMode commNetUIMode;

	public bool recoverButtonMissionAllowed = true;

	[SerializeField]
	public List<MissionObjective> missionsListObjectives;

	public static Guid tgtVesselId = Guid.Empty;

	public Coroutine requestCoroutine;

	public int lastSetVesselFrame;

	public Vessel SelectedVessel
	{
		get
		{
			return selectedVessel;
		}
		set
		{
			selectedVessel = value;
		}
	}

	public PlanetariumCamera MainCamera => mainCamera;

	public void Awake()
	{
		Instance = this;
		GameEvents.onGUIRecoveryDialogSpawn.Add(onSummaryDialogSpawn);
		GameEvents.onGUIRecoveryDialogDespawn.Add(onSummaryDialogDespawn);
		GameEvents.onNewVesselCreated.Add(onVesselCreated);
		GameEvents.onVesselDestroy.Add(onVesselDestroyed);
		GameEvents.onInputLocksModified.Add(onInputLocksModified);
		GameEvents.onGameSceneLoadRequested.Add(OnSceneLoadRequested);
		GameEvents.OnMapViewFiltersModified.Add(OnMapViewFiltersModified);
		GameEvents.onPlanetariumTargetChanged.Add(OnPlanetariumTargetChanged);
		vesselWidgets = new List<TrackingStationWidget>();
	}

	public void Start()
	{
		commNetUIMode = CommNetUI.ModeTrackingStation;
		mainCamera = (PlanetariumCamera)UnityEngine.Object.FindObjectOfType(typeof(PlanetariumCamera));
		mainCamera.enabled = true;
		st = GamePersistence.LoadGame("persistent", HighLogic.SaveFolder, nullIfIncompatible: true, suppressIncompatibleMessage: true);
		if (st != null && st.flightState != null)
		{
			st.Load();
			MapViewFiltering.Initialize();
			buildVesselsList();
		}
		else
		{
			trackedVessels = new List<Vessel>();
			scaledTargets = new Dictionary<Vessel, MapObject>();
		}
		LeaveBtn.onClick.AddListener(BtnOnClick_LeaveTrackingStation);
		FlyButton.onClick.AddListener(BtnOnClick_FlySelectedVessel);
		FlyButton.Lock();
		DeleteButton.onClick.AddListener(BtnOnClick_DeleteSelectedVessel);
		DeleteButton.Lock();
		RecoverButton.onClick.AddListener(BtnOnclick_RecoverSelectedVessel);
		RecoverButton.Lock();
		TrackButton.onClick.AddListener(BtnOnclick_TrackSelectedVessel);
		TrackButton.Lock();
		TrackButton.gameObject.SetActive(value: false);
		tglTrackedObjects.onValueChanged.AddListener(btnOnClick_TrackedObjects);
		tglMissionObjectives.onValueChanged.AddListener(btnOnClick_MissionObjectives);
		StartCoroutine(CompleteStartUp());
	}

	public IEnumerator CompleteStartUp()
	{
		if (HighLogic.CurrentGame.Mode != Game.Modes.MISSION_BUILDER && HighLogic.CurrentGame.Mode != Game.Modes.MISSION)
		{
			tglMissionObjectives.gameObject.SetActive(value: false);
			missionsListContainer.gameObject.SetActive(value: false);
			tglTrackedObjects.isOn = true;
			TimeScrubberWidget.SetActive(value: false);
			TimeWarpWidget.SetActive(value: true);
			setMissionObjectives(active: false);
			setTrackedObjects(active: true);
		}
		else
		{
			Planetarium.GetUniversalTime();
			while (!MissionSystem.IsActive)
			{
				yield return new WaitForFixedUpdate();
			}
			if (HighLogic.CurrentGame.Mode == Game.Modes.MISSION_BUILDER)
			{
				FlyButton.gameObject.SetActive(value: false);
				DeleteButton.gameObject.SetActive(value: false);
				RecoverButton.gameObject.SetActive(value: false);
				tglMissionObjectives.isOn = true;
			}
			else
			{
				tglTrackedObjects.isOn = true;
			}
			mission = MissionSystem.missions[0];
			createMissionObjectivesItems();
			GameEvents.Mission.onCompleted.Add(RefreshMissionRequest);
			GameEvents.Mission.onFailed.Add(RefreshMissionRequest);
			GameEvents.Mission.onActiveNodeChanged.Add(onMissionNodeChanged);
			GameEvents.Mission.onTestGroupCleared.Add(onTestGroupChanged);
			GameEvents.Mission.onTestGroupInitialized.Add(onTestGroupChanged);
			if (HighLogic.CurrentGame.Mode == Game.Modes.MISSION_BUILDER)
			{
				TimeScrubberWidget.SetActive(value: true);
				TimeWarpWidget.SetActive(value: false);
				setMissionObjectives(active: true);
				setTrackedObjects(active: false);
			}
			else
			{
				TimeScrubberWidget.SetActive(value: false);
				TimeWarpWidget.SetActive(value: true);
				setMissionObjectives(active: false);
				setTrackedObjects(active: true);
			}
		}
		TooltipController_Text component = RecoverButton.GetComponent<TooltipController_Text>();
		if (MissionModeValid() && HighLogic.CurrentGame.Mode == Game.Modes.MISSION && HighLogic.CurrentGame.Parameters.CustomParams<MissionParamsGeneral>().preventVesselRecovery)
		{
			RecoverButton.interactable = false;
			recoverButtonMissionAllowed = false;
			if (component != null)
			{
				component.enabled = true;
			}
		}
		else if (component != null)
		{
			component.enabled = false;
		}
		unownedTrackingUnlocked = GameVariables.Instance.UnlockedSpaceObjectDiscovery(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.TrackingStation));
		if (HighLogic.CurrentGame != null && !HighLogic.CurrentGame.Parameters.TrackingStation.CanLeaveToMainMenu && !HighLogic.CurrentGame.Parameters.TrackingStation.CanLeaveToSpaceCenter)
		{
			LeaveBtn.Lock();
		}
		yield return null;
		if (MissionModeValid() && HighLogic.CurrentGame != null && HighLogic.CurrentGame.Mode == Game.Modes.MISSION_BUILDER)
		{
			btnOnClick_MissionObjectives(toggle: true);
		}
		else
		{
			btnOnClick_TrackedObjects(toggle: true);
		}
		StartCoroutine(CallbackUtil.DelayedCallback(9, delegate
		{
			for (int i = 0; i < FlightGlobals.Bodies.Count; i++)
			{
				if (FlightGlobals.Bodies[i].orbitDriver != null)
				{
					FlightGlobals.Bodies[i].orbitDriver.Renderer.onCelestialBodyIconClicked.Add(onCBIconClicked);
				}
			}
		}));
		if (tgtVesselId != Guid.Empty)
		{
			StartCoroutine(PostInitTgtFocus(10));
		}
	}

	public void OnDestroy()
	{
		GameEvents.onGUIRecoveryDialogSpawn.Remove(onSummaryDialogSpawn);
		GameEvents.onGUIRecoveryDialogDespawn.Remove(onSummaryDialogDespawn);
		GameEvents.onInputLocksModified.Remove(onInputLocksModified);
		GameEvents.OnMapViewFiltersModified.Remove(OnMapViewFiltersModified);
		GameEvents.onPlanetariumTargetChanged.Remove(OnPlanetariumTargetChanged);
		GameEvents.Mission.onCompleted.Remove(RefreshMissionRequest);
		GameEvents.Mission.onFailed.Remove(RefreshMissionRequest);
		GameEvents.Mission.onActiveNodeChanged.Remove(onMissionNodeChanged);
		GameEvents.Mission.onTestGroupCleared.Remove(onTestGroupChanged);
		GameEvents.Mission.onTestGroupInitialized.Remove(onTestGroupChanged);
		if (scaledTargets == null)
		{
			return;
		}
		Dictionary<Vessel, MapObject>.ValueCollection.Enumerator enumerator = scaledTargets.Values.GetEnumerator();
		while (enumerator.MoveNext())
		{
			MapObject current = enumerator.Current;
			if (current != null)
			{
				UnityEngine.Object.DestroyImmediate(current.gameObject);
			}
		}
		scaledTargets.Clear();
		if (missionsListObjectives != null)
		{
			for (int i = 0; i < missionsListObjectives.Count; i++)
			{
				ScenarioCustomWaypoints.RemoveWaypoint(missionsListObjectives[i].waypoint);
				if (missionsListObjectives[i].missionOrbitRenderer != null)
				{
					UnityEngine.Object.DestroyImmediate(missionsListObjectives[i].missionOrbitRenderer.gameObject);
				}
				if (missionsListObjectives[i].mapNode != null)
				{
					missionsListObjectives[i].mapNode.Terminate();
				}
			}
			missionsListObjectives.Clear();
		}
		if (flowParser != null)
		{
			flowParser.gameObject.DestroyGameObject();
		}
		if (HighLogic.CurrentGame.Mode == Game.Modes.MISSION_BUILDER)
		{
			missionFilePath = "";
			for (int j = 0; j < mission.nodes.ValuesList.Count; j++)
			{
				if (!mission.nodes.ValuesList[j].IsVesselNode)
				{
					continue;
				}
				for (int k = 0; k < mission.nodes.ValuesList[j].actionModules.Count; k++)
				{
					if (mission.nodes.ValuesList[j].actionModules[k] is ActionCreateVessel)
					{
						ActionCreateVessel actionCreateVessel = mission.nodes.ValuesList[j].actionModules[k] as ActionCreateVessel;
						if (actionCreateVessel.vesselSituation.playerCreated)
						{
							actionCreateVessel.DeletePlaceholderCraft();
						}
					}
				}
			}
		}
		Instance = null;
	}

	public void buildVesselsList()
	{
		List<Guid> list = new List<Guid>();
		if (scaledTargets == null)
		{
			scaledTargets = new Dictionary<Vessel, MapObject>();
		}
		trackedVessels = new List<Vessel>();
		for (int i = 0; i < FlightGlobals.Vessels.Count; i++)
		{
			Vessel vessel = FlightGlobals.Vessels[i];
			list.Add(vessel.id);
			if (vessel.DiscoveryInfo.HaveKnowledgeAbout(DiscoveryLevels.StateVectors))
			{
				trackedVessels.Add(vessel);
			}
			if (!scaledTargets.ContainsKey(vessel))
			{
				scaledTargets.Add(vessel, vessel.mapObject as ScaledMovement);
			}
			vessel.orbitRenderer.onVesselIconClicked.Add(onVesselIconClick);
		}
		IOUtils.Cleanup(list);
		ConstructUIList();
	}

	public void ClearUIList()
	{
		int count = vesselWidgets.Count;
		while (count-- > 0)
		{
			vesselWidgets[count].Terminate();
		}
		vesselWidgets.Clear();
	}

	public void ConstructUIList()
	{
		ClearUIList();
		int i = 0;
		for (int count = trackedVessels.Count; i < count; i++)
		{
			Vessel v = trackedVessels[i];
			if (MapViewFiltering.CheckAgainstFilter(v))
			{
				TrackingStationWidget trackingStationWidget = UnityEngine.Object.Instantiate(listItemPrefab);
				trackingStationWidget.transform.SetParent(listContainer, worldPositionStays: false);
				trackingStationWidget.SetVessel(v, RequestVessel, listToggleGroup);
				vesselWidgets.Add(trackingStationWidget);
			}
		}
		if (SelectedVessel != null)
		{
			SetVessel(SelectedVessel, keepFocus: true);
		}
	}

	public static void StartTrackingObject(Vessel v)
	{
		v.DiscoveryInfo.SetLevel(v.DiscoveryInfo.Level | (DiscoveryLevels.Name | DiscoveryLevels.StateVectors | DiscoveryLevels.Appearance));
	}

	public static void StopTrackingObject(Vessel v)
	{
		v.DiscoveryInfo.SetLevel(v.DiscoveryInfo.Level & ~DiscoveryLevels.StateVectors);
		v.DiscoveryInfo.SetLastObservedTime(Planetarium.GetUniversalTime());
	}

	public static void GoToAndFocusVessel(Vessel v)
	{
		ClearToSaveStatus clearToSaveStatus = FlightGlobals.ClearToSave();
		if (clearToSaveStatus == ClearToSaveStatus.CLEAR)
		{
			tgtVesselId = v.id;
			if (HighLogic.LoadedSceneIsFlight)
			{
				FlightInputHandler.SetNeutralControls();
			}
			GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
			HighLogic.LoadScene(GameScenes.TRACKSTATION);
		}
		else
		{
			ScreenMessages.PostScreenMessage(FlightGlobals.GetNotClearToSaveStatusReason(clearToSaveStatus, Localizer.Format("#autoLOC_7003255")), 5f);
		}
	}

	public IEnumerator PostInitTgtFocus(int frameDelay)
	{
		for (int i = 0; i < frameDelay; i++)
		{
			yield return null;
		}
		Vessel vessel = FlightGlobals.FindVessel(tgtVesselId);
		if ((bool)vessel)
		{
			SetVessel(vessel, keepFocus: true);
		}
		else
		{
			Debug.LogError("[Tracking Station]: Could not find vessel with id " + tgtVesselId.ToString() + " something went wrong there.");
		}
		tgtVesselId = Guid.Empty;
	}

	public void RequestVessel(Vessel v)
	{
		if (FlightDriver.Pause)
		{
			return;
		}
		if (v == selectedVessel)
		{
			if (Mouse.Left.GetDoubleClick(isDelegate: true))
			{
				FlyVessel(selectedVessel);
			}
		}
		else if (requestCoroutine == null)
		{
			requestCoroutine = StartCoroutine(setRequestedVessel(v));
		}
	}

	public IEnumerator setRequestedVessel(Vessel v)
	{
		yield return null;
		SetVessel(v, keepFocus: true);
		requestCoroutine = null;
	}

	public void SetVessel(Vessel v, bool keepFocus)
	{
		if (lastSetVesselFrame == Time.frameCount)
		{
			return;
		}
		lastSetVesselFrame = Time.frameCount;
		Debug.Log("[Tracking Station]: SetVessel(" + (v ? v.GetDisplayName() : "null") + ")", base.gameObject);
		if (MissionModeValid() && (HighLogic.CurrentGame.Mode == Game.Modes.MISSION_BUILDER || HighLogic.CurrentGame.Mode == Game.Modes.MISSION))
		{
			btnOnClick_TrackedObjects(toggle: true);
		}
		listToggleGroup.SetAllTogglesOff();
		TrackingStationWidget trackingStationWidget = vesselWidgets.Find((TrackingStationWidget vw) => vw.vessel == v);
		if (trackingStationWidget != null)
		{
			trackingStationWidget.toggle.isOn = true;
		}
		if (selectedVessel != null && selectedVessel != v)
		{
			selectedVessel.orbitRenderer.isFocused = false;
			selectedVessel.orbitRenderer.drawIcons = OrbitRendererBase.DrawIcons.const_1;
			selectedVessel.DetachPatchedConicsSolver();
		}
		if (v != null)
		{
			if (selectedVessel != v)
			{
				mainCamera.SetTarget(scaledTargets[v]);
			}
			v.orbitRenderer.isFocused = true;
			if (v.DiscoveryInfo.HaveKnowledgeAbout(DiscoveryLevels.StateVectors))
			{
				if (!v.PatchedConicsAttached)
				{
					v.orbitRenderer.isFocused = true;
					v.AttachPatchedConicsSolver();
				}
			}
			else if (v.PatchedConicsAttached)
			{
				v.DetachPatchedConicsSolver();
				v.orbitRenderer.isFocused = true;
				v.orbitRenderer.drawIcons = OrbitRendererBase.DrawIcons.const_1;
			}
			if (HighLogic.CurrentGame.Mode != Game.Modes.MISSION_BUILDER)
			{
				if (v.DiscoveryInfo.Level == DiscoveryLevels.Owned)
				{
					if (recoverButtonMissionAllowed)
					{
						RecoverButton.gameObject.SetActive(value: true);
						RecoverButton.Unlock();
					}
					TrackButton.gameObject.SetActive(value: false);
					TrackButton.Lock();
					if (HighLogic.CurrentGame.Parameters.TrackingStation.CanFlyVessel)
					{
						FlyButton.Unlock();
					}
					if (HighLogic.CurrentGame.Parameters.TrackingStation.CanAbortVessel)
					{
						if (v.IsRecoverable)
						{
							if (recoverButtonMissionAllowed)
							{
								RecoverButton.Unlock();
								DeleteButton.Lock();
							}
						}
						else
						{
							DeleteButton.Unlock();
							RecoverButton.Lock();
							DeleteButton.gameObject.GetComponent<UIHoverToggler>().SetText(Localizer.Format("#autoLOC_481507"));
						}
					}
				}
				else
				{
					RecoverButton.Lock();
					if (unownedTrackingUnlocked)
					{
						RecoverButton.gameObject.SetActive(value: false);
						TrackButton.gameObject.SetActive(value: true);
					}
					if (!v.DiscoveryInfo.HaveKnowledgeAbout(DiscoveryLevels.StateVectors))
					{
						TrackButton.Unlock();
						DeleteButton.Lock();
					}
					else
					{
						TrackButton.Lock();
						DeleteButton.Unlock();
						DeleteButton.gameObject.GetComponent<UIHoverToggler>().SetText("End Track");
					}
					FlyButton.Lock();
				}
			}
		}
		else
		{
			if (selectedVessel == null || !keepFocus)
			{
				mainCamera.SetTarget(mainCamera.FindNearestTarget());
			}
			FlyButton.Lock();
			DeleteButton.Lock();
			RecoverButton.Lock();
			TrackButton.Lock();
		}
		selectedVessel = v;
	}

	public void FlyVessel(Vessel v)
	{
		if (HighLogic.CurrentGame.Mode != Game.Modes.MISSION_BUILDER)
		{
			if (v.DiscoveryInfo.Level == DiscoveryLevels.Owned)
			{
				GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
				FlightDriver.StartAndFocusVessel("persistent", FlightGlobals.Vessels.IndexOf(v));
			}
			else
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_7003256", (v.vesselType > VesselType.Unknown) ? Localizer.Format("#autoLOC_7003257") : Localizer.Format("#autoLOC_7003258")), 5f, ScreenMessageStyle.UPPER_CENTER);
			}
		}
	}

	public void onCBIconClicked(CelestialBody data)
	{
		mainCamera.SetTarget(data);
	}

	public void onVesselIconClick(Vessel v)
	{
		if (v != selectedVessel)
		{
			RequestVessel(v);
		}
		else if (Mouse.Left.GetDoubleClick())
		{
			FlyVessel(selectedVessel);
		}
	}

	public void BtnOnClick_LeaveTrackingStation()
	{
		GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
		if (HighLogic.CurrentGame.Mode == Game.Modes.MISSION_BUILDER && MissionModeValid())
		{
			FlightGlobals.ClearpersistentIdDictionaries();
			if (FlightGlobals.fetch != null && FlightGlobals.ActiveVessel != null)
			{
				FlightGlobals.fetch.activeVessel = null;
			}
			MissionEditorLogic.StartUpMissionEditor(missionFilePath);
		}
		else if (!HighLogic.CurrentGame.Parameters.TrackingStation.CanLeaveToSpaceCenter && HighLogic.CurrentGame.Parameters.TrackingStation.CanLeaveToMainMenu)
		{
			HighLogic.LoadScene(GameScenes.MAINMENU);
		}
		else if (HighLogic.CurrentGame.Parameters.TrackingStation.CanLeaveToSpaceCenter)
		{
			HighLogic.LoadScene(GameScenes.SPACECENTER);
		}
	}

	public void BtnOnClick_FlySelectedVessel()
	{
		if ((bool)selectedVessel)
		{
			FlyVessel(selectedVessel);
		}
	}

	public void BtnOnClick_DeleteSelectedVessel()
	{
		if (HighLogic.CurrentGame.Mode != Game.Modes.MISSION_BUILDER)
		{
			lockUI();
			if (selectedVessel.DiscoveryInfo.Level != DiscoveryLevels.Owned)
			{
				PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("StopTrackingObject", Localizer.Format("#autoLOC_481619"), Localizer.Format("#autoLOC_5050047"), HighLogic.UISkin, new DialogGUIButton(Localizer.Format("#autoLOC_481620"), StopTrackingVessel), new DialogGUIButton(Localizer.Format("#autoLOC_481621"), OnDialogDismiss)), persistAcrossScenes: false, HighLogic.UISkin).OnDismiss = OnDialogDismiss;
			}
			else
			{
				PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("TerminateMission", Localizer.Format("#autoLOC_481625"), Localizer.Format("#autoLOC_5050048"), HighLogic.UISkin, new DialogGUIButton(Localizer.Format("#autoLOC_481626"), OnVesselDeleteConfirm), new DialogGUIButton(Localizer.Format("#autoLOC_481627"), OnDialogDismiss)), persistAcrossScenes: false, HighLogic.UISkin).OnDismiss = OnDialogDismiss;
			}
		}
	}

	public void BtnOnclick_RecoverSelectedVessel()
	{
		if (HighLogic.CurrentGame.Mode != Game.Modes.MISSION_BUILDER)
		{
			lockUI();
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("Recover Vessel", Localizer.Format("#autoLOC_481635"), Localizer.Format("#autoLOC_5050049"), HighLogic.UISkin, new DialogGUIButton(Localizer.Format("#autoLOC_481636"), OnRecoverConfirm), new DialogGUIButton(Localizer.Format("#autoLOC_481637"), OnDialogDismiss)), persistAcrossScenes: false, HighLogic.UISkin).OnDismiss = OnDialogDismiss;
		}
	}

	public void BtnOnclick_TrackSelectedVessel()
	{
		Debug.Log("Now Tracking " + selectedVessel.GetDisplayName() + ".");
		StartTrackingObject(selectedVessel);
		buildVesselsList();
		SetVessel(selectedVessel, keepFocus: true);
	}

	public void btnOnClick_TrackedObjects(bool toggle)
	{
		if (toggle)
		{
			listToggleGroup.SetAllTogglesOff();
			missionsListContainer.gameObject.SetActive(value: false);
			listContainer.gameObject.SetActive(value: true);
			sideBarScrollRect.content = listContainer.GetComponent<RectTransform>();
			tglTrackedObjects.isOn = true;
			setMissionObjectives(active: false);
			setTrackedObjects(active: true);
		}
	}

	public void btnOnClick_MissionObjectives(bool toggle)
	{
		if (toggle)
		{
			listToggleGroup.SetAllTogglesOff();
			listContainer.gameObject.SetActive(value: false);
			missionsListContainer.gameObject.SetActive(value: true);
			sideBarScrollRect.content = missionsListContainer.GetComponent<RectTransform>();
			tglMissionObjectives.isOn = true;
			setMissionObjectives(active: true);
			setTrackedObjects(active: false);
		}
	}

	public void setMissionObjectives(bool active)
	{
		if (missionsListObjectives == null || !MissionModeValid())
		{
			return;
		}
		for (int i = 0; i < missionsListObjectives.Count; i++)
		{
			MissionObjective missionObjective = missionsListObjectives[i];
			if (missionObjective.waypoint != null && missionObjective.waypoint.node != null && missionObjective.waypoint.node.VisualIconData != null)
			{
				missionsListObjectives[i].waypoint.node.VisualIconData.iconEnabled = active;
			}
			if (missionObjective.missionOrbitRenderer != null)
			{
				missionObjective.missionOrbitRenderer.activeDraw = active;
				missionObjective.missionOrbitRenderer.isFocused = active;
			}
			if (missionObjective.mapNode != null)
			{
				missionObjective.mapNode.VisualIconData.iconEnabled = active;
			}
		}
	}

	public void setTrackedObjects(bool active)
	{
		if (scaledTargets != null)
		{
			if (!active)
			{
				commNetUIMode = CommNetUI.ModeTrackingStation;
				CommNetUI.ModeTrackingStation = CommNetUI.DisplayMode.None;
			}
			else
			{
				CommNetUI.ModeTrackingStation = commNetUIMode;
			}
		}
	}

	public void StopTrackingVessel()
	{
		Debug.Log("Stopped Tracking " + selectedVessel.GetDisplayName() + ".");
		StopTrackingObject(selectedVessel);
		SetVessel(null, keepFocus: true);
		buildVesselsList();
		OnDialogDismiss();
	}

	public void OnVesselDeleteConfirm()
	{
		Vessel vessel = selectedVessel;
		GameEvents.onVesselTerminated.Fire(vessel.protoVessel);
		SetVessel(null, keepFocus: false);
		UnityEngine.Object.Destroy(scaledTargets[vessel].gameObject);
		scaledTargets.Remove(vessel);
		List<ProtoCrewMember> vesselCrew = vessel.GetVesselCrew();
		int count = vesselCrew.Count;
		for (int i = 0; i < count; i++)
		{
			ProtoCrewMember protoCrewMember = vesselCrew[i];
			Debug.Log("Crewmember " + protoCrewMember.name + " is lost.");
			protoCrewMember.StartRespawnPeriod();
		}
		UnityEngine.Object.DestroyImmediate(vessel.gameObject);
		buildVesselsList();
		GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
		OnDialogDismiss();
	}

	public void OnRecoverConfirm()
	{
		Vessel vessel = selectedVessel;
		SetVessel(null, keepFocus: false);
		GameEvents.onVesselRecovered.Fire(vessel.protoVessel, data1: false);
		UnityEngine.Object.Destroy(scaledTargets[vessel].gameObject);
		scaledTargets.Remove(vessel);
		UnityEngine.Object.DestroyImmediate(vessel.gameObject);
		buildVesselsList();
		GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
		OnDialogDismiss();
	}

	public void OnDialogDismiss()
	{
		if (!(summaryScreen != null))
		{
			unlockUI();
		}
	}

	public void OnPlanetariumTargetChanged(MapObject obj)
	{
	}

	public void OnSceneLoadRequested(GameScenes scn)
	{
		GameEvents.onGameSceneLoadRequested.Remove(OnSceneLoadRequested);
		SetVessel(null, keepFocus: false);
		GameEvents.onNewVesselCreated.Remove(onVesselCreated);
		GameEvents.onVesselDestroy.Remove(onVesselDestroyed);
		for (int i = 0; i < FlightGlobals.Bodies.Count; i++)
		{
			if (FlightGlobals.Bodies[i].orbitDriver != null)
			{
				FlightGlobals.Bodies[i].orbitDriver.Renderer.onCelestialBodyIconClicked.Remove(onCBIconClicked);
			}
		}
	}

	public void onSummaryDialogSpawn(MissionRecoveryDialog dialog)
	{
		summaryScreen = dialog;
		InputLockManager.SetControlLock("summaryDialogLock");
	}

	public void onSummaryDialogDespawn(MissionRecoveryDialog dialog)
	{
		summaryScreen = null;
		InputLockManager.RemoveControlLock("summaryDialogLock");
		OnDialogDismiss();
	}

	public void onVesselCreated(Vessel v)
	{
		buildVesselsList();
	}

	public void onVesselDestroyed(Vessel v)
	{
		if (scaledTargets != null && scaledTargets.ContainsKey(v))
		{
			if (v == selectedVessel || scaledTargets[v] == mainCamera.target)
			{
				SetVessel(null, keepFocus: false);
			}
			if (scaledTargets[v] != null && scaledTargets[v].gameObject != null)
			{
				UnityEngine.Object.Destroy(scaledTargets[v].gameObject);
			}
			scaledTargets.Remove(v);
			buildVesselsList();
		}
	}

	public void onInputLocksModified(GameEvents.FromToAction<ControlTypes, ControlTypes> l)
	{
		if ((l.from & ControlTypes.TRACKINGSTATION_UI) == ControlTypes.None && (l.to & ControlTypes.TRACKINGSTATION_UI) != ControlTypes.None)
		{
			DeleteButton.Lock();
			FlyButton.Lock();
			RecoverButton.Lock();
			TrackButton.Lock();
			LeaveBtn.Lock();
		}
		if ((l.from & ControlTypes.TRACKINGSTATION_UI) != ControlTypes.None && (l.to & ControlTypes.TRACKINGSTATION_UI) == ControlTypes.None)
		{
			unlockUI();
		}
	}

	public void OnMapViewFiltersModified(MapViewFiltering.VesselTypeFilter data)
	{
		ConstructUIList();
	}

	public void lockUI()
	{
		FlyButton.Lock();
		DeleteButton.Lock();
		LeaveBtn.Lock();
		RecoverButton.Lock();
		TrackButton.Lock();
	}

	public void unlockUI()
	{
		if (!InputLockManager.IsUnlocked(ControlTypes.TRACKINGSTATION_UI))
		{
			return;
		}
		if ((bool)selectedVessel && HighLogic.CurrentGame.Mode != Game.Modes.MISSION_BUILDER)
		{
			if (HighLogic.CurrentGame.Parameters.TrackingStation.CanFlyVessel)
			{
				if (selectedVessel.DiscoveryInfo.Level == DiscoveryLevels.Owned)
				{
					FlyButton.Unlock();
				}
				else
				{
					TrackButton.Unlock();
				}
			}
			if (HighLogic.CurrentGame.Parameters.TrackingStation.CanAbortVessel)
			{
				if (selectedVessel.IsRecoverable && selectedVessel.DiscoveryInfo.Level == DiscoveryLevels.Owned)
				{
					if (recoverButtonMissionAllowed)
					{
						RecoverButton.Unlock();
						DeleteButton.Lock();
					}
				}
				else
				{
					DeleteButton.Unlock();
					RecoverButton.Lock();
				}
			}
		}
		if (HighLogic.CurrentGame.Parameters.TrackingStation.CanLeaveToMainMenu || HighLogic.CurrentGame.Parameters.TrackingStation.CanLeaveToSpaceCenter)
		{
			LeaveBtn.Unlock();
		}
	}

	public bool MissionModeValid()
	{
		byte[] hashBytes = null;
		string signature = null;
		if (MissionSystem.Instance != null && MissionSystem.missions.Count > 0 && MissionSystem.missions[0] != null)
		{
			hashBytes = MissionSystem.missions[0].HashBytes;
			signature = MissionSystem.missions[0].Signature;
		}
		return ExpansionsLoader.IsExpansionInstalled("MakingHistory", hashBytes, signature);
	}

	public void createMissionObjectivesItems()
	{
		missionsListObjectives = new List<MissionObjective>();
		flowParser = MEFlowParser.Create(base.gameObject.transform, missionsListContainer, null, null);
		MEFlowParser.ParseMission(mission);
		if (HighLogic.CurrentGame.Mode == Game.Modes.MISSION_BUILDER)
		{
			flowParser.CreateMissionFlowUI_Toggle(mission, MissionsObjectiveCallback, missionsListToggleGroup, showEvents: true, showNonObjectives: true, showStartNodes: true);
		}
		else if (HighLogic.CurrentGame.Mode == Game.Modes.MISSION)
		{
			flowParser.CreateMissionFlowUI_Toggle(mission, MissionsObjectiveCallback, missionsListToggleGroup);
		}
		for (int i = 0; i < mission.nodes.Count; i++)
		{
			MENode mENode = mission.nodes.At(i);
			if (!mENode.HasTestModules)
			{
				continue;
			}
			for (int j = 0; j < mENode.testGroups.Count; j++)
			{
				for (int k = 0; k < mENode.testGroups[j].testModules.Count; k++)
				{
					TestModule testModule = mENode.testGroups[j].testModules[k] as TestModule;
					if (!(testModule != null))
					{
						continue;
					}
					if (testModule.hasWaypoint && testModule is INodeWaypoint nodeWaypoint && nodeWaypoint.HasNodeWaypoint())
					{
						Waypoint nodeWaypoint2 = nodeWaypoint.GetNodeWaypoint();
						if (nodeWaypoint2 != null)
						{
							ScenarioCustomWaypoints.AddWaypoint(nodeWaypoint2, isMission: true);
							MissionObjective missionObjective = new MissionObjective();
							missionObjective.waypoint = nodeWaypoint2;
							missionObjective.mapNode = nodeWaypoint2.node;
							missionObjective.node = mENode;
							missionsListObjectives.Add(missionObjective);
							continue;
						}
					}
					Orbit testOrbit;
					MissionOrbitRenderer testOrbitRenderer;
					if (mENode.orbitRenderer != null)
					{
						MissionObjective missionObjective2 = new MissionObjective();
						missionObjective2.node = mENode;
						missionObjective2.body = mENode.orbitRenderer.celestialBody;
						if (mENode.mapObject != null)
						{
							missionObjective2.mapNode = mENode.mapObject.uiNode;
						}
						else
						{
							MapObject mapObject = MapObject.Create(mENode.name + " TestOrbit", mENode.Title, mENode.orbitRenderer.driver.orbit, MapObject.ObjectType.MENode);
							missionObjective2.mapNode = mapObject.uiNode;
						}
						missionObjective2.missionOrbitRenderer = mENode.orbitRenderer;
						missionsListObjectives.Add(missionObjective2);
					}
					else if (mENode.HasTestOrbit(out testOrbit, out testOrbitRenderer))
					{
						MissionObjective missionObjective3 = new MissionObjective();
						missionObjective3.node = mENode;
						missionObjective3.body = testOrbit.referenceBody;
						if (mENode.mapObject != null)
						{
							missionObjective3.mapNode = mENode.mapObject.uiNode;
						}
						else
						{
							MapObject mapObject2 = MapObject.Create(mENode.name + " TestOrbit", mENode.Title, testOrbit, MapObject.ObjectType.MENode);
							missionObjective3.mapNode = mapObject2.uiNode;
						}
						missionObjective3.missionOrbitRenderer = testOrbitRenderer;
						missionsListObjectives.Add(missionObjective3);
					}
					else if (testModule is INodeBody nodeBody && nodeBody.HasNodeBody() && nodeBody.GetNodeBody() != null)
					{
						MissionObjective missionObjective4 = new MissionObjective();
						missionObjective4.node = mENode;
						missionObjective4.body = nodeBody.GetNodeBody();
						missionsListObjectives.Add(missionObjective4);
					}
				}
			}
		}
	}

	public void onTestGroupChanged(TestGroup testGroup)
	{
		updateMissionObjectivesItems();
	}

	public void RefreshMissionRequest(Mission mission)
	{
		updateMissionObjectivesItems();
	}

	public void onMissionNodeChanged(Mission mission, GameEvents.FromToAction<MENode, MENode> nodeChanges)
	{
		updateMissionObjectivesItems();
	}

	public void updateMissionObjectivesItems()
	{
		flowParser.UpdateFlowUIItems();
	}

	public void MissionsObjectiveCallback(MEFlowUINode uiNode)
	{
		if (!(uiNode != null) || !(uiNode.Node != null))
		{
			return;
		}
		if (uiNode.Node.IsVesselNode)
		{
			string text = "";
			for (int i = 0; i < uiNode.Node.actionModules.Count; i++)
			{
				if (uiNode.Node.actionModules[i] is ActionCreateVessel)
				{
					text = (uiNode.Node.actionModules[i] as ActionCreateVessel).vesselSituation.vesselName;
				}
				else if (uiNode.Node.actionModules[i] is ActionCreateAsteroid)
				{
					text = (uiNode.Node.actionModules[i] as ActionCreateAsteroid).asteroid.name;
				}
				else if (uiNode.Node.actionModules[i] is ActionCreateComet)
				{
					text = (uiNode.Node.actionModules[i] as ActionCreateComet).comet.name;
				}
				else if (uiNode.Node.actionModules[i] is ActionCreateKerbal)
				{
					text = (uiNode.Node.actionModules[i] as ActionCreateKerbal).missionKerbal.Kerbal.name;
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			{
				foreach (KeyValuePair<Vessel, MapObject> scaledTarget in scaledTargets)
				{
					if (scaledTarget.Key.vesselName == text)
					{
						mainCamera.SetTarget(scaledTarget.Value);
						break;
					}
				}
				return;
			}
		}
		for (int j = 0; j < missionsListObjectives.Count; j++)
		{
			MissionObjective missionObjective = missionsListObjectives[j];
			if (missionObjective.node == uiNode.Node)
			{
				if (missionObjective.waypoint != null)
				{
					MapObject target = mainCamera.FindNearestTarget(missionsListObjectives[j].waypoint.worldPosition);
					mainCamera.SetTarget(target);
				}
				if (missionObjective.body != null)
				{
					mainCamera.SetTarget(missionObjective.body.MapObject);
				}
			}
		}
	}
}
