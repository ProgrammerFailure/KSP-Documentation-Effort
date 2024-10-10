using FinePrint.Utilities;
using ns9;

namespace KSPAchievements;

public class BaseConstruction : ProgressNode
{
	public CelestialBody body;

	public VesselRef firstBase;

	public BaseConstruction(CelestialBody cb)
		: base("BaseConstruction", startReached: false)
	{
		body = cb;
		firstBase = new VesselRef();
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
		if (!pa.from.vessel.isEVA && !pa.to.vessel.isEVA && pa.from.missionID != pa.to.missionID && pa.to.vessel.LandedOrSplashed && pa.to.vessel.mainBody == body)
		{
			CrewSensitiveComplete(pa.from.vessel);
			if (!base.IsComplete)
			{
				firstBase = VesselRef.FromVessel(pa.to.vessel);
				Complete();
				AwardProgressStandard(Localizer.Format("#autoLOC_295188", body.displayName), ProgressType.BASECONSTRUCTION, body);
			}
			Achieve();
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		if (node.HasNode("base"))
		{
			firstBase.Load(node.GetNode("base"));
		}
	}

	public override void OnSave(ConfigNode node)
	{
		firstBase.Save(node.AddNode("base"));
	}
}
