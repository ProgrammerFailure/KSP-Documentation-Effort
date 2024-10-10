using System.Collections;
using UnityEngine;

namespace KSPAchievements;

public class TargetedLanding : ProgressNode
{
	public string targetTag;

	public ReturnFrom Level;

	public TargetedLanding(string targetTag, ReturnFrom level)
		: base(targetTag + "Landing", startReached: false)
	{
		this.targetTag = targetTag;
		Level = level;
		OnDeploy = delegate
		{
			GameEvents.onVesselSituationChange.Add(OnVesselSituationChange);
		};
		OnStow = delegate
		{
			GameEvents.onVesselSituationChange.Remove(OnVesselSituationChange);
		};
	}

	public void OnVesselSituationChange(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> vs)
	{
		if (vs.host.DiscoveryInfo.Level == DiscoveryLevels.Owned && (vs.from == Vessel.Situations.FLYING || vs.from == Vessel.Situations.SUB_ORBITAL || vs.from == Vessel.Situations.SPLASHED) && vs.to == Vessel.Situations.LANDED)
		{
			vs.host.StartCoroutine(WaitAndCheckLandedTag(vs.host, targetTag));
		}
	}

	public IEnumerator WaitAndCheckLandedTag(Vessel host, string tgtTag)
	{
		yield return new WaitForEndOfFrame();
		if (host.landedAt == null || !host.landedAt.Contains(tgtTag))
		{
			yield break;
		}
		VesselTripLog vesselTripLog = VesselTripLog.FromVessel(host);
		switch (Level)
		{
		case ReturnFrom.FlyBy:
			if (vesselTripLog.Log.HasEntry(FlightLog.EntryType.Flyby, FlightGlobals.GetHomeBodyName()))
			{
				if (!base.IsComplete)
				{
					Complete();
				}
				Achieve();
				GameEvents.VesselSituation.onTargetedLanding.Fire(host, tgtTag, Level);
			}
			break;
		case ReturnFrom.Orbit:
			if (vesselTripLog.Log.HasEntry(FlightLog.EntryType.Orbit, FlightGlobals.GetHomeBodyName()))
			{
				if (!base.IsComplete)
				{
					Complete();
				}
				Achieve();
				GameEvents.VesselSituation.onTargetedLanding.Fire(host, tgtTag, Level);
			}
			break;
		case ReturnFrom.SubOrbit:
			if (vesselTripLog.Log.HasEntry(FlightLog.EntryType.Suborbit, FlightGlobals.GetHomeBodyName()))
			{
				if (!base.IsComplete)
				{
					Complete();
				}
				Achieve();
				GameEvents.VesselSituation.onTargetedLanding.Fire(host, tgtTag, Level);
			}
			break;
		case ReturnFrom.Flight:
			if (vesselTripLog.Log.HasEntry(FlightLog.EntryType.Flight, FlightGlobals.GetHomeBodyName()))
			{
				if (!base.IsComplete)
				{
					Complete();
				}
				Achieve();
				GameEvents.VesselSituation.onTargetedLanding.Fire(host, tgtTag, Level);
			}
			break;
		}
	}
}
