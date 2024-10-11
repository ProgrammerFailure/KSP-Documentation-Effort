using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ColorSlider : MonoBehaviour
{
	public ColorPicker hsvpicker;

	public ColorValues type;

	private Slider slider;

	private bool listen;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ColorSlider()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ColorChanged(Color newColor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void HSVChanged(float hue, float saturation, float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SliderChanged(float newValue)
	{
		throw null;
	}
}
