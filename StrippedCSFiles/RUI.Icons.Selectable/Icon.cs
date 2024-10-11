using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RUI.Icons.Selectable;

[Serializable]
public class Icon
{
	public string name;

	public Texture iconNormal;

	public Texture iconSelected;

	public bool simple;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Icon(string name, Texture iconNormal, Texture iconSelected, bool simple = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool Equals(object obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Equals(Icon p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override int GetHashCode()
	{
		throw null;
	}
}
