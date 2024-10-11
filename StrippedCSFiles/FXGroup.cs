using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class FXGroup
{
	[CompilerGenerated]
	private sealed class _003CVolumeFade_003Ed__31 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public AudioSource audioSource;

		public float fadeLength;

		public float endVolume;

		private float _003CstartTime_003E5__2;

		private float _003CstartVolume_003E5__3;

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
		public _003CVolumeFade_003Ed__31(int _003C_003E1__state)
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

	public List<ParticleSystem> fxEmittersNewSystem;

	public List<Light> lights;

	public AudioClip sfx;

	public AudioSource audio;

	private List<float> initSizeValuesNewSystem;

	private List<float> initLifeValuesNewSystem;

	private List<float> initLightValues;

	private List<float> initSizeValuesNewSystemVariation;

	private List<float> initLifeValuesNewSystemVariation;

	private bool valid;

	public string name;

	private float _minVisualPower;

	private float _maxVisualPower;

	public bool clampVisualPower;

	private bool active;

	private IEnumerator fadeCoroutine;

	private CoroutineHost fadeCoroutineHost;

	private float power;

	public float powerVariation;

	public bool activeLatch;

	public bool isValid
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float minVisualPower
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public float maxVisualPower
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public bool Active
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float Power
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FXGroup(string groupID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void begin(AudioSource audioRef)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void setActive(bool value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void setActiveWithAudioFade(bool value, float audioFadeLength)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CVolumeFade_003Ed__31))]
	private IEnumerator VolumeFade(AudioSource audioSource, float endVolume, float fadeLength)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Burst()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetVisualMinMax(float min, float max)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetLatch(bool latch)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetPower(float pwr)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetPowerLatch(float pwr)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Unlatch()
	{
		throw null;
	}
}
