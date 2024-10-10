using System;
using FinePrint.Utilities;
using ns9;

namespace Contracts.Parameters;

[Serializable]
public class ReachDestination : ContractParameter
{
	public CelestialBody Destination;

	public string title = "";

	public ReachDestination()
	{
	}

	public ReachDestination(CelestialBody destination, string title)
	{
		Destination = destination;
		this.title = title;
	}

	public override string GetTitle()
	{
		return title + GetTitleStringShort(Destination);
	}

	public static string GetTitleStringShort(CelestialBody dest)
	{
		return Localizer.Format("#autoLOC_7001301", dest.displayName);
	}

	public override void OnLoad(ConfigNode node)
	{
		if (node.HasValue("dest"))
		{
			Destination = FlightGlobals.Bodies[int.Parse(node.GetValue("dest"))];
		}
		if (node.HasValue("title"))
		{
			title = node.GetValue("title");
		}
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("dest", FlightGlobals.Bodies.IndexOf(Destination));
		node.AddValue("title", title);
	}

	public override void OnRegister()
	{
		GameEvents.onVesselSOIChanged.Add(OnVesselSOIChange);
	}

	public override void OnUnregister()
	{
		GameEvents.onVesselSOIChanged.Remove(OnVesselSOIChange);
	}

	public override void OnReset()
	{
		if (SystemUtilities.FlightIsReady(checkVessel: true))
		{
			TrackVessel(FlightGlobals.ActiveVessel);
		}
		else
		{
			SetIncomplete();
		}
	}

	public void OnVesselSOIChange(GameEvents.HostedFromToAction<Vessel, CelestialBody> v)
	{
		if (v.host == FlightGlobals.ActiveVessel)
		{
			TrackVessel(v.host);
		}
	}

	public void TrackVessel(Vessel v)
	{
		if (base.State == ParameterState.Incomplete && checkVesselDestination(v))
		{
			SetComplete();
		}
		else if (base.State == ParameterState.Complete && !checkVesselDestination(v))
		{
			SetIncomplete();
		}
	}

	public bool checkVesselDestination(Vessel v)
	{
		return v.mainBody == Destination;
	}
}
