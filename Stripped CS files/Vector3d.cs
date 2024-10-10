using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public struct Vector3d
{
	public double x;

	public double y;

	public double z;

	public Vector3d xzy => new Vector3d(x, z, y);

	public double this[int index]
	{
		get
		{
			return index switch
			{
				0 => x, 
				1 => y, 
				2 => z, 
				_ => throw new IndexOutOfRangeException("Invalid Vector3 index!"), 
			};
		}
		set
		{
			switch (index)
			{
			default:
				throw new IndexOutOfRangeException("Invalid Vector3 index!");
			case 0:
				x = value;
				break;
			case 1:
				y = value;
				break;
			case 2:
				z = value;
				break;
			}
		}
	}

	public Vector3d normalized => Normalize(this);

	public double magnitude => Math.Sqrt(x * x + y * y + z * z);

	public double sqrMagnitude => x * x + y * y + z * z;

	public static Vector3d zero => new Vector3d(0.0, 0.0, 0.0);

	public static Vector3d one => new Vector3d(1.0, 1.0, 1.0);

	public static Vector3d forward => new Vector3d(0.0, 0.0, 1.0);

	public static Vector3d back => new Vector3d(0.0, 0.0, -1.0);

	public static Vector3d up => new Vector3d(0.0, 1.0, 0.0);

	public static Vector3d down => new Vector3d(0.0, -1.0, 0.0);

	public static Vector3d left => new Vector3d(-1.0, 0.0, 0.0);

	public static Vector3d right => new Vector3d(1.0, 0.0, 0.0);

	[Obsolete("Use Vector3.forward instead.")]
	public static Vector3d fwd => new Vector3d(0.0, 0.0, 1.0);

	public Vector3d(double x, double y, double z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public Vector3d(double x, double y)
	{
		this.x = x;
		this.y = y;
		z = 0.0;
	}

	public void Swizzle()
	{
		double num = y;
		y = z;
		z = num;
	}

	public static implicit operator Vector3(Vector3d v)
	{
		return new Vector3((float)v.x, (float)v.y, (float)v.z);
	}

	public static implicit operator Vector3d(Vector3 v)
	{
		return new Vector3d(v.x, v.y, v.z);
	}

	public static Vector3d Lerp(Vector3d from, Vector3d to, double t)
	{
		t = Math.Max(0.0, Math.Min(t, 1.0));
		return new Vector3d(from.x + (to.x - from.x) * t, from.y + (to.y - from.y) * t, from.z + (to.z - from.z) * t);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern Vector3d Slerp(Vector3d from, Vector3d to, float t);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void Internal_OrthoNormalize2(ref Vector3d a, ref Vector3d b);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void Internal_OrthoNormalize3(ref Vector3d a, ref Vector3d b, ref Vector3d c);

	public static void OrthoNormalize(ref Vector3d normal, ref Vector3d tangent)
	{
		Internal_OrthoNormalize2(ref normal, ref tangent);
	}

	public static void OrthoNormalize(ref Vector3d normal, ref Vector3d tangent, ref Vector3d binormal)
	{
		Internal_OrthoNormalize3(ref normal, ref tangent, ref binormal);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern Vector3d RotateTowards(Vector3d from, Vector3d to, float maxRadiansDelta, float maxMagnitudeDelta);

	public static Vector3d Scale(Vector3d a, Vector3d b)
	{
		return new Vector3d(a.x * b.x, a.y * b.y, a.z * b.z);
	}

	public void Scale(Vector3d scale)
	{
		x *= scale.x;
		y *= scale.y;
		z *= scale.z;
	}

	public static Vector3d Cross(Vector3d lhs, Vector3d rhs)
	{
		return new Vector3d(lhs.y * rhs.z - lhs.z * rhs.y, lhs.z * rhs.x - lhs.x * rhs.z, lhs.x * rhs.y - lhs.y * rhs.x);
	}

	public override int GetHashCode()
	{
		return x.GetHashCode() ^ (y.GetHashCode() << 2) ^ (z.GetHashCode() >> 2);
	}

	public override bool Equals(object other)
	{
		if (!(other is Vector3))
		{
			return false;
		}
		Vector3d vector3d = (Vector3d)other;
		if (x.Equals(vector3d.x) && y.Equals(vector3d.y))
		{
			return z.Equals(vector3d.z);
		}
		return false;
	}

	public static Vector3d Reflect(Vector3d inDirection, Vector3d inNormal)
	{
		return -2.0 * Dot(inNormal, inDirection) * inNormal + inDirection;
	}

	public static Vector3d Normalize(Vector3d value)
	{
		double num = Magnitude(value);
		if (num > 0.0)
		{
			return value / num;
		}
		return zero;
	}

	public void Normalize()
	{
		double num = Magnitude(this);
		if (num > 0.0)
		{
			this /= num;
		}
		else
		{
			this = zero;
		}
	}

	public static double Dot(Vector3d lhs, Vector3d rhs)
	{
		return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
	}

	public static Vector3d Project(Vector3d vector, Vector3d onNormal)
	{
		double num = Dot(onNormal, onNormal);
		if (num < 1.401298464324817E-45)
		{
			return zero;
		}
		return onNormal * Dot(vector, onNormal) / num;
	}

	public static Vector3d Exclude(Vector3d excludeThis, Vector3d fromThat)
	{
		return fromThat - Project(fromThat, excludeThis);
	}

	public static double Angle(Vector3d from, Vector3d to)
	{
		return Math.Acos(Math.Min(Math.Max(Dot(from.normalized, to.normalized), -1.0), 1.0)) * (180.0 / Math.PI);
	}

	public static double Distance(Vector3d a, Vector3d b)
	{
		Vector3d vector3d = new Vector3d(a.x - b.x, a.y - b.y, a.z - b.z);
		return Math.Sqrt(vector3d.x * vector3d.x + vector3d.y * vector3d.y + vector3d.z * vector3d.z);
	}

	public static double Magnitude(Vector3d a)
	{
		return Math.Sqrt(a.x * a.x + a.y * a.y + a.z * a.z);
	}

	public static double SqrMagnitude(Vector3d a)
	{
		return a.x * a.x + a.y * a.y + a.z * a.z;
	}

	public static Vector3d Min(Vector3d lhs, Vector3d rhs)
	{
		return new Vector3d(Math.Min(lhs.x, rhs.x), Math.Min(lhs.y, rhs.y), Math.Min(lhs.z, rhs.z));
	}

	public static Vector3d Max(Vector3d lhs, Vector3d rhs)
	{
		return new Vector3d(Math.Max(lhs.x, rhs.x), Math.Max(lhs.y, rhs.y), Math.Max(lhs.z, rhs.z));
	}

	public Vector3d Basis(Vector3d vector3d_0, Vector3d vector3d_1, Vector3d vector3d_2)
	{
		return x * vector3d_0 + y * vector3d_1 + z * vector3d_2;
	}

	[Obsolete("Use Vector3.Angle instead. AngleBetween uses radians instead of degrees and was deprecated for this reason")]
	public static double AngleBetween(Vector3d from, Vector3d to)
	{
		return Math.Acos(Math.Min(Math.Max(Dot(from.normalized, to.normalized), -1.0), 1.0));
	}

	public static Vector3d operator +(Vector3d a, Vector3d b)
	{
		return new Vector3d(a.x + b.x, a.y + b.y, a.z + b.z);
	}

	public static Vector3d operator +(Vector3 a, Vector3d b)
	{
		return new Vector3d((double)a.x + b.x, (double)a.y + b.y, (double)a.z + b.z);
	}

	public static Vector3d operator +(Vector3d a, Vector3 b)
	{
		return new Vector3d(a.x + (double)b.x, a.y + (double)b.y, a.z + (double)b.z);
	}

	public static Vector3d operator -(Vector3d a, Vector3d b)
	{
		return new Vector3d(a.x - b.x, a.y - b.y, a.z - b.z);
	}

	public static Vector3d operator -(Vector3 a, Vector3d b)
	{
		return new Vector3d((double)a.x - b.x, (double)a.y - b.y, (double)a.z - b.z);
	}

	public static Vector3d operator -(Vector3d a, Vector3 b)
	{
		return new Vector3d(a.x - (double)b.x, a.y - (double)b.y, a.z - (double)b.z);
	}

	public static Vector3d operator -(Vector3d a)
	{
		return new Vector3d(0.0 - a.x, 0.0 - a.y, 0.0 - a.z);
	}

	public static Vector3d operator *(Vector3d a, double d)
	{
		return new Vector3d(a.x * d, a.y * d, a.z * d);
	}

	public static Vector3d operator *(double d, Vector3d a)
	{
		return new Vector3d(a.x * d, a.y * d, a.z * d);
	}

	public static Vector3d operator /(Vector3d a, double d)
	{
		return new Vector3d(a.x / d, a.y / d, a.z / d);
	}

	public static bool operator ==(Vector3d lhs, Vector3d rhs)
	{
		if (lhs.x == rhs.x && lhs.y == rhs.y)
		{
			return lhs.z == rhs.z;
		}
		return false;
	}

	public static bool operator !=(Vector3d lhs, Vector3d rhs)
	{
		if (lhs.x == rhs.x && lhs.y == rhs.y)
		{
			return lhs.z != rhs.z;
		}
		return true;
	}

	public override string ToString()
	{
		return "[" + x + ", " + y + ", " + z + "]";
	}

	public string ToString(string format)
	{
		return "[" + x.ToString(format) + ", " + y.ToString(format) + ", " + z.ToString(format) + "]";
	}

	public void Zero()
	{
		x = 0.0;
		y = 0.0;
		z = 0.0;
	}

	public bool IsZero()
	{
		if (x == 0.0 && y == 0.0)
		{
			return z == 0.0;
		}
		return false;
	}
}
