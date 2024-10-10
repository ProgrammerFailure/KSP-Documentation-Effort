using System;
using UnityEngine;

namespace LibNoise.Modifiers;

public class Blend : Math, IModule
{
	public IModule SourceModule1 { get; set; }

	public IModule SourceModule2 { get; set; }

	public IModule WeightModule { get; set; }

	public Blend(IModule sourceModule1, IModule sourceModule2, IModule weightModule)
	{
		if (sourceModule1 == null || sourceModule2 == null || weightModule == null)
		{
			throw new ArgumentNullException();
		}
		SourceModule1 = sourceModule1;
		SourceModule2 = sourceModule2;
		WeightModule = weightModule;
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
		if (SourceModule1 == null || SourceModule2 == null || WeightModule == null)
		{
			throw new NullReferenceException();
		}
		return LinearInterpolate(SourceModule1.GetValue(x, y, z), SourceModule2.GetValue(x, y, z), (WeightModule.GetValue(x, y, z) + 1.0) / 2.0);
	}
}
