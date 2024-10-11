using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine.UI;

namespace KSP.UI.Screens.Settings.Controls;

public class SettingsTerrainDetail : SettingsControlBase
{
	public Slider slider;

	public TextMeshProUGUI valueText;

	[SettingsValue(new int[] { 0, 1 })]
	public int[] values;

	[SettingsValue(new string[] { "A", "B" })]
	public string[] stringValues;

	[SettingsValue(false)]
	public bool displayStringValue;

	private int value;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SettingsTerrainDetail()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSliderValueChange(float newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnApply()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnRevert()
	{
		throw null;
	}
}
