using System;
using System.Collections.Generic;
using UnityEngine;

namespace Expansions.Missions.Flow;

public class MEPath : IConfigNode
{
	public Mission mission;

	public List<MENode> Nodes { get; set; }

	public MENode First
	{
		get
		{
			if (Nodes != null && Nodes.Count > 0)
			{
				return Nodes[0];
			}
			return null;
		}
	}

	public MEPath(Mission mission)
	{
		this.mission = mission;
		Nodes = new List<MENode>();
	}

	public MEPath(MENode firstNode)
		: this(firstNode.mission)
	{
		Nodes.Add(firstNode);
	}

	public MEPath Clone()
	{
		MEPath mEPath = new MEPath(mission);
		mEPath.AddNodes(Nodes);
		return mEPath;
	}

	public void AddNodes(List<MENode> addNodes)
	{
		Nodes.AddRange(addNodes);
	}

	public bool GetConvergence(MEPath target, out MEFlowConvergence c, int minHops = 0)
	{
		int num = minHops;
		while (true)
		{
			if (num < Nodes.Count)
			{
				if (target.Nodes.IndexOf(Nodes[num]) > -1)
				{
					break;
				}
				num++;
				continue;
			}
			c = null;
			return false;
		}
		c = new MEFlowConvergence(Nodes[num], num);
		return true;
	}

	public void Load(ConfigNode node)
	{
		Nodes = new List<MENode>();
		string[] values = node.GetValues("nodeId");
		for (int i = 0; i < values.Length; i++)
		{
			Guid key = new Guid(values[i]);
			if (mission.nodes.ContainsKey(key))
			{
				Nodes.Add(mission.nodes[key]);
				continue;
			}
			Debug.LogErrorFormat("Unable to find a node in the mission from the flow - Mission flow may be incorrect. id={0} ", values[i]);
		}
	}

	public void Save(ConfigNode node)
	{
		for (int i = 0; i < Nodes.Count; i++)
		{
			node.AddValue("nodeId", Nodes[i].id.ToString());
		}
	}
}
