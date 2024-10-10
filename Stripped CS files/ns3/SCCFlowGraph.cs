using System.Collections.Generic;

namespace ns3;

public class SCCFlowGraph
{
	public class GraphSet
	{
		public FlowGraph<HashSet<Part>> flowGraph { get; set; }

		public LinkedList<HashSet<Part>> sCC { get; set; }

		public Dictionary<Part, Vertex<HashSet<Part>>> lookup { get; set; }

		public Dictionary<Vertex<HashSet<Part>>, HashSet<Part>> supersetLookup { get; set; }

		public GraphSet(LinkedList<HashSet<Part>> sCC)
		{
			this.sCC = sCC;
			flowGraph = new FlowGraph<HashSet<Part>>();
			lookup = new Dictionary<Part, Vertex<HashSet<Part>>>();
			supersetLookup = new Dictionary<Vertex<HashSet<Part>>, HashSet<Part>>();
		}
	}

	public GraphSet requests { get; set; }

	public GraphSet delivery { get; set; }

	public StackFlowGraph stackFlowGraph { get; set; }

	public SCCFlowGraph(List<Part> ship)
	{
		stackFlowGraph = new StackFlowGraph(ship);
		requests = new GraphSet(StronglyConnectedComponentFinder.DetectCycleSets(stackFlowGraph.request));
		delivery = new GraphSet(StronglyConnectedComponentFinder.DetectCycleSets(stackFlowGraph.delivery));
		Process(requests, stackFlowGraph.request);
		Process(delivery, stackFlowGraph.delivery);
	}

	public HashSet<Part> GetAllRequests(Part part)
	{
		return requests.supersetLookup[requests.lookup[part]];
	}

	public HashSet<Part> GetAllDeliveries(Part part)
	{
		return delivery.supersetLookup[delivery.lookup[part]];
	}

	public LinkedList<HashSet<Part>> GetConnectedComponentsInRequests()
	{
		return requests.sCC;
	}

	public LinkedList<HashSet<Part>> GetConnectedComponentsInDeliveries()
	{
		return delivery.sCC;
	}

	public void Process(GraphSet sCCFlowGraphSet, FlowGraph<Part> stackFlowGraph)
	{
		IEnumerator<HashSet<Part>> enumerator = sCCFlowGraphSet.sCC.GetEnumerator();
		while (enumerator.MoveNext())
		{
			HashSet<Part> current = enumerator.Current;
			Vertex<HashSet<Part>> value = sCCFlowGraphSet.flowGraph.Add(current);
			IEnumerator<Part> enumerator2 = current.GetEnumerator();
			while (enumerator2.MoveNext())
			{
				Part current2 = enumerator2.Current;
				sCCFlowGraphSet.lookup.Add(current2, value);
			}
		}
		enumerator = sCCFlowGraphSet.sCC.GetEnumerator();
		while (enumerator.MoveNext())
		{
			HashSet<Part> current = enumerator.Current;
			IEnumerator<Part> enumerator2 = current.GetEnumerator();
			while (enumerator2.MoveNext())
			{
				Part current2 = enumerator2.Current;
				Vertex<Part> vertex = stackFlowGraph.lookup[current2];
				int i = 0;
				for (int count = vertex.Dependencies.Count; i < count; i++)
				{
					Vertex<Part> vertex2 = vertex.Dependencies[i];
					if (!current.Contains(vertex2.Value))
					{
						Vertex<HashSet<Part>> value = sCCFlowGraphSet.flowGraph.lookup[current];
						value.Dependencies.Add(sCCFlowGraphSet.lookup[vertex2.Value]);
					}
				}
			}
		}
		int j = 0;
		for (int count2 = sCCFlowGraphSet.flowGraph.graph.Count; j < count2; j++)
		{
			Vertex<HashSet<Part>> value = sCCFlowGraphSet.flowGraph.graph[j];
			Connect(sCCFlowGraphSet, value, new HashSet<Part>());
		}
	}

	public HashSet<Part> Connect(GraphSet graph, Vertex<HashSet<Part>> vhp, HashSet<Part> hp)
	{
		if (vhp.Index < 0)
		{
			hp = new HashSet<Part>(vhp.Value);
			vhp.Index = 1;
			int i = 0;
			for (int count = vhp.Dependencies.Count; i < count; i++)
			{
				hp.UnionWith(Connect(graph, vhp.Dependencies[i], hp));
			}
			graph.supersetLookup.Add(vhp, hp);
		}
		else
		{
			hp = graph.supersetLookup[vhp];
		}
		return hp;
	}
}
