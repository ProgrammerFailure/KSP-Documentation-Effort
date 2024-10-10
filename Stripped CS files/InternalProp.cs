using System;
using System.Collections.Generic;
using UnityEngine;

public class InternalProp : MonoBehaviour
{
	public InternalModel internalModel;

	public string propName;

	public int propID;

	public bool hasModel;

	public List<InternalModule> internalModules = new List<InternalModule>();

	public Part part => internalModel.part;

	public Vessel vessel => part.vessel;

	public InternalModule AddModule(ConfigNode node)
	{
		if (!node.HasValue("name"))
		{
			Debug.LogError("Cannot add a Module because ConfigNode contains no module name");
			return null;
		}
		InternalModule internalModule = AddModule(node.GetValue("name"));
		if (internalModule == null)
		{
			return null;
		}
		internalModule.Load(node);
		return internalModule;
	}

	public InternalModule AddModule(string moduleName)
	{
		Type classByName = AssemblyLoader.GetClassByName(typeof(InternalModule), moduleName);
		if (classByName == null)
		{
			Debug.LogError("Cannot find an InternalModule of typename '" + moduleName + "'");
			return null;
		}
		InternalModule internalModule = (InternalModule)base.gameObject.AddComponent(classByName);
		if (internalModule == null)
		{
			Debug.LogError("Cannot create a Module of typename '" + moduleName + "'");
			return null;
		}
		internalModule.internalProp = this;
		internalModule.moduleID = internalModules.Count;
		internalModules.Add(internalModule);
		return internalModule;
	}

	public void Load(ConfigNode node)
	{
		Vector3 localPosition = Vector3.zero;
		Quaternion localRotation = Quaternion.identity;
		Vector3 localScale = Vector3.one;
		if (node.HasValue("position"))
		{
			localPosition = KSPUtil.ParseVector3(node.GetValue("position"));
		}
		if (node.HasValue("rotation"))
		{
			localRotation = KSPUtil.ParseQuaternion(node.GetValue("rotation"));
		}
		if (node.HasValue("scale"))
		{
			localScale = KSPUtil.ParseVector3(node.GetValue("scale"));
		}
		base.transform.localPosition = localPosition;
		base.transform.localRotation = localRotation;
		base.transform.localScale = localScale;
		int i = 0;
		for (int count = node.nodes.Count; i < count; i++)
		{
			ConfigNode configNode = node.nodes[i];
			if (configNode.name == "MODULE")
			{
				AddModule(configNode);
			}
		}
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("position", base.transform.localPosition);
		node.AddValue("rotation", base.transform.localRotation);
		node.AddValue("scale", base.transform.localScale);
		int i = 0;
		for (int count = internalModules.Count; i < count; i++)
		{
			internalModules[i].Save(node.AddNode("MODULE"));
		}
	}

	public void OnAwake()
	{
		int i = 0;
		for (int count = internalModules.Count; i < count; i++)
		{
			internalModules[i].OnAwake();
		}
	}

	public void OnUpdate()
	{
		int i = 0;
		for (int count = internalModules.Count; i < count; i++)
		{
			internalModules[i].OnUpdate();
		}
	}

	public void OnFixedUpdate()
	{
		int i = 0;
		for (int count = internalModules.Count; i < count; i++)
		{
			internalModules[i].OnFixedUpdate();
		}
	}

	public Transform FindModelTransform(string childName)
	{
		if (hasModel)
		{
			return FindHeirarchyTransform(base.transform, childName);
		}
		return FindHeirarchyTransform(internalModel.transform, childName);
	}

	public static Transform FindHeirarchyTransform(Transform parent, string childName)
	{
		if (parent.gameObject.name == childName)
		{
			return parent;
		}
		Transform transform = null;
		int num = 0;
		while (true)
		{
			if (num < parent.childCount)
			{
				transform = FindHeirarchyTransform(parent.GetChild(num), childName);
				if (transform != null)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return transform;
	}

	public Transform[] FindModelTransforms(string childName)
	{
		List<Transform> list = new List<Transform>();
		if (hasModel)
		{
			FindHeirarchyTransforms(base.transform, childName, list);
		}
		else
		{
			FindHeirarchyTransforms(internalModel.transform, childName, list);
		}
		return list.ToArray();
	}

	public static void FindHeirarchyTransforms(Transform parent, string childName, List<Transform> tList)
	{
		if (parent.gameObject.name == childName)
		{
			tList.Add(parent);
		}
		for (int i = 0; i < parent.childCount; i++)
		{
			FindHeirarchyTransforms(parent.GetChild(i), childName, tList);
		}
	}

	public T FindModelComponent<T>() where T : Component
	{
		return FindModelComponent<T>(base.transform, "");
	}

	public T FindModelComponent<T>(string childName) where T : Component
	{
		if (hasModel)
		{
			return FindModelComponent<T>(base.transform, childName);
		}
		return FindModelComponent<T>(internalModel.transform, childName);
	}

	public static T FindModelComponent<T>(Transform parent, string childName) where T : Component
	{
		if (parent == null)
		{
			return null;
		}
		if (childName == string.Empty || parent.gameObject.name == childName)
		{
			T component = parent.gameObject.GetComponent<T>();
			if (component != null)
			{
				return component;
			}
		}
		int num = 0;
		T val;
		while (true)
		{
			if (num < parent.childCount)
			{
				val = FindModelComponent<T>(parent.GetChild(num), childName);
				if (val != null)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return val;
	}

	public T[] FindModelComponents<T>() where T : Component
	{
		List<T> list = new List<T>();
		if (hasModel)
		{
			FindModelComponents(base.transform, "", list);
		}
		else
		{
			FindModelComponents(internalModel.transform, "", list);
		}
		return list.ToArray();
	}

	public T[] FindModelComponents<T>(string childName) where T : Component
	{
		List<T> list = new List<T>();
		if (hasModel)
		{
			FindModelComponents(base.transform, "", list);
		}
		else
		{
			FindModelComponents(internalModel.transform, "", list);
		}
		return list.ToArray();
	}

	public static void FindModelComponents<T>(Transform parent, string childName, List<T> tList) where T : Component
	{
		if (parent == null)
		{
			return;
		}
		if (childName == string.Empty || parent.gameObject.name == childName)
		{
			T component = parent.gameObject.GetComponent<T>();
			if (component != null)
			{
				tList.Add(component);
			}
		}
		for (int i = 0; i < parent.childCount; i++)
		{
			FindModelComponents(parent.GetChild(i), childName, tList);
		}
	}

	public Animation[] FindModelAnimators(string clipName)
	{
		List<Animation> list = new List<Animation>(FindModelComponents<Animation>());
		int count = list.Count;
		while (count-- > 0)
		{
			if (list[count].GetClip(clipName) == null)
			{
				list.RemoveAt(count);
			}
		}
		return list.ToArray();
	}

	public Animation[] FindModelAnimators()
	{
		return new List<Animation>(FindModelComponents<Animation>()).ToArray();
	}
}
