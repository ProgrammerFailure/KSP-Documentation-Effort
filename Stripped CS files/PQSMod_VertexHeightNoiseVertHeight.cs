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

	public IModule noiseMap;

	public double hDeltaR;

	public double h;

	public double n;

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
		heightStart = 0f;
		heightEnd = 1f;
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
		hDeltaR = 1.0 / (double)(heightEnd - heightStart);
	}

	public override void OnVertexBuildHeight(GClass4.VertexBuildData data)
	{
		h = (data.vertHeight - sphere.radiusMin) / sphere.radiusDelta;
		if (h >= (double)heightStart && h <= (double)heightEnd)
		{
			h = (h - (double)heightStart) * hDeltaR;
			n = noiseMap.GetValue(data.directionFromCenter);
			if (n < -1.0)
			{
				n = -1.0;
			}
			if (n > 1.0)
			{
				n = 1.0;
			}
			data.vertHeight += (n + 1.0) * 0.5 * (double)deformity * h;
		}
	}

	public override double GetVertexMaxHeight()
	{
		return deformity;
	}

	public override double GetVertexMinHeight()
	{
		return 0.0;
	}
}
