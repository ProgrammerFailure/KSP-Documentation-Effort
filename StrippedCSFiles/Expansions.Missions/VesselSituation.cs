using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;
using UnityEngine;

namespace Expansions.Missions;

[Serializable]
public class VesselSituation : IConfigNode
{
	public bool playerCreated;

	public string craftFile;

	public uint persistentId;

	public string vesselName;

	[MEGUI_TextArea(tabStop = true, guiName = "#autoLOC_8002017", Tooltip = "#autoLOC_8002018")]
	public string vesselDescription;

	[MEGUI_VesselLocation(checkpointValidation = CheckpointValidationType.Controls, guiName = "#autoLOC_8100085", Tooltip = "#autoLOC_8100088")]
	public VesselLocation location;

	[MEGUI_DynamicModuleList(allowMultipleModuleInstances = true, guiName = "#autoLOC_8100089", Tooltip = "#autoLOC_8100090")]
	public VesselRestrictionList vesselRestrictionList;

	public Mission mission;

	public MENode node;

	public List<Crew> vesselCrew;

	[MEGUI_Checkbox(canBePinned = true, guiName = "#autoLOC_8100091", Tooltip = "#autoLOC_8100092")]
	public bool focusonSpawn;

	[MEGUI_Checkbox(onControlCreated = "OnAutoGenerateCrewControlCreated", guiName = "#autoLOC_8100071")]
	public bool autoGenerateCrew;

	[SerializeField]
	internal bool readyToLaunch;

	[SerializeField]
	internal bool launched;

	[MEGUI_ParameterSwitchCompound(checkpointValidation = CheckpointValidationType.Controls, guiName = "#autoLOC_8100074")]
	public MissionPartFilter partFilter;

	[MEGUI_PartPicker(getExcludedPartsFilter = "GetRequiredExcludedParts", checkpointValidation = CheckpointValidationType.CustomMethod, compareValuesForCheckpoint = "OnRequieredPartsCheckpointValidator", updatePartnerExcludedPartsFilter = "OnPartPickerNeedsToUpdateExcludedPartsFilter", onControlCreated = "OnRequiredPartsParameterCreated", gapDisplay = true, guiName = "#autoLOC_8100094", DialogTitle = "#autoLOC_8000204", Tooltip = "#autoLOC_8000204", SelectedPartsColorString = "#0000FF")]
	public List<string> requiredParts;

	private MEGUIParameterPartPicker requiredPartsParameter;

	private MEGUIParameterCheckbox autoGenerateCrewParameter;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselSituation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselSituation(Mission mission, MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetVesselCrew(KerbalRoster crewRoster, VesselCrewManifest newCrew)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetCrewAvailable(KerbalRoster crewRoster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Dictionary<string, List<string>> GetRequiredExcludedParts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void StartEditorEvents()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearEditorEvents()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateRequiredPartsDisplayedParts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateAutoGenerateCrewSettings(bool newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnRequiredPartsParameterCreated(MEGUIParameterPartPicker parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartPickerNeedsToUpdateExcludedPartsFilter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnAutoGenerateCrewControlCreated(MEGUIParameterCheckbox parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool OnRequieredPartsCheckpointValidator(List<string> value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}
}
