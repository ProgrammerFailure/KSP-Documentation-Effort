using UnityEngine;

public class KerbalInstructor : KerbalInstructorBase
{
	public CharacterAnimationState anim_idle;

	public CharacterAnimationState anim_idle_lookAround;

	public CharacterAnimationState anim_idle_sigh;

	public CharacterAnimationState anim_idle_wonder;

	public CharacterAnimationState anim_true_thumbUp;

	public CharacterAnimationState anim_true_thumbsUp;

	public CharacterAnimationState anim_true_nodA;

	public CharacterAnimationState anim_true_nodB;

	public CharacterAnimationState anim_true_smileA;

	public CharacterAnimationState anim_true_smileB;

	public CharacterAnimationState anim_false_disappointed;

	public CharacterAnimationState anim_false_disagreeA;

	public CharacterAnimationState anim_false_disagreeB;

	public CharacterAnimationState anim_false_disagreeC;

	public CharacterAnimationState anim_false_sadA;

	public float rptInterval;

	public new void Start()
	{
		base.Start();
		if (isUsingAnimator)
		{
			animator.Play(anim_idle);
		}
		else
		{
			anim.Play(anim_idle);
		}
	}

	public void PlayEmote(CharacterAnimationState st)
	{
		if (isUsingAnimator)
		{
			animator.CrossFade(st, 0.5f);
		}
		else
		{
			anim.CrossFade(st, 0.5f, PlayMode.StopSameLayer);
		}
		this.GetComponentCached(ref _audioSource).volume = GameSettings.VOICE_VOLUME;
		if (st != currentEmote && st.audioClip != null)
		{
			this.GetComponentCached(ref _audioSource).PlayOneShot(st.audioClip);
		}
		if (st.wrapMode != WrapMode.Loop && st.wrapMode != WrapMode.PingPong)
		{
			if (isUsingAnimator)
			{
				animator.CrossFade(anim_idle, 0.5f);
			}
			else
			{
				anim.CrossFadeQueued(anim_idle, 0.5f, QueueMode.CompleteOthers);
			}
		}
	}

	public void PlayEmoteRepeating(CharacterAnimationState st, float repeatInterval)
	{
		rptInterval = repeatInterval;
		if (IsInvoking())
		{
			CancelInvoke();
		}
		PlayEmote(st);
		currentEmote = st;
		if (rptInterval != 0f && currentEmote != anim_idle)
		{
			Invoke("RepeatEmote", st.clip.length + rptInterval);
		}
	}

	public void StopRepeatingEmote()
	{
		if (IsInvoking())
		{
			CancelInvoke();
		}
	}

	public void RepeatEmote()
	{
		PlayEmoteRepeating(currentEmote, rptInterval);
	}

	public void PlayEmoteQueued(CharacterAnimationState st, CharacterAnimationState fallbackAnim)
	{
		if (isUsingAnimator)
		{
			animator.CrossFade(st, 0.5f, 0);
			this.GetComponentCached(ref _audioSource).volume = GameSettings.VOICE_VOLUME;
			if (st != currentEmote && st.audioClip != null)
			{
				this.GetComponentCached(ref _audioSource).PlayOneShot(st.audioClip);
			}
		}
		else if (anim.clip.wrapMode == WrapMode.Loop || anim.clip.wrapMode == WrapMode.PingPong)
		{
			anim.CrossFade(st, 0.5f, PlayMode.StopSameLayer);
			this.GetComponentCached(ref _audioSource).volume = GameSettings.VOICE_VOLUME;
			if (st != currentEmote && st.audioClip != null)
			{
				this.GetComponentCached(ref _audioSource).PlayOneShot(st.audioClip);
			}
			if (st.wrapMode != WrapMode.Loop && st.wrapMode != WrapMode.PingPong)
			{
				anim.CrossFadeQueued(fallbackAnim, 0.5f, QueueMode.CompleteOthers);
			}
		}
	}

	public void PlayEmote(CharacterAnimationState st, CharacterAnimationState fallbackAnim, bool playSound = true)
	{
		if (playSound)
		{
			this.GetComponentCached(ref _audioSource).volume = GameSettings.VOICE_VOLUME;
			if (st != currentEmote && st.audioClip != null && !this.GetComponentCached(ref _audioSource).isPlaying)
			{
				this.GetComponentCached(ref _audioSource).PlayOneShot(st.audioClip);
			}
		}
		if (isUsingAnimator)
		{
			animator.CrossFade(st, 0.5f);
			return;
		}
		anim.CrossFade(st, 0.5f, PlayMode.StopAll);
		if (st.wrapMode != WrapMode.Loop && st.wrapMode != WrapMode.PingPong)
		{
			anim.CrossFadeQueued(fallbackAnim, 0.5f, QueueMode.CompleteOthers);
		}
	}
}
