using System;
using LibNoise;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/Height/LibNoise (HeightCurve3)")]
public class PQSMod_VertexHeightNoiseVertHeightCurve3 : PQSMod
{
	[Serializable]
	public class RidgedNoise
	{
		public int seed;

		public double frequency;

		public double lacunarity;

		public int octaves;

		public NoiseQuality quality;

		public RidgedMultifractal fractal;

		public RidgedNoise()
		{
			seed = UnityEngine.Random.Range(0, int.MaxValue);
			frequency = 8.0;
			lacunarity = 2.0;
			octaves = 4;
			quality = NoiseQuality.Low;
		}

		public void Setup()
		{
			fractal = new RidgedMultifractal(frequency, lacunarity, octaves, seed, NoiseQuality.Low);
		}

		public double Noise(Vector3d radial)
		{
			return fractal.GetValue(radial);
		}
	}

	[Serializable]
	public class SimplexNoise
	{
		public int seed;

		public double frequency;

		public double persistence;

		public int octaves;

		public Simplex fractal;

		public SimplexNoise()
		{
			seed = UnityEngine.Random.Range(0, int.MaxValue);
			frequency = 8.0;
			persistence = 0.5;
			octaves = 4;
		}

		public void Setup()
		{
			fractal = new Simplex(seed, octaves, persistence, frequency);
		}

		public double Noise(Vector3d radial)
		{
			return fractal.noise(radial);
		}

		public double NoiseNormalized(Vector3d radial)
		{
			return fractal.noiseNormalized(radial);
		}
	}

	public double inputHeightStart;

	public double inputHeightEnd;

	public AnimationCurve inputHeightCurve;

	public SimplexNoise curveMultiplier;

	public double deformityMin;

	public double deformityMax;

	public SimplexNoise deformity;

	public RidgedNoise ridgedAdd;

	public RidgedNoise ridgedSub;

	public double hDeltaR;

	public double h;

	public double r;

	public double s;

	public double d;

	public float t;

	public void Reset()
	{
		deformity = new SimplexNoise();
		curveMultiplier = new SimplexNoise();
		ridgedAdd = new RidgedNoise();
		ridgedSub = new RidgedNoise();
		inputHeightCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));
		inputHeightStart = 0.0;
		inputHeightEnd = 1000.0;
		deformityMin = 1000.0;
		deformityMax = 2000.0;
	}

	public override void OnSetup()
	{
		requirements = GClass4.ModiferRequirements.MeshCustomNormals;
		ridgedAdd.Setup();
		ridgedSub.Setup();
		curveMultiplier.Setup();
		deformity.Setup();
		hDeltaR = 1.0 / (inputHeightEnd - inputHeightStart);
	}

	public override void OnVertexBuildHeight(GClass4.VertexBuildData data)
	{
		h = data.vertHeight - sphere.radiusMin;
		if (h <= inputHeightStart)
		{
			t = 0f;
		}
		else if (h >= inputHeightEnd)
		{
			t = 1f;
		}
		else
		{
			t = (float)((h - inputHeightStart) * hDeltaR);
		}
		s = curveMultiplier.NoiseNormalized(data.directionFromCenter) * (double)inputHeightCurve.Evaluate(t);
		if (s != 0.0)
		{
			r = ridgedAdd.Noise(data.directionFromCenter) - ridgedSub.Noise(data.directionFromCenter);
			d = Lerp(deformityMin, deformityMax, deformity.NoiseNormalized(data.directionFromCenter));
			if (r < -1.0)
			{
				r = -1.0;
			}
			if (r > 1.0)
			{
				r = 1.0;
			}
			r = (r + 1.0) * 0.5;
			data.vertHeight += r * d * s;
		}
	}

	public static double Lerp(double a, double b, double t)
	{
		return a * t + b * (1.0 - t);
	}

	public override double GetVertexMaxHeight()
	{
		return deformityMax;
	}

	public override double GetVertexMinHeight()
	{
		return 0.0;
	}
}
