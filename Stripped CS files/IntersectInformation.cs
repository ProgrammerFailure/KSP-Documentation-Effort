public struct IntersectInformation
{
	public int numberOfIntersections;

	public double intersect1UT;

	public double intersect2Distance;

	public double intersect2UT;

	public double intersect1Distance;

	public IntersectInformation(int num, double i1Dist, double i1UT, double i2Dist, double i2UT)
	{
		intersect1UT = i1UT;
		intersect2Distance = i1Dist;
		intersect2UT = i2UT;
		intersect1Distance = i2Dist;
		numberOfIntersections = num;
	}
}
