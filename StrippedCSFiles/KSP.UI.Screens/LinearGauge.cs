using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI.Screens;

public class LinearGauge : MonoBehaviour
{
	public RectTransform pointer;

	public float minValue;

	public Vector2 minValuePosition;

	public float maxValue;

	public Vector2 maxValuePosition;

	public float sharpness;

	public float logarithmic;

	public float exponential;

	private float currentValue;

	[SerializeField]
	private float value;

	public float Value
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
	public LinearGauge()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetValue(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetValue(double value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Reset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}
}
