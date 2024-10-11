using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MCAvatarController : MonoBehaviour
{
	public class AvatarAnimationClip
	{
		public CharacterAnimationState animationState;

		public float chanse;

		public float minLength;

		public float maxLength;

		public bool playOneShot;

		public bool isFallback;

		public bool IsFallback
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public AvatarAnimationClip(CharacterAnimationState animationState, float chanse, float minLength, float maxLength, bool isFallback = false)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public AvatarAnimationClip(CharacterAnimationState animationState, float chanse)
		{
			throw null;
		}
	}

	public delegate void OnReady();

	public class AvatarState
	{
		public string name;

		public List<CharacterAnimationState> anims;

		public List<string> comments;

		public Vector2 thoughtVector;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public AvatarState(string name, Vector2 thoughtVector)
		{
			throw null;
		}
	}

	public class AvatarAnimation
	{
		public KerbalInstructor avatar;

		public float minDelay;

		public float maxDelay;

		public List<AvatarAnimationClip> randomLoop;

		private float triggerTime;

		private float currentDelay;

		private bool started;

		private AvatarAnimationClip lastLoop;

		private float lastAnimTrigger;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public AvatarAnimation(KerbalInstructor avatar, float minDelay, float maxDelay)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool DetermineAndSetFallbackLoop(AvatarAnimationClip clip)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void Start(bool force = false)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public string TriggerAnimation(AvatarState state, bool playSound = true)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Update()
		{
			throw null;
		}
	}

	public KerbalInstructor avatar;

	public float rangeWeight;

	private float avgScore;

	private float scoreRange;

	private int nSamples;

	public CharacterAnimationState anim_mc_happyA_idle;

	public CharacterAnimationState anim_mc_happyB_idle;

	public CharacterAnimationState anim_mc_happyC_idle;

	public CharacterAnimationState anim_mc_idle;

	public CharacterAnimationState anim_mc_idle_drinkCoffee;

	public CharacterAnimationState anim_mc_lookLeft;

	public CharacterAnimationState anim_mc_lookRight;

	public CharacterAnimationState anim_mc_lookUp;

	public CharacterAnimationState anim_mc_scaredA_idle;

	public CharacterAnimationState anim_mc_scaredB_idle;

	public CharacterAnimationState anim_mc_scaredC_idle;

	public CharacterAnimationState anim_sound_acceptA;

	public CharacterAnimationState anim_sound_acceptB;

	public CharacterAnimationState anim_sound_acceptC;

	public CharacterAnimationState anim_sound_declineA;

	public CharacterAnimationState anim_sound_declineB;

	public CharacterAnimationState anim_sound_cancelA;

	public CharacterAnimationState anim_sound_cancelB;

	public CharacterAnimationState anim_sound_selectNormal;

	public CharacterAnimationState anim_sound_selectEasy;

	public CharacterAnimationState anim_sound_selectHard;

	public CharacterAnimationState anim_sound_selectA;

	public CharacterAnimationState anim_sound_selectB;

	public CharacterAnimationState anim_sound_selectC;

	public CharacterAnimationState anim_sound_selectD;

	public CharacterAnimationState anim_sound_selectE;

	public AvatarState animTrigger_accept;

	public AvatarState animTrigger_decline;

	public AvatarState animTrigger_cancel;

	public AvatarState animTrigger_selectNormal;

	public AvatarState animTrigger_selectEasy;

	public AvatarState animTrigger_selectHard;

	public AvatarState animTrigger_select;

	public AvatarAnimation animLoop_default;

	public AvatarAnimation animLoop_excited;

	public AvatarAnimation animLoop_happy;

	public AvatarAnimation currentAvatarAnimation;

	public List<AvatarState> states;

	private OnReady onInstructorReady;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MCAvatarController()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void NewAvatarState(CharacterAnimationState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string TriggerAnim(AvatarState trigger, AvatarAnimation loop, bool playSound = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnInstructorReady(OnReady onInstructorReady)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector2 getThoughtVector(float curiousness, float certainty)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector2 getThoughtVector(float score, float range, int samples)
	{
		throw null;
	}
}
