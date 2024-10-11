using System.Runtime.CompilerServices;
using LibNoise;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/Color/LibNoise (RGB)")]
public class PQSMod_VertexColorNoiseRGB : PQSMod
{
	public enum NoiseType
	{
		Perlin,
		RidgedMultifractal,
		Billow
	}

	public NoiseType noiseType;

	public float blend;

	public float rBlend;

	public float gBlend;

	public float bBlend;

	public int seed;

	public float frequency;

	public float lacunarity;

	public float persistance;

	public int octaves;

	public NoiseQuality mode;

	private IModule noiseMap;

	private float h;

	private Color c;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQSMod_VertexColorNoiseRGB()
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
