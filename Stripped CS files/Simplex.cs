using UnityEngine;

public class Simplex
{
	public static int[][] grad3 = new int[12][]
	{
		new int[3] { 1, 1, 0 },
		new int[3] { -1, 1, 0 },
		new int[3] { 1, -1, 0 },
		new int[3] { -1, -1, 0 },
		new int[3] { 1, 0, 1 },
		new int[3] { -1, 0, 1 },
		new int[3] { 1, 0, -1 },
		new int[3] { -1, 0, -1 },
		new int[3] { 0, 1, 1 },
		new int[3] { 0, -1, 1 },
		new int[3] { 0, 1, -1 },
		new int[3] { 0, -1, -1 }
	};

	public static int[] p = new int[256]
	{
		151, 160, 137, 91, 90, 15, 131, 13, 201, 95,
		96, 53, 194, 233, 7, 225, 140, 36, 103, 30,
		69, 142, 8, 99, 37, 240, 21, 10, 23, 190,
		6, 148, 247, 120, 234, 75, 0, 26, 197, 62,
		94, 252, 219, 203, 117, 35, 11, 32, 57, 177,
		33, 88, 237, 149, 56, 87, 174, 20, 125, 136,
		171, 168, 68, 175, 74, 165, 71, 134, 139, 48,
		27, 166, 77, 146, 158, 231, 83, 111, 229, 122,
		60, 211, 133, 230, 220, 105, 92, 41, 55, 46,
		245, 40, 244, 102, 143, 54, 65, 25, 63, 161,
		1, 216, 80, 73, 209, 76, 132, 187, 208, 89,
		18, 169, 200, 196, 135, 130, 116, 188, 159, 86,
		164, 100, 109, 198, 173, 186, 3, 64, 52, 217,
		226, 250, 124, 123, 5, 202, 38, 147, 118, 126,
		255, 82, 85, 212, 207, 206, 59, 227, 47, 16,
		58, 17, 182, 189, 28, 42, 223, 183, 170, 213,
		119, 248, 152, 2, 44, 154, 163, 70, 221, 153,
		101, 155, 167, 43, 172, 9, 129, 22, 39, 253,
		19, 98, 108, 110, 79, 113, 224, 232, 178, 185,
		112, 104, 218, 246, 97, 228, 251, 34, 242, 193,
		238, 210, 144, 12, 191, 179, 162, 241, 81, 51,
		145, 235, 249, 14, 239, 107, 49, 192, 214, 31,
		181, 199, 106, 157, 184, 84, 204, 176, 115, 121,
		50, 45, 127, 4, 150, 254, 138, 236, 205, 93,
		222, 114, 67, 29, 24, 72, 243, 141, 128, 195,
		78, 66, 215, 61, 156, 180
	};

	public int[] perm;

	public double n0;

	public double n1;

	public double n2;

	public double n3;

	public double F3;

	public double s;

	public int i;

	public int j;

	public int k;

	public double G3;

	public double t;

	public double X0;

	public double Y0;

	public double Z0;

	public double x0;

	public double y0;

	public double z0;

	public int i1;

	public int j1;

	public int k1;

	public int i2;

	public int j2;

	public int k2;

	public double x1;

	public double y1;

	public double z1;

	public double x2;

	public double y2;

	public double z2;

	public double x3;

	public double y3;

	public double z3;

	public int ii;

	public int jj;

	public int kk;

	public int gi0;

	public int gi1;

	public int gi2;

	public int gi3;

	public double t0;

	public double t1;

	public double t2;

	public double t3;

	public double itr;

	public double total;

	public double amplitude;

	public double maxAmplitude;

	public double f;

	public double octaves;

	public double persistence;

	public double frequency;

	public int seed
	{
		set
		{
			Random.InitState(value);
			for (int i = 0; i < 255; i++)
			{
				p[i] = Random.Range(0, 255);
			}
			SetupPermTable();
		}
	}

	public Simplex()
	{
		SetupPermTable();
	}

	public Simplex(int seed)
	{
		this.seed = seed;
	}

	public Simplex(int seed, double octaves, double persistence, double frequency)
	{
		this.seed = seed;
		this.octaves = octaves;
		this.persistence = persistence;
		this.frequency = frequency;
	}

	public void SetupPermTable()
	{
		perm = new int[512];
		for (int i = 0; i < 512; i++)
		{
			perm[i] = p[i & 0xFF];
		}
	}

	public static int fastfloor(double x)
	{
		if (!(x > 0.0))
		{
			return (int)x - 1;
		}
		return (int)x;
	}

	public static double dot(int[] g, double x, double y)
	{
		return (double)g[0] * x + (double)g[1] * y;
	}

	public static double dot(int[] g, double x, double y, double z)
	{
		return (double)g[0] * x + (double)g[1] * y + (double)g[2] * z;
	}

	public static double dot(int[] g, double x, double y, double z, double w)
	{
		return (double)g[0] * x + (double)g[1] * y + (double)g[2] * z + (double)g[3] * w;
	}

	public double value(double xin, double yin, double zin)
	{
		F3 = 1.0 / 3.0;
		s = (xin + yin + zin) * F3;
		i = fastfloor(xin + s);
		j = fastfloor(yin + s);
		k = fastfloor(zin + s);
		G3 = 1.0 / 6.0;
		t = (double)(i + j + k) * G3;
		X0 = (double)i - t;
		Y0 = (double)j - t;
		Z0 = (double)k - t;
		x0 = xin - X0;
		y0 = yin - Y0;
		z0 = zin - Z0;
		if (x0 >= y0)
		{
			if (y0 >= z0)
			{
				i1 = 1;
				j1 = 0;
				k1 = 0;
				i2 = 1;
				j2 = 1;
				k2 = 0;
			}
			else if (x0 >= z0)
			{
				i1 = 1;
				j1 = 0;
				k1 = 0;
				i2 = 1;
				j2 = 0;
				k2 = 1;
			}
			else
			{
				i1 = 0;
				j1 = 0;
				k1 = 1;
				i2 = 1;
				j2 = 0;
				k2 = 1;
			}
		}
		else if (y0 < z0)
		{
			i1 = 0;
			j1 = 0;
			k1 = 1;
			i2 = 0;
			j2 = 1;
			k2 = 1;
		}
		else if (x0 < z0)
		{
			i1 = 0;
			j1 = 1;
			k1 = 0;
			i2 = 0;
			j2 = 1;
			k2 = 1;
		}
		else
		{
			i1 = 0;
			j1 = 1;
			k1 = 0;
			i2 = 1;
			j2 = 1;
			k2 = 0;
		}
		x1 = x0 - (double)i1 + G3;
		y1 = y0 - (double)j1 + G3;
		z1 = z0 - (double)k1 + G3;
		x2 = x0 - (double)i2 + 2.0 * G3;
		y2 = y0 - (double)j2 + 2.0 * G3;
		z2 = z0 - (double)k2 + 2.0 * G3;
		x3 = x0 - 1.0 + 3.0 * G3;
		y3 = y0 - 1.0 + 3.0 * G3;
		z3 = z0 - 1.0 + 3.0 * G3;
		ii = i & 0xFF;
		jj = j & 0xFF;
		kk = k & 0xFF;
		gi0 = perm[ii + perm[jj + perm[kk]]] % 12;
		gi1 = perm[ii + i1 + perm[jj + j1 + perm[kk + k1]]] % 12;
		gi2 = perm[ii + i2 + perm[jj + j2 + perm[kk + k2]]] % 12;
		gi3 = perm[ii + 1 + perm[jj + 1 + perm[kk + 1]]] % 12;
		t0 = 0.6 - x0 * x0 - y0 * y0 - z0 * z0;
		if (t0 < 0.0)
		{
			n0 = 0.0;
		}
		else
		{
			t0 *= t0;
			n0 = t0 * t0 * dot(grad3[gi0], x0, y0, z0);
		}
		t1 = 0.6 - x1 * x1 - y1 * y1 - z1 * z1;
		if (t1 < 0.0)
		{
			n1 = 0.0;
		}
		else
		{
			t1 *= t1;
			n1 = t1 * t1 * dot(grad3[gi1], x1, y1, z1);
		}
		t2 = 0.6 - x2 * x2 - y2 * y2 - z2 * z2;
		if (t2 < 0.0)
		{
			n2 = 0.0;
		}
		else
		{
			t2 *= t2;
			n2 = t2 * t2 * dot(grad3[gi2], x2, y2, z2);
		}
		t3 = 0.6 - x3 * x3 - y3 * y3 - z3 * z3;
		if (t3 < 0.0)
		{
			n3 = 0.0;
		}
		else
		{
			t3 *= t3;
			n3 = t3 * t3 * dot(grad3[gi3], x3, y3, z3);
		}
		return 32.0 * (n0 + n1 + n2 + n3);
	}

	public double noiseNormalized(Vector3d v3d)
	{
		return (noise(v3d.x, v3d.y, v3d.z) + 1.0) * 0.5;
	}

	public double noise(Vector3d v3d)
	{
		return noise(v3d.x, v3d.y, v3d.z);
	}

	public double noiseNormalized(double x, double y, double z)
	{
		return (noise(x, y, z) + 1.0) * 0.5;
	}

	public double noise(double x, double y, double z)
	{
		total = 0.0;
		amplitude = 1.0;
		f = frequency;
		maxAmplitude = 0.0;
		for (itr = 0.0; itr < octaves; itr += 1.0)
		{
			total += value(x * f, y * f, z * f) * amplitude;
			f *= 2.0;
			maxAmplitude += amplitude;
			amplitude *= persistence;
		}
		return total / maxAmplitude;
	}
}
