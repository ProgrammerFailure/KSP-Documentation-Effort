using System;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class KSPAction : Attribute
{
	public string guiName;

	public bool requireFullControl;

	public bool advancedTweakable;

	public bool isPersistent;

	public bool wasActiveBeforePartWasAdjusted;

	public KSPActionGroup actionGroup;

	public bool activeEditor;

	public bool noLongerAssignable;

	public KSPAction(string guiName, KSPActionGroup actionGroup)
	{
		this.guiName = guiName;
		this.actionGroup = actionGroup;
		isPersistent = true;
		advancedTweakable = false;
		wasActiveBeforePartWasAdjusted = false;
		activeEditor = true;
		noLongerAssignable = false;
	}

	public KSPAction(string guiName)
	{
		this.guiName = guiName;
		actionGroup = KSPActionGroup.None;
		isPersistent = true;
		advancedTweakable = false;
		wasActiveBeforePartWasAdjusted = false;
		activeEditor = true;
		noLongerAssignable = false;
	}

	public KSPAction(string guiName, KSPActionGroup actionGroup, bool advancedTweakable)
	{
		this.guiName = guiName;
		this.actionGroup = actionGroup;
		this.advancedTweakable = advancedTweakable;
		isPersistent = true;
		wasActiveBeforePartWasAdjusted = false;
		activeEditor = true;
		noLongerAssignable = false;
	}

	public KSPAction(string guiName, KSPActionGroup actionGroup, bool advancedTweakable, bool isPersistent)
	{
		this.guiName = guiName;
		this.actionGroup = actionGroup;
		this.advancedTweakable = advancedTweakable;
		this.isPersistent = isPersistent;
		wasActiveBeforePartWasAdjusted = false;
		activeEditor = true;
		noLongerAssignable = false;
	}

	public KSPAction()
	{
	}
}
