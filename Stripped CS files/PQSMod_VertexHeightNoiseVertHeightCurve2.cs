using LibNoise;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/Height/LibNoise (HeightCurve2)")]
public class PQSMod_VertexHeightNoiseVertHeightCurve2 : PQSMod
{
	public float deformity;

	public int ridgedAddSeed;

	public float ridgedAddFrequency;

	public float ridgedAddLacunarity;

	public int ridgedAddOctaves;

	public int ridgedSubSeed;

	public float ridgedSubFrequency;

	public float ridgedSubLacunarity;

	public int ridgedSubOctaves;

	public NoiseQuality ridgedMode;

	public AnimationCurve simplexCurve;

	public double simplexHeightStart;

	public double simplexHeightEnd;

	public int simplexSeed;

	public double simplexOctaves;

	public double simplexPersistence;

	public double simplexFrequency;

	public Simplex simplex;

	public RidgedMultifractal ridgedAdd;

	public RidgedMultifractal ridgedSub;

	public double hDeltaR;

	public double h;

	public double r;

	public double s;

	public float t;

	public void Reset()
	{
		deformity = 100f;
		ridgedAddSeed = Random.Range(0, int.MaxValue);
		ridgedAddFrequency = 1f;
		ridgedAddLacunarity = 0.5f;
		ridgedAddOctaves = 1;
		ridgedSubSeed = Random.Range(0, int.MaxValue);
		ridgedSubFrequency = 1f;
		ridgedSubLacunarity = 0.5f;
		ridgedSubOctaves = 1;
		ridgedMode = NoiseQuality.Low;
		simplexCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));
		simplexHeightStart = 0.0;
		simplexHeightEnd = 1000.0;
		simplexSeed = Random.Range(0, int.MaxValue);
		simplexOctaves = 4.0;
		simplexPersistence = 0.5;
		simplexFrequency = 8.0;
	}

	public override void OnSetup()
	{
		requirements = GClass4.ModiferRequirements.MeshCustomNormals;
		ridgedAdd = new RidgedMultifractal(ridgedAddFrequency, ridgedAddLacunarity, ridgedAddOctaves, ridgedAddSeed, ridgedMode);
		ridgedSub = new RidgedMultifractal(ridgedSubFrequency, ridgedSubLacunarity, ridgedSubOctaves, ridgedSubSeed, ridgedMode);
		simplex = new Simplex(simplexSeed, simplexOctaves, simplexPersistence, simplexFrequency);
		hDeltaR = 1.0 / (simplexHeightEnd - simplexHeightStart);
	}

	public override void OnVertexBuildHeight(GClass4.VertexBuildData data)
	{
		h = data.vertHeight - sphere.radiusMin;
		if (h <= simplexHeightStart)
		{
			t = 0f;
		}
		else if (h >= simplexHeightEnd)
		{
			t = 1f;
		}
		else
		{
			t = (float)((h - simplexHeightStart) * hDeltaR);
		}
		s = simplex.noiseNormalized(data.directionFromCenter) * (double)simplexCurve.Evaluate(t);
		if (s != 0.0)
		{
			r = ridgedAdd.GetValue(data.directionFromCenter) - ridgedSub.GetValue(data.directionFromCenter);
			if (r < -1.0)
			{
				r = -1.0;
			}
			if (r > 1.0)
			{
				r = 1.0;
			}
			data.vertHeight += (r + 1.0) * 0.5 * (double)deformity * s;
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
