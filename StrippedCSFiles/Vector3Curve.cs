using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class Vector3Curve
{
	[SerializeField]
	private AnimationCurve x;

	[SerializeField]
	private AnimationCurve y;

	[SerializeField]
	private AnimationCurve z;

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
	public Vector3Curve()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Add(float time, Vector3 value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Add(float time, Color value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 EvaluateVector(float time)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Color EvaluateColor(float time)
	{
		throw null;
	}
}
