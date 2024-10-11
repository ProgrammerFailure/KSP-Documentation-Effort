using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Serenity;
using UnityEngine;

[Serializable]
public class BaseAction
{
	public string name;

	[SerializeField]
	private string _guiName;

	public KSPActionGroup defaultActionGroup;

	public KSPActionGroup actionGroup;

	public KSPActionGroup[] overrideGroups;

	public bool active;

	public bool activeEditor;

	public bool requireFullControl;

	public bool advancedTweakable;

	public bool isPersistent;

	public bool wasActiveBeforePartWasAdjusted;

	public bool noLongerAssignable;

	[NonSerialized]
	public BaseActionList listParent;

	private static int actionGroupsLength;

	protected BaseActionDelegate onEvent
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

	public static int ActionGroupsLength
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public BaseAction(BaseActionList listParent, string name, BaseActionDelegate onEvent, KSPAction actionAttr)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static BaseAction()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KSPActionGroup GetActionGroup(int groupOverride)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void CopyAction(BaseAction action)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ContainsNonDefaultActions()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Invoke(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private KSPActionGroup ParseGroup(string groupName, KSPActionGroup defaultGroup)
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<BaseAction> CreateActionList(List<Part> parts, KSPActionGroup group, int overrideGroup, bool overrideDefault, bool include)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void AddAction(List<BaseAction> actionList, KSPActionGroup group, int overrideGroup, bool overrideDefault, bool include, BaseAction action)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<BaseAction> CreateActionList(Part p, KSPActionGroup group, int overrideGroup, bool overrideDefault, bool include)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<bool> CreateGroupList(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<bool> CreateGroupList(Part p, int overrideGroup)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int GetActionGroupsLength(float facilityLevel, bool isVAB = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string[] GetActionGroups(int maxlevel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<bool> CreateGroupList(List<Part> parts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<bool> CreateGroupList(List<Part> parts, int groupOverride)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool ContainsNonDefaultActions(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void FireAction(List<Part> parts, KSPActionGroup group, int overrideGroup, KSPActionType type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int GetGroupIndex(KSPActionGroup group)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<BaseAction> CreateActionList(List<Part> parts, ModuleRoboticController controller, bool include)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<BaseAction> CreateActionList(Part part, ModuleRoboticController controller, bool include)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void AddAction(List<BaseAction> actionList, ModuleRoboticController controller, Part part, bool include, BaseAction action)
	{
		throw null;
	}
}
