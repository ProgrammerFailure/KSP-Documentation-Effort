using System;
using System.Runtime.CompilerServices;
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

		private RidgedMultifractal fractal;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public RidgedNoise()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Setup()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public double Noise(Vector3d radial)
		{
			throw null;
		}
	}

	[Serializable]
	public class SimplexNoise
	{
		public int seed;

		public double frequency;

		public double persistence;

		public int octaves;

		private Simplex fractal;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public SimplexNoise()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Setup()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public double Noise(Vector3d radial)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public double NoiseNormalized(Vector3d radial)
		{
			throw null;
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

	private double hDeltaR;

	private double h;

	private double r;

	private double s;

	private double d;

	private float t;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQSMod_VertexHeightNoiseVertHeightCurve3()
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
	private static double Lerp(double a, double b, double t)
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
