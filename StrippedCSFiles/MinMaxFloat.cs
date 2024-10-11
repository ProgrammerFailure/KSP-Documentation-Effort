using System;
using System.Runtime.CompilerServices;

[Serializable]
public class MinMaxFloat
{
	public float min;

	public float max;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MinMaxFloat()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetLerp(float t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetInverseLerp(float v)
	{
		throw null;
	}
}
