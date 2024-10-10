using System;
using ns9;

namespace Contracts.Parameters;

[Serializable]
public class LandOnBody : ContractParameter
{
	public CelestialBody targetBody;

	public CelestialBody TargetBody => targetBody;

	public LandOnBody()
	{
	}

	public LandOnBody(CelestialBody targetBody)
	{
		this.targetBody = targetBody;
	}

	public override string GetHashString()
	{
		return targetBody.name;
	}

	public override string GetTitle()
	{
		return Localizer.Format("#autoLOC_269752", targetBody.displayName);
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
		GameEvents.VesselSituation.onLand.Add(OnVesselLand);
	}

	public override void OnUnregister()
	{
		GameEvents.VesselSituation.onLand.Remove(OnVesselLand);
	}

	public void OnVesselLand(Vessel vessel, CelestialBody body)
	{
		if (body == targetBody)
		{
			SetComplete();
		}
	}
}
