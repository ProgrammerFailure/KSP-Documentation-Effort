using FinePrint.Utilities;
using ns9;

namespace KSPAchievements;

public class CelestialBodySuborbit : ProgressNode
{
	public CelestialBody body;

	public VesselRef firstVessel;

	public CrewRef firstCrew;

	public CelestialBodySuborbit(CelestialBody cb)
		: base("Suborbit", startReached: false)
	{
		body = cb;
		firstVessel = new VesselRef();
		firstCrew = new CrewRef();
		OnDeploy = delegate
		{
			GameEvents.onVesselSituationChange.Add(onVesselSitChange);
		};
		OnStow = delegate
		{
			GameEvents.onVesselSituationChange.Remove(onVesselSitChange);
		};
	}

	public void onVesselSitChange(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> vs)
	{
		if (vs.host.DiscoveryInfo.Level == DiscoveryLevels.Owned && vs.host.mainBody == body && (vs.from == Vessel.Situations.SPLASHED || vs.from == Vessel.Situations.LANDED || vs.from == Vessel.Situations.FLYING || vs.from == Vessel.Situations.ORBITING) && vs.to == Vessel.Situations.SUB_ORBITAL)
		{
			CrewSensitiveComplete(vs.host);
			if (!base.IsComplete)
			{
				firstVessel = VesselRef.FromVessel(vs.host);
				firstCrew = CrewRef.FromVessel(vs.host);
				Complete();
				AwardProgressStandard((body == FlightGlobals.GetHomeBody()) ? Localizer.Format("#autoLOC_6001941") : Localizer.Format("#autoLOC_6001942", body.displayName), ProgressType.SUBORBIT, body);
			}
			Achieve();
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
}
