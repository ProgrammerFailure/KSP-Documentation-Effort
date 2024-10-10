using System;
using ns9;

namespace Contracts.Parameters;

[Serializable]
public class EnterSOI : ContractParameter
{
	public CelestialBody targetBody;

	public CelestialBody TargetBody => targetBody;

	public EnterSOI()
	{
	}

	public EnterSOI(CelestialBody targetBody)
	{
		this.targetBody = targetBody;
	}

	public override string GetHashString()
	{
		return targetBody.name;
	}

	public override string GetTitle()
	{
		return Localizer.Format("#autoLOC_269618", targetBody.displayName);
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
		GameEvents.VesselSituation.onFlyBy.Add(OnEnterSOI);
	}

	public override void OnUnregister()
	{
		GameEvents.VesselSituation.onFlyBy.Remove(OnEnterSOI);
	}

	public void OnEnterSOI(Vessel vessel, CelestialBody body)
	{
		if (body == targetBody)
		{
			SetComplete();
		}
	}
}
