using System;
using Expansions.Missions.Editor;
using ns9;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[]
{
	typeof(ScoreModule_Resource),
	typeof(ScoreModule_Time),
	typeof(ScoreModule_Accuracy)
})]
public class TestVesselSituationSplashed : TestVesselSituation, INodeWaypoint, ITestNodeLabel, INodeBody
{
	[MEGUI_NumberRange(onControlCreated = "OnSurfaceVelocityControlCreated", maxValue = 10f, roundToPlaces = 1, displayUnits = "#autoLOC_180095", minValue = 0f, resetValue = "0.1", displayFormat = "0.0", order = 100, hideOnSetup = true, guiName = "#autoLOC_8003020")]
	public double surfaceVelocity = 0.10000000149011612;

	[MEGUI_Checkbox(onValueChange = "OnIgnoreSurfaceVelocity", order = 110, canBePinned = false, guiName = "#autoLOC_8003050", Tooltip = "#autoLOC_8003051")]
	public bool ignoreSurfaceVelocity = true;

	[MEGUI_ParameterSwitchCompound(order = 10, excludeParamFields = "biomeData,anyData,launchsiteName", guiName = "#autoLOC_8000149")]
	public ParamChoices_CelestialBodySurface locationSituation;

	public override double SurfaceVelocity => surfaceVelocity;

	public override bool IgnoreSurfaceVelocity => ignoreSurfaceVelocity;

	public override ParamChoices_CelestialBodySurface LocationSituation => locationSituation;

	public override void Awake()
	{
		base.Awake();
		situation = Vessel.Situations.SPLASHED;
		title = Localizer.Format("#autoLOC_8003052");
		locationSituation = new ParamChoices_CelestialBodySurface();
		locationSituation.bodyData = new MissionCelestialBody(null);
		locationSituation.biomeData = new MissionBiome(FlightGlobals.GetHomeBody(), "");
		locationSituation.areaData = new SurfaceArea(FlightGlobals.GetHomeBody(), 0.0, 0.0, 150000f);
		locationSituation.launchSiteName = "";
	}

	public override void ParameterSetupComplete()
	{
		base.ParameterSetupComplete();
		OnIgnoreSurfaceVelocity(ignoreSurfaceVelocity);
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		LocationSituation.Save(node);
		node.AddValue("surfaceVelocity", surfaceVelocity);
		node.AddValue("ignoreSurfaceVelocity", ignoreSurfaceVelocity);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		LocationSituation.Load(node);
		node.TryGetValue("surfaceVelocity", ref surfaceVelocity);
		node.TryGetValue("ignoreSurfaceVelocity", ref ignoreSurfaceVelocity);
	}
}
