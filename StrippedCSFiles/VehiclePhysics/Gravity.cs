using System.Runtime.CompilerServices;
using UnityEngine;

namespace VehiclePhysics;

public static class Gravity
{
	private static bool s_gravityCached;

	private static float s_gravityMagnitude;

	private static Vector3 s_gravityUp;

	public const float reference = 9.807f;

	public const float forceToMass = 0.10196798f;

	public const float massToForce = 9.807f;

	public static Vector3 value
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public static float magnitude
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Vector3 up
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static Gravity()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Refresh()
	{
		throw null;
	}
}
