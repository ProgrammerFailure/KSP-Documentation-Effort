using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Planetarium : MonoBehaviour
{
	public struct CelestialFrame
	{
		public Vector3d X;

		public Vector3d Y;

		public Vector3d Z;

		public QuaternionD Rotation
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Vector3d WorldToLocal(Vector3d r)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Vector3d LocalToWorld(Vector3d r)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void SetFrame(double A, double B, double C, ref CelestialFrame cf)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void OrbitalFrame(double LAN, double Inc, double ArgPe, ref CelestialFrame cf)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void PlanetaryFrame(double ra, double dec, double rot, ref CelestialFrame cf)
		{
			throw null;
		}
	}

	public List<OrbitDriver> orbits;

	public double time;

	public double timeScale;

	public bool pause;

	public static Planetarium fetch;

	public QuaternionD rotation;

	public double inverseRotAngle;

	public static CelestialFrame Zup;

	public CelestialBody Sun;

	public CelestialBody Home;

	public CelestialBody CurrentMainBody;

	public double fixedDeltaTime;

	public static List<OrbitDriver> Orbits
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static double TimeScale
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

	public static bool Pause
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static QuaternionD Rotation
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

	public static double InverseRotAngle
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

	public static Vector3d up
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Vector3d forward
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Vector3d right
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Planetarium()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ZupAtT(double UT, CelestialBody body, ref CelestialFrame tempZup)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d SphericalVector(double lat, double lon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateCBs()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private CelestialBody FindRootBody(CelestialBody cb)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateCBsRecursive(CelestialBody cb)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double GetUniversalTime()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetUniversalTime(double t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool FrameIsRotating()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool findRotatingBodiesRecursive(CelestialBody cb)
	{
		throw null;
	}
}
