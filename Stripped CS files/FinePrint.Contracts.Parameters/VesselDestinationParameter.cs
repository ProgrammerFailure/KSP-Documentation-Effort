using System.Collections.Generic;
using System.Globalization;
using Contracts;
using FinePrint.Utilities;
using ns9;

namespace FinePrint.Contracts.Parameters;

public class VesselDestinationParameter : ContractParameter
{
	public CelestialBody targetBody;

	public FlightLog.EntryType targetType;

	public bool eventsAdded;

	public VesselDestinationParameter()
	{
		targetBody = Planetarium.fetch.Home;
		targetType = FlightLog.EntryType.Suborbit;
	}

	public VesselDestinationParameter(CelestialBody targetBody, FlightLog.EntryType targetType)
	{
		this.targetBody = targetBody;
		this.targetType = targetType;
	}

	public override string GetHashString()
	{
		return SystemUtilities.SuperSeed(base.Root).ToString(CultureInfo.InvariantCulture) + base.String_0;
	}

	public override string GetTitle()
	{
		return targetType switch
		{
			FlightLog.EntryType.Land => Localizer.Format("#autoLOC_286262", targetBody.displayName), 
			FlightLog.EntryType.Flight => Localizer.Format("#autoLOC_286266", targetBody.displayName), 
			FlightLog.EntryType.Flyby => Localizer.Format("#autoLOC_286268", targetBody.displayName), 
			FlightLog.EntryType.Orbit => Localizer.Format("#autoLOC_286270", targetBody.displayName), 
			FlightLog.EntryType.Suborbit => Localizer.Format("#autoLOC_286264", targetBody.displayName), 
			_ => Localizer.Format("#autoLOC_286272"), 
		};
	}

	public override void OnRegister()
	{
		base.DisableOnStateChange = false;
		if (base.Root.ContractState == Contract.State.Active)
		{
			GameEvents.OnFlightLogRecorded.Add(CheckFlightLog);
			GameEvents.onVesselWasModified.Add(CheckFlightLog);
			eventsAdded = true;
		}
	}

	public override void OnUnregister()
	{
		if (eventsAdded)
		{
			GameEvents.OnFlightLogRecorded.Remove(CheckFlightLog);
			GameEvents.onVesselWasModified.Remove(CheckFlightLog);
		}
	}

	public override void OnReset()
	{
		if (SystemUtilities.FlightIsReady(checkVessel: true))
		{
			CheckFlightLog(FlightGlobals.ActiveVessel);
		}
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("targetBody", targetBody.flightGlobalsIndex);
		node.AddValue("targetType", targetType);
	}

	public override void OnLoad(ConfigNode node)
	{
		SystemUtilities.LoadNode(node, "VesselDestinationParameter", "targetBody", ref targetBody, Planetarium.fetch.Home);
		SystemUtilities.LoadNode(node, "VesselDestinationParameter", "targetType", ref targetType, FlightLog.EntryType.Suborbit);
	}

	public void CheckFlightLog(Vessel v)
	{
		if (v.vesselType <= VesselType.SpaceObject)
		{
			return;
		}
		List<FlightLog.Entry> entries = VesselTripLog.FromVessel(v).Log.Entries;
		int count = entries.Count;
		FlightLog.Entry entry;
		do
		{
			if (count-- > 0)
			{
				entry = entries[count];
				continue;
			}
			SetIncomplete();
			return;
		}
		while (entry.type != targetType.ToString() || entry.target != targetBody.GetName());
		SetComplete();
	}
}
