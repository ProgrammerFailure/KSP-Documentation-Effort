using System;

public class UtilMath
{
	public const double Rad2Deg = 180.0 / Math.PI;

	public const double Deg2Rad = Math.PI / 180.0;

	public static double TwoPI = Math.PI * 2.0;

	public static float TwoPIf = (float)Math.PI * 2f;

	public static double HalfPI = Math.PI / 2.0;

	public static float HalfPIf = (float)Math.PI / 2f;

	public static float RPM2RadPerSec = TwoPIf / 60f;

	public static float RadPerSec2RPM = 60f / TwoPIf;

	public static void SwapValues(ref double a, ref double b)
	{
		double num = a;
		a = b;
		b = num;
	}

	public static double Lerp(double a, double b, double t)
	{
		t = Clamp01(t);
		return (1.0 - t) * a + t * b;
	}

	public static double LerpUnclamped(double a, double b, double t)
	{
		return (1.0 - t) * a + t * b;
	}

	public static double InverseLerp(double a, double b, double t)
	{
		return (t - a) / (b - a);
	}

	public static double DegreesToRadians(double degrees)
	{
		return Math.PI / 180.0 * degrees;
	}

	public static double RadiansToDegrees(double radians)
	{
		return 180.0 / Math.PI * radians;
	}

	public static double ASinh(double x)
	{
		return Math.Log(x + Math.Sqrt(x * x + 1.0));
	}

	public static double ACosh(double x)
	{
		return Math.Log(x + Math.Sqrt(x * x - 1.0));
	}

	public static double ATanh(double x)
	{
		return Math.Log((1.0 + x) / (1.0 - x)) / 2.0;
	}

	public static double ACoth(double x)
	{
		return ATanh(1.0 / x);
	}

	public static double ASech(double x)
	{
		return ACosh(1.0 / x);
	}

	public static double ACsch(double x)
	{
		return ASinh(1.0 / x);
	}

	public static double Sech(double x)
	{
		return 1.0 / Math.Cosh(x);
	}

	public static double Csch(double x)
	{
		return 1.0 / Math.Sinh(x);
	}

	public static double Coth(double x)
	{
		return Math.Cosh(x) / Math.Sinh(x);
	}

	public static float WrapAround(float value, float min, float max)
	{
		if (value < min)
		{
			value += max;
		}
		return value % max;
	}

	public static double WrapAround(double value, double min, double max)
	{
		if (value < min)
		{
			value += max;
		}
		return value % max;
	}

	public static int WrapAround(int value, int min, int max)
	{
		if (value < min)
		{
			value += max;
		}
		return value % max;
	}

	public static double Clamp(double value, double min, double max)
	{
		if (double.IsNaN(value))
		{
			return min;
		}
		if (value < min)
		{
			return min;
		}
		if (value > max)
		{
			return max;
		}
		return value;
	}

	public static double Clamp01(double value)
	{
		if (value < 0.0)
		{
			return 0.0;
		}
		if (value > 1.0)
		{
			return 1.0;
		}
		return value;
	}

	public static double ClampRadians(double angle)
	{
		while (angle > Math.PI * 2.0)
		{
			angle -= Math.PI * 2.0;
		}
		while (angle < 0.0)
		{
			angle += Math.PI * 2.0;
		}
		return angle;
	}

	public static double ClampRadiansPI(double angle)
	{
		while (angle > Math.PI)
		{
			angle -= Math.PI;
		}
		while (angle < -Math.PI)
		{
			angle += Math.PI;
		}
		return angle;
	}

	public static double ClampRadiansTwoPI(double angle)
	{
		angle %= TwoPI;
		if (angle < 0.0)
		{
			return angle + TwoPI;
		}
		return angle;
	}

	public static double ClampDegrees360(double angle)
	{
		angle %= 360.0;
		if (angle < 0.0)
		{
			return angle + 360.0;
		}
		return angle;
	}

	public static double ClampDegrees180(double angle)
	{
		angle = ClampDegrees360(angle);
		if (angle > 180.0)
		{
			angle -= 360.0;
		}
		return angle;
	}

	public static double Flatten(double z, double midPoint, double easing)
	{
		return 1.0 - 1.0 / (Math.Pow(1.0 / midPoint * Math.Abs(z), easing) + 1.0) * (double)Math.Sign(z);
	}

	public static double Max(params double[] values)
	{
		double num = double.MinValue;
		int num2 = values.Length;
		while (num2-- > 0)
		{
			if (values[num2] > num)
			{
				num = values[num2];
			}
		}
		return num;
	}

	public static double Min(params double[] values)
	{
		double num = double.MaxValue;
		int num2 = values.Length;
		while (num2-- > 0)
		{
			if (values[num2] < num)
			{
				num = values[num2];
			}
		}
		return num;
	}

	public static T MaxFrom<T>(Func<T, double> getValue, params T[] values)
	{
		double num = double.MinValue;
		T result = values[0];
		int num2 = values.Length;
		while (num2-- > 0)
		{
			double num3 = getValue(values[num2]);
			if (num3 > num)
			{
				num = num3;
				result = values[num2];
			}
		}
		return result;
	}

	public static T MinFrom<T>(Func<T, double> getValue, params T[] values)
	{
		double num = double.MaxValue;
		T result = values[0];
		int num2 = values.Length;
		while (num2-- > 0)
		{
			double num3 = getValue(values[num2]);
			if (num3 < num)
			{
				num = num3;
				result = values[num2];
			}
		}
		return result;
	}

	public static T MinFrom<T>(Func<T, double> getValue, out double min, params T[] values)
	{
		min = double.MaxValue;
		T result = values[0];
		int num = values.Length;
		while (num-- > 0)
		{
			double num2 = getValue(values[num]);
			if (num2 < min)
			{
				min = num2;
				result = values[num];
			}
		}
		return result;
	}

	public static T MaxFrom<T>(Func<T, double> getValue, out double max, params T[] values)
	{
		max = double.MinValue;
		T result = values[0];
		int num = values.Length;
		while (num-- > 0)
		{
			double num2 = getValue(values[num]);
			if (num2 > max)
			{
				max = num2;
				result = values[num];
			}
		}
		return result;
	}

	public static T MaxFrom<T>(Func<T, float> getValue, params T[] values)
	{
		float num = float.MinValue;
		T result = values[0];
		int num2 = values.Length;
		while (num2-- > 0)
		{
			float num3 = getValue(values[num2]);
			if (num3 > num)
			{
				num = num3;
				result = values[num2];
			}
		}
		return result;
	}

	public static T MinFrom<T>(Func<T, float> getValue, params T[] values)
	{
		float num = float.MaxValue;
		T result = values[0];
		int num2 = values.Length;
		while (num2-- > 0)
		{
			float num3 = getValue(values[num2]);
			if (num3 < num)
			{
				num = num3;
				result = values[num2];
			}
		}
		return result;
	}

	public static T MinFrom<T>(Func<T, float> getValue, out float min, params T[] values)
	{
		min = float.MaxValue;
		T result = values[0];
		int num = values.Length;
		while (num-- > 0)
		{
			float num2 = getValue(values[num]);
			if (num2 < min)
			{
				min = num2;
				result = values[num];
			}
		}
		return result;
	}

	public static T MaxFrom<T>(Func<T, float> getValue, out float max, params T[] values)
	{
		max = float.MinValue;
		T result = values[0];
		int num = values.Length;
		while (num-- > 0)
		{
			float num2 = getValue(values[num]);
			if (num2 > max)
			{
				max = num2;
				result = values[num];
			}
		}
		return result;
	}

	public static float RoundToPlaces(float value, int decimalPlaces)
	{
		float num = (float)Math.Pow(10.0, decimalPlaces);
		return (float)(Math.Round(value * num) / (double)num);
	}

	public static double RoundToPlaces(double value, int decimalPlaces)
	{
		double num = Math.Pow(10.0, decimalPlaces);
		return Math.Round(value * num) / num;
	}

	public static int BSPSolver(ref double v0, double dv, Func<double, double> solveFor, double vMin, double vMax, double epsilon, int maxIterations)
	{
		if (v0 < vMin)
		{
			return 0;
		}
		if (v0 > vMax)
		{
			return 0;
		}
		int num = 0;
		double num2 = solveFor(v0);
		while (!(dv <= epsilon) && num < maxIterations)
		{
			double value = solveFor(v0 + dv);
			double value2 = solveFor(v0 - dv);
			if (v0 - dv < vMin)
			{
				value2 = double.MaxValue;
			}
			if (v0 + dv > vMax)
			{
				value = double.MaxValue;
			}
			value = Math.Abs(value);
			value2 = Math.Abs(value2);
			num2 = Math.Min(num2, Math.Min(value, value2));
			if (num2 == value2)
			{
				v0 -= dv;
			}
			else if (num2 == value)
			{
				v0 += dv;
			}
			dv /= 2.0;
			num++;
		}
		return num;
	}

	public static int BSPSolver(ref float v0, float dv, Func<float, float> solveFor, float vMin, float vMax, float epsilon, int maxIterations)
	{
		if (v0 < vMin)
		{
			return 0;
		}
		if (v0 > vMax)
		{
			return 0;
		}
		int num = 0;
		float num2 = solveFor(v0);
		while (!(dv <= epsilon) && num < maxIterations)
		{
			float value = solveFor(v0 + dv);
			float value2 = solveFor(v0 - dv);
			if (v0 - dv < vMin)
			{
				value2 = float.MaxValue;
			}
			if (v0 + dv > vMax)
			{
				value = float.MaxValue;
			}
			value = Math.Abs(value);
			value2 = Math.Abs(value2);
			num2 = Math.Min(num2, Math.Min(value, value2));
			if (num2 == value2)
			{
				v0 -= dv;
			}
			else if (num2 == value)
			{
				v0 += dv;
			}
			dv /= 2f;
			num++;
		}
		return num;
	}

	public static bool IsDivisible(int n, int byN)
	{
		while (byN % 2 == 0)
		{
			byN /= 2;
		}
		while (byN % 5 == 0)
		{
			byN /= 5;
		}
		return n % byN == 0;
	}

	public static bool IsPowerOfTwo(int x)
	{
		return (x & (x - 1)) == 0;
	}

	public static bool Approximately(double a, double b, double epsilon = double.Epsilon)
	{
		double num = Math.Abs(a);
		double num2 = Math.Abs(b);
		double num3 = Math.Abs(a - b);
		if (a == b)
		{
			return true;
		}
		if (a != 0.0 && b != 0.0 && num3 >= double.Epsilon)
		{
			return num3 / (num + num2) < epsilon;
		}
		return num3 < epsilon;
	}

	public static bool SphereIntersection(double radius, Vector3d position, Vector3d velocity, out double time, bool later)
	{
		double num = Vector3d.Dot(position, velocity);
		double num2 = Vector3d.Dot(velocity, velocity);
		double num3 = Vector3d.Dot(position, position);
		double num4 = num * num - num2 * (num3 - radius * radius);
		if (num4 >= 0.0 && num2 > 0.0)
		{
			if (later)
			{
				time = (0.0 - num + Math.Sqrt(num4)) / num2;
			}
			else
			{
				time = (0.0 - num - Math.Sqrt(num4)) / num2;
			}
			return true;
		}
		time = 0.0;
		return false;
	}

	public static bool SphereIntersection(double radius, Vector3d position, Vector3d velocity, out Vector3d impact, bool later)
	{
		if (SphereIntersection(radius, position, velocity, out double time, later))
		{
			impact = position + velocity * time;
			return true;
		}
		impact = Vector3d.zero;
		return false;
	}

	public static double AngleBetween(Vector3d v, Vector3d w)
	{
		Vector3d vector3d = v * w.magnitude;
		Vector3d vector3d2 = w * v.magnitude;
		return 2.0 * Math.Atan2((vector3d - vector3d2).magnitude, (vector3d + vector3d2).magnitude);
	}
}
