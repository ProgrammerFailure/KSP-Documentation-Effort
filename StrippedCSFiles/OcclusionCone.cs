using System.Runtime.CompilerServices;
using UnityEngine;

public class OcclusionCone
{
	public OcclusionData part;

	public Vector3 center;

	public Vector2 extents;

	public Vector2 offset;

	public double radius;

	public double cylNoseDot;

	public double shockNoseDot;

	public double shockAngle;

	public double shockConvectionTempMult;

	public double shockConvectionCoeffMult;

	public double occludeConvectionTempMult;

	public double occludeConvectionCoeffMult;

	public double occludeConvectionAreaMult;

	public static double detachedShockHeatMult;

	public static double detachedShockCoeffMult;

	public static double detachedBehindShockHeatMult;

	public static double detachedBehindShockCoeffMult;

	public static double detachedShockMachAngleMult;

	public static double detachedShockStartAngle;

	public static double detachedShockEndAngle;

	public static double obliqueShockAngleMult;

	public static double obliqueShockPartAngleMult;

	public static double obliqueShockMinAngleMult;

	public static double obliqueShockConeHeatMult;

	public static double obliqueShockConeCoeffMult;

	public static double obliqueShockCylHeatMult;

	public static double obliqueShockCylCoeffMult;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public OcclusionCone()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static OcclusionCone()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(OcclusionData part, double sqrtMach, double sqrtMachAngle, double detachAngle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetShockRadius(double dot)
	{
		throw null;
	}
}
