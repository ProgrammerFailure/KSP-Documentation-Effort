using System;
using System.Collections.Generic;
using UnityEngine;

public static class VesselModuleManager
{
	public class VesselModuleWrapper
	{
		public Type type;

		public bool active;

		public int order;

		public VesselModuleWrapper(Type type)
		{
			this.type = type;
			active = true;
			order = 0;
		}
	}

	public static List<VesselModuleWrapper> modules = new List<VesselModuleWrapper>();

	public static List<VesselModuleWrapper> Modules => modules;

	public static void CompileModules()
	{
		modules = new List<VesselModuleWrapper>();
		AssemblyLoader.loadedAssemblies.TypeOperation(delegate(Type t)
		{
			if (t.IsSubclassOf(typeof(VesselModule)) && !(t == typeof(VesselModule)))
			{
				VesselModuleWrapper vesselModuleWrapper = new VesselModuleWrapper(t);
				try
				{
					GameObject gameObject = new GameObject("Temp");
					VesselModule vesselModule = gameObject.AddComponent(t) as VesselModule;
					if (vesselModule != null)
					{
						vesselModuleWrapper.order = vesselModule.GetOrder();
						Debug.Log("VesselModules: Found VesselModule of type " + t.Name + " with order " + vesselModuleWrapper.order);
						UnityEngine.Object.DestroyImmediate(vesselModule);
					}
					UnityEngine.Object.DestroyImmediate(gameObject);
					modules.Add(vesselModuleWrapper);
				}
				catch (Exception ex)
				{
					Debug.LogError("VesselModules: Error getting order of VesselModule of type " + t.Name + " so it was not added. Exception: " + ex);
				}
			}
		});
		Debug.Log("VesselModules: Found " + modules.Count + " VesselModule types");
	}

	public static List<VesselModuleWrapper> GetModules(bool activeOnly, bool order)
	{
		if (!activeOnly && !order)
		{
			return modules;
		}
		List<VesselModuleWrapper> list = new List<VesselModuleWrapper>();
		int count = modules.Count;
		for (int i = 0; i < count; i++)
		{
			if (!activeOnly || modules[i].active)
			{
				list.Add(modules[i]);
			}
		}
		if (order)
		{
			list.Sort((VesselModuleWrapper a, VesselModuleWrapper b) => a.order.CompareTo(b.order));
		}
		return list;
	}

	public static VesselModuleWrapper GetWrapper(Type moduleType)
	{
		int count = modules.Count;
		int num = 0;
		while (true)
		{
			if (num < count)
			{
				if (modules[num].type == moduleType)
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

	public static VesselModuleWrapper GetWrapper(string moduleTypeName)
	{
		int count = modules.Count;
		int num = 0;
		while (true)
		{
			if (num < count)
			{
				if (modules[num].type.Name == moduleTypeName)
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

	public static void SetWrapperActive(Type moduleType, bool isActive)
	{
		int count = modules.Count;
		for (int i = 0; i < count; i++)
		{
			if (modules[i].type == moduleType)
			{
				modules[i].active = isActive;
			}
		}
	}

	public static void SetWrapperActive(string moduleTypeName, bool isActive)
	{
		int count = modules.Count;
		for (int i = 0; i < count; i++)
		{
			if (modules[i].type.Name == moduleTypeName)
			{
				modules[i].active = isActive;
			}
		}
	}

	public static void AddModulesToVessel(Vessel vessel, List<VesselModule> modules)
	{
		List<VesselModuleWrapper> list = GetModules(activeOnly: true, order: true);
		if (modules == null)
		{
			modules = new List<VesselModule>();
		}
		else
		{
			modules.Clear();
		}
		int count = list.Count;
		GameObject gameObject = vessel.gameObject;
		for (int i = 0; i < count; i++)
		{
			if (gameObject.GetComponent(list[i].type) == null || list[i].type == typeof(CometVessel))
			{
				VesselModule vesselModule = gameObject.AddComponent(list[i].type) as VesselModule;
				vesselModule.Vessel = vessel;
				vesselModule.enabled = vesselModule.ShouldBeActive();
				modules.Add(vesselModule);
			}
		}
	}

	public static void RemoveModulesFromVessel(Vessel vessel)
	{
		VesselModule[] components = vessel.gameObject.GetComponents<VesselModule>();
		int i = 0;
		for (int num = components.Length; i < num; i++)
		{
			UnityEngine.Object.Destroy(components[i]);
		}
	}

	public static bool RemoveModuleOfType(Type vesselModuleType)
	{
		int num = 0;
		int count = modules.Count;
		while (true)
		{
			if (num < count)
			{
				if (modules[num].type == vesselModuleType)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		modules.RemoveAt(num);
		return true;
	}
}
