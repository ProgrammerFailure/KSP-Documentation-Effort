using System.Reflection;

namespace ns20;

public class SettingsFieldWrapper
{
	public FieldInfo field;

	public object defaultValue;

	public SettingsFieldWrapper(FieldInfo field, SettingsValue attr)
	{
		this.field = field;
		defaultValue = attr.defaultValue;
	}
}
