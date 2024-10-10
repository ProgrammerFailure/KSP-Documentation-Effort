using System;
using UnityEngine;

public static class GameObjectExtension
{
	public static T GetComponentCached<T>(this GameObject gameobject, ref T cache) where T : Component
	{
		if (cache != null)
		{
			return cache;
		}
		cache = gameobject.GetComponent<T>();
		return cache;
	}

	public static T GetComponentCached<T>(this Component component, ref T cache) where T : Component
	{
		if (cache != null)
		{
			return cache;
		}
		cache = component.GetComponent<T>();
		return cache;
	}

	public static T AddOrGetComponent<T>(this GameObject obj) where T : Component
	{
		return obj.GetComponent<T>() ?? obj.AddComponent<T>();
	}

	public static T GetComponentOnParent<T>(this GameObject obj) where T : Component
	{
		if (obj.transform.parent == null)
		{
			return null;
		}
		return obj.transform.parent.gameObject.GetComponent<T>();
	}

	public static Component GetComponentOnParent(this GameObject obj, string type)
	{
		if (obj.transform.parent == null)
		{
			return null;
		}
		return obj.transform.parent.gameObject.GetComponent(type);
	}

	public static T GetComponentUpwards<T>(this GameObject obj) where T : Component
	{
		T component = obj.GetComponent<T>();
		if (component != null)
		{
			return component;
		}
		if (obj.transform.parent != null)
		{
			return obj.transform.parent.gameObject.GetComponentUpwards<T>();
		}
		return null;
	}

	public static Component GetComponentUpwards(this GameObject obj, string type)
	{
		Component component = obj.GetComponent(type);
		if (component != null)
		{
			return component;
		}
		if (obj.transform.parent != null)
		{
			return obj.transform.parent.gameObject.GetComponentUpwards(type);
		}
		return null;
	}

	public static int GetComponentIndex<T>(this GameObject host, T tgt)
	{
		return Array.IndexOf(host.GetComponents<T>(), tgt);
	}

	public static GameObject GetChild(this GameObject obj, string childName)
	{
		int num = 0;
		Transform child;
		while (true)
		{
			if (num < obj.transform.childCount)
			{
				child = obj.transform.GetChild(num);
				if (obj.transform.GetChild(num).name == childName)
				{
					break;
				}
				num++;
				continue;
			}
			int num2 = 0;
			GameObject child2;
			while (true)
			{
				if (num2 < obj.transform.childCount)
				{
					child = obj.transform.GetChild(num2);
					child2 = child.gameObject.GetChild(childName);
					if (child2 != null)
					{
						break;
					}
					num2++;
					continue;
				}
				return null;
			}
			return child2;
		}
		return child.gameObject;
	}

	public static void SetLayerRecursive(this GameObject obj, int layer, int ignoreLayersMask = 0)
	{
		obj.SetLayerRecursive(layer, filterTranslucent: false, ignoreLayersMask);
	}

	public static void SetLayerRecursive(this GameObject obj, int layer, bool filterTranslucent, int ignoreLayersMask = 0)
	{
		if (((1 << obj.layer) & ignoreLayersMask) == 0)
		{
			bool flag = false;
			if (filterTranslucent)
			{
				Renderer[] components = obj.GetComponents<Renderer>();
				for (int i = 0; i < components.Length; i++)
				{
					Material[] materials = components[i].materials;
					for (int j = 0; j < materials.Length; j++)
					{
						if (materials[j].shader.name.Contains("Translucent"))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
			if (!flag)
			{
				obj.layer = layer;
			}
		}
		for (int k = 0; k < obj.transform.childCount; k++)
		{
			obj.transform.GetChild(k).gameObject.SetLayerRecursive(layer, filterTranslucent, ignoreLayersMask);
		}
	}

	public static void SetTagsRecursive(this GameObject obj, string tag, params string[] ignoreTags)
	{
		bool flag = true;
		int num = ignoreTags.Length;
		while (num-- > 0)
		{
			if (ignoreTags[num] == obj.tag)
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			obj.tag = tag;
		}
		for (int i = 0; i < obj.transform.childCount; i++)
		{
			obj.transform.GetChild(i).gameObject.SetTagsRecursive(tag, ignoreTags);
		}
	}

	public static void DestroyGameObject(this GameObject obj)
	{
		UnityEngine.Object.Destroy(obj);
	}

	public static void DestroyGameObjectImmediate(this GameObject obj)
	{
		UnityEngine.Object.DestroyImmediate(obj);
	}
}
