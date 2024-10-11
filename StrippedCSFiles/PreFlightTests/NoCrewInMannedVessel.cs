using System;
using System.Runtime.CompilerServices;

namespace PreFlightTests;

[Obsolete("because NoControlSources handles all cases this doesn't")]
public class NoCrewInMannedVessel : DesignConcernBase, IPreFlightTest
{
	private VesselCrewManifest manifest;

	private bool updateManifest;

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Obsolete("because NoControlSources handles all cases this doesn't")]
	public NoCrewInMannedVessel(VesselCrewManifest crewManifest)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Obsolete("because NoControlSources handles all cases this doesn't")]
	public NoCrewInMannedVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool TestCondition()
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetWarningTitle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetWarningDescription()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetProceedOption()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetAbortOption()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetTestName()
	{
		throw null;
	}
}
