using System.Collections.Generic;
using UnityEngine;

public class FXModuleAnimateThrottle : PartModule
{
	[KSPField]
	public string animationName = "throttleAnim";

	[KSPField]
	public int layer = 1;

	[KSPField]
	public float responseSpeed = 0.5f;

	[KSPField]
	public bool dependOnEngineState;

	[KSPField]
	public bool dependOnOutput;

	[KSPField]
	public bool dependOnThrottle;

	[KSPField]
	public bool preferMultiMode;

	[KSPField]
	public int engineIndex;

	[KSPField]
	public string engineName;

	[KSPField]
	public bool weightOnOperational;

	[KSPField]
	public float baseAnimSpeed;

	[KSPField]
	public int animWrapMode = 8;

	[KSPField]
	public bool affectTime = true;

	[KSPField]
	public float baseAnimSpeedMult = 1f;

	[KSPField]
	public bool playInEditor;

	[KSPField(isPersistant = true)]
	public float animState;

	public Animation anim;

	public IEngineStatus engineReference;

	public override void OnStart(StartState state)
	{
		anim = null;
		List<Animation> list = base.part.FindModelComponents<Animation>();
		int i = 0;
		for (int count = list.Count; i < count; i++)
		{
			Animation animation = list[i];
			if (animation != null && animation[animationName] != null)
			{
				anim = animation;
				break;
			}
		}
		if (!(anim == null) && !(anim[animationName] == null))
		{
			anim[animationName].wrapMode = (WrapMode)animWrapMode;
			anim[animationName].weight = 1f;
			anim[animationName].speed = baseAnimSpeed;
			anim[animationName].normalizedTime = 0f;
			anim[animationName].layer = layer;
			if (dependOnEngineState)
			{
				engineReference = base.part.Modules.FindEngineNearby(engineName, engineIndex, preferMultiMode);
				if (engineReference == null)
				{
					dependOnEngineState = false;
				}
			}
		}
		else
		{
			Debug.Log("FXModuleAnimateThrottle: Could not find animation " + animationName + " in part's animation components. Check the animationName and model file");
			base.enabled = false;
		}
	}

	public void FixedUpdate()
	{
		bool loadedSceneIsEditor;
		if ((!(loadedSceneIsEditor = HighLogic.LoadedSceneIsEditor) && !HighLogic.LoadedSceneIsFlight) || base.vessel == null)
		{
			return;
		}
		AnimationState animationState = anim[animationName];
		if (animationState == null)
		{
			return;
		}
		if (!anim.IsPlaying(animationName))
		{
			anim.Play(animationName);
		}
		float num = 0f;
		float num2;
		if (!dependOnEngineState)
		{
			num = (loadedSceneIsEditor ? (playInEditor ? 1f : 0f) : Mathf.Clamp01(base.vessel.ctrlState.mainThrottle));
		}
		else
		{
			if (weightOnOperational)
			{
				num = ((!loadedSceneIsEditor || !playInEditor) ? (engineReference.isOperational ? 1f : 0f) : 1f);
				num2 = Mathf.Lerp(animationState.weight, num, responseSpeed * 3f * 25f * TimeWarp.fixedDeltaTime);
				if (num2 > 0.999f && animationState.weight < num)
				{
					num2 = 1f;
				}
				else if (num2 < 0.001f && num < animationState.weight)
				{
					num2 = 0f;
				}
				animationState.weight = num2;
			}
			num = ((engineReference.isOperational && !loadedSceneIsEditor) ? (dependOnOutput ? Mathf.Clamp01(engineReference.normalizedOutput) : ((!dependOnThrottle) ? Mathf.Clamp01(base.vessel.ctrlState.mainThrottle) : Mathf.Clamp01(engineReference.throttleSetting))) : ((!loadedSceneIsEditor || !playInEditor) ? 0f : 1f));
		}
		if (affectTime)
		{
			num2 = Mathf.Lerp(animState, num, responseSpeed * 25f * TimeWarp.fixedDeltaTime);
			if (num2 > 0.99995f && animState < num)
			{
				num2 = 1f;
			}
			if (num2 < 5E-05f && num < animState)
			{
				num2 = 0f;
			}
			animationState.normalizedTime = num2;
		}
		else
		{
			float num3 = 0f;
			if (num > 0f || (dependOnEngineState && engineReference.isOperational))
			{
				num3 = baseAnimSpeed;
			}
			num2 = Mathf.Lerp(animState, num3 + num * baseAnimSpeedMult, responseSpeed * TimeWarp.fixedDeltaTime);
			if (num2 > 0.99995f)
			{
				num2 = 1f;
			}
			if (num2 < 5E-05f)
			{
				num2 = 0f;
			}
			animationState.speed = num2;
		}
		if (!loadedSceneIsEditor)
		{
			animState = num2;
		}
	}

	public override void OnInventoryModeDisable()
	{
		anim = null;
		List<Animation> list = base.part.FindModelComponents<Animation>();
		int i = 0;
		for (int count = list.Count; i < count; i++)
		{
			Animation animation = list[i];
			if (animation != null && animation[animationName] != null)
			{
				anim = animation;
				break;
			}
		}
		if (anim != null)
		{
			anim[animationName].speed = 0f;
			anim[animationName].normalizedTime = 0f;
		}
	}
}
