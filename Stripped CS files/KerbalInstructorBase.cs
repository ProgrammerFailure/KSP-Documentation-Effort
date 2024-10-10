using System;
using System.Collections.Generic;
using UnityEngine;

public class KerbalInstructorBase : MonoBehaviour
{
	public Camera instructorCamera;

	public Material PortraitRenderMaterial;

	public string CharacterName = "";

	public Transform AnimationRoot;

	public Animation anim;

	public Animator animator;

	public bool isUsingAnimator;

	[NonSerialized]
	public List<CharacterAnimationState> anims;

	[NonSerialized]
	public CharacterAnimationState currentEmote;

	public AudioSource _audioSource;

	public void Start()
	{
		SetupAnimations();
	}

	public void SetupCamera(RenderTexture rt)
	{
		instructorCamera.targetTexture = rt;
		instructorCamera.ResetAspect();
		instructorCamera.enabled = true;
	}

	public void ClearCamera()
	{
		if (instructorCamera != null)
		{
			if (instructorCamera.targetTexture != null)
			{
				instructorCamera.targetTexture.DiscardContents();
				instructorCamera.targetTexture.Release();
				instructorCamera.targetTexture = null;
			}
			instructorCamera.enabled = false;
		}
	}

	public void OnDestroy()
	{
		ClearCamera();
	}

	[ContextMenu("Reset Animations")]
	public void SetupAnimations()
	{
		anims = CharacterAnimationUtil.GetAnimationsListFromScript(this);
		CharacterAnimationUtil.SetupAnimations(anims, AnimationRoot);
		animator = AnimationRoot.GetComponent<Animator>();
		isUsingAnimator = animator != null;
		anim = AnimationRoot.GetComponent<Animation>();
	}
}
