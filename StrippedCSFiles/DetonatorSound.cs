using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Detonator))]
[AddComponentMenu("Detonator/Sound")]
public class DetonatorSound : DetonatorComponent
{
	public AudioClip[] nearSounds;

	public AudioClip[] farSounds;

	public float distanceThreshold;

	public float minVolume;

	public float maxVolume;

	public float rolloffFactor;

	private AudioSource _soundComponent;

	private bool _delayedExplosionStarted;

	private float _explodeDelay;

	public static Func<float> GetSoundVolume;

	private int _idx;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DetonatorSound()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static DetonatorSound()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Init()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Explode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Reset()
	{
		throw null;
	}
}
