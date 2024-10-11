using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class CharacterAnimationState
{
	public AnimationClip clip;

	public string name;

	public WrapMode wrapMode;

	public AnimationBlendMode blendMode;

	public float weight;

	public int layer;

	public float start;

	public float end;

	public float speed;

	public Transform[] addMixingTransforms;

	public bool addRecursive;

	public Transform[] excludeMixingTransforms;

	public AudioClip audioClip;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CharacterAnimationState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static implicit operator string(CharacterAnimationState st)
	{
		throw null;
	}
}
