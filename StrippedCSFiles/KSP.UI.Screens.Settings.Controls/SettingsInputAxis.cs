using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine.UI;

namespace KSP.UI.Screens.Settings.Controls;

public class SettingsInputAxis : SettingsControlReflection
{
	public Button primaryButton;

	public TextMeshProUGUI primaryButtonText;

	public UIButtonToggle primaryInvert;

	public Slider primarySensitivity;

	public TextMeshProUGUI primarySensitivityText;

	public Slider primaryDeadzone;

	public TextMeshProUGUI primaryDeadzoneText;

	public Button secondaryButton;

	public TextMeshProUGUI secondaryButtonText;

	public UIButtonToggle secondaryInvert;

	public Slider secondarySensitivity;

	public TextMeshProUGUI secondarySensitivityText;

	public Slider secondaryDeadzone;

	public TextMeshProUGUI secondaryDeadzoneText;

	public EventData<object, string, SettingsInputBinding.BindingType, SettingsInputBinding.BindingVariant> OnTryBind;

	private AxisBinding axis;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SettingsInputAxis()
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
	protected void OnClickPrimary()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnTogglePrimaryInvert()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnChangePrimarySensitivity(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnChangePrimaryDeadzone(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnClickSecondary()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnToggleSecondaryInvert()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnChangeSecondarySensitivity(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnChangeSecondaryDeadzone(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateInputSettings()
	{
		throw null;
	}
}
