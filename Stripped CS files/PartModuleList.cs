using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PartModuleList : IEnumerable
{
	[HideInInspector]
	[SerializeField]
	public Part part;

	[SerializeField]
	[HideInInspector]
	public List<PartModule> modules = new List<PartModule>();

	public int Count => modules.Count;

	public PartModule this[string className]
	{
		get
		{
			int hashCode = className.GetHashCode();
			int count = modules.Count;
			int num = 0;
			PartModule partModule;
			while (true)
			{
				if (num < count)
				{
					partModule = modules[num];
					if (partModule.ClassID == hashCode)
					{
						break;
					}
					num++;
					continue;
				}
				Debug.LogError("Cannot find module '" + className + "' (" + hashCode + ")");
				return null;
			}
			return partModule;
		}
	}

	public PartModule this[int index]
	{
		get
		{
			return modules[index];
		}
		set
		{
			modules[index] = value;
			part.ClearModuleReferenceCache();
		}
	}

	public PartModule this[uint persistentId]
	{
		get
		{
			int count = modules.Count;
			int num = 0;
			PartModule partModule;
			while (true)
			{
				if (num < count)
				{
					partModule = modules[num];
					if (partModule.PersistentId == persistentId)
					{
						break;
					}
					num++;
					continue;
				}
				return null;
			}
			return partModule;
		}
	}

	public PartModuleList(Part part)
	{
		this.part = part;
		PartModule[] components = part.GetComponents<PartModule>();
		modules = new List<PartModule>(components.Length);
		int i = 0;
		for (int num = components.Length; i < num; i++)
		{
			PartModule partModule = components[i];
			partModule.part = part;
			modules.Add(partModule);
		}
	}

	public List<T> GetModules<T>() where T : class
	{
		List<T> list = new List<T>();
		int count = modules.Count;
		for (int i = 0; i < count; i++)
		{
			if (modules[i] is T)
			{
				list.Add(modules[i] as T);
			}
		}
		return list;
	}

	public IScalarModule GetScalarModule(string name)
	{
		int num = 0;
		int count = modules.Count;
		IScalarModule scalarModule;
		while (true)
		{
			if (num < count)
			{
				scalarModule = modules[num] as IScalarModule;
				if (scalarModule != null && scalarModule.ScalarModuleID == name)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return scalarModule;
	}

	public List<IScalarModule> GetScalarModules(string name)
	{
		List<IScalarModule> list = new List<IScalarModule>();
		for (int i = 0; i < Count; i++)
		{
			if (this[i] is IScalarModule scalarModule && scalarModule.ScalarModuleID == name)
			{
				list.Add(scalarModule);
			}
		}
		return list;
	}

	public IEngineStatus FindEngineNearby(string engineName, int engineIndex, bool preferMultiMode)
	{
		IEngineStatus engineStatus = FindEngineInterface(engineName, engineIndex, preferMultiMode);
		if (engineStatus != null)
		{
			return engineStatus;
		}
		engineStatus = part.parent.Modules.FindEngineInterface(engineName, engineIndex, preferMultiMode);
		if (engineStatus != null)
		{
			return engineStatus;
		}
		int num = 0;
		while (true)
		{
			if (num < part.children.Count)
			{
				engineStatus = part.children[num].Modules.FindEngineInterface(engineName, engineIndex, preferMultiMode);
				if (engineStatus != null)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return engineStatus;
	}

	public IEngineStatus FindEngineInterface(string engineName, int engineIndex, bool preferMultiMode)
	{
		int count = modules.Count;
		int num = 0;
		if (preferMultiMode)
		{
			for (int i = 0; i < count; i++)
			{
				PartModule partModule = modules[i];
				if (partModule is IEngineStatus && partModule is MultiModeEngine)
				{
					if (num == engineIndex)
					{
						return (IEngineStatus)partModule;
					}
					num++;
				}
			}
		}
		if (!string.IsNullOrEmpty(engineName))
		{
			for (int j = 0; j < count; j++)
			{
				PartModule partModule = modules[j];
				if (partModule is IEngineStatus)
				{
					IEngineStatus engineStatus = partModule as IEngineStatus;
					if (engineStatus.engineName == engineName)
					{
						return engineStatus;
					}
				}
			}
		}
		else
		{
			num = 0;
			for (int k = 0; k < count; k++)
			{
				PartModule partModule = modules[k];
				if (partModule is IEngineStatus && !(partModule is MultiModeEngine))
				{
					if (num == engineIndex)
					{
						return (IEngineStatus)partModule;
					}
					num++;
				}
			}
		}
		return null;
	}

	public List<PartModule>.Enumerator GetEnumerator()
	{
		return modules.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return modules.GetEnumerator();
	}

	public void Add(PartModule module)
	{
		if (module.gameObject == part.gameObject && !modules.Contains(module))
		{
			module.part = part;
			modules.Add(module);
			part.ClearModuleReferenceCache();
		}
	}

	public void Remove(PartModule module)
	{
		modules.Remove(module);
		part.ClearModuleReferenceCache();
	}

	public int IndexOf(PartModule module)
	{
		return modules.IndexOf(module);
	}

	public PartModule GetModule(int index)
	{
		return modules[index];
	}

	public PartModule GetModule(string moduleName)
	{
		int num = 0;
		while (true)
		{
			if (num < modules.Count)
			{
				if (modules[num].moduleName.Equals(moduleName))
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return modules[num];
	}

	public T GetModule<T>(int index = 0) where T : PartModule
	{
		int count = modules.Count;
		int num = 0;
		int num2 = 0;
		PartModule partModule;
		while (true)
		{
			if (num2 < count)
			{
				partModule = modules[num2];
				if (partModule is T)
				{
					if (num == index)
					{
						break;
					}
					num++;
				}
				num2++;
				continue;
			}
			return null;
		}
		return partModule as T;
	}

	public bool Contains(string className)
	{
		return Contains(className.GetHashCode());
	}

	public bool Contains(int classID)
	{
		int count = modules.Count;
		int num = 0;
		while (true)
		{
			if (num < count)
			{
				if (modules[num].ClassID == classID)
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

	public bool Contains(uint persistentId)
	{
		int count = modules.Count;
		int num = 0;
		while (true)
		{
			if (num < count)
			{
				if (modules[num].PersistentId == persistentId)
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

	public bool Contains<T>() where T : PartModule
	{
		int count = modules.Count;
		int num = 0;
		while (true)
		{
			if (num < count)
			{
				if (modules[num] is T)
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

	public void Clear()
	{
		int count = modules.Count;
		for (int i = 0; i < count; i++)
		{
			if (modules[i] != null)
			{
				UnityEngine.Object.DestroyImmediate(modules[i]);
			}
		}
		modules.Clear();
		part.ClearModuleReferenceCache();
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
		return Send(actionName.GetHashCode(), actionData);
	}

	public bool Send(int actionID, BaseEventDetails actionData)
	{
		bool result = false;
		int count = modules.Count;
		for (int i = 0; i < count; i++)
		{
			if (modules[i].Events.Send(actionID, actionData))
			{
				result = true;
			}
		}
		return result;
	}

	public bool SendDefault()
	{
		return SendDefault(null);
	}

	public bool SendDefault(BaseEventDetails actionData)
	{
		bool result = false;
		int count = modules.Count;
		for (int i = 0; i < count; i++)
		{
			if (modules[i].Events.SendDefault(actionData))
			{
				result = true;
			}
		}
		return result;
	}
}
