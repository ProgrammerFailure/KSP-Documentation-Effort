using System;
using UnityEngine;

namespace LibNoise.Modifiers;

public class ScaleBiasOutput : IModule
{
	public double Scale { get; set; }

	public double Bias { get; set; }

	public IModule SourceModule { get; set; }

	public ScaleBiasOutput(IModule sourceModule)
	{
		if (sourceModule == null)
		{
			throw new ArgumentNullException("A source module must be provided.");
		}
		SourceModule = sourceModule;
		Bias = 0.0;
		Scale = 1.0;
	}

	public ScaleBiasOutput(double scale, double bias, IModule sourcemodule)
	{
		if (sourcemodule == null)
		{
			throw new ArgumentNullException("A source module must be provided.");
		}
		SourceModule = sourcemodule;
		Bias = bias;
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
			throw new Exception("A source module must be provided.");
		}
		return SourceModule.GetValue(x, y, z) * Scale + Bias;
	}
}
