using FinePrint.Utilities;
using ns9;

namespace KSPAchievements;

public class CrewRecovery : ProgressNode
{
	public CrewRef firstCrew;

	public CrewRecovery()
		: base("FirstCrewToSurvive", startReached: false)
	{
		firstCrew = new CrewRef();
		OnDeploy = delegate
		{
			GameEvents.onVesselRecovered.Add(onVesselRecovered);
		};
		OnStow = delegate
		{
			GameEvents.onVesselRecovered.Remove(onVesselRecovered);
		};
	}

	public void onVesselRecovered(ProtoVessel pv, bool quick)
	{
		if (pv.situation != Vessel.Situations.PRELAUNCH && pv.GetVesselCrew().Count > 0)
		{
			if (!base.IsComplete)
			{
				firstCrew = CrewRef.FromProtoVessel(pv);
				Complete();
				AwardProgressStandard(Localizer.Format("#autoLOC_296238"), ProgressType.CREWRECOVERY);
			}
			Achieve();
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		if (node.HasNode("crew"))
		{
			firstCrew.Load(node.GetNode("crew"));
		}
	}

	public override void OnSave(ConfigNode node)
	{
		firstCrew.Save(node.AddNode("crew"));
	}
}
