using System;
using System.Runtime.InteropServices;
using UnityEngine;

[ComVisible(true)]
public class KSPRandom : System.Random
{
	public int[] seedArray = new int[56];

	public const int MBIG = int.MaxValue;

	public const int MSEED = 161803398;

	public const int int_0 = 0;

	public int inexta;

	public int inextb;

	public KSPRandom()
		: this(Environment.TickCount)
	{
	}

	public KSPRandom(int Seed)
	{
		int num = 161803398 - Math.Abs(Seed);
		seedArray[55] = num;
		int num2 = 1;
		for (int i = 1; i < 55; i++)
		{
			int num3 = 21 * i % 55;
			seedArray[num3] = num2;
			num2 = num - num2;
			if (num2 < 0)
			{
				num2 += int.MaxValue;
			}
			num = seedArray[num3];
		}
		for (int j = 1; j < 5; j++)
		{
			for (int k = 1; k < 56; k++)
			{
				seedArray[k] -= seedArray[1 + (k + 30) % 55];
				if (seedArray[k] < 0)
				{
					seedArray[k] += int.MaxValue;
				}
			}
		}
		inexta = 0;
		inextb = 31;
	}

	public new virtual double Sample()
	{
		if (++inexta >= 56)
		{
			inexta = 1;
		}
		if (++inextb >= 56)
		{
			inextb = 1;
		}
		int num = seedArray[inexta] - seedArray[inextb];
		if (num < 0)
		{
			num += int.MaxValue;
		}
		seedArray[inexta] = num;
		return (double)num * 4.6566128752458E-10;
	}

	public new virtual int Next()
	{
		return (int)(Sample() * 2147483647.0);
	}

	public new virtual int Next(int maxValue)
	{
		if (maxValue < 0)
		{
			throw new ArgumentOutOfRangeException("Max value is less than min value.");
		}
		return (int)(Sample() * (double)maxValue);
	}

	public new virtual int Next(int minValue, int maxValue)
	{
		if (minValue > maxValue)
		{
			throw new ArgumentOutOfRangeException("Min value is greater than max value.");
		}
		uint num = (uint)(maxValue - minValue);
		if (num <= 1)
		{
			return minValue;
		}
		return (int)((uint)(Sample() * (double)num) + minValue);
	}

	public new virtual void NextBytes(byte[] buffer)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		for (int i = 0; i < buffer.Length; i++)
		{
			buffer[i] = (byte)(Sample() * 256.0);
		}
	}

	public new virtual double NextDouble()
	{
		return Sample();
	}

	public virtual double NextDouble(double minValue, double maxValue)
	{
		if (minValue > maxValue)
		{
			Debug.LogError("[KSPRandom]: Min value is greater than max value.");
			return 0.0;
		}
		double num = maxValue - minValue;
		if (num <= 1.0)
		{
			return minValue;
		}
		return Sample() * num + minValue;
	}
}
