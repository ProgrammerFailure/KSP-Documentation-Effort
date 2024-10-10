using System;
using Expansions.Missions.Editor;
using ns9;

namespace Expansions.Missions;

[Serializable]
public class VesselPartIDPair : IConfigNode
{
	[MEGUI_VesselSelect(addDefaultOption = false, gapDisplay = true, guiName = "#autoLOC_8000001")]
	public uint VesselID;

	public uint partID;

	[MEGUI_Label(simpleLabel = false, guiName = "#autoLOC_8000012")]
	public string partName;

	public VesselPartIDPair()
	{
		VesselID = 0u;
		partID = 0u;
		partName = "#autoLOC_8001004";
	}

	public override bool Equals(object obj)
	{
		if (!(obj is VesselPartIDPair vesselPartIDPair))
		{
			return false;
		}
		if (VesselID.Equals(vesselPartIDPair.VesselID))
		{
			return partID.Equals(vesselPartIDPair.partID);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return VesselID.GetHashCode() ^ partID.GetHashCode();
	}

	public void ValidatePartAgainstCraft(MENode node, MissionEditorValidator validator)
	{
		if (partID == 0 || !(node != null))
		{
			return;
		}
		MissionCraft craftBySituationsVesselID = node.mission.GetCraftBySituationsVesselID(VesselID);
		if (craftBySituationsVesselID == null)
		{
			return;
		}
		ShipConstruct shipConstruct = MissionEditorLogic.Instance.LoadVessel(craftBySituationsVesselID);
		if (shipConstruct == null)
		{
			return;
		}
		bool flag = true;
		for (int i = 0; i < shipConstruct.parts.Count; i++)
		{
			if (shipConstruct.parts[i].persistentId == partID)
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			validator.AddNodeFail(node, Localizer.Format("#autoLOC_8002115"));
		}
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("partPersistentId", partID);
		node.AddValue("vesselPersistentId", VesselID);
		node.AddValue("partName", partName);
	}

	public void Load(ConfigNode node)
	{
		node.TryGetValue("partPersistentId", ref partID);
		node.TryGetValue("vesselPersistentId", ref VesselID);
		node.TryGetValue("partName", ref partName);
	}
}
