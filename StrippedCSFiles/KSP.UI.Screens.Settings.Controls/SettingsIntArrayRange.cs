using System.Runtime.CompilerServices;
using UnityEngine.UI;

namespace KSP.UI.Screens.Settings.Controls;

public class SettingsIntArrayRange : SettingsControlReflection
{
	[SettingsValue(new int[] { 0, 1 })]
	public int[] values;

	[SettingsValue(new string[] { "A", "B" })]
	public string[] stringValues;

	[SettingsValue(false)]
	public bool displayStringValue;

	public Slider slider;

	protected int currentValue;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SettingsIntArrayRange()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnStart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSliderValueChange(float newValue)
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
}
