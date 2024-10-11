using System;
using System.Runtime.CompilerServices;
using Contracts;
using KSP.UI.Screens.Mapview;
using UnityEngine;

namespace FinePrint;

public class Waypoint
{
	public int seed;

	public int index;

	public string id;

	public string name;

	public string celestialName;

	public double latitude;

	public double longitude;

	public double altitude;

	public double height;

	public double radius;

	public bool isOccluded;

	public bool isExplored;

	public bool isClustered;

	public bool isOnSurface;

	public bool isNavigatable;

	public bool isCustom;

	public bool visible;

	public bool landLocked;

	public bool enableMarker;

	public bool enableTooltip;

	public bool blocksInput;

	public bool isMission;

	public Contract contractReference;

	public Vector3d worldPosition;

	public Vector3 orbitPosition;

	private Vector3d surfacePosition;

	private Vector3d cameraPosition;

	private CelestialBody focusBody;

	private CelestialBody cachedBody;

	public int iconSize;

	public const float minimumAlpha = 0.6f;

	public const float maximumAlpha = 1f;

	public float alphaRatio;

	public float iconAlpha;

	public double fadeRange;

	public bool withinZoom;

	private bool mapOpen;

	public MapNode node;

	private MapContextMenu menu;

	public string nodeCaption1;

	public string nodeCaption2;

	public string nodeCaption3;

	public Guid navigationId;

	public CelestialBody celestialBody
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int uniqueSeed
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string FullName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Waypoint()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Waypoint(Waypoint that)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void setName(bool uniqueSites = true, bool allowNamed = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetFadeRange(double referenceAltitude = -1.0)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RandomizeNear(double centerLatitude, double centerLongitude, double minimumDistance, double maximumDistance, bool waterAllowed = true, System.Random generator = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RandomizeNear(double centerLatitude, double centerLongitude, double maximumDistance, bool waterAllowed = true, System.Random generator = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RandomizeAwayFrom(double originLatitude, double originLongitude, double minimumDistance, double maximumDistance, int samples = 3, bool waterAllowed = true, System.Random generator = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RandomizeAwayFrom(double originLatitude, double originLongitude, double maximumDistance, int samples = 3, bool waterAllowed = true, System.Random generator = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetupMapNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CleanupMapNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateWaypoint(bool mapOpen, bool clicked, CelestialBody focusBody, Vector3d cameraPosition)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CachePositions()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CheckOcclusion()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CheckZoom()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CheckAlpha()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUpdateNodeIcon(MapNode n, MapNode.IconData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector3d OnUpdateNodePosition(MapNode n)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUpdateNodeCaption(MapNode n, MapNode.CaptionData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ProcessClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnMenuDismissed()
	{
		throw null;
	}
}
