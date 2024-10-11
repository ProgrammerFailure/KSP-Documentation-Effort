using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class VFXSequencer : MonoBehaviour
{
	[Serializable]
	public class SequenceFX
	{
		public float startTime;

		public float startTimeVariation;

		public float scheduled;

		public ParticleSystem particleSystem;

		public AudioClip audioFx;

		public AudioSource audioSource;

		public float pitchVariance;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public SequenceFX()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Play()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public float GetFXDuration()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public float getSubFXHierarchyDuration(ParticleSystem ps, float d)
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CplaySequence_003Ed__13 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public VFXSequencer _003C_003E4__this;

		public List<SequenceFX> fxList;

		public Callback<VFXSequencer> onComplete;

		private float _003Ct_003E5__2;

		private float _003CtToComplete_003E5__3;

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
		public _003CplaySequence_003Ed__13(int _003C_003E1__state)
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

	public bool PlayOnStart;

	public bool SelfDestructOnComplete;

	public List<SequenceFX> FXList;

	private bool isPlaying;

	private bool isComplete;

	public bool IsPlaying
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsComplete
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VFXSequencer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Play")]
	public void Play()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Play(Callback<VFXSequencer> onComplete)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CplaySequence_003Ed__13))]
	private IEnumerator playSequence(List<SequenceFX> fxList, Callback<VFXSequencer> onComplete)
	{
		throw null;
	}
}
