using System;
using ns9;
using UnityEngine;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class FieldAttribute : Attribute
{
	[SerializeField]
	public string _guiName;

	public string guiName
	{
		get
		{
			return _guiName;
		}
		set
		{
			_guiName = Localizer.Format(value);
		}
	}

	public FieldAttribute()
	{
		guiName = "";
	}
}
