using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace PreFlightTests;

public class ResourceConsumersReachable : DesignConcernBase
{
	private PartResourceDefinition resourceDefinition;

	private ShipConstruct ship;

	private List<Part> failedParts;

	private bool exceptionCommandPod;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ResourceConsumersReachable(PartResourceDefinition resourceDefinition)
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
	public override string GetConcernTitle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetConcernDescription()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override DesignConcernSeverity GetSeverity()
	{
		throw null;
	}
}
