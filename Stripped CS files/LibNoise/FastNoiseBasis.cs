using System;

namespace LibNoise;

public class FastNoiseBasis : Math
{
	public int[] RandomPermutations = new int[512];

	public int[] SelectedPermutations = new int[512];

	public float[] GradientTable = new float[512];

	public int mSeed;

	public int Seed
	{
		get
		{
			return mSeed;
		}
		set
		{
			mSeed = value;
			KSPRandom kSPRandom = new KSPRandom(mSeed);
			for (int i = 0; i < 512; i++)
			{
				RandomPermutations[i] = kSPRandom.Next(255);
			}
			for (int j = 0; j < 256; j++)
			{
				SelectedPermutations[256 + j] = (SelectedPermutations[j] = RandomPermutations[j]);
			}
			float[] array = new float[256];
			for (int k = 0; k < 256; k++)
			{
				array[k] = -1f + 2f * ((float)k / 255f);
			}
			for (int l = 0; l < 256; l++)
			{
				GradientTable[l] = array[SelectedPermutations[l]];
			}
			for (int m = 256; m < 512; m++)
			{
				GradientTable[m] = GradientTable[m & 0xFF];
			}
		}
	}

	public FastNoiseBasis()
		: this(0)
	{
	}

	public FastNoiseBasis(int seed)
	{
		if (seed < 0)
		{
			throw new ArgumentException("Seed must be positive.");
		}
		Seed = seed;
	}

	public double GradientCoherentNoise(double x, double y, double z, int seed, NoiseQuality noiseQuality)
	{
		int num = ((x > 0.0) ? ((int)x) : ((int)x - 1));
		int num2 = ((y > 0.0) ? ((int)y) : ((int)y - 1));
		int num3 = ((z > 0.0) ? ((int)z) : ((int)z - 1));
		int num4 = num & 0xFF;
		int num5 = num2 & 0xFF;
		int num6 = num3 & 0xFF;
		double a = 0.0;
		double a2 = 0.0;
		double a3 = 0.0;
		switch (noiseQuality)
		{
		case NoiseQuality.Low:
			a = x - (double)num;
			a2 = y - (double)num2;
			a3 = z - (double)num3;
			break;
		case NoiseQuality.Standard:
			a = SCurve3(x - (double)num);
			a2 = SCurve3(y - (double)num2);
			a3 = SCurve3(z - (double)num3);
			break;
		case NoiseQuality.High:
			a = SCurve5(x - (double)num);
			a2 = SCurve5(y - (double)num2);
			a3 = SCurve5(z - (double)num3);
			break;
		}
		int num7 = SelectedPermutations[num4] + num5;
		int num8 = SelectedPermutations[num7] + num6;
		int num9 = SelectedPermutations[num7 + 1] + num6;
		int num10 = SelectedPermutations[num4 + 1] + num5;
		int num11 = SelectedPermutations[num10] + num6;
		int num12 = SelectedPermutations[num10 + 1] + num6;
		double n = LinearInterpolate(GradientTable[num8], GradientTable[num11], a);
		double n2 = LinearInterpolate(GradientTable[num9], GradientTable[num12], a);
		double n3 = LinearInterpolate(n, n2, a2);
		double n4 = LinearInterpolate(GradientTable[num8 + 1], GradientTable[num11 + 1], a);
		double n5 = LinearInterpolate(GradientTable[num9 + 1], GradientTable[num12 + 1], a);
		double n6 = LinearInterpolate(n4, n5, a2);
		return LinearInterpolate(n3, n6, a3);
	}
}
