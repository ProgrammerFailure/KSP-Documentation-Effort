using System;
using ns9;

namespace Contracts.Parameters;

[Serializable]
public class EnterOrbit : ContractParameter
{
	public CelestialBody targetBody;

	public CelestialBody TargetBody => targetBody;

	public EnterOrbit()
	{
	}

	public EnterOrbit(CelestialBody targetBody)
	{
		this.targetBody = targetBody;
	}

	public override string GetHashString()
	{
		return targetBody.name;
	}

	public override string GetTitle()
	{
		return Localizer.Format("#autoLOC_269557", targetBody.displayName);
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
		GameEvents.VesselSituation.onOrbit.Add(OnEnterOrbit);
	}

	public override void OnUnregister()
	{
		GameEvents.VesselSituation.onOrbit.Remove(OnEnterOrbit);
	}

	public void OnEnterOrbit(Vessel vessel, CelestialBody body)
	{
		if (body == targetBody)
		{
			SetComplete();
		}
	}
}
