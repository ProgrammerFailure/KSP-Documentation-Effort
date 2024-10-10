using System;
using UnityEngine;

namespace LibNoise;

public class FastTurbulence : IModule
{
	public FastNoise XDistort;

	public FastNoise YDistort;

	public FastNoise ZDistort;

	public IModule SourceModule { get; set; }

	public double Power { get; set; }

	public double Frequency
	{
		get
		{
			return XDistort.Frequency;
		}
		set
		{
			FastNoise xDistort = XDistort;
			FastNoise yDistort = YDistort;
			double num2 = (ZDistort.Frequency = value);
			double frequency = (yDistort.Frequency = num2);
			xDistort.Frequency = frequency;
		}
	}

	public int Roughness
	{
		get
		{
			return XDistort.OctaveCount;
		}
		set
		{
			FastNoise xDistort = XDistort;
			FastNoise yDistort = YDistort;
			int num2 = (ZDistort.OctaveCount = value);
			int octaveCount = (yDistort.OctaveCount = num2);
			xDistort.OctaveCount = octaveCount;
		}
	}

	public int Seed
	{
		get
		{
			return XDistort.Seed;
		}
		set
		{
			XDistort.Seed = value;
			YDistort.Seed = value + 1;
			ZDistort.Seed = value + 2;
		}
	}

	public FastTurbulence(IModule sourceModule)
	{
		if (sourceModule == null)
		{
			throw new ArgumentNullException();
		}
		SourceModule = sourceModule;
		XDistort = new FastNoise();
		YDistort = new FastNoise();
		ZDistort = new FastNoise();
		Frequency = 1.0;
		Power = 1.0;
		Roughness = 3;
		Seed = 0;
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
			throw new NullReferenceException();
		}
		double x2 = x + 0.189422607421875;
		double y2 = y + 0.99371337890625;
		double z2 = z + 0.4781646728515625;
		double x3 = x + 0.4046478271484375;
		double y3 = y + 0.276611328125;
		double z3 = z + 0.9230499267578125;
		double x4 = x + 0.82122802734375;
		double y4 = y + 0.1710968017578125;
		double z4 = z + 0.6842803955078125;
		double x5 = x + XDistort.GetValue(x2, y2, z2) * Power;
		double y5 = y + YDistort.GetValue(x3, y3, z3) * Power;
		double z5 = z + ZDistort.GetValue(x4, y4, z4) * Power;
		return SourceModule.GetValue(x5, y5, z5);
	}
}
