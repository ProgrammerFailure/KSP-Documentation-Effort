using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[ComVisible(true)]
public class KSPRandom : Random
{
	private int[] seedArray;

	private const int MBIG = int.MaxValue;

	private const int MSEED = 161803398;

	private const int MZ = 0;

	private int inexta;

	private int inextb;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KSPRandom()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KSPRandom(int Seed)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected new virtual double Sample()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public new virtual int Next()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public new virtual int Next(int maxValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public new virtual int Next(int minValue, int maxValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public new virtual void NextBytes(byte[] buffer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public new virtual double NextDouble()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double NextDouble(double minValue, double maxValue)
	{
		throw null;
	}
}
