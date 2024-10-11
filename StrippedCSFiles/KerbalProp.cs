using System;
using System.Runtime.CompilerServices;
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KerbalProp()
	{
		throw null;
	}
}
