using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class FlagMesh
{
	public string name;

	public int index;

	public int indexOffset;

	public FlagOrientation flagOrientation;

	public string displayName;

	public string meshName;

	public Transform flagTransform;

	public Renderer flagRend;

	public float mass;

	public float cost;

	public string variantNames;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FlagMesh()
	{
		throw null;
	}
}
