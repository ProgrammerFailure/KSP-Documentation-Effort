using System;
using UnityEngine;

[Serializable]
public class KerbalAnimationState
{
	public string animationName;

	public bool hasJpVariant;

	public bool scaleWithGravity;

	public float speedAt1Gee;

	public float start;

	public float end = 1f;

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
	public KerbalAnimationManager manager;

	public AnimationState State => manager.evaController.GetComponent<Animation>()[GetAnimationString()];

	public void SetManager(KerbalAnimationManager manager)
	{
		this.manager = manager;
	}

	public static implicit operator string(KerbalAnimationState st)
	{
		return st.GetAnimationString();
	}

	public string GetAnimationString()
	{
		if (manager.evaController.JetpackDeployed && hasJpVariant)
		{
			return "jp_" + animationName;
		}
		return animationName;
	}
}
