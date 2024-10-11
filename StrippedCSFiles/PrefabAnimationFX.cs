using System.Runtime.CompilerServices;
using UnityEngine;

[EffectDefinition("PREFAB_ANIMATION")]
public class PrefabAnimationFX : EffectBehaviour
{
	public enum AnimationType
	{
		HoldValue,
		Play
	}

	[Persistent]
	public string prefabName;

	[Persistent]
	public string transformName;

	[Persistent]
	public string animationName;

	[Persistent]
	public string clipName;

	[Persistent]
	public AnimationType animationType;

	private Transform modelParent;

	private GameObject model;

	private Animation anim;

	private AnimationState animState;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PrefabAnimationFX()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnInitialize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnEvent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnEvent(float power)
	{
		throw null;
	}
}
