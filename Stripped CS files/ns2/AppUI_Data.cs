using System;
using System.Collections.Generic;
using ns11;
using UnityEngine;

namespace ns2;

[Serializable]
public abstract class AppUI_Data : IConfigNode
{
	public Callback onDataChanged;

	public AppUIInputPanel uiPanel;

	public AppUI_Data()
	{
	}

	public void SetupDataChangeCallback(Callback onDataChanged)
	{
		this.onDataChanged = onDataChanged;
	}

	public void CallOnDataChanged()
	{
		UIInputPanelDataChanged();
		if (onDataChanged != null)
		{
			onDataChanged();
		}
	}

	public virtual void UIInputPanelDataChanged()
	{
	}

	public virtual void UIInputPanelUpdate()
	{
	}

	public virtual void OnLoad(ConfigNode node)
	{
	}

	public virtual void OnSave(ConfigNode node)
	{
	}

	public static List<T> CreateAppUIDataList<T>(ConfigNode[] nodes) where T : AppUI_Data
	{
		List<T> list = new List<T>();
		for (int i = 0; i < nodes.Length; i++)
		{
			T val = CreateInstanceOfAppUIData<T>(nodes[i]);
			if (val != null)
			{
				list.Add(val);
			}
		}
		return list;
	}

	public static T CreateInstanceOfAppUIData<T>(ConfigNode node) where T : AppUI_Data
	{
		T val = null;
		string value = "";
		if (node.TryGetValue("type", ref value))
		{
			val = CreateInstanceOfAppUIData<T>(value);
			val?.Load(node);
		}
		else
		{
			Debug.LogError("[AlarmTypeBase]: Cannot create alarm with type=" + value + ". That Type is not in the loaded assemblies.");
		}
		return val;
	}

	public static T CreateInstanceOfAppUIData<T>(string className) where T : AppUI_Data
	{
		List<Type> subclassesOfParentClass = AssemblyLoader.GetSubclassesOfParentClass(typeof(T));
		Type type = null;
		for (int i = 0; i < subclassesOfParentClass.Count; i++)
		{
			if (subclassesOfParentClass[i].Name == className)
			{
				type = subclassesOfParentClass[i];
				break;
			}
		}
		object obj = null;
		if (type != null)
		{
			try
			{
				obj = Activator.CreateInstance(type);
			}
			catch (Exception arg)
			{
				Debug.LogError($"[AppUI_Data]: Cannot create alarm with type={className}. Error: {arg}");
			}
		}
		return obj as T;
	}

	public void Load(ConfigNode node)
	{
		OnLoad(node);
	}

	public void Save(ConfigNode node)
	{
		OnSave(node);
	}
}
