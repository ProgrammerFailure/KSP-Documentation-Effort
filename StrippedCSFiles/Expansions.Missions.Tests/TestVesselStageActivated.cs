using System;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[]
{
	typeof(ScoreModule_Resource),
	typeof(ScoreModule_Time)
})]
internal class TestVesselStageActivated : TestVessel, IScoreableObjective
{
	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.DecimalNumber, resetValue = "0", guiName = "#autoLOC_8000130", Tooltip = "#autoLOC_8000131")]
	public int activation;

	private bool eventFound;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TestVesselStageActivated()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool Test()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Initialized()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Cleared()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void StageActivated(int act)
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
