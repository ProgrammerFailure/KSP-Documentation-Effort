using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class CharacterAnimationUtil
{
	public static List<CharacterAnimationState> GetAnimationsListFromScript(MonoBehaviour script)
	{
		List<CharacterAnimationState> list = new List<CharacterAnimationState>();
		FieldInfo[] fields = script.GetType().GetFields();
		foreach (FieldInfo fieldInfo in fields)
		{
			if (!(fieldInfo.FieldType != typeof(CharacterAnimationState)))
			{
				list.Add((CharacterAnimationState)script.GetType().InvokeMember(fieldInfo.Name, BindingFlags.GetField, null, script, new object[0]));
			}
		}
		return list;
	}

	public static void SetupAnimations(List<CharacterAnimationState> animations, Transform animationRoot, bool UseAnimator = false)
	{
		if ((UseAnimator && !animationRoot.GetComponent<Animator>()) || !animationRoot.GetComponent<Animation>())
		{
			Debug.LogError("[Animation Setup Error]: no animation component found on given animation root object.");
			return;
		}
		if (UseAnimator)
		{
			foreach (CharacterAnimationState animation in animations)
			{
				animation.name = ((animation.name != "") ? animation.name : animation.clip.name);
			}
			return;
		}
		foreach (CharacterAnimationState animation2 in animations)
		{
			animation2.name = ((animation2.name != "") ? animation2.name : animation2.clip.name);
			animationRoot.GetComponent<Animation>().AddClip(animation2.clip, animation2.name);
			AnimationState animationState = animationRoot.GetComponent<Animation>()[animation2.name];
			animationState.weight = animation2.weight;
			animationState.wrapMode = animation2.wrapMode;
			animationState.layer = animation2.layer;
			animationState.speed = animation2.speed;
			Transform[] addMixingTransforms = animation2.addMixingTransforms;
			foreach (Transform mix in addMixingTransforms)
			{
				animationState.AddMixingTransform(mix, animation2.addRecursive);
			}
			addMixingTransforms = animation2.excludeMixingTransforms;
			foreach (Transform mix2 in addMixingTransforms)
			{
				animationState.RemoveMixingTransform(mix2);
			}
		}
	}
}
