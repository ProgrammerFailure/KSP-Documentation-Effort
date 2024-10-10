using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectList
{
	public class EffectType
	{
		public string name;

		public Type type;

		public EffectType(string name, Type type)
		{
			this.name = name;
			this.type = type;
		}
	}

	public static List<EffectType> effectTypes;

	public Part hostPart;

	public Dictionary<string, List<EffectBehaviour>> effectList;

	public static Dictionary<string, List<EffectBehaviour>>.Enumerator fxEnumerator;

	public EffectList(Part hostPart)
	{
		CreateEffectTypes();
		this.hostPart = hostPart;
		effectList = new Dictionary<string, List<EffectBehaviour>>();
		ConstructEffectList();
	}

	public static EffectType GetEffectType(string name)
	{
		int num = 0;
		int count = effectTypes.Count;
		while (true)
		{
			if (num < count)
			{
				if (effectTypes[num].name == name)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return effectTypes[num];
	}

	public static EffectType GetEffectType(Type type)
	{
		int num = 0;
		int count = effectTypes.Count;
		while (true)
		{
			if (num < count)
			{
				if (effectTypes[num].type == type)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return effectTypes[num];
	}

	public static void CreateEffectTypes()
	{
		if (effectTypes != null)
		{
			return;
		}
		effectTypes = new List<EffectType>();
		AssemblyLoader.loadedAssemblies.TypeOperation(delegate(Type t)
		{
			if (t.IsSubclassOf(typeof(EffectBehaviour)) && !(t == typeof(EffectBehaviour)))
			{
				EffectDefinition[] array = (EffectDefinition[])t.GetCustomAttributes(typeof(EffectDefinition), inherit: true);
				if (array.Length != 0)
				{
					EffectDefinition effectDefinition = array[0];
					if (GetEffectType(effectDefinition.nodeName) != null)
					{
						Debug.LogError("Effect definitions already contains an effect with node name of '" + effectDefinition.nodeName + "'");
					}
					else
					{
						effectTypes.Add(new EffectType(effectDefinition.nodeName, t));
					}
				}
			}
		});
		Debug.Log("EffectList: Created " + effectTypes.Count + " effect types");
	}

	public void ConstructEffectList()
	{
		EffectBehaviour[] components = hostPart.GetComponents<EffectBehaviour>();
		int i = 0;
		for (int num = components.Length; i < num; i++)
		{
			EffectBehaviour effectBehaviour = components[i];
			if (!effectList.ContainsKey(effectBehaviour.effectName))
			{
				effectList.Add(effectBehaviour.effectName, new List<EffectBehaviour>());
			}
			effectBehaviour.hostPart = hostPart;
			effectList[effectBehaviour.effectName].Add(effectBehaviour);
		}
	}

	public EffectBehaviour GetEffect(string effectName, string instanceName, Type effectType)
	{
		List<EffectBehaviour> list = effectList[effectName];
		int num = 0;
		int count = list.Count;
		while (true)
		{
			if (num < count)
			{
				if ((instanceName == null || !(list[num].instanceName != instanceName)) && list[num].GetType() == effectType)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return list[num];
	}

	public List<string> EffectsStartingWith(string effectName)
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, List<EffectBehaviour>> effect in effectList)
		{
			if (effect.Key.Contains(effectName))
			{
				list.Add(effect.Key);
			}
		}
		return list;
	}

	public void Initialize()
	{
		fxEnumerator = effectList.GetEnumerator();
		while (fxEnumerator.MoveNext())
		{
			List<EffectBehaviour> value = fxEnumerator.Current.Value;
			int i = 0;
			for (int count = value.Count; i < count; i++)
			{
				value[i].OnInitialize();
			}
		}
	}

	public void OnLoad(ConfigNode node)
	{
		int i = 0;
		for (int count = node.nodes.Count; i < count; i++)
		{
			LoadEffect(node.nodes[i]);
		}
	}

	public void LoadEffect(ConfigNode node)
	{
		string name = node.name;
		if (!effectList.ContainsKey(name))
		{
			effectList.Add(name, new List<EffectBehaviour>());
		}
		int i = 0;
		for (int count = node.nodes.Count; i < count; i++)
		{
			ConfigNode configNode = node.nodes[i];
			EffectType effectType = GetEffectType(configNode.name);
			if (effectType == null)
			{
				Debug.LogError("EffectList: Cannot find an effect type with node name of '" + configNode.name + "'");
				continue;
			}
			string value = configNode.GetValue("name");
			EffectBehaviour effectBehaviour = GetEffect(name, value, effectType.type);
			if (effectBehaviour == null)
			{
				effectBehaviour = (EffectBehaviour)hostPart.gameObject.AddComponent(effectType.type);
				effectBehaviour.effectName = name;
				effectBehaviour.instanceName = value;
				effectList[name].Add(effectBehaviour);
			}
			effectBehaviour.hostPart = hostPart;
			effectBehaviour.OnLoad(configNode);
		}
	}

	public void OnSave(ConfigNode node)
	{
		fxEnumerator = effectList.GetEnumerator();
		while (fxEnumerator.MoveNext())
		{
			ConfigNode configNode = node.AddNode(fxEnumerator.Current.Key);
			List<EffectBehaviour> value = fxEnumerator.Current.Value;
			int i = 0;
			for (int count = value.Count; i < count; i++)
			{
				EffectBehaviour effectBehaviour = value[i];
				EffectType effectType = GetEffectType(effectBehaviour.GetType());
				ConfigNode configNode2 = configNode.AddNode(effectType.name);
				if (!string.IsNullOrEmpty(effectBehaviour.instanceName))
				{
					configNode2.AddValue("name", effectBehaviour.instanceName);
				}
				value[i].OnSave(configNode2);
			}
		}
	}

	public void PlayRandomEffect()
	{
		string eventName = new List<string>(effectList.Keys)[UnityEngine.Random.Range(0, effectList.Keys.Count - 1)];
		Event(eventName, -1);
	}

	public void Event(string eventName, int transformIdx)
	{
		if (effectList.TryGetValue(eventName, out var value))
		{
			int i = 0;
			for (int count = value.Count; i < count; i++)
			{
				value[i].OnEvent(transformIdx);
			}
		}
	}

	public void Event(string eventName, float power, int transformIdx)
	{
		if (effectList.TryGetValue(eventName, out var value))
		{
			int i = 0;
			for (int count = value.Count; i < count; i++)
			{
				value[i].OnEvent(power, transformIdx);
			}
		}
	}
}
