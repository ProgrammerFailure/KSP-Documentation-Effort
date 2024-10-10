using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Expansions.Missions.Actions;
using Expansions.Missions.Editor;
using ns9;
using UnityEngine;

namespace Expansions.Missions;

[Serializable]
public class MissionSituation : IConfigNode, IMENodeDisplay
{
	public enum StartTypeEnum
	{
		const_0,
		const_1,
		VesselList,
		SpaceCenter,
		SaveFile
	}

	public enum VesselStartSituations
	{
		[Description("#autoLOC_8100075")]
		LANDED = 1,
		[Description("#autoLOC_8000067")]
		PRELAUNCH = 4,
		[Description("#autoLOC_8100077")]
		ORBITING = 0x20
	}

	public StartTypeEnum startType = StartTypeEnum.VesselList;

	public uint startVesselID;

	[SerializeField]
	public DictionaryValueList<VesselSituation, Guid> vesselSituationList;

	public List<IActionModule> startingActions;

	public Mission mission;

	[MEGUI_Button(canBePinned = false, onClick = "OnMissionRosterClick", guiName = "#autoLOC_8003057")]
	public string missionRoster = "";

	[MEGUI_Checkbox(onValueChange = "OnAutoPopulateCrewValueChange", guiName = "#autoLOC_8100071")]
	public bool autoGenerateCrew;

	public KerbalRoster crewRoster;

	[MEGUI_Time(guiName = "#autoLOC_8100072", Tooltip = "#autoLOC_8100174")]
	public double startUT;

	[MEGUI_GameParameters(canBePinned = false, guiName = "#autoLOC_8100073")]
	public GameParameters gameParameters;

	public List<string> parametersDisplayedInSAP;

	[MEGUI_ParameterSwitchCompound(guiName = "#autoLOC_8100074")]
	public MissionPartFilter partFilter;

	public ConfigNode vesselsToBuildNode;

	public ConfigNode startingActionsNode;

	[MEGUI_InputField(CharacterLimit = 12, ContentType = MEGUI_Control.InputContentType.IntegerNumber, onValueChange = "OnResourceSeedChanged", guiName = "#autoLOC_8100182")]
	public int resourceSeed;

	[MEGUI_InputField(CharacterLimit = 12, ContentType = MEGUI_Control.InputContentType.IntegerNumber, onValueChange = "OnResourceROCMissionSeedChanged", guiName = "#autoLOC_6011063")]
	public int rocMissionSeed;

	[MEGUI_Checkbox(onValueChange = "OnShowBriefingChange", guiName = "#autoLOC_8002029", Tooltip = "#autoLOC_8002030")]
	public bool showBriefing;

	[MEGUI_MissionInstructor(onControlCreated = "OninstructorCreated", gapDisplay = true, guiName = "#autoLOC_8006002", Tooltip = "#autoLOC_8006003")]
	public MissionInstructor missionInstructor;

	public MEGUIParameterMissionInstructor instructorControl;

	public uint currentVesselToBuildId;

	public int vesselsBuilt;

	public MENode startNode;

	public DictionaryValueList<VesselSituation, Guid> VesselSituationList => vesselSituationList;

	public List<IActionModule> StartingActions => startingActions;

	public GameScenes startScene
	{
		get
		{
			switch (startType)
			{
			case StartTypeEnum.const_0:
			case StartTypeEnum.const_1:
				return GameScenes.EDITOR;
			case StartTypeEnum.VesselList:
				return GameScenes.FLIGHT;
			default:
				return GameScenes.SPACECENTER;
			}
		}
	}

	public EditorFacility startFacility => startType switch
	{
		StartTypeEnum.const_1 => EditorFacility.const_2, 
		StartTypeEnum.const_0 => EditorFacility.const_1, 
		_ => EditorFacility.None, 
	};

	public bool VesselsArePending
	{
		get
		{
			if (vesselSituationList != null)
			{
				return NumberofVesselsPending > 0;
			}
			return false;
		}
	}

	public int NumberofVesselsPending
	{
		get
		{
			int num = 0;
			for (int i = 0; i < vesselSituationList.Count; i++)
			{
				VesselSituation vesselSituation = vesselSituationList.KeyAt(i);
				if (vesselSituation.playerCreated && !vesselSituation.readyToLaunch)
				{
					num++;
				}
			}
			return num;
		}
	}

	public uint CurrentVesselToBuildId => currentVesselToBuildId;

	public VesselSituation CurrentVesselToBuild
	{
		get
		{
			if (currentVesselToBuildId == 0)
			{
				return null;
			}
			int num = 0;
			while (true)
			{
				if (num < vesselSituationList.Count)
				{
					if (vesselSituationList.KeyAt(num).persistentId == currentVesselToBuildId)
					{
						break;
					}
					num++;
					continue;
				}
				return null;
			}
			return vesselSituationList.KeyAt(num);
		}
	}

	public int VesselsBuilt => vesselsBuilt;

	public MENode StartNode
	{
		get
		{
			if (startNode == null)
			{
				if (MissionEditorLogic.Instance.CurrentSelectedNode != null && MissionEditorLogic.Instance.CurrentSelectedNode.Node.isStartNode)
				{
					startNode = MissionEditorLogic.Instance.CurrentSelectedNode.Node;
				}
				else
				{
					List<MENode>.Enumerator listEnumerator = mission.nodes.GetListEnumerator();
					while (listEnumerator.MoveNext())
					{
						if (listEnumerator.Current.isStartNode)
						{
							startNode = listEnumerator.Current;
							break;
						}
					}
				}
			}
			return startNode;
		}
	}

	public MissionSituation()
	{
		vesselSituationList = new DictionaryValueList<VesselSituation, Guid>();
		startingActions = new List<IActionModule>();
		parametersDisplayedInSAP = new List<string>();
		currentVesselToBuildId = 0u;
		vesselsBuilt = 0;
		vesselsToBuildNode = new ConfigNode();
		showBriefing = false;
		missionInstructor = new MissionInstructor();
		missionRoster = Localizer.Format("#autoLOC_8003056");
	}

	public MissionSituation(Mission mission)
		: this()
	{
		this.mission = mission;
		partFilter = new MissionPartFilter(mission, null);
		crewRoster = KerbalRoster.GenerateInitialCrewRoster(Game.Modes.MISSION);
		gameParameters = GameParameters.GetDefaultParameters(Game.Modes.MISSION, GameParameters.Preset.Normal);
		int i = 0;
		for (int count = crewRoster.Count; i < count; i++)
		{
			KerbalRoster.SetExperienceLevel(crewRoster[i], 5, gameParameters, Game.Modes.MISSION);
		}
	}

	public void InitSituation()
	{
		if (HighLogic.LoadedScene == GameScenes.EDITOR)
		{
			partFilter.ApplyPartsFilter();
		}
		if (ResourceScenario.Instance != null)
		{
			ResourceScenario.Instance.gameSettings.Seed = resourceSeed;
			if (rocMissionSeed != 0)
			{
				ResourceScenario.Instance.gameSettings.ROCMissionSeed = rocMissionSeed;
			}
		}
	}

	public void DestroySituation()
	{
		partFilter.RemovePartsFilter();
	}

	public void OnMissionRosterClick()
	{
		MissionEditorLogic.Instance.Lock(lockTopRight: true, lockSAPGAP: true, lockTool: true, lockKeystrokes: true, lockNodeCanvas: true, "missionBuilder_crewDialog");
		MissionEditorLogic.Instance.crewAssignmentDialog.Spawn(delegate
		{
		}, delegate
		{
			MissionEditorLogic.Instance.Unlock("missionBuilder_crewDialog");
		});
	}

	public void OnResourceSeedChanged(int newResourceSeed)
	{
		ResourceScenario.Instance.gameSettings.Seed = resourceSeed;
	}

	public void OnResourceROCMissionSeedChanged(int newResourceSeed)
	{
		if (rocMissionSeed != 0)
		{
			ResourceScenario.Instance.gameSettings.ROCMissionSeed = rocMissionSeed;
		}
	}

	public void OnAutoPopulateCrewValueChange(bool newValue)
	{
		List<ActionCreateVessel> allActionModules = mission.GetAllActionModules<ActionCreateVessel>();
		int i = 0;
		for (int count = allActionModules.Count; i < count; i++)
		{
			allActionModules[i].vesselSituation.UpdateAutoGenerateCrewSettings(newValue);
		}
	}

	public void OninstructorCreated(MEGUIParameterMissionInstructor parameter)
	{
		instructorControl = parameter;
	}

	public void OnShowBriefingChange(bool value)
	{
		instructorControl.gameObject.SetActive(value);
	}

	public void RunValidationWrapper(MissionEditorValidator validator)
	{
		RunValidation(validator);
	}

	public virtual void RunValidation(MissionEditorValidator validator)
	{
	}

	public void Load(ConfigNode node)
	{
		node.TryGetEnum("startType", ref startType, StartTypeEnum.VesselList);
		node.TryGetValue("startVessel", ref startVesselID);
		node.TryGetValue("autoGenerateCrew", ref autoGenerateCrew);
		vesselsToBuildNode = new ConfigNode();
		node.TryGetNode("VESSELSTOBUILDLIST", ref vesselsToBuildNode);
		startingActionsNode = new ConfigNode();
		node.TryGetNode("STARTINGACTIONSLIST", ref startingActionsNode);
		ConfigNode node2 = new ConfigNode();
		if (node.TryGetNode("ROSTER", ref node2))
		{
			crewRoster = new KerbalRoster(node2, Game.Modes.MISSION);
		}
		node.TryGetValue("startUT", ref startUT);
		ConfigNode node3 = new ConfigNode();
		if (node.TryGetNode("PARAMETERS", ref node3))
		{
			gameParameters.Load(node3);
		}
		ConfigNode node4 = new ConfigNode();
		if (node.TryGetNode("SAP_PARAMETERS", ref node4))
		{
			parametersDisplayedInSAP = node4.GetValuesList("parametersDisplayedInSAP");
		}
		partFilter.Load(node);
		node.TryGetValue("resourceSeed", ref resourceSeed);
		node.TryGetValue("rocMissionSeed", ref rocMissionSeed);
		node.TryGetValue("showBriefing", ref showBriefing);
		missionInstructor.Load(node);
	}

	public void setStartType()
	{
		startType = StartTypeEnum.SpaceCenter;
		List<MENode> dockedNodes = StartNode.dockedNodes;
		if (dockedNodes == null)
		{
			return;
		}
		bool flag = false;
		for (int i = 0; i < dockedNodes.Count; i++)
		{
			List<IActionModule> actionModules = dockedNodes[i].actionModules;
			if (actionModules == null)
			{
				continue;
			}
			for (int j = 0; j < actionModules.Count; j++)
			{
				if (actionModules[j].GetType() == typeof(ActionCreateVessel))
				{
					ActionCreateVessel actionCreateVessel = actionModules[j] as ActionCreateVessel;
					if (!(actionCreateVessel != null))
					{
						continue;
					}
					if (actionCreateVessel.vesselSituation.playerCreated)
					{
						if (startType == StartTypeEnum.SpaceCenter || startType == StartTypeEnum.VesselList)
						{
							startType = ((actionCreateVessel.vesselSituation.location.facility == EditorFacility.const_2) ? StartTypeEnum.const_1 : StartTypeEnum.const_0);
						}
					}
					else if (startType == StartTypeEnum.SpaceCenter)
					{
						startType = StartTypeEnum.VesselList;
					}
					if (!flag)
					{
						startVesselID = actionCreateVessel.vesselSituation.persistentId;
						flag = true;
					}
				}
				else if (actionModules[j].GetType() == typeof(ActionCreateKerbal))
				{
					ActionCreateKerbal actionCreateKerbal = actionModules[j] as ActionCreateKerbal;
					if (actionCreateKerbal != null)
					{
						if (startType == StartTypeEnum.SpaceCenter)
						{
							startType = StartTypeEnum.VesselList;
						}
						if (!flag)
						{
							startVesselID = actionCreateKerbal.persistentId;
							flag = true;
						}
					}
				}
				else if (actionModules[j].GetType() == typeof(ActionCreateAsteroid))
				{
					ActionCreateAsteroid actionCreateAsteroid = actionModules[j] as ActionCreateAsteroid;
					if (actionCreateAsteroid != null)
					{
						if (startType == StartTypeEnum.SpaceCenter)
						{
							startType = StartTypeEnum.VesselList;
						}
						if (!flag)
						{
							startVesselID = actionCreateAsteroid.PersistentID;
							flag = true;
						}
					}
				}
				else
				{
					if (!(actionModules[j].GetType() == typeof(ActionCreateComet)))
					{
						continue;
					}
					ActionCreateComet actionCreateComet = actionModules[j] as ActionCreateComet;
					if (actionCreateComet != null)
					{
						if (startType == StartTypeEnum.SpaceCenter)
						{
							startType = StartTypeEnum.VesselList;
						}
						if (!flag)
						{
							startVesselID = actionCreateComet.PersistentID;
							flag = true;
						}
					}
				}
			}
		}
	}

	public void SaveVesselsToBuild(ConfigNode node)
	{
		if (vesselsToBuildNode != null)
		{
			node.AddNode("VESSELSTOBUILDLIST", vesselsToBuildNode);
		}
		if (startingActionsNode != null)
		{
			node.AddNode("STARTINGACTIONSLIST", startingActionsNode);
		}
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("startType", startType);
		node.AddValue("startVessel", startVesselID);
		node.AddValue("autoGenerateCrew", autoGenerateCrew);
		if (HighLogic.LoadedScene == GameScenes.MISSIONBUILDER)
		{
			setStartType();
			node.SetValue("startType", startType.ToString());
			node.SetValue("startVessel", startVesselID, createIfNotFound: true);
		}
		if (HighLogic.CurrentGame != null && HighLogic.CurrentGame.Mode == Game.Modes.MISSION)
		{
			if (vesselSituationList.Count > 0)
			{
				ConfigNode configNode = node.AddNode("VESSELSTOBUILDLIST");
				for (int i = 0; i < vesselSituationList.Count; i++)
				{
					configNode.AddNode("VESSELTOBUILD").AddValue("NodeGuid", vesselSituationList.At(i));
				}
			}
			if (startingActions.Count > 0)
			{
				ConfigNode configNode2 = node.AddNode("STARTINGACTIONSLIST");
				for (int j = 0; j < startingActions.Count; j++)
				{
					configNode2.AddNode("STARTINGACTION").AddValue("NodeGuid", startingActions[j].GetNode().id);
				}
			}
		}
		ConfigNode node2 = node.AddNode("ROSTER");
		crewRoster.Save(node2);
		node.AddValue("startUT", startUT);
		ConfigNode node3 = node.AddNode("PARAMETERS");
		gameParameters.Save(node3);
		ConfigNode configNode3 = node.AddNode("SAP_PARAMETERS");
		for (int k = 0; k < parametersDisplayedInSAP.Count; k++)
		{
			configNode3.AddValue("parametersDisplayedInSAP", parametersDisplayedInSAP[k]);
		}
		partFilter.Save(node);
		if (ResourceScenario.Instance != null)
		{
			node.AddValue("resourceSeed", ResourceScenario.Instance.gameSettings.Seed);
			node.AddValue("rocMissionSeed", ResourceScenario.Instance.gameSettings.ROCMissionSeed);
		}
		else
		{
			node.AddValue("resourceSeed", resourceSeed);
			node.AddValue("rocMissionSeed", rocMissionSeed);
		}
		node.AddValue("showBriefing", showBriefing);
		missionInstructor.Save(node);
	}

	public VesselSituation GetVesselSituationByVesselID(uint PersistentId)
	{
		int num = 0;
		while (true)
		{
			if (num < vesselSituationList.Count)
			{
				if (vesselSituationList.KeyAt(num).persistentId == PersistentId)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return vesselSituationList.KeyAt(num);
	}

	public bool VesselSituationListContains(uint persistentId)
	{
		int num = 0;
		while (true)
		{
			if (num < vesselSituationList.Count)
			{
				if (vesselSituationList.KeyAt(num).persistentId == persistentId)
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

	public void FillVesselSituationList(MENode missionStartNode, bool replaceIfFound = false, bool fillAllNodes = false)
	{
		vesselSituationList.Clear();
		startingActions.Clear();
		List<MENode> list = new List<MENode>();
		list = ((!fillAllNodes) ? missionStartNode.dockedNodes : mission.nodes.ValuesList);
		for (int i = 0; i < list.Count; i++)
		{
			List<IActionModule> actionModules = list[i].actionModules;
			if (actionModules == null)
			{
				continue;
			}
			for (int j = 0; j < actionModules.Count; j++)
			{
				if (actionModules[j].GetType() == typeof(ActionCreateVessel))
				{
					ActionCreateVessel actionCreateVessel = actionModules[j] as ActionCreateVessel;
					if (!(actionCreateVessel != null))
					{
						continue;
					}
					if (actionCreateVessel.vesselSituation.playerCreated)
					{
						actionCreateVessel.vesselSituation.readyToLaunch = false;
						if (fillAllNodes)
						{
							actionCreateVessel.CreatePlaceholderCraft();
						}
					}
					else
					{
						actionCreateVessel.vesselSituation.readyToLaunch = true;
					}
					if (!replaceIfFound && VesselSituationListContains(actionCreateVessel.vesselSituation.persistentId))
					{
						Debug.Log("[MissionSituation]: Skipped adding " + actionCreateVessel.vesselSituation.vesselName + " to Vessel Create List");
					}
					else if (vesselSituationList.Add(actionCreateVessel.vesselSituation, actionCreateVessel.node.id))
					{
						Debug.Log("[MissionSituation]: Replaced " + actionCreateVessel.vesselSituation.vesselName + " in Vessel Create List");
					}
				}
				else
				{
					startingActions.Add(actionModules[j]);
				}
			}
		}
		UpdateVesselBuildValues();
	}

	public void resetVesselsCountsLists()
	{
		currentVesselToBuildId = 0u;
		vesselsBuilt = 0;
	}

	public void UpdateVesselBuildValues()
	{
		if (!VesselsArePending)
		{
			return;
		}
		vesselsBuilt = 0;
		for (int i = 0; i < vesselSituationList.Count; i++)
		{
			if (vesselSituationList.KeyAt(i).playerCreated && (!vesselSituationList.KeyAt(i).playerCreated || !vesselSituationList.KeyAt(i).launched))
			{
				if (vesselSituationList.KeyAt(i).readyToLaunch)
				{
					vesselsBuilt++;
				}
				else if (currentVesselToBuildId == 0)
				{
					currentVesselToBuildId = vesselSituationList.KeyAt(i).persistentId;
					GameEvents.Mission.onMissionCurrentVesselToBuildChanged.Fire(mission, vesselSituationList.KeyAt(i));
				}
			}
		}
	}

	public List<VesselSituation> GetAvailableVesselSituations()
	{
		List<VesselSituation> list = new List<VesselSituation>();
		List<MENode>.Enumerator listEnumerator = mission.nodes.GetListEnumerator();
		while (listEnumerator.MoveNext())
		{
			List<IActionModule> actionModules = listEnumerator.Current.actionModules;
			if (actionModules == null)
			{
				continue;
			}
			for (int i = 0; i < actionModules.Count; i++)
			{
				if (actionModules[i].GetType() == typeof(ActionCreateVessel))
				{
					ActionCreateVessel actionCreateVessel = actionModules[i] as ActionCreateVessel;
					if (actionCreateVessel != null)
					{
						list.Add(actionCreateVessel.vesselSituation);
					}
				}
			}
		}
		return list;
	}

	public bool AddNodeWithVesselToBuild(MENode node)
	{
		if (HighLogic.CurrentGame != null && HighLogic.CurrentGame.Mode == Game.Modes.MISSION)
		{
			if (mission.GetNodeById(node.id) == null)
			{
				return false;
			}
			List<IActionModule> actionModules = node.actionModules;
			for (int i = 0; i < actionModules.Count; i++)
			{
				ActionCreateVessel actionCreateVessel = actionModules[i] as ActionCreateVessel;
				if (actionCreateVessel != null)
				{
					if (!VesselSituationListContains(actionCreateVessel.vesselSituation.persistentId))
					{
						vesselSituationList.Add(actionCreateVessel.vesselSituation, node.id);
						return true;
					}
					Debug.Log("[MissionSituation]: Skipped adding duplicate " + actionCreateVessel.vesselSituation.vesselName + " to Vessel Create List");
					return false;
				}
			}
		}
		return false;
	}

	public bool VesselSituationReadyToLaunch(VesselSituation situation)
	{
		Guid nodeGuidByVesselID = mission.GetNodeGuidByVesselID(situation.persistentId);
		if (nodeGuidByVesselID != Guid.Empty)
		{
			MENode nodeById = mission.GetNodeById(nodeGuidByVesselID);
			if (nodeById != null)
			{
				ActionCreateVessel actionCreateVessel = mission.GetActionCreateVessel(nodeById);
				if (actionCreateVessel != null)
				{
					actionCreateVessel.vesselSituation.readyToLaunch = true;
					return true;
				}
			}
		}
		return false;
	}

	public bool VesselSituationRevertLaunch(VesselSituation situation)
	{
		Guid nodeGuidByVesselID = mission.GetNodeGuidByVesselID(situation.persistentId);
		if (nodeGuidByVesselID != Guid.Empty)
		{
			MENode nodeById = mission.GetNodeById(nodeGuidByVesselID);
			if (nodeById != null)
			{
				ActionCreateVessel actionCreateVessel = mission.GetActionCreateVessel(nodeById);
				if (actionCreateVessel != null)
				{
					actionCreateVessel.vesselSituation.readyToLaunch = false;
					return true;
				}
			}
		}
		return false;
	}

	public bool VesselSituationLaunched(VesselSituation situation)
	{
		Guid nodeGuidByVesselID = mission.GetNodeGuidByVesselID(situation.persistentId);
		if (nodeGuidByVesselID != Guid.Empty)
		{
			MENode nodeById = mission.GetNodeById(nodeGuidByVesselID);
			if (nodeById != null)
			{
				ActionCreateVessel actionCreateVessel = mission.GetActionCreateVessel(nodeById);
				if (actionCreateVessel != null)
				{
					actionCreateVessel.vesselSituation.launched = true;
					return true;
				}
			}
		}
		return false;
	}

	public void RemoveLaunchedVessels()
	{
		List<VesselSituation> list = new List<VesselSituation>();
		for (int i = 0; i < vesselSituationList.Count; i++)
		{
			if (vesselSituationList.KeyAt(i).launched)
			{
				list.Add(vesselSituationList.KeyAt(i));
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			vesselSituationList.Remove(list[j]);
		}
	}

	public string GetName()
	{
		return "MissionSituation";
	}

	public string GetDisplayName()
	{
		return Localizer.Format("#autoLOC_8004154");
	}

	public void AddParameterToSAP(string parameter)
	{
		parametersDisplayedInSAP.AddUnique(parameter);
	}

	public void RemoveParameterFromSAP(string parameter)
	{
		parametersDisplayedInSAP.Remove(parameter);
		if (StartNode.HasNodeBodyParameter("MissionSituation", parameter))
		{
			StartNode.RemoveParameterFromNodeBody("MissionSituation", parameter);
			UpdateNodeBodyUI();
		}
	}

	public void AddParameterToNodeBody(string parameter)
	{
		StartNode.AddParameterToNodeBody("MissionSituation", parameter);
	}

	public void AddParameterToNodeBodyAndUpdateUI(string parameter)
	{
		StartNode.AddParameterToNodeBody("MissionSituation", parameter);
		UpdateNodeBodyUI();
	}

	public void RemoveParameterFromNodeBody(string parameter)
	{
		StartNode.RemoveParameterFromNodeBody("MissionSituation", parameter);
	}

	public void RemoveParameterFromNodeBodyAndUpdateUI(string parameter)
	{
		StartNode.RemoveParameterFromNodeBody("MissionSituation", parameter);
		UpdateNodeBodyUI();
	}

	public bool HasNodeBodyParameter(string parameter)
	{
		return StartNode.HasNodeBodyParameter("MissionSituation", parameter);
	}

	public bool HasSAPParameter(string parameter)
	{
		return parametersDisplayedInSAP.Contains(parameter);
	}

	public virtual string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "instructorName")
		{
			if (field.GetValue<string>() == "Instructor_Wernher")
			{
				return Localizer.Format("#autoLOC_8006002") + ": " + Localizer.Format("#autoLOC_900000");
			}
			if (field.GetValue<string>() == "Instructor_Gene")
			{
				return Localizer.Format("#autoLOC_8006002") + ": " + Localizer.Format("#autoLOC_900001");
			}
			if (field.GetValue<string>() == "Strategy_MechanicGuy")
			{
				return Localizer.Format("#autoLOC_8006002") + ": " + Localizer.Format("#autoLOC_900002");
			}
			if (field.GetValue<string>() == "Strategy_Mortimer")
			{
				return Localizer.Format("#autoLOC_8006002") + ": " + Localizer.Format("#autoLOC_900004");
			}
			if (field.GetValue<string>() == "Strategy_PRGuy")
			{
				return Localizer.Format("#autoLOC_8006002") + ": " + Localizer.Format("#autoLOC_900005");
			}
			if (field.GetValue<string>() == "Strategy_ScienceGuy")
			{
				return Localizer.Format("#autoLOC_8006002") + ": " + Localizer.Format("#autoLOC_900006");
			}
		}
		if (field.FieldType.Name.Equals("List`1"))
		{
			return Localizer.Format("#autoLOC_8004190", field.guiName, ((IList)field.GetValue()).Count);
		}
		if (field.name == "startUT")
		{
			if (startUT != 0.0)
			{
				return Localizer.Format("#autoLOC_8004190", field.guiName, KSPUtil.PrintDateCompact(startUT, includeTime: true));
			}
			return Localizer.Format("#autoLOC_8004190", field.guiName, KSPUtil.PrintDateDeltaCompact(startUT, includeTime: true, includeSeconds: true));
		}
		return Localizer.Format("#autoLOC_8004190", field.guiName, MissionsUtils.GetFieldValueForDisplay(field));
	}

	public void UpdateNodeBodyUI()
	{
		StartNode.guiNode.DisplayNodeBodyParameters();
	}

	public List<IMENodeDisplay> GetInternalParametersToDisplay()
	{
		return new List<IMENodeDisplay>();
	}

	public MENode GetNode()
	{
		return null;
	}

	public string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004153");
	}

	public void ParameterSetupComplete()
	{
		OnShowBriefingChange(showBriefing);
	}

	public void Initialize()
	{
	}

	public void Destroy()
	{
	}
}
