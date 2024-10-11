using System.Runtime.CompilerServices;

namespace UnityEngine;

public struct Vector2d
{
	public double x;

	public double y;

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

	public Vector2d normalized
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

	public static Vector2d zero
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Vector2d one
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Vector2d up
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Vector2d right
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector2d(double x, double y)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector2d Lerp(Vector2d from, Vector2d to, double t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector2d MoveTowards(Vector2d current, Vector2d target, double maxDistanceDelta)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector2d Scale(Vector2d a, Vector2d b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Scale(Vector2d scale)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Normalize()
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
	public static double Dot(Vector2d lhs, Vector2d rhs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double Angle(Vector2d from, Vector2d to)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double Distance(Vector2d a, Vector2d b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector2d ClampMagnitude(Vector2d vector, double maxLength)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double SqrMagnitude(Vector2d a)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double SqrMagnitude()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector2d Min(Vector2d lhs, Vector2d rhs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector2d Max(Vector2d lhs, Vector2d rhs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector2d operator +(Vector2d a, Vector2d b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector2d operator -(Vector2d a, Vector2d b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector2d operator -(Vector2d a)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector2d operator *(Vector2d a, double d)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector2d operator *(double d, Vector2d a)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector2d operator /(Vector2d a, double d)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool operator ==(Vector2d lhs, Vector2d rhs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool operator !=(Vector2d lhs, Vector2d rhs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static implicit operator Vector2d(Vector3 v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static implicit operator Vector3d(Vector2d v)
	{
		throw null;
	}
}
