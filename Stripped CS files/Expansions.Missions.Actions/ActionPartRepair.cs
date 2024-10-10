using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Expansions.Missions.Adjusters;
using Expansions.Missions.Editor;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Actions;

public class ActionPartRepair : ActionModule
{
	public enum RepairChoices
	{
		[Description("#autoLOC_8100115")]
		entireVessel,
		[Description("#autoLOC_8100116")]
		onePart,
		[Description("#autoLOC_8100117")]
		partModule,
		[Description("#autoLOC_8100118")]
		failureID
	}

	[MEGUI_VesselPartSelect(onValueChange = "OnPartIDChanged", onControlCreated = "VesselPartSelectorControlCreated", resetValue = "0", gapDisplay = true, guiName = "#autoLOC_8100120")]
	public VesselPartIDPair vesselPartIDs;

	[MEGUI_Dropdown(onDropDownValueChange = "OnRepairTypeValueChange", SetDropDownItems = "SetRepairTypeDropdown", onControlCreated = "OnRepairTypeControlCreated", gapDisplay = false, guiName = "#autoLOC_8100119")]
	public RepairChoices repairType;

	public MEGUIParameterDropdownList RepairTypeDropdown;

	[MEGUI_Dropdown(SetDropDownItems = "SetPartModuleNamesForDropdown", canBePinned = false, onControlCreated = "OnPartModuleDropdownControlCreated", hideOnSetup = true, guiName = "#autoLOC_8100121")]
	public string partModule;

	[MEGUI_Dropdown(SetDropDownItems = "SetFailureNamesForDropdown", canBePinned = false, onControlCreated = "OnFailureDropdownControlCreated", hideOnSetup = true, guiName = "#autoLOC_8100122")]
	public Guid failureID;

	public MEGUIParameterDropdownList FailureDropdown;

	public VesselSituation vesselSituation;

	public bool playerCreated;

	public MEGUIParameterDropdownList PartModuleDropdown;

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8000063");
		vesselPartIDs = new VesselPartIDPair();
		partModule = "";
		failureID = Guid.Empty;
		if (HighLogic.LoadedSceneIsMissionBuilder)
		{
			GameEvents.Mission.onFailureListChanged.Add(onFailureListChanged);
			GameEvents.Mission.onVesselSituationChanged.Add(onVesselSituationChanged);
			GameEvents.Mission.onMissionLoaded.Add(onVesselSituationChanged);
		}
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		GameEvents.Mission.onFailureListChanged.Remove(onFailureListChanged);
		GameEvents.Mission.onVesselSituationChanged.Remove(onVesselSituationChanged);
		GameEvents.Mission.onMissionLoaded.Remove(onVesselSituationChanged);
	}

	public void VesselPartSelectorControlCreated(MEGUIParameterVesselPartSelector sender)
	{
		OnPartIDChanged(vesselPartIDs);
	}

	public void OnRepairTypeControlCreated(MEGUIParameterDropdownList parameter)
	{
		RepairTypeDropdown = parameter;
	}

	public void OnPartModuleDropdownControlCreated(MEGUIParameterDropdownList parameter)
	{
		PartModuleDropdown = parameter;
	}

	public void OnFailureDropdownControlCreated(MEGUIParameterDropdownList parameter)
	{
		FailureDropdown = parameter;
	}

	public List<MEGUIDropDownItem> SetRepairTypeDropdown()
	{
		List<MEGUIDropDownItem> list = new List<MEGUIDropDownItem>();
		Array values = Enum.GetValues(repairType.GetType());
		vesselSituation = node.mission.GetVesselSituationByVesselID(vesselPartIDs.VesselID);
		playerCreated = true;
		if (vesselSituation != null)
		{
			playerCreated = vesselSituation.playerCreated;
		}
		IEnumerator enumerator = values.GetEnumerator();
		while (enumerator.MoveNext())
		{
			Enum @enum = (Enum)enumerator.Current;
			if ((playerCreated || vesselPartIDs.partID != 0 || !(@enum.ToString() == "onePart")) && (!playerCreated || (!(@enum.ToString() == "onePart") && !(@enum.ToString() == "partModule"))))
			{
				list.Add(new MEGUIDropDownItem(@enum.ToString(), @enum, @enum.displayDescription()));
			}
		}
		return list;
	}

	public List<MEGUIDropDownItem> SetPartModuleNamesForDropdown()
	{
		List<string> list = new List<string>();
		MissionCraft craftBySituationsVesselID = node.mission.GetCraftBySituationsVesselID(vesselPartIDs.VesselID);
		if (craftBySituationsVesselID != null)
		{
			ConfigNode[] nodes = craftBySituationsVesselID.CraftNode.GetNodes("PART");
			int i = 0;
			for (int num = nodes.Length; i < num; i++)
			{
				uint value = 0u;
				nodes[i].TryGetValue("persistentId", ref value);
				ConfigNode[] nodes2 = nodes[i].GetNodes("MODULE");
				int j = 0;
				for (int num2 = nodes2.Length; j < num2; j++)
				{
					if (vesselPartIDs.partID == 0 || (vesselPartIDs.partID != 0 && vesselPartIDs.partID == value))
					{
						list.AddUnique(nodes2[j].GetValue("name"));
					}
				}
			}
		}
		GameObject gameObject = new GameObject("DiscardMe");
		gameObject.SetActive(value: false);
		List<MEGUIDropDownItem> list2 = new List<MEGUIDropDownItem>();
		for (int k = 0; k < list.Count; k++)
		{
			if (MissionsUtils.adjusterTypesSupportedByPartModule.ContainsKey(list[k]))
			{
				Type classByName = AssemblyLoader.GetClassByName(typeof(PartModule), list[k]);
				PartModule partModule = null;
				if (classByName != null)
				{
					partModule = (PartModule)gameObject.AddComponent(classByName);
				}
				list2.Add(new MEGUIDropDownItem(list[k], list[k], partModule.GetModuleDisplayName()));
			}
		}
		gameObject.DestroyGameObject();
		if (list2.Count == 0)
		{
			list2.Add(new MEGUIDropDownItem("None", "None Available", Localizer.Format("#autoLOC_8100310")));
		}
		return list2;
	}

	public List<MEGUIDropDownItem> SetFailureNamesForDropdown()
	{
		List<MEGUIDropDownItem> list = new List<MEGUIDropDownItem>();
		List<ActionPartFailure> allActionModules = node.mission.GetAllActionModules<ActionPartFailure>();
		for (int i = 0; i < allActionModules.Count; i++)
		{
			if (allActionModules[i].vesselPartIDs.VesselID != vesselPartIDs.VesselID)
			{
				continue;
			}
			for (int j = 0; j < allActionModules[i].Modules.activeModules.Count; j++)
			{
				if (allActionModules[i].Modules.activeModules[j] is AdjusterPartModuleBase adjusterPartModuleBase)
				{
					list.Add(new MEGUIDropDownItem(adjusterPartModuleBase.adjusterID.ToString(), adjusterPartModuleBase.adjusterID, adjusterPartModuleBase.GetDisplayName()));
				}
			}
		}
		if (list.Count == 0)
		{
			list.Add(new MEGUIDropDownItem("None Available", Guid.Empty, Localizer.Format("#autoLOC_8100310")));
		}
		return list;
	}

	public void OnRepairTypeValueChange(MEGUIParameterDropdownList sender, int newIndex)
	{
		if (sender.SelectedValue != null && node != null)
		{
			switch ((RepairChoices)sender.SelectedValue)
			{
			case RepairChoices.entireVessel:
				if (FailureDropdown != null)
				{
					FailureDropdown.gameObject.SetActive(value: false);
				}
				if (PartModuleDropdown != null)
				{
					PartModuleDropdown.gameObject.SetActive(value: false);
				}
				break;
			case RepairChoices.onePart:
				if (FailureDropdown != null)
				{
					FailureDropdown.gameObject.SetActive(value: false);
				}
				if (PartModuleDropdown != null)
				{
					PartModuleDropdown.gameObject.SetActive(value: false);
				}
				break;
			case RepairChoices.partModule:
				if (FailureDropdown != null)
				{
					FailureDropdown.gameObject.SetActive(value: false);
				}
				if (PartModuleDropdown != null)
				{
					PartModuleDropdown.gameObject.SetActive(value: true);
					PartModuleDropdown.RebuildDropDown();
				}
				break;
			case RepairChoices.failureID:
				if (FailureDropdown != null)
				{
					FailureDropdown.gameObject.SetActive(value: true);
					FailureDropdown.RebuildDropDown();
				}
				if (PartModuleDropdown != null)
				{
					PartModuleDropdown.gameObject.SetActive(value: false);
				}
				break;
			}
		}
		UpdateNodeBodyUI();
	}

	public void onFailureListChanged()
	{
		if (FailureDropdown != null)
		{
			FailureDropdown.RebuildDropDown();
		}
	}

	public void OnPartIDChanged(VesselPartIDPair newVesselPartIDs)
	{
		if (newVesselPartIDs != null)
		{
			vesselPartIDs = newVesselPartIDs;
			onVesselSituationChanged();
		}
	}

	public void onVesselSituationChanged()
	{
		if (node != null && node.mission != null)
		{
			vesselSituation = node.mission.GetVesselSituationByVesselID(vesselPartIDs.VesselID);
			if (RepairTypeDropdown != null)
			{
				RepairTypeDropdown.RebuildDropDown();
				OnRepairTypeValueChange(RepairTypeDropdown, RepairTypeDropdown.GetItemIndex(RepairTypeDropdown.SelectedValue));
			}
			if (PartModuleDropdown != null)
			{
				PartModuleDropdown.RebuildDropDown();
			}
			if (FailureDropdown != null)
			{
				FailureDropdown.RebuildDropDown();
			}
			UpdateNodeBodyUI();
		}
	}

	public override IEnumerator Fire()
	{
		if (!ExpansionsLoader.IsExpansionInstalled("MakingHistory"))
		{
			yield return null;
		}
		Vessel vessel = null;
		if (FlightGlobals.FindVessel(vesselPartIDs.VesselID, out vessel))
		{
			if (vessel.loaded)
			{
				switch (repairType)
				{
				case RepairChoices.entireVessel:
					RepairFailures(vessel);
					break;
				case RepairChoices.onePart:
				{
					Part partout = null;
					if (FlightGlobals.FindLoadedPart(vesselPartIDs.partID, out partout))
					{
						RepairFailures(partout);
					}
					break;
				}
				case RepairChoices.partModule:
					RepairFailures(vessel, partModule);
					break;
				case RepairChoices.failureID:
					RepairFailures(vessel, "", useGuid: true);
					break;
				}
			}
			else
			{
				switch (repairType)
				{
				case RepairChoices.entireVessel:
					RepairFailures(vessel.protoVessel);
					break;
				case RepairChoices.onePart:
				{
					ProtoPartSnapshot partout2 = null;
					if (FlightGlobals.FindUnloadedPart(vesselPartIDs.partID, out partout2))
					{
						partout2.ProtoPartRepair();
					}
					break;
				}
				case RepairChoices.partModule:
					RepairFailures(vessel.protoVessel, partModule);
					break;
				case RepairChoices.failureID:
					RepairFailures(vessel.protoVessel, "", useGuid: true);
					break;
				}
			}
		}
		else
		{
			Debug.LogWarning($"[ActionPartRepair]: Unable to perform action, vessel {vesselPartIDs.VesselID} was not found!");
		}
		yield return null;
	}

	public void RepairFailures(Vessel vessel, string repairpartModule = "", bool useGuid = false)
	{
		for (int i = 0; i < vessel.Parts.Count; i++)
		{
			RepairFailures(vessel.Parts[i], repairpartModule, useGuid);
		}
	}

	public void RepairFailures(Part part, string repairpartModule = "", bool useGuid = false)
	{
		for (int i = 0; i < part.Modules.Count; i++)
		{
			PartModule partModule = part.Modules[i];
			if (!(repairpartModule == "") && !(repairpartModule == partModule.ClassName))
			{
				continue;
			}
			for (int num = partModule.CurrentModuleAdjusterList.Count - 1; num >= 0; num--)
			{
				if (!useGuid || failureID == partModule.CurrentModuleAdjusterList[num].adjusterID)
				{
					partModule.RemovePartModuleAdjuster(partModule.CurrentModuleAdjusterList[num]);
				}
			}
		}
	}

	public void RepairFailures(ProtoVessel vessel, string repairpartModule = "", bool useGuid = false)
	{
		for (int i = 0; i < vessel.protoPartSnapshots.Count; i++)
		{
			RepairFailures(vessel.protoPartSnapshots[i], repairpartModule, useGuid);
		}
	}

	public void RepairFailures(ProtoPartSnapshot part, string repairpartModule = "", bool useGuid = false)
	{
		for (int i = 0; i < part.modules.Count; i++)
		{
			ProtoPartModuleSnapshot protoPartModuleSnapshot = part.modules[i];
			if (repairpartModule == "" || repairpartModule == protoPartModuleSnapshot.moduleName)
			{
				if (useGuid)
				{
					protoPartModuleSnapshot.RemovePartModuleAdjuster(failureID);
				}
				else
				{
					protoPartModuleSnapshot.ProtoPartModuleRepair();
				}
			}
		}
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004025");
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (node != null)
		{
			node.UpdateInfoOnRefresh = true;
		}
		if (field.name == "vesselPartIDs" && node.mission != null)
		{
			string text = "";
			string vesselName = "";
			bool flag = false;
			VesselSituation vesselSituationByVesselID = node.mission.GetVesselSituationByVesselID(vesselPartIDs.VesselID);
			if (vesselSituationByVesselID != null)
			{
				vesselName = vesselSituationByVesselID.vesselName;
			}
			else if (node.mission.UpdateFromMappedVesselIDs(vesselPartIDs.VesselID, ref vesselName, ref vesselSituationByVesselID))
			{
				flag = true;
				if (vesselSituationByVesselID == null)
				{
					text = Localizer.Format("#autoLOC_8000001") + ": " + Localizer.Format(vesselName) + "\n";
				}
			}
			if (vesselSituationByVesselID != null)
			{
				text = Localizer.Format("#autoLOC_8000001") + ": " + Localizer.Format(vesselName) + "\n";
				if (flag)
				{
					text = text + Localizer.Format("#autoLOC_6002491", vesselSituationByVesselID.vesselName) + "\n";
				}
			}
			return text + field.guiName + ": " + Localizer.Format(vesselPartIDs.partName);
		}
		if (field.name == "repairType")
		{
			switch (repairType)
			{
			default:
				return Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8100122"), Localizer.Format("#autoLOC_8100115"));
			case RepairChoices.onePart:
				return Localizer.Format("#autoLOC_8004190", RepairChoices.onePart.displayDescription(), Localizer.Format(vesselPartIDs.partName));
			case RepairChoices.partModule:
				if (node.mission != null)
				{
					return Localizer.Format("#autoLOC_8004190", RepairChoices.partModule.displayDescription(), partModule);
				}
				break;
			case RepairChoices.failureID:
			{
				if (!(node.mission != null))
				{
					break;
				}
				List<ActionPartFailure> allActionModules = node.mission.GetAllActionModules<ActionPartFailure>();
				for (int i = 0; i < allActionModules.Count; i++)
				{
					if (allActionModules[i].vesselPartIDs.VesselID != vesselPartIDs.VesselID)
					{
						continue;
					}
					for (int j = 0; j < allActionModules[i].Modules.activeModules.Count; j++)
					{
						if (allActionModules[i].Modules.activeModules[j] is AdjusterPartModuleBase adjusterPartModuleBase && adjusterPartModuleBase.adjusterID == failureID)
						{
							return Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8100122"), adjusterPartModuleBase.GetDisplayName());
						}
					}
				}
				break;
			}
			}
		}
		if (!(field.name == "partModule") && !(field.name == "failureID"))
		{
			return base.GetNodeBodyParameterString(field);
		}
		return "";
	}

	public override void RunValidation(MissionEditorValidator validator)
	{
		if (vesselPartIDs != null)
		{
			vesselPartIDs.ValidatePartAgainstCraft(node, validator);
		}
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		vesselPartIDs.Save(node);
		vesselPartIDs.Load(node);
		node.AddValue("repairType", repairType);
		if (partModule != "None Available")
		{
			node.AddValue("partModule", partModule);
		}
		if (failureID != Guid.Empty)
		{
			node.AddValue("failureID", failureID.ToString());
		}
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		vesselPartIDs.Load(node);
		if (!node.TryGetEnum("repairType", ref repairType, RepairChoices.entireVessel))
		{
			Debug.LogError("Failed to parse repairType from " + base.name);
		}
		if (node.HasValue("partModule"))
		{
			partModule = node.GetValue("partModule");
		}
		if (node.HasValue("failureID"))
		{
			failureID = new Guid(node.GetValue("failureID"));
		}
		else
		{
			failureID = Guid.NewGuid();
		}
	}
}
