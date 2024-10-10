using System;
using UnityEngine;

namespace LibNoise.Modifiers;

public class TranslateInput : IModule
{
	public double Double_0 { get; set; }

	public double Double_1 { get; set; }

	public double Double_2 { get; set; }

	public IModule SourceModule { get; set; }

	public TranslateInput(IModule sourceModule, double x, double y, double z)
	{
		if (sourceModule == null)
		{
			throw new ArgumentNullException("A source module must be provided.");
		}
		SourceModule = sourceModule;
		Double_0 = x;
		Double_1 = y;
		Double_2 = z;
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
		return SourceModule.GetValue(x + Double_0, y + Double_1, z + Double_2);
	}
}
