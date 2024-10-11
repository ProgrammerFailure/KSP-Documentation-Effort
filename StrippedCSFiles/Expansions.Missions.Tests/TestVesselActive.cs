using System;
using System.Runtime.CompilerServices;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[] { })]
internal class TestVesselActive : TestVessel
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public TestVesselActive()
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
	public override string GetInfo()
	{
		throw null;
	}
}
