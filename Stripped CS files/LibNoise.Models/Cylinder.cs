using System;

namespace LibNoise.Models;

public class Cylinder
{
	public IModule SourceModule { get; set; }

	public Cylinder(IModule sourceModule)
	{
		if (sourceModule == null)
		{
			throw new ArgumentNullException("A source module must be provided.");
		}
		SourceModule = sourceModule;
	}

	public double GetValue(double angle, double height)
	{
		if (SourceModule == null)
		{
			throw new NullReferenceException("A source module must be provided.");
		}
		double x = System.Math.Cos(angle);
		double z = System.Math.Sin(angle);
		return SourceModule.GetValue(x, height, z);
	}
}
