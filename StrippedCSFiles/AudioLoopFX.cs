using System.Runtime.CompilerServices;
using UnityEngine;

[EffectDefinition("AUDIO_LOOP")]
public class AudioLoopFX : EffectBehaviour
{
	[Persistent]
	public AudioFX.AudioChannel channel;

	[Persistent]
	public string clip;

	public FXCurve volume;

	public FXCurve pitch;

	private float volumeSet;

	private AudioClip clipAudio;

	private AudioSource src;

	private bool isPaused;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AudioLoopFX()
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
}
