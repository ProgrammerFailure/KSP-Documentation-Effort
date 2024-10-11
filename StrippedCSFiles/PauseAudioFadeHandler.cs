using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PauseAudioFadeHandler : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CPauseWithFadeRoutine_003Ed__15 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public PauseAudioFadeHandler _003C_003E4__this;

		public AudioSource fadeSource;

		public float fadeDuration;

		public AudioClip fadeClip;

		public float fadeClipTime;

		public bool fadeClipLoop;

		public float fadeClipVolume;

		private float _003Ctime_003E5__2;

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
		public _003CPauseWithFadeRoutine_003Ed__15(int _003C_003E1__state)
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

	public AudioSource Source;

	private AudioClip pausedClip;

	private float pausedVolume;

	private bool pausedLoop;

	private float pausedTime;

	private Coroutine fadeRoutine;

	[HideInInspector]
	public bool IsPaused
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	[HideInInspector]
	public bool IsFading
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PauseAudioFadeHandler()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PauseWithFade(float fadeDuration, AudioClip fadeClip, float fadeClipVolume, float fadeClipTime, bool fadeClipLoop)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CPauseWithFadeRoutine_003Ed__15))]
	private IEnumerator PauseWithFadeRoutine(AudioSource fadeSource, float fadeDuration, AudioClip fadeClip, float fadeClipVolume, float fadeClipTime, bool fadeClipLoop)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UnpauseWithFade()
	{
		throw null;
	}
}
