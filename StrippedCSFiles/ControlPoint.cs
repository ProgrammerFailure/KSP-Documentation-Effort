using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class ControlPoint
{
	[SerializeField]
	public string name;

	[SerializeField]
	public Vector3 orientation;

	[SerializeField]
	public Transform transform;

	[SerializeField]
	public string displayName;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ControlPoint()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ControlPoint(string name, string displayName, Transform transform, Vector3 orientation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}
}
