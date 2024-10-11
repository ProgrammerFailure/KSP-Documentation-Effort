using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Terrain/Flatten Area")]
public class PQSMod_FlattenArea : PQSMod
{
	public double outerRadius;

	public double innerRadius;

	public Vector3 position;

	public double flattenTo;

	public bool removeScatter;

	public bool DEBUG_showColors;

	private double flattenToRadius;

	private double angleInner;

	private double angleOuter;

	private double angleQuadInclusion;

	private double angleDelta;

	private bool quadActive;

	private Vector3d posNorm;

	private double testAngle;

	private double aDelta;

	public double smoothStart;

	public double smoothEnd;

	public bool useLatLon;

	public string bodyName;

	public CelestialBody body;

	public double lat;

	public double lon;

	public double alt;

	private double ct2;

	private double ct3;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQSMod_FlattenArea()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Reset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnQuadPreBuild(PQ quad)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnQuadBuilt(PQ quad)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnVertexBuildHeight(PQS.VertexBuildData vbData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double Lerp(double v2, double v1, double dt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double CubicHermite(double start, double end, double startTangent, double endTangent, double t)
	{
		throw null;
	}
}
