using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RUI.Algorithms;

public static class StronglyConnectedComponentFinder
{
	private static LinkedList<LinkedList<Part>> stronglyConnectedComponents;

	private static LinkedList<HashSet<Part>> stronglyConnectedComponentSets;

	private static Stack<Vertex<Part>> stack;

	private static int index;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static LinkedList<LinkedList<Part>> DetectCycle(FlowGraph<Part> flowGraph)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void StrongConnect(Vertex<Part> v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static LinkedList<HashSet<Part>> DetectCycleSets(FlowGraph<Part> flowGraph)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void StrongConnectSet(Vertex<Part> v)
	{
		throw null;
	}
}
