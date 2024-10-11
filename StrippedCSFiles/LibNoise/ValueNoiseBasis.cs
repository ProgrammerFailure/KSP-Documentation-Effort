using System.Runtime.CompilerServices;

namespace LibNoise;

public class ValueNoiseBasis
{
	private const int XNoiseGen = 1619;

	private const int YNoiseGen = 31337;

	private const int ZNoiseGen = 6971;

	private const int SeedNoiseGen = 1013;

	private const int ShiftNoiseGen = 8;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ValueNoiseBasis()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int IntValueNoise(int x, int y, int z, int seed)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double ValueNoise(int x, int y, int z)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double ValueNoise(int x, int y, int z, int seed)
	{
		throw null;
	}
}
