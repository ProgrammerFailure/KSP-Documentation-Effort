using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.FX.Fireworks;

[Serializable]
public class FireworkFXList : IEnumerable
{
	[SerializeField]
	public Dictionary<int, FireworkFXDefinition> fireworkFX;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FireworkFXList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FireworkFXDefinition Add(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Contains(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int Count()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public IEnumerator<FireworkFXDefinition> GetEnumerator()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
