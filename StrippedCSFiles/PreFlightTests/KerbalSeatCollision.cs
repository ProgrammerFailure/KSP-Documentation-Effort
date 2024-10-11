using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace PreFlightTests;

public class KerbalSeatCollision : DesignConcernBase
{
	private List<Part> failedParts;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KerbalSeatCollision()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool TestCondition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override List<Part> GetAffectedParts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override DesignConcernSeverity GetSeverity()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetConcernTitle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetConcernDescription()
	{
		throw null;
	}
}
