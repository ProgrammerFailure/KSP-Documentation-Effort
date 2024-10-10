using UnityEngine;

[EffectDefinition("MODEL_ANIMATION")]
public class ModelAnimationFX : EffectBehaviour
{
	public enum AnimationType
	{
		HoldValue,
		Play
	}

	[Persistent]
	public string modelName = "";

	[Persistent]
	public string transformName = "";

	[Persistent]
	public string animationName = "";

	[Persistent]
	public string clipName = "";

	[Persistent]
	public AnimationType animationType;

	public Transform modelParent;

	public GameObject model;

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
		modelParent = hostPart.FindModelTransform(transformName);
		if (modelParent == null)
		{
			Debug.LogError("ModelAnimationFX: Cannot find transform of name '" + transformName + "'");
			return;
		}
		model = GameDatabase.Instance.GetModel(modelName);
		if (model == null)
		{
			Debug.LogError("ModelAnimationFX: Cannot find model of name '" + modelName + "'");
			return;
		}
		model.transform.NestToParent(modelParent);
		Transform transform = model.transform.Find(animationName);
		if (transform == null)
		{
			Debug.LogError("ModelAnimationFX: Cannot find transform of name '" + animationName + "'");
			return;
		}
		anim = transform.GetComponent<Animation>();
		if (anim == null)
		{
			Debug.LogError("ModelAnimationFX: Cannot find animation component on transform of name '" + animationName + "'");
		}
		else
		{
			animState = anim[clipName];
		}
	}

	public override void OnEvent()
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

	public override void OnEvent(float power)
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
