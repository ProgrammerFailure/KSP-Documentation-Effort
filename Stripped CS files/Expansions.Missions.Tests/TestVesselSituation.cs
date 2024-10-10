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
public abstract class TestVesselSituation : TestVessel, IScoreableObjective
{
	[MEGUI_Time(order = 50, resetValue = "1", guiName = "#autoLOC_8003019")]
	public double stabilizationTime = 1.0;

	public Vessel.Situations situation;

	public MEGUIParameterNumberRange surfaceVelocityRange;

	public string currentBiome;

	public bool speedSuccess;

	public bool situationSuccess;

	public bool locationSuccess;

	public double successStartTime = -1.0;

	public abstract double SurfaceVelocity { get; }

	public abstract bool IgnoreSurfaceVelocity { get; }

	public abstract ParamChoices_CelestialBodySurface LocationSituation { get; }

	public TestVesselSituation()
	{
	}

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8000118");
		situation = Vessel.Situations.PRELAUNCH;
		useActiveVessel = true;
	}

	public override void Initialized()
	{
		base.Initialized();
		locationSuccess = false;
		situationSuccess = false;
		speedSuccess = false;
	}

	public override bool Test()
	{
		base.Test();
		situationSuccess = vessel != null && vessel.situation == situation;
		if (!situationSuccess)
		{
			successStartTime = -1.0;
			return false;
		}
		switch (LocationSituation.locationChoice)
		{
		default:
			locationSuccess = false;
			break;
		case ParamChoices_CelestialBodySurface.Choices.bodyData:
			locationSuccess = LocationSituation.bodyData.IsValid(vessel.mainBody);
			break;
		case ParamChoices_CelestialBodySurface.Choices.biomeData:
			currentBiome = ScienceUtil.GetExperimentBiome(vessel.mainBody, vessel.latitude, vessel.longitude);
			locationSuccess = string.IsNullOrEmpty(LocationSituation.biomeData.biomeName) || currentBiome.Equals(LocationSituation.biomeData.biomeName, StringComparison.InvariantCultureIgnoreCase);
			break;
		case ParamChoices_CelestialBodySurface.Choices.areaData:
			if (LocationSituation.areaData.body == vessel.mainBody)
			{
				locationSuccess = LocationSituation.areaData.IsPointInCircle(vessel.latitude, vessel.longitude);
			}
			break;
		case ParamChoices_CelestialBodySurface.Choices.launchSiteName:
			if (vessel.crashObjectName != null)
			{
				locationSuccess = LocationSituation.launchSiteName == vessel.crashObjectName.objectName;
			}
			else
			{
				locationSuccess = LocationSituation.launchSiteName == MiniBiome.ConvertTagtoLandedAt(vessel.landedAt);
			}
			break;
		}
		if (!locationSuccess)
		{
			successStartTime = -1.0;
			return false;
		}
		speedSuccess = IgnoreSurfaceVelocity || vessel.srfSpeed <= SurfaceVelocity + 1.401298464324817E-45;
		if (!speedSuccess)
		{
			successStartTime = -1.0;
			return false;
		}
		if (successStartTime < 0.0)
		{
			successStartTime = Planetarium.GetUniversalTime();
		}
		return successStartTime + stabilizationTime < Planetarium.GetUniversalTime();
	}

	public void OnSurfaceVelocityControlCreated(MEGUIParameterNumberRange parameter)
	{
		surfaceVelocityRange = parameter;
	}

	public void OnIgnoreSurfaceVelocity(bool value)
	{
		surfaceVelocityRange.gameObject.SetActive(!value);
	}

	public bool HasNodeWaypoint()
	{
		return LocationSituation.HasWaypoint();
	}

	public Waypoint GetNodeWaypoint()
	{
		Waypoint waypoint = LocationSituation.GetWaypoint();
		if (waypoint == null)
		{
			return null;
		}
		waypoint.name = title;
		return waypoint;
	}

	public bool HasNodeLabel()
	{
		return LocationSituation.HasNodeLabel();
	}

	public bool HasWorldPosition()
	{
		return LocationSituation.HasWorldPosition();
	}

	public Vector3 GetWorldPosition()
	{
		return LocationSituation.GetWorldPosition();
	}

	public string GetExtraText()
	{
		return LocationSituation.GetExtraText();
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "surfaceVelocity")
		{
			if (IgnoreSurfaceVelocity)
			{
				return Localizer.Format("#autoLOC_8004190", field.guiName, Localizer.Format("#autoLOC_8001004"));
			}
			return field.guiName + ": " + SurfaceVelocity.ToString("0.0") + Localizer.Format("#autoLOC_180095");
		}
		if (field.name == "stabilizationTime")
		{
			return field.guiName + ": " + stabilizationTime.ToString("0") + Localizer.Format("#autoLOC_6002317");
		}
		return base.GetNodeBodyParameterString(field);
	}

	public bool HasNodeBody()
	{
		if (LocationSituation != null)
		{
			return LocationSituation.bodyData.Body != null;
		}
		return false;
	}

	public CelestialBody GetNodeBody()
	{
		if (LocationSituation != null && LocationSituation.bodyData.Body != null)
		{
			return LocationSituation.bodyData.Body;
		}
		return null;
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004031");
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("stabilizationTime", stabilizationTime);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("stabilizationTime", ref stabilizationTime);
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
			Debug.LogErrorFormat("[TestVesselSituation] Unable to find VesselID ({0}) for Score Modifier.", num);
			return null;
		}
		if (scoreModule == typeof(ScoreModule_Accuracy))
		{
			if (LocationSituation.locationChoice == ParamChoices_CelestialBodySurface.Choices.areaData)
			{
				return LocationSituation.areaData.PointInCircleAccuracy(vessel.latitude, vessel.longitude);
			}
			return 1.0;
		}
		return null;
	}
}
