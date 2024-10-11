using System.Runtime.CompilerServices;
using LibNoise;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/Height/LibNoise")]
public class PQSMod_VertexHeightNoise : PQSMod
{
	public enum NoiseType
	{
		Perlin,
		RidgedMultifractal,
		Billow
	}

	public NoiseType noiseType;

	public float deformity;

	public int seed;

	public float frequency;

	public float lacunarity;

	public float persistance;

	public int octaves;

	public NoiseQuality mode;

	private IModule noiseMap;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQSMod_VertexHeightNoise()
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
}
