using UnityEngine;

namespace EdyCommonTools;

public class BiasedRatio
{
	public float m_lastBias = -1f;

	public float m_lastExponent;

	public float Bias(float x, float bias)
	{
		if (x <= 0f)
		{
			return 0f;
		}
		if (x >= 1f)
		{
			return 1f;
		}
		if (bias != m_lastBias)
		{
			if (bias <= 0f)
			{
				if (!(x >= 1f))
				{
					return 0f;
				}
				return 1f;
			}
			if (bias >= 1f)
			{
				if (!(x > 0f))
				{
					return 0f;
				}
				return 1f;
			}
			if (bias == 0.5f)
			{
				return x;
			}
			m_lastExponent = Mathf.Log(bias) * -1.442695f;
			m_lastBias = bias;
		}
		return Mathf.Pow(x, m_lastExponent);
	}

	public float BiasedLerp(float x, float bias)
	{
		float num = ((bias <= 0.5f) ? Bias(MathUtility.FastAbs(x), bias) : (1f - Bias(1f - MathUtility.FastAbs(x), 1f - bias)));
		if (!(x < 0f))
		{
			return num;
		}
		return 0f - num;
	}

	public float BiasedLerp(float from, float to, float t, float bias)
	{
		return from + (to - from) * BiasedLerp(t, bias);
	}

	public static float BiasRaw(float x, float bias)
	{
		if (x <= 0f)
		{
			return 0f;
		}
		if (x >= 1f)
		{
			return 1f;
		}
		if (bias <= 0f)
		{
			if (!(x >= 1f))
			{
				return 0f;
			}
			return 1f;
		}
		if (bias >= 1f)
		{
			if (!(x > 0f))
			{
				return 0f;
			}
			return 1f;
		}
		if (bias == 0.5f)
		{
			return x;
		}
		float p = Mathf.Log(bias) * -1.442695f;
		return Mathf.Pow(x, p);
	}

	public static float BiasedLerpRaw(float x, float bias)
	{
		float num = ((bias <= 0.5f) ? BiasRaw(MathUtility.FastAbs(x), bias) : (1f - BiasRaw(1f - MathUtility.FastAbs(x), 1f - bias)));
		if (!(x < 0f))
		{
			return num;
		}
		return 0f - num;
	}

	public static float BiasedLerpRaw(float from, float to, float t, float bias)
	{
		return from + (to - from) * BiasedLerpRaw(t, bias);
	}
}
