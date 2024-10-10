using UnityEngine;

public static class TransformExtension
{
	public static void NestToParent(this Transform t, Transform newParent)
	{
		if (!(newParent == t))
		{
			t.SetParent(newParent);
			if (newParent != null)
			{
				t.localPosition = Vector3.zero;
				t.localRotation = Quaternion.identity;
			}
		}
	}

	public static void NestToParent(this Transform t, Transform newParent, bool resetParent)
	{
		if (resetParent)
		{
			t.SetParent(null);
			t.localScale = Vector3.one;
		}
		t.NestToParent(newParent);
	}

	public static Transform FindChild(this Transform t, string childName)
	{
		return FindChildRecursive(t, childName, findActiveChild: false);
	}

	public static Transform FindChild(this Transform t, string childName, bool findActiveChild)
	{
		return FindChildRecursive(t, childName, findActiveChild);
	}

	public static Transform FindChildRecursive(Transform parent, string childName, bool findActiveChild)
	{
		if (findActiveChild)
		{
			if (parent != null && parent.gameObject.activeInHierarchy && parent.gameObject.name == childName)
			{
				return parent;
			}
		}
		else if (parent.gameObject.name == childName)
		{
			return parent;
		}
		int num = 0;
		Transform transform;
		while (true)
		{
			if (num < parent.childCount)
			{
				transform = FindChildRecursive(parent.GetChild(num), childName, findActiveChild);
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

	public static Transform FindParent(this Transform t, string parentName, bool findActiveParent)
	{
		return FindParentRecursive(t, parentName, findActiveParent);
	}

	public static Transform FindParentRecursive(Transform parent, string parentName, bool findActiveParent)
	{
		if (findActiveParent)
		{
			if (parent != null && parent.gameObject.activeInHierarchy && parent.gameObject.name == parentName)
			{
				return parent;
			}
		}
		else if (parent.gameObject.name == parentName)
		{
			return parent;
		}
		if (parent.parent != null)
		{
			Transform transform = FindParentRecursive(parent.parent, parentName, findActiveParent);
			if (transform != null)
			{
				return transform;
			}
		}
		return null;
	}

	public static void SetLayerRecursive(this Transform root, int layer)
	{
		root.gameObject.layer = layer;
		for (int i = 0; i < root.childCount; i++)
		{
			root.GetChild(i).SetLayerRecursive(layer);
		}
	}

	public static void SetShader(this Transform root, string shader)
	{
		Shader shader2 = Shader.Find(shader);
		if (shader2 == null)
		{
			return;
		}
		MeshRenderer[] componentsInChildren = root.GetComponentsInChildren<MeshRenderer>();
		if (componentsInChildren != null)
		{
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].material.shader = shader2;
			}
		}
	}
}
