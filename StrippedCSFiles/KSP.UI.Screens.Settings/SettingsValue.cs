using System;
using System.Runtime.CompilerServices;

namespace KSP.UI.Screens.Settings;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class SettingsValue : Attribute
{
	public object defaultValue;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SettingsValue(object defaultValue)
	{
		throw null;
	}
}
