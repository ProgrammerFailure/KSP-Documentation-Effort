using UnityEngine;

namespace LibNoise.Modifiers;

public class Constant : IModule
{
	public double Value { get; set; }

	public Constant(double value)
	{
		Value = value;
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
		return Value;
	}
}
