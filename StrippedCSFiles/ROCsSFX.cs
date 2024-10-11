using System.Runtime.CompilerServices;
using UnityEngine;

[EffectDefinition("ROCSAUDIO")]
public class ROCsSFX : EffectBehaviour
{
	[KSPField]
	public string idleEffectName;

	[KSPField]
	public string burstEffectName;

	[Persistent]
	public bool loop;

	private AudioClip clipAudio;

	private AudioSource audioSource;

	private float soundMaxDistance;

	private bool isPaused;

	private bool onPauseWasPlaying;

	private float sfxPower;

	private bool fadeoutPlayback;

	private bool fadeInPlayback;

	private string clipPath;

	public float playDelay;

	public float fadeOutSpeed;

	public float fadeInSpeed;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ROCsSFX()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDisable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Play(float power)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PlayIdleSFX()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float PlayBurstSFX()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGamePause()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGameUnpause()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FadeOutPlayback()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FadeOut()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FadeIn()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateSource()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetSFXVolume(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetClipsPath(string idleClipPath, string burstClipPath)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FadeInPlayback()
	{
		throw null;
	}
}
