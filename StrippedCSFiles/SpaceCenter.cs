using System.Runtime.CompilerServices;
using UnityEngine;

public class SpaceCenter : MonoBehaviour
{
	public static SpaceCenter Instance;

	private Transform trf;

	private double latitude;

	private double longitude;

	public double AreaRadius;

	private Vector3d srfNVector;

	public CelestialBody cb;

	public Collider spaceCenterAreaTrigger;

	public Transform SpaceCenterTransform
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

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

	public Vector3d SrfNVector
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SpaceCenter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGameSceneLoaded(GameScenes scn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GreatCircleDistance(Vector3d fromSrfNrm)
	{
		throw null;
	}
}
