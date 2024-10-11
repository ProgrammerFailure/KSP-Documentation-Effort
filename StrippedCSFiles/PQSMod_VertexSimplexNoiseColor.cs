using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/Color/Simplex")]
public class PQSMod_VertexSimplexNoiseColor : PQSMod
{
	public int seed;

	public float blend;

	public Color colorStart;

	public Color colorEnd;

	public double octaves;

	public double persistence;

	public double frequency;

	private float n;

	private Simplex simplex;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQSMod_VertexSimplexNoiseColor()
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
	public override void OnVertexBuild(PQS.VertexBuildData data)
	{
		throw null;
	}
}
