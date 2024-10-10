using System;
using FinePrint.Utilities;
using ns9;

namespace Contracts.Parameters;

[Serializable]
public class ReachSituation : ContractParameter
{
	public Vessel.Situations Situation;

	public string title = "";

	public ReachSituation()
	{
	}

	public ReachSituation(Vessel.Situations sit, string title)
	{
		Situation = sit;
		this.title = title;
	}

	public override string GetTitle()
	{
		return title + GetTitleStringShort(Situation);
	}

	public static string GetTitleStringShort(Vessel.Situations sit)
	{
		return sit switch
		{
			Vessel.Situations.FLYING => Localizer.Format("#autoLOC_268856"), 
			Vessel.Situations.LANDED => Localizer.Format("#autoLOC_268855"), 
			Vessel.Situations.SPLASHED => Localizer.Format("#autoLOC_268858"), 
			Vessel.Situations.PRELAUNCH => Localizer.Format("#autoLOC_268854"), 
			Vessel.Situations.ESCAPING => Localizer.Format("#autoLOC_268860"), 
			Vessel.Situations.ORBITING => Localizer.Format("#autoLOC_268857"), 
			Vessel.Situations.SUB_ORBITAL => Localizer.Format("#autoLOC_268859"), 
			_ => Localizer.Format("#autoLOC_268861"), 
		};
	}

	public override void OnLoad(ConfigNode node)
	{
		if (node.HasValue("sit"))
		{
			Situation = (Vessel.Situations)Enum.Parse(typeof(Vessel.Situations), node.GetValue("sit"));
		}
		if (node.HasValue("title"))
		{
			title = node.GetValue("title");
		}
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("sit", Situation.ToString());
		node.AddValue("title", title);
	}

	public override void OnRegister()
	{
		GameEvents.onVesselSituationChange.Add(OnVesselSituationChange);
	}

	public override void OnUnregister()
	{
		GameEvents.onVesselSituationChange.Remove(OnVesselSituationChange);
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

	public void OnVesselSituationChange(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> v)
	{
		if (v.host == FlightGlobals.ActiveVessel)
		{
			TrackVessel(v.host);
		}
	}

	public void TrackVessel(Vessel v)
	{
		if (base.State == ParameterState.Incomplete && checkVesselSituation(v))
		{
			SetComplete();
		}
		else if (base.State == ParameterState.Complete && !checkVesselSituation(v))
		{
			SetIncomplete();
		}
	}

	public bool checkVesselSituation(Vessel v)
	{
		if (Situation == Vessel.Situations.LANDED)
		{
			if (v.situation != Vessel.Situations.LANDED)
			{
				return v.situation == Vessel.Situations.PRELAUNCH;
			}
			return true;
		}
		if (Situation == Vessel.Situations.PRELAUNCH)
		{
			if (v.situation != Vessel.Situations.PRELAUNCH)
			{
				if (v.situation == Vessel.Situations.LANDED)
				{
					if (!v.landedAt.Contains("Runway"))
					{
						return v.landedAt.Contains("LaunchPad");
					}
					return true;
				}
				return false;
			}
			return true;
		}
		return v.situation == Situation;
	}
}
