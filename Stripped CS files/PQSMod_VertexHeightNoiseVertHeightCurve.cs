using LibNoise;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/Height/LibNoise (HeightCurve)")]
public class PQSMod_VertexHeightNoiseVertHeightCurve : PQSMod
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

	public AnimationCurve curve;

	public float heightStart;

	public float heightEnd;

	public IModule noiseMap;

	public double h;

	public double n;

	public float t;

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
		curve = new AnimationCurve();
		heightStart = 0f;
		heightEnd = 1f;
	}

	public override void OnSetup()
	{
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
		h = data.vertHeight - sphere.radiusMin;
		if (h <= (double)heightStart)
		{
			t = 0f;
		}
		else if (h >= (double)heightEnd)
		{
			t = 1f;
		}
		else
		{
			t = (float)((h - (double)heightStart) / (double)(heightEnd - heightStart));
		}
		n = noiseMap.GetValue(data.directionFromCenter);
		if (n < -1.0)
		{
			n = -1.0;
		}
		if (n > 1.0)
		{
			n = 1.0;
		}
		data.vertHeight += (n + 1.0) * 0.5 * (double)deformity * (double)curve.Evaluate(t);
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
