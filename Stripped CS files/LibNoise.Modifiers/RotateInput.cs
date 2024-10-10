using System;
using UnityEngine;

namespace LibNoise.Modifiers;

public class RotateInput : IModule
{
	public double XAngle;

	public double YAngle;

	public double ZAngle;

	public double m_x1Matrix;

	public double m_x2Matrix;

	public double m_x3Matrix;

	public double m_y1Matrix;

	public double m_y2Matrix;

	public double m_y3Matrix;

	public double m_z1Matrix;

	public double m_z2Matrix;

	public double m_z3Matrix;

	public IModule SourceModule { get; set; }

	public RotateInput(IModule sourceModule, double xAngle, double yAngle, double zAngle)
	{
		if (sourceModule == null)
		{
			throw new ArgumentNullException("A source module must be provided.");
		}
		SourceModule = sourceModule;
		XAngle = xAngle;
		YAngle = yAngle;
		ZAngle = zAngle;
	}

	public void SetAngles(double xAngle, double yAngle, double zAngle)
	{
		XAngle = xAngle;
		YAngle = yAngle;
		ZAngle = zAngle;
		double num = System.Math.Cos(xAngle);
		double num2 = System.Math.Cos(yAngle);
		double num3 = System.Math.Cos(zAngle);
		double num4 = System.Math.Sin(xAngle);
		double num5 = System.Math.Sin(yAngle);
		double num6 = System.Math.Sin(zAngle);
		m_x1Matrix = num5 * num4 * num6 + num2 * num3;
		m_y1Matrix = num * num6;
		m_z1Matrix = num5 * num3 - num2 * num4 * num6;
		m_x2Matrix = num5 * num4 * num3 - num2 * num6;
		m_y2Matrix = num * num3;
		m_z2Matrix = (0.0 - num2) * num4 * num3 - num5 * num6;
		m_x3Matrix = (0.0 - num5) * num;
		m_y3Matrix = num4;
		m_z3Matrix = num2 * num;
	}

	public double GetValue(Vector3d coordinate)
	{
		return GetValue(coordinate.x, coordinate.y, coordinate.z);
	}

	public double GetValue(Vector3 coordinate)
	{
		return GetValue(coordinate.x, coordinate.y, coordinate.z);
	}

	public double GetValue(double x, double y, double z)
	{
		if (SourceModule == null)
		{
			throw new NullReferenceException("A source module must be provided.");
		}
		double x2 = m_x1Matrix * x + m_y1Matrix * y + m_z1Matrix * z;
		double y2 = m_x2Matrix * x + m_y2Matrix * y + m_z2Matrix * z;
		double z2 = m_x3Matrix * x + m_y3Matrix * y + m_z3Matrix * z;
		return SourceModule.GetValue(x2, y2, z2);
	}
}
