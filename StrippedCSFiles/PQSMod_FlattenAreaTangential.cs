using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Terrain/Flatten Area Tangential")]
public class PQSMod_FlattenAreaTangential : PQSMod
{
	public double outerRadius;

	public double innerRadius;

	public Vector3 position;

	public double flattenTo;

	public double smoothStart;

	public double smoothEnd;

	public bool DEBUG_showColors;

	private double angleInner;

	private double angleOuter;

	private double angleQuadInclusion;

	private double angleDelta;

	private bool quadActive;

	private Vector3d posNorm;

	private double testAngle;

	private double aDelta;

	private double flattenToRadius;

	private double vHeight;

	private double ct2;

	private double ct3;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQSMod_FlattenAreaTangential()
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
	public double CubicHermite(double start, double end, double startTangent, double endTangent, double t)
	{
		throw null;
	}
}
