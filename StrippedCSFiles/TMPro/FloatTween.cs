using System.Runtime.CompilerServices;
using UnityEngine.Events;

namespace TMPro;

internal struct FloatTween : ITweenValue
{
	public class FloatTweenCallback : UnityEvent<float>
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public FloatTweenCallback()
		{
			throw null;
		}
	}

	private FloatTweenCallback m_Target;

	private float m_StartValue;

	private float m_TargetValue;

	private float m_Duration;

	private bool m_IgnoreTimeScale;

	public float startValue
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

	public float targetValue
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

	public float duration
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

	public bool ignoreTimeScale
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
	public void TweenValue(float floatPercentage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddOnChangedCallback(UnityAction<float> callback)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool GetIgnoreTimescale()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetDuration()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ValidTarget()
	{
		throw null;
	}
}
