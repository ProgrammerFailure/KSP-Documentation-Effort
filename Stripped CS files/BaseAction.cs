using System;
using System.Collections.Generic;
using Expansions;
using Expansions.Serenity;
using ns9;
using UnityEngine;

[Serializable]
public class BaseAction
{
	public string name;

	[SerializeField]
	public string _guiName;

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

	public static int actionGroupsLength = -1;

	public BaseActionDelegate onEvent { get; set; }

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

	public static int ActionGroupsLength
	{
		get
		{
			if (actionGroupsLength == -1)
			{
				actionGroupsLength = Enum.GetNames(typeof(KSPActionGroup)).Length - 2;
			}
			return actionGroupsLength;
		}
	}

	public BaseAction(BaseActionList listParent, string name, BaseActionDelegate onEvent, KSPAction actionAttr)
	{
		this.name = name;
		this.listParent = listParent;
		this.onEvent = onEvent;
		defaultActionGroup = actionAttr.actionGroup;
		actionGroup = actionAttr.actionGroup;
		guiName = actionAttr.guiName;
		requireFullControl = actionAttr.requireFullControl;
		overrideGroups = new KSPActionGroup[Vessel.NumOverrideGroups];
		active = true;
		advancedTweakable = actionAttr.advancedTweakable;
		isPersistent = actionAttr.isPersistent;
		wasActiveBeforePartWasAdjusted = actionAttr.wasActiveBeforePartWasAdjusted;
		activeEditor = actionAttr.activeEditor;
		noLongerAssignable = actionAttr.noLongerAssignable;
	}

	public KSPActionGroup GetActionGroup(int groupOverride)
	{
		if (groupOverride > 0 && groupOverride <= overrideGroups.Length)
		{
			return overrideGroups[groupOverride - 1];
		}
		return actionGroup;
	}

	public virtual void CopyAction(BaseAction action)
	{
		actionGroup = action.actionGroup;
		int num = overrideGroups.Length;
		while (num-- > 0)
		{
			overrideGroups[num] = action.overrideGroups[num];
		}
	}

	public bool ContainsNonDefaultActions()
	{
		if (actionGroup != defaultActionGroup)
		{
			return true;
		}
		int num = 0;
		while (true)
		{
			if (num < overrideGroups.Length)
			{
				if (overrideGroups[num] != 0)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public void Invoke(KSPActionParam param)
	{
		if (onEvent != null)
		{
			onEvent(param);
		}
		else
		{
			PDebug.Error("Action event delegate is null");
		}
	}

	public KSPActionGroup ParseGroup(string groupName, KSPActionGroup defaultGroup)
	{
		KSPActionGroup result = defaultGroup;
		if (!string.IsNullOrEmpty(groupName))
		{
			try
			{
				result = (KSPActionGroup)Enum.Parse(typeof(KSPActionGroup), groupName);
			}
			catch (ArgumentException)
			{
			}
		}
		return result;
	}

	public void OnLoad(ConfigNode node)
	{
		string text = "";
		text = node.GetValue("actionGroup");
		actionGroup = ParseGroup(text, defaultActionGroup);
		for (int i = 0; i < overrideGroups.Length; i++)
		{
			text = "overrideGroup" + i;
			overrideGroups[i] = ParseGroup(node.GetValue(text), KSPActionGroup.None);
		}
		text = node.GetValue("active");
		if (text != null)
		{
			bool result = true;
			if (bool.TryParse(text, out result))
			{
				active = result;
			}
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
		node.AddValue("actionGroup", actionGroup.ToStringCached());
		if (isPersistent && !active)
		{
			node.AddValue("active", false.ToString());
		}
		for (int i = 0; i < overrideGroups.Length; i++)
		{
			if (overrideGroups[i] != 0)
			{
				string value = overrideGroups[i].ToStringCached();
				node.AddValue("overrideGroup" + i, value);
			}
		}
		if (ExpansionsLoader.IsExpansionInstalled("MakingHistory"))
		{
			node.AddValue("wasActiveBeforePartWasAdjusted", wasActiveBeforePartWasAdjusted.ToString());
		}
	}

	public static List<BaseAction> CreateActionList(List<Part> parts, KSPActionGroup group, int overrideGroup, bool overrideDefault, bool include)
	{
		List<BaseAction> list = new List<BaseAction>();
		int count = parts.Count;
		for (int i = 0; i < count; i++)
		{
			list.AddRange(CreateActionList(parts[i], group, overrideGroup, overrideDefault, include));
		}
		return list;
	}

	public static void AddAction(List<BaseAction> actionList, KSPActionGroup group, int overrideGroup, bool overrideDefault, bool include, BaseAction action)
	{
		if (action.active && (GameSettings.ADVANCED_TWEAKABLES || !action.advancedTweakable))
		{
			bool flag = false;
			if (overrideGroup > 0 && overrideGroup <= action.overrideGroups.Length)
			{
				flag = (action.overrideGroups[overrideGroup - 1] & group) != 0;
			}
			if (overrideGroup == 0 || (!overrideDefault && include))
			{
				flag |= (action.actionGroup & group) != 0;
			}
			if (include == flag)
			{
				actionList.Add(action);
			}
		}
	}

	public static List<BaseAction> CreateActionList(Part p, KSPActionGroup group, int overrideGroup, bool overrideDefault, bool include)
	{
		List<BaseAction> list = new List<BaseAction>();
		int count = p.Actions.Count;
		for (int i = 0; i < count; i++)
		{
			BaseAction action = p.Actions[i];
			AddAction(list, group, overrideGroup, overrideDefault, include, action);
		}
		int count2 = p.Modules.Count;
		for (int j = 0; j < count2; j++)
		{
			PartModule partModule = p.Modules[j];
			if (partModule.isEnabled)
			{
				int count3 = partModule.Actions.Count;
				for (int k = 0; k < count3; k++)
				{
					BaseAction action = partModule.Actions[k];
					AddAction(list, group, overrideGroup, overrideDefault, include, action);
				}
			}
		}
		return list;
	}

	public static List<bool> CreateGroupList(Part p)
	{
		return CreateGroupList(p, 0);
	}

	public static List<bool> CreateGroupList(Part p, int overrideGroup)
	{
		int num = ActionGroupsLength;
		List<bool> list = new List<bool>(num);
		for (int i = 0; i < num; i++)
		{
			list.Add(p.Actions.Contains((KSPActionGroup)(1 << i), overrideGroup));
		}
		return list;
	}

	public static int GetActionGroupsLength(float facilityLevel, bool isVAB = true)
	{
		if (GameVariables.Instance.UnlockedActionGroupsCustom(facilityLevel, isVAB))
		{
			return Mathf.Max(Enum.GetNames(typeof(KSPActionGroup)).Length - 2, 0);
		}
		if (GameVariables.Instance.UnlockedActionGroupsStock(facilityLevel, isVAB))
		{
			return Mathf.Max(GetActionGroups(64).Length - 1, 0);
		}
		return 0;
	}

	public static string[] GetActionGroups(int maxlevel)
	{
		string[] array = new string[(int)Mathf.Sqrt(maxlevel)];
		string[] names = Enum.GetNames(typeof(KSPActionGroup));
		int[] array2 = (int[])Enum.GetValues(typeof(KSPActionGroup));
		for (int i = 0; i + 1 < array2.Length && array2[i + 1] <= maxlevel; i++)
		{
			array[i] = names[i + 1];
		}
		return array;
	}

	public static List<bool> CreateGroupList(List<Part> parts)
	{
		return CreateGroupList(parts, 0);
	}

	public static List<bool> CreateGroupList(List<Part> parts, int groupOverride)
	{
		int num = ActionGroupsLength;
		List<bool> list = new List<bool>(num);
		bool flag = false;
		for (int i = 0; i < num; i++)
		{
			flag = false;
			KSPActionGroup group = (KSPActionGroup)(1 << i);
			int count = parts.Count;
			for (int j = 0; j < count; j++)
			{
				Part part = parts[j];
				if (!part.Actions.Contains(group, groupOverride))
				{
					int count2 = part.Modules.Count;
					for (int k = 0; k < count2; k++)
					{
						if (part.Modules[k].Actions.Contains(group, groupOverride))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						break;
					}
					continue;
				}
				flag = true;
				break;
			}
			list.Add(flag);
		}
		return list;
	}

	public static bool ContainsNonDefaultActions(Part p)
	{
		int count = p.Actions.Count;
		int num = 0;
		while (true)
		{
			if (num < count)
			{
				if (p.Actions[num].ContainsNonDefaultActions())
				{
					break;
				}
				num++;
				continue;
			}
			int count2 = p.Modules.Count;
			for (int i = 0; i < count2; i++)
			{
				PartModule partModule = p.Modules[i];
				int count3 = partModule.Actions.Count;
				for (int j = 0; j < count3; j++)
				{
					if (partModule.Actions[j].ContainsNonDefaultActions())
					{
						return true;
					}
				}
			}
			return false;
		}
		return true;
	}

	public static void FireAction(List<Part> parts, KSPActionGroup group, int overrideGroup, KSPActionType type)
	{
		KSPActionParam param = new KSPActionParam(group, type);
		List<BaseAction> list = CreateActionList(parts, group, overrideGroup, overrideDefault: false, include: true);
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			BaseAction baseAction = list[i];
			if (baseAction.active && (!baseAction.requireFullControl || !InputLockManager.IsLocked(ControlTypes.TWEAKABLES_FULLONLY)) && (baseAction.activeEditor || !HighLogic.LoadedSceneIsEditor))
			{
				baseAction?.Invoke(param);
			}
		}
	}

	public static int GetGroupIndex(KSPActionGroup group)
	{
		int num = 0;
		while (true)
		{
			if (num < ActionGroupsLength)
			{
				if (1 << num == (int)group)
				{
					break;
				}
				num++;
				continue;
			}
			return 0;
		}
		return num;
	}

	public static List<BaseAction> CreateActionList(List<Part> parts, ModuleRoboticController controller, bool include)
	{
		List<BaseAction> list = new List<BaseAction>();
		for (int i = 0; i < parts.Count; i++)
		{
			if (controller.PartPersistentId != parts[i].persistentId)
			{
				list.AddRange(CreateActionList(parts[i], controller, include));
			}
		}
		return list;
	}

	public static List<BaseAction> CreateActionList(Part part, ModuleRoboticController controller, bool include)
	{
		List<BaseAction> list = new List<BaseAction>();
		int count = part.Actions.Count;
		for (int i = 0; i < count; i++)
		{
			BaseAction action = part.Actions[i];
			AddAction(list, controller, part, include, action);
		}
		int count2 = part.Modules.Count;
		for (int j = 0; j < count2; j++)
		{
			PartModule partModule = part.Modules[j];
			if (partModule.isEnabled)
			{
				int count3 = partModule.Actions.Count;
				for (int k = 0; k < count3; k++)
				{
					BaseAction action = partModule.Actions[k];
					AddAction(list, controller, part, include, action);
				}
			}
		}
		return list;
	}

	public static void AddAction(List<BaseAction> actionList, ModuleRoboticController controller, Part part, bool include, BaseAction action)
	{
		if (action.active && (GameSettings.ADVANCED_TWEAKABLES || !action.advancedTweakable))
		{
			ControlledAction action2 = null;
			bool symmetryPartner = false;
			bool flag = controller.TryGetPartAction(part, action, out action2, out symmetryPartner);
			if (include == flag && !symmetryPartner)
			{
				actionList.Add(action);
			}
		}
	}
}
