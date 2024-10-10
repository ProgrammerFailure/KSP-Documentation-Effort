using System;

namespace LibNoise;

public class Math
{
	public static readonly double double_0 = System.Math.PI;

	public static readonly double Sqrt2 = 1.4142135623730951;

	public static readonly double Sqrt3 = 1.7320508075688772;

	public static readonly double DEG_TO_RAD = double_0 / 180.0;

	public static int ClampValue(int value, int lowerBound, int upperBound)
	{
		if (value < lowerBound)
		{
			return lowerBound;
		}
		if (value > upperBound)
		{
			return upperBound;
		}
		return value;
	}

	public double CubicInterpolate(double n0, double n1, double n2, double n3, double a)
	{
		double num = n3 - n2 - (n0 - n1);
		double num2 = n0 - n1 - num;
		double num3 = n2 - n0;
		return num * a * a * a + num2 * a * a + num3 * a + n1;
	}

	public static double GetSmaller(double a, double b)
	{
		if (!(a < b))
		{
			return b;
		}
		return a;
	}

	public static double GetLarger(double a, double b)
	{
		if (!(a > b))
		{
			return b;
		}
		return a;
	}

	public static void SwapValues(ref double a, ref double b)
	{
		double num = a;
		a = b;
		b = num;
	}

	public double LinearInterpolate(double n0, double n1, double a)
	{
		return (1.0 - a) * n0 + a * n1;
	}

	public double SCurve3(double a)
	{
		return a * a * (3.0 - 2.0 * a);
	}

	public double SCurve5(double a)
	{
		double num = a * a * a;
		double num2 = num * a;
		double num3 = num2 * a;
		return 6.0 * num3 - 15.0 * num2 + 10.0 * num;
	}

	public void LatLonToXYZ(double lat, double lon, ref double x, ref double y, ref double z)
	{
		double num = System.Math.Cos(DEG_TO_RAD * lat);
		x = num * System.Math.Cos(DEG_TO_RAD * lon);
		y = System.Math.Sin(DEG_TO_RAD * lat);
		z = num * System.Math.Sin(DEG_TO_RAD * lon);
	}
}
