using System;
using UnityEngine;

namespace ns5;

[Serializable]
public class Icon
{
	public string name = "";

	public Texture iconNormal;

	public Texture iconSelected;

	public bool simple;

	public Icon(string name, Texture iconNormal, Texture iconSelected, bool simple = false)
	{
		this.name = name;
		this.iconNormal = iconNormal;
		this.iconSelected = iconSelected;
		this.simple = simple;
	}

	public string GetName()
	{
		if (name == string.Empty)
		{
			return iconNormal.name;
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
		if (iconNormal == icon.iconNormal)
		{
			return iconSelected == icon.iconSelected;
		}
		return false;
	}

	public bool Equals(Icon p)
	{
		if (p == null)
		{
			return false;
		}
		if (iconNormal == p.iconNormal)
		{
			return iconSelected == p.iconSelected;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return iconNormal.GetHashCode() ^ iconSelected.GetHashCode();
	}
}
