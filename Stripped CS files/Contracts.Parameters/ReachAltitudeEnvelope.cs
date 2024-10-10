using System;
using FinePrint.Utilities;
using ns9;

namespace Contracts.Parameters;

[Serializable]
public class ReachAltitudeEnvelope : ContractParameter
{
	public double minAltitude;

	public double maxAltitude;

	public ReachAltitudeEnvelope()
	{
		minAltitude = 0.0;
		maxAltitude = double.MaxValue;
	}

	public ReachAltitudeEnvelope(float minAlt, float maxAlt)
	{
		minAltitude = Math.Min(minAlt, maxAlt);
		maxAltitude = Math.Max(minAlt, maxAlt);
	}

	public override string GetTitle()
	{
		return Localizer.Format("#autoLOC_268606", minAltitude.ToString("N0"), maxAltitude.ToString("N0"));
	}

	public override void OnReset()
	{
		SetIncomplete();
	}

	public override void OnLoad(ConfigNode node)
	{
		SystemUtilities.LoadNode(node, "ReachAltitudeEnvelope", "minAlt", ref minAltitude, 0.0);
		SystemUtilities.LoadNode(node, "ReachAltitudeEnvelope", "maxAlt", ref maxAltitude, double.MaxValue);
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("minAlt", minAltitude);
		node.AddValue("maxAlt", maxAltitude);
	}

	public override void OnUpdate()
	{
		if (base.Root.ContractState != Contract.State.Active || !HighLogic.LoadedSceneIsFlight || !FlightGlobals.ready)
		{
			return;
		}
		if (FlightGlobals.ActiveVessel.altitude < maxAltitude)
		{
			bool num = FlightGlobals.ActiveVessel.altitude >= minAltitude;
			if (num && base.State == ParameterState.Incomplete)
			{
				SetComplete();
			}
			if (num)
			{
				return;
			}
		}
		if (base.State == ParameterState.Complete)
		{
			SetIncomplete();
		}
	}
}
