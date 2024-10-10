using System.Collections.Generic;
using System.Globalization;
using Contracts;
using FinePrint.Utilities;
using ns9;

namespace FinePrint.Contracts.Parameters;

public class KerbalDestinationParameter : ContractParameter
{
	public CelestialBody targetBody;

	public FlightLog.EntryType targetType;

	public string kerbalName;

	public bool eventsAdded;

	public KerbalDestinationParameter()
	{
		targetBody = Planetarium.fetch.Home;
		targetType = FlightLog.EntryType.Suborbit;
		kerbalName = "Jebediah Kerman";
	}

	public KerbalDestinationParameter(CelestialBody targetBody, FlightLog.EntryType targetType, string kerbalName)
	{
		this.targetBody = targetBody;
		this.targetType = targetType;
		this.kerbalName = kerbalName;
	}

	public override string GetHashString()
	{
		return SystemUtilities.SuperSeed(base.Root).ToString(CultureInfo.InvariantCulture) + base.String_0;
	}

	public override string GetTitle()
	{
		return targetType switch
		{
			FlightLog.EntryType.Land => Localizer.Format("#autoLOC_283270", targetBody.displayName), 
			FlightLog.EntryType.Flight => Localizer.Format("#autoLOC_283274", targetBody.displayName), 
			FlightLog.EntryType.Flyby => Localizer.Format("#autoLOC_283276", targetBody.displayName), 
			FlightLog.EntryType.Orbit => Localizer.Format("#autoLOC_283278", targetBody.displayName), 
			FlightLog.EntryType.Suborbit => Localizer.Format("#autoLOC_283272", targetBody.displayName), 
			_ => Localizer.Format("#autoLOC_283280"), 
		};
	}

	public override void OnRegister()
	{
		base.DisableOnStateChange = true;
		if (base.Root.ContractState == Contract.State.Active && !CheckFlightLogs())
		{
			GameEvents.OnFlightLogRecorded.Add(CheckFlightLog);
			eventsAdded = true;
		}
	}

	public override void OnUnregister()
	{
		if (eventsAdded)
		{
			GameEvents.OnFlightLogRecorded.Remove(CheckFlightLog);
		}
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("targetBody", targetBody.flightGlobalsIndex);
		node.AddValue("targetType", targetType);
		node.AddValue("kerbalName", kerbalName);
	}

	public override void OnLoad(ConfigNode node)
	{
		SystemUtilities.LoadNode(node, "KerbalDestinationParameter", "targetBody", ref targetBody, Planetarium.fetch.Home);
		SystemUtilities.LoadNode(node, "KerbalDestinationParameter", "targetType", ref targetType, FlightLog.EntryType.Suborbit);
		SystemUtilities.LoadNode(node, "KerbalDestinationParameter", "kerbalName", ref kerbalName, "Jebediah Kerman");
	}

	public void CheckFlightLog(Vessel v)
	{
		List<ProtoCrewMember> vesselCrew = v.GetVesselCrew();
		int count = vesselCrew.Count;
		while (count-- > 0)
		{
			ProtoCrewMember protoCrewMember = vesselCrew[count];
			if (protoCrewMember.name != kerbalName)
			{
				continue;
			}
			List<FlightLog.Entry> list = new List<FlightLog.Entry>(protoCrewMember.flightLog.Entries);
			list.AddRange(protoCrewMember.careerLog.Entries);
			for (int num = list.Count - 1; num >= 0; num--)
			{
				FlightLog.Entry entry = list[num];
				if (entry.type == targetType.ToString() && entry.target == targetBody.GetName())
				{
					SetComplete();
				}
			}
		}
	}

	public bool CheckFlightLogs()
	{
		ProtoCrewMember protoCrewMember = null;
		if (HighLogic.CurrentGame != null)
		{
			if (HighLogic.CurrentGame.CrewRoster.Exists(kerbalName))
			{
				protoCrewMember = HighLogic.CurrentGame.CrewRoster[kerbalName];
			}
			if (protoCrewMember != null)
			{
				List<FlightLog.Entry> list = new List<FlightLog.Entry>(protoCrewMember.flightLog.Entries);
				list.AddRange(protoCrewMember.careerLog.Entries);
				int num = list.Count - 1;
				while (num >= 0)
				{
					FlightLog.Entry entry = list[num];
					if (!(entry.type == targetType.ToString()) || !(entry.target == targetBody.GetName()))
					{
						num--;
						continue;
					}
					SetComplete();
					return true;
				}
			}
		}
		return false;
	}
}
