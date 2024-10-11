using System;
using System.Runtime.CompilerServices;
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
	private string _guiName;

	public string category;

	public bool advancedTweakable;

	public bool isPersistent;

	public bool wasActiveBeforePartWasAdjusted;

	public string groupName;

	public string groupDisplayName;

	public bool groupStartCollapsed;

	public string guiName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KSPEvent()
	{
		throw null;
	}
}
