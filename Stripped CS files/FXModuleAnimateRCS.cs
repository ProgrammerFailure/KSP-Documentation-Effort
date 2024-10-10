using System.Collections.Generic;
using UnityEngine;

public class FXModuleAnimateRCS : PartModule
{
	[KSPField]
	public string animationName = "throttleAnim";

	[KSPField]
	public int layer = 1;

	[KSPField]
	public float responseSpeed = 0.5f;

	[KSPField]
	public float baseAnimSpeed;

	[KSPField]
	public int animWrapMode = 8;

	[KSPField]
	public bool affectTime = true;

	[KSPField]
	public float baseAnimSpeedMult = 1f;

	[KSPField]
	public float thrustForceMult = 1f;

	public float[] animState;

	[SerializeField]
	public Animation[] anims;

	[SerializeField]
	public ModuleRCS moduleRCSReference;

	public override void OnStart(StartState state)
	{
		moduleRCSReference = base.part.FindModuleImplementing<ModuleRCS>();
		if (moduleRCSReference == null)
		{
			Debug.Log("[FXModuleAnimateRCS]: Could not ModuleRCS on Part. Check the Part and it's cfg file. Mult contain ModuleRCS/ModuleRCSFX. Module disabled.");
			base.enabled = false;
			return;
		}
		anims = null;
		List<Animation> list = base.part.FindModelComponents<Animation>();
		if (list.Count > 0)
		{
			anims = new Animation[list.Count];
			animState = new float[list.Count];
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				Animation animation = list[i];
				if (animation != null && animation[animationName] != null)
				{
					anims[i] = animation;
				}
			}
		}
		if (anims == null)
		{
			Debug.Log("[FXModuleAnimateRCS]: Could not find animation " + animationName + " in part's animation components. Check the animationName and model file. Module disabled.");
			base.enabled = false;
			return;
		}
		for (int j = 0; j < anims.Length; j++)
		{
			anims[j][animationName].wrapMode = (WrapMode)animWrapMode;
			anims[j][animationName].weight = 1f;
			anims[j][animationName].speed = baseAnimSpeed;
			anims[j][animationName].normalizedTime = 0f;
			anims[j][animationName].layer = layer;
		}
		List<Transform> list2 = new List<Transform>(base.part.FindModelTransforms(moduleRCSReference.thrusterTransformName));
		if (anims.Length != list2.Count)
		{
			Debug.LogFormat("[FXModuleAnimateRCS]: Found {0} Animations and {1} Thrust Transforms. Check the Part model file. Module disabled.", anims.Length, moduleRCSReference.thrusterTransforms.Count);
			base.enabled = false;
		}
	}

	public override void OnSave(ConfigNode node)
	{
		if (animState == null)
		{
			return;
		}
		string text = "";
		for (int i = 0; i < animState.Length; i++)
		{
			if (i != 0)
			{
				text += ",";
			}
			text += animState[i];
		}
		node.AddValue("animStates", text);
	}

	public override void OnLoad(ConfigNode node)
	{
		string value = "";
		if (node.TryGetValue("animStates", ref value))
		{
			string[] array = value.Split(',');
			animState = new float[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				float.TryParse(array[i], out animState[i]);
			}
		}
	}

	public void FixedUpdate()
	{
		bool loadedSceneIsEditor;
		if (!(loadedSceneIsEditor = HighLogic.LoadedSceneIsEditor) && !HighLogic.LoadedSceneIsFlight)
		{
			return;
		}
		for (int i = 0; i < anims.Length; i++)
		{
			AnimationState animationState = anims[i][animationName];
			if (animationState == null)
			{
				break;
			}
			if (!anims[i].IsPlaying(animationName))
			{
				anims[i].Play(animationName);
			}
			float num = Mathf.Clamp01(moduleRCSReference.thrustForces[i] * thrustForceMult);
			float num2 = 0f;
			if (affectTime)
			{
				num2 = Mathf.Lerp(animState[i], num, responseSpeed * 25f * TimeWarp.fixedDeltaTime);
				if (num2 > 0.99995f && animState[i] < num)
				{
					num2 = 1f;
				}
				if (num2 < 5E-05f && num < animState[i])
				{
					num2 = 0f;
				}
				animationState.normalizedTime = num2;
			}
			else
			{
				float num3 = 0f;
				if (num > 0f)
				{
					num3 = baseAnimSpeed;
				}
				num2 = Mathf.Lerp(animState[i], num3 + num * baseAnimSpeedMult, responseSpeed * TimeWarp.fixedDeltaTime);
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
				animState[i] = num2;
			}
		}
	}
}
