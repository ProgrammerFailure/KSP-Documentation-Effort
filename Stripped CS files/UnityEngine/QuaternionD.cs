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
		get
		{
			return index switch
			{
				0 => x, 
				1 => y, 
				2 => z, 
				3 => w, 
				_ => throw new IndexOutOfRangeException("Invalid QuaternionD index!"), 
			};
		}
		set
		{
			switch (index)
			{
			default:
				throw new IndexOutOfRangeException("Invalid QuaternionD index!");
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

	public QuaternionD swizzle => new QuaternionD(0.0 - x, 0.0 - z, 0.0 - y, w);

	public static QuaternionD identity => new QuaternionD(0.0, 0.0, 0.0, 1.0);

	public Vector3d eulerAngles
	{
		get
		{
			return Internal_ToEulerRad(this) * (180.0 / Math.PI);
		}
		set
		{
			this = Internal_FromEulerRad(value * (Math.PI / 180.0));
		}
	}

	public QuaternionD(double x, double y, double z, double w)
	{
		this.x = x;
		this.y = y;
		this.z = z;
		this.w = w;
	}

	public QuaternionD(Vector3d vector3d_0, Vector3d vector3d_1, Vector3d vector3d_2)
	{
		double num = vector3d_0.x;
		double num2 = vector3d_0.y;
		double num3 = vector3d_0.z;
		double num4 = vector3d_1.x;
		double num5 = vector3d_1.y;
		double num6 = vector3d_1.z;
		double num7 = vector3d_2.x;
		double num8 = vector3d_2.y;
		double num9 = vector3d_2.z;
		if (num + num5 + num9 >= 0.0)
		{
			double num10 = num + num5 + num9 + 1.0;
			double num11 = 0.5 / Math.Sqrt(num10);
			w = num10 * num11;
			z = (num2 - num4) * num11;
			y = (num7 - num3) * num11;
			x = (num6 - num8) * num11;
		}
		else if (num > num5 && num > num9)
		{
			double num12 = num - num5 - num9 + 1.0;
			double num13 = 0.5 / Math.Sqrt(num12);
			x = num12 * num13;
			y = (num2 + num4) * num13;
			z = (num7 + num3) * num13;
			w = (num6 - num8) * num13;
		}
		else if (num5 > num9)
		{
			double num14 = 0.0 - num + num5 - num9 + 1.0;
			double num15 = 0.5 / Math.Sqrt(num14);
			y = num14 * num15;
			x = (num2 + num4) * num15;
			w = (num7 - num3) * num15;
			z = (num6 + num8) * num15;
		}
		else
		{
			double num16 = 0.0 - num - num5 + num9 + 1.0;
			double num17 = 0.5 / Math.Sqrt(num16);
			z = num16 * num17;
			w = (num2 - num4) * num17;
			x = (num7 + num3) * num17;
			y = (num6 + num8) * num17;
		}
	}

	public void FrameVectors(out Vector3d frameX, out Vector3d frameY, out Vector3d frameZ)
	{
		frameX = new Vector3d(1.0 - 2.0 * y * y - 2.0 * z * z, 2.0 * x * y + 2.0 * w * z, 2.0 * x * z - 2.0 * w * y);
		frameY = new Vector3d(2.0 * x * y - 2.0 * w * z, 1.0 - 2.0 * x * x - 2.0 * z * z, 2.0 * y * z + 2.0 * w * x);
		frameZ = new Vector3d(2.0 * x * z + 2.0 * w * y, 2.0 * y * z - 2.0 * w * x, 1.0 - 2.0 * x * x - 2.0 * y * y);
	}

	public static implicit operator Quaternion(QuaternionD q)
	{
		return new Quaternion((float)q.x, (float)q.y, (float)q.z, (float)q.w);
	}

	public static implicit operator QuaternionD(Quaternion q)
	{
		return new QuaternionD(q.x, q.y, q.z, q.w);
	}

	public static double Dot(QuaternionD a, QuaternionD b)
	{
		return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
	}

	public static QuaternionD AngleAxis(double angle, Vector3d axis)
	{
		double magnitude = axis.magnitude;
		double num3;
		double num4;
		double num5;
		double num6;
		if (magnitude > 0.0001)
		{
			double num = Math.Cos(angle * (Math.PI / 180.0) / 2.0);
			double num2 = Math.Sin(angle * (Math.PI / 180.0) / 2.0);
			num3 = axis.x / magnitude * num2;
			num4 = axis.y / magnitude * num2;
			num5 = axis.z / magnitude * num2;
			num6 = num;
		}
		else
		{
			num6 = 1.0;
			num3 = 0.0;
			num4 = 0.0;
			num5 = 0.0;
		}
		return new QuaternionD(num3, num4, num5, num6);
	}

	public void ToAngleAxis(out double angle, out Vector3d axis)
	{
		Internal_ToAxisAngleRad(this, out axis, out angle);
		angle *= 180.0 / Math.PI;
	}

	public static QuaternionD FromToRotation(Vector3d fromDirection, Vector3d toDirection)
	{
		return INTERNAL_CALL_FromToRotation(ref fromDirection, ref toDirection);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern QuaternionD INTERNAL_CALL_FromToRotation(ref Vector3d fromDirection, ref Vector3d toDirection);

	public void SetFromToRotation(Vector3d fromDirection, Vector3d toDirection)
	{
		this = FromToRotation(fromDirection, toDirection);
	}

	public static QuaternionD LookRotation(Vector3d forward, Vector3d up)
	{
		QuaternionD result = default(QuaternionD);
		forward = Vector3d.Normalize(forward);
		Vector3d rhs = Vector3d.Normalize(Vector3d.Cross(up, forward));
		up = Vector3d.Cross(forward, rhs);
		double num = forward.x;
		double num2 = forward.y;
		double num3 = forward.z;
		double num4 = up.x;
		double num5 = up.y;
		double num6 = up.z;
		double num7 = rhs.x;
		double num8 = rhs.y;
		double num9 = rhs.z;
		double num10 = num7 + num5 + num3;
		if (num10 > 0.0)
		{
			double num11 = Math.Sqrt(num10 + 1.0);
			result.w = num11 * 0.5;
			num11 = 0.5 / num11;
			result.x = (num6 - num2) * num11;
			result.y = (num - num9) * num11;
			result.z = (num8 - num4) * num11;
			return result;
		}
		if (num7 >= num5 && num7 >= num3)
		{
			double num12 = Math.Sqrt(1.0 + num7 - num5 - num3);
			double num13 = 0.5 / num12;
			result.x = 0.5 * num12;
			result.y = (num8 + num4) * num13;
			result.z = (num9 + num) * num13;
			result.w = (num6 - num2) * num13;
			return result;
		}
		if (num5 > num3)
		{
			double num14 = Math.Sqrt(1.0 + num5 - num7 - num3);
			double num15 = 0.5 / num14;
			result.x = (num4 + num8) * num15;
			result.y = 0.5 * num14;
			result.z = (num2 + num6) * num15;
			result.w = (num - num9) * num15;
			return result;
		}
		double num16 = Math.Sqrt(1.0 + num3 - num7 - num5);
		double num17 = 0.5 / num16;
		result.x = (num + num9) * num17;
		result.y = (num2 + num6) * num17;
		result.z = 0.5 * num16;
		result.w = (num8 - num4) * num17;
		return result;
	}

	public static QuaternionD LookRotation(Vector3d forward)
	{
		Vector3d up = Vector3d.up;
		return LookRotation(forward, up);
	}

	public void SetLookRotation(Vector3d view)
	{
		Vector3d up = Vector3d.up;
		SetLookRotation(view, up);
	}

	public void SetLookRotation(Vector3d view, Vector3d up)
	{
		this = LookRotation(view, up);
	}

	public static QuaternionD Slerp(QuaternionD from, QuaternionD to, double t)
	{
		return INTERNAL_CALL_Slerp(ref from, ref to, t);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern QuaternionD INTERNAL_CALL_Slerp(ref QuaternionD from, ref QuaternionD to, double t);

	public static QuaternionD Lerp(QuaternionD from, QuaternionD to, double t)
	{
		return INTERNAL_CALL_Lerp(ref from, ref to, t);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern QuaternionD INTERNAL_CALL_Lerp(ref QuaternionD from, ref QuaternionD to, double t);

	public static QuaternionD RotateTowards(QuaternionD from, QuaternionD to, double maxDegreesDelta)
	{
		double t = Math.Min(1.0, maxDegreesDelta / Angle(from, to));
		return UnclampedSlerp(from, to, t);
	}

	public static QuaternionD UnclampedSlerp(QuaternionD from, QuaternionD to, double t)
	{
		return INTERNAL_CALL_UnclampedSlerp(ref from, ref to, t);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern QuaternionD INTERNAL_CALL_UnclampedSlerp(ref QuaternionD from, ref QuaternionD to, double t);

	public static QuaternionD Inverse(QuaternionD q)
	{
		return new QuaternionD(0.0 - q.x, 0.0 - q.y, 0.0 - q.z, q.w);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern QuaternionD INTERNAL_CALL_Inverse(ref QuaternionD rotation);

	public override string ToString()
	{
		return $"({x:F1}, {y:F1}, {z:F1}, {w:F1})";
	}

	public string ToString(string format)
	{
		return $"({x.ToString(format)}, {y.ToString(format)}, {z.ToString(format)}, {w.ToString(format)})";
	}

	public static double Angle(QuaternionD a, QuaternionD b)
	{
		return Math.Acos(Math.Min(Math.Abs(Dot(a, b)), 1.0)) * 2.0 * (180.0 / Math.PI);
	}

	public static QuaternionD Euler(double x, double y, double z)
	{
		return Internal_FromEulerRad(new Vector3d(x, y, z) * (Math.PI / 180.0));
	}

	public static QuaternionD Euler(Vector3d euler)
	{
		return Internal_FromEulerRad(euler * (Math.PI / 180.0));
	}

	public static Vector3d Internal_ToEulerRad(QuaternionD rotation)
	{
		return INTERNAL_CALL_Internal_ToEulerRad(ref rotation);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern Vector3d INTERNAL_CALL_Internal_ToEulerRad(ref QuaternionD rotation);

	public static QuaternionD Internal_FromEulerRad(Vector3d euler)
	{
		return INTERNAL_CALL_Internal_FromEulerRad(ref euler);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern QuaternionD INTERNAL_CALL_Internal_FromEulerRad(ref Vector3d euler);

	public static void Internal_ToAxisAngleRad(QuaternionD q, out Vector3d axis, out double angle)
	{
		INTERNAL_CALL_Internal_ToAxisAngleRad(ref q, out axis, out angle);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void INTERNAL_CALL_Internal_ToAxisAngleRad(ref QuaternionD q, out Vector3d axis, out double angle);

	[Obsolete("Use QuaternionD.Euler instead. This function was deprecated because it uses radians instad of degrees")]
	public static QuaternionD EulerRotation(double x, double y, double z)
	{
		return Internal_FromEulerRad(new Vector3d(x, y, z));
	}

	[Obsolete("Use QuaternionD.Euler instead. This function was deprecated because it uses radians instad of degrees")]
	public static QuaternionD EulerRotation(Vector3d euler)
	{
		return Internal_FromEulerRad(euler);
	}

	[Obsolete("Use QuaternionD.Euler instead. This function was deprecated because it uses radians instad of degrees")]
	public void SetEulerRotation(double x, double y, double z)
	{
		this = Internal_FromEulerRad(new Vector3d(x, y, z));
	}

	[Obsolete("Use QuaternionD.Euler instead. This function was deprecated because it uses radians instad of degrees")]
	public void SetEulerRotation(Vector3d euler)
	{
		this = Internal_FromEulerRad(euler);
	}

	[Obsolete("Use QuaternionD.eulerAngles instead. This function was deprecated because it uses radians instad of degrees")]
	public Vector3d ToEuler()
	{
		return Internal_ToEulerRad(this);
	}

	[Obsolete("Use QuaternionD.Euler instead. This function was deprecated because it uses radians instad of degrees")]
	public static QuaternionD EulerAngles(double x, double y, double z)
	{
		return Internal_FromEulerRad(new Vector3d(x, y, z));
	}

	[Obsolete("Use QuaternionD.Euler instead. This function was deprecated because it uses radians instad of degrees")]
	public static QuaternionD EulerAngles(Vector3d euler)
	{
		return Internal_FromEulerRad(euler);
	}

	[Obsolete("Use QuaternionD.ToAngleAxis instead. This function was deprecated because it uses radians instad of degrees")]
	public void ToAxisAngle(out Vector3d axis, out double angle)
	{
		Internal_ToAxisAngleRad(this, out axis, out angle);
	}

	[Obsolete("Use QuaternionD.Euler instead. This function was deprecated because it uses radians instad of degrees")]
	public void SetEulerAngles(double x, double y, double z)
	{
		SetEulerRotation(new Vector3d(x, y, z));
	}

	[Obsolete("Use QuaternionD.Euler instead. This function was deprecated because it uses radians instad of degrees")]
	public void SetEulerAngles(Vector3d euler)
	{
		this = EulerRotation(euler);
	}

	[Obsolete("Use QuaternionD.eulerAngles instead. This function was deprecated because it uses radians instad of degrees")]
	public static Vector3d ToEulerAngles(QuaternionD rotation)
	{
		return Internal_ToEulerRad(rotation);
	}

	[Obsolete("Use QuaternionD.eulerAngles instead. This function was deprecated because it uses radians instad of degrees")]
	public Vector3d ToEulerAngles()
	{
		return Internal_ToEulerRad(this);
	}

	[Obsolete("Use QuaternionD.AngleAxis instead. This function was deprecated because it uses radians instad of degrees")]
	public static QuaternionD AxisAngle(Vector3d axis, double angle)
	{
		return INTERNAL_CALL_AxisAngle(ref axis, angle);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern QuaternionD INTERNAL_CALL_AxisAngle(ref Vector3d axis, double angle);

	[Obsolete("Use QuaternionD.AngleAxis instead. This function was deprecated because it uses radians instad of degrees")]
	public void SetAxisAngle(Vector3d axis, double angle)
	{
		this = AxisAngle(axis, angle);
	}

	public override int GetHashCode()
	{
		return x.GetHashCode() ^ (y.GetHashCode() << 2) ^ (z.GetHashCode() >> 2) ^ (w.GetHashCode() >> 1);
	}

	public override bool Equals(object other)
	{
		if (!(other is QuaternionD quaternionD))
		{
			return false;
		}
		if (x.Equals(quaternionD.x) && y.Equals(quaternionD.y) && z.Equals(quaternionD.z))
		{
			return w.Equals(quaternionD.w);
		}
		return false;
	}

	public static QuaternionD operator *(QuaternionD lhs, QuaternionD rhs)
	{
		return new QuaternionD(lhs.w * rhs.x + lhs.x * rhs.w + lhs.y * rhs.z - lhs.z * rhs.y, lhs.w * rhs.y + lhs.y * rhs.w + lhs.z * rhs.x - lhs.x * rhs.z, lhs.w * rhs.z + lhs.z * rhs.w + lhs.x * rhs.y - lhs.y * rhs.x, lhs.w * rhs.w - lhs.x * rhs.x - lhs.y * rhs.y - lhs.z * rhs.z);
	}

	public static Vector3d operator *(QuaternionD rotation, Vector3d point)
	{
		double num = rotation.x * 2.0;
		double num2 = rotation.y * 2.0;
		double num3 = rotation.z * 2.0;
		double num4 = rotation.x * num;
		double num5 = rotation.y * num2;
		double num6 = rotation.z * num3;
		double num7 = rotation.x * num2;
		double num8 = rotation.x * num3;
		double num9 = rotation.y * num3;
		double num10 = rotation.w * num;
		double num11 = rotation.w * num2;
		double num12 = rotation.w * num3;
		Vector3d result = default(Vector3d);
		result.x = (1.0 - (num5 + num6)) * point.x + (num7 - num12) * point.y + (num8 + num11) * point.z;
		result.y = (num7 + num12) * point.x + (1.0 - (num4 + num6)) * point.y + (num9 - num10) * point.z;
		result.z = (num8 - num11) * point.x + (num9 + num10) * point.y + (1.0 - (num4 + num5)) * point.z;
		return result;
	}

	public static bool operator ==(QuaternionD lhs, QuaternionD rhs)
	{
		if (lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z)
		{
			return lhs.w == rhs.w;
		}
		return false;
	}

	public static bool operator !=(QuaternionD lhs, QuaternionD rhs)
	{
		if (lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z)
		{
			return lhs.w != rhs.w;
		}
		return true;
	}
}
