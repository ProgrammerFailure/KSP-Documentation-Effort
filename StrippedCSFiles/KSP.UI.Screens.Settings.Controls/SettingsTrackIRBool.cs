using System.Runtime.CompilerServices;

namespace KSP.UI.Screens.Settings.Controls;

public class SettingsTrackIRBool : SettingsControlReflection
{
	public UIButtonToggle toggle;

	[SettingsValue("True")]
	public string valueEnabled;

	[SettingsValue("False")]
	public string valueDisabled;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SettingsTrackIRBool()
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override AccessorBase GetAccessor(string settingName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void GetValue()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void SetValue()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SettingChanged(string settingString)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void EnableInteraction(bool enabled)
	{
		throw null;
	}
}
