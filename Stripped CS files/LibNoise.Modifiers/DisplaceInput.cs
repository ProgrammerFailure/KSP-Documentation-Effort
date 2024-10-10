using System;
using UnityEngine;

namespace LibNoise.Modifiers;

public class DisplaceInput : IModule
{
	public IModule SourceModule { get; set; }

	public IModule XDisplaceModule { get; set; }

	public IModule YDisplaceModule { get; set; }

	public IModule ZDisplaceModule { get; set; }

	public DisplaceInput(IModule sourceModule, IModule xDisplaceModule, IModule yDisplaceModule, IModule zDisplaceModule)
	{
		if (sourceModule == null || xDisplaceModule == null || yDisplaceModule == null || zDisplaceModule == null)
		{
			throw new ArgumentNullException("Source and X, Y, and Z displacement modules must be provided.");
		}
		SourceModule = sourceModule;
		XDisplaceModule = xDisplaceModule;
		YDisplaceModule = yDisplaceModule;
		ZDisplaceModule = zDisplaceModule;
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
		if (SourceModule == null || XDisplaceModule == null || YDisplaceModule == null || ZDisplaceModule == null)
		{
			throw new NullReferenceException("Source and X, Y, and Z displacement modules must be provided.");
		}
		x += XDisplaceModule.GetValue(x, y, z);
		y += YDisplaceModule.GetValue(x, y, z);
		z += ZDisplaceModule.GetValue(x, y, z);
		return SourceModule.GetValue(x, y, z);
	}
}
