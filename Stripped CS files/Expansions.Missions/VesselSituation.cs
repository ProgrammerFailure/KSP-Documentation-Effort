using System;
using System.Collections.Generic;
using Expansions.Missions.Editor;
using ns9;
using UnityEngine;

namespace Expansions.Missions;

[Serializable]
public class VesselSituation : IConfigNode
{
	public bool playerCreated = true;

	public string craftFile = "";

	public uint persistentId;

	public string vesselName = "#autoLOC_8100086";

	[MEGUI_TextArea(tabStop = true, guiName = "#autoLOC_8002017", Tooltip = "#autoLOC_8002018")]
	public string vesselDescription = "";

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
	public bool readyToLaunch;

	[SerializeField]
	public bool launched;

	[MEGUI_ParameterSwitchCompound(checkpointValidation = CheckpointValidationType.Controls, guiName = "#autoLOC_8100074")]
	public MissionPartFilter partFilter;

	[MEGUI_PartPicker(getExcludedPartsFilter = "GetRequiredExcludedParts", checkpointValidation = CheckpointValidationType.CustomMethod, compareValuesForCheckpoint = "OnRequieredPartsCheckpointValidator", updatePartnerExcludedPartsFilter = "OnPartPickerNeedsToUpdateExcludedPartsFilter", onControlCreated = "OnRequiredPartsParameterCreated", gapDisplay = true, guiName = "#autoLOC_8100094", DialogTitle = "#autoLOC_8000204", Tooltip = "#autoLOC_8000204", SelectedPartsColorString = "#0000FF")]
	public List<string> requiredParts;

	public MEGUIParameterPartPicker requiredPartsParameter;

	public MEGUIParameterCheckbox autoGenerateCrewParameter;

	public VesselSituation()
	{
		persistentId = 0u;
		vesselCrew = new List<Crew>();
		launched = false;
		readyToLaunch = false;
		focusonSpawn = false;
		requiredParts = new List<string>();
	}

	public VesselSituation(Mission mission, MENode node)
		: this()
	{
		persistentId = FlightGlobals.GetUniquepersistentId();
		vesselName = Localizer.Format("#autoLOC_8100086");
		location = new VesselLocation(node);
		this.mission = mission;
		this.node = node;
		partFilter = new MissionPartFilter(mission, this);
		partFilter.SetUpdatePartnerExcludedPartFilterCallback(UpdateRequiredPartsDisplayedParts);
		vesselRestrictionList = new VesselRestrictionList(node);
	}

	public void SetVesselCrew(KerbalRoster crewRoster, VesselCrewManifest newCrew)
	{
		if (MissionEditorLogic.Instance != null)
		{
			MissionEditorLogic.Instance.Unlock("missionBuilder_SetVesselCrew");
		}
		List<string> list = new List<string>();
		for (int i = 0; i < vesselCrew.Count; i++)
		{
			for (int j = 0; j < vesselCrew[i].crewNames.Count; j++)
			{
				list.Add(vesselCrew[i].crewNames[j]);
			}
		}
		List<Crew> list2 = new List<Crew>();
		for (int k = 0; k < newCrew.PartManifests.Count; k++)
		{
			PartCrewManifest partCrewManifest = newCrew.PartManifests[k];
			if (partCrewManifest.partCrew.Length == 0)
			{
				continue;
			}
			Crew crew = new Crew();
			crew.crewNames = new List<string>();
			for (int l = 0; l < partCrewManifest.partCrew.Length; l++)
			{
				if (partCrewManifest.partCrew[l] != "")
				{
					crew.crewNames.Add(partCrewManifest.partCrew[l]);
					list.Remove(partCrewManifest.partCrew[l]);
					ProtoCrewMember protoCrewMember = crewRoster[partCrewManifest.partCrew[l]];
					if (protoCrewMember != null)
					{
						protoCrewMember.seatIdx = partCrewManifest.GetCrewSeat(protoCrewMember);
						protoCrewMember.rosterStatus = ProtoCrewMember.RosterStatus.Assigned;
					}
				}
			}
			if (crew.crewNames.Count > 0)
			{
				crew.partPersistentID = partCrewManifest.PartID;
				crew.vesselPersistentID = persistentId;
				list2.Add(crew);
			}
		}
		for (int m = 0; m < list.Count; m++)
		{
			ProtoCrewMember protoCrewMember2 = crewRoster[list[m]];
			if (protoCrewMember2 != null)
			{
				protoCrewMember2.seatIdx = -1;
				protoCrewMember2.rosterStatus = ProtoCrewMember.RosterStatus.Available;
			}
		}
		vesselCrew = list2;
	}

	public void SetCrewAvailable(KerbalRoster crewRoster)
	{
		for (int i = 0; i < vesselCrew.Count; i++)
		{
			for (int j = 0; j < vesselCrew[i].crewNames.Count; j++)
			{
				string name = vesselCrew[i].crewNames[j];
				if (crewRoster.Exists(name))
				{
					crewRoster[name].rosterStatus = ProtoCrewMember.RosterStatus.Available;
				}
			}
		}
	}

	public Dictionary<string, List<string>> GetRequiredExcludedParts()
	{
		return new Dictionary<string, List<string>>
		{
			{
				Localizer.Format("#autoLOC_8000261"),
				mission.situation.partFilter.GetExcludedParts()
			},
			{
				Localizer.Format("#autoLOC_8000262"),
				partFilter.GetExcludedParts()
			}
		};
	}

	public void StartEditorEvents()
	{
		vesselRestrictionList.StartAppEvents();
	}

	public void ClearEditorEvents()
	{
		vesselRestrictionList.ClearAppEvents();
	}

	public void UpdateRequiredPartsDisplayedParts()
	{
		requiredPartsParameter.UpdateDisplayedParts();
	}

	public void UpdateAutoGenerateCrewSettings(bool newValue)
	{
		autoGenerateCrew = newValue;
		if (autoGenerateCrewParameter != null)
		{
			autoGenerateCrewParameter.toggle.isOn = newValue;
		}
	}

	public void OnRequiredPartsParameterCreated(MEGUIParameterPartPicker parameter)
	{
		requiredPartsParameter = parameter;
	}

	public void OnPartPickerNeedsToUpdateExcludedPartsFilter()
	{
		partFilter.UpdateExcludedPartsFilter();
	}

	public void OnAutoGenerateCrewControlCreated(MEGUIParameterCheckbox parameter)
	{
		autoGenerateCrewParameter = parameter;
		autoGenerateCrewParameter.toggle.isOn = autoGenerateCrew;
	}

	public bool OnRequieredPartsCheckpointValidator(List<string> value)
	{
		return MissionCheckpointValidator.CompareObjectLists(requiredParts, value);
	}

	public void Load(ConfigNode node)
	{
		for (int i = 0; i < node.values.Count; i++)
		{
			ConfigNode.Value value = node.values[i];
			switch (value.name)
			{
			case "persistentId":
				persistentId = uint.Parse(value.value);
				break;
			case "focusonSpawn":
				focusonSpawn = bool.Parse(value.value);
				break;
			case "ship":
				vesselName = value.value;
				break;
			case "autoGenerateCrew":
				autoGenerateCrew = bool.Parse(value.value);
				break;
			case "playerCreated":
				playerCreated = bool.Parse(value.value);
				break;
			case "vesselDescription":
				vesselDescription = value.value;
				break;
			case "craftFile":
				craftFile = value.value;
				break;
			case "launched":
				launched = bool.Parse(value.value);
				break;
			case "readyToLaunch":
				readyToLaunch = bool.Parse(value.value);
				break;
			}
		}
		for (int j = 0; j < node.nodes.Count; j++)
		{
			ConfigNode configNode = node.nodes[j];
			string name = configNode.name;
			if (!(name == "LOCATION"))
			{
				if (name == "PARTCREW")
				{
					Crew crew = new Crew();
					crew.Load(configNode);
					vesselCrew.Add(crew);
				}
			}
			else
			{
				location.Load(configNode);
			}
		}
		partFilter.Load(node);
		requiredParts = node.GetValuesList("requiredParts");
		MissionsUtils.UpdatePartNames(ref requiredParts);
		if (playerCreated)
		{
			vesselRestrictionList.Load(node);
			if (HighLogic.CurrentGame != null && HighLogic.CurrentGame.Mode == Game.Modes.MISSION && requiredParts != null && requiredParts.Count > 0)
			{
				vesselRestrictionList.activeModules.Add(new VesselRestriction_RequiredParts());
			}
		}
	}

	public void Save(ConfigNode node)
	{
		ConfigNode configNode = node.AddNode("VESSELSITUATION");
		configNode.AddValue("ship", vesselName);
		string text = vesselDescription.Replace("\n", "\\n");
		text = text.Replace("\t", "\\t");
		configNode.AddValue("vesselDescription", text);
		configNode.AddValue("playerCreated", playerCreated);
		configNode.AddValue("persistentId", persistentId);
		configNode.AddValue("craftFile", craftFile);
		configNode.AddValue("focusonSpawn", focusonSpawn);
		configNode.AddValue("launched", launched);
		configNode.AddValue("readyToLaunch", readyToLaunch);
		configNode.AddValue("autoGenerateCrew", autoGenerateCrew);
		location.Save(configNode.AddNode("LOCATION"));
		for (int i = 0; i < vesselCrew.Count; i++)
		{
			vesselCrew[i].Save(configNode.AddNode("PARTCREW"));
		}
		if (playerCreated)
		{
			vesselRestrictionList.Save(configNode);
		}
		partFilter.Save(configNode);
		for (int j = 0; j < requiredParts.Count; j++)
		{
			configNode.AddValue("requiredParts", requiredParts[j]);
		}
	}
}
