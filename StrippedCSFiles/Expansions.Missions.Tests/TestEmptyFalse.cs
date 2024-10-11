using System.Runtime.CompilerServices;

namespace Expansions.Missions.Tests;

public class TestEmptyFalse : TestModule
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public TestEmptyFalse()
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
