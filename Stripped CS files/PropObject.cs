using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PropObject : MonoBehaviour
{
	public PropTools.Prop prop;

	public static PropObject Create(Transform parent, PropTools.Prop prop)
	{
		GameObject obj = new GameObject();
		obj.name = prop.propName;
		obj.transform.parent = parent;
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localRotation = Quaternion.identity;
		PropObject propObject = obj.AddComponent<PropObject>();
		propObject.prop = prop;
		CreateProxies(obj, prop);
		return propObject;
	}

	public static void CreateProxies(GameObject go, PropTools.Prop prop)
	{
		List<Mesh> list = new List<Mesh>();
		List<Material> list2 = new List<Material>();
		GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
		Mesh sharedMesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
		foreach (PropTools.Proxy proxy in prop.proxies)
		{
			Vector3[] vertices = sharedMesh.vertices;
			for (int i = 0; i < vertices.Length; i++)
			{
				vertices[i].Scale(proxy.size);
				vertices[i] += proxy.center;
			}
			Mesh mesh = new Mesh();
			mesh.vertices = vertices;
			mesh.triangles = sharedMesh.triangles;
			mesh.uv = sharedMesh.uv;
			mesh.normals = sharedMesh.normals;
			Material material = new Material(Shader.Find("Diffuse"));
			material.color = proxy.color;
			material.hideFlags = HideFlags.HideInInspector | HideFlags.NotEditable;
			list.Add(mesh);
			list2.Add(material);
		}
		CombineInstance[] array = new CombineInstance[list.Count];
		for (int j = 0; j < list.Count; j++)
		{
			array[j].mesh = list[j];
		}
		Mesh mesh2 = new Mesh();
		mesh2.CombineMeshes(array, mergeSubMeshes: false, useMatrices: false);
		mesh2.RecalculateBounds();
		MeshFilter meshFilter = go.AddComponent<MeshFilter>();
		meshFilter.hideFlags = HideFlags.HideInInspector | HideFlags.NotEditable;
		meshFilter.sharedMesh = mesh2;
		MeshRenderer meshRenderer = go.AddComponent<MeshRenderer>();
		meshRenderer.hideFlags = HideFlags.HideInInspector | HideFlags.NotEditable;
		meshRenderer.sharedMaterials = list2.ToArray();
		BoxCollider boxCollider = go.AddComponent<BoxCollider>();
		boxCollider.hideFlags = HideFlags.HideInInspector | HideFlags.NotEditable;
		boxCollider.center = mesh2.bounds.center;
		boxCollider.size = mesh2.bounds.size;
		Object.DestroyImmediate(gameObject);
		for (int k = 0; k < list.Count; k++)
		{
			Object.DestroyImmediate(list[k]);
		}
	}

	public void RecreateProxies(PropTools.Prop newProp)
	{
		prop = newProp;
		MeshRenderer component = base.gameObject.GetComponent<MeshRenderer>();
		if (component != null)
		{
			Object.DestroyImmediate(component);
		}
		MeshFilter component2 = base.gameObject.GetComponent<MeshFilter>();
		if (component2 != null)
		{
			Object.DestroyImmediate(component2);
		}
		BoxCollider component3 = base.gameObject.GetComponent<BoxCollider>();
		if (component3 != null)
		{
			Object.DestroyImmediate(component3);
		}
		CreateProxies(base.gameObject, prop);
	}

	public void RefreshProxy()
	{
		PropTools propTools = (PropTools)GetComponentOnParent("PropTools", base.gameObject);
		if (!Directory.Exists(Path.Combine(propTools.propRootDirectory, Path.Combine(this.prop.directory, "prop.cfg"))))
		{
			string[] array = this.prop.directory.Split('\\');
			this.prop.directory = array[array.Length - 1];
		}
		FileInfo fileInfo = null;
		if (string.IsNullOrEmpty(this.prop.configName))
		{
			string text = Path.Combine(propTools.propRootDirectory, Path.Combine(this.prop.directory, "prop.cfg"));
			fileInfo = new FileInfo(text);
			if (fileInfo == null)
			{
				Debug.LogError("Cannot find prop file at '" + text + "'");
				return;
			}
		}
		else
		{
			string text2 = Path.Combine(propTools.propRootDirectory, Path.Combine(this.prop.directory, this.prop.configName));
			fileInfo = new FileInfo(text2);
			if (fileInfo == null)
			{
				Debug.LogError("Cannot find prop file at '" + text2 + "'");
				return;
			}
		}
		PropTools.Prop prop = propTools.CreatePropInfo(fileInfo);
		if (prop == null)
		{
			Debug.LogError("PropInfo is null");
		}
		else
		{
			RecreateProxies(prop);
		}
	}

	public static Component GetComponentOnParent(string type, GameObject obj)
	{
		if (obj.transform.parent != null)
		{
			Component component = obj.transform.parent.GetComponent(type);
			if (component != null)
			{
				return component;
			}
			return GetComponentOnParent(type, obj.transform.parent.gameObject);
		}
		return null;
	}
}
