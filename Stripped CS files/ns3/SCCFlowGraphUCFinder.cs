using System.Collections.Generic;

namespace ns3;

public class SCCFlowGraphUCFinder
{
	public class GraphSet
	{
		public Dictionary<int, HashSet<Part>> entries { get; set; }

		public Dictionary<int, HashSet<Vertex<Part>>> entriesVertex { get; set; }

		public Dictionary<Part, HashSet<int>> reverseEntries { get; set; }

		public GraphSet()
		{
			entries = new Dictionary<int, HashSet<Part>>();
			entriesVertex = new Dictionary<int, HashSet<Vertex<Part>>>();
			reverseEntries = new Dictionary<Part, HashSet<int>>();
		}

		public void AddEntrypoint(int id, Vertex<Part> partVertex)
		{
			if (entriesVertex.ContainsKey(id))
			{
				entriesVertex[id].Add(partVertex);
				entries[id].Add(partVertex.Value);
			}
			else
			{
				entriesVertex.Add(id, new HashSet<Vertex<Part>>(new Vertex<Part>[1] { partVertex }));
				entries.Add(id, new HashSet<Part> { partVertex.Value });
			}
			if (reverseEntries.ContainsKey(partVertex.Value))
			{
				reverseEntries[partVertex.Value].Add(id);
				return;
			}
			reverseEntries.Add(partVertex.Value, new HashSet<int> { id });
		}
	}

	public SCCFlowGraph sccFlowGraph { get; set; }

	public GraphSet delivery { get; set; }

	public GraphSet request { get; set; }

	public SCCFlowGraphUCFinder(List<Part> ship)
	{
		sccFlowGraph = new SCCFlowGraph(ship);
		delivery = new GraphSet();
		request = new GraphSet();
		BuildEntrySets();
	}

	public SCCFlowGraphUCFinder(SCCFlowGraph graph)
	{
		sccFlowGraph = graph;
		delivery = new GraphSet();
		request = new GraphSet();
		BuildEntrySets();
	}

	public void BuildEntrySets()
	{
		StackFlowGraph stackFlowGraph = sccFlowGraph.stackFlowGraph;
		int i = 0;
		for (int count = stackFlowGraph.delivery.graph.Count; i < count; i++)
		{
			Part value = stackFlowGraph.delivery.graph[i].Value;
			Vertex<Part> partVertex = stackFlowGraph.delivery.GetPartVertex(value);
			Vertex<Part> partVertexR = stackFlowGraph.request.GetPartVertex(value);
			IsEntryPoint(value, delegate(int id)
			{
				delivery.AddEntrypoint(id, partVertex);
			}, delegate(int id)
			{
				request.AddEntrypoint(id, partVertexR);
			});
		}
	}

	public static bool IsEntryPoint(Part part)
	{
		return IsEntryPoint(part, null, null);
	}

	public static bool IsEntryPointDelivery(Part part)
	{
		return IsEntryPointDelivery(part, null);
	}

	public static bool IsEntryPointRequest(Part part)
	{
		return IsEntryPointRequest(part, null);
	}

	public static bool IsEntryPoint(Part part, Callback<int> deliveryCallback, Callback<int> requestCallback)
	{
		if (deliveryCallback != null)
		{
			IsEntryPointDelivery(part, deliveryCallback);
		}
		else if (IsEntryPointDelivery(part, deliveryCallback))
		{
			return true;
		}
		if (requestCallback != null)
		{
			IsEntryPointRequest(part, requestCallback);
		}
		else if (IsEntryPointRequest(part, requestCallback))
		{
			return true;
		}
		return false;
	}

	public static bool IsEntryPointDelivery(Part part, Callback<int> deliveryCallback)
	{
		int num = 0;
		int count = part.Resources.Count;
		while (true)
		{
			if (num < count)
			{
				PartResource partResource = part.Resources[num];
				if (partResource.amount > 0.0 && (partResource.info.resourceFlowMode == ResourceFlowMode.STACK_PRIORITY_SEARCH || partResource.info.resourceFlowMode == ResourceFlowMode.STAGE_STACK_FLOW || partResource.info.resourceFlowMode == ResourceFlowMode.STAGE_STACK_FLOW_BALANCE))
				{
					if (deliveryCallback == null)
					{
						break;
					}
					deliveryCallback(partResource.info.id);
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public static bool IsEntryPointRequest(Part part, Callback<int> requestCallback)
	{
		List<IResourceConsumer> modules = part.Modules.GetModules<IResourceConsumer>();
		int i = 0;
		for (int count = modules.Count; i < count; i++)
		{
			List<PartResourceDefinition> consumedResources = modules[i].GetConsumedResources();
			int count2 = consumedResources.Count;
			for (int j = 0; j < count2; j++)
			{
				PartResourceDefinition partResourceDefinition = consumedResources[j];
				if (partResourceDefinition.resourceFlowMode == ResourceFlowMode.STACK_PRIORITY_SEARCH || partResourceDefinition.resourceFlowMode == ResourceFlowMode.STAGE_STACK_FLOW || partResourceDefinition.resourceFlowMode == ResourceFlowMode.STAGE_STACK_FLOW_BALANCE)
				{
					if (requestCallback == null)
					{
						return true;
					}
					requestCallback(partResourceDefinition.id);
				}
			}
		}
		return false;
	}

	public HashSet<Part> GetUnreachableFuelRequests(int resourceId)
	{
		HashSet<Part> value;
		bool num = request.entries.TryGetValue(resourceId, out value);
		HashSet<Part> value2;
		bool flag = delivery.entries.TryGetValue(resourceId, out value2);
		if (num)
		{
			if (flag)
			{
				HashSet<Part> hashSet = new HashSet<Part>(value);
				IEnumerator<Part> enumerator = value.GetEnumerator();
				while (enumerator.MoveNext())
				{
					Part current = enumerator.Current;
					if (sccFlowGraph.GetAllRequests(current).Overlaps(value2))
					{
						hashSet.Remove(current);
					}
				}
				return hashSet;
			}
			return value;
		}
		return new HashSet<Part>();
	}

	public HashSet<Part> GetUnreachableFuelDeliveries(int resourceId)
	{
		HashSet<Part> value;
		bool num = delivery.entries.TryGetValue(resourceId, out value);
		HashSet<Part> value2;
		bool flag = request.entries.TryGetValue(resourceId, out value2);
		if (num)
		{
			if (flag)
			{
				HashSet<Part> hashSet = new HashSet<Part>(value);
				IEnumerator<Part> enumerator = value.GetEnumerator();
				while (enumerator.MoveNext())
				{
					Part current = enumerator.Current;
					if (sccFlowGraph.GetAllDeliveries(current).Overlaps(value2))
					{
						hashSet.Remove(current);
					}
				}
				return hashSet;
			}
			return value;
		}
		return new HashSet<Part>();
	}
}
