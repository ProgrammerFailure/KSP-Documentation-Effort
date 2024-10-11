using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RUI.Icons.Simple;

[Serializable]
public class Icon
{
	public string name;

	public Texture texture;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Icon(string name, Texture texture)
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
