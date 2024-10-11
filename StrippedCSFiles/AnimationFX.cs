using System.Runtime.CompilerServices;
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
	public string clipName;

	[Persistent]
	public string transformName;

	[Persistent]
	public AnimationType animationType;

	private Animation anim;

	private AnimationState animState;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AnimationFX()
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool FindAnimator()
	{
		throw null;
	}
}
