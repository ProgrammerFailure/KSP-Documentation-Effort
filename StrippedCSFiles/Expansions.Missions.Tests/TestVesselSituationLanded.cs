using System;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[]
{
	typeof(ScoreModule_Resource),
	typeof(ScoreModule_Time),
	typeof(ScoreModule_Accuracy)
})]
public class TestVesselSituationLanded : TestVesselSituation, INodeWaypoint, ITestNodeLabel, INodeBody
{
	[MEGUI_NumberRange(onControlCreated = "OnSurfaceVelocityControlCreated", maxValue = 10f, roundToPlaces = 1, displayUnits = "#autoLOC_180095", minValue = 0f, resetValue = "0.1", displayFormat = "0.0", order = 100, guiName = "#autoLOC_8003020")]
	protected double surfaceVelocity;

	[MEGUI_Checkbox(onValueChange = "OnIgnoreSurfaceVelocity", order = 110, canBePinned = false, guiName = "#autoLOC_8003050", Tooltip = "#autoLOC_8003051")]
	protected bool ignoreSurfaceVelocity;

	[MEGUI_ParameterSwitchCompound(order = 10, excludeParamFields = "anyData", guiName = "#autoLOC_8000149")]
	protected ParamChoices_CelestialBodySurface locationSituation;

	public override double SurfaceVelocity
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public override bool IgnoreSurfaceVelocity
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public override ParamChoices_CelestialBodySurface LocationSituation
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TestVesselSituationLanded()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void ParameterSetupComplete()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}
}
