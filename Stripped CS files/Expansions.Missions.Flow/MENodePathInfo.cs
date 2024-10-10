using System;
using System.Collections.Generic;
using UnityEngine;

namespace Expansions.Missions.Flow;

public class MENodePathInfo : IConfigNode
{
	public MENode node;

	public List<MEPath> paths;

	public Mission mission;

	public MENodePathInfo(Mission mission)
	{
		this.mission = mission;
		paths = new List<MEPath>();
	}

	public MENodePathInfo(MENode node)
		: this(node.mission)
	{
		this.node = node;
	}

	public void AddPath(MEPath path)
	{
		paths.Add(path);
	}

	public void Load(ConfigNode node)
	{
		Guid value = Guid.Empty;
		if (node.TryGetValue("startNodeId", ref value))
		{
			if (mission.nodes.ContainsKey(value))
			{
				this.node = mission.nodes[value];
			}
			else
			{
				Debug.LogErrorFormat("Unable to find startNodeId ({0}) in the loaded mission ({1}) Nodes.", value, mission.title);
			}
		}
		paths = new List<MEPath>();
		ConfigNode[] nodes = node.GetNodes("PATH");
		for (int i = 0; i < nodes.Length; i++)
		{
			MEPath mEPath = new MEPath(mission);
			mEPath.Load(nodes[i]);
			paths.Add(mEPath);
		}
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("startNodeId", this.node.id.ToString());
		for (int i = 0; i < paths.Count; i++)
		{
			ConfigNode configNode = node.AddNode("PATH");
			paths[i].Save(configNode);
		}
	}
}
