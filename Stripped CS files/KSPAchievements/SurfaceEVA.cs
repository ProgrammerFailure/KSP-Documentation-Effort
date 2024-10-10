using FinePrint.Utilities;
using ns9;

namespace KSPAchievements;

public class SurfaceEVA : ProgressNode
{
	public CelestialBody body;

	public CrewRef firstKerbal;

	public SurfaceEVA(CelestialBody cb)
		: base("SurfaceEVA", startReached: false)
	{
		body = cb;
		firstKerbal = new CrewRef();
		OnDeploy = delegate
		{
			GameEvents.onCrewOnEva.Add(OnGoEVA);
		};
		OnStow = delegate
		{
			GameEvents.onCrewOnEva.Remove(OnGoEVA);
		};
	}

	public void OnGoEVA(GameEvents.FromToAction<Part, Part> fv)
	{
		if (fv.from.vessel.LandedOrSplashed && fv.from.vessel.mainBody == body)
		{
			if (!base.IsComplete)
			{
				firstKerbal = CrewRef.FromVessel(fv.to.vessel);
				Complete();
				AwardProgressStandard((body == FlightGlobals.GetHomeBody()) ? Localizer.Format("#autoLOC_6001955") : Localizer.Format("#autoLOC_6001956", body.displayName), ProgressType.SURFACEEVA, body);
			}
			Achieve();
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		if (node.HasNode("crew"))
		{
			firstKerbal.Load(node.GetNode("crew"));
		}
	}

	public override void OnSave(ConfigNode node)
	{
		if (firstKerbal.HasAny)
		{
			firstKerbal.Save(node.AddNode("crew"));
		}
	}
}
