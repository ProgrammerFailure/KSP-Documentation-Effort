using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI.Screens.Settings.Controls;

public class SettingsLabel : SettingsControlReflection
{
	[SettingsValue("#FFE805FF")]
	public Color textColor;

	[SettingsValue(14)]
	public int fontSize;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SettingsLabel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnStart()
	{
		throw null;
	}
}
