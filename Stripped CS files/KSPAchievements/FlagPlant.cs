using FinePrint.Utilities;
using ns9;

namespace KSPAchievements;

public class FlagPlant : ProgressNode
{
	public CelestialBody body;

	public CrewRef firstKerbal;

	public FlagPlant(CelestialBody cb)
		: base("FlagPlant", startReached: false)
	{
		body = cb;
		firstKerbal = new CrewRef();
		OnDeploy = delegate
		{
			GameEvents.onFlagPlant.Add(OnFlagPlant);
		};
		OnStow = delegate
		{
			GameEvents.onFlagPlant.Remove(OnFlagPlant);
		};
	}

	public void OnFlagPlant(Vessel v)
	{
		if (v.orbit.referenceBody == body)
		{
			if (!base.IsComplete)
			{
				firstKerbal = CrewRef.FromVessel(v);
				Complete();
				AwardProgressStandard((body == FlightGlobals.GetHomeBody()) ? Localizer.Format("#autoLOC_6001953") : Localizer.Format("#autoLOC_6001954", body.displayName), ProgressType.FLAGPLANT, body);
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
