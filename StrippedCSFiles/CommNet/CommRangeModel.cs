using System.Runtime.CompilerServices;

namespace CommNet;

public class CommRangeModel : IRangeModel
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public CommRangeModel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool InRange(double aPower, double bPower, double sqrDistance)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetNormalizedRange(double aPower, double bPower, double distance)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetMaximumRange(double aPower, double bPower)
	{
		throw null;
	}
}
