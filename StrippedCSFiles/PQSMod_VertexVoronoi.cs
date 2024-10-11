using System.Runtime.CompilerServices;
using LibNoise;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/Voronoi")]
public class PQSMod_VertexVoronoi : PQSMod
{
	public class Voronoi
	{
		private static double defaultDisplacement;

		private static double defaultFrequency;

		private static int defaultSeed;

		private static double SQRT_3;

		private static int xNoiseGen;

		private static int yNoiseGen;

		private static int zNoiseGen;

		private static int seedNoiseGen;

		public double displacement
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			get
			{
				throw null;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			set
			{
				throw null;
			}
		}

		public bool enableDistance
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			get
			{
				throw null;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			set
			{
				throw null;
			}
		}

		public double frequency
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			get
			{
				throw null;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			set
			{
				throw null;
			}
		}

		public int seed
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			get
			{
				throw null;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			set
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Voronoi()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Voronoi(int seed, double displacement, double frequency, bool enableDistance)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		static Voronoi()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public double GetValue(double x, double y, double z)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private double ValueNoise3D(int x, int y, int z, int seed)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private int IntValueNoise3D(int x, int y, int z, int seed)
		{
			throw null;
		}
	}

	private LibNoise.Voronoi voronoi;

	public double deformation;

	public int voronoiSeed;

	public double voronoiDisplacement;

	public double voronoiFrequency;

	public bool voronoiEnableDistance;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQSMod_VertexVoronoi()
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
}
