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

	public IModule noiseMap;

	public void Reset()
	{
		deformity = 100f;
		noiseType = NoiseType.Perlin;
		seed = Random.Range(0, int.MaxValue);
		frequency = 1f;
		lacunarity = 0.5f;
		persistance = 0.5f;
		octaves = 1;
		mode = NoiseQuality.Low;
	}

	public override void OnSetup()
	{
		requirements = GClass4.ModiferRequirements.MeshColorChannel;
		switch (noiseType)
		{
		default:
			noiseMap = new Perlin(frequency, lacunarity, persistance, octaves, seed, mode);
			break;
		case NoiseType.RidgedMultifractal:
			noiseMap = new RidgedMultifractal(frequency, lacunarity, octaves, seed, mode);
			break;
		case NoiseType.Billow:
			noiseMap = new Billow(frequency, lacunarity, persistance, octaves, seed, mode);
			break;
		}
	}

	public override void OnVertexBuildHeight(GClass4.VertexBuildData data)
	{
		data.vertHeight += noiseMap.GetValue(data.directionFromCenter) * (double)deformity;
	}

	public override double GetVertexMaxHeight()
	{
		return deformity;
	}
}
