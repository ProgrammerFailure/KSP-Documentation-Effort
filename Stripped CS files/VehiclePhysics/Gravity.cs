using UnityEngine;

namespace VehiclePhysics;

public static class Gravity
{
	public static bool s_gravityCached;

	public static float s_gravityMagnitude;

	public static Vector3 s_gravityUp;

	public const float reference = 9.807f;

	public const float forceToMass = 0.10196798f;

	public const float massToForce = 9.807f;

	public static Vector3 value
	{
		get
		{
			return Physics.gravity;
		}
		set
		{
			if (Physics.gravity != value)
			{
				Physics.gravity = value;
				Refresh();
			}
		}
	}

	public static float magnitude
	{
		get
		{
			if (!s_gravityCached)
			{
				Refresh();
			}
			return s_gravityMagnitude;
		}
	}

	public static Vector3 up
	{
		get
		{
			if (!s_gravityCached)
			{
				Refresh();
			}
			return s_gravityUp;
		}
	}

	public static void Refresh()
	{
		s_gravityMagnitude = Physics.gravity.magnitude;
		s_gravityUp = (-Physics.gravity).normalized;
		s_gravityCached = true;
	}
}
