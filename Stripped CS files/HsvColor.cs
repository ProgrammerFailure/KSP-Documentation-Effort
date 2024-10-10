public struct HsvColor
{
	public double h;

	public double s;

	public double v;

	public float normalizedH
	{
		get
		{
			return (float)h / 360f;
		}
		set
		{
			h = (double)value * 360.0;
		}
	}

	public float normalizedS
	{
		get
		{
			return (float)s;
		}
		set
		{
			s = value;
		}
	}

	public float normalizedV
	{
		get
		{
			return (float)v;
		}
		set
		{
			v = value;
		}
	}

	public HsvColor(double h, double s, double v)
	{
		this.h = h;
		this.s = s;
		this.v = v;
	}

	public override string ToString()
	{
		return "{" + h.ToString("f2") + "," + s.ToString("f2") + "," + v.ToString("f2") + "}";
	}
}
