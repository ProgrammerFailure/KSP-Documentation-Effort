using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class FloatCurve : IConfigNode
{
	[SerializeField]
	private AnimationCurve fCurve;

	[SerializeField]
	private float _minTime;

	[SerializeField]
	private float _maxTime;

	private static char[] delimiters;

	private static int findCurveMinMaxInterations;

	public AnimationCurve Curve
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public float minTime
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float maxTime
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FloatCurve()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FloatCurve(Keyframe[] keyframes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static FloatCurve()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Add(float time, float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Add(float time, float value, float inTangent, float outTangent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float Evaluate(float time)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FindMinMaxValue(out float min, out float max)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FindMinMaxValue(out float min, out float max, out float tMin, out float tMax)
	{
		throw null;
	}
}
