using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class PhysicMaterialColor
{
	[SerializeField]
	public string materialName;

	[SerializeField]
	public Color color;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PhysicMaterialColor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PhysicMaterialColor(string name, Color color)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}
}
