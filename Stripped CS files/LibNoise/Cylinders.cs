using System;
using UnityEngine;

namespace LibNoise;

public class Cylinders : IModule
{
	public double Frequency { get; set; }

	public Cylinders()
	{
		Frequency = 1.0;
	}

	public double GetValue(Vector3d coordinate)
	{
		return GetValue(coordinate.x, coordinate.y, coordinate.z);
	}

	public double GetValue(Vector3 coordinate)
	{
		return GetValue(coordinate.x, coordinate.y, coordinate.z);
	}

	public double GetValue(double x, double y, double z)
	{
		x *= Frequency;
		z *= Frequency;
		double num = System.Math.Sqrt(x * x + z * z);
		int num2 = ((num > 0.0) ? ((int)num) : ((int)num - 1));
		double num3 = num - (double)num2;
		double b = 1.0 - num3;
		double smaller = Math.GetSmaller(num3, b);
		return 1.0 - smaller * 4.0;
	}
}
