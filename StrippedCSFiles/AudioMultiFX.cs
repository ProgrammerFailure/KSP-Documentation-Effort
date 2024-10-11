using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[EffectDefinition("AUDIO_MULTI")]
internal class AudioMultiFX : EffectBehaviour
{
	[Persistent]
	public AudioFX.AudioChannel channel;

	[Persistent]
	public string clip;

	public FXCurve volume;

	public FXCurve pitch;

	[Persistent]
	public string transformName;

	private AudioClip clipAudio;

	private AudioSource source;

	private float[] volumes;

	private List<Transform> modelParents;

	private bool isPaused;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AudioMultiFX()
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
	public override void OnEvent(float power, int transformIdx)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnEvent(int transformIdx)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Play(float power, int transformIdx)
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
	private AudioSource CreateSource(Transform transform)
	{
		throw null;
	}
}
