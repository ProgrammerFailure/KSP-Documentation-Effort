using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI.Screens.Settings.Controls.Values;

public class SettingsValueIntToString : MonoBehaviour
{
	[SettingsValue(new string[] { "A", "B" })]
	public string[] values;

	private SettingsControlReflection control;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SettingsValueIntToString()
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
