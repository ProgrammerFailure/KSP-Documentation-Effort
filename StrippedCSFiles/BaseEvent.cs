using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BaseEvent
{
	public bool active;

	public bool guiActive;

	public bool requireFullControl;

	public bool guiActiveEditor;

	public bool guiActiveUncommand;

	public string guiIcon;

	[SerializeField]
	private string _guiName;

	public string category;

	public bool guiActiveUnfocused;

	public float unfocusedRange;

	public bool externalToEVAOnly;

	public bool advancedTweakable;

	public bool isPersistent;

	public bool wasActiveBeforePartWasAdjusted;

	[NonSerialized]
	public BaseEventList listParent;

	public bool assigned;

	private bool eventIsDisabledByVariant;

	public BasePAWGroup group;

	public int id
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public string name
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	protected BaseEventDelegate onEvent
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	protected BaseEventDetailsDelegate onDataEvent
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

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

	public string GUIName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool EventIsDisabledByVariant
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public BaseEvent(BaseEventList listParent, string name, BaseEventDelegate onEvent, KSPEvent actionAttr)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public BaseEvent(BaseEventList listParent, string name, BaseEventDetailsDelegate onDataEvent, KSPEvent actionAttr)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void VariantToggleEventDisabled(bool disabled)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Invoke()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Invoke(BaseEventDetails data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string ToString()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnSave(ConfigNode node)
	{
		throw null;
	}
}
