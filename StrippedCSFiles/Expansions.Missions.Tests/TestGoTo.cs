using System;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;
using FinePrint;
using UnityEngine;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[]
{
	typeof(ScoreModule_Resource),
	typeof(ScoreModule_Time),
	typeof(ScoreModule_Accuracy)
})]
internal class TestGoTo : TestVessel, IScoreableObjective, INodeWaypoint, ITestNodeLabel
{
	[MEGUI_SurfaceArea(order = 10, gapDisplay = true, guiName = "#autoLOC_8000264", Tooltip = "#autoLOC_8000147")]
	public SurfaceArea areaData;

	[MEGUI_NumberRange(onControlCreated = "OnSurfaceVelocityControlCreated", maxValue = 10f, roundToPlaces = 1, displayUnits = "#autoLOC_180095", minValue = 0f, resetValue = "0.1", displayFormat = "0.0", order = 30, guiName = "#autoLOC_8003020")]
	protected double surfaceVelocity;

	[MEGUI_Checkbox(onValueChange = "OnIgnoreSurfaceVelocity", order = 20, canBePinned = false, guiName = "#autoLOC_8003050", Tooltip = "#autoLOC_8003051")]
	protected bool ignoreSurfaceVelocity;

	protected MEGUIParameterNumberRange surfaceVelocityRange;

	protected bool speedSuccess;

	private bool passed;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TestGoTo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasNodeWaypoint()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Waypoint GetNodeWaypoint()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool Test()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void OnSurfaceVelocityControlCreated(MEGUIParameterNumberRange parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void OnSurfaceVelocitySetState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void OnIgnoreSurfaceVelocity(bool value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasNodeLabel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasWorldPosition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetWorldPosition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetExtraText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
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
	public object GetScoreModifier(Type scoreModule)
	{
		throw null;
	}
}
