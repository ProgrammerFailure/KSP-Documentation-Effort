using System;

namespace UnityEngine;

public struct Vector4d
{
	public double x;

	public double y;

	public double z;

	public double w;

	public double this[int index]
	{
		get
		{
			return index switch
			{
				0 => x, 
				1 => y, 
				2 => z, 
				3 => w, 
				_ => throw new IndexOutOfRangeException("Invalid Vector4d index!"), 
			};
		}
		set
		{
			switch (index)
			{
			default:
				throw new IndexOutOfRangeException("Invalid Vector4d index!");
			case 0:
				x = value;
				break;
			case 1:
				y = value;
				break;
			case 2:
				z = value;
				break;
			case 3:
				w = value;
				break;
			}
		}
	}

	public Vector4d normalized => Normalize(this);

	public double magnitude => Math.Sqrt(Dot(this, this));

	public double sqrMagnitude => Dot(this, this);

	public static Vector4d zero => new Vector4d(0.0, 0.0, 0.0, 0.0);

	public static Vector4d one => new Vector4d(1.0, 1.0, 1.0, 1.0);

	public Vector4d(double x, double y, double z, double w)
	{
		this.x = x;
		this.y = y;
		this.z = z;
		this.w = w;
	}

	public Vector4d(double x, double y, double z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
		w = 0.0;
	}

	public Vector4d(double x, double y)
	{
		this.x = x;
		this.y = y;
		z = 0.0;
		w = 0.0;
	}

	public static implicit operator Vector4(Vector4d q)
	{
		return new Vector4((float)q.x, (float)q.y, (float)q.z, (float)q.w);
	}

	public static implicit operator Vector4d(Vector4 q)
	{
		return new Vector4d(q.x, q.y, q.z, q.w);
	}

	public static Vector4d Lerp(Vector4d from, Vector4d to, double t)
	{
		t = Math.Min(1.0, Math.Max(0.0, t));
		return new Vector4d(from.x + (to.x - from.x) * t, from.y + (to.y - from.y) * t, from.z + (to.z - from.z) * t, from.w + (to.w - from.w) * t);
	}

	public static Vector4d MoveTowards(Vector4d current, Vector4d target, double maxDistanceDelta)
	{
		Vector4d vector4d = target - current;
		double num = vector4d.magnitude;
		if (!(num <= maxDistanceDelta) && num != 0.0)
		{
			return current + vector4d / num * maxDistanceDelta;
		}
		return target;
	}

	public static Vector4d Scale(Vector4d a, Vector4d b)
	{
		return new Vector4d(a.x * b.x, a.y * b.y, a.z * b.z, a.w * b.w);
	}

	public void Scale(Vector4d scale)
	{
		x *= scale.x;
		y *= scale.y;
		z *= scale.z;
		w *= scale.w;
	}

	public override int GetHashCode()
	{
		return x.GetHashCode() ^ (y.GetHashCode() << 2) ^ (z.GetHashCode() >> 2) ^ (w.GetHashCode() >> 1);
	}

	public override bool Equals(object other)
	{
		if (!(other is Vector4d vector4d))
		{
			return false;
		}
		if (x.Equals(vector4d.x) && y.Equals(vector4d.y) && z.Equals(vector4d.z))
		{
			return w.Equals(vector4d.w);
		}
		return false;
	}

	public static Vector4d Normalize(Vector4d a)
	{
		double num = Magnitude(a);
		if (num > 0.0)
		{
			return a / num;
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

	public override string ToString()
	{
		return $"({x:F1}, {y:F1}, {z:F1}, {w:F1})";
	}

	public string ToString(string format)
	{
		return $"({x.ToString(format)}, {y.ToString(format)}, {z.ToString(format)}, {w.ToString(format)})";
	}

	public static double Dot(Vector4d a, Vector4d b)
	{
		return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
	}

	public static Vector4d Project(Vector4d a, Vector4d b)
	{
		return b * Dot(a, b) / Dot(b, b);
	}

	public static double Distance(Vector4d a, Vector4d b)
	{
		return Magnitude(a - b);
	}

	public static double Magnitude(Vector4d a)
	{
		return Math.Sqrt(Dot(a, a));
	}

	public static double SqrMagnitude(Vector4d a)
	{
		return Dot(a, a);
	}

	public double SqrMagnitude()
	{
		return Dot(this, this);
	}

	public static Vector4d Min(Vector4d lhs, Vector4d rhs)
	{
		return new Vector4d(Math.Min(lhs.x, rhs.x), Math.Min(lhs.y, rhs.y), Math.Min(lhs.z, rhs.z), Math.Min(lhs.w, rhs.w));
	}

	public static Vector4d Max(Vector4d lhs, Vector4d rhs)
	{
		return new Vector4d(Math.Max(lhs.x, rhs.x), Math.Max(lhs.y, rhs.y), Math.Max(lhs.z, rhs.z), Math.Max(lhs.w, rhs.w));
	}

	public static Vector4d operator +(Vector4d a, Vector4d b)
	{
		return new Vector4d(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
	}

	public static Vector4d operator -(Vector4d a, Vector4d b)
	{
		return new Vector4d(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
	}

	public static Vector4d operator -(Vector4d a)
	{
		return new Vector4d(0.0 - a.x, 0.0 - a.y, 0.0 - a.z, 0.0 - a.w);
	}

	public static Vector4d operator *(Vector4d a, double d)
	{
		return new Vector4d(a.x * d, a.y * d, a.z * d, a.w * d);
	}

	public static Vector4d operator *(double d, Vector4d a)
	{
		return new Vector4d(a.x * d, a.y * d, a.z * d, a.w * d);
	}

	public static Vector4d operator /(Vector4d a, double d)
	{
		return new Vector4d(a.x / d, a.y / d, a.z / d, a.w / d);
	}

	public static bool operator ==(Vector4d lhs, Vector4d rhs)
	{
		if (lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z)
		{
			return lhs.w == rhs.w;
		}
		return false;
	}

	public static bool operator !=(Vector4d lhs, Vector4d rhs)
	{
		if (lhs.x != rhs.x && lhs.y != rhs.y && lhs.z != rhs.z)
		{
			return lhs.w != rhs.w;
		}
		return false;
	}

	public static implicit operator Vector4d(Vector3d v)
	{
		return new Vector4d(v.x, v.y, v.z, 0.0);
	}

	public static implicit operator Vector3d(Vector4d v)
	{
		return new Vector3d(v.x, v.y, v.z);
	}
}
