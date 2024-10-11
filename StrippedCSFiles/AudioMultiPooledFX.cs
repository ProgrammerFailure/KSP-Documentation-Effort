using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Smooth.Pools;
using UnityEngine;

[EffectDefinition("AUDIO_MULTI_POOL")]
internal class AudioMultiPooledFX : EffectBehaviour
{
	private class PooledAudioSource
	{
		[CompilerGenerated]
		private sealed class _003CCheckEnd_003Ed__12 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public PooledAudioSource _003C_003E4__this;

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
			public _003CCheckEnd_003Ed__12(int _003C_003E1__state)
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

		private static Pool<PooledAudioSource> Pool;

		public static int maxAudioSource;

		private GameObject host;

		private AudioSource source;

		private Action onRelease;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public PooledAudioSource()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		static PooledAudioSource()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static PooledAudioSource Create()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void Reset(PooledAudioSource pooledAudioSource)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static PooledAudioSource BorrowAndPlay(Transform parent, AudioClip clip, float volume, float pitch, AudioFX.AudioChannel channel, Action onRelease)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Update(float volume, AudioFX.AudioChannel channel)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Update(float volume, float pitch, AudioFX.AudioChannel channel)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void StopAndRelease()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[IteratorStateMachine(typeof(_003CCheckEnd_003Ed__12))]
		private IEnumerator CheckEnd()
		{
			throw null;
		}
	}

	[Persistent]
	public AudioFX.AudioChannel channel;

	[Persistent]
	public string clip;

	public FXCurve volume;

	public FXCurve pitch;

	[Persistent]
	public string transformName;

	private AudioClip clipAudio;

	private PooledAudioSource source;

	private float[] volumes;

	private List<Transform> modelParents;

	private bool isPaused;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AudioMultiPooledFX()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
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
	private void SetupAudioClip()
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
}
