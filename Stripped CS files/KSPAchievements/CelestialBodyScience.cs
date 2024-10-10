using FinePrint.Utilities;
using ns9;

namespace KSPAchievements;

public class CelestialBodyScience : ProgressNode
{
	public CelestialBody body;

	public VesselRef firstVessel;

	public CrewRef firstCrew;

	public CelestialBodyScience(CelestialBody cb)
		: base("Science", startReached: false)
	{
		body = cb;
		firstVessel = new VesselRef();
		firstCrew = new CrewRef();
		OnDeploy = delegate
		{
			GameEvents.OnScienceRecieved.Add(OnScience);
			GameEvents.OnTriggeredDataTransmission.Add(OnTriggeredScience);
		};
		OnStow = delegate
		{
			GameEvents.OnScienceRecieved.Remove(OnScience);
			GameEvents.OnTriggeredDataTransmission.Remove(OnTriggeredScience);
		};
	}

	public void OnScience(float science, ScienceSubject subject, ProtoVessel pv, bool reverseEngineered)
	{
		if (pv == null || reverseEngineered)
		{
			return;
		}
		DiscoveryLevels discoveryLevels = DiscoveryLevels.Unowned;
		if (pv.discoveryInfo.HasValue("state"))
		{
			discoveryLevels = (DiscoveryLevels)int.Parse(pv.discoveryInfo.GetValue("state"));
		}
		if (discoveryLevels == DiscoveryLevels.Owned && subject.IsFromBody(body) && science > 0f)
		{
			CrewSensitiveComplete(pv);
			if (!base.IsComplete)
			{
				firstVessel = new VesselRef
				{
					Name = pv.vesselName,
					FlagURL = pv.protoPartSnapshots[0].flagURL
				};
				firstCrew = CrewRef.FromProtoVessel(pv);
				Complete();
				AwardProgressStandard((body == FlightGlobals.GetHomeBody()) ? Localizer.Format("#autoLOC_6001949") : Localizer.Format("#autoLOC_6001950", body.displayName), ProgressType.SCIENCE, body);
				AnalyticsUtil.LogCelestialBodyProgress(body, AnalyticsUtil.cbProgressTypes.science);
			}
			Achieve();
		}
	}

	public void OnTriggeredScience(ScienceData data, Vessel origin, bool xmitAborted)
	{
		if (!(data == null || origin == null || origin.mainBody == null || xmitAborted) && origin.DiscoveryInfo.Level == DiscoveryLevels.Owned && !(origin.mainBody != body) && data.dataAmount > 0f)
		{
			CrewSensitiveComplete(origin);
			if (!base.IsComplete)
			{
				firstVessel = VesselRef.FromVessel(origin);
				firstCrew = CrewRef.FromVessel(origin);
				Complete();
				AwardProgressStandard((body == FlightGlobals.GetHomeBody()) ? Localizer.Format("#autoLOC_6001951") : Localizer.Format("#autoLOC_6001952", body.displayName), ProgressType.SCIENCE, body);
			}
			Achieve();
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

	public override void OnSave(ConfigNode node)
	{
		firstVessel.Save(node.AddNode("vessel"));
		if (firstCrew.HasAny)
		{
			firstCrew.Save(node.AddNode("crew"));
		}
	}
}
