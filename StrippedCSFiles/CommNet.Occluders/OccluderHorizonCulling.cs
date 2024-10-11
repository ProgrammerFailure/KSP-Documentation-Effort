using System.Runtime.CompilerServices;
using UnityEngine;

namespace CommNet.Occluders;

public class OccluderHorizonCulling : Occluder
{
	protected Transform transform;

	protected CelestialBody body;

	protected bool useBody;

	protected double radiusXRecip;

	protected double radiusYRecip;

	protected double radiusZRecip;

	private bool anyZero;

	protected QuaternionD invRotation;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public OccluderHorizonCulling(Transform transform, double radiusX, double radiusY, double radiusZ)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool InRange(Vector3d source, double distance)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool Raycast(Vector3d source, Vector3d dest)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Update()
	{
		throw null;
	}
}
