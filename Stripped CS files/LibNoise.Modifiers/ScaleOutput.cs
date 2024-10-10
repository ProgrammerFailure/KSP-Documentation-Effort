using System;
using UnityEngine;

namespace LibNoise.Modifiers;

public class ScaleOutput : IModule
{
	public IModule SourceModule { get; set; }

	public double Scale { get; set; }

	public ScaleOutput(IModule sourceModule, double scale)
	{
		if (sourceModule == null)
		{
			throw new ArgumentNullException("A source module must be provided.");
		}
		SourceModule = sourceModule;
		Scale = scale;
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
		return SourceModule.GetValue(x, y, z) * Scale;
	}
}
