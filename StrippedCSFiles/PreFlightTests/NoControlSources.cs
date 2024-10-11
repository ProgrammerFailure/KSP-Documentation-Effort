using System.Runtime.CompilerServices;

namespace PreFlightTests;

public class NoControlSources : DesignConcernBase, IPreFlightTest
{
	private VesselCrewManifest manifest;

	private bool updateManifest;

	private KerbalRoster roster;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public NoControlSources(VesselCrewManifest crewManifest)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public NoControlSources(VesselCrewManifest crewManifest, KerbalRoster roster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public NoControlSources()
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
