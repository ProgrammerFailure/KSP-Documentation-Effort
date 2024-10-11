using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

public class BaseEventList : List<BaseEvent>
{
	public class ReflectedData
	{
		public List<KSPEvent> eventAttributes;

		public List<MethodInfo> eventDelegates;

		public List<KSPEvent> eventDataAttributes;

		public List<MethodInfo> eventDataDelegates;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ReflectedData(Type type)
		{
			throw null;
		}
	}

	protected static Dictionary<Type, ReflectedData> reflectedAttributeCache;

	public Part part;

	public PartModule module;

	public object obj;

	private BaseEvent defaultEvent;

	public BaseEvent this[string actionName]
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public new BaseEvent this[int actionID]
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public BaseEventList(object obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public BaseEventList(Part part, PartModule module)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static BaseEventList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static ReflectedData GetReflectedAttributes(Type type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateDelegates(object part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private BaseEvent Add(string eventName, BaseEventDelegate onEvent, KSPEvent actionAttr)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private BaseEvent Add(string eventName, BaseEventDetailsDelegate onEvent, KSPEvent actionAttr)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddBaseEventList(BaseEventList addList)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveBaseEventList(BaseEventList removeList)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public BaseEvent GetByIndex(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Contains(string actionName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Contains(int actionID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetDefault(string actionName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetDefault(int actionID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearDefault()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Send(string actionName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Send(int actionID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Send(string actionName, BaseEventDetails actionData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Send(int actionID, BaseEventDetails actionData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SendDefault()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SendDefault(BaseEventDetails actionData)
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
	public void LoadStagingIcon()
	{
		throw null;
	}
}
