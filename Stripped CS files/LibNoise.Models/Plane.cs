using System;

namespace LibNoise.Models;

public class Plane
{
	public IModule SourceModule { get; set; }

	public Plane(IModule sourceModule)
	{
		if (sourceModule == null)
		{
			throw new ArgumentNullException("A source module must be provided.");
		}
		SourceModule = sourceModule;
	}

	public double GetValue(double x, double z)
	{
		if (SourceModule == null)
		{
			throw new NullReferenceException("A source module must be provided.");
		}
		return SourceModule.GetValue(x, 0.0, z);
	}
}
