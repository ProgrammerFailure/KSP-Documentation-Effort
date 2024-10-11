using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/Color/Simplex (RGB)")]
public class PQSMod_VertexSimplexColorRGB : PQSMod
{
	public int seed;

	public float blend;

	public float rBlend;

	public float gBlend;

	public float bBlend;

	public double octaves;

	public double persistence;

	public double frequency;

	private float n;

	private Color c;

	private Simplex simplex;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQSMod_VertexSimplexColorRGB()
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
