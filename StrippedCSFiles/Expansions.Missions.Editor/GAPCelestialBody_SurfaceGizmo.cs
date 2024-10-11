using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Missions.Editor;

public class GAPCelestialBody_SurfaceGizmo : MonoBehaviour
{
	public Transform gizmoContainer;

	protected CelestialBody celestialBody;

	protected double longitude;

	protected double latitude;

	protected double altitude;

	protected GAPCelestialBody gapRef;

	public double Latitude
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double Longitude
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double Altitude
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GAPCelestialBody_SurfaceGizmo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Initialize(GAPCelestialBody newGapRef)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnLoadPlanet(CelestialBody newCelestialBody)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool IsActive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double SetGizmoPosition(double newLatitude, double newLongitude, double newAltitude)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetInSurfaceFromPoint(Vector3d point)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetInSurfaceFromPointAtSeaLevel(Vector3d point, double seaLevel)
	{
		throw null;
	}
}
