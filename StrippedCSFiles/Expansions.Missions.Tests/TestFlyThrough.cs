using System;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;
using FinePrint;
using UnityEngine;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[]
{
	typeof(ScoreModule_Resource),
	typeof(ScoreModule_Time)
})]
internal class TestFlyThrough : TestVessel, IScoreableObjective, INodeWaypoint, ITestNodeLabel
{
	[MEGUI_SurfaceVolume(order = 10, gapDisplay = true, guiName = "#autoLOC_8000146", Tooltip = "#autoLOC_8000147")]
	public SurfaceVolume volumeData;

	[MEGUI_Checkbox(onValueChange = "OnShowLabelChanged", order = 20, guiName = "#autoLOC_8003073", Tooltip = "#autoLOC_8003074")]
	public bool showNodeLabel;

	[MEGUI_Checkbox(order = 21, guiName = "#autoLOC_8003394", Tooltip = "#autoLOC_8003395")]
	public bool showMapNode;

	private bool passed;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TestFlyThrough()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnShowLabelChanged(bool value)
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
	public override string GetNodeBodyParameterString(BaseAPField field)
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
