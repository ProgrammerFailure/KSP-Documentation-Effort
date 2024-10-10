using System;
using ns9;
using UnityEngine;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class KSPEvent : Attribute
{
	public string name;

	public bool isDefault;

	public bool active;

	public bool guiActive;

	public bool requireFullControl;

	public bool guiActiveEditor;

	public bool guiActiveUncommand;

	public bool guiActiveUnfocused;

	public float unfocusedRange;

	public bool externalToEVAOnly;

	public string guiIcon;

	[SerializeField]
	public string _guiName;

	public string category;

	public bool advancedTweakable;

	public bool isPersistent;

	public bool wasActiveBeforePartWasAdjusted;

	public string groupName;

	public string groupDisplayName;

	public bool groupStartCollapsed;

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

	public KSPEvent()
	{
		name = "";
		isDefault = false;
		active = true;
		guiActive = false;
		advancedTweakable = false;
		guiIcon = "";
		guiName = "";
		category = "";
		guiActiveUnfocused = false;
		guiActiveUncommand = false;
		externalToEVAOnly = true;
		unfocusedRange = 2f;
		isPersistent = false;
		wasActiveBeforePartWasAdjusted = false;
		groupName = "";
		groupDisplayName = "";
		groupStartCollapsed = false;
	}
}
