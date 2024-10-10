using System;
using FinePrint.Utilities;
using ns9;

namespace Contracts.Parameters;

[Serializable]
public class ReachSpeedEnvelope : ContractParameter
{
	public double minSpeed;

	public double maxSpeed;

	public ReachSpeedEnvelope()
	{
		minSpeed = 0.0;
		maxSpeed = double.MaxValue;
	}

	public ReachSpeedEnvelope(float minSpd, float maxSpd)
	{
		minSpeed = Math.Min(minSpd, maxSpd);
		maxSpeed = Math.Max(minSpd, maxSpd);
	}

	public override string GetTitle()
	{
		return Localizer.Format("#autoLOC_268955", minSpeed.ToString("N1"), maxSpeed.ToString("N1"));
	}

	public override void OnReset()
	{
		SetIncomplete();
	}

	public override void OnLoad(ConfigNode node)
	{
		SystemUtilities.LoadNode(node, "ReachSpeedEnvelope", "minSpd", ref minSpeed, 0.0);
		SystemUtilities.LoadNode(node, "ReachSpeedEnvelope", "maxSpd", ref maxSpeed, double.MaxValue);
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("minSpd", minSpeed);
		node.AddValue("maxSpd", maxSpeed);
	}

	public override void OnUpdate()
	{
		if (base.Root.ContractState == Contract.State.Active && HighLogic.LoadedSceneIsFlight && FlightGlobals.ready)
		{
			bool num = checkVesselWithinEnvelope(FlightGlobals.ActiveVessel);
			if (num && base.State == ParameterState.Incomplete)
			{
				SetComplete();
			}
			if (!num && base.State == ParameterState.Complete)
			{
				SetIncomplete();
			}
		}
	}

	public bool checkVesselWithinEnvelope(Vessel v)
	{
		switch (v.situation)
		{
		case Vessel.Situations.LANDED:
		case Vessel.Situations.SPLASHED:
		case Vessel.Situations.PRELAUNCH:
		case Vessel.Situations.FLYING:
			if (v.srfSpeed < maxSpeed)
			{
				return v.srfSpeed >= minSpeed;
			}
			return false;
		default:
			return false;
		case Vessel.Situations.SUB_ORBITAL:
		case Vessel.Situations.ORBITING:
		case Vessel.Situations.ESCAPING:
			if (v.obt_speed < maxSpeed)
			{
				return v.srfSpeed >= minSpeed;
			}
			return false;
		}
	}
}
