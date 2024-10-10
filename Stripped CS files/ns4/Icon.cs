using System;
using UnityEngine;

namespace ns4;

[Serializable]
public class Icon
{
	public string name = "";

	public Texture texture;

	public Icon(string name, Texture texture)
	{
		this.name = name;
		this.texture = texture;
	}

	public string GetName()
	{
		if (name == string.Empty)
		{
			return texture.name;
		}
		return name;
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (!(obj is Icon icon))
		{
			return false;
		}
		return texture == icon.texture;
	}

	public bool Equals(Icon p)
	{
		if (p == null)
		{
			return false;
		}
		return texture == p.texture;
	}

	public override int GetHashCode()
	{
		return texture.GetHashCode();
	}
}
