using System;
using System.Runtime.CompilerServices;

namespace UnityEngine;

public struct QuaternionD
{
	public double x;

	public double y;

	public double z;

	public double w;

	public double this[int index]
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

	public QuaternionD swizzle
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static QuaternionD identity
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Vector3d eulerAngles
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public QuaternionD(double x, double y, double z, double w)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public QuaternionD(Vector3d X, Vector3d Y, Vector3d Z)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FrameVectors(out Vector3d frameX, out Vector3d frameY, out Vector3d frameZ)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static implicit operator Quaternion(QuaternionD q)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static implicit operator QuaternionD(Quaternion q)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double Dot(QuaternionD a, QuaternionD b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static QuaternionD AngleAxis(double angle, Vector3d axis)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToAngleAxis(out double angle, out Vector3d axis)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static QuaternionD FromToRotation(Vector3d fromDirection, Vector3d toDirection)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern QuaternionD INTERNAL_CALL_FromToRotation(ref Vector3d fromDirection, ref Vector3d toDirection);

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetFromToRotation(Vector3d fromDirection, Vector3d toDirection)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static QuaternionD LookRotation(Vector3d forward, Vector3d up)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static QuaternionD LookRotation(Vector3d forward)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetLookRotation(Vector3d view)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetLookRotation(Vector3d view, Vector3d up)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static QuaternionD Slerp(QuaternionD from, QuaternionD to, double t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern QuaternionD INTERNAL_CALL_Slerp(ref QuaternionD from, ref QuaternionD to, double t);

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static QuaternionD Lerp(QuaternionD from, QuaternionD to, double t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern QuaternionD INTERNAL_CALL_Lerp(ref QuaternionD from, ref QuaternionD to, double t);

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static QuaternionD RotateTowards(QuaternionD from, QuaternionD to, double maxDegreesDelta)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static QuaternionD UnclampedSlerp(QuaternionD from, QuaternionD to, double t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern QuaternionD INTERNAL_CALL_UnclampedSlerp(ref QuaternionD from, ref QuaternionD to, double t);

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static QuaternionD Inverse(QuaternionD q)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern QuaternionD INTERNAL_CALL_Inverse(ref QuaternionD rotation);

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string ToString()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string ToString(string format)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double Angle(QuaternionD a, QuaternionD b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static QuaternionD Euler(double x, double y, double z)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static QuaternionD Euler(Vector3d euler)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Vector3d Internal_ToEulerRad(QuaternionD rotation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern Vector3d INTERNAL_CALL_Internal_ToEulerRad(ref QuaternionD rotation);

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static QuaternionD Internal_FromEulerRad(Vector3d euler)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern QuaternionD INTERNAL_CALL_Internal_FromEulerRad(ref Vector3d euler);

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Internal_ToAxisAngleRad(QuaternionD q, out Vector3d axis, out double angle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void INTERNAL_CALL_Internal_ToAxisAngleRad(ref QuaternionD q, out Vector3d axis, out double angle);

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Obsolete("Use QuaternionD.Euler instead. This function was deprecated because it uses radians instad of degrees")]
	public static QuaternionD EulerRotation(double x, double y, double z)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Obsolete("Use QuaternionD.Euler instead. This function was deprecated because it uses radians instad of degrees")]
	public static QuaternionD EulerRotation(Vector3d euler)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Obsolete("Use QuaternionD.Euler instead. This function was deprecated because it uses radians instad of degrees")]
	public void SetEulerRotation(double x, double y, double z)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Obsolete("Use QuaternionD.Euler instead. This function was deprecated because it uses radians instad of degrees")]
	public void SetEulerRotation(Vector3d euler)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Obsolete("Use QuaternionD.eulerAngles instead. This function was deprecated because it uses radians instad of degrees")]
	public Vector3d ToEuler()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Obsolete("Use QuaternionD.Euler instead. This function was deprecated because it uses radians instad of degrees")]
	public static QuaternionD EulerAngles(double x, double y, double z)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Obsolete("Use QuaternionD.Euler instead. This function was deprecated because it uses radians instad of degrees")]
	public static QuaternionD EulerAngles(Vector3d euler)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Obsolete("Use QuaternionD.ToAngleAxis instead. This function was deprecated because it uses radians instad of degrees")]
	public void ToAxisAngle(out Vector3d axis, out double angle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Obsolete("Use QuaternionD.Euler instead. This function was deprecated because it uses radians instad of degrees")]
	public void SetEulerAngles(double x, double y, double z)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Obsolete("Use QuaternionD.Euler instead. This function was deprecated because it uses radians instad of degrees")]
	public void SetEulerAngles(Vector3d euler)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Obsolete("Use QuaternionD.eulerAngles instead. This function was deprecated because it uses radians instad of degrees")]
	public static Vector3d ToEulerAngles(QuaternionD rotation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Obsolete("Use QuaternionD.eulerAngles instead. This function was deprecated because it uses radians instad of degrees")]
	public Vector3d ToEulerAngles()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Obsolete("Use QuaternionD.AngleAxis instead. This function was deprecated because it uses radians instad of degrees")]
	public static QuaternionD AxisAngle(Vector3d axis, double angle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern QuaternionD INTERNAL_CALL_AxisAngle(ref Vector3d axis, double angle);

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Obsolete("Use QuaternionD.AngleAxis instead. This function was deprecated because it uses radians instad of degrees")]
	public void SetAxisAngle(Vector3d axis, double angle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override int GetHashCode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool Equals(object other)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static QuaternionD operator *(QuaternionD lhs, QuaternionD rhs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d operator *(QuaternionD rotation, Vector3d point)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool operator ==(QuaternionD lhs, QuaternionD rhs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool operator !=(QuaternionD lhs, QuaternionD rhs)
	{
		throw null;
	}
}
