using System;
using System.Runtime.CompilerServices;

public struct DoubleKeyframe : IComparable
{
	public double inTangent;

	public double outTangent;

	public double time;

	public double value;

	public bool autoTangent;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DoubleKeyframe(double time, double value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DoubleKeyframe(double time, double value, double inTangent, double outTangent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	int IComparable.CompareTo(object obj)
	{
		throw null;
	}
}
