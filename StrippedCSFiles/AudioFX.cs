using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[EffectDefinition("AUDIO")]
public class AudioFX : EffectBehaviour
{
	public enum AudioChannel
	{
		Ship,
		Voice,
		Ambient,
		Music,
		UI
	}

	[Persistent]
	public AudioChannel channel;

	[Persistent]
	public bool loop;

	[Persistent]
	public int polyphony;

	[Persistent]
	public string clip;

	[Persistent]
	public float maxVolumeDistance;

	public FXCurve volume;

	public FXCurve pitch;

	private float volumeSet;

	private AudioClip clipAudio;

	private List<AudioSource> sources;

	private bool isPaused;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AudioFX()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
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
	public override void OnEvent(float power)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnEvent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Play(float power)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetTime(float newTime)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Stop(AudioSource src)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Stop()
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
	private AudioSource CreateSource()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetSourceVolume(AudioSource source, float volumeSet, AudioChannel channel)
	{
		throw null;
	}
}
