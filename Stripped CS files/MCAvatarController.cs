using System.Collections.Generic;
using ns9;
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
			get
			{
				if (!playOneShot)
				{
					return isFallback;
				}
				return false;
			}
		}

		public AvatarAnimationClip(CharacterAnimationState animationState, float chanse, float minLength, float maxLength, bool isFallback = false)
		{
			this.animationState = animationState;
			this.chanse = chanse;
			this.minLength = minLength;
			this.maxLength = maxLength;
			if (animationState.wrapMode != WrapMode.Loop && animationState.wrapMode != WrapMode.PingPong)
			{
				playOneShot = true;
			}
			this.isFallback = isFallback;
		}

		public AvatarAnimationClip(CharacterAnimationState animationState, float chanse)
		{
			this.animationState = animationState;
			this.chanse = chanse;
			playOneShot = true;
		}
	}

	public delegate void OnReady();

	public class AvatarState
	{
		public string name;

		public List<CharacterAnimationState> anims = new List<CharacterAnimationState>();

		public List<string> comments = new List<string>();

		public Vector2 thoughtVector;

		public AvatarState(string name, Vector2 thoughtVector)
		{
			this.name = name;
			this.thoughtVector = thoughtVector;
		}
	}

	public class AvatarAnimation
	{
		public KerbalInstructor avatar;

		public float minDelay;

		public float maxDelay;

		public List<AvatarAnimationClip> randomLoop = new List<AvatarAnimationClip>();

		public float triggerTime;

		public float currentDelay;

		public bool started;

		public AvatarAnimationClip lastLoop;

		public float lastAnimTrigger;

		public AvatarAnimation(KerbalInstructor avatar, float minDelay, float maxDelay)
		{
			this.avatar = avatar;
			this.minDelay = minDelay;
			this.maxDelay = maxDelay;
		}

		public bool DetermineAndSetFallbackLoop(AvatarAnimationClip clip)
		{
			if (clip.IsFallback)
			{
				lastLoop = clip;
				return true;
			}
			return false;
		}

		public void Start(bool force = false)
		{
			if (!(!started || force))
			{
				return;
			}
			started = true;
			currentDelay = Random.Range(minDelay, maxDelay);
			triggerTime = Time.realtimeSinceStartup;
			if (lastLoop == null)
			{
				int count = randomLoop.Count;
				for (int i = 0; i < count && !DetermineAndSetFallbackLoop(randomLoop[i]); i++)
				{
				}
			}
		}

		public string TriggerAnimation(AvatarState state, bool playSound = true)
		{
			Start(force: true);
			if (lastAnimTrigger + 1f < Time.realtimeSinceStartup)
			{
				avatar.PlayEmote(state.anims[Random.Range(0, state.anims.Count)], lastLoop.animationState, playSound);
				lastAnimTrigger = Time.realtimeSinceStartup;
			}
			return state.comments[Random.Range(0, state.comments.Count)];
		}

		public void Update()
		{
			Start();
			if (!(triggerTime + currentDelay < Time.realtimeSinceStartup))
			{
				return;
			}
			AvatarAnimationClip avatarAnimationClip = null;
			float num = 0f;
			int count = randomLoop.Count;
			for (int i = 0; i < count; i++)
			{
				AvatarAnimationClip avatarAnimationClip2 = randomLoop[i];
				float num2 = Random.Range(1E-10f, avatarAnimationClip2.chanse);
				if (num2 > num)
				{
					num = num2;
					avatarAnimationClip = avatarAnimationClip2;
				}
			}
			if (lastLoop == null)
			{
				DetermineAndSetFallbackLoop(avatarAnimationClip);
			}
			avatar.PlayEmoteQueued(avatarAnimationClip.animationState, lastLoop.animationState);
			started = false;
		}
	}

	public KerbalInstructor avatar;

	public float rangeWeight = 1f;

	public float avgScore;

	public float scoreRange = 0.5f;

	public int nSamples;

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

	public List<AvatarState> states = new List<AvatarState>();

	public OnReady onInstructorReady = delegate
	{
	};

	public void NewAvatarState(CharacterAnimationState st)
	{
		AvatarState avatarState = new AvatarState(st.name, getThoughtVector(0f, -1f));
		avatarState.anims.Add(st);
		avatarState.comments.Add(st.clip.name);
		states.Add(avatarState);
	}

	public void Update()
	{
		if (currentAvatarAnimation != null)
		{
			currentAvatarAnimation.Update();
		}
	}

	public string TriggerAnim(AvatarState trigger, AvatarAnimation loop, bool playSound = true)
	{
		currentAvatarAnimation = loop;
		return currentAvatarAnimation.TriggerAnimation(trigger, playSound);
	}

	public void OnInstructorReady(OnReady onInstructorReady)
	{
		this.onInstructorReady = onInstructorReady;
	}

	public void Start()
	{
		NewAvatarState(avatar.anim_idle);
		NewAvatarState(anim_mc_happyA_idle);
		NewAvatarState(anim_mc_happyB_idle);
		NewAvatarState(anim_mc_happyC_idle);
		NewAvatarState(anim_mc_idle);
		NewAvatarState(anim_mc_idle_drinkCoffee);
		NewAvatarState(anim_mc_lookLeft);
		NewAvatarState(anim_mc_lookRight);
		NewAvatarState(anim_mc_lookUp);
		NewAvatarState(anim_mc_scaredA_idle);
		NewAvatarState(anim_mc_scaredB_idle);
		NewAvatarState(anim_mc_scaredC_idle);
		NewAvatarState(anim_sound_acceptA);
		NewAvatarState(anim_sound_acceptB);
		NewAvatarState(anim_sound_acceptC);
		NewAvatarState(anim_sound_declineA);
		NewAvatarState(anim_sound_declineB);
		NewAvatarState(anim_sound_cancelA);
		NewAvatarState(anim_sound_cancelB);
		NewAvatarState(anim_sound_selectNormal);
		NewAvatarState(anim_sound_selectEasy);
		NewAvatarState(anim_sound_selectHard);
		NewAvatarState(anim_sound_selectA);
		NewAvatarState(anim_sound_selectB);
		NewAvatarState(anim_sound_selectC);
		NewAvatarState(anim_sound_selectD);
		NewAvatarState(anim_sound_selectE);
		animLoop_default = new AvatarAnimation(avatar, 2f, 5f);
		animLoop_default.randomLoop.Add(new AvatarAnimationClip(anim_mc_idle, 0.5f, float.MaxValue, float.MaxValue, isFallback: true));
		animLoop_default.randomLoop.Add(new AvatarAnimationClip(anim_mc_idle_drinkCoffee, 0.3f));
		animLoop_default.randomLoop.Add(new AvatarAnimationClip(anim_mc_lookLeft, 0.2f));
		animLoop_default.randomLoop.Add(new AvatarAnimationClip(anim_mc_lookRight, 0.2f));
		animLoop_default.randomLoop.Add(new AvatarAnimationClip(anim_mc_lookUp, 0.2f));
		animLoop_excited = new AvatarAnimation(avatar, 2f, 5f);
		animLoop_excited.randomLoop.Add(new AvatarAnimationClip(anim_mc_happyA_idle, 0.5f, float.MaxValue, float.MaxValue, isFallback: true));
		animLoop_excited.randomLoop.Add(new AvatarAnimationClip(anim_mc_idle_drinkCoffee, 0.45f));
		animLoop_happy = new AvatarAnimation(avatar, 2f, 5f);
		animLoop_happy.randomLoop.Add(new AvatarAnimationClip(anim_mc_happyA_idle, 0.5f, float.MaxValue, float.MaxValue, isFallback: true));
		animLoop_happy.randomLoop.Add(new AvatarAnimationClip(anim_mc_idle_drinkCoffee, 0.3f));
		animLoop_happy.randomLoop.Add(new AvatarAnimationClip(anim_mc_lookLeft, 0.2f));
		animLoop_happy.randomLoop.Add(new AvatarAnimationClip(anim_mc_lookRight, 0.2f));
		animLoop_happy.randomLoop.Add(new AvatarAnimationClip(anim_mc_lookUp, 0.2f));
		animTrigger_accept = new AvatarState("animTrigger_accept", getThoughtVector(0f, -1f));
		animTrigger_accept.anims.Add(anim_sound_acceptA);
		animTrigger_accept.anims.Add(anim_sound_acceptB);
		animTrigger_accept.anims.Add(anim_sound_acceptC);
		animTrigger_accept.comments.Add(Localizer.Format("#autoLOC_467299"));
		animTrigger_accept.comments.Add(Localizer.Format("#autoLOC_467300"));
		animTrigger_accept.comments.Add(Localizer.Format("#autoLOC_467301"));
		animTrigger_accept.comments.Add(Localizer.Format("#autoLOC_467302"));
		animTrigger_accept.comments.Add(Localizer.Format("#autoLOC_467303"));
		animTrigger_accept.comments.Add(Localizer.Format("#autoLOC_467304"));
		states.Add(animTrigger_accept);
		animTrigger_decline = new AvatarState("animTrigger_decline", getThoughtVector(0f, -1f));
		animTrigger_decline.anims.Add(anim_sound_declineA);
		animTrigger_decline.anims.Add(anim_sound_declineB);
		animTrigger_decline.comments.Add(Localizer.Format("#autoLOC_467310"));
		animTrigger_decline.comments.Add(Localizer.Format("#autoLOC_467311"));
		animTrigger_decline.comments.Add(Localizer.Format("#autoLOC_467312"));
		animTrigger_decline.comments.Add(Localizer.Format("#autoLOC_467313"));
		animTrigger_decline.comments.Add(Localizer.Format("#autoLOC_467314"));
		states.Add(animTrigger_decline);
		animTrigger_cancel = new AvatarState("animTrigger_cancel", getThoughtVector(0f, -1f));
		animTrigger_cancel.anims.Add(anim_sound_cancelA);
		animTrigger_cancel.anims.Add(anim_sound_cancelB);
		animTrigger_cancel.comments.Add(Localizer.Format("#autoLOC_467320"));
		animTrigger_cancel.comments.Add(Localizer.Format("#autoLOC_467321"));
		animTrigger_cancel.comments.Add(Localizer.Format("#autoLOC_467322"));
		animTrigger_cancel.comments.Add(Localizer.Format("#autoLOC_467323"));
		animTrigger_cancel.comments.Add(Localizer.Format("#autoLOC_467324"));
		states.Add(animTrigger_cancel);
		animTrigger_selectNormal = new AvatarState("animTrigger_selectNormal", getThoughtVector(0f, -1f));
		animTrigger_selectNormal.anims.Add(anim_sound_selectA);
		animTrigger_selectNormal.comments.Add(Localizer.Format("#autoLOC_467329"));
		animTrigger_selectNormal.comments.Add(Localizer.Format("#autoLOC_467330"));
		animTrigger_selectNormal.comments.Add(Localizer.Format("#autoLOC_467331"));
		animTrigger_selectNormal.comments.Add(Localizer.Format("#autoLOC_467332"));
		animTrigger_selectNormal.comments.Add(Localizer.Format("#autoLOC_467333"));
		states.Add(animTrigger_selectNormal);
		animTrigger_selectEasy = new AvatarState("animTrigger_selectEasy", getThoughtVector(0f, -1f));
		animTrigger_selectEasy.anims.Add(anim_sound_selectC);
		animTrigger_selectEasy.anims.Add(anim_sound_selectD);
		animTrigger_selectEasy.anims.Add(anim_sound_selectE);
		animTrigger_selectEasy.comments.Add(Localizer.Format("#autoLOC_467340"));
		animTrigger_selectEasy.comments.Add(Localizer.Format("#autoLOC_467341"));
		animTrigger_selectEasy.comments.Add(Localizer.Format("#autoLOC_467342"));
		animTrigger_selectEasy.comments.Add(Localizer.Format("#autoLOC_467343"));
		animTrigger_selectEasy.comments.Add(Localizer.Format("#autoLOC_467344"));
		animTrigger_selectEasy.comments.Add(Localizer.Format("#autoLOC_467345"));
		animTrigger_selectEasy.comments.Add(Localizer.Format("#autoLOC_467346"));
		states.Add(animTrigger_selectEasy);
		animTrigger_selectHard = new AvatarState("animTrigger_selectHard", getThoughtVector(0f, -1f));
		animTrigger_selectHard.anims.Add(anim_sound_selectHard);
		animTrigger_selectHard.comments.Add(Localizer.Format("#autoLOC_467351"));
		animTrigger_selectHard.comments.Add(Localizer.Format("#autoLOC_467352"));
		animTrigger_selectHard.comments.Add(Localizer.Format("#autoLOC_467353"));
		animTrigger_selectHard.comments.Add(Localizer.Format("#autoLOC_467354"));
		animTrigger_selectHard.comments.Add(Localizer.Format("#autoLOC_467355"));
		animTrigger_selectHard.comments.Add(Localizer.Format("#autoLOC_467356"));
		animTrigger_selectHard.comments.Add(Localizer.Format("#autoLOC_467357"));
		animTrigger_selectHard.comments.Add(Localizer.Format("#autoLOC_467358"));
		animTrigger_selectHard.comments.Add(Localizer.Format("#autoLOC_467359"));
		states.Add(animTrigger_selectHard);
		animTrigger_select = new AvatarState("animTrigger_select", getThoughtVector(0f, -1f));
		animTrigger_select.anims.Add(anim_sound_selectA);
		animTrigger_select.anims.Add(anim_sound_selectB);
		animTrigger_select.anims.Add(anim_sound_selectC);
		animTrigger_select.anims.Add(anim_sound_selectD);
		animTrigger_select.anims.Add(anim_sound_selectE);
		animTrigger_select.comments.Add(Localizer.Format("#autoLOC_467368"));
		states.Add(animTrigger_select);
		List<CharacterAnimationState> list = new List<CharacterAnimationState>();
		int count = states.Count;
		for (int i = 0; i < count; i++)
		{
			int count2 = states[i].anims.Count;
			for (int j = 0; j < count2; j++)
			{
				list.Add(states[i].anims[j]);
			}
		}
		CharacterAnimationUtil.SetupAnimations(list, avatar.AnimationRoot, UseAnimator: true);
		onInstructorReady();
	}

	public Vector2 getThoughtVector(float curiousness, float certainty)
	{
		return new Vector2(curiousness, certainty);
	}

	public Vector2 getThoughtVector(float score, float range, int samples)
	{
		float num = score * 2f - 1f;
		float num2 = ((samples >= 1) ? range : (-1f));
		return new Vector2(num * (1f - Mathf.Abs(num2) * rangeWeight), num2);
	}
}
