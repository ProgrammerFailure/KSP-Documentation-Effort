using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class PartResourceDefinitionList : IEnumerable
{
	[SerializeField]
	private Dictionary<int, PartResourceDefinition> dict;

	public PartResourceDefinition this[string name]
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public PartResourceDefinition this[int id]
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int Count
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartResourceDefinitionList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartResourceDefinitionList(PartResourceDefinitionList old)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartResourceDefinition Add(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Add(PartResourceDefinition def)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Contains(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public IEnumerator<PartResourceDefinition> GetEnumerator()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Clear()
	{
		throw null;
	}
}
