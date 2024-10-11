using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class FXCurve
{
	[Serializable]
	public class FXKeyFrame
	{
		public float time;

		public float value;

		public Keyframe Keyframe
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public FXKeyFrame(float time, float value)
		{
			throw null;
		}
	}

	public float singleValue;

	public string valueName;

	public float lastValue;

	public bool evalSingle;

	public List<FXKeyFrame> keyFrames;

	public AnimationCurve fCurve;

	public bool fCurveCompiled;

	private static char[] delimiters;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FXCurve(string valueName, float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static FXCurve()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddKeyframe(float time, float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CompileCurve()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float Value()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float Value(float time)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(string valueName, ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static implicit operator float(FXCurve instance)
	{
		throw null;
	}
}
