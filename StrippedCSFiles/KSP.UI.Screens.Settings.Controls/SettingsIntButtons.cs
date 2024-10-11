using System.Runtime.CompilerServices;
using UnityEngine.UI;

namespace KSP.UI.Screens.Settings.Controls;

public class SettingsIntButtons : SettingsControlReflection
{
	public Button buttonUp;

	public Button buttonDown;

	[SettingsValue(0)]
	public int minValue;

	[SettingsValue(1)]
	public int maxValue;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SettingsIntButtons()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnStart()
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
