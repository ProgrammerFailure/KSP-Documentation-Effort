using System;
using ns9;

namespace Contracts.Parameters;

[Serializable]
public class PlantFlag : ContractParameter
{
	public CelestialBody targetBody;

	public CelestialBody TargetBody => targetBody;

	public PlantFlag()
	{
	}

	public PlantFlag(CelestialBody targetBody)
	{
		this.targetBody = targetBody;
	}

	public override string GetHashString()
	{
		return targetBody.name;
	}

	public override string GetTitle()
	{
		return Localizer.Format("#autoLOC_270135", targetBody.displayName);
	}

	public override void OnLoad(ConfigNode node)
	{
		if (node.HasValue("body"))
		{
			targetBody = FlightGlobals.fetch.bodies[int.Parse(node.GetValue("body"))];
		}
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("body", targetBody.flightGlobalsIndex);
	}

	public override void OnRegister()
	{
		GameEvents.onFlagPlant.Add(OnPlantFlag);
	}

	public override void OnUnregister()
	{
		GameEvents.onFlagPlant.Remove(OnPlantFlag);
	}

	public void OnPlantFlag(Vessel vessel)
	{
		if (vessel.orbit.referenceBody == targetBody)
		{
			SetComplete();
		}
	}
}
