using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.FX.Fireworks;

public class FireworkFXComponent : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CPlaySFXOnParticleDeath_003Ed__31 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public float lifetime;

		public FireworkFXComponent _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CPlaySFXOnParticleDeath_003Ed__31(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	[Tooltip("Usually the particle system on the parent object.")]
	public ParticleSystem parentParticles;

	[Tooltip("These will have a Color Over Lifetime gradient applied with the first and second trail colors of the PAW.")]
	public List<ParticleSystem> trailMainColorParticles;

	[Tooltip("These will have a Color Over Lifetime gradient applied with the first and second burst colors of the PAW.")]
	public List<ParticleSystem> burstMainColorParticles;

	[Tooltip("The third color of the PAW will replace the start color of these.")]
	public List<ParticleSystem> burstColor3Particles;

	[Tooltip("These will have their Start Speed multiplied by the Burst Duration value of the PAW.")]
	public List<ParticleSystem> burstSpreadParticles;

	[Tooltip("These will have their Start Lifetime multiplied by the Burst Duration value of the PAW.")]
	public List<ParticleSystem> burstDurationParticles;

	[Tooltip("These will have their Start Size multiplied by the Burst Duration value of the PAW.")]
	public List<ParticleSystem> burstFlareSizeParticles;

	public FireworkEffectType fwEffectType;

	public ParticleSystem[] particleSystems;

	public AudioSource audioSource;

	public AudioClip crackleSFX;

	public int maxAmountOfCrackleInstances;

	public Quaternion initialRotation;

	private Transform objectTransform;

	private FireworkFX fwFXController;

	private bool hasStarted;

	private bool hasEnded;

	private bool burstActivated;

	private ParticleSystem.Particle[] particlePool;

	private float[] particleLifetimes;

	private float currentCrackleSFXInstances;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FireworkFXComponent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Die()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Initialize(GameObject trailGO, FireworkFX controller, FireworkEffectType type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ActivateBurstPS(Vector3 position)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void configureParticleSystems()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private ParticleSystem.MinMaxGradient createColorGradient(Gradient baseGradient, GradientAlphaKey[] alphaKeys, GradientColorKey[] colorKeys)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool particleSystemsPlaying()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void setParticlesCBGravity(ParticleSystem.ForceOverLifetimeModule folm)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CPlaySFXOnParticleDeath_003Ed__31))]
	private IEnumerator PlaySFXOnParticleDeath(float lifetime)
	{
		throw null;
	}
}
