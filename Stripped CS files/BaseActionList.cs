using System;
using System.Collections.Generic;
using System.Reflection;

[Serializable]
public class BaseActionList : List<BaseAction>
{
	public class ReflectedData
	{
		public List<KSPAction> actionAttributes = new List<KSPAction>();

		public List<string> actions = new List<string>();

		public ReflectedData(Type type)
		{
			MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
			int i = 0;
			for (int num = methods.Length; i < num; i++)
			{
				MethodInfo methodInfo = methods[i];
				bool flag = false;
				KSPAction[] array = (KSPAction[])methodInfo.GetCustomAttributes(typeof(KSPAction), inherit: true);
				if (methodInfo.IsVirtual && methodInfo.DeclaringType != type)
				{
					flag = true;
				}
				int j = 0;
				for (int num2 = array.Length; j < num2; j++)
				{
					KSPAction item = array[j];
					if ((!flag || !actions.Contains(methodInfo.Name)) && methodInfo.GetParameters().Length == 1)
					{
						actionAttributes.Add(item);
						actions.Add(methodInfo.Name);
					}
				}
			}
		}
	}

	public const KSPActionGroup ControlAction = KSPActionGroup.Stage | KSPActionGroup.flag_5 | KSPActionGroup.flag_6;

	public static Dictionary<Type, ReflectedData> reflectedAttributeCache = new Dictionary<Type, ReflectedData>();

	public Part part;

	public PartModule module;

	public BaseAction this[string name]
	{
		get
		{
			int count = base.Count;
			int num = 0;
			BaseAction baseAction;
			while (true)
			{
				if (num < count)
				{
					baseAction = base[num];
					if (baseAction.name == name)
					{
						break;
					}
					num++;
					continue;
				}
				return null;
			}
			return baseAction;
		}
	}

	public BaseActionList(Part part, PartModule module)
	{
		this.part = part;
		this.module = module;
		if (module != null)
		{
			ReflectActions(module);
		}
		else
		{
			ReflectActions(part);
		}
	}

	public static ReflectedData GetReflectedAttributes(Type type)
	{
		if (reflectedAttributeCache.TryGetValue(type, out var value))
		{
			return value;
		}
		value = new ReflectedData(type);
		reflectedAttributeCache[type] = value;
		return value;
	}

	public void ReflectActions(object obj)
	{
		ReflectedData reflectedAttributes = GetReflectedAttributes(obj.GetType());
		int i = 0;
		for (int count = reflectedAttributes.actions.Count; i < count; i++)
		{
			BaseActionDelegate onEvent = (BaseActionDelegate)Delegate.CreateDelegate(typeof(BaseActionDelegate), obj, reflectedAttributes.actions[i], ignoreCase: false);
			Add(reflectedAttributes.actions[i], onEvent, reflectedAttributes.actionAttributes[i]);
		}
	}

	public BaseAction Add(string eventName, BaseActionDelegate onEvent, KSPAction actionAttr)
	{
		BaseAction baseAction = null;
		baseAction = new BaseAction(this, eventName, onEvent, actionAttr);
		Add(baseAction);
		return baseAction;
	}

	public void OnLoad(ConfigNode node)
	{
		int count = base.Count;
		for (int i = 0; i < count; i++)
		{
			BaseAction baseAction = base[i];
			ConfigNode node2 = node.GetNode(baseAction.name);
			if (node2 != null)
			{
				baseAction.OnLoad(node2);
			}
		}
	}

	public void OnSave(ConfigNode node)
	{
		int count = base.Count;
		for (int i = 0; i < count; i++)
		{
			BaseAction baseAction = base[i];
			baseAction.OnSave(node.AddNode(baseAction.name));
		}
	}

	public bool Contains(KSPActionGroup group)
	{
		return Contains(group, 0);
	}

	public bool Contains(KSPActionGroup group, int groupOverride)
	{
		int count = base.Count;
		int num = 0;
		while (true)
		{
			if (num < count)
			{
				if ((base[num].GetActionGroup(groupOverride) & group) != 0)
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
}
