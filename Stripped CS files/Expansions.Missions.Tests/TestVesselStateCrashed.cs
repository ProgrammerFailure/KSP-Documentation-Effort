using System;
using Expansions.Missions.Editor;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[]
{
	typeof(ScoreModule_Resource),
	typeof(ScoreModule_Time)
})]
public class TestVesselStateCrashed : TestVessel, IScoreableObjective
{
	public bool eventFound;

	[MEGUI_ParameterSwitchCompound(order = 20, onControlCreated = "OnLocationControlCreated", guiName = "#autoLOC_8000149")]
	public ParamChoices_CelestialBodySurface locationSituation;

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8000242");
		eventFound = false;
		useActiveVessel = true;
		locationSituation = new ParamChoices_CelestialBodySurface();
		locationSituation.locationChoice = ParamChoices_CelestialBodySurface.Choices.anyData;
		locationSituation.bodyData = new MissionCelestialBody(null);
		locationSituation.biomeData = new MissionBiome(FlightGlobals.GetHomeBody(), "");
		locationSituation.areaData = new SurfaceArea(FlightGlobals.GetHomeBody(), 0.0, 0.0, 150000f);
		locationSituation.launchSiteName = "";
	}

	public override void Initialized()
	{
		eventFound = false;
		GameEvents.onVesselWillDestroy.Add(OnVesselToBeDestroyed);
	}

	public override void Cleared()
	{
		GameEvents.onVesselWillDestroy.Remove(OnVesselToBeDestroyed);
	}

	public void OnVesselToBeDestroyed(Vessel vesselDestroyed)
	{
		if (!eventFound)
		{
			base.Test();
			if (vessel == vesselDestroyed && TestVesselLocation())
			{
				eventFound = true;
			}
		}
	}

	public override bool Test()
	{
		return eventFound;
	}

	public bool TestVesselLocation()
	{
		bool result = false;
		switch (locationSituation.locationChoice)
		{
		case ParamChoices_CelestialBodySurface.Choices.anyData:
			result = true;
			break;
		case ParamChoices_CelestialBodySurface.Choices.bodyData:
			result = locationSituation.bodyData.IsValid(vessel.mainBody);
			break;
		case ParamChoices_CelestialBodySurface.Choices.biomeData:
		{
			string experimentBiome = ScienceUtil.GetExperimentBiome(vessel.mainBody, vessel.latitude, vessel.longitude);
			result = string.IsNullOrEmpty(locationSituation.biomeData.biomeName) || experimentBiome.Equals(locationSituation.biomeData.biomeName, StringComparison.InvariantCultureIgnoreCase);
			break;
		}
		case ParamChoices_CelestialBodySurface.Choices.areaData:
			if (locationSituation.areaData.body == vessel.mainBody)
			{
				result = locationSituation.areaData.IsPointInCircle(vessel.latitude, vessel.longitude);
			}
			break;
		case ParamChoices_CelestialBodySurface.Choices.launchSiteName:
			result = ((!(vessel.crashObjectName != null)) ? (locationSituation.launchSiteName == MiniBiome.ConvertTagtoLandedAt(vessel.landedAt)) : (locationSituation.launchSiteName == vessel.crashObjectName.objectName));
			break;
		}
		return result;
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004006");
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		locationSituation.Save(node);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		locationSituation.Load(node);
	}

	public object GetScoreModifier(Type scoreModule)
	{
		if (scoreModule == typeof(ScoreModule_Resource))
		{
			if (vesselID == 0)
			{
				return FlightGlobals.ActiveVessel;
			}
			uint num = base.node.mission.CurrentVesselID(base.node, vesselID);
			if (FlightGlobals.PersistentVesselIds.ContainsKey(num))
			{
				return FlightGlobals.PersistentVesselIds[num];
			}
			Debug.LogErrorFormat("[TestVesselStateCrashed] Unable to find VesselID ({0}) for Score Modifier.", num);
			return null;
		}
		return null;
	}
}
