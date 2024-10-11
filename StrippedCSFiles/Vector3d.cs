using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public struct Vector3d
{
	public double x;

	public double y;

	public double z;

	public Vector3d xzy
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

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

	public Vector3d normalized
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double magnitude
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double sqrMagnitude
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Vector3d zero
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Vector3d one
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Vector3d forward
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Vector3d back
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Vector3d up
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Vector3d down
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Vector3d left
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Vector3d right
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[Obsolete("Use Vector3.forward instead.")]
	public static Vector3d fwd
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d(double x, double y, double z)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d(double x, double y)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Swizzle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static implicit operator Vector3(Vector3d v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static implicit operator Vector3d(Vector3 v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d Lerp(Vector3d from, Vector3d to, double t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern Vector3d Slerp(Vector3d from, Vector3d to, float t);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void Internal_OrthoNormalize2(ref Vector3d a, ref Vector3d b);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void Internal_OrthoNormalize3(ref Vector3d a, ref Vector3d b, ref Vector3d c);

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void OrthoNormalize(ref Vector3d normal, ref Vector3d tangent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void OrthoNormalize(ref Vector3d normal, ref Vector3d tangent, ref Vector3d binormal)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern Vector3d RotateTowards(Vector3d from, Vector3d to, float maxRadiansDelta, float maxMagnitudeDelta);

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d Scale(Vector3d a, Vector3d b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Scale(Vector3d scale)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d Cross(Vector3d lhs, Vector3d rhs)
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
	public static Vector3d Reflect(Vector3d inDirection, Vector3d inNormal)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d Normalize(Vector3d value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Normalize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double Dot(Vector3d lhs, Vector3d rhs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d Project(Vector3d vector, Vector3d onNormal)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d Exclude(Vector3d excludeThis, Vector3d fromThat)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double Angle(Vector3d from, Vector3d to)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double Distance(Vector3d a, Vector3d b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double Magnitude(Vector3d a)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double SqrMagnitude(Vector3d a)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d Min(Vector3d lhs, Vector3d rhs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d Max(Vector3d lhs, Vector3d rhs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d Basis(Vector3d X, Vector3d Y, Vector3d Z)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Obsolete("Use Vector3.Angle instead. AngleBetween uses radians instead of degrees and was deprecated for this reason")]
	public static double AngleBetween(Vector3d from, Vector3d to)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d operator +(Vector3d a, Vector3d b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d operator +(Vector3 a, Vector3d b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d operator +(Vector3d a, Vector3 b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d operator -(Vector3d a, Vector3d b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d operator -(Vector3 a, Vector3d b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d operator -(Vector3d a, Vector3 b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d operator -(Vector3d a)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d operator *(Vector3d a, double d)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d operator *(double d, Vector3d a)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d operator /(Vector3d a, double d)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool operator ==(Vector3d lhs, Vector3d rhs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool operator !=(Vector3d lhs, Vector3d rhs)
	{
		throw null;
	}

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
	public void Zero()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsZero()
	{
		throw null;
	}
}
