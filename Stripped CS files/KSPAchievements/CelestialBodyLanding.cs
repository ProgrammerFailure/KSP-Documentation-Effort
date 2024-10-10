using FinePrint.Utilities;
using ns9;

namespace KSPAchievements;

public class CelestialBodyLanding : ProgressNode
{
	public CelestialBody body;

	public VesselRef firstVessel;

	public CrewRef firstCrew;

	public CelestialBodyLanding(CelestialBody cb)
		: base("Landing", startReached: false)
	{
		body = cb;
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
		Vessel host = vs.host;
		if (IsValidVessel(host) && host.mainBody == body && (vs.from == Vessel.Situations.FLYING || vs.from == Vessel.Situations.SUB_ORBITAL || vs.from == Vessel.Situations.SPLASHED) && vs.to == Vessel.Situations.LANDED && (!(body == FlightGlobals.GetHomeBody()) || host.landedAt == null || (!host.landedAt.Contains("Runway") && !host.landedAt.Contains("LaunchPad"))))
		{
			CrewSensitiveComplete(host);
			if (!base.IsComplete)
			{
				firstVessel = VesselRef.FromVessel(host);
				firstCrew = CrewRef.FromVessel(host);
				Complete();
				AwardProgressStandard((body == FlightGlobals.GetHomeBody()) ? Localizer.Format("#autoLOC_6001943") : Localizer.Format("#autoLOC_6001944", body.displayName), ProgressType.LANDING, body);
				AnalyticsUtil.LogCelestialBodyProgress(body, AnalyticsUtil.cbProgressTypes.landed);
			}
			Achieve();
			GameEvents.VesselSituation.onLand.Fire(host, host.mainBody);
			AnalyticsUtil.LogAsteroidVesselEvent(AnalyticsUtil.SpaceObjectEventTypes.landed, HighLogic.CurrentGame, host);
			AnalyticsUtil.LogCometVesselEvent(AnalyticsUtil.SpaceObjectEventTypes.landed, HighLogic.CurrentGame, host);
		}
	}

	public bool IsValidVessel(Vessel v)
	{
		if (!(v == null) && v.DiscoveryInfo != null && !(v.mainBody == null))
		{
			if (v.DiscoveryInfo.Level != DiscoveryLevels.Owned)
			{
				return false;
			}
			return true;
		}
		return false;
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
