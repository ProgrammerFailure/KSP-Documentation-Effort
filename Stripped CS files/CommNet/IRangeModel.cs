namespace CommNet;

public interface IRangeModel
{
	bool InRange(double aPower, double bPower, double sqrDistance);

	double GetNormalizedRange(double aPower, double bPower, double distance);

	double GetMaximumRange(double aPower, double bPower);
}
