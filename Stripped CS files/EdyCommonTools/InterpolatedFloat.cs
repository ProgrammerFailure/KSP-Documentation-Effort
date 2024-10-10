using UnityEngine;

namespace EdyCommonTools;

public class InterpolatedFloat
{
	public float m_prevValue;

	public float m_value;

	public InterpolatedFloat(float initialValue = 0f)
	{
		Reset(initialValue);
	}

	public void Reset(float value)
	{
		m_prevValue = value;
		m_value = value;
	}

	public void Set(float value)
	{
		m_prevValue = m_value;
		m_value = value;
	}

	public float Get()
	{
		return m_value;
	}

	public static float GetFrameRatio()
	{
		return Mathf.InverseLerp(Time.fixedTime, Time.fixedTime + Time.fixedDeltaTime, Time.time);
	}

	public float GetInterpolated()
	{
		float frameRatio = GetFrameRatio();
		return Mathf.Lerp(m_prevValue, m_value, frameRatio);
	}

	public float GetInterpolated(float t)
	{
		return Mathf.Lerp(m_prevValue, m_value, t);
	}
}
