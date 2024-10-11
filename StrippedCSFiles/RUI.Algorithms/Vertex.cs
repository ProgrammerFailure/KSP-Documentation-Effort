using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RUI.Algorithms;

public class Vertex<T>
{
	public HashSet<int> contained;

	internal int Index
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		set
		{
			throw null;
		}
	}

	internal int LowLink
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		set
		{
			throw null;
		}
	}

	public T Value
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		set
		{
			throw null;
		}
	}

	public List<Vertex<T>> Dependencies
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		set
		{
			throw null;
		}
	}

	public Dictionary<Vertex<T>, KeyValuePair<Transform, Transform>> transformGuide
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vertex()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vertex(T value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vertex(IEnumerable<Vertex<T>> dependencies)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vertex(T value, IEnumerable<Vertex<T>> dependencies)
	{
		throw null;
	}
}
