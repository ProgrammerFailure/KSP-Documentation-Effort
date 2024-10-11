using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens.DebugToolbar.Screens;

public class DebugScreenSlider : MonoBehaviour
{
	public Slider slider;

	public float sliderMin;

	public float sliderMax;

	public float sliderDefault;

	public TextMeshProUGUI sliderText;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DebugScreenSlider()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetSlider(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetSliderText(string text)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SetupValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnSliderChanged(float value)
	{
		throw null;
	}
}
