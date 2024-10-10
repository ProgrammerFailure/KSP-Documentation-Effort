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

	public IModule noiseMap;

	public float h;

	public Color c;

	public void Reset()
	{
		blend = 1f;
		rBlend = 1f;
		gBlend = 1f;
		bBlend = 1f;
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
		c = Color.white;
	}

	public override void OnVertexBuild(GClass4.VertexBuildData data)
	{
		h = (float)((noiseMap.GetValue(data.directionFromCenter) + 1.0) * 0.5);
		c.r = h * rBlend;
		c.g = h * gBlend;
		c.b = h * bBlend;
		data.vertColor = Color.Lerp(data.vertColor, c, blend);
	}
}
