using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class OcclusionData : IComparable<OcclusionData>
{
	public Part part;

	public PartThermalData ptd;

	public double projectedArea;

	public double projectedRadius;

	public double invFineness;

	public double minimumDot;

	public double maximumDot;

	public double centroidDot;

	public double maxWidthDepth;

	public Vector3 boundsCenter;

	public Vector3[] boundsVertices;

	public Vector3 projectedCenter;

	public Vector3[] projectedVertices;

	public float[] projectedDots;

	public Vector2 center;

	public Vector2 minExtents;

	public Vector2 maxExtents;

	public Vector2 extents;

	public OcclusionCone convCone;

	public OcclusionCylinder sunCyl;

	public OcclusionCylinder bodyCyl;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public OcclusionData(PartThermalData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void CreateCornerArray()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Update(Vector3 velocity, bool useDragArea = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetConvectionMultVerts(OcclusionCone cone)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetShockStats(OcclusionCone cone, ref double newTempMult, ref double newCoeffMult)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetCylinderOcclusion(OcclusionCylinder cyl)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetCylinderOcclusionVerts(OcclusionCylinder cyl)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static double sA(double r, double x, double y)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static double RectRectIntersection(double centralExtentX, double centralExtentY, double minX, double maxX, double minY, double maxY)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static double AreaOfIntersection(double existingConeRadius, double potentialConeRadius, double sqrDistance)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int CompareTo(OcclusionData b)
	{
		throw null;
	}
}
