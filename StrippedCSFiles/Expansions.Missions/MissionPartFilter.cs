using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;

namespace Expansions.Missions;

public class MissionPartFilter : IConfigNode
{
	public enum Choices
	{
		[Description("#autoLOC_8100288")]
		availableParts,
		[Description("#autoLOC_8100289")]
		unavailableParts
	}

	[MEGUI_ParameterSwitchCompound_KeyField(onValueChange = "OnFilterValueChange", gapDisplay = false, guiName = "#autoLOC_8100317")]
	public Choices filterType;

	[MEGUI_PartPicker(getExcludedPartsFilter = "GetExcludedPartsFilterDictionary", checkpointValidation = CheckpointValidationType.CustomMethod, compareValuesForCheckpoint = "OnAvailablePartsCheckpointValidator", updatePartnerExcludedPartsFilter = "OnPartPickerNeedsToUpdateExcludedPartsFilter", onControlCreated = "OnAvailablePartsParameterCreated", gapDisplay = true, guiName = "#autoLOC_8100288", DialogTitle = "#autoLOC_8100299", SelectedPartsColorString = "#00FF00")]
	private List<string> availableParts;

	private MEGUIParameterPartPicker availablePartsParameter;

	[MEGUI_PartPicker(getExcludedPartsFilter = "GetExcludedPartsFilterDictionary", checkpointValidation = CheckpointValidationType.CustomMethod, compareValuesForCheckpoint = "OnUnavailablePartsCheckpointValidator", updatePartnerExcludedPartsFilter = "OnPartPickerNeedsToUpdateExcludedPartsFilter", onControlCreated = "OnUnavailablePartsParameterCreated", gapDisplay = true, guiName = "#autoLOC_8100289", DialogTitle = "#autoLOC_8100300", SelectedPartsColorString = "#FF0000")]
	private List<string> unavailableParts;

	private MEGUIParameterPartPicker unavailablePartsParameter;

	private Callback updatePartnerExcludedPartFilter;

	private VesselSituation vesselSituation;

	private Mission mission;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MissionPartFilter(Mission mission, VesselSituation vesselSituation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Dictionary<string, List<string>> GetExcludedPartsFilterDictionary()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<string> GetExcludedParts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ApplyPartsFilter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemovePartsFilter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool CheckPartExcludelist(AvailablePart ap)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateExcludedPartsFilter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetUpdatePartnerExcludedPartFilterCallback(Callback newCallback)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFilterValueChange(Choices newFilter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnAvailablePartsParameterCreated(MEGUIParameterPartPicker parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUnavailablePartsParameterCreated(MEGUIParameterPartPicker parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartPickerNeedsToUpdateExcludedPartsFilter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool OnAvailablePartsCheckpointValidator(List<string> value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool OnUnavailablePartsCheckpointValidator(List<string> value)
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
