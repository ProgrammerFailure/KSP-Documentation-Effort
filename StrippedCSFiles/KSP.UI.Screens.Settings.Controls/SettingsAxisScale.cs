using System.Runtime.CompilerServices;
using UnityEngine.UI;

namespace KSP.UI.Screens.Settings.Controls;

public class SettingsAxisScale : SettingsControlReflection
{
	[SettingsValue(true)]
	public bool isPrimaryAxis;

	[SettingsValue(0f)]
	public float minValue;

	[SettingsValue(1f)]
	public float maxValue;

	[SettingsValue(1f)]
	public float displayMultiply;

	[SettingsValue("F0")]
	public string displayFormat;

	[SettingsValue("")]
	public string displayUnits;

	[SettingsValue(-1)]
	public int roundToPlaces;

	public Slider slider;

	private float axisSensitivity;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SettingsAxisScale()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnStart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SliderValueChange(float newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void ValueInitialized()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void ValueUpdated()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void PreApply()
	{
		throw null;
	}
}
