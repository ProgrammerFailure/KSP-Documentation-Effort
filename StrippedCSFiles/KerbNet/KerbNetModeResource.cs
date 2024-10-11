using System.Runtime.CompilerServices;
using UnityEngine;

namespace KerbNet;

internal class KerbNetModeResource : KerbNetMode
{
	public MapDisplayTypes displayMode;

	private PartResourceDefinition partResource;

	private Color seedColor;

	private Color modeButtonColor;

	private Color modeTextColor;

	private Color medianColor;

	private Color startColor;

	private Color endColor;

	private float lowerBounds;

	private float upperBounds;

	private float boundsRange;

	private double vesselAltitude;

	private CelestialBody body;

	private PlanetaryResource planetResource;

	public bool scannedBody;

	private static string cacheAutoLOC_438998;

	private static string cacheAutoLOC_439000;

	private static string cacheAutoLOC_439019;

	private static string cacheAutoLOC_258912;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KerbNetModeResource()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KerbNetModeResource(string resource)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnInit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnActivated()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnDeactivated()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetErrorState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OrbitalSurveyCompleted(Vessel v, CelestialBody cb)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void VesselSOIChange(GameEvents.HostedFromToAction<Vessel, CelestialBody> fromTo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CheckScannedBody()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool AutoGenerateMode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override Color GetModeColorTint()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetModeCaption()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnColorClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnPrecache(Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float GetAbundance(double latitude = 0.0, double longitude = 0.0)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override Color GetCoordinateColor(Vessel vessel, double currentLatitude, double currentLongitude)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string LocalCoordinateInfo(Vessel vessel, double centerLatitude, double centerLongitude, double waypointLatitude, double waypointLongitude, bool waypointInSpace)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}
}
