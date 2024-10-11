using System.Runtime.CompilerServices;

namespace KSP.UI.Screens.Settings.Controls;

public class SettingsBoolToggle : SettingsControlReflection
{
	public UIButtonToggle toggle;

	[SettingsValue("True")]
	public string valueEnabled;

	[SettingsValue("False")]
	public string valueDisabled;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SettingsBoolToggle()
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
	protected override void ValueUpdated()
	{
		throw null;
	}
}
