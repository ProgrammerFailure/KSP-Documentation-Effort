using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RUI.Algorithms;

public class FlowGraph<T>
{
	public List<Vertex<T>> graph;

	public Dictionary<T, Vertex<T>> lookup;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FlowGraph()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vertex<T> Add(T part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vertex<T> GetPartVertex(T part)
	{
		throw null;
	}
}
