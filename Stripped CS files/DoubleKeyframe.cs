using System;

public struct DoubleKeyframe : IComparable
{
	public double inTangent;

	public double outTangent;

	public double time;

	public double value;

	public bool autoTangent;

	public DoubleKeyframe(double time, double value)
	{
		this.time = time;
		this.value = value;
		double num = 0.0;
		outTangent = 0.0;
		inTangent = num;
		autoTangent = true;
	}

	public DoubleKeyframe(double time, double value, double inTangent, double outTangent)
	{
		this.time = time;
		this.value = value;
		this.inTangent = inTangent;
		this.outTangent = outTangent;
		autoTangent = false;
	}

	int IComparable.CompareTo(object obj)
	{
		return time.CompareTo(((DoubleKeyframe)obj).time);
	}
}
