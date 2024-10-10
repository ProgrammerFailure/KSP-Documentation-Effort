using System;
using Contracts;
using ns9;

namespace FinePrint.Contracts.Parameters;

[Serializable]
public class RepairPartParameter : ContractParameter
{
	public CelestialBody targetBody;

	public uint vesselPersistentId;

	public uint partPersistentId;

	public string partName;

	public string vesselName = "";

	public AvailablePart ap;

	public Vessel vsl;

	public CelestialBody TargetBody => targetBody;

	public uint VesselPersistentId => vesselPersistentId;

	public uint PartPersistentId => partPersistentId;

	public string PartName => partName;

	public string VesselName => vesselName;

	public RepairPartParameter()
	{
	}

	public RepairPartParameter(uint partId, string partName, uint vesselId, string vesselName, CelestialBody targetBody)
	{
		UpdatePartInfo(partId, partName, vesselId, vesselName, targetBody);
	}

	public void UpdatePartInfo(uint partId, string partName, uint vesselId, string vesselName, CelestialBody targetBody)
	{
		partPersistentId = partId;
		if (!string.IsNullOrEmpty(partName))
		{
			this.partName = partName;
			ap = PartLoader.getPartInfoByName(partName);
		}
		vesselPersistentId = vesselId;
		if (!string.IsNullOrEmpty(vesselName))
		{
			this.vesselName = vesselName;
		}
		if (targetBody != null)
		{
			this.targetBody = targetBody;
		}
		FlightGlobals.FindVessel(vesselId, out vsl);
	}

	public override string GetHashString()
	{
		return partName + partPersistentId;
	}

	public override string GetTitle()
	{
		if (vsl == null)
		{
			FlightGlobals.FindVessel(vesselPersistentId, out vsl);
		}
		return Localizer.Format("#autoLOC_6002578", (vsl != null) ? vsl.vesselName : vesselName, (ap != null) ? ap.title : partName, (targetBody != null) ? targetBody.bodyDisplayName : "");
	}

	public override void OnLoad(ConfigNode node)
	{
		string value = "";
		if (node.TryGetValue("bodyName", ref value))
		{
			targetBody = FlightGlobals.GetBodyByName(value);
		}
		node.TryGetValue("partName", ref partName);
		node.TryGetValue("partPersistentId", ref partPersistentId);
		node.TryGetValue("vesselPersistentId", ref vesselPersistentId);
		node.TryGetValue("vesselName", ref vesselName);
		ap = PartLoader.getPartInfoByName(partName);
		FlightGlobals.FindVessel(vesselPersistentId, out vsl);
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("bodyName", targetBody.bodyName);
		node.AddValue("partName", partName);
		node.AddValue("partPersistentId", partPersistentId);
		node.AddValue("vesselPersistentId", vesselPersistentId);
		if (!string.IsNullOrEmpty(vesselName))
		{
			node.AddValue("vesselName", vesselName);
		}
	}

	public override void OnRegister()
	{
		GameEvents.onVesselPersistentIdChanged.Add(VesselIdChanged);
		GameEvents.onPartPersistentIdChanged.Add(PartIdChanged);
		GameEvents.onPartRepaired.Add(OnPartRepaired);
	}

	public override void OnUnregister()
	{
		GameEvents.onVesselPersistentIdChanged.Remove(VesselIdChanged);
		GameEvents.onPartPersistentIdChanged.Remove(PartIdChanged);
		GameEvents.onPartRepaired.Remove(OnPartRepaired);
	}

	public override string GetMessageComplete()
	{
		return Localizer.Format("#autoLOC_6002580");
	}

	public void PartIdChanged(uint vslId, uint oldId, uint newId)
	{
		if (vslId == vesselPersistentId && oldId == partPersistentId)
		{
			partPersistentId = newId;
		}
	}

	public void VesselIdChanged(uint oldId, uint newId)
	{
		if (vesselPersistentId == oldId)
		{
			vesselPersistentId = newId;
		}
	}

	public void OnPartRepaired(Part p)
	{
		if ((partPersistentId == 0 || p.persistentId == partPersistentId) && p.vessel.persistentId == vesselPersistentId)
		{
			SetComplete();
			base.Root.Complete();
		}
	}
}
