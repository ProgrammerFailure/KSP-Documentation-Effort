using System;
using UnityEngine;

[Serializable]
public class CharacterAnimationState
{
	public AnimationClip clip;

	public string name = "";

	public WrapMode wrapMode;

	public AnimationBlendMode blendMode;

	public float weight = 1f;

	public int layer = 1;

	public float start;

	public float end;

	public float speed = 1f;

	public Transform[] addMixingTransforms;

	public bool addRecursive;

	public Transform[] excludeMixingTransforms;

	public AudioClip audioClip;

	public static implicit operator string(CharacterAnimationState st)
	{
		return st.name;
	}
}
