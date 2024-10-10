using System;

namespace LibNoise.Models;

public class Line
{
	public double m_x0;

	public double m_x1;

	public double m_y0;

	public double m_y1;

	public double m_z0;

	public double m_z1;

	public IModule SourceModule { get; set; }

	public bool Attenuate { get; set; }

	public Line(IModule sourceModule)
	{
		if (sourceModule == null)
		{
			throw new ArgumentNullException("A source module must be provided.");
		}
		SourceModule = sourceModule;
		Attenuate = true;
		m_x0 = 0.0;
		m_x1 = 1.0;
		m_y0 = 0.0;
		m_y1 = 1.0;
		m_z0 = 0.0;
		m_z1 = 1.0;
	}

	public double GetValue(double p)
	{
		if (SourceModule == null)
		{
			throw new NullReferenceException("A source module must be provided.");
		}
		double x = (m_x1 - m_x0) * p + m_x0;
		double y = (m_y1 - m_y0) * p + m_y0;
		double z = (m_z1 - m_z0) * p + m_z0;
		double value = SourceModule.GetValue(x, y, z);
		if (Attenuate)
		{
			return p * (1.0 - p) * 4.0 * value;
		}
		return value;
	}

	public void SetStartPoint(double x, double y, double z)
	{
		m_x0 = x;
		m_y0 = y;
		m_z0 = z;
	}

	public void SetEndPoint(double x, double y, double z)
	{
		m_x1 = x;
		m_y1 = y;
		m_z1 = z;
	}
}
