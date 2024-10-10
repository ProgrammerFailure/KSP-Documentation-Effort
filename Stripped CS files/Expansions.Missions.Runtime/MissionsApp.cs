using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Expansions.Missions.Actions;
using Expansions.Missions.Flow;
using ns11;
using ns12;
using ns2;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Runtime;

public class MissionsApp : UIApp
{
	public class MissionListItem
	{
		public UICascadingList.CascadingListItem listItem;

		public MEFlowParser parser;
	}

	public enum AppIconStates
	{
		Normal,
		Warning
	}

	public static MissionsApp Instance;

	public DictionaryValueList<string, MissionListItem> missionList = new DictionaryValueList<string, MissionListItem>();

	public DictionaryValueList<uint, UICascadingList.CascadingListItem> vesselList = new DictionaryValueList<uint, UICascadingList.CascadingListItem>();

	public float refreshTimeToWait = 0.5f;

	public Callback onCreateNextVessel;

	public bool refreshRequested;

	public bool refreshRequestedCurrentVessel;

	public VesselSituation requestedCurrentVessel;

	public List<Mission> refreshMissions;

	public bool refreshMissionPos;

	public MissionAppMode mode;

	public bool waitingForEditor;

	public bool startNewVessel;

	public bool lockAppOpen;

	public bool ShowEnterBuildMsg = true;

	[SerializeField]
	public GenericAppFrame appFramePrefab;

	public GenericAppFrame appFrame;

	[SerializeField]
	public GenericCascadingList cascadingListPrefab;

	public GenericCascadingList cascadingList;

	[SerializeField]
	public UIListItem BodyMissionVesselHeader_prefab;

	[SerializeField]
	public UIListItem_spacer BodyMissionVesselItem_prefab;

	[SerializeField]
	public UIListItem BodySettingsToggleItem_prefab;

	[SerializeField]
	public UIListItem BodySettingsButtonItem_prefab;

	public UIListItem TestModeActiveNode;

	public TextMeshProUGUI TestModeActiveNodeText;

	public MissionsAppVesselInfo currentVessel;

	public bool appInitializing;

	public bool updateMissionsDaemonRunning;

	public bool updateVesselsDaemonRunning;

	public uint lastVesselBuildId;

	public DictionaryValueList<string, MissionListItem> MissionList => missionList;

	public DictionaryValueList<uint, UICascadingList.CascadingListItem> VesselList => vesselList;

	public MissionAppMode Mode => mode;

	public MissionsAppVesselInfo CurrentVessel => currentVessel;

	public MissionsAppVesselInfo GetVesselListAppVesselInfo(uint persistentId)
	{
		int num = 0;
		while (true)
		{
			if (num < vesselList.Count)
			{
				if (vesselList.KeyAt(num) == persistentId)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return vesselList.At(num).header.Data as MissionsAppVesselInfo;
	}

	public void Start()
	{
		mode = ((HighLogic.LoadedScene == GameScenes.EDITOR) ? MissionAppMode.Editor : MissionAppMode.Play);
	}

	public override bool OnAppAboutToStart()
	{
		return true;
	}

	public override void OnAppInitialized()
	{
		appInitializing = true;
		Debug.Log("[MissionsApp] OnAppStarted(): id: " + GetInstanceID());
		if (Instance != null)
		{
			Debug.Log("MissionsApp already exist, destroying this instance");
			UnityEngine.Object.DestroyImmediate(base.gameObject);
			return;
		}
		if (HighLogic.CurrentGame.Mode != Game.Modes.MISSION)
		{
			Debug.Log("MissionsApp does not execute in this game mode, destroying this instance");
			UnityEngine.Object.DestroyImmediate(base.gameObject);
			return;
		}
		Instance = this;
		appFrame = UnityEngine.Object.Instantiate(appFramePrefab);
		appFrame.transform.SetParent(base.transform, worldPositionStays: false);
		appFrame.transform.localPosition = Vector3.zero;
		appFrame.Setup(base.appLauncherButton, "Missions", "#autoLOC_8006067", 266, 440);
		appFrame.AddGlobalInputDelegate(base.MouseInput_PointerEnter, base.MouseInput_PointerExit);
		cascadingList = UnityEngine.Object.Instantiate(cascadingListPrefab);
		cascadingList.Setup(appFrame.scrollList);
		refreshMissions = new List<Mission>();
		GameEvents.Mission.onMissionsLoaded.Add(OnMissionsLoaded);
		HideApp();
		if (mode == MissionAppMode.Play)
		{
			GameEvents.Mission.onCompleted.Add(RefreshMissionRequest);
			GameEvents.Mission.onFailed.Add(RefreshMissionRequest);
			GameEvents.Mission.onActiveNodeChanged.Add(onMissionNodeChanged);
			GameEvents.Mission.onTestGroupCleared.Add(onTestGroupChanged);
			GameEvents.Mission.onTestGroupInitialized.Add(onTestGroupChanged);
			if (MissionSystem.Instance != null && MissionSystem.missions.Count > 0)
			{
				OnMissionsLoaded();
			}
			else
			{
				CreateMissionsList();
			}
			StartCoroutine(UpdateMissionsDaemon());
		}
		else
		{
			GameEvents.Mission.onMissionCurrentVesselToBuildChanged.Add(RefreshCurrentVesselToBuild);
			GameEvents.onEditorStarted.Add(onEditorStarted);
			EditorLogic.fetch.saveBtn.onClick.AddListener(editorSaveClicked);
			if (MissionSystem.Instance != null && MissionSystem.missions.Count > 0)
			{
				OnMissionsLoaded();
			}
			else
			{
				CreateVesselsList(null);
				mode = MissionAppMode.EditorFreeBuild;
			}
			startVesselDaemon();
			lockAppOpen = true;
		}
		appInitializing = false;
	}

	public override void DisplayApp()
	{
		if (appFrame != null)
		{
			appFrame.gameObject.SetActive(value: true);
		}
	}

	public override void HideApp()
	{
		if (appFrame != null)
		{
			appFrame.gameObject.SetActive(value: false);
		}
	}

	public override ApplicationLauncher.AppScenes GetAppScenes()
	{
		return ApplicationLauncher.AppScenes.SPACECENTER | ApplicationLauncher.AppScenes.FLIGHT | ApplicationLauncher.AppScenes.MAPVIEW | ApplicationLauncher.AppScenes.flag_5 | ApplicationLauncher.AppScenes.flag_6 | ApplicationLauncher.AppScenes.TRACKSTATION;
	}

	public override void OnAppDestroy()
	{
		if (refreshMissions != null)
		{
			refreshMissions.Clear();
		}
		if (missionList != null)
		{
			missionList.Clear();
		}
		GameEvents.Mission.onCompleted.Remove(RefreshMissionRequest);
		GameEvents.Mission.onFailed.Remove(RefreshMissionRequest);
		GameEvents.Mission.onMissionsLoaded.Remove(OnMissionsLoaded);
		GameEvents.Mission.onActiveNodeChanged.Remove(onMissionNodeChanged);
		GameEvents.Mission.onTestGroupCleared.Remove(onTestGroupChanged);
		GameEvents.Mission.onTestGroupInitialized.Remove(onTestGroupChanged);
		GameEvents.Mission.onMissionCurrentVesselToBuildChanged.Remove(RefreshCurrentVesselToBuild);
		GameEvents.onEditorStarted.Remove(onEditorStarted);
		if (cascadingList != null)
		{
			cascadingList.gameObject.DestroyGameObject();
		}
		if (appFrame != null)
		{
			if ((bool)ApplicationLauncher.Instance)
			{
				ApplicationLauncher.Instance.RemoveOnRepositionCallback(appFrame.Reposition);
			}
			appFrame.gameObject.DestroyGameObject();
		}
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public override Vector3 GetAppScreenPos(Vector3 defaultAnchorPos)
	{
		return new Vector3(ApplicationLauncher.Instance.transform.position.x, defaultAnchorPos.y, defaultAnchorPos.z);
	}

	public void OnMissionsLoaded()
	{
		if (mode == MissionAppMode.Play && HighLogic.CurrentGame != null && HighLogic.CurrentGame.Mode == Game.Modes.MISSION)
		{
			CreateMissionsList();
			if (MissionSystem.missions.Count > 0)
			{
				if (MissionSystem.missions[0].situation.VesselsArePending)
				{
					SetAppIconState(AppIconStates.Warning);
				}
				else
				{
					SetAppIconState(AppIconStates.Normal);
				}
			}
		}
		if (mode == MissionAppMode.Editor && HighLogic.CurrentGame != null && HighLogic.CurrentGame.Mode == Game.Modes.MISSION)
		{
			onCreateNextVessel = null;
			Mission mission = null;
			if (MissionSystem.missions.Count > 0)
			{
				mission = MissionSystem.missions[0];
			}
			if (mission != null && (vesselList == null || (vesselList != null && vesselList.Count == 0)))
			{
				mission.situation.UpdateVesselBuildValues();
			}
			CreateVesselsList(mission);
			ShowEnterBuildMsg = true;
			if (appInitializing && mission != null && !mission.situation.VesselsArePending)
			{
				mode = MissionAppMode.EditorFreeBuild;
				ShowEnterBuildMsg = false;
			}
			if (File.Exists(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/lastcreatevesseleditor.missionsfs"))
			{
				File.Delete(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/lastcreatevesseleditor.missionsfs");
			}
			bool flag = false;
			if (mission != null && mission.briefingNodeActive)
			{
				flag = true;
				mission.briefingNodeActive = false;
			}
			if (!string.IsNullOrEmpty(GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE, GameScenes.EDITOR)))
			{
				File.Copy(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/persistent.sfs", KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/lastcreatevesseleditor.missionsfs", overwrite: true);
			}
			else
			{
				Debug.Log("[MissionsApp]: Unable to save revert to editor file for mission as autosave is not available.");
			}
			if (flag)
			{
				mission.briefingNodeActive = true;
			}
			CheckforVesselstoBuild(onCreateNextVessel, 0u);
			if (mission.situation.CurrentVesselToBuild != null)
			{
				RefreshCurrentVesselToBuild(mission, mission.situation.CurrentVesselToBuild);
			}
			else if (!mission.situation.VesselsArePending && mission.situation.VesselSituationList.Count > 0)
			{
				RefreshCurrentVesselToBuild(mission, mission.situation.VesselSituationList.KeyAt(0));
			}
			if (mode != MissionAppMode.EditorFreeBuild)
			{
				DisplayApp();
			}
			string text = "";
			if (mission != null && mission.situation.CurrentVesselToBuild != null)
			{
				text = mission.situation.CurrentVesselToBuild.vesselName;
			}
			else if (requestedCurrentVessel != null)
			{
				text = requestedCurrentVessel.vesselName;
			}
			if (!string.IsNullOrEmpty(text))
			{
				EditorLogic.fetch.ship.shipName = Localizer.Format(text);
				EditorLogic.fetch.shipNameField.text = Localizer.Format(text);
			}
		}
		if (!ExpansionsLoader.IsExpansionInstalled("MakingHistory") && missionList.Count <= 0 && Instance != null)
		{
			UnityEngine.Object.Destroy(Instance.gameObject);
		}
	}

	public void RemoveMission(Mission mission)
	{
		if (missionList.ContainsKey(mission.title))
		{
			MissionListItem missionListItem = missionList[mission.title];
			cascadingList.ruiList.RemoveCascadingItem(missionListItem.listItem);
			missionList.Remove(mission.title);
		}
	}

	public void AddMission(Mission mission)
	{
		if (missionList.ContainsKey(mission.title))
		{
			Debug.Log("[Missions App]: Trying to add a mission " + mission.title + "which already exists in the list.");
		}
		else
		{
			missionList.Add(mission.title, CreateMissionItem(mission));
		}
	}

	public void RefreshMissionParameter(Mission mission, GameEvents.FromToAction<MENode, MENode> nodeChanges)
	{
		RefreshMissionRequested(mission, changePositions: false);
	}

	public void RefreshMissionRequest(Mission mission)
	{
		RefreshMissionRequested(mission);
	}

	public void RefreshMissionRequested(Mission mission, bool changePositions = true)
	{
		refreshRequested = true;
		refreshMissions.AddUnique(mission);
		refreshMissionPos = changePositions;
	}

	public void RefreshMission(Mission mission, bool changePositions = true)
	{
		if (!missionList.ContainsKey(mission.title))
		{
			Debug.Log("[Missions App]: No mission found while attempting to refresh missions list");
		}
		else
		{
			missionList[mission.title].parser.UpdateFlowUIItems();
		}
	}

	public IEnumerator UpdateMissionsDaemon()
	{
		yield return null;
		yield return null;
		if (updateMissionsDaemonRunning)
		{
			yield break;
		}
		updateMissionsDaemonRunning = true;
		while ((bool)this)
		{
			if (ApplicationLauncher.Ready && appFrame.gameObject.activeSelf && refreshRequested)
			{
				int i = 0;
				for (int count = refreshMissions.Count; i < count; i++)
				{
					RefreshMission(refreshMissions[i], refreshMissionPos);
				}
				refreshMissions.Clear();
				refreshRequested = false;
				refreshMissionPos = false;
			}
			yield return new WaitForSeconds(refreshTimeToWait);
		}
	}

	public void onTestGroupChanged(TestGroup testGroup)
	{
		RefreshMissionRequested(testGroup.node.mission, changePositions: false);
	}

	public void onMissionNodeChanged(Mission mission, GameEvents.FromToAction<MENode, MENode> nodeChanges)
	{
		RefreshMissionParameter(mission, nodeChanges);
		if (TestModeActiveNode != null && TestModeActiveNodeText != null)
		{
			TestModeActiveNodeText.text = Localizer.Format("#autoLOC_8002101", nodeChanges.to.Title);
		}
		if (mission.situation.VesselsArePending)
		{
			SetAppIconState(AppIconStates.Warning);
		}
		else
		{
			SetAppIconState(AppIconStates.Normal);
		}
		UpdateLauncherButtonPlayAnim();
	}

	public void onEditorIcon(UIStateButton button)
	{
		if (button.currentStateIndex == 0)
		{
			button.SetState(1, invokeChange: false);
		}
		else
		{
			button.SetState(0, invokeChange: false);
		}
		UIListItem componentInParent = button.GetComponentInParent<UIListItem>();
		MissionsAppVesselInfo vesselSwitchToInfo = componentInParent.Data as MissionsAppVesselInfo;
		if (vesselSwitchToInfo != null)
		{
			if (vesselSwitchToInfo.mission.situation.CurrentVesselToBuild != null && componentInParent != null)
			{
				string text = ((EditorDriver.editorFacility == EditorFacility.const_1) ? "SPH" : "VAB");
				EditorFacility switchtoEditor = ((EditorDriver.editorFacility != EditorFacility.const_1) ? EditorFacility.const_1 : EditorFacility.const_2);
				string msg = Localizer.Format("#autoLOC_8006068", PSystemSetup.Instance.GetLaunchSiteDisplayName(text));
				PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("SwitchFacility", msg, "#autoLOC_8006069", UISkinManager.GetSkin("KSP window 7"), new DialogGUIHorizontalLayout(new DialogGUIFlexibleSpace(), new DialogGUIButton("#autoLOC_226976", delegate
				{
					setEditorIconStates();
				}, 80f, 30f, true), new DialogGUIButton("#autoLOC_417274", delegate
				{
					stopVesselsDaemon();
					changeVesselListState(vesselSwitchToInfo);
					waitingForEditor = true;
					EditorDriver.StartupBehaviour = EditorDriver.StartupBehaviours.LOAD_FROM_CACHE;
					EditorDriver.SwitchEditor(switchtoEditor);
				}, 80f, 30f, true))), persistAcrossScenes: false, UISkinManager.GetSkin("KSP window 7")).OnDismiss = setEditorIconStates;
			}
			else
			{
				setEditorIconStates();
			}
		}
		else
		{
			setEditorIconStates();
		}
	}

	public void onStateIcon(UIStateButton button)
	{
		button.SetState(0, invokeChange: false);
		UIListItem componentInParent = button.GetComponentInParent<UIListItem>();
		if (componentInParent != null)
		{
			MissionsAppVesselInfo vesselSwitchToInfo = componentInParent.Data as MissionsAppVesselInfo;
			if (vesselSwitchToInfo != null && vesselSwitchToInfo.mission.situation.CurrentVesselToBuild != null)
			{
				if (vesselSwitchToInfo.vesselSituation == vesselSwitchToInfo.mission.situation.CurrentVesselToBuild)
				{
					setEditorIconStates();
					return;
				}
				string msg = Localizer.Format("#autoLOC_8006070", vesselSwitchToInfo.mission.situation.CurrentVesselToBuild.vesselName, vesselSwitchToInfo.vesselSituation.vesselName);
				PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("SwitchVessel", msg, "Switch Vessel", UISkinManager.GetSkin("KSP window 7"), new DialogGUIHorizontalLayout(new DialogGUIFlexibleSpace(), new DialogGUIButton("#autoLOC_226976", delegate
				{
					setEditorIconStates();
				}, 80f, 30f, true), new DialogGUIButton("#autoLOC_417274", delegate
				{
					GameEvents.Mission.onMissionCurrentVesselToBuildChanged.Fire(vesselSwitchToInfo.mission, vesselSwitchToInfo.vesselSituation);
				}, 80f, 30f, true))), persistAcrossScenes: false, UISkinManager.GetSkin("KSP window 7")).OnDismiss = setEditorIconStates;
			}
			else
			{
				setEditorIconStates();
			}
		}
		else
		{
			setEditorIconStates();
		}
	}

	public void changeVessels(MissionsAppVesselInfo vesselSwitchToInfo)
	{
		if (currentVessel != null && EditorLogic.fetch.ship.Parts.Count > 0)
		{
			string text = KSPUtil.SanitizeString(EditorLogic.fetch.shipNameField.text, '_', replaceEmpty: true);
			ShipConstruction.SaveShip(text);
			currentVessel.vesselSituation.craftFile = text + ".craft";
			currentVessel.vesselSituation.SetVesselCrew(HighLogic.CurrentGame.CrewRoster, CrewAssignmentDialog.Instance.GetManifest());
			currentVessel.vesselSituation.SetVesselCrew(currentVessel.mission.situation.crewRoster, CrewAssignmentDialog.Instance.GetManifest());
		}
		changeVesselListState(vesselSwitchToInfo);
		currentVessel.vesselSituation.SetCrewAvailable(HighLogic.CurrentGame.CrewRoster);
		currentVessel.vesselSituation.SetCrewAvailable(currentVessel.mission.situation.crewRoster);
		EditorFacility facility = EditorDriver.editorFacility;
		if (!string.IsNullOrEmpty(currentVessel.vesselSituation.craftFile))
		{
			ConfigNode configNode = findCraftFile(MissionSystem.missions[0].ShipsPath, currentVessel.vesselSituation.craftFile, out facility);
			if (configNode != null)
			{
				ShipConstruction.ShipConfig = configNode;
				EditorLogic.fetch.ResetCrewAssignment(ShipConstruction.ShipConfig, allowAutoHire: false);
			}
			else
			{
				ShipConstruction.ShipConfig = null;
				startNewVessel = true;
			}
		}
		else
		{
			ShipConstruction.ShipConfig = null;
			try
			{
				facility = currentVessel.vesselSituation.location.facility;
			}
			catch
			{
			}
			startNewVessel = true;
		}
		stopVesselsDaemon();
		if (facility != EditorDriver.editorFacility)
		{
			waitingForEditor = true;
			EditorDriver.StartupBehaviour = EditorDriver.StartupBehaviours.LOAD_FROM_CACHE;
			EditorDriver.SwitchEditor(facility);
		}
		else
		{
			waitingForEditor = true;
			EditorDriver.StartupBehaviour = EditorDriver.StartupBehaviours.LOAD_FROM_CACHE;
			EditorLogic.fetch.StartEditor(isRestart: true);
		}
	}

	public void onEditorStarted()
	{
		if (waitingForEditor)
		{
			if (startNewVessel)
			{
				EditorLogic.fetch.ship.parts.Clear();
				InputLockManager.RemoveControlLock("Editor_outOfPartMode");
				InputLockManager.RemoveControlLock("EditorLogic_rootPartMode");
				ShipConstruction.ClearBackups();
				if (EditorLogic.fetch.ship != null && EditorLogic.fetch.ship.vesselDeltaV != null)
				{
					EditorLogic.fetch.ship.vesselDeltaV.gameObject.DestroyGameObject();
				}
				EditorLogic.fetch.ship = new ShipConstruct(EditorDriver.editorFacility);
				EditorLogic.fetch.ship.vesselDeltaV = VesselDeltaV.Create(EditorLogic.fetch.ship);
				EditorLogic.fetch.UpdateUI();
				startNewVessel = false;
			}
			EditorLogic.fetch.ship.shipName = currentVessel.vesselSituation.vesselName;
			EditorLogic.fetch.shipNameField.text = EditorLogic.fetch.ship.shipName;
			waitingForEditor = false;
		}
		if (currentVessel != null)
		{
			if (currentVessel.vesselSituation.location.situation == MissionSituation.VesselStartSituations.PRELAUNCH && EditorLogic.fetch != null)
			{
				EditorLogic.fetch.launchSiteName = currentVessel.vesselSituation.location.launchSite;
			}
			else
			{
				EditorLogic.fetch.launchSiteName = "";
			}
		}
		setEditorIconStates();
		startVesselDaemon();
	}

	public void RefreshVesselsToCreate(Mission mission)
	{
		refreshRequested = true;
	}

	public void RefreshCurrentVesselToBuild(Mission mission, VesselSituation currentVesselToBuild)
	{
		if (EditorPartList.Instance.ExcludeFilters["Vessel Parts"] != null)
		{
			EditorPartList.Instance.ExcludeFilters.RemoveFilter("Vessel Parts");
		}
		requestedCurrentVessel = currentVesselToBuild;
		requestedCurrentVessel.partFilter.ApplyPartsFilter();
		refreshRequestedCurrentVessel = true;
	}

	public void editorSaveClicked()
	{
		if (currentVessel != null)
		{
			currentVessel.vesselSituation.craftFile = KSPUtil.SanitizeString(EditorLogic.fetch.shipNameField.text, '_', replaceEmpty: true) + ".craft";
		}
	}

	public ConfigNode findCraftFile(string craftFolder, string craftFile, out EditorFacility facility)
	{
		facility = EditorDriver.editorFacility;
		if (File.Exists(craftFolder.TrimEnd('/') + "/" + craftFile))
		{
			return ConfigNode.Load(craftFolder.TrimEnd('/') + "/" + craftFile);
		}
		if (File.Exists(craftFolder.TrimEnd('/') + "/VAB/" + craftFile))
		{
			ConfigNode result = ConfigNode.Load(craftFolder.TrimEnd('/') + "/VAB/" + craftFile);
			facility = EditorFacility.const_1;
			return result;
		}
		if (File.Exists(craftFolder.TrimEnd('/') + "/SPH/" + craftFile))
		{
			ConfigNode result2 = ConfigNode.Load(craftFolder.TrimEnd('/') + "/SPH/" + craftFile);
			facility = EditorFacility.const_2;
			return result2;
		}
		return null;
	}

	public void changeVesselListState(MissionsAppVesselInfo vesselToSwitch)
	{
		if (vesselToSwitch != null)
		{
			if (currentVessel != null)
			{
				currentVessel.vesselSituation.ClearEditorEvents();
			}
			currentVessel = vesselToSwitch;
			currentVessel.vesselSituation.StartEditorEvents();
			onEditorStarted();
			vesselToSwitch.mission.situation.currentVesselToBuildId = currentVessel.vesselSituation.persistentId;
		}
	}

	public void setEditorIconStates()
	{
		for (int i = 0; i < vesselList.Count; i++)
		{
			if (!(vesselList.At(i).header.Data is MissionsAppVesselInfo missionsAppVesselInfo))
			{
				continue;
			}
			if (missionsAppVesselInfo == currentVessel)
			{
				missionsAppVesselInfo.stateButton.SetState(1, invokeChange: false);
				missionsAppVesselInfo.editorButton.gameObject.SetActive(value: true);
				string textString = ((EditorDriver.editorFacility == EditorFacility.const_2) ? "#autoLOC_8006071" : "#autoLOC_8006072");
				missionsAppVesselInfo.tooltipeditorButton.textString = textString;
				missionsAppVesselInfo.tooltipstateButton.textString = "#autoLOC_8006073";
				if (EditorDriver.editorFacility == EditorFacility.const_1)
				{
					missionsAppVesselInfo.editorButton.SetState(1, invokeChange: false);
				}
				else
				{
					missionsAppVesselInfo.editorButton.SetState(0, invokeChange: false);
				}
			}
			else
			{
				missionsAppVesselInfo.tooltipstateButton.textString = "#autoLOC_8006074";
				missionsAppVesselInfo.stateButton.SetState(0, invokeChange: false);
				missionsAppVesselInfo.editorButton.gameObject.SetActive(value: false);
			}
		}
	}

	public IEnumerator UpdateVesselsDaemon()
	{
		yield return null;
		yield return null;
		if (updateVesselsDaemonRunning)
		{
			yield break;
		}
		updateVesselsDaemonRunning = true;
		while ((bool)this)
		{
			if (ApplicationLauncher.Ready && appFrame.gameObject.activeSelf && refreshRequested)
			{
				int i = 0;
				for (int count = refreshMissions.Count; i < count; i++)
				{
					CreateVesselsList(refreshMissions[i]);
				}
				refreshRequested = false;
				refreshMissions.Clear();
			}
			if (ApplicationLauncher.Ready && appFrame.gameObject.activeSelf && refreshRequestedCurrentVessel)
			{
				if (currentVessel == null || currentVessel.vesselSituation != requestedCurrentVessel)
				{
					for (int j = 0; j < vesselList.Count; j++)
					{
						MissionsAppVesselInfo vesselListAppVesselInfo = GetVesselListAppVesselInfo(requestedCurrentVessel.persistentId);
						if (vesselListAppVesselInfo != null)
						{
							refreshRequestedCurrentVessel = false;
							changeVessels(vesselListAppVesselInfo);
							break;
						}
					}
				}
				requestedCurrentVessel = null;
				refreshRequestedCurrentVessel = false;
			}
			if (ApplicationLauncher.Ready && appFrame.gameObject.activeSelf && lockAppOpen)
			{
				yield return new WaitForSeconds(2f);
				base.appLauncherButton.toggleButton.SetState(UIRadioButton.State.True, UIRadioButton.CallType.APPLICATIONSILENT, null);
				base.appLauncherButton.onLeftClickBtn(base.appLauncherButton.toggleButton);
				lockAppOpen = false;
			}
			yield return new WaitForSeconds(refreshTimeToWait);
		}
	}

	public void stopVesselsDaemon()
	{
		if (updateVesselsDaemonRunning)
		{
			StopCoroutine(UpdateVesselsDaemon());
			updateVesselsDaemonRunning = false;
		}
	}

	public void startVesselDaemon()
	{
		if (!updateVesselsDaemonRunning)
		{
			StartCoroutine(UpdateVesselsDaemon());
		}
	}

	public bool EditorVesselCompleted(string craftFile, VesselCrewManifest vesselManifest, Callback onCreateNextVessel)
	{
		if (MissionSystem.missions.Count < 1)
		{
			Debug.LogError("[MissionSystem]: Fatal Error. Missions not loaded for Mission Play. Cannot create Vessel.");
			return false;
		}
		if (mode == MissionAppMode.EditorFreeBuild)
		{
			return false;
		}
		Mission mission = MissionSystem.missions[0];
		VesselSituation vesselSituationByVesselID;
		if (!mission.situation.VesselsArePending)
		{
			vesselSituationByVesselID = mission.situation.GetVesselSituationByVesselID(currentVessel.vesselSituation.persistentId);
			if (vesselSituationByVesselID != null)
			{
				vesselSituationByVesselID.SetVesselCrew(HighLogic.CurrentGame.CrewRoster, vesselManifest);
				vesselSituationByVesselID.SetVesselCrew(mission.situation.crewRoster, vesselManifest);
			}
			CheckforVesselstoBuild(onCreateNextVessel, mission.situation.currentVesselToBuildId);
			return true;
		}
		vesselSituationByVesselID = mission.situation.GetVesselSituationByVesselID(mission.situation.currentVesselToBuildId);
		if (vesselSituationByVesselID != null)
		{
			vesselSituationByVesselID.craftFile = (mission.situation.CurrentVesselToBuild.craftFile = craftFile);
			string vesselName = craftFile.Substring(0, craftFile.LastIndexOf(".craft"));
			vesselSituationByVesselID.vesselName = (mission.situation.CurrentVesselToBuild.vesselName = vesselName);
			mission.situation.VesselSituationReadyToLaunch(vesselSituationByVesselID);
			vesselSituationByVesselID.SetVesselCrew(HighLogic.CurrentGame.CrewRoster, vesselManifest);
			vesselSituationByVesselID.SetVesselCrew(mission.situation.crewRoster, vesselManifest);
			SetVesselItemReadyIcon(vesselSituationByVesselID, null);
		}
		this.onCreateNextVessel = onCreateNextVessel;
		uint currentVesselToBuildId = mission.situation.currentVesselToBuildId;
		mission.situation.currentVesselToBuildId = 0u;
		CheckforVesselstoBuild(onCreateNextVessel, currentVesselToBuildId);
		return true;
	}

	public void CheckforVesselstoBuild(Callback onCreateNextVessel, uint inlastVesselBuildId)
	{
		if (MissionSystem.missions.Count <= 0)
		{
			return;
		}
		Mission missionToTest = MissionSystem.missions[0];
		lastVesselBuildId = inlastVesselBuildId;
		missionToTest.situation.UpdateVesselBuildValues();
		if (mode == MissionAppMode.EditorFreeBuild)
		{
			return;
		}
		if (missionToTest.situation.VesselsArePending)
		{
			if (missionToTest.situation.vesselsBuilt == 0)
			{
				HighLogic.CurrentGame.editorFacility = ((missionToTest.situation.CurrentVesselToBuild.location.facility != EditorFacility.const_2) ? EditorFacility.const_1 : EditorFacility.const_2);
				string message = Localizer.Format("#autoLOC_8006075");
				if (!missionToTest.isStarted)
				{
					missionToTest.situation.startType = ((missionToTest.situation.CurrentVesselToBuild.location.facility == EditorFacility.const_2) ? MissionSituation.StartTypeEnum.const_1 : MissionSituation.StartTypeEnum.const_0);
				}
				else
				{
					message = Localizer.Format("#autoLOC_8006076");
				}
				this.onCreateNextVessel = onCreateNextVessel;
				if (GameSettings.MISSION_SHOW_CREATE_VESSEL_WARNING && ShowEnterBuildMsg)
				{
					UIConfirmDialog.Spawn(Localizer.Format("#autoLOC_8006077"), message, OnCreateVesselWarningOK, showCancelBtn: false);
				}
				else
				{
					FireCreateNextVessel();
				}
			}
			else
			{
				FireCreateNextVessel();
			}
		}
		else if (!missionToTest.isStarted)
		{
			string msg = Localizer.Format("#autoLOC_8006078");
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("CreateMissionVessel", msg, Localizer.Format("#autoLOC_8006077"), UISkinManager.GetSkin("KSP window 7"), new DialogGUIButton(Localizer.Format("#autoLOC_8006082"), delegate
			{
				missionToTest.RebuildCraftFileList();
				Planetarium.SetUniversalTime(missionToTest.situation.startUT);
				if (HighLogic.CurrentGame.flightState != null)
				{
					HighLogic.CurrentGame.flightState.universalTime = missionToTest.situation.startUT;
				}
				if ((bool)LoadingBufferMask.Instance)
				{
					LoadingBufferMask.Instance.Show();
				}
				StartCoroutine(missionToTest.GenerateMissionLaunchSites(onMissionLaunchSitesGenerated));
			}), new DialogGUIButton(Localizer.Format("#autoLOC_8006079"), delegate
			{
				StayOnEditor(missionToTest);
			})), persistAcrossScenes: false, UISkinManager.GetSkin("KSP window 7")).OnDismiss = delegate
			{
				StayOnEditor(missionToTest);
			};
		}
		else
		{
			if (!missionToTest.isStarted)
			{
				return;
			}
			Guid nodeGuidByVesselID = missionToTest.GetNodeGuidByVesselID(lastVesselBuildId);
			if (nodeGuidByVesselID != Guid.Empty)
			{
				MENode nodeById = missionToTest.GetNodeById(nodeGuidByVesselID);
				if (nodeById != null)
				{
					List<ActionCreateVessel> allActionModules = nodeById.GetAllActionModules<ActionCreateVessel>();
					for (int i = 0; i < allActionModules.Count; i++)
					{
						if (allActionModules[i].activeMessage != null && allActionModules[i].activeMessage.textInstance != null)
						{
							ScreenMessages.RemoveMessage(allActionModules[i].activeMessage);
						}
					}
				}
			}
			string msg2 = Localizer.Format("#autoLOC_8006080");
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("CreateMissionVessel", msg2, Localizer.Format("#autoLOC_8006077"), UISkinManager.GetSkin("KSP window 7"), new DialogGUIButton(Localizer.Format("#autoLOC_8006081"), delegate
			{
				missionToTest.RebuildCraftFileList();
				if ((bool)LoadingBufferMask.Instance)
				{
					LoadingBufferMask.Instance.Show();
				}
				StartCoroutine(missionToTest.GenerateMissionLaunchSites(onMissionLaunchSitesGeneratedContinue));
			}), new DialogGUIButton(Localizer.Format("#autoLOC_8006079"), delegate
			{
				if (missionToTest.situation.GetVesselSituationByVesselID(lastVesselBuildId) != null)
				{
					StayOnEditor(missionToTest);
				}
			})), persistAcrossScenes: false, UISkinManager.GetSkin("KSP window 7")).OnDismiss = delegate
			{
				StayOnEditor(missionToTest);
			};
		}
	}

	public void StayOnEditor(Mission mission)
	{
		VesselSituation vesselSituationByVesselID = mission.situation.GetVesselSituationByVesselID(lastVesselBuildId);
		if (vesselSituationByVesselID != null)
		{
			vesselSituationByVesselID.SetCrewAvailable(HighLogic.CurrentGame.CrewRoster);
			vesselSituationByVesselID.SetCrewAvailable(mission.situation.crewRoster);
			EditorLogic.fetch.RefreshCrewDialog();
			vesselSituationByVesselID.readyToLaunch = false;
			GetVesselListAppVesselInfo(vesselSituationByVesselID.persistentId).readyImage.gameObject.SetActive(value: false);
		}
	}

	public void onMissionLaunchSitesGenerated(Mission missionToTest)
	{
		if (missionToTest.situation.GetVesselSituationByVesselID(lastVesselBuildId) != null)
		{
			currentVessel.vesselSituation.SetVesselCrew(HighLogic.CurrentGame.CrewRoster, CrewAssignmentDialog.Instance.GetManifest());
			currentVessel.vesselSituation.SetVesselCrew(currentVessel.mission.situation.crewRoster, CrewAssignmentDialog.Instance.GetManifest());
		}
		if ((bool)LoadingBufferMask.Instance)
		{
			LoadingBufferMask.Instance.textInfo.text = Localizer.Format("#autoLOC_8002116");
		}
		StartCoroutine(MissionSystem.Instance.SetupFlight(missionToTest, trackingStation: false, onSetupFlightSuccess, onSetupFlightFailed));
	}

	public void onMissionLaunchSitesGeneratedContinue(Mission missionToTest)
	{
		currentVessel.vesselSituation.SetVesselCrew(HighLogic.CurrentGame.CrewRoster, CrewAssignmentDialog.Instance.GetManifest());
		currentVessel.vesselSituation.SetVesselCrew(currentVessel.mission.situation.crewRoster, CrewAssignmentDialog.Instance.GetManifest());
		StartCoroutine(buildStartedMissionVessels(missionToTest, onBuildStartedMissionVesselsComplete));
	}

	public void onBuildStartedMissionVesselsComplete(Mission missionToTest)
	{
		ResetVesselsCountsLists(missionToTest);
		bool flag = false;
		if (missionToTest != null && missionToTest.briefingNodeActive)
		{
			flag = true;
			missionToTest.briefingNodeActive = false;
		}
		if (File.Exists(missionToTest.MissionInfo.SaveFolderPath + "lastcreatevesselspawn.missionsfs"))
		{
			File.Delete(missionToTest.MissionInfo.SaveFolderPath + "lastcreatevesselspawn.missionsfs");
		}
		if (!string.IsNullOrEmpty(GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE, GameScenes.FLIGHT)))
		{
			File.Copy(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/persistent.sfs", missionToTest.MissionInfo.SaveFolderPath + "lastcreatevesselspawn.missionsfs", overwrite: true);
		}
		else
		{
			Debug.Log("[MissionsApp]: Unable to save revert to vessel spawn for mission as autosave is not available.");
		}
		if (flag)
		{
			missionToTest.briefingNodeActive = true;
		}
		if ((bool)LoadingBufferMask.Instance)
		{
			LoadingBufferMask.Instance.Hide();
		}
		FlightDriver.StartupBehaviour = FlightDriver.StartupBehaviours.RESUME_SAVED_FILE;
		HighLogic.CurrentGame.Start();
	}

	public void onSetupFlightSuccess(Mission missionToTest)
	{
		ResetVesselsCountsLists(missionToTest);
		missionToTest.StartMission();
		bool flag = false;
		if (missionToTest != null && missionToTest.briefingNodeActive)
		{
			flag = true;
			missionToTest.briefingNodeActive = false;
		}
		if (File.Exists(missionToTest.MissionInfo.SaveFolderPath + "lastcreatevesselspawn.missionsfs"))
		{
			File.Delete(missionToTest.MissionInfo.SaveFolderPath + "lastcreatevesselspawn.missionsfs");
		}
		if (!string.IsNullOrEmpty(GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE, GameScenes.FLIGHT)))
		{
			File.Copy(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/persistent.sfs", missionToTest.MissionInfo.SaveFolderPath + "lastcreatevesselspawn.missionsfs", overwrite: true);
		}
		else
		{
			Debug.Log("[MissionsApp]: Unable to save revert to vessel spawn for mission as autosave is not available.");
		}
		if (flag)
		{
			missionToTest.briefingNodeActive = true;
		}
		if ((bool)LoadingBufferMask.Instance)
		{
			LoadingBufferMask.Instance.Hide();
		}
		HighLogic.CurrentGame.Start();
	}

	public void onSetupFlightFailed(Mission missionToTest, string errorString)
	{
		if ((bool)LoadingBufferMask.Instance)
		{
			LoadingBufferMask.Instance.Hide();
		}
		missionToTest.MissionCriticalError(errorString);
	}

	public void OnCreateVesselWarningOK(bool dontShowAgain)
	{
		if (dontShowAgain == GameSettings.MISSION_SHOW_CREATE_VESSEL_WARNING)
		{
			GameSettings.MISSION_SHOW_CREATE_VESSEL_WARNING = !dontShowAgain;
			GameSettings.SaveGameSettingsOnly();
		}
		ShowEnterBuildMsg = false;
		FireCreateNextVessel();
	}

	public void FireCreateNextVessel()
	{
		if (onCreateNextVessel != null)
		{
			onCreateNextVessel();
		}
	}

	public IEnumerator buildStartedMissionVessels(Mission missionToTest, Callback<Mission> OnComplete)
	{
		if ((bool)LoadingBufferMask.Instance)
		{
			LoadingBufferMask.Instance.textInfo.text = Localizer.Format("#autoLOC_8002117");
		}
		for (int vslI = 0; vslI < missionToTest.situation.VesselSituationList.Count; vslI++)
		{
			VesselSituation vesselSituation = missionToTest.situation.VesselSituationList.KeyAt(vslI);
			if (!vesselSituation.readyToLaunch || vesselSituation.launched)
			{
				continue;
			}
			if (vesselSituation.location.situation == MissionSituation.VesselStartSituations.PRELAUNCH)
			{
				List<ProtoVessel> list = new List<ProtoVessel>();
				if (HighLogic.CurrentGame.flightState != null && HighLogic.CurrentGame.flightState.protoVessels != null)
				{
					list = ShipConstruction.FindVesselsLandedAt(HighLogic.CurrentGame.flightState, vesselSituation.location.launchSite);
				}
				int i = 0;
				for (int count = list.Count; i < count; i++)
				{
					ShipConstruction.RecoverVesselFromFlight(list[i], HighLogic.CurrentGame.flightState);
				}
			}
			yield return StartCoroutine(MissionSystem.Instance.ConstructShip(vesselSituation, missionToTest, instantiateVessel: true));
		}
		missionToTest.situation.RemoveLaunchedVessels();
		yield return null;
		OnComplete(missionToTest);
	}

	public void ExitEditor()
	{
		if (MissionSystem.missions.Count > 0)
		{
			Mission mission = MissionSystem.missions[0];
			if (mission.situation.CurrentVesselToBuild != null)
			{
				mission.situation.startType = ((mission.situation.CurrentVesselToBuild.location.facility == EditorFacility.const_2) ? MissionSituation.StartTypeEnum.const_1 : MissionSituation.StartTypeEnum.const_0);
				HighLogic.CurrentGame.editorFacility = ((mission.situation.CurrentVesselToBuild.location.facility != EditorFacility.const_2) ? EditorFacility.const_1 : EditorFacility.const_2);
				HighLogic.CurrentGame.startScene = GameScenes.EDITOR;
			}
			else
			{
				mission.situation.startType = ((EditorDriver.editorFacility == EditorFacility.const_2) ? MissionSituation.StartTypeEnum.const_1 : MissionSituation.StartTypeEnum.const_0);
				HighLogic.CurrentGame.editorFacility = EditorDriver.editorFacility;
				HighLogic.CurrentGame.startScene = GameScenes.EDITOR;
			}
		}
		if (EditorPartList.Instance != null)
		{
			if (EditorPartList.Instance.ExcludeFilters["Vessel Parts"] != null)
			{
				EditorPartList.Instance.ExcludeFilters.RemoveFilter("Vessel Parts");
			}
			EditorPartList.Instance.Refresh(EditorPartList.State.PartsList);
		}
	}

	public void ResetVesselsCountsLists(Mission mission)
	{
		if (currentVessel != null)
		{
			currentVessel.vesselSituation.ClearEditorEvents();
		}
		mission.situation.resetVesselsCountsLists();
		vesselList.Clear();
		appFrame.scrollList.Clear(destroyElements: true);
		missionList.Clear();
	}

	public UICascadingList.CascadingListItem CreateTestModeSettingsItem()
	{
		Button button;
		UIListItem header = cascadingList.CreateHeader("#autoLOC_8006083", out button, scaleBg: true);
		return cascadingList.ruiList.AddCascadingItem(header, cascadingList.CreateFooter(), CreateTestModeSettingsList(), button);
	}

	public List<UIListItem> CreateTestModeSettingsList()
	{
		List<UIListItem> list = new List<UIListItem>();
		UIListItem uIListItem = UnityEngine.Object.Instantiate(BodySettingsToggleItem_prefab);
		uIListItem.GetComponentInChildren<TextMeshProUGUI>().text = "#autoLOC_8006084";
		UIStateButton componentInChildren = uIListItem.GetComponentInChildren<UIStateButton>();
		componentInChildren.SetState(GameSettings.MISSION_TEST_AUTOMATIC_CHECKPOINTS ? 1 : 0);
		componentInChildren.onValueChanged.AddListener(onCheckpointsSettingsChange);
		list.Add(uIListItem);
		uIListItem = UnityEngine.Object.Instantiate(BodySettingsButtonItem_prefab);
		uIListItem.GetComponentInChildren<TextMeshProUGUI>().text = "#autoLOC_8006085";
		uIListItem.GetComponentInChildren<Button>().onClick.AddListener(onCheckpointCreate);
		list.Add(uIListItem);
		TestModeActiveNode = UnityEngine.Object.Instantiate(BodySettingsToggleItem_prefab);
		TestModeActiveNodeText = TestModeActiveNode.GetComponentInChildren<TextMeshProUGUI>();
		TestModeActiveNodeText.text = Localizer.Format("#autoLOC_8002101", MissionSystem.missions[0].activeNode.Title);
		TestModeActiveNode.GetComponentInChildren<UIStateButton>().gameObject.SetActive(value: false);
		list.Add(TestModeActiveNode);
		return list;
	}

	public void onCheckpointsSettingsChange(UIStateButton button)
	{
		GameSettings.MISSION_TEST_AUTOMATIC_CHECKPOINTS = ((button.currentStateIndex != 0) ? true : false);
		GameSettings.SaveSettings();
	}

	public void onCheckpointCreate()
	{
		MissionSystem.CreateCheckpoint(MissionSystem.missions[0].activeNode, addUTStamp: true);
	}

	public string GetColoredMissionHeader(Mission mission)
	{
		string text = StringBuilderCache.Format("<color=#bdbdbd>");
		if (mission.isStarted)
		{
			text = StringBuilderCache.Format("<color=#E5E5E5>");
		}
		if (mission.isEnded)
		{
			text = ((!mission.isSuccesful) ? StringBuilderCache.Format("<color=#d80000>") : StringBuilderCache.Format("<color=#20d223>"));
		}
		return text + StringBuilderCache.Format(Localizer.Format(mission.title) + "</color>");
	}

	public string GetColoredObjectiveHeader(string text, bool complete, bool alert = false)
	{
		string text2 = StringBuilderCache.Format("");
		if (alert)
		{
			return StringBuilderCache.Format("<color=#ff0000><b>" + Localizer.Format(text) + "</b></color>");
		}
		if (complete)
		{
			return StringBuilderCache.Format("<color=#bdbdbd>" + text + "</color>");
		}
		return StringBuilderCache.Format("<color=#CDEA83FF>" + text + "</color>");
	}

	public void CreateMissionsList()
	{
		appFrame.scrollList.Clear(destroyElements: true);
		missionList.Clear();
		vesselList.Clear();
		if (HighLogic.CurrentGame != null && HighLogic.CurrentGame.Mode == Game.Modes.MISSION && MissionSystem.IsTestMode)
		{
			CreateTestModeSettingsItem();
		}
		if (!(MissionSystem.Instance != null) || MissionSystem.missions.Count <= 0)
		{
			return;
		}
		int i = 0;
		for (int count = MissionSystem.missions.Count; i < count; i++)
		{
			Mission mission = MissionSystem.missions[i];
			if (ExpansionsLoader.IsExpansionInstalled("MakingHistory", mission.HashBytes, mission.Signature))
			{
				missionList.Add(mission.title, CreateMissionItem(mission));
			}
		}
	}

	public MissionListItem CreateMissionItem(Mission mission)
	{
		Button button;
		UIListItem uIListItem = cascadingList.CreateHeader(GetColoredMissionHeader(mission), out button, scaleBg: true);
		MEFlowParser parser = MEFlowParser.Create(uIListItem.gameObject.transform, null, null);
		UICascadingList.CascadingListItem listItem = cascadingList.ruiList.AddCascadingItem(uIListItem, cascadingList.CreateFooter(), CreateObjectivesList(mission, parser), button);
		return new MissionListItem
		{
			listItem = listItem,
			parser = parser
		};
	}

	public List<UIListItem> CreateObjectivesList(Mission mission, MEFlowParser parser)
	{
		List<UIListItem> list = new List<UIListItem>();
		GameObject obj = new GameObject("MEFlowUITarget");
		UIListItem uIListItem = obj.AddComponent<UIListItem>();
		obj.AddComponent<VerticalLayoutGroup>();
		parser.flowObjectsParent = uIListItem.transform;
		parser.CreateMissionFlowUI_Button(mission, MEFlowUINode.ButtonAction.ToggleDetails);
		list.Add(uIListItem);
		return list;
	}

	public string setVesselHeaderColor(VesselSituation vessel, bool active)
	{
		string text = StringBuilderCache.Format("");
		text = ((!active) ? StringBuilderCache.Format("<color=#bdbdbd>") : StringBuilderCache.Format("<color=#CDEA83FF>"));
		return text + StringBuilderCache.Format(Localizer.Format(vessel.vesselName) + "</color>");
	}

	public void CreateVesselsList(Mission mission)
	{
		appFrame.scrollList.Clear(destroyElements: true);
		missionList.Clear();
		vesselList.Clear();
		if (!(mission != null))
		{
			return;
		}
		bool flag = false;
		int count = mission.situation.VesselSituationList.Count;
		for (int i = 0; i < count; i++)
		{
			bool firstVessel = false;
			VesselSituation vesselSituation = mission.situation.VesselSituationList.KeyAt(i);
			if (!vesselSituation.playerCreated || (vesselSituation.playerCreated && vesselSituation.launched))
			{
				continue;
			}
			if (!flag)
			{
				if (mission.situation.CurrentVesselToBuild == null)
				{
					firstVessel = true;
					flag = true;
				}
				else if (mission.situation.CurrentVesselToBuild != null && vesselSituation.persistentId == mission.situation.currentVesselToBuildId)
				{
					firstVessel = true;
					flag = true;
				}
			}
			vesselList.Add(vesselSituation.persistentId, CreateItem(mission, vesselSituation, firstVessel));
		}
	}

	public UICascadingList.CascadingListItem CreateItem(Mission mission, VesselSituation vessel, bool firstVessel)
	{
		UIListItem uIListItem = UnityEngine.Object.Instantiate(BodyMissionVesselHeader_prefab);
		MissionsAppVesselInfo missionsAppVesselInfo = (MissionsAppVesselInfo)(uIListItem.Data = new MissionsAppVesselInfo());
		missionsAppVesselInfo.mission = mission;
		missionsAppVesselInfo.vesselSituation = vessel;
		missionsAppVesselInfo.vesselName = uIListItem.GetComponentInChildren<TextMeshProUGUI>();
		missionsAppVesselInfo.headerButton = uIListItem.GetComponentInChildren<Button>();
		UIStateButton[] componentsInChildren = uIListItem.GetComponentsInChildren<UIStateButton>();
		int num = componentsInChildren.Length;
		for (int i = 0; i < num; i++)
		{
			UIStateButton uIStateButton = componentsInChildren[i];
			string text = uIStateButton.gameObject.name;
			if (!(text == "editorImage"))
			{
				if (text == "stateImage")
				{
					missionsAppVesselInfo.stateButton = uIStateButton;
					missionsAppVesselInfo.stateButton.onValueChanged.AddListener(onStateIcon);
				}
			}
			else
			{
				missionsAppVesselInfo.editorButton = uIStateButton;
				missionsAppVesselInfo.editorButton.onValueChanged.AddListener(onEditorIcon);
			}
		}
		missionsAppVesselInfo.tooltipeditorButton = missionsAppVesselInfo.editorButton.GetComponent<TooltipController_Text>();
		missionsAppVesselInfo.tooltipstateButton = missionsAppVesselInfo.stateButton.GetComponent<TooltipController_Text>();
		Image[] componentsInChildren2 = uIListItem.GetComponentsInChildren<Image>(includeInactive: true);
		num = componentsInChildren2.Length;
		for (int j = 0; j < num; j++)
		{
			Image image = componentsInChildren2[j];
			string text = image.gameObject.name;
			if (text == "selectedImage")
			{
				missionsAppVesselInfo.readyImage = image;
			}
		}
		if (EditorDriver.editorFacility == EditorFacility.const_1)
		{
			missionsAppVesselInfo.editorButton.SetState(1, invokeChange: false);
		}
		else
		{
			missionsAppVesselInfo.editorButton.SetState(0, invokeChange: false);
		}
		if (firstVessel)
		{
			missionsAppVesselInfo.editorButton.gameObject.SetActive(value: true);
			missionsAppVesselInfo.stateButton.SetState(1, invokeChange: false);
			currentVessel = missionsAppVesselInfo;
			onEditorStarted();
			currentVessel.vesselSituation.StartEditorEvents();
			missionsAppVesselInfo.vesselName.text = setVesselHeaderColor(vessel, active: true);
			string textString = ((EditorDriver.editorFacility == EditorFacility.const_2) ? "#autoLOC_8006071" : "#autoLOC_8006072");
			missionsAppVesselInfo.tooltipeditorButton.textString = textString;
			missionsAppVesselInfo.tooltipstateButton.textString = "#autoLOC_8006073";
		}
		else
		{
			missionsAppVesselInfo.editorButton.gameObject.SetActive(value: false);
			missionsAppVesselInfo.stateButton.SetState(0, invokeChange: false);
			missionsAppVesselInfo.vesselName.text = setVesselHeaderColor(vessel, active: false);
			missionsAppVesselInfo.tooltipstateButton.textString = "#autoLOC_8006074";
		}
		UICascadingList.CascadingListItem result = cascadingList.ruiList.AddCascadingItem(uIListItem, cascadingList.CreateFooter(), CreateVesselDetails(missionsAppVesselInfo), missionsAppVesselInfo.headerButton);
		SetVesselItemReadyIcon(vessel, missionsAppVesselInfo);
		return result;
	}

	public void SetVesselItemReadyIcon(VesselSituation vesselSituation, MissionsAppVesselInfo vesselInfo)
	{
		bool active = false;
		if (vesselInfo == null)
		{
			vesselInfo = GetVesselListAppVesselInfo(vesselSituation.persistentId);
		}
		if (vesselInfo != null)
		{
			EditorFacility facility = EditorDriver.editorFacility;
			ConfigNode configNode = findCraftFile(MissionSystem.missions[0].ShipsPath, currentVessel.vesselSituation.craftFile, out facility);
			if (vesselSituation.craftFile != null && configNode != null && (vesselInfo.vesselSituation.readyToLaunch || vesselInfo.vesselSituation.launched))
			{
				active = true;
			}
			vesselInfo.readyImage.gameObject.SetActive(active);
		}
	}

	public List<UIListItem> CreateVesselDetails(MissionsAppVesselInfo vesselInfo)
	{
		List<UIListItem> list = new List<UIListItem>();
		AddVesselSubParameter(list, vesselInfo);
		return list;
	}

	public void AddVesselSubParameter(List<UIListItem> requirements, MissionsAppVesselInfo vesselInfo)
	{
		if (!string.IsNullOrEmpty(vesselInfo.vesselSituation.vesselDescription))
		{
			UIListItem uIListItem = cascadingList.CreateBody(BodyMissionVesselItem_prefab, vesselInfo.vesselSituation.vesselDescription);
			requirements.Add(uIListItem);
			UIStateImage componentInChildren = uIListItem.GetComponentInChildren<UIStateImage>();
			if (componentInChildren != null)
			{
				componentInChildren.gameObject.SetActive(value: false);
				Transform transform = componentInChildren.transform.parent.Find("spacer");
				if (transform != null)
				{
					transform.gameObject.SetActive(value: false);
				}
			}
			uIListItem.Data = vesselInfo;
		}
		List<VesselRestriction> activeRestrictions = vesselInfo.vesselSituation.vesselRestrictionList.ActiveRestrictions;
		for (int i = 0; i < activeRestrictions.Count; i++)
		{
			UIListItem listItem = cascadingList.CreateBody(BodyMissionVesselItem_prefab, activeRestrictions[i].GetStateMessage());
			requirements.Add(listItem);
			UIStateImage listItemState = listItem.GetComponentInChildren<UIStateImage>();
			activeRestrictions[i].AddAppUIReference(ref listItem, ref listItemState);
			listItem.Data = vesselInfo;
		}
	}

	public void UpdateLauncherButtonPlayAnim(float duration = 5f)
	{
		if (ApplicationLauncher.Instance != null && ApplicationLauncher.Ready && base.appLauncherButton != null)
		{
			base.appLauncherButton.PlayAnim(ApplicationLauncherButton.AnimatedIconType.NOTIFICATION, duration);
		}
	}

	public void UpdateLauncherButtonStopAnim()
	{
		if (ApplicationLauncher.Instance != null && ApplicationLauncher.Ready && base.appLauncherButton != null)
		{
			base.appLauncherButton.StopAnim();
		}
	}

	public void SetAppIconState(AppIconStates iconState)
	{
		switch (iconState)
		{
		case AppIconStates.Warning:
			base.appLauncherButton.spriteAnim.SetBool("Warning", value: true);
			break;
		case AppIconStates.Normal:
			base.appLauncherButton.spriteAnim.SetBool("Warning", value: false);
			break;
		}
	}
}
