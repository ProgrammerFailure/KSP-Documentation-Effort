using System.Runtime.CompilerServices;
using UnityEngine;

namespace PreFlightTests;

public class CraftWithinSizeLimits : IPreFlightTest
{
	private Vector3 craftSize;

	private Vector3 maxSizeAllowed;

	private string facilityName;

	private string shipName;

	private SpaceCenterFacility facility;

	private LaunchSite launchsite;

	private bool isSCFacility;

	private static string cacheAutoLOC_250789;

	private static string cacheAutoLOC_250834;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CraftWithinSizeLimits(ShipConstruct ship, SpaceCenterFacility facility, Vector3 maxSize)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CraftWithinSizeLimits(ShipTemplate template, SpaceCenterFacility facility, Vector3 maxSize)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CraftWithinSizeLimits(Vector3 shipSize, string shipName, SpaceCenterFacility facility, Vector3 maxSize)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CraftWithinSizeLimits(ShipConstruct ship, LaunchSite facility, Vector3 maxSize)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CraftWithinSizeLimits(ShipTemplate template, LaunchSite facility, Vector3 maxSize)
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
	internal static void CacheLocalStrings()
	{
		throw null;
	}
}
