using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class BaseEventList : List<BaseEvent>
{
	public class ReflectedData
	{
		public List<KSPEvent> eventAttributes = new List<KSPEvent>();

		public List<MethodInfo> eventDelegates = new List<MethodInfo>();

		public List<KSPEvent> eventDataAttributes = new List<KSPEvent>();

		public List<MethodInfo> eventDataDelegates = new List<MethodInfo>();

		public ReflectedData(Type type)
		{
			MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
			int i = 0;
			for (int num = methods.Length; i < num; i++)
			{
				MethodInfo methodInfo = methods[i];
				bool flag = false;
				KSPEvent[] array = (KSPEvent[])methodInfo.GetCustomAttributes(typeof(KSPEvent), inherit: true);
				if (methodInfo.IsVirtual && methodInfo.DeclaringType != type)
				{
					flag = true;
				}
				int j = 0;
				for (int num2 = array.Length; j < num2; j++)
				{
					KSPEvent item = array[j];
					if (!flag || !eventDelegates.Contains(methodInfo))
					{
						ParameterInfo[] parameters = methodInfo.GetParameters();
						if (parameters.Length == 0)
						{
							eventAttributes.Add(item);
							eventDelegates.Add(methodInfo);
						}
						else if (parameters.Length == 1 && parameters[0].ParameterType == typeof(BaseEventDetails))
						{
							eventDataAttributes.Add(item);
							eventDataDelegates.Add(methodInfo);
						}
					}
				}
			}
		}
	}

	public static Dictionary<Type, ReflectedData> reflectedAttributeCache = new Dictionary<Type, ReflectedData>();

	public Part part;

	public PartModule module;

	public object obj;

	public BaseEvent defaultEvent;

	public BaseEvent this[string actionName] => this[actionName.GetHashCode()];

	public new BaseEvent this[int actionID]
	{
		get
		{
			int count = base.Count;
			int num = 0;
			while (true)
			{
				if (num < count)
				{
					if (base[num].id == actionID)
					{
						break;
					}
					num++;
					continue;
				}
				return null;
			}
			return base[num];
		}
	}

	public BaseEventList(object obj)
	{
		part = null;
		module = null;
		this.obj = obj;
		CreateDelegates(obj);
	}

	public BaseEventList(Part part, PartModule module)
	{
		this.part = part;
		this.module = module;
		obj = ((module != null) ? ((MonoBehaviour)module) : ((MonoBehaviour)part));
		CreateDelegates(obj);
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

	public void CreateDelegates(object part)
	{
		ReflectedData reflectedAttributes = GetReflectedAttributes(obj.GetType());
		base.Capacity = reflectedAttributes.eventDelegates.Count + reflectedAttributes.eventDataDelegates.Count;
		int i = 0;
		for (int count = reflectedAttributes.eventDelegates.Count; i < count; i++)
		{
			BaseEventDelegate onEvent = (BaseEventDelegate)Delegate.CreateDelegate(typeof(BaseEventDelegate), part, reflectedAttributes.eventDelegates[i].Name, ignoreCase: false);
			Add(reflectedAttributes.eventDelegates[i].Name, onEvent, reflectedAttributes.eventAttributes[i]);
		}
		int j = 0;
		for (int count2 = reflectedAttributes.eventDataDelegates.Count; j < count2; j++)
		{
			BaseEventDetailsDelegate onEvent2 = (BaseEventDetailsDelegate)Delegate.CreateDelegate(typeof(BaseEventDetailsDelegate), part, reflectedAttributes.eventDataDelegates[j].Name, ignoreCase: false);
			Add(reflectedAttributes.eventDataDelegates[j].Name, onEvent2, reflectedAttributes.eventDataAttributes[j]);
		}
	}

	public BaseEvent Add(string eventName, BaseEventDelegate onEvent, KSPEvent actionAttr)
	{
		BaseEvent baseEvent = null;
		if (!Contains(eventName))
		{
			baseEvent = new BaseEvent(this, eventName, onEvent, actionAttr);
			Add(baseEvent);
		}
		else
		{
			PDebug.Error("Action '" + eventName + "' already defined.");
		}
		return baseEvent;
	}

	public BaseEvent Add(string eventName, BaseEventDetailsDelegate onEvent, KSPEvent actionAttr)
	{
		BaseEvent baseEvent = null;
		if (!Contains(eventName))
		{
			baseEvent = new BaseEvent(this, eventName, onEvent, actionAttr);
			Add(baseEvent);
		}
		else
		{
			PDebug.Error("Action '" + eventName + "' already defined.");
		}
		return baseEvent;
	}

	public void AddBaseEventList(BaseEventList addList)
	{
		for (int i = 0; i < addList.Count; i++)
		{
			Add(addList.GetByIndex(i));
		}
	}

	public void RemoveBaseEventList(BaseEventList removeList)
	{
		for (int i = 0; i < removeList.Count; i++)
		{
			Remove(removeList.GetByIndex(i));
		}
	}

	public BaseEvent GetByIndex(int index)
	{
		return base[index];
	}

	public bool Contains(string actionName)
	{
		return Contains(actionName.GetHashCode());
	}

	public bool Contains(int actionID)
	{
		int count = base.Count;
		int num = 0;
		while (true)
		{
			if (num < count)
			{
				if (base[num].id == actionID)
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

	public void SetDefault(string actionName)
	{
		BaseEvent baseEvent = this[actionName];
		if (baseEvent != null)
		{
			defaultEvent = baseEvent;
		}
	}

	public void SetDefault(int actionID)
	{
		BaseEvent baseEvent = this[actionID];
		if (baseEvent != null)
		{
			defaultEvent = baseEvent;
		}
	}

	public void ClearDefault()
	{
		defaultEvent = null;
	}

	public bool Send(string actionName)
	{
		return Send(actionName.GetHashCode(), null);
	}

	public bool Send(int actionID)
	{
		return Send(actionID, null);
	}

	public bool Send(string actionName, BaseEventDetails actionData)
	{
		if (module != null && !module.isEnabled)
		{
			return false;
		}
		int count = base.Count;
		int num = 0;
		BaseEvent baseEvent;
		while (true)
		{
			if (num < count)
			{
				baseEvent = base[num];
				if (baseEvent.active && baseEvent.name == actionName)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		baseEvent.Invoke(actionData);
		return true;
	}

	public bool Send(int actionID, BaseEventDetails actionData)
	{
		if (module != null && !module.isEnabled)
		{
			return false;
		}
		int count = base.Count;
		int num = 0;
		BaseEvent baseEvent;
		while (true)
		{
			if (num < count)
			{
				baseEvent = base[num];
				if (baseEvent.active && baseEvent.id == actionID)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		baseEvent.Invoke(actionData);
		return true;
	}

	public bool SendDefault()
	{
		return SendDefault(null);
	}

	public bool SendDefault(BaseEventDetails actionData)
	{
		if (module != null && !module.isEnabled)
		{
			return false;
		}
		if (defaultEvent != null && defaultEvent.active)
		{
			defaultEvent.Invoke(actionData);
			return true;
		}
		return false;
	}

	public void OnLoad(ConfigNode node)
	{
		int count = base.Count;
		for (int i = 0; i < count; i++)
		{
			BaseEvent baseEvent = base[i];
			ConfigNode node2 = node.GetNode(baseEvent.name);
			if (node2 != null)
			{
				baseEvent.OnLoad(node2);
			}
		}
	}

	public void OnSave(ConfigNode node)
	{
		int count = base.Count;
		for (int i = 0; i < count; i++)
		{
			BaseEvent baseEvent = base[i];
			if (baseEvent.isPersistent)
			{
				baseEvent.OnSave(node.AddNode(baseEvent.name));
			}
		}
	}

	public void LoadStagingIcon()
	{
	}
}
