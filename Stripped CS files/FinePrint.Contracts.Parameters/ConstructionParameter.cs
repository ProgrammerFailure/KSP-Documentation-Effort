using System;
using Contracts;
using ns9;

namespace FinePrint.Contracts.Parameters;

[Serializable]
public class ConstructionParameter : ContractParameter
{
	public CelestialBody targetBody;

	public uint vesselPersistentId;

	public string partName;

	public string vesselName;

	public AvailablePart ap;

	public Vessel vsl;

	public CelestialBody TargetBody => targetBody;

	public uint VesselPersistentId => vesselPersistentId;

	public string PartName => partName;

	public string VesselName => vesselName;

	public ConstructionParameter()
	{
	}

	public ConstructionParameter(string partName, uint vesselId, string vesselName, CelestialBody targetBody)
	{
		UpdatePartInfo(partName, vesselId, vesselName, targetBody);
	}

	public void UpdatePartInfo(string partName, uint vesselId, string vesselName, CelestialBody targetBody)
	{
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
		return partName + vesselPersistentId;
	}

	public override string GetTitle()
	{
		if (vsl == null)
		{
			FlightGlobals.FindVessel(vesselPersistentId, out vsl);
		}
		return Localizer.Format("#autoLOC_6002602", (ap != null) ? ap.title : partName, (vsl != null) ? vsl.vesselName : vesselName);
	}

	public override void OnLoad(ConfigNode node)
	{
		string value = "";
		if (node.TryGetValue("bodyName", ref value))
		{
			targetBody = FlightGlobals.GetBodyByName(value);
		}
		node.TryGetValue("partName", ref partName);
		node.TryGetValue("vesselName", ref vesselName);
		node.TryGetValue("vesselPersistentId", ref vesselPersistentId);
		ap = PartLoader.getPartInfoByName(partName);
		FlightGlobals.FindVessel(vesselPersistentId, out vsl);
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("bodyName", targetBody.bodyName);
		node.AddValue("partName", partName);
		node.AddValue("vesselName", vesselName);
		node.AddValue("vesselPersistentId", vesselPersistentId);
	}

	public override void OnRegister()
	{
		GameEvents.onVesselPersistentIdChanged.Add(VesselIdChanged);
		GameEvents.OnEVAConstructionModePartAttached.Add(OnConstructionPartAttached);
	}

	public override void OnUnregister()
	{
		GameEvents.onVesselPersistentIdChanged.Remove(VesselIdChanged);
		GameEvents.OnEVAConstructionModePartAttached.Remove(OnConstructionPartAttached);
	}

	public override string GetMessageComplete()
	{
		return Localizer.Format("#autoLOC_6002604");
	}

	public void VesselIdChanged(uint oldId, uint newId)
	{
		if (vesselPersistentId == oldId)
		{
			vesselPersistentId = newId;
		}
	}

	public void OnConstructionPartAttached(Vessel vsl, Part p)
	{
		if (vsl.persistentId == vesselPersistentId && p.partInfo.name == partName)
		{
			SetComplete();
			base.Root.Complete();
		}
	}
}
