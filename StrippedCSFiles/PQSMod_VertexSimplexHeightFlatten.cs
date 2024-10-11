using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/Height/Flatten Simplex")]
public class PQSMod_VertexSimplexHeightFlatten : PQSMod
{
	public int seed;

	public double cutoff;

	public double deformity;

	public double octaves;

	public double persistence;

	public double frequency;

	private double n;

	private Simplex simplex;

	private double val;

	private double valRatio;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQSMod_VertexSimplexHeightFlatten()
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
	public override void OnVertexBuildHeight(PQS.VertexBuildData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override double GetVertexMaxHeight()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override double GetVertexMinHeight()
	{
		throw null;
	}
}
