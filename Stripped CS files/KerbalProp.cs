using System;
using UnityEngine;

[Serializable]
public class KerbalProp
{
	public string name;

	public GameObject prefab;

	public GameObject jetpackPrefab;

	public Transform anchor;

	public GameObject instance;

	public MeshRenderer mesh;

	public Transform additionalObject;

	public GameObject additionalObjectPrefab;

	public Animation animation;
}
