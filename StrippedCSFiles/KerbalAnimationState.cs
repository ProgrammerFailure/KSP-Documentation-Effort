using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class KerbalAnimationState
{
	public string animationName;

	public bool hasJpVariant;

	public bool scaleWithGravity;

	public float speedAt1Gee;

	public float start;

	public float end;

	public int layer;

	public WrapMode wrapMode;

	public AnimationBlendMode blendMode;

	public float weight;

	public Transform[] addMixingTransforms;

	public bool addRecursive;

	public KerbalMixingTransforms[] specificMixingTransforms;

	public Transform[] excludeMixingTransforms;

	public float wheeLevel;

	public float fearFactor;

	public float startExpressionTime;

	public float CutAnimationTime;

	[NonSerialized]
	protected KerbalAnimationManager manager;

	public AnimationState State
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KerbalAnimationState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetManager(KerbalAnimationManager manager)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static implicit operator string(KerbalAnimationState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetAnimationString()
	{
		throw null;
	}
}
