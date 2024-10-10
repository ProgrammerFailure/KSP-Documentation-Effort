using System;
using System.Collections.Generic;
using UnityEngine;

namespace LibNoise.Modifiers;

public class Terrace : Math, IModule
{
	public List<double> ControlPoints = new List<double>();

	public IModule SourceModule { get; set; }

	public bool InvertTerraces { get; set; }

	public Terrace(IModule sourceModule)
	{
		if (sourceModule == null)
		{
			throw new ArgumentNullException("A source module must be provided.");
		}
		SourceModule = sourceModule;
		InvertTerraces = false;
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
		if (SourceModule == null)
		{
			throw new NullReferenceException("A source module must be provided.");
		}
		if (ControlPoints.Count < 2)
		{
			throw new Exception("Two or more control points must be specified.");
		}
		double value = SourceModule.GetValue(x, y, z);
		int count = ControlPoints.Count;
		int i;
		for (i = 0; i < count && !(value < ControlPoints[i]); i++)
		{
		}
		int num = Math.ClampValue(i - 1, 0, count - 1);
		int num2 = Math.ClampValue(i, 0, count - 1);
		if (num == num2)
		{
			return ControlPoints[num2];
		}
		double a = ControlPoints[num];
		double b = ControlPoints[num2];
		double num3 = (value - a) / (b - a);
		if (InvertTerraces)
		{
			num3 = 1.0 - num3;
			Math.SwapValues(ref a, ref b);
		}
		num3 *= num3;
		return LinearInterpolate(a, b, num3);
	}
}
