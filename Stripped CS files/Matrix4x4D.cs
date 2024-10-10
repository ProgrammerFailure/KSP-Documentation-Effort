using System;
using UnityEngine;

public struct Matrix4x4D
{
	public double m00;

	public double m01;

	public double m02;

	public double m03;

	public double m10;

	public double m11;

	public double m12;

	public double m13;

	public double m20;

	public double m21;

	public double m22;

	public double m23;

	public double m30;

	public double m31;

	public double m32;

	public double m33;

	public Matrix4x4D(double p00, double p01, double p02, double p03, double p10, double p11, double p12, double p13, double p20, double p21, double p22, double p23, double p30, double p31, double p32, double p33)
	{
		m00 = p00;
		m01 = p01;
		m02 = p02;
		m03 = p03;
		m10 = p10;
		m11 = p11;
		m12 = p12;
		m13 = p13;
		m20 = p20;
		m21 = p21;
		m22 = p22;
		m23 = p23;
		m30 = p30;
		m31 = p31;
		m32 = p32;
		m33 = p33;
	}

	public Matrix4x4D(Matrix4x4 m)
	{
		m00 = m.m00;
		m01 = m.m01;
		m02 = m.m02;
		m03 = m.m03;
		m10 = m.m10;
		m11 = m.m11;
		m12 = m.m12;
		m13 = m.m13;
		m20 = m.m20;
		m21 = m.m21;
		m22 = m.m22;
		m23 = m.m23;
		m30 = m.m30;
		m31 = m.m31;
		m32 = m.m32;
		m33 = m.m33;
	}

	public double det2x2(double a, double b, double c, double d)
	{
		return a * d - b * c;
	}

	public double det3x3(double a1, double a2, double a3, double b1, double b2, double b3, double c1, double c2, double c3)
	{
		return a1 * det2x2(b2, b3, c2, c3) - b1 * det2x2(a2, a3, c2, c3) + c1 * det2x2(a2, a3, b2, b3);
	}

	public Matrix4x4D Translate(double dx, double dy, double dz)
	{
		SetIdentity();
		m30 = dx;
		m31 = dy;
		m32 = dz;
		return this;
	}

	public Matrix4x4D RotateXaxis(double angle)
	{
		return RotateXaxis(Math.Sin(angle), Math.Cos(angle));
	}

	public Matrix4x4D RotateYaxis(double angle)
	{
		return RotateYaxis(Math.Sin(angle), Math.Cos(angle));
	}

	public Matrix4x4D RotateZaxis(double angle)
	{
		return RotateZaxis(Math.Sin(angle), Math.Cos(angle));
	}

	public Matrix4x4D RotateXaxis(double sina, double cosa)
	{
		SetIdentity();
		m11 = cosa;
		m12 = sina;
		m21 = 0.0 - sina;
		m22 = cosa;
		return this;
	}

	public Matrix4x4D RotateYaxis(double sina, double cosa)
	{
		SetIdentity();
		m00 = cosa;
		m02 = 0.0 - sina;
		m20 = sina;
		m22 = cosa;
		return this;
	}

	public Matrix4x4D RotateZaxis(double sina, double cosa)
	{
		SetIdentity();
		m00 = cosa;
		m01 = sina;
		m10 = 0.0 - sina;
		m11 = cosa;
		return this;
	}

	public Matrix4x4D Scale(double factor)
	{
		SetIdentity();
		m00 = factor;
		m11 = factor;
		m22 = factor;
		return this;
	}

	public Matrix4x4D Scale(double sx, double sy, double sz)
	{
		SetIdentity();
		m00 = sx;
		m11 = sy;
		m22 = sz;
		return this;
	}

	public static Matrix4x4D Identity()
	{
		return new Matrix4x4D(1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0);
	}

	public static explicit operator Matrix4x4D(Matrix4x4 m)
	{
		return new Matrix4x4D(m.m00, m.m01, m.m02, m.m03, m.m10, m.m11, m.m12, m.m13, m.m20, m.m21, m.m22, m.m23, m.m30, m.m31, m.m32, m.m33);
	}

	public Matrix4x4D Zero()
	{
		double num = 0.0;
		m03 = 0.0;
		double num2 = num;
		num = 0.0;
		m02 = num2;
		double num3 = num;
		num = 0.0;
		m01 = num3;
		m00 = num;
		num = 0.0;
		m13 = 0.0;
		double num4 = num;
		num = 0.0;
		m12 = num4;
		double num5 = num;
		num = 0.0;
		m11 = num5;
		m10 = num;
		num = 0.0;
		m23 = 0.0;
		double num6 = num;
		num = 0.0;
		m22 = num6;
		double num7 = num;
		num = 0.0;
		m21 = num7;
		m20 = num;
		num = 0.0;
		m33 = 0.0;
		double num8 = num;
		num = 0.0;
		m32 = num8;
		double num9 = num;
		num = 0.0;
		m31 = num9;
		m30 = num;
		return this;
	}

	public Matrix4x4D SetIdentity()
	{
		double num = 0.0;
		m03 = 0.0;
		double num2 = num;
		num = 0.0;
		m02 = num2;
		m01 = num;
		num = 0.0;
		m13 = 0.0;
		double num3 = num;
		num = 0.0;
		m12 = num3;
		m10 = num;
		num = 0.0;
		m23 = 0.0;
		double num4 = num;
		num = 0.0;
		m21 = num4;
		m20 = num;
		num = 0.0;
		m32 = 0.0;
		double num5 = num;
		num = 0.0;
		m31 = num5;
		m30 = num;
		num = 1.0;
		m33 = 1.0;
		double num6 = num;
		num = 1.0;
		m22 = num6;
		double num7 = num;
		num = 1.0;
		m11 = num7;
		m00 = num;
		return this;
	}

	public Matrix4x4D Inverse()
	{
		double num = Det();
		double a = m00;
		double num2 = m10;
		double num3 = m20;
		double c = m30;
		double num4 = m01;
		double num5 = m11;
		double num6 = m21;
		double num7 = m31;
		double num8 = m02;
		double num9 = m12;
		double num10 = m22;
		double num11 = m32;
		double a2 = m03;
		double num12 = m13;
		double num13 = m23;
		double c2 = m33;
		m00 = det3x3(num5, num9, num12, num6, num10, num13, num7, num11, c2) / num;
		m01 = (0.0 - det3x3(num4, num8, a2, num6, num10, num13, num7, num11, c2)) / num;
		m02 = det3x3(num4, num8, a2, num5, num9, num12, num7, num11, c2) / num;
		m03 = (0.0 - det3x3(num4, num8, a2, num5, num9, num12, num6, num10, num13)) / num;
		m10 = (0.0 - det3x3(num2, num9, num12, num3, num10, num13, c, num11, c2)) / num;
		m11 = det3x3(a, num8, a2, num3, num10, num13, c, num11, c2) / num;
		m12 = (0.0 - det3x3(a, num8, a2, num2, num9, num12, c, num11, c2)) / num;
		m13 = det3x3(a, num8, a2, num2, num9, num12, num3, num10, num13) / num;
		m20 = det3x3(num2, num5, num12, num3, num6, num13, c, num7, c2) / num;
		m21 = (0.0 - det3x3(a, num4, a2, num3, num6, num13, c, num7, c2)) / num;
		m22 = det3x3(a, num4, a2, num2, num5, num12, c, num7, c2) / num;
		m23 = (0.0 - det3x3(a, num4, a2, num2, num5, num12, num3, num6, num13)) / num;
		m30 = (0.0 - det3x3(num2, num5, num9, num3, num6, num10, c, num7, num11)) / num;
		m31 = det3x3(a, num4, num8, num3, num6, num10, c, num7, num11) / num;
		m32 = (0.0 - det3x3(a, num4, num8, num2, num5, num9, c, num7, num11)) / num;
		m33 = det3x3(a, num4, num8, num2, num5, num9, num3, num6, num10) / num;
		return this;
	}

	public double Det()
	{
		double num = m00;
		double num2 = m10;
		double num3 = m20;
		double num4 = m30;
		double a = m01;
		double num5 = m11;
		double num6 = m21;
		double c = m31;
		double a2 = m02;
		double num7 = m12;
		double num8 = m22;
		double c2 = m32;
		double a3 = m03;
		double num9 = m13;
		double num10 = m23;
		double c3 = m33;
		return num * det3x3(num5, num7, num9, num6, num8, num10, c, c2, c3) - num2 * det3x3(a, a2, a3, num6, num8, num10, c, c2, c3) + num3 * det3x3(a, a2, a3, num5, num7, num9, c, c2, c3) - num4 * det3x3(a, a2, a3, num5, num7, num9, num6, num8, num10);
	}

	public Vector3d TransformVector(Vector3d v)
	{
		return new Vector3d(v.x * m00 + v.y * m10 + v.z * m20, v.x * m01 + v.y * m11 + v.z * m21, v.x * m02 + v.y * m12 + v.z * m22);
	}

	public Vector3d TransformPoint(Vector3d v)
	{
		return new Vector3d(v.x * m00 + v.y * m10 + v.z * m20 + m30, v.x * m01 + v.y * m11 + v.z * m21 + m31, v.x * m02 + v.y * m12 + v.z * m22 + m32);
	}

	public Vector3d MultiplyPoint3x4(Vector3d v)
	{
		Vector3d result = default(Vector3d);
		result.x = m00 * v.x + m01 * v.y + m02 * v.z + m03;
		result.y = m10 * v.x + m11 * v.y + m12 * v.z + m13;
		result.z = m20 * v.x + m21 * v.y + m22 * v.z + m23;
		return result;
	}

	public static Matrix4x4D operator +(Matrix4x4D m1, Matrix4x4D m2)
	{
		Matrix4x4D result = default(Matrix4x4D);
		result.m00 = m1.m00 + m2.m00;
		result.m01 = m1.m01 + m2.m01;
		result.m02 = m1.m02 + m2.m02;
		result.m03 = m1.m03 + m2.m03;
		result.m10 = m1.m10 + m2.m10;
		result.m11 = m1.m11 + m2.m11;
		result.m12 = m1.m12 + m2.m12;
		result.m13 = m1.m13 + m2.m13;
		result.m20 = m1.m20 + m2.m20;
		result.m21 = m1.m21 + m2.m21;
		result.m22 = m1.m22 + m2.m22;
		result.m23 = m1.m23 + m2.m23;
		result.m30 = m1.m30 + m2.m30;
		result.m31 = m1.m31 + m2.m31;
		result.m32 = m1.m32 + m2.m32;
		result.m33 = m1.m33 + m2.m33;
		return result;
	}

	public static Matrix4x4D operator -(Matrix4x4D m1, Matrix4x4D m2)
	{
		Matrix4x4D result = default(Matrix4x4D);
		result.m00 = m1.m00 - m2.m00;
		result.m01 = m1.m01 - m2.m01;
		result.m02 = m1.m02 - m2.m02;
		result.m03 = m1.m03 - m2.m03;
		result.m10 = m1.m10 - m2.m10;
		result.m11 = m1.m11 - m2.m11;
		result.m12 = m1.m12 - m2.m12;
		result.m13 = m1.m13 - m2.m13;
		result.m20 = m1.m20 - m2.m20;
		result.m21 = m1.m21 - m2.m21;
		result.m22 = m1.m22 - m2.m22;
		result.m23 = m1.m23 - m2.m23;
		result.m30 = m1.m30 - m2.m30;
		result.m31 = m1.m31 - m2.m31;
		result.m32 = m1.m32 - m2.m32;
		result.m33 = m1.m33 - m2.m33;
		return result;
	}

	public static Matrix4x4D operator *(Matrix4x4D m1, Matrix4x4D m2)
	{
		Matrix4x4D result = default(Matrix4x4D);
		result.m00 = m1.m00 * m2.m00 + m1.m01 * m2.m10 + m1.m02 * m2.m20 + m1.m03 * m2.m30;
		result.m01 = m1.m00 * m2.m01 + m1.m01 * m2.m11 + m1.m02 * m2.m21 + m1.m03 * m2.m31;
		result.m02 = m1.m00 * m2.m02 + m1.m01 * m2.m12 + m1.m02 * m2.m22 + m1.m03 * m2.m32;
		result.m03 = m1.m00 * m2.m03 + m1.m01 * m2.m13 + m1.m02 * m2.m23 + m1.m03 * m2.m33;
		result.m10 = m1.m10 * m2.m00 + m1.m11 * m2.m10 + m1.m12 * m2.m20 + m1.m13 * m2.m30;
		result.m11 = m1.m10 * m2.m01 + m1.m11 * m2.m11 + m1.m12 * m2.m21 + m1.m13 * m2.m31;
		result.m12 = m1.m10 * m2.m02 + m1.m11 * m2.m12 + m1.m12 * m2.m22 + m1.m13 * m2.m32;
		result.m13 = m1.m10 * m2.m03 + m1.m11 * m2.m13 + m1.m12 * m2.m23 + m1.m13 * m2.m33;
		result.m20 = m1.m20 * m2.m00 + m1.m21 * m2.m10 + m1.m22 * m2.m20 + m1.m23 * m2.m30;
		result.m21 = m1.m20 * m2.m01 + m1.m21 * m2.m11 + m1.m22 * m2.m21 + m1.m23 * m2.m31;
		result.m22 = m1.m20 * m2.m02 + m1.m21 * m2.m12 + m1.m22 * m2.m22 + m1.m23 * m2.m32;
		result.m23 = m1.m20 * m2.m03 + m1.m21 * m2.m13 + m1.m22 * m2.m23 + m1.m23 * m2.m33;
		result.m30 = m1.m30 * m2.m00 + m1.m31 * m2.m10 + m1.m32 * m2.m20 + m1.m33 * m2.m30;
		result.m31 = m1.m30 * m2.m01 + m1.m31 * m2.m11 + m1.m32 * m2.m21 + m1.m33 * m2.m31;
		result.m32 = m1.m30 * m2.m02 + m1.m31 * m2.m12 + m1.m32 * m2.m22 + m1.m33 * m2.m32;
		result.m33 = m1.m30 * m2.m03 + m1.m31 * m2.m13 + m1.m32 * m2.m23 + m1.m33 * m2.m33;
		return result;
	}
}
