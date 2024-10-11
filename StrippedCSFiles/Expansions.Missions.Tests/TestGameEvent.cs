using System;
using System.Runtime.CompilerServices;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[] { })]
internal class TestGameEvent : TestModule, IScoreableObjective
{
	private bool eventTriggered;

	private bool eventSuscribed;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TestGameEvent()
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
	private void SuscribeToEvent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void EventCallback_StageSeparation(int eventReport)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public object GetScoreModifier(Type scoreModule)
	{
		throw null;
	}
}
