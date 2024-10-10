using FinePrint.Utilities;
using ns9;

namespace KSPAchievements;

public class CelestialBodySplashdown : ProgressNode
{
	public CelestialBody body;

	public CelestialBodySplashdown(CelestialBody cb)
		: base("Splashdown", startReached: false)
	{
		body = cb;
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
		if (IsValidVessel(host) && host.mainBody == body && (vs.from == Vessel.Situations.FLYING || vs.from == Vessel.Situations.SUB_ORBITAL || vs.from == Vessel.Situations.LANDED) && vs.to == Vessel.Situations.SPLASHED)
		{
			CrewSensitiveComplete(host);
			if (!base.IsComplete)
			{
				Complete();
				AwardProgressStandard((body == FlightGlobals.GetHomeBody()) ? Localizer.Format("#autoLOC_6001945") : Localizer.Format("#autoLOC_6001946", body.displayName), ProgressType.SPLASHDOWN, body);
			}
			Achieve();
			GameEvents.VesselSituation.onLand.Fire(host, host.mainBody);
			AnalyticsUtil.LogAsteroidVesselEvent(AnalyticsUtil.SpaceObjectEventTypes.splashed, HighLogic.CurrentGame, host);
			AnalyticsUtil.LogCometVesselEvent(AnalyticsUtil.SpaceObjectEventTypes.splashed, HighLogic.CurrentGame, host);
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
}
