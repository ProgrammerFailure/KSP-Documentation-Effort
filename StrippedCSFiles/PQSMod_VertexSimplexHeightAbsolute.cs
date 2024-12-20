using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/Height/Simplex (Absolute)")]
public class PQSMod_VertexSimplexHeightAbsolute : PQSMod
{
	public int seed;

	public double deformity;

	public double octaves;

	public double persistence;

	public double frequency;

	private double n;

	private Simplex simplex;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQSMod_VertexSimplexHeightAbsolute()
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
