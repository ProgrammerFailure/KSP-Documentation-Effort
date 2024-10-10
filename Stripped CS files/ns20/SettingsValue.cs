using System;

namespace ns20;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class SettingsValue : Attribute
{
	public object defaultValue;

	public SettingsValue(object defaultValue)
	{
		this.defaultValue = defaultValue;
	}
}
