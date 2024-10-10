using FinePrint.Utilities;
using ns9;

namespace KSPAchievements;

public class CelestialBodyReturn : ProgressNode
{
	public CelestialBody body;

	public ReturnFrom Level;

	public VesselRef firstVessel;

	public CrewRef firstCrew;

	public CelestialBodyReturn(CelestialBody cb, ReturnFrom level)
		: base("ReturnFrom" + level, startReached: false)
	{
		body = cb;
		Level = level;
		firstVessel = new VesselRef();
		firstCrew = new CrewRef();
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
		if ((vs.to != Vessel.Situations.LANDED && vs.to != Vessel.Situations.SPLASHED) || vs.from != Vessel.Situations.FLYING || !vs.host.mainBody.isHomeWorld)
		{
			return;
		}
		VesselTripLog vesselTripLog = VesselTripLog.FromVessel(vs.host);
		if (!base.IsComplete)
		{
			firstVessel = VesselRef.FromVessel(vs.host);
			firstCrew = CrewRef.FromVessel(vs.host);
		}
		CrewSensitiveComplete(vs.host);
		switch (Level)
		{
		case ReturnFrom.FlyBy:
			if (vesselTripLog.Log.HasEntry(FlightLog.EntryType.Flyby, body.name))
			{
				if (!base.IsComplete)
				{
					Complete();
					AwardProgressStandard(Localizer.Format("#autoLOC_295665", body.displayName), ProgressType.FLYBYRETURN, body);
					AnalyticsUtil.LogCelestialBodyProgress(body, AnalyticsUtil.cbProgressTypes.returned);
				}
				Achieve();
				GameEvents.VesselSituation.onReturnFromOrbit.Fire(vs.host, body);
			}
			break;
		case ReturnFrom.Orbit:
			if (vesselTripLog.Log.HasEntry(FlightLog.EntryType.Orbit, body.name))
			{
				if (!base.IsComplete)
				{
					Complete();
					AwardProgressStandard((body == FlightGlobals.GetHomeBody()) ? Localizer.Format("#autoLOC_6001947") : Localizer.Format("#autoLOC_6001948", body.displayName), ProgressType.ORBITRETURN, body);
					AnalyticsUtil.LogCelestialBodyProgress(body, AnalyticsUtil.cbProgressTypes.returned);
				}
				Achieve();
				GameEvents.VesselSituation.onReturnFromOrbit.Fire(vs.host, body);
			}
			break;
		case ReturnFrom.Surface:
			if (vesselTripLog.Log.HasEntry(FlightLog.EntryType.Land, body.name))
			{
				if (!base.IsComplete)
				{
					Complete();
					AwardProgressStandard(Localizer.Format("#autoLOC_295693", body.displayName), ProgressType.LANDINGRETURN, body);
					AnalyticsUtil.LogCelestialBodyProgress(body, AnalyticsUtil.cbProgressTypes.returned);
				}
				Achieve();
				GameEvents.VesselSituation.onReturnFromSurface.Fire(vs.host, body);
			}
			break;
		case ReturnFrom.SubOrbit:
		case ReturnFrom.Flight:
			break;
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		if (node.HasNode("vessel"))
		{
			firstVessel.Load(node.GetNode("vessel"));
		}
		if (node.HasNode("crew"))
		{
			firstCrew.Load(node.GetNode("crew"));
		}
	}

	public override void OnSave(ConfigNode node)
	{
		firstVessel.Save(node.AddNode("vessel"));
		if (firstCrew.HasAny)
		{
			firstCrew.Save(node.AddNode("crew"));
		}
	}
}
