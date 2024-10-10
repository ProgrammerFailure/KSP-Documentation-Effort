using System;
using UnityEngine;

namespace LibNoise.Modifiers;

public class ClampOutput : IModule
{
	public double LowerBound { get; set; }

	public double UpperBound { get; set; }

	public IModule SourceModule { get; set; }

	public ClampOutput(IModule sourceModule)
	{
		if (sourceModule == null)
		{
			throw new ArgumentNullException("A source module must be provided.");
		}
		SourceModule = sourceModule;
		LowerBound = -1.0;
		UpperBound = 1.0;
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
		double value = SourceModule.GetValue(x, y, z);
		if (value < LowerBound)
		{
			return LowerBound;
		}
		if (value > UpperBound)
		{
			return UpperBound;
		}
		return value;
	}

	public void SetBounds(double lowerBound, double upperBound)
	{
		if (LowerBound >= upperBound)
		{
			throw new Exception("Lower bound must be lower than upper bound.");
		}
		LowerBound = lowerBound;
		UpperBound = upperBound;
	}
}
