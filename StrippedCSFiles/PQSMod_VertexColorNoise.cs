using System.Runtime.CompilerServices;
using LibNoise;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/Color/LibNoise (Grey)")]
public class PQSMod_VertexColorNoise : PQSMod
{
	public enum NoiseType
	{
		Perlin,
		RidgedMultifractal,
		Billow
	}

	public NoiseType noiseType;

	public float blend;

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
	public PQSMod_VertexColorNoise()
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
