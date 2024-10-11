using System.Runtime.CompilerServices;

namespace KSP.UI.Screens.Settings.Controls;

public class SettingsBoolTogglePPFX : SettingsControlReflection
{
	public UIButtonToggle toggle;

	[SettingsValue("True")]
	public string valueEnabled;

	[SettingsValue("False")]
	public string valueDisabled;

	protected bool supported;

	protected bool settingsSupported;

	protected bool interactable;

	protected SettingsIntArrayButtons aaField;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SettingsBoolTogglePPFX()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnStart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupAdditionalFields()
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
	private void Update()
	{
		throw null;
	}
}
