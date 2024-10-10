using System.Collections.Generic;
using CompoundParts;
using UnityEngine;

namespace ns3;

public class StackFlowGraph
{
	public FlowGraph<Part> delivery { get; set; }

	public FlowGraph<Part> request { get; set; }

	public StackFlowGraph(List<Part> ship)
	{
		delivery = new FlowGraph<Part>();
		request = new FlowGraph<Part>();
		int i = 0;
		for (int count = ship.Count; i < count; i++)
		{
			Part part = ship[i];
			delivery.Add(part);
			request.Add(part);
		}
		int j = 0;
		for (int count2 = ship.Count; j < count2; j++)
		{
			Part part = ship[j];
			UpdatePartVertex(request: false, part, delivery.lookup);
			UpdatePartVertex(request: true, part, request.lookup);
		}
	}

	public void UpdatePartVertex(bool request, Part part, Dictionary<Part, Vertex<Part>> lookup)
	{
		Vertex<Part> vertex = lookup[part];
		int i = 0;
		for (int count = part.fuelLookupTargets.Count; i < count; i++)
		{
			if ((!(part.fuelLookupTargets[i] == part.parent) || !part.isAttached) && (!(part.fuelLookupTargets[i] != part.parent) || !part.fuelLookupTargets[i].isAttached))
			{
				continue;
			}
			Vertex<Part> value;
			if (part.fuelLookupTargets[i] is CompoundPart)
			{
				if (part.fuelLookupTargets[i].parent != null && lookup.TryGetValue((part.fuelLookupTargets[i] is CompoundPart) ? part.fuelLookupTargets[i].parent : part.fuelLookupTargets[i], out value))
				{
					if (request)
					{
						vertex.Dependencies.Add(value);
					}
					else
					{
						value.Dependencies.Add(vertex);
					}
				}
			}
			else if (lookup.TryGetValue(part.fuelLookupTargets[i], out value))
			{
				if (request)
				{
					vertex.Dependencies.Add(value);
				}
				else
				{
					value.Dependencies.Add(vertex);
				}
			}
		}
		if (!part.fuelCrossFeed)
		{
			return;
		}
		int j = 0;
		for (int count2 = part.attachNodes.Count; j < count2; j++)
		{
			AttachNode attachNode = part.attachNodes[j];
			if (attachNode.attachedPart == null || !attachNode.ResourceXFeed)
			{
				continue;
			}
			bool flag = false;
			AttachNode attachNode2 = attachNode.FindOpposingNode();
			if (attachNode2 != null)
			{
				flag = !attachNode2.ResourceXFeed && !attachNode2.AllowOneWayXFeed;
			}
			if (!flag && (string.IsNullOrEmpty(part.NoCrossFeedNodeKey) || !attachNode.id.Contains(part.NoCrossFeedNodeKey)) && lookup.TryGetValue(attachNode.attachedPart, out var value2))
			{
				if (request)
				{
					vertex.Dependencies.Add(value2);
				}
				else
				{
					value2.Dependencies.Add(vertex);
				}
			}
		}
		if (part.srfAttachNode != null && part.srfAttachNode.attachedPart != null && part.srfAttachNode.attachedPart.fuelCrossFeed && lookup.TryGetValue(part.srfAttachNode.attachedPart, out var value3))
		{
			vertex.Dependencies.Add(value3);
			value3.Dependencies.Add(vertex);
		}
	}

	public void BuildTransformGuides()
	{
		int i = 0;
		for (int count = delivery.graph.Count; i < count; i++)
		{
			Vertex<Part> partVertex = delivery.graph[i];
			TransformGuideOnVertex(request: false, partVertex, delivery.lookup);
		}
		int j = 0;
		for (int count2 = request.graph.Count; j < count2; j++)
		{
			Vertex<Part> partVertex = request.graph[j];
			TransformGuideOnVertex(request: true, partVertex, request.lookup);
		}
	}

	public void TransformGuideOnVertex(bool request, Vertex<Part> partVertex, Dictionary<Part, Vertex<Part>> lookup)
	{
		Part value = partVertex.Value;
		int i = 0;
		for (int count = value.fuelLookupTargets.Count; i < count; i++)
		{
			if (((value.fuelLookupTargets[i] == value.parent && value.isAttached) || (value.fuelLookupTargets[i] != value.parent && value.fuelLookupTargets[i].isAttached)) && lookup.TryGetValue(value.fuelLookupTargets[i].parent, out var value2))
			{
				CModuleLinkedMesh component = value.fuelLookupTargets[i].GetComponent<CModuleLinkedMesh>();
				if (request)
				{
					partVertex.transformGuide = new Dictionary<Vertex<Part>, KeyValuePair<Transform, Transform>>();
					partVertex.transformGuide.Add(value2, new KeyValuePair<Transform, Transform>(component.targetAnchor, component.transform));
				}
				else
				{
					value2.transformGuide = new Dictionary<Vertex<Part>, KeyValuePair<Transform, Transform>>();
					value2.transformGuide.Add(partVertex, new KeyValuePair<Transform, Transform>(component.targetAnchor, component.transform));
				}
			}
		}
	}
}
