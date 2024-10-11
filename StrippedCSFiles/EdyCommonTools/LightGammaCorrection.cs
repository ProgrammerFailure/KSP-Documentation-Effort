using System.Runtime.CompilerServices;
using UnityEngine;

namespace EdyCommonTools;

[RequireComponent(typeof(Light))]
public class LightGammaCorrection : MonoBehaviour
{
	public ColorSpace intendedColorSpace;

	[Range(0.5f, 1.5f)]
	public float extraCorrectionFactor;

	private Light m_light;

	private float m_originalIntensity;

	private Color m_originalColor;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LightGammaCorrection()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDisable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float LinearToGamma(float l)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float GammaToLinear(float g)
	{
		throw null;
	}
}
