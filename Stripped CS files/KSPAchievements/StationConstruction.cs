using FinePrint.Utilities;
using ns9;

namespace KSPAchievements;

public class StationConstruction : ProgressNode
{
	public CelestialBody body;

	public VesselRef firstStation;

	public StationConstruction(CelestialBody cb)
		: base("StationConstruction", startReached: false)
	{
		body = cb;
		firstStation = new VesselRef();
		OnDeploy = delegate
		{
			GameEvents.onPartCouple.Add(onPartsCoupled);
		};
		OnStow = delegate
		{
			GameEvents.onPartCouple.Remove(onPartsCoupled);
		};
	}

	public void onPartsCoupled(GameEvents.FromToAction<Part, Part> pa)
	{
		if (!pa.from.vessel.isEVA && !pa.to.vessel.isEVA && pa.from.missionID != pa.to.missionID && pa.to.vessel.situation == Vessel.Situations.ORBITING && pa.to.vessel.mainBody == body)
		{
			CrewSensitiveComplete(pa.from.vessel);
			if (!base.IsComplete)
			{
				firstStation = VesselRef.FromVessel(pa.to.vessel);
				Complete();
				AwardProgressStandard(Localizer.Format("#autoLOC_298375", body.displayName), ProgressType.STATIONCONSTRUCTION, body);
			}
			Achieve();
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		if (node.HasNode("station"))
		{
			firstStation.Load(node.GetNode("station"));
		}
	}

	public override void OnSave(ConfigNode node)
	{
		firstStation.Save(node.AddNode("station"));
	}
}
