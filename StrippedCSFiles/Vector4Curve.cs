using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class Vector4Curve
{
	[SerializeField]
	private AnimationCurve x;

	[SerializeField]
	private AnimationCurve y;

	[SerializeField]
	private AnimationCurve z;

	[SerializeField]
	private AnimationCurve w;

	public float minTime
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public float maxTime
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector4Curve()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Add(float time, Vector4 value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Add(float time, Color value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector4 EvaluateVector(float time)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Color EvaluateColor(float time)
	{
		throw null;
	}
}
