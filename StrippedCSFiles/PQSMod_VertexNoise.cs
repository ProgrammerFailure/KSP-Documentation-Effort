using System.Runtime.CompilerServices;
using LibNoise.Modifiers;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/Height/LibNoise (Multi)")]
public class PQSMod_VertexNoise : PQSMod
{
	public int seed;

	public float noiseDeformity;

	public int noisePasses;

	public float smoothness;

	public float falloff;

	public float mesaVsPlainsBias;

	public float plainsVsMountainSmoothness;

	public float plainsVsMountainThreshold;

	public float plainSmoothness;

	private Select terrainHeightMap;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQSMod_VertexNoise()
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
