using System.Reflection;
using System.Runtime.CompilerServices;

namespace KSP.UI.Screens.Settings;

public class SettingsFieldWrapper
{
	public FieldInfo field;

	public object defaultValue;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SettingsFieldWrapper(FieldInfo field, SettingsValue attr)
	{
		throw null;
	}
}
