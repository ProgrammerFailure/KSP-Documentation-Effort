using System.Runtime.CompilerServices;

namespace LibNoise;

public class FastNoiseBasis : Math
{
	private int[] RandomPermutations;

	private int[] SelectedPermutations;

	private float[] GradientTable;

	private int mSeed;

	public int Seed
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FastNoiseBasis()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FastNoiseBasis(int seed)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GradientCoherentNoise(double x, double y, double z, int seed, NoiseQuality noiseQuality)
	{
		throw null;
	}
}
