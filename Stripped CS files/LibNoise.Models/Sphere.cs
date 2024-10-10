using System;

namespace LibNoise.Models;

public class Sphere : Math
{
	public IModule SourceModule { get; set; }

	public Sphere(IModule sourceModule)
	{
		if (sourceModule == null)
		{
			throw new ArgumentNullException("A source module must be provided.");
		}
		SourceModule = sourceModule;
	}

	public double GetValue(double latitude, double longitude)
	{
		if (SourceModule == null)
		{
			throw new NullReferenceException("A source module must be provided.");
		}
		double x = 0.0;
		double y = 0.0;
		double z = 0.0;
		LatLonToXYZ(latitude, longitude, ref x, ref y, ref z);
		return SourceModule.GetValue(x, y, z);
	}
}
