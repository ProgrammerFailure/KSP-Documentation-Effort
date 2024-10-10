using System;
using System.Collections.Generic;

namespace ns3;

public static class StronglyConnectedComponentFinder
{
	public static LinkedList<LinkedList<Part>> stronglyConnectedComponents;

	public static LinkedList<HashSet<Part>> stronglyConnectedComponentSets;

	public static Stack<Vertex<Part>> stack;

	public static int index;

	public static LinkedList<LinkedList<Part>> DetectCycle(FlowGraph<Part> flowGraph)
	{
		stronglyConnectedComponents = new LinkedList<LinkedList<Part>>();
		index = 0;
		stack = new Stack<Vertex<Part>>();
		int i = 0;
		for (int count = flowGraph.graph.Count; i < count; i++)
		{
			Vertex<Part> vertex = flowGraph.graph[i];
			if (vertex.Index < 0)
			{
				StrongConnect(vertex);
			}
		}
		return stronglyConnectedComponents;
	}

	public static void StrongConnect(Vertex<Part> v)
	{
		v.Index = index;
		v.LowLink = index;
		index++;
		stack.Push(v);
		int i = 0;
		for (int count = v.Dependencies.Count; i < count; i++)
		{
			Vertex<Part> vertex = v.Dependencies[i];
			if (vertex.Index < 0)
			{
				StrongConnect(vertex);
				v.LowLink = Math.Min(v.LowLink, vertex.LowLink);
			}
			else if (stack.Contains(vertex))
			{
				v.LowLink = Math.Min(v.LowLink, vertex.Index);
			}
		}
		if (v.LowLink == v.Index)
		{
			LinkedList<Part> linkedList = new LinkedList<Part>();
			Vertex<Part> vertex;
			do
			{
				vertex = stack.Pop();
				linkedList.AddLast(vertex.Value);
			}
			while (v != vertex);
			stronglyConnectedComponents.AddLast(linkedList);
		}
	}

	public static LinkedList<HashSet<Part>> DetectCycleSets(FlowGraph<Part> flowGraph)
	{
		stronglyConnectedComponentSets = new LinkedList<HashSet<Part>>();
		index = 0;
		stack = new Stack<Vertex<Part>>();
		int i = 0;
		for (int count = flowGraph.graph.Count; i < count; i++)
		{
			Vertex<Part> vertex = flowGraph.graph[i];
			if (vertex.Index < 0)
			{
				StrongConnectSet(vertex);
			}
		}
		return stronglyConnectedComponentSets;
	}

	public static void StrongConnectSet(Vertex<Part> v)
	{
		v.Index = index;
		v.LowLink = index;
		index++;
		stack.Push(v);
		int i = 0;
		for (int count = v.Dependencies.Count; i < count; i++)
		{
			Vertex<Part> vertex = v.Dependencies[i];
			if (vertex.Index < 0)
			{
				StrongConnectSet(vertex);
				v.LowLink = Math.Min(v.LowLink, vertex.LowLink);
			}
			else if (stack.Contains(vertex))
			{
				v.LowLink = Math.Min(v.LowLink, vertex.Index);
			}
		}
		if (v.LowLink == v.Index)
		{
			HashSet<Part> hashSet = new HashSet<Part>();
			Vertex<Part> vertex;
			do
			{
				vertex = stack.Pop();
				hashSet.Add(vertex.Value);
			}
			while (v != vertex);
			stronglyConnectedComponentSets.AddLast(hashSet);
		}
	}
}
