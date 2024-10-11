using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;
using UnityEngine;

namespace Expansions.Missions;

[Serializable]
public class MissionSituation : IConfigNode, IMENodeDisplay
{
	public enum StartTypeEnum
	{
		VAB,
		SPH,
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

	public StartTypeEnum startType;

	public uint startVesselID;

	[SerializeField]
	internal DictionaryValueList<VesselSituation, Guid> vesselSituationList;

	internal List<IActionModule> startingActions;

	public Mission mission;

	[MEGUI_Button(canBePinned = false, onClick = "OnMissionRosterClick", guiName = "#autoLOC_8003057")]
	public string missionRoster;

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

	internal ConfigNode vesselsToBuildNode;

	internal ConfigNode startingActionsNode;

	[MEGUI_InputField(CharacterLimit = 12, ContentType = MEGUI_Control.InputContentType.IntegerNumber, onValueChange = "OnResourceSeedChanged", guiName = "#autoLOC_8100182")]
	public int resourceSeed;

	[MEGUI_InputField(CharacterLimit = 12, ContentType = MEGUI_Control.InputContentType.IntegerNumber, onValueChange = "OnResourceROCMissionSeedChanged", guiName = "#autoLOC_6011063")]
	public int rocMissionSeed;

	[MEGUI_Checkbox(onValueChange = "OnShowBriefingChange", guiName = "#autoLOC_8002029", Tooltip = "#autoLOC_8002030")]
	public bool showBriefing;

	[MEGUI_MissionInstructor(onControlCreated = "OninstructorCreated", gapDisplay = true, guiName = "#autoLOC_8006002", Tooltip = "#autoLOC_8006003")]
	public MissionInstructor missionInstructor;

	private MEGUIParameterMissionInstructor instructorControl;

	internal uint currentVesselToBuildId;

	internal int vesselsBuilt;

	private MENode startNode;

	public DictionaryValueList<VesselSituation, Guid> VesselSituationList
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<IActionModule> StartingActions
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public GameScenes startScene
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public EditorFacility startFacility
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool VesselsArePending
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int NumberofVesselsPending
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public uint CurrentVesselToBuildId
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public VesselSituation CurrentVesselToBuild
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int VesselsBuilt
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	private MENode StartNode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MissionSituation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MissionSituation(Mission mission)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void InitSituation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DestroySituation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnMissionRosterClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnResourceSeedChanged(int newResourceSeed)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnResourceROCMissionSeedChanged(int newResourceSeed)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnAutoPopulateCrewValueChange(bool newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void OninstructorCreated(MEGUIParameterMissionInstructor parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void OnShowBriefingChange(bool value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void RunValidationWrapper(MissionEditorValidator validator)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void RunValidation(MissionEditorValidator validator)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void setStartType()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SaveVesselsToBuild(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselSituation GetVesselSituationByVesselID(uint PersistentId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool VesselSituationListContains(uint persistentId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FillVesselSituationList(MENode missionStartNode, bool replaceIfFound = false, bool fillAllNodes = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void resetVesselsCountsLists()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateVesselBuildValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<VesselSituation> GetAvailableVesselSituations()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool AddNodeWithVesselToBuild(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool VesselSituationReadyToLaunch(VesselSituation situation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool VesselSituationRevertLaunch(VesselSituation situation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool VesselSituationLaunched(VesselSituation situation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveLaunchedVessels()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddParameterToSAP(string parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveParameterFromSAP(string parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddParameterToNodeBody(string parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddParameterToNodeBodyAndUpdateUI(string parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveParameterFromNodeBody(string parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveParameterFromNodeBodyAndUpdateUI(string parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasNodeBodyParameter(string parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasSAPParameter(string parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual string GetNodeBodyParameterString(BaseAPField field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateNodeBodyUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<IMENodeDisplay> GetInternalParametersToDisplay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MENode GetNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ParameterSetupComplete()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Initialize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Destroy()
	{
		throw null;
	}
}
