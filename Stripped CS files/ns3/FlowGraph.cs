using System.Collections.Generic;
using UnityEngine;

namespace ns3;

public class FlowGraph<T>
{
	public List<Vertex<T>> graph;

	public Dictionary<T, Vertex<T>> lookup;

	public FlowGraph()
	{
		graph = new List<Vertex<T>>();
		lookup = new Dictionary<T, Vertex<T>>();
	}

	public Vertex<T> Add(T part)
	{
		if (lookup.ContainsKey(part))
		{
			string text = "[FlowGraph]: Graph already contains item!";
			if (part is Part)
			{
				Part part2 = part as Part;
				text = text + " Part " + part2.name + " with id " + part2.craftID;
			}
			Debug.LogError(text);
			return null;
		}
		Vertex<T> vertex = new Vertex<T>(part);
		graph.Add(vertex);
		lookup.Add(part, vertex);
		return vertex;
	}

	public Vertex<T> GetPartVertex(T part)
	{
		return lookup[part];
	}
}
