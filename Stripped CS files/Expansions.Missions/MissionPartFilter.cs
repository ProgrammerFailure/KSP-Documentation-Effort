using System.Collections.Generic;
using System.ComponentModel;
using Expansions.Missions.Editor;
using ns11;
using ns9;
using UnityEngine;

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
	public Choices filterType = Choices.unavailableParts;

	[MEGUI_PartPicker(getExcludedPartsFilter = "GetExcludedPartsFilterDictionary", checkpointValidation = CheckpointValidationType.CustomMethod, compareValuesForCheckpoint = "OnAvailablePartsCheckpointValidator", updatePartnerExcludedPartsFilter = "OnPartPickerNeedsToUpdateExcludedPartsFilter", onControlCreated = "OnAvailablePartsParameterCreated", gapDisplay = true, guiName = "#autoLOC_8100288", DialogTitle = "#autoLOC_8100299", SelectedPartsColorString = "#00FF00")]
	public List<string> availableParts;

	public MEGUIParameterPartPicker availablePartsParameter;

	[MEGUI_PartPicker(getExcludedPartsFilter = "GetExcludedPartsFilterDictionary", checkpointValidation = CheckpointValidationType.CustomMethod, compareValuesForCheckpoint = "OnUnavailablePartsCheckpointValidator", updatePartnerExcludedPartsFilter = "OnPartPickerNeedsToUpdateExcludedPartsFilter", onControlCreated = "OnUnavailablePartsParameterCreated", gapDisplay = true, guiName = "#autoLOC_8100289", DialogTitle = "#autoLOC_8100300", SelectedPartsColorString = "#FF0000")]
	public List<string> unavailableParts;

	public MEGUIParameterPartPicker unavailablePartsParameter;

	public Callback updatePartnerExcludedPartFilter;

	public VesselSituation vesselSituation;

	public Mission mission;

	public MissionPartFilter(Mission mission, VesselSituation vesselSituation)
	{
		availableParts = new List<string>();
		unavailableParts = new List<string>();
		this.mission = mission;
		this.vesselSituation = vesselSituation;
	}

	public Dictionary<string, List<string>> GetExcludedPartsFilterDictionary()
	{
		Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
		if (vesselSituation != null)
		{
			List<string> excludedParts = mission.situation.partFilter.GetExcludedParts();
			dictionary.Add("#autoLOC_8100294", excludedParts);
			dictionary.Add("#autoLOC_8100295", vesselSituation.requiredParts);
		}
		else
		{
			foreach (VesselSituation availableVesselSituation in mission.situation.GetAvailableVesselSituations())
			{
				if (availableVesselSituation.partFilter.filterType == Choices.availableParts)
				{
					string key = Localizer.Format("#autoLOC_8100296");
					if (!dictionary.ContainsKey(key))
					{
						dictionary.Add(key, availableVesselSituation.partFilter.availableParts);
					}
					else
					{
						for (int i = 0; i < availableVesselSituation.partFilter.availableParts.Count; i++)
						{
							dictionary[key].AddUnique(availableVesselSituation.partFilter.availableParts[i]);
						}
					}
				}
				else
				{
					string key2 = Localizer.Format("#autoLOC_8100297");
					if (!dictionary.ContainsKey(key2))
					{
						dictionary.Add(key2, availableVesselSituation.partFilter.unavailableParts);
					}
					else
					{
						for (int j = 0; j < availableVesselSituation.partFilter.unavailableParts.Count; j++)
						{
							dictionary[key2].AddUnique(availableVesselSituation.partFilter.unavailableParts[j]);
						}
					}
				}
				string key3 = Localizer.Format("#autoLOC_8100298");
				if (!dictionary.ContainsKey(key3))
				{
					dictionary.Add(key3, availableVesselSituation.requiredParts);
					continue;
				}
				for (int k = 0; k < availableVesselSituation.requiredParts.Count; k++)
				{
					dictionary[key3].AddUnique(availableVesselSituation.requiredParts[k]);
				}
			}
		}
		return dictionary;
	}

	public List<string> GetExcludedParts()
	{
		if (filterType == Choices.unavailableParts)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < unavailableParts.Count; i++)
			{
				list.Add(unavailableParts[i]);
			}
			return list;
		}
		List<AvailablePart> loadedPartsList = PartLoader.LoadedPartsList;
		List<string> list2 = new List<string>();
		int j = 0;
		for (int count = loadedPartsList.Count; j < count; j++)
		{
			if (!availableParts.Contains(loadedPartsList[j].name))
			{
				list2.AddUnique(loadedPartsList[j].name);
			}
			else
			{
				Debug.Log(loadedPartsList[j].name);
			}
		}
		return list2;
	}

	public void ApplyPartsFilter()
	{
		string text = ((vesselSituation == null) ? "Mission Parts" : "Vessel Parts");
		if (EditorPartList.Instance != null)
		{
			if (EditorPartList.Instance.ExcludeFilters[text] != null)
			{
				EditorPartList.Instance.ExcludeFilters.RemoveFilter(text);
			}
			EditorPartList.Instance.ExcludeFilters.AddFilter(new EditorPartListFilter<AvailablePart>(text, CheckPartExcludelist));
			EditorPartList.Instance.Refresh(EditorPartList.State.PartsList);
		}
	}

	public void RemovePartsFilter()
	{
		string text = ((vesselSituation == null) ? "Mission Parts" : "Vessel Parts");
		if (EditorPartList.Instance != null)
		{
			if (EditorPartList.Instance.ExcludeFilters[text] != null)
			{
				EditorPartList.Instance.ExcludeFilters.RemoveFilter(text);
			}
			EditorPartList.Instance.Refresh(EditorPartList.State.PartsList);
		}
	}

	public bool CheckPartExcludelist(AvailablePart ap)
	{
		if (filterType == Choices.unavailableParts)
		{
			if (unavailableParts.Count > 0 && unavailableParts.IndexOf(ap.name) >= 0)
			{
				return false;
			}
		}
		else if (availableParts.Count > 0 && availableParts.IndexOf(ap.name) < 0)
		{
			return false;
		}
		return true;
	}

	public void UpdateExcludedPartsFilter()
	{
		availablePartsParameter.UpdateDisplayedParts();
		unavailablePartsParameter.UpdateDisplayedParts();
	}

	public void SetUpdatePartnerExcludedPartFilterCallback(Callback newCallback)
	{
		updatePartnerExcludedPartFilter = newCallback;
	}

	public void OnFilterValueChange(Choices newFilter)
	{
		if (vesselSituation == null || vesselSituation.requiredParts == null)
		{
			return;
		}
		if (newFilter == Choices.availableParts)
		{
			bool flag = false;
			for (int i = 0; i < vesselSituation.requiredParts.Count; i++)
			{
				if (!availableParts.Contains(vesselSituation.requiredParts[i]))
				{
					availableParts.Add(vesselSituation.requiredParts[i]);
					flag = true;
				}
			}
			if (flag)
			{
				availablePartsParameter.RefreshUI();
			}
			return;
		}
		bool flag2 = false;
		for (int j = 0; j < vesselSituation.requiredParts.Count; j++)
		{
			if (unavailableParts.Contains(vesselSituation.requiredParts[j]))
			{
				flag2 = true;
				unavailableParts.Remove(vesselSituation.requiredParts[j]);
			}
		}
		if (flag2)
		{
			unavailablePartsParameter.RefreshUI();
		}
	}

	public void OnAvailablePartsParameterCreated(MEGUIParameterPartPicker parameter)
	{
		availablePartsParameter = parameter;
	}

	public void OnUnavailablePartsParameterCreated(MEGUIParameterPartPicker parameter)
	{
		unavailablePartsParameter = parameter;
	}

	public void OnPartPickerNeedsToUpdateExcludedPartsFilter()
	{
		if (updatePartnerExcludedPartFilter != null)
		{
			updatePartnerExcludedPartFilter();
		}
	}

	public bool OnAvailablePartsCheckpointValidator(List<string> value)
	{
		if (filterType == Choices.unavailableParts)
		{
			return true;
		}
		return MissionCheckpointValidator.CompareObjectLists(availableParts, value);
	}

	public bool OnUnavailablePartsCheckpointValidator(List<string> value)
	{
		if (filterType == Choices.availableParts)
		{
			return true;
		}
		return MissionCheckpointValidator.CompareObjectLists(unavailableParts, value);
	}

	public void Load(ConfigNode node)
	{
		node.TryGetEnum("filterType", ref filterType, Choices.unavailableParts);
		if (filterType == Choices.availableParts)
		{
			availableParts = node.GetValuesList("availableParts");
			MissionsUtils.UpdatePartNames(ref availableParts);
		}
		else
		{
			unavailableParts = node.GetValuesList("unavailableParts");
			MissionsUtils.UpdatePartNames(ref unavailableParts);
		}
	}

	public void Save(ConfigNode node)
	{
		if (filterType == Choices.availableParts)
		{
			for (int i = 0; i < availableParts.Count; i++)
			{
				if (i == 0)
				{
					node.AddValue("filterType", filterType);
				}
				node.AddValue("availableParts", availableParts[i]);
			}
			return;
		}
		for (int j = 0; j < unavailableParts.Count; j++)
		{
			if (j == 0)
			{
				node.AddValue("filterType", filterType);
			}
			node.AddValue("unavailableParts", unavailableParts[j]);
		}
	}
}
