using System;
using System.Runtime.CompilerServices;

public class UtilMath
{
	public const double Rad2Deg = 180.0 / Math.PI;

	public const double Deg2Rad = Math.PI / 180.0;

	public static double TwoPI;

	public static float TwoPIf;

	public static double HalfPI;

	public static float HalfPIf;

	public static float RPM2RadPerSec;

	public static float RadPerSec2RPM;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UtilMath()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static UtilMath()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SwapValues(ref double a, ref double b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double Lerp(double a, double b, double t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double LerpUnclamped(double a, double b, double t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double InverseLerp(double a, double b, double t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double DegreesToRadians(double degrees)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double RadiansToDegrees(double radians)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double ASinh(double x)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double ACosh(double x)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double ATanh(double x)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double ACoth(double x)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double ASech(double x)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double ACsch(double x)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double Sech(double x)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double Csch(double x)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double Coth(double x)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float WrapAround(float value, float min, float max)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double WrapAround(double value, double min, double max)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int WrapAround(int value, int min, int max)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double Clamp(double value, double min, double max)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double Clamp01(double value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double ClampRadians(double angle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double ClampRadiansPI(double angle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double ClampRadiansTwoPI(double angle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double ClampDegrees360(double angle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double ClampDegrees180(double angle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double Flatten(double z, double midPoint, double easing)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double Max(params double[] values)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double Min(params double[] values)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static T MaxFrom<T>(Func<T, double> getValue, params T[] values)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static T MinFrom<T>(Func<T, double> getValue, params T[] values)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static T MinFrom<T>(Func<T, double> getValue, out double min, params T[] values)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static T MaxFrom<T>(Func<T, double> getValue, out double max, params T[] values)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static T MaxFrom<T>(Func<T, float> getValue, params T[] values)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static T MinFrom<T>(Func<T, float> getValue, params T[] values)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static T MinFrom<T>(Func<T, float> getValue, out float min, params T[] values)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static T MaxFrom<T>(Func<T, float> getValue, out float max, params T[] values)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float RoundToPlaces(float value, int decimalPlaces)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double RoundToPlaces(double value, int decimalPlaces)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int BSPSolver(ref double v0, double dv, Func<double, double> solveFor, double vMin, double vMax, double epsilon, int maxIterations)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int BSPSolver(ref float v0, float dv, Func<float, float> solveFor, float vMin, float vMax, float epsilon, int maxIterations)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool IsDivisible(int n, int byN)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool IsPowerOfTwo(int x)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool Approximately(double a, double b, double epsilon = double.Epsilon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool SphereIntersection(double radius, Vector3d position, Vector3d velocity, out double time, bool later)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool SphereIntersection(double radius, Vector3d position, Vector3d velocity, out Vector3d impact, bool later)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double AngleBetween(Vector3d v, Vector3d w)
	{
		throw null;
	}
}
