using System.Collections.Generic;
using Expansions.Missions.Editor;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Tests;

public class TestVessel : TestModule
{
	[MEGUI_VesselSelect(order = 0, onControlCreated = "vesselSelectorCreated", resetValue = "0", gapDisplay = true, guiName = "#autoLOC_8000001")]
	public uint vesselID;

	public bool useActiveVessel;

	public Vessel vessel;

	public List<uint> missingVessels;

	public uint oldRootPart;

	public string bodyScore;

	public override void Awake()
	{
		base.Awake();
		missingVessels = new List<uint>();
	}

	public override void OnVesselPersistentIdChanged(uint oldId, uint newId)
	{
		if (vesselID == oldId)
		{
			vesselID = newId;
		}
	}

	public override void OnVesselDocking(uint oldId, uint newId)
	{
		if (oldId == vesselID)
		{
			Vessel vessel = null;
			FlightGlobals.FindVessel(oldId, out vessel);
			if (vessel != null)
			{
				oldRootPart = vessel.rootPart.persistentId;
			}
		}
	}

	public override void OnVesselsUndocking(Vessel oldVessel, Vessel newVessel)
	{
		if (oldVessel.persistentId == vesselID || (newVessel.persistentId == vesselID && oldRootPart != 0))
		{
			Part partout = null;
			ProtoPartSnapshot partout2 = null;
			if (FlightGlobals.FindLoadedPart(oldRootPart, out partout))
			{
				vesselID = partout.vessel.persistentId;
			}
			else if (FlightGlobals.FindUnloadedPart(oldRootPart, out partout2))
			{
				vesselID = partout2.pVesselRef.persistentId;
			}
		}
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (base.node != null)
		{
			base.node.UpdateInfoOnRefresh = true;
		}
		if (field.name == "vesselID" && base.node.mission != null)
		{
			VesselSituation vesselSituation = base.node.mission.GetVesselSituationByVesselID(vesselID);
			string vesselName = "";
			bool flag = false;
			if (vesselSituation != null)
			{
				vesselName = vesselSituation.vesselName;
			}
			else if (base.node.mission.UpdateFromMappedVesselIDs(vesselID, ref vesselName, ref vesselSituation))
			{
				flag = true;
			}
			if (vesselSituation == null && !flag)
			{
				return Localizer.Format("#autoLOC_8000069", useActiveVessel ? Localizer.Format("#autoLOC_8004217") : Localizer.Format("#autoLOC_8001004"));
			}
			if (HighLogic.LoadedSceneIsMissionBuilder)
			{
				MissionCraft craftBySituationsVesselID = MissionEditorLogic.Instance.EditorMission.GetCraftBySituationsVesselID(vesselSituation.persistentId);
				if (craftBySituationsVesselID != null && MissionEditorLogic.Instance.incompatibleCraft.Contains(craftBySituationsVesselID.craftFile))
				{
					vesselName = Localizer.Format("#autoLOC_8004245", vesselName);
				}
			}
			string text = Localizer.Format("#autoLOC_8000069", vesselName);
			if (flag && vesselSituation != null)
			{
				text = text + "\n" + Localizer.Format("#autoLOC_6002491", vesselSituation.vesselName) + "\n";
			}
			return text;
		}
		if (field.name == "resourceName")
		{
			PartResourceDefinition definition = PartResourceLibrary.Instance.GetDefinition(field.GetValue().ToString());
			return Localizer.Format("#autoLOC_8004190", field.guiName, definition.displayName);
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override bool Test()
	{
		if (vesselID == 0)
		{
			vessel = FlightGlobals.ActiveVessel;
		}
		else
		{
			uint num = base.node.mission.CurrentVesselID(base.node, vesselID);
			if (FlightGlobals.PersistentVesselIds.ContainsKey(num))
			{
				vessel = FlightGlobals.PersistentVesselIds[num];
			}
			else if (vessel != null)
			{
				if (!missingVessels.Contains(num))
				{
					missingVessels.Add(num);
					Debug.LogErrorFormat("[TestVessel] Unable to find VesselID ({0}) from Node ({1}) in FlightGlobals.", num, base.node.Title);
				}
				vessel = null;
			}
		}
		return true;
	}

	public void vesselSelectorCreated(MEGUIParameter parameter)
	{
		(parameter as MEGUIParameterVesselDropdownList).OverrideDefaultValue(useActiveVessel);
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("vesselID", vesselID);
		if (oldRootPart != 0)
		{
			node.AddValue("oldRootPart", oldRootPart);
		}
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("vesselID", ref vesselID);
		node.TryGetValue("oldRootPart", ref oldRootPart);
	}
}
