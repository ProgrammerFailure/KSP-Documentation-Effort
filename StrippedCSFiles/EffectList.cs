using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class EffectList
{
	private class EffectType
	{
		public string name;

		public Type type;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public EffectType(string name, Type type)
		{
			throw null;
		}
	}

	private static List<EffectType> effectTypes;

	private Part hostPart;

	private Dictionary<string, List<EffectBehaviour>> effectList;

	private static Dictionary<string, List<EffectBehaviour>>.Enumerator fxEnumerator;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public EffectList(Part hostPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static EffectList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static EffectType GetEffectType(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static EffectType GetEffectType(Type type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void CreateEffectTypes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ConstructEffectList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private EffectBehaviour GetEffect(string effectName, string instanceName, Type effectType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<string> EffectsStartingWith(string effectName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Initialize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LoadEffect(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PlayRandomEffect()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Event(string eventName, int transformIdx)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Event(string eventName, float power, int transformIdx)
	{
		throw null;
	}
}
