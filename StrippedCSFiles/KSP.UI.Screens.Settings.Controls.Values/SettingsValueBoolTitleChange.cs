using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI.Screens.Settings.Controls.Values;

public class SettingsValueBoolTitleChange : MonoBehaviour
{
	[SettingsValue("True")]
	public string titleEnabled;

	[SettingsValue("False")]
	public string titleDisabled;

	private SettingsControlReflection control;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SettingsValueBoolTitleChange()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateValue(object value)
	{
		throw null;
	}
}
