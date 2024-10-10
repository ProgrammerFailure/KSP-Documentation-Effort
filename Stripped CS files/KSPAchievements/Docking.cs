using FinePrint.Utilities;
using ns9;

namespace KSPAchievements;

public class Docking : ProgressNode
{
	public CelestialBody body;

	public Docking(CelestialBody cb)
		: base("Docking", startReached: false)
	{
		body = cb;
		OnDeploy = delegate
		{
			GameEvents.onPartCouple.Add(OnPartsCoupled);
		};
		OnStow = delegate
		{
			GameEvents.onPartCouple.Remove(OnPartsCoupled);
		};
	}

	public void OnPartsCoupled(GameEvents.FromToAction<Part, Part> pa)
	{
		if (!pa.from.vessel.isEVA && !pa.to.vessel.isEVA && pa.from.missionID != pa.to.missionID && pa.from.vessel.mainBody == body)
		{
			CrewSensitiveComplete(pa.from.vessel);
			if (!base.IsComplete)
			{
				Complete();
				AwardProgressStandard(Localizer.Format("#autoLOC_296294", body.displayName), ProgressType.DOCKING, body);
				AnalyticsUtil.LogVesselDocked(pa.from.vessel);
			}
			Achieve();
		}
	}
}
