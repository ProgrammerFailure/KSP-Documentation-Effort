using System;
using LibNoise;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/Voronoi")]
public class PQSMod_VertexVoronoi : PQSMod
{
	public class Voronoi
	{
		public static double defaultDisplacement = 1.0;

		public static double defaultFrequency = 1.0;

		public static int defaultSeed = 0;

		public static double SQRT_3 = System.Math.Sqrt(3.0);

		public static int xNoiseGen = 1;

		public static int yNoiseGen = 31337;

		public static int zNoiseGen = 263;

		public static int seedNoiseGen = 1013;

		public double displacement { get; set; }

		public bool enableDistance { get; set; }

		public double frequency { get; set; }

		public int seed { get; set; }

		public Voronoi()
		{
			displacement = defaultDisplacement;
			enableDistance = false;
			frequency = defaultFrequency;
			seed = defaultSeed;
		}

		public Voronoi(int seed, double displacement, double frequency, bool enableDistance)
		{
			this.seed = seed;
			this.displacement = displacement;
			this.frequency = frequency;
			this.enableDistance = enableDistance;
		}

		public double GetValue(double x, double y, double z)
		{
			x *= frequency;
			y *= frequency;
			z *= frequency;
			int num = ((x > 0.0) ? ((int)x) : ((int)x - 1));
			int num2 = ((y > 0.0) ? ((int)y) : ((int)y - 1));
			int num3 = ((z > 0.0) ? ((int)z) : ((int)z - 1));
			double num4 = 2147483647.0;
			double num5 = 0.0;
			double num6 = 0.0;
			double num7 = 0.0;
			for (int i = num3 - 2; i <= num3 + 2; i++)
			{
				for (int j = num2 - 2; j <= num2 + 2; j++)
				{
					for (int k = num - 2; k <= num + 2; k++)
					{
						double num8 = (double)k + ValueNoise3D(k, j, i, seed);
						double num9 = (double)j + ValueNoise3D(k, j, i, seed + 1);
						double num10 = (double)i + ValueNoise3D(k, j, i, seed + 2);
						double num11 = num8 - x;
						double num12 = num9 - y;
						double num13 = num10 - z;
						double num14 = num11 * num11 + num12 * num12 + num13 * num13;
						if (num14 < num4)
						{
							num4 = num14;
							num5 = num8;
							num6 = num9;
							num7 = num10;
						}
					}
				}
			}
			double num18;
			if (enableDistance)
			{
				double num15 = num5 - x;
				double num16 = num6 - y;
				double num17 = num7 - z;
				num18 = System.Math.Sqrt(num15 * num15 + num16 * num16 + num17 * num17) * SQRT_3 - 1.0;
			}
			else
			{
				num18 = 0.0;
			}
			return num18 + displacement * ValueNoise3D((int)System.Math.Floor(num5), (int)System.Math.Floor(num6), (int)System.Math.Floor(num7), 0);
		}

		public double ValueNoise3D(int x, int y, int z, int seed)
		{
			return 1.0 - (double)IntValueNoise3D(x, y, z, seed) / 1073741824.0;
		}

		public int IntValueNoise3D(int x, int y, int z, int seed)
		{
			int num = (xNoiseGen * x + yNoiseGen * y + zNoiseGen * z + seedNoiseGen * seed) & 0x7FFFFFFF;
			num = (num >> 13) ^ num;
			return (num * (num * num * 60493 + 19990303) + 1376312589) & 0x7FFFFFFF;
		}
	}

	public LibNoise.Voronoi voronoi;

	public double deformation = 1000.0;

	public int voronoiSeed;

	public double voronoiDisplacement = 1.0;

	public double voronoiFrequency = 1.0;

	public bool voronoiEnableDistance;

	public override void OnSetup()
	{
		voronoi = new LibNoise.Voronoi(voronoiFrequency, voronoiDisplacement, voronoiSeed, voronoiEnableDistance);
	}

	public override void OnVertexBuildHeight(GClass4.VertexBuildData data)
	{
		data.vertHeight += voronoi.GetValue(data.directionFromCenter.x, data.directionFromCenter.y, data.directionFromCenter.z) * deformation;
	}
}
