using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

namespace TMPro;

internal struct ColorTween : ITweenValue
{
	public enum ColorTweenMode
	{
		All,
		RGB,
		Alpha
	}

	public class ColorTweenCallback : UnityEvent<Color>
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public ColorTweenCallback()
		{
			throw null;
		}
	}

	private ColorTweenCallback m_Target;

	private Color m_StartColor;

	private Color m_TargetColor;

	private ColorTweenMode m_TweenMode;

	private float m_Duration;

	private bool m_IgnoreTimeScale;

	public Color startColor
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

	public Color targetColor
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

	public ColorTweenMode tweenMode
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
	public void AddOnChangedCallback(UnityAction<Color> callback)
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
