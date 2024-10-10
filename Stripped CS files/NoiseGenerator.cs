using System;
using UnityEngine;

public class NoiseGenerator
{
	public readonly int[] int_0 = new int[3];

	public int[] int_1;

	public int i;

	public int j;

	public int k;

	public float onesixth = 1f / 6f;

	public float onethird = 1f / 3f;

	public float s;

	public float u;

	public float v;

	public float w;

	public NoiseGenerator()
	{
		if (int_1 == null)
		{
			System.Random random = new System.Random();
			int_1 = new int[8];
			for (int i = 0; i < 8; i++)
			{
				int_1[i] = random.Next();
			}
		}
	}

	public void SetSeed(int[] seed)
	{
		for (int i = 0; i < 8; i++)
		{
			int_1[i] = seed[i];
		}
	}

	public string GetSeed()
	{
		string text = "";
		for (int i = 0; i < 8; i++)
		{
			text += int_1[i];
			if (i < 7)
			{
				text += " ";
			}
		}
		return text;
	}

	public float coherentNoise(float x, float y, float z, int octaves = 1, int multiplier = 25, float amplitude = 0.5f, float lacunarity = 2f, float persistence = 0.9f)
	{
		Vector3 vector = new Vector3(x, y, z) / multiplier;
		float num = 0f;
		for (int i = 0; i < octaves; i++)
		{
			num += noise(vector.x, vector.y, vector.z) * amplitude;
			vector *= lacunarity;
			amplitude *= persistence;
		}
		return num;
	}

	public int getDensity(Vector3 loc)
	{
		float t = coherentNoise(loc.x, loc.y, loc.z);
		return (int)Mathf.Lerp(0f, 255f, t);
	}

	public float noise(float x, float y, float z)
	{
		s = (x + y + z) * onethird;
		i = fastfloor(x + s);
		j = fastfloor(y + s);
		k = fastfloor(z + s);
		s = (float)(i + j + k) * onesixth;
		u = x - (float)i + s;
		v = y - (float)j + s;
		w = z - (float)k + s;
		int_0[0] = 0;
		int_0[1] = 0;
		int_0[2] = 0;
		int num = ((!(u >= w)) ? ((v >= w) ? 1 : 2) : ((!(u >= v)) ? 1 : 0));
		int num2 = ((!(u < w)) ? ((v < w) ? 1 : 2) : ((!(u < v)) ? 1 : 0));
		return kay(num) + kay(3 - num - num2) + kay(num2) + kay(0);
	}

	public float kay(int a)
	{
		s = (float)(int_0[0] + int_0[1] + int_0[2]) * onesixth;
		float num = u - (float)int_0[0] + s;
		float num2 = v - (float)int_0[1] + s;
		float num3 = w - (float)int_0[2] + s;
		float num4 = 0.6f - num * num - num2 * num2 - num3 * num3;
		int num5 = shuffle(i + int_0[0], j + int_0[1], k + int_0[2]);
		int_0[a]++;
		if (num4 < 0f)
		{
			return 0f;
		}
		int num6 = (num5 >> 5) & 1;
		int num7 = (num5 >> 4) & 1;
		int num8 = (num5 >> 3) & 1;
		int num9 = (num5 >> 2) & 1;
		int num10 = num5 & 3;
		float num11 = num10 switch
		{
			2 => num2, 
			1 => num, 
			_ => num3, 
		};
		float num12 = num10 switch
		{
			2 => num3, 
			1 => num2, 
			_ => num, 
		};
		float num13 = num10 switch
		{
			2 => num, 
			1 => num3, 
			_ => num2, 
		};
		num11 = ((num6 == num8) ? (0f - num11) : num11);
		num12 = ((num6 == num7) ? (0f - num12) : num12);
		num13 = ((num6 != (num7 ^ num8)) ? (0f - num13) : num13);
		num4 *= num4;
		return 8f * num4 * num4 * (num11 + ((num10 == 0) ? (num12 + num13) : ((num9 == 0) ? num12 : num13)));
	}

	public int shuffle(int i, int j, int k)
	{
		return b(i, j, k, 0) + b(j, k, i, 1) + b(k, i, j, 2) + b(i, j, k, 3) + b(j, k, i, 4) + b(k, i, j, 5) + b(i, j, k, 6) + b(j, k, i, 7);
	}

	public int b(int i, int j, int k, int int_2)
	{
		return int_1[(b(i, int_2) << 2) | (b(j, int_2) << 1) | b(k, int_2)];
	}

	public int b(int int_2, int int_3)
	{
		return (int_2 >> int_3) & 1;
	}

	public int fastfloor(float n)
	{
		if (!(n > 0f))
		{
			return (int)n - 1;
		}
		return (int)n;
	}
}
