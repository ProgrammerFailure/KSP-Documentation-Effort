using System.Runtime.CompilerServices;
using UnityEngine;

public class Krakensbane : MonoBehaviour
{
	private static Krakensbane fetch;

	public double MaxV;

	public double MaxVSqr;

	public double extraAltOffsetForVel;

	public double altThreshold;

	public double altThresholdAlone;

	public Vector3d FrameVel;

	public Vector3d totalVel;

	public Vector3d RBVel;

	public Vector3d excessV;

	public Vector3d lastCorrection;

	protected int loadedVesselsCount;

	public static double SqrThreshold
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Krakensbane()
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
	protected void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SafeToEngage(out bool safeForFloatingOrigin)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddExcess(Vector3d vel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Zero()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d GetFrameVelocity()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3 GetFrameVelocityV3f()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d GetLastCorrection()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ResetVelocityFrame(bool resetObjects)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void AddFrameVelocity(Vector3d vel)
	{
		throw null;
	}
}
