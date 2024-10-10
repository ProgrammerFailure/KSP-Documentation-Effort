using System;
using System.Collections;
using System.Collections.Generic;
using Expansions.Missions.Adjusters;
using Expansions.Missions.Editor;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Actions;

public class ActionPartFailure : ActionModule
{
	[MEGUI_VesselPartSelect(onValueChange = "OnPartIDChanged", onControlCreated = "VesselPartSelectorControlCreated", resetValue = "0", gapDisplay = true, guiName = "#autoLOC_8000012", Tooltip = "#autoLOC_8000004")]
	public VesselPartIDPair vesselPartIDs;

	public MEGUIParameterVesselPartSelector partSelectorRef;

	[MEGUI_DynamicModuleList(displayEmptyMessage = "#autoLOC_8000006", onValueChange = "OnAdjusterListModified", allowMultipleModuleInstances = true, onControlCreated = "DynamicModuleListControlCreated", displayMessageWhenEmpty = true, guiName = "#autoLOC_8000005")]
	public DynamicModuleList Modules;

	public MEGUIParameterDynamicModuleList dynamicModuleListRef;

	public override void OnVesselPersistentIdChanged(uint oldId, uint newId)
	{
		if (vesselPartIDs.VesselID == oldId)
		{
			vesselPartIDs.VesselID = newId;
			Debug.LogFormat("[ActionPartFailure]: Node ({0}) VesselId changed from {1} to {2}", node.id, oldId, newId);
		}
	}

	public override void OnPartPersistentIdChanged(uint vesselID, uint oldId, uint newId)
	{
		if (oldId == 3827239225u)
		{
			Debug.Log("break here");
		}
		if (vesselPartIDs.VesselID == vesselID && vesselPartIDs.partID == oldId)
		{
			vesselPartIDs.partID = newId;
			Debug.LogFormat("[ActionPartFailure]: Node ({0}) PartId changed from {1} to {2}", node.id, oldId, newId);
		}
	}

	public override void OnVesselsUndocking(Vessel oldVessel, Vessel newVessel)
	{
		if (vesselPartIDs.VesselID != oldVessel.persistentId && vesselPartIDs.VesselID != newVessel.persistentId)
		{
			return;
		}
		int num = 0;
		while (true)
		{
			if (num < newVessel.parts.Count)
			{
				if (vesselPartIDs.partID == newVessel.parts[num].persistentId)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		vesselPartIDs.VesselID = newVessel.persistentId;
		Debug.LogFormat("[ActionPartFailure]: Node ({0}) VesselId changed on undocking from {1} to {2}", node.id, oldVessel.persistentId, newVessel.persistentId);
	}

	public void VesselPartSelectorControlCreated(MEGUIParameterVesselPartSelector sender)
	{
		partSelectorRef = sender;
		OnPartIDChanged(vesselPartIDs);
	}

	public void DynamicModuleListControlCreated(MEGUIParameterDynamicModuleList sender)
	{
		dynamicModuleListRef = sender;
		dynamicModuleListRef.addButtonText.text = Localizer.Format("#autoLOC_8002056");
		OnPartIDChanged(vesselPartIDs);
	}

	public void OnAdjusterListModified(DynamicModuleList newModuleList)
	{
		GameEvents.Mission.onFailureListChanged.Fire();
	}

	public void onVesselSituationChanged()
	{
		if (node != null && node.mission != null && vesselPartIDs.partID != 0 && node.mission.GetSituationsIndexByPart(vesselPartIDs.partID) < 0)
		{
			vesselPartIDs = new VesselPartIDPair();
			Modules = new DynamicModuleList(node);
		}
		UpdateNodeBodyUI();
		if (partSelectorRef != null)
		{
			partSelectorRef.RefreshUI();
		}
	}

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8000002");
		vesselPartIDs = new VesselPartIDPair();
		if (HighLogic.LoadedSceneIsMissionBuilder)
		{
			GameEvents.Mission.onVesselSituationChanged.Add(onVesselSituationChanged);
			GameEvents.Mission.onMissionLoaded.Add(onVesselSituationChanged);
		}
	}

	public override void Initialize(MENode node)
	{
		base.Initialize(node);
		Modules = new DynamicModuleList(node);
		if (node != null)
		{
			node.UpdateInfoOnRefresh = true;
		}
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		GameEvents.Mission.onVesselSituationChanged.Remove(onVesselSituationChanged);
		GameEvents.Mission.onMissionLoaded.Remove(onVesselSituationChanged);
	}

	public void OnPartIDChanged(VesselPartIDPair newVesselPartIDs)
	{
		if (partSelectorRef == null || dynamicModuleListRef == null)
		{
			return;
		}
		Part selectedPart = partSelectorRef.GetSelectedPart();
		List<Type> list = new List<Type>();
		if (selectedPart != null)
		{
			for (int i = 0; i < selectedPart.Modules.Count; i++)
			{
				if (MissionsUtils.adjusterTypesSupportedByPartModule.ContainsKey(selectedPart.Modules[i].GetType().Name))
				{
					List<Type> collection = MissionsUtils.adjusterTypesSupportedByPartModule[selectedPart.Modules[i].GetType().Name];
					list.AddRange(collection);
				}
			}
		}
		else
		{
			for (int j = 0; j < MissionsUtils.adjusterTypesSupportedByPartModule.ValuesList.Count; j++)
			{
				list.AddRange(MissionsUtils.adjusterTypesSupportedByPartModule.ValuesList[j]);
			}
		}
		Modules.SetSupportedTypes(list);
		dynamicModuleListRef.RefreshUI();
	}

	public override List<IMENodeDisplay> GetInternalParametersToDisplay()
	{
		return Modules.GetInternalParameters();
	}

	public override IEnumerator Fire()
	{
		if (!ExpansionsLoader.IsExpansionInstalled("MakingHistory"))
		{
			yield return null;
		}
		if (vesselPartIDs.partID == 0)
		{
			uint key = node.mission.CurrentVesselID(node, vesselPartIDs.VesselID);
			if (FlightGlobals.PersistentVesselIds.ContainsKey(key))
			{
				Vessel vessel = FlightGlobals.PersistentVesselIds[key];
				if (vessel != null)
				{
					addAdjustersVessel(vessel);
					yield return null;
				}
			}
			Debug.LogWarning($"[ActionPartFailure]: Unable to perform action on entire vessel{vesselPartIDs.VesselID}! Vessel Not found!");
			yield return null;
		}
		Part partout = null;
		ProtoPartSnapshot partout2 = null;
		if (FlightGlobals.FindLoadedPart(vesselPartIDs.partID, out partout))
		{
			if (Modules.activeModules.Count > 0)
			{
				List<AdjusterPartModuleBase> list = new List<AdjusterPartModuleBase>();
				foreach (DynamicModule activeModule in Modules.activeModules)
				{
					list.Add(activeModule as AdjusterPartModuleBase);
				}
				partout.AddPartModuleAdjusterList(list);
			}
			else
			{
				Debug.LogWarning($"[ActionPartFailure]: Unable to perform action on part{vesselPartIDs.partID}! No adjuster types were specified!");
			}
		}
		else if (FlightGlobals.FindUnloadedPart(vesselPartIDs.partID, out partout2))
		{
			if (Modules.activeModules.Count > 0)
			{
				List<AdjusterPartModuleBase> list2 = new List<AdjusterPartModuleBase>();
				foreach (DynamicModule activeModule2 in Modules.activeModules)
				{
					list2.Add(activeModule2 as AdjusterPartModuleBase);
				}
				partout2.AddProtoPartModuleAdjusters(list2);
			}
			else
			{
				Debug.LogWarning($"[ActionPartFailure]: Unable to perform action on part{vesselPartIDs.partID}! No adjuster types were specified!");
			}
		}
		else
		{
			Debug.LogWarning($"[ActionPartFailure]: Unable to perform action, part {vesselPartIDs.partID} was not found!");
		}
		yield return null;
	}

	public void addAdjustersVessel(Vessel vessel)
	{
		for (int i = 0; i < vessel.parts.Count; i++)
		{
			List<AdjusterPartModuleBase> list = new List<AdjusterPartModuleBase>();
			Part part = vessel.parts[i];
			for (int j = 0; j < part.Modules.Count; j++)
			{
				PartModule partModule = part.Modules[j];
				if (!MissionsUtils.adjusterTypesSupportedByPartModule.ContainsKey(partModule.GetType().Name))
				{
					continue;
				}
				List<Type> list2 = MissionsUtils.adjusterTypesSupportedByPartModule[partModule.GetType().Name];
				for (int k = 0; k < Modules.activeModules.Count; k++)
				{
					if (list2.Contains(Modules.activeModules[k].GetType()))
					{
						list.Add(Modules.activeModules[k] as AdjusterPartModuleBase);
					}
				}
			}
			part.AddPartModuleAdjusterList(list);
		}
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004024");
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (node != null)
		{
			node.UpdateInfoOnRefresh = true;
		}
		string text = "";
		string vesselName = "";
		bool flag = false;
		if (field.name == "vesselPartIDs" && node.mission != null)
		{
			VesselSituation vesselSituation = node.mission.GetVesselSituationByVesselID(vesselPartIDs.VesselID);
			if (vesselSituation != null)
			{
				vesselName = vesselSituation.vesselName;
			}
			else if (node.mission.UpdateFromMappedVesselIDs(vesselPartIDs.VesselID, ref vesselName, ref vesselSituation))
			{
				flag = true;
				if (vesselSituation == null)
				{
					text = Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8000001"), vesselName) + "\n";
				}
			}
			if (vesselSituation != null)
			{
				text = Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8000001"), vesselName) + "\n";
				if (flag)
				{
					text = text + Localizer.Format("#autoLOC_6002491", vesselSituation.vesselName) + "\n";
				}
			}
			return text + Localizer.Format("#autoLOC_8004190", field.guiName, Localizer.Format(vesselPartIDs.partName));
		}
		if (field.name == "Modules")
		{
			if (field.GetValue() is DynamicModuleList dynamicModuleList)
			{
				int num = 0;
				for (int i = 0; i < dynamicModuleList.activeModules.Count; i++)
				{
					text += dynamicModuleList.activeModules[i].GetDisplayName();
					num++;
					if (i != dynamicModuleList.activeModules.Count - 1)
					{
						text += "\n";
					}
				}
				if (num > 0)
				{
					return Localizer.Format("#autoLOC_8004190", field.guiName, text);
				}
			}
			return Localizer.Format("#autoLOC_8004190", field.guiName, Localizer.Format("#autoLOC_8100310"));
		}
		return base.GetNodeBodyParameterString(field);
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
		Modules.Save(node);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		vesselPartIDs.Load(node);
		Modules.Load(node);
	}
}
