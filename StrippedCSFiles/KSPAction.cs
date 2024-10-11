using System;
using System.Runtime.CompilerServices;

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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KSPAction(string guiName, KSPActionGroup actionGroup)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KSPAction(string guiName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KSPAction(string guiName, KSPActionGroup actionGroup, bool advancedTweakable)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KSPAction(string guiName, KSPActionGroup actionGroup, bool advancedTweakable, bool isPersistent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KSPAction()
	{
		throw null;
	}
}
