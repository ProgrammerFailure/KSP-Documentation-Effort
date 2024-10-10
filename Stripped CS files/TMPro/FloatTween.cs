using UnityEngine;
using UnityEngine.Events;

namespace TMPro;

public struct FloatTween : ITweenValue
{
	public class FloatTweenCallback : UnityEvent<float>
	{
	}

	public FloatTweenCallback m_Target;

	public float m_StartValue;

	public float m_TargetValue;

	public float m_Duration;

	public bool m_IgnoreTimeScale;

	public float startValue
	{
		get
		{
			return m_StartValue;
		}
		set
		{
			m_StartValue = value;
		}
	}

	public float targetValue
	{
		get
		{
			return m_TargetValue;
		}
		set
		{
			m_TargetValue = value;
		}
	}

	public float duration
	{
		get
		{
			return m_Duration;
		}
		set
		{
			m_Duration = value;
		}
	}

	public bool ignoreTimeScale
	{
		get
		{
			return m_IgnoreTimeScale;
		}
		set
		{
			m_IgnoreTimeScale = value;
		}
	}

	public void TweenValue(float floatPercentage)
	{
		if (ValidTarget())
		{
			float arg = Mathf.Lerp(m_StartValue, m_TargetValue, floatPercentage);
			m_Target.Invoke(arg);
		}
	}

	public void AddOnChangedCallback(UnityAction<float> callback)
	{
		if (m_Target == null)
		{
			m_Target = new FloatTweenCallback();
		}
		m_Target.AddListener(callback);
	}

	public bool GetIgnoreTimescale()
	{
		return m_IgnoreTimeScale;
	}

	public float GetDuration()
	{
		return m_Duration;
	}

	public bool ValidTarget()
	{
		return m_Target != null;
	}
}
