using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace PreFlightTests;

public class LaunchSiteClear : IPreFlightTest
{
	private string siteName;

	private string siteDisplayName;

	private string obstructingVesselName;

	private int obstructingVesselIndex;

	private Game st;

	private bool stValid;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LaunchSiteClear(string SiteName, string SiteDisplayName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LaunchSiteClear(string SiteName, string SiteDisplayName, Game gameState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Test()
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
	public string GetObstructingVesselName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int GetObstructingVesselIndex()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<ProtoVessel> GetObstructingVessels()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Game GetGameState()
	{
		throw null;
	}
}
