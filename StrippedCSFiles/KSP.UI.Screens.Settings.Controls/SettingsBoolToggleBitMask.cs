using System.Runtime.CompilerServices;

namespace KSP.UI.Screens.Settings.Controls;

public class SettingsBoolToggleBitMask : SettingsControlReflection
{
	public UIButtonToggle toggle;

	[SettingsValue(0)]
	public int bitMask;

	[SettingsValue("Enabled")]
	public string valueOn;

	[SettingsValue("Disabled")]
	public string valueOff;

	private bool isOn;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SettingsBoolToggleBitMask()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnStart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnToggled()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void ValueInitialized()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnApply()
	{
		throw null;
	}
}
