using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class PartModuleList : IEnumerable
{
	[HideInInspector]
	[SerializeField]
	private Part part;

	[SerializeField]
	[HideInInspector]
	private List<PartModule> modules;

	public int Count
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public PartModule this[string className]
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public PartModule this[int index]
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public PartModule this[uint persistentId]
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartModuleList(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<T> GetModules<T>() where T : class
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public IScalarModule GetScalarModule(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<IScalarModule> GetScalarModules(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public IEngineStatus FindEngineNearby(string engineName, int engineIndex, bool preferMultiMode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public IEngineStatus FindEngineInterface(string engineName, int engineIndex, bool preferMultiMode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<PartModule>.Enumerator GetEnumerator()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Add(PartModule module)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Remove(PartModule module)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int IndexOf(PartModule module)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartModule GetModule(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartModule GetModule(string moduleName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T GetModule<T>(int index = 0) where T : PartModule
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Contains(string className)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Contains(int classID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Contains(uint persistentId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Contains<T>() where T : PartModule
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Clear()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Send(string actionName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Send(int actionID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Send(string actionName, BaseEventDetails actionData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Send(int actionID, BaseEventDetails actionData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SendDefault()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SendDefault(BaseEventDetails actionData)
	{
		throw null;
	}
}
