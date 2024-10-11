using System.Runtime.CompilerServices;
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Matrix4x4D(double p00, double p01, double p02, double p03, double p10, double p11, double p12, double p13, double p20, double p21, double p22, double p23, double p30, double p31, double p32, double p33)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Matrix4x4D(Matrix4x4 m)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private double det2x2(double a, double b, double c, double d)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private double det3x3(double a1, double a2, double a3, double b1, double b2, double b3, double c1, double c2, double c3)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Matrix4x4D Translate(double dx, double dy, double dz)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Matrix4x4D RotateXaxis(double angle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Matrix4x4D RotateYaxis(double angle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Matrix4x4D RotateZaxis(double angle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Matrix4x4D RotateXaxis(double sina, double cosa)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Matrix4x4D RotateYaxis(double sina, double cosa)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Matrix4x4D RotateZaxis(double sina, double cosa)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Matrix4x4D Scale(double factor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Matrix4x4D Scale(double sx, double sy, double sz)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Matrix4x4D Identity()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static explicit operator Matrix4x4D(Matrix4x4 m)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Matrix4x4D Zero()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Matrix4x4D SetIdentity()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Matrix4x4D Inverse()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double Det()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d TransformVector(Vector3d v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d TransformPoint(Vector3d v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d MultiplyPoint3x4(Vector3d v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Matrix4x4D operator +(Matrix4x4D m1, Matrix4x4D m2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Matrix4x4D operator -(Matrix4x4D m1, Matrix4x4D m2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Matrix4x4D operator *(Matrix4x4D m1, Matrix4x4D m2)
	{
		throw null;
	}
}
