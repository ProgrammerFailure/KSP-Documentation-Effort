using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[Serializable]
public class BaseActionList : List<BaseAction>
{
	public class ReflectedData
	{
		public List<KSPAction> actionAttributes;

		public List<string> actions;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ReflectedData(Type type)
		{
			throw null;
		}
	}

	public const KSPActionGroup ControlAction = KSPActionGroup.Stage | KSPActionGroup.RCS | KSPActionGroup.SAS;

	protected static Dictionary<Type, ReflectedData> reflectedAttributeCache;

	public Part part;

	public PartModule module;

	public BaseAction this[string name]
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public BaseActionList(Part part, PartModule module)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static BaseActionList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static ReflectedData GetReflectedAttributes(Type type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ReflectActions(object obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private BaseAction Add(string eventName, BaseActionDelegate onEvent, KSPAction actionAttr)
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
	public bool Contains(KSPActionGroup group)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Contains(KSPActionGroup group, int groupOverride)
	{
		throw null;
	}
}
