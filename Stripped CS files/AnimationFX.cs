using UnityEngine;

[EffectDefinition("ANIMATION")]
public class AnimationFX : EffectBehaviour
{
	public enum AnimationType
	{
		HoldValue,
		Play
	}

	[Persistent]
	public string clipName = "";

	[Persistent]
	public string transformName = "";

	[Persistent]
	public AnimationType animationType;

	public Animation anim;

	public AnimationState animState;

	public override void OnLoad(ConfigNode node)
	{
		ConfigNode.LoadObjectFromConfig(this, node);
	}

	public override void OnSave(ConfigNode node)
	{
		ConfigNode.CreateConfigFromObject(this, node).CopyTo(node);
	}

	public override void OnInitialize()
	{
		FindAnimator();
	}

	public override void OnEvent()
	{
		if (!(anim == null))
		{
			switch (animationType)
			{
			case AnimationType.Play:
				animState.normalizedTime = 0f;
				anim.Play(clipName);
				break;
			case AnimationType.HoldValue:
				animState.normalizedTime = 0f;
				anim.Sample();
				break;
			}
		}
	}

	public override void OnEvent(float power)
	{
		if (!(anim == null) || FindAnimator())
		{
			switch (animationType)
			{
			case AnimationType.Play:
				animState.normalizedTime = power;
				anim.Play(clipName);
				break;
			case AnimationType.HoldValue:
				animState.normalizedTime = power;
				anim.Sample();
				break;
			}
		}
	}

	public bool FindAnimator()
	{
		anim = hostPart.FindModelAnimator(transformName, clipName);
		if (anim == null)
		{
			return false;
		}
		animState = anim[clipName];
		return true;
	}
}
