using System.Runtime.CompilerServices;

namespace LibNoise;

public class Math
{
	public static readonly double PI;

	public static readonly double Sqrt2;

	public static readonly double Sqrt3;

	public static readonly double DEG_TO_RAD;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Math()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static Math()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int ClampValue(int value, int lowerBound, int upperBound)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected double CubicInterpolate(double n0, double n1, double n2, double n3, double a)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double GetSmaller(double a, double b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double GetLarger(double a, double b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SwapValues(ref double a, ref double b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected double LinearInterpolate(double n0, double n1, double a)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected double SCurve3(double a)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected double SCurve5(double a)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void LatLonToXYZ(double lat, double lon, ref double x, ref double y, ref double z)
	{
		throw null;
	}
}
