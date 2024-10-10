using System;
using Expansions.Missions.Editor;
using FinePrint;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[]
{
	typeof(ScoreModule_Resource),
	typeof(ScoreModule_Time),
	typeof(ScoreModule_Accuracy)
})]
public class TestPlantFlag : TestModule, IScoreableObjective, INodeWaypoint, ITestNodeLabel, INodeBody
{
	[MEGUI_ParameterSwitchCompound(excludeParamFields = "launchsiteName", guiName = "#autoLOC_8000149")]
	public ParamChoices_CelestialBodySurface situation;

	public bool eventFound;

	public Vessel v;

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8000192");
		eventFound = false;
		base.Initialized();
		situation = new ParamChoices_CelestialBodySurface();
		situation.bodyData = new MissionCelestialBody(FlightGlobals.GetHomeBody());
		situation.biomeData = new MissionBiome(FlightGlobals.GetHomeBody(), "");
		situation.areaData = new SurfaceArea(FlightGlobals.GetHomeBody(), 0.0, 0.0, 150000f);
		situation.launchSiteName = "";
	}

	public override void Initialized()
	{
		eventFound = false;
		GameEvents.afterFlagPlanted.Add(AfterFlagPlanted);
	}

	public override void Cleared()
	{
		base.Cleared();
		GameEvents.afterFlagPlanted.Remove(AfterFlagPlanted);
	}

	public override bool Test()
	{
		return eventFound;
	}

	public void AfterFlagPlanted(FlagSite flagSite)
	{
		v = flagSite.vessel;
		CelestialBody mainBody = v.mainBody;
		switch (situation.locationChoice)
		{
		default:
			eventFound = true;
			break;
		case ParamChoices_CelestialBodySurface.Choices.bodyData:
			if (situation.bodyData.IsValid(mainBody))
			{
				eventFound = true;
			}
			break;
		case ParamChoices_CelestialBodySurface.Choices.biomeData:
			if (!(mainBody != situation.biomeData.body))
			{
				string experimentBiome = ScienceUtil.GetExperimentBiome(mainBody, v.latitude, v.longitude);
				if (string.IsNullOrEmpty(situation.biomeData.biomeName) || experimentBiome.Equals(situation.biomeData.biomeName, StringComparison.InvariantCultureIgnoreCase))
				{
					eventFound = true;
				}
			}
			break;
		case ParamChoices_CelestialBodySurface.Choices.areaData:
			if (!(mainBody != situation.areaData.body))
			{
				eventFound = situation.areaData.IsPointInCircle(v.latitude, v.longitude);
			}
			break;
		}
	}

	public bool HasNodeWaypoint()
	{
		return situation.HasWaypoint();
	}

	public Waypoint GetNodeWaypoint()
	{
		Waypoint waypoint = situation.GetWaypoint();
		if (waypoint == null)
		{
			return null;
		}
		waypoint.name = title;
		return waypoint;
	}

	public bool HasNodeBody()
	{
		return true;
	}

	public CelestialBody GetNodeBody()
	{
		return situation.bodyData.Body;
	}

	public bool HasNodeLabel()
	{
		return situation.HasNodeLabel();
	}

	public bool HasWorldPosition()
	{
		return situation.HasWorldPosition();
	}

	public Vector3 GetWorldPosition()
	{
		return situation.GetWorldPosition();
	}

	public string GetExtraText()
	{
		return situation.GetExtraText();
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004026");
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		situation.Save(node);
		node.AddValue("eventFound", eventFound);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		situation.Load(node);
		node.TryGetValue("eventFound", ref eventFound);
	}

	public object GetScoreModifier(Type scoreModule)
	{
		if (scoreModule == typeof(ScoreModule_Resource))
		{
			return FlightGlobals.ActiveVessel;
		}
		if (scoreModule == typeof(ScoreModule_Accuracy))
		{
			if (situation.locationChoice == ParamChoices_CelestialBodySurface.Choices.areaData)
			{
				return situation.areaData.PointInCircleAccuracy(v.latitude, v.longitude);
			}
			return 1.0;
		}
		return null;
	}
}
