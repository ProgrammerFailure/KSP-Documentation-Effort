using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class KerbalInstructorBase : MonoBehaviour
{
	public Camera instructorCamera;

	public Material PortraitRenderMaterial;

	public string CharacterName;

	public Transform AnimationRoot;

	public Animation anim;

	protected Animator animator;

	protected bool isUsingAnimator;

	[NonSerialized]
	protected List<CharacterAnimationState> anims;

	[NonSerialized]
	protected CharacterAnimationState currentEmote;

	protected AudioSource _audioSource;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KerbalInstructorBase()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetupCamera(RenderTexture rt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearCamera()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Reset Animations")]
	public void SetupAnimations()
	{
		throw null;
	}
}
