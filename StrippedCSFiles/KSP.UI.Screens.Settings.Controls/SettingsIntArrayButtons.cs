using System.Runtime.CompilerServices;
using UnityEngine.UI;

namespace KSP.UI.Screens.Settings.Controls;

public class SettingsIntArrayButtons : SettingsControlReflection
{
	public Button buttonUp;

	public Button buttonDown;

	[SettingsValue(new int[] { 0, 1 })]
	public int[] values;

	[SettingsValue(new string[] { "A", "B" })]
	public string[] stringValues;

	[SettingsValue(false)]
	public bool displayStringValue;

	protected int currentValue;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SettingsIntArrayButtons()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnStart()
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
	private void OnButtonUp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnButtonDown()
	{
		throw null;
	}
}
