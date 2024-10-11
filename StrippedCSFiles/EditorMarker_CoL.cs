using System.Runtime.CompilerServices;
using UnityEngine;

public class EditorMarker_CoL : EditorMarker
{
	public Vector3 referenceVelocity;

	public float referencePitch;

	public float referenceSpeed;

	public double refAlt;

	public double refStP;

	public double refDens;

	private Ray CoL;

	private double refTemp;

	private static CenterOfLiftQuery lQry;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public EditorMarker_CoL()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static EditorMarker_CoL()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override Vector3 UpdatePosition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Ray FindCoL(Part rootPart, Vector3 refVel, double refAlt, double refStp, double refDens)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Ray FindCoL(Part rootPart, Vector3 refVel, double refAlt, double refStp, double refDens, out bool noLift)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Ray FindCoL(Vector3 refVel, double refAlt, double refStp, double refDens)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void recurseParts(Part part, Vector3 refVel, ref Vector3 CoL, ref Vector3 DoL, ref float t, double refAlt, double refStp, double refDens)
	{
		throw null;
	}
}
