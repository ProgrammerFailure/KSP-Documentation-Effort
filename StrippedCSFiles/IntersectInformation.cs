using System.Runtime.CompilerServices;

public struct IntersectInformation
{
	public int numberOfIntersections;

	public double intersect1UT;

	public double intersect2Distance;

	public double intersect2UT;

	public double intersect1Distance;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public IntersectInformation(int num, double i1Dist, double i1UT, double i2Dist, double i2UT)
	{
		throw null;
	}
}
