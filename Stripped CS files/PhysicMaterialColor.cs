using System;
using UnityEngine;

[Serializable]
public class PhysicMaterialColor
{
	[SerializeField]
	public string materialName;

	[SerializeField]
	public Color color;

	public PhysicMaterialColor()
	{
	}

	public PhysicMaterialColor(string name, Color color)
		: this()
	{
		materialName = name;
		this.color = color;
	}

	public void Load(ConfigNode node)
	{
		materialName = "";
		color = Color.white;
		node.TryGetValue("name", ref materialName);
		node.TryGetValue("color", ref color);
	}
}
