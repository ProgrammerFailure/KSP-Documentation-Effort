using System;
using Expansions;
using ns9;
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
	public string _guiName;

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

	public bool eventIsDisabledByVariant;

	public BasePAWGroup group;

	public int id { get; set; }

	public string name { get; set; }

	public BaseEventDelegate onEvent { get; set; }

	public BaseEventDetailsDelegate onDataEvent { get; set; }

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

	public string GUIName
	{
		get
		{
			if (string.IsNullOrEmpty(guiName))
			{
				return name;
			}
			return guiName;
		}
	}

	public bool EventIsDisabledByVariant => eventIsDisabledByVariant;

	public BaseEvent(BaseEventList listParent, string name, BaseEventDelegate onEvent, KSPEvent actionAttr)
	{
		this.name = name;
		id = name.GetHashCode();
		this.onEvent = onEvent;
		active = actionAttr.active;
		guiActive = actionAttr.guiActive;
		requireFullControl = actionAttr.requireFullControl;
		guiActiveEditor = actionAttr.guiActiveEditor;
		guiActiveUncommand = actionAttr.guiActiveUncommand;
		guiIcon = actionAttr.guiName;
		guiName = actionAttr.guiName;
		category = actionAttr.category;
		advancedTweakable = actionAttr.advancedTweakable;
		guiActiveUnfocused = actionAttr.guiActiveUnfocused;
		unfocusedRange = actionAttr.unfocusedRange;
		externalToEVAOnly = actionAttr.externalToEVAOnly;
		isPersistent = actionAttr.isPersistent;
		wasActiveBeforePartWasAdjusted = actionAttr.wasActiveBeforePartWasAdjusted;
		this.listParent = listParent;
		group = new BasePAWGroup(actionAttr.groupName, actionAttr.groupDisplayName, actionAttr.groupStartCollapsed);
	}

	public BaseEvent(BaseEventList listParent, string name, BaseEventDetailsDelegate onDataEvent, KSPEvent actionAttr)
	{
		this.name = name;
		id = name.GetHashCode();
		this.onDataEvent = onDataEvent;
		active = actionAttr.active;
		guiActive = actionAttr.guiActive;
		requireFullControl = actionAttr.requireFullControl;
		guiActiveEditor = actionAttr.guiActiveEditor;
		guiActiveUncommand = actionAttr.guiActiveUncommand;
		guiIcon = actionAttr.guiName;
		guiName = actionAttr.guiName;
		category = actionAttr.category;
		advancedTweakable = actionAttr.advancedTweakable;
		guiActiveUnfocused = actionAttr.guiActiveUnfocused;
		unfocusedRange = actionAttr.unfocusedRange;
		externalToEVAOnly = actionAttr.externalToEVAOnly;
		isPersistent = actionAttr.isPersistent;
		wasActiveBeforePartWasAdjusted = actionAttr.wasActiveBeforePartWasAdjusted;
		this.listParent = listParent;
		group = new BasePAWGroup(actionAttr.groupName, actionAttr.groupDisplayName, actionAttr.groupStartCollapsed);
	}

	public void VariantToggleEventDisabled(bool disabled)
	{
		eventIsDisabledByVariant = disabled;
	}

	public void Invoke()
	{
		if (onEvent != null)
		{
			onEvent();
		}
		else if (onDataEvent != null)
		{
			onDataEvent(null);
		}
		else
		{
			PDebug.Error("Action event delegate is null");
		}
	}

	public void Invoke(BaseEventDetails data)
	{
		if (onEvent != null)
		{
			onEvent();
		}
		else if (onDataEvent != null)
		{
			onDataEvent(data);
		}
		else
		{
			PDebug.Error("Action event delegate is null");
		}
	}

	public override string ToString()
	{
		return name + " (Active:" + active + ", GUIActive:" + guiActive + ")";
	}

	public void OnLoad(ConfigNode node)
	{
		string text = "";
		text = node.GetValue("active");
		if (text != null)
		{
			active = bool.Parse(text);
		}
		text = node.GetValue("guiActive");
		if (text != null)
		{
			guiActive = bool.Parse(text);
		}
		text = node.GetValue("guiActiveEditor");
		if (text != null)
		{
			guiActiveEditor = bool.Parse(text);
		}
		text = node.GetValue("requireFullControl");
		if (text != null)
		{
			requireFullControl = bool.Parse(text);
		}
		text = node.GetValue("guiActiveUncommand");
		if (text != null)
		{
			guiActiveUncommand = bool.Parse(text);
		}
		text = node.GetValue("advancedTweakable");
		if (text != null)
		{
			advancedTweakable = bool.Parse(text);
		}
		text = node.GetValue("guiIcon");
		if (text != null)
		{
			guiIcon = text;
		}
		text = node.GetValue("guiName");
		if (text != null)
		{
			guiName = text;
		}
		text = node.GetValue("category");
		if (text != null)
		{
			category = text;
		}
		text = node.GetValue("guiActiveUnfocused");
		if (text != null)
		{
			guiActiveUnfocused = bool.Parse(text);
		}
		text = node.GetValue("unfocusedRange");
		if (text != null)
		{
			unfocusedRange = float.Parse(text);
		}
		text = node.GetValue("externalToEVAOnly");
		if (text != null)
		{
			externalToEVAOnly = bool.Parse(text);
		}
		if (ExpansionsLoader.IsExpansionInstalled("MakingHistory"))
		{
			text = node.GetValue("wasActiveBeforePartWasAdjusted");
			if (text != null)
			{
				wasActiveBeforePartWasAdjusted = bool.Parse(text);
			}
		}
	}

	public void OnSave(ConfigNode node)
	{
		node.AddValue("active", active.ToString());
		node.AddValue("guiActive", guiActive.ToString());
		if (requireFullControl)
		{
			node.AddValue("requireFullControl", requireFullControl.ToString());
		}
		if (guiActiveEditor)
		{
			node.AddValue("guiActiveEditor", guiActiveEditor.ToString());
		}
		node.AddValue("guiActiveUncommand", guiActiveUncommand.ToString());
		node.AddValue("guiIcon", guiIcon);
		node.AddValue("guiName", guiName);
		node.AddValue("category", guiIcon);
		node.AddValue("advancedTweakable", advancedTweakable);
		node.AddValue("guiActiveUnfocused", guiActiveUnfocused.ToString());
		node.AddValue("unfocusedRange", unfocusedRange.ToString());
		node.AddValue("externalToEVAOnly", externalToEVAOnly.ToString());
		if (ExpansionsLoader.IsExpansionInstalled("MakingHistory"))
		{
			node.AddValue("wasActiveBeforePartWasAdjusted", wasActiveBeforePartWasAdjusted.ToString());
		}
	}
}
