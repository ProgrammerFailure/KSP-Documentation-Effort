using FinePrint.Utilities;
using ns9;

namespace KSPAchievements;

public class CelestialBodyFlyby : ProgressNode
{
	public CelestialBody body;

	public CelestialBodyFlyby(CelestialBody cb)
		: base("Flyby", startReached: false)
	{
		body = cb;
		OnDeploy = delegate
		{
			GameEvents.onVesselSOIChanged.Add(OnVesselSOIChange);
		};
		OnStow = delegate
		{
			GameEvents.onVesselSOIChanged.Remove(OnVesselSOIChange);
		};
	}

	public void OnVesselSOIChange(GameEvents.HostedFromToAction<Vessel, CelestialBody> vcb)
	{
		if (vcb.host.DiscoveryInfo.Level == DiscoveryLevels.Owned && vcb.from != null && vcb.to == body)
		{
			CrewSensitiveComplete(vcb.host);
			if (!base.IsComplete)
			{
				Complete();
				AwardProgressStandard(Localizer.Format("#autoLOC_295360", body.displayName), ProgressType.FLYBY, body);
				AnalyticsUtil.LogCelestialBodyProgress(body, AnalyticsUtil.cbProgressTypes.reached);
			}
			Achieve();
			GameEvents.VesselSituation.onFlyBy.Fire(vcb.host, body);
		}
	}
}
