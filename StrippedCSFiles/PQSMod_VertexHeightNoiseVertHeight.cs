using System.Runtime.CompilerServices;
using LibNoise;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/Height/LibNoise (VertHeight)")]
public class PQSMod_VertexHeightNoiseVertHeight : PQSMod
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

	public float heightStart;

	public float heightEnd;

	private IModule noiseMap;

	private double hDeltaR;

	private double h;

	private double n;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQSMod_VertexHeightNoiseVertHeight()
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
