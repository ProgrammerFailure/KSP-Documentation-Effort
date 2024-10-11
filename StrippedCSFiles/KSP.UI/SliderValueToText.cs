using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI;

public class SliderValueToText : MonoBehaviour
{
	public Slider slider;

	public TextMeshProUGUI text;

	public string prefix;

	public string suffix;

	public float sliderValueMultiplier;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SliderValueToText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SliderValueChangeListener(float value)
	{
		throw null;
	}
}
