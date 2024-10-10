using System;

namespace CommNet;

public class CommRangeModel : IRangeModel
{
	public bool InRange(double aPower, double bPower, double sqrDistance)
	{
		double num = aPower * bPower;
		if (sqrDistance <= num)
		{
			return num > 0.0;
		}
		return false;
	}

	public double GetNormalizedRange(double aPower, double bPower, double distance)
	{
		return 1.0 - distance / Math.Sqrt(aPower * bPower);
	}

	public double GetMaximumRange(double aPower, double bPower)
	{
		return Math.Sqrt(aPower * bPower);
	}
}
