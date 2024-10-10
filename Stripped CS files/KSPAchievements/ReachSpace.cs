using FinePrint.Utilities;
using ns9;

namespace KSPAchievements;

public class ReachSpace : ProgressNode
{
	public VesselRef firstVessel;

	public CrewRef firstCrew;

	public ReachSpace()
		: base("ReachedSpace", startReached: false)
	{
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
		if (vs.host.DiscoveryInfo.Level == DiscoveryLevels.Owned && vs.from == Vessel.Situations.FLYING && vs.to == Vessel.Situations.SUB_ORBITAL)
		{
			CrewSensitiveComplete(vs.host);
			if (!base.IsComplete)
			{
				firstVessel = VesselRef.FromVessel(vs.host);
				firstCrew = CrewRef.FromVessel(vs.host);
				Complete();
				AwardProgressStandard(Localizer.Format("#autoLOC_297725"), ProgressType.REACHSPACE);
			}
			Achieve();
			GameEvents.VesselSituation.onReachSpace.Fire(vs.host);
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
			firstVessel = new VesselRef();
			firstVessel.Load(node.GetNode("vessel"));
		}
		if (node.HasNode("crew"))
		{
			firstCrew = new CrewRef();
			firstCrew.Load(node.GetNode("crew"));
		}
	}
}
