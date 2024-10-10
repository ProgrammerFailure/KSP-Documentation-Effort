using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ProgressTree : IConfigNode
{
	[SerializeField]
	public List<ProgressNode> nodes;

	public int Count => nodes.Count;

	public ProgressNode this[int i] => nodes[i];

	public ProgressNode this[string s]
	{
		get
		{
			int num = 0;
			int count = nodes.Count;
			ProgressNode progressNode;
			while (true)
			{
				if (num < count)
				{
					progressNode = nodes[num];
					if (progressNode.Id == s)
					{
						break;
					}
					num++;
					continue;
				}
				return null;
			}
			return progressNode;
		}
	}

	public ProgressTree()
	{
		nodes = new List<ProgressNode>();
	}

	public void AddNode(ProgressNode node)
	{
		nodes.Add(node);
	}

	public bool Contains(ProgressNode node)
	{
		return nodes.Contains(node);
	}

	public bool AllComplete()
	{
		int count = nodes.Count;
		do
		{
			if (count-- <= 0)
			{
				return true;
			}
		}
		while (nodes[count].IsComplete);
		return false;
	}

	public List<ProgressNode>.Enumerator GetEnumerator()
	{
		return nodes.GetEnumerator();
	}

	public void Load(ConfigNode node)
	{
		int i = 0;
		for (int count = nodes.Count; i < count; i++)
		{
			ProgressNode progressNode = nodes[i];
			if (node.HasNode(progressNode.Id))
			{
				progressNode.Load(node.GetNode(progressNode.Id));
			}
		}
	}

	public void Save(ConfigNode node)
	{
		Debug.Log("Saving Achievements Tree...");
		int i = 0;
		for (int count = nodes.Count; i < count; i++)
		{
			ProgressNode progressNode = nodes[i];
			if (progressNode.IsReached)
			{
				progressNode.Save(node.AddNode(progressNode.Id));
			}
		}
	}

	public void Deploy()
	{
		int i = 0;
		for (int count = nodes.Count; i < count; i++)
		{
			ProgressNode progressNode = nodes[i];
			if (progressNode.OnDeploy != null)
			{
				progressNode.OnDeploy();
			}
		}
	}

	public void IterateVessels(Vessel v)
	{
		int i = 0;
		for (int count = nodes.Count; i < count; i++)
		{
			ProgressNode progressNode = nodes[i];
			if (progressNode.OnIterateVessels != null)
			{
				progressNode.OnIterateVessels(v);
			}
		}
	}

	public void Stow()
	{
		int i = 0;
		for (int count = nodes.Count; i < count; i++)
		{
			ProgressNode progressNode = nodes[i];
			if (progressNode.OnStow != null)
			{
				progressNode.OnStow();
			}
		}
	}

	public string GetTreeSummary(string baseID)
	{
		string text = string.Empty;
		int count = nodes.Count;
		for (int i = 0; i < count; i++)
		{
			ProgressNode progressNode = nodes[i];
			if (progressNode.IsReached || progressNode.IsComplete)
			{
				text += progressNode.GetNodeSummary(baseID);
				if (i < count - 1)
				{
					text += ", ";
				}
			}
		}
		return text;
	}
}
