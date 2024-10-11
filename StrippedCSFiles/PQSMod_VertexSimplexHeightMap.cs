using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/Height/Simplex (HeightMap)")]
public class PQSMod_VertexSimplexHeightMap : PQSMod
{
	public int seed;

	public double deformity;

	public double octaves;

	public double persistence;

	public double frequency;

	private double n;

	private Simplex simplex;

	public MapSO heightMap;

	public float heightStart;

	public float heightEnd;

	private double hDeltaR;

	private double h;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQSMod_VertexSimplexHeightMap()
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
